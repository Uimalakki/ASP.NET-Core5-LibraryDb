using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryApi.Models;
using AutoMapper;
using LibraryApi.DataTransferObjects.Outgoing;
using AutoMapper.QueryableExtensions;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public LoansController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Loans
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetLoans()
        {
            return await _context.Loans
                .AsNoTracking()
                .ProjectTo<LoanDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Loans/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoan(long id)
        {
            var loan = await _context.Loans.FindAsync(id);

            if (loan == null)
            {
                return NotFound();
            }

            return loan;
        }

        // PUT: api/Loans/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLoan(long id, Loan loan)
        {
            if (id != loan.Id)
            {
                return BadRequest();
            }

            _context.Entry(loan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Loans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Loan>> PostLoan(Loan loan)
        {
            var customersWithOverdueLoans = await LoanChecker.CheckDueLoans(_context);

            bool isBookAvailable = await LoanChecker.isThereAvailableCopies(loan.BookId, _context);
            System.Diagnostics.Debug.Print(isBookAvailable.ToString());

            foreach (long customerId in customersWithOverdueLoans)
            {
                if (customerId == loan.CustomerId)
                {
                    Console.WriteLine("Loan denied, customerId has overdue loans.");
                    System.Diagnostics.Debug.Print("Loan denied, customerId has overdue loans.");
                    return BadRequest();
                }

                if (!isBookAvailable)
                {
                    System.Diagnostics.Debug.Print("All the copies are loaned out");
                    return BadRequest();
                }
            }

            var fetchedBookCollection = await _context.BookCollection
                .Where(x => x.Book.Id == loan.BookId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            fetchedBookCollection.Quantity -= 1;

            _context.Entry(fetchedBookCollection).State = EntityState.Modified;

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLoan", new { id = loan.Id }, loan);
        }

        // DELETE: api/Loans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLoan(long id)
        {
            var loan = await _context.Loans.FindAsync(id);
            if (loan == null)
            {
                return NotFound();
            }

            _context.Loans.Remove(loan);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LoanExists(long id)
        {
            return _context.Loans.Any(e => e.Id == id);
        }
    }
}

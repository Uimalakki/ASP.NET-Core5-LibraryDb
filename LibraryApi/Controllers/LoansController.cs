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
using LibraryApi.DataTransferObjects.Incoming;

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
        public async Task<IActionResult> PutLoan(long id, LoanDtoIn loan)
        {
            if (id != loan.Id)
            {
                return BadRequest();
            }

            try
            {
                var loanEntity = await _context.Loans.FindAsync(id);

                loanEntity = _mapper.Map(loan, loanEntity);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!LoanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            /*_context.Entry(loan).State = EntityState.Modified;

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
            }*/

            return NoContent();
        }

        // POST: api/Loans
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewLoanDtoIn>> PostLoan(NewLoanDtoIn loan)
        {
            bool isBookAvailable = await LoanChecker.IsThereAvailableCopies(loan.BookId, _context);
            bool customerHasOverdueLoans = await LoanChecker.HasCustomerDueLoans(loan.CustomerId, _context);
            bool isDeniedToLoan = await LoanChecker.IsCustomerDeniedToLoan(loan.CustomerId, _context);

            if (customerHasOverdueLoans || isDeniedToLoan || !isBookAvailable)
            {
                return BadRequest();
            }

            var fetchedBookCollection = await _context.BookCollections
                .Where(x => x.Book.Id == loan.BookId)
                .SingleOrDefaultAsync();
            fetchedBookCollection.Quantity -= 1;

            _context.Entry(fetchedBookCollection).State = EntityState.Modified;

            var entityLoan = _mapper.Map<Loan>(loan);

            entityLoan.DueDate = DateTime.Now.AddDays(30);

            _context.Loans.Add(entityLoan);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetLoan", new { id = entityLoan.Id }, loan);
        }

        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ReturnLoan/{loanId},{customerId}")]
        public async Task<IActionResult> ReturnLoan(long loanId, long customerId)
        {
            var returnableLoan = await _context.Loans
                .Where(x => x.Id == loanId)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            var loanBookCollection = await _context.BookCollections.
                Where(x => x.Book.Id == returnableLoan.BookId)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if(returnableLoan.CustomerId != customerId)
            {
                return BadRequest();
            }

            returnableLoan.Returned = true;
            loanBookCollection.Quantity += 1;

            _context.Entry(returnableLoan).State = EntityState.Modified;
            _context.Entry(loanBookCollection).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LoanExists(loanId))
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

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
    public class CustomersController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public CustomersController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
        {
            return await _context.Customers
                .AsNoTracking()
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(long id)
        {
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(long id, CustomerDtoIn customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            try
            {
                var customerEntity = await _context.Customers.FindAsync(id);

                customerEntity = _mapper.Map(customer, customerEntity);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NewCustomerDtoIn>> PostCustomer(NewCustomerDtoIn customer)
        {
            var entityCustomer = _mapper.Map<Customer>(customer);

            var newPerson = new Person()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
            };

            entityCustomer.Person = newPerson;

            _context.People.Add(newPerson);
            _context.Customers.Add(entityCustomer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = entityCustomer.Id }, customer);
        }

        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("ChangeCustomersPermitToLoan/{customerId}")]
        [LibrarianAuthorization]
        public async Task<IActionResult> ChangeCustomersPermitToLoan(long customerId, bool deniedToLoan = false)
        {
            var customer = await _context.Customers.FindAsync(customerId);

            if (customer == null)
            {
                return NotFound();
            }

            customer.IsDeniedToLoan = deniedToLoan;

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(customerId))
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


         // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        [LibrarianAuthorization]
        public async Task<IActionResult> DeleteCustomer(long id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomerExists(long id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}

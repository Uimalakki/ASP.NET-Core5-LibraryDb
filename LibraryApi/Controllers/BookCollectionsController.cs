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
    public class BookCollectionsController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public BookCollectionsController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/BookQuantities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookCollectionDto>>> GetBookQuantities()
        {
            return await _context.BookCollection
                .Include(x => x.Book)
                .AsNoTracking()
                .ProjectTo<BookCollectionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/BookQuantities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookCollection>> GetBookQuantity(long id)
        {
            var bookQuantity = await _context.BookCollection.FindAsync(id);

            if (bookQuantity == null)
            {
                return NotFound();
            }

            return bookQuantity;
        }

        // PUT: api/BookQuantities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookQuantity(long id, BookCollection bookQuantity)
        {
            if (id != bookQuantity.Id)
            {
                return BadRequest();
            }

            _context.Entry(bookQuantity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookQuantityExists(id))
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

        // POST: api/BookQuantities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookCollection>> PostBookQuantity(BookCollection bookQuantity)
        {
            _context.BookCollection.Add(bookQuantity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookQuantity", new { id = bookQuantity.Id }, bookQuantity);
        }

        // DELETE: api/BookQuantities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookQuantity(long id)
        {
            var bookQuantity = await _context.BookCollection.FindAsync(id);
            if (bookQuantity == null)
            {
                return NotFound();
            }

            _context.BookCollection.Remove(bookQuantity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookQuantityExists(long id)
        {
            return _context.BookCollection.Any(e => e.Id == id);
        }
    }
}

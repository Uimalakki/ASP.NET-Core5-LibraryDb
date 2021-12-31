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
            return await _context.BookCollections
                .Include(x => x.Book)
                .AsNoTracking()
                .ProjectTo<BookCollectionDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/BookQuantities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookCollection>> GetBookQuantity(long id)
        {
            var bookQuantity = await _context.BookCollections.FindAsync(id);

            if (bookQuantity == null)
            {
                return NotFound();
            }

            return bookQuantity;
        }

        // PUT: api/BookQuantities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookQuantity(long id, BookCollectionDtoIn bookCollection)
        {
            if (id != bookCollection.Id)
            {
                return BadRequest();
            }

            try
            {
                var bookCollectionEntity = await _context.BookCollections.FindAsync(id);

                bookCollectionEntity = _mapper.Map(bookCollection, bookCollectionEntity);

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
        public async Task<ActionResult<NewBookCollectionDtoIn>> PostBookQuantity(NewBookCollectionDtoIn bookCollection)
        {
            var book = await _context.Books.FindAsync(bookCollection.BookId);

            var bookCollectionEntity = new BookCollection()
            {
                Book = book,
                ShelfNumber = bookCollection.ShelfNumber,
                Quantity = bookCollection.Quantity
            };

            _context.BookCollections.Add(bookCollectionEntity);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookQuantity", new { id = bookCollectionEntity.Id }, bookCollection);
        }

        // DELETE: api/BookQuantities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookQuantity(long id)
        {
            var bookQuantity = await _context.BookCollections.FindAsync(id);
            if (bookQuantity == null)
            {
                return NotFound();
            }

            _context.BookCollections.Remove(bookQuantity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookQuantityExists(long id)
        {
            return _context.BookCollections.Any(e => e.Id == id);
        }
    }
}

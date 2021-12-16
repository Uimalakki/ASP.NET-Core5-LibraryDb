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
    public class BooksController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public BooksController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            return await _context.Books
                .Include(x => x.OriginalLanguage)
                .Include(x => x.Topics)
                .Include(x => x.Author)
                .Include(x => x.Publisher)
                .Include(x => x.Language)
                .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Books
        [HttpGet("GetBooksWithOptionalParameters")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksWithOptionalParameters(
            [FromQuery] string bookName = "", 
            [FromQuery] string bookPublisher = "",
            [FromQuery] string bookAuthor= "",
            [FromQuery] string publishingYear = "",
            [FromQuery] string isbn = "",
            [FromQuery] string language = "")
        {
            return await _context.Books
                .Include(x => x.OriginalLanguage).Include(x => x.Topics).Include(x => x.Author).Include(x => x.Publisher)
                .Include(x => x.Language) 
                .Where(x => x.Name.Contains(bookName) && 
                            x.Publisher.Name.Contains(bookPublisher) && 
                            x.Author.Person.FirstName.Contains(bookAuthor) &&
                            x.Isbn.Contains(isbn) &&
                            x.Language.Name.Contains(language) &&
                            x.PublishingYear.Contains(publishingYear))
                .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(long id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(long id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            _context.Entry(book).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BookDtoIn>> PostBook(BookDtoIn book)
        {
            var entityBook = _mapper.Map<Book>(book);

            _context.Books.Add(entityBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = entityBook.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(long id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(long id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}

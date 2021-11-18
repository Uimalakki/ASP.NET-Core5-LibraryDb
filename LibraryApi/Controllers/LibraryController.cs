using AutoMapper;
using AutoMapper.QueryableExtensions;
using LibraryApi.DataTransferObjects.Outgoing;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly IMapper _mapper;

        public LibraryController(LibraryContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllBooks")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            return await _context.Books
                .Include(x => x.OriginalLanguage).Include(x => x.Topics).Include(x => x.Author).Include(x => x.Publisher)
                .Include(x => x.Language)
                .AsNoTracking()
                .ProjectTo<BookDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpGet("GetAllPeople")]
        public async Task<ActionResult<IEnumerable<PersonDto>>> GetPeople()
        {
            return await _context.People
                .AsNoTracking()
                .ProjectTo<PersonDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpGet("GetAllAuthors")]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            return await _context.Authors
                .AsNoTracking()
                .ProjectTo<AuthorDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpGet("GetAllTopics")]
        public async Task<ActionResult<IEnumerable<TopicDto>>> GetTopics()
        {
            return await _context.Topics
                .AsNoTracking()
                .ProjectTo<TopicDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpGet("GetAllPublishers")]
        public async Task<ActionResult<IEnumerable<PublisherDto>>> GetPublishers()
        {
            return await _context.Publishers
                .AsNoTracking()
                .ProjectTo<PublisherDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }

    
}

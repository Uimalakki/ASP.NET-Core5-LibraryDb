using AutoMapper;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class BookDto
    {
        public long BookId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public LanguageDto Language { get; set; }

        public LanguageDto OriginalLanguage { get; set; }

        public ICollection<TopicDto> Topics { get; set; }

        public string PublishingYear { get; set; }

        public AuthorDto Author { get; set; }

        public PublisherDto Publisher { get; set; }

        public string Isbn { get; set; }
    }

    public class BookDtoProfile : Profile 
    {
        public BookDtoProfile()
        {
            CreateMap<Models.Book, BookDto>()
                .ForMember(x => x.BookId, o => o.MapFrom(f => f.Id)).ReverseMap();

            
        }
    }
}

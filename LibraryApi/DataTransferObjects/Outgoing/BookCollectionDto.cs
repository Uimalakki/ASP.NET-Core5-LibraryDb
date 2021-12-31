using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class BookCollectionDto
    {
        public long BookId { get; set; }

        public long Quantity { get; set; }

        public int ShelfNumber { get; set; }
    }

    public class BookCollectionDtoProfile : Profile
    {
        public BookCollectionDtoProfile()
        {
            CreateMap<Models.BookCollection, BookCollectionDto>()
                .ForMember(bookCollectionDto => bookCollectionDto.BookId, x => x.MapFrom(bookCollection => bookCollection.Book.Id));
        }
    }
}

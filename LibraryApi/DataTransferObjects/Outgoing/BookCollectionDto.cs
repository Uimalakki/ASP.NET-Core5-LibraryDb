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

    public class BookQuantityDtoProfile : Profile
    {
        public BookQuantityDtoProfile()
        {
            CreateMap<Models.BookCollection, BookCollectionDto>()
                .ForMember(bookQuantityDto => bookQuantityDto.BookId, x => x.MapFrom(bookQuantity => bookQuantity.Book.Id));
        }
    }
}

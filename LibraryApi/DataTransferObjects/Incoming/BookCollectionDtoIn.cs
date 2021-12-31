using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Incoming
{
    public class BookCollectionDtoIn
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public long Quantity { get; set; }
        public int ShelfNumber { get; set; }
    }

    public class BookCollectionDtoInProfile : Profile
    {
        public BookCollectionDtoInProfile()
        {
            CreateMap<Models.BookCollection, BookCollectionDtoIn>().ReverseMap();
        }
    }
}

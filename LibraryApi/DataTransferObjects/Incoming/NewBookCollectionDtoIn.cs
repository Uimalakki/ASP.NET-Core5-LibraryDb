using AutoMapper;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Incoming
{
    public class NewBookCollectionDtoIn
    {
        public long BookId { get; set; }
        public long Quantity { get; set; }
        public int ShelfNumber { get; set; }

    }

    public class NewBookCollectionDtoInProfile : Profile
    {
        public NewBookCollectionDtoInProfile()
        {
            CreateMap<Models.BookCollection, BookCollectionDtoIn>().ReverseMap();
        }
    }

}

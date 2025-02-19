﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Incoming
{
    public class BookDtoIn
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string PublishingYear { get; set; }

        public string Isbn { get; set; }
    }
    public class BookDtoInProfile : Profile
    {
        public BookDtoInProfile()
        {
            CreateMap<Models.Book, BookDtoIn>().ReverseMap();
        }
    }
}

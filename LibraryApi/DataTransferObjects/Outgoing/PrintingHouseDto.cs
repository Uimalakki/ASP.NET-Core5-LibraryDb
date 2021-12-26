
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class PrintingHouseDto
    {
        public string Name { get; set; }

        public string City { get; set; }
    }

    public class PrintingHouseDtoProfile : Profile
    {
        public PrintingHouseDtoProfile()
        {
            CreateMap<Models.PrintingHouse, PrintingHouseDto>().ReverseMap();
        }

    }
}

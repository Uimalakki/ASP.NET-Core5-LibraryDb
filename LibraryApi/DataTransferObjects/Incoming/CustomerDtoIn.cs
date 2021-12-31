using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Incoming
{
    public class CustomerDtoIn
    {
        public long Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class CustomerDtoInProfile : Profile
    {
        public CustomerDtoInProfile()
        {
            CreateMap<Models.Customer, CustomerDtoIn>().ReverseMap();
        }
    }
}

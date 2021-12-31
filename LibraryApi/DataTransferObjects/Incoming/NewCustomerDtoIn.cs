using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Incoming
{
    public class NewCustomerDtoIn
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }

    public class NewCustomerDtoInProfile : Profile
    {
        public NewCustomerDtoInProfile()
        {
            CreateMap<Models.Customer, NewCustomerDtoIn>().ReverseMap();
        }
    }
}

using AutoMapper;
using LibraryApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class CustomerDto
    {
        public long CustomerId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public ICollection<LoanDto> Loans { get; set; }
    }

    public class CustomerDtoProfile : Profile
    {
        public CustomerDtoProfile()
        {
            CreateMap<Models.Customer, CustomerDto>()
                .ForMember(x => x.CustomerId, o => o.MapFrom(x => x.Id))
                .ForMember(customerDto => customerDto.FirstName, x => x.MapFrom(customer => customer.Person.FirstName))
                .ForMember(customerDto => customerDto.LastName, x => x.MapFrom(customer => customer.Person.LastName));
        }
    }
}

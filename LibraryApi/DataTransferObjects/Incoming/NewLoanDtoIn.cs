using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Incoming
{
    public class NewLoanDtoIn
    {
        public long BookId { get; set; }

        public long CustomerId { get; set; }
    }

    public class NewLoanDtoInProfile : Profile
    {
        public NewLoanDtoInProfile()
        {
            CreateMap<Models.Loan, NewLoanDtoIn>().ReverseMap();
        }
    }
}

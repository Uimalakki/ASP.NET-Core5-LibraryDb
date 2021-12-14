using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Incoming
{
    public class LoanDtoIn
    {
        public long BookId { get; set; }

        public long CustomerId { get; set; }
    }

    public class LoanDtoInProfile : Profile
    {
        public LoanDtoInProfile()
        {
            CreateMap<Models.Loan, LoanDtoIn>().ReverseMap();
        }
    }
}

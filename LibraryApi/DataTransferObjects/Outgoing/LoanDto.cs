using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class LoanDto
    {
        public long BookId { get; set; }
        public string BookName { get; set; }

        public long CustomerId { get; set; }

        public DateTime DueDate { get; set; }
    }

    public class LoanDtoProfile : Profile
    {
        public LoanDtoProfile()
        {
            CreateMap<Models.Loan, LoanDto>()
                .ForMember(loanDto => loanDto.BookId, x => x.MapFrom(loan => loan.Book.Id))
                .ForMember(loanDto => loanDto.BookName, x => x.MapFrom(loan => loan.Book.Name))
                .ForMember(loanDto => loanDto.CustomerId, x => x.MapFrom(loan => loan.Customer.Id));      
        }
    }


}

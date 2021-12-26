using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.DataTransferObjects.Outgoing
{
    public class LoanDto
    {
        public long LoanId { get; set; }
        public long BookId { get; set; }

        public long CustomerId { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime DueDateReminder { get; set; }

        public bool Returned { get; set; }
    }

    public class LoanDtoProfile : Profile
    {
        public LoanDtoProfile()
        {
            CreateMap<Models.Loan, LoanDto>()
                .ForMember(loanDto => loanDto.LoanId, x => x.MapFrom(loan => loan.Id));   
        }
    }


}

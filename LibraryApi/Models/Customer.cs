using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class Customer : BaseModel
    {
        
        public Person Person { get; set; }
        
        public long PersonId { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public bool IsDeniedToLoan { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class Loan : BaseModel
    {
        public Book Book { get; set; }

        public DateTime DueDate { get; set; }

        public Customer Customer { get; set; }

    }
}

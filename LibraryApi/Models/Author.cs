using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class Author : BaseModel
    {
        [Required]
        public long PersonId { get; set; }

        public Person Person { get; set; }

        public ICollection<Book> Books { get; set; }

    }
}

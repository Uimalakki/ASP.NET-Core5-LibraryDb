using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class Topic : BaseModel
    {
        [Required]
        public string Description { get; set; }

        public ICollection<Book> Book { get; set; }
    }
}

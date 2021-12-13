using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class BookCollection : BaseModel
    {
        [Required]
        public Book Book { get; set; }
        [Required]
        public long Quantity { get; set; }

        public int ShelfNumber { get; set; }
    }
}

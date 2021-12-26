using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class Company : BaseModel
    {
        [Required]
        public string Name { get; set; }
        public string City { get; set; }
    }
}

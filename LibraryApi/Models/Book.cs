using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class Book : BaseModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Language Language { get; set; }

        public Language OriginalLanguage { get; set; }

        public ICollection<Topic> Topics { get; set; }

        public string PublishingYear { get; set; }

        public Author Author { get; set; }

        public Publisher Publisher { get; set; }

        public string Isbn { get; set; }
    }
}

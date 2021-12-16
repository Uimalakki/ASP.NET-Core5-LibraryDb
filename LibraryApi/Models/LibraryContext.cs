using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi.Models
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookCollection> BookCollection { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Loan> Loans { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Publisher> Publishers { get; set; }

        public DbSet<Person> People { get; set; }

        public DbSet<Language> Languages { get; set; }

        public DbSet<Topic> Topics { get; set; }

    }
}

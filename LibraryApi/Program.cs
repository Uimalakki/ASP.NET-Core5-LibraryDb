using LibraryApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<LibraryContext>();

                CreateTestDataToDatabase(context);

                LoanChecker.CheckDueLoans(context);

                host.Run();
            }

        }

        /// <summary>
        /// For testing purposes only, method creates test data to database
        /// </summary>
        /// <param name="context"></param>
        public static void CreateTestDataToDatabase(LibraryContext context)
        {
            
            AddPeopleToDatabase(context);
            AddPublishersToDatabase(context);
            AddBooksToDatabase(context);

            if(context.Loans.Any())
            {
                return;
            }

            long mathBookId = context.Books.Where(x => x.Name.Equals("Essential Computer Mathematics")).Select(x => x.Id).SingleOrDefault();
            long flyBookId = context.Books.Where(x => x.Name.Equals("Flycasting Scandinavian Style")).Select(x => x.Id).SingleOrDefault();
            long cSharpBookId = context.Books.Where(x => x.Name.Equals("C# In Depth")).Select(x => x.Id).SingleOrDefault();
            long customerId1 = context.Customers.Where(x => x.Person.FirstName.Equals("James")).Select(x => x.Id).SingleOrDefault();
            long customerId2 = context.Customers.Where(x => x.Person.FirstName.Equals("Palle")).Select(x => x.Id).SingleOrDefault();
            long customerId3 = context.Customers.Where(x => x.Person.FirstName.Equals("Helmut")).Select(x => x.Id).SingleOrDefault();

            var pastDueDateLoan = new Loan()
            {
                BookId = mathBookId,
                CustomerId = customerId2,
                LastReminderSent = true,
                DueDate = new DateTime(2021, 11, 15)
            };
            var dueDateSendReminder = new Loan()
            {
                BookId = mathBookId,
                CustomerId = customerId3,
                DueDate = DateTime.Now.AddDays(-1)
            };


            var loanDueInOneWeek = new Loan()
            {
                BookId = flyBookId,
                CustomerId = customerId1,
                DueDate = DateTime.Now.AddDays(6)
            };

            context.Loans.Add(pastDueDateLoan);
            context.Loans.Add(loanDueInOneWeek);
            context.Loans.Add(dueDateSendReminder);

            AddLoanToDatabase(context, mathBookId, customerId1);
            AddLoanToDatabase(context, cSharpBookId, customerId1);
            AddLoanToDatabase(context, flyBookId, customerId2);
            context.SaveChanges();
        }

        /// <summary>
        /// For testing purposes only, method adds books to database
        /// </summary>
        /// <param name="context">Database context</param>
        public static void AddBooksToDatabase(LibraryContext context)
        {
            if(context.Books.Any())
            {
                return;
            }

            var cSharpBook = new Book() {
                Name = "C# In Depth",
                Description = "The powerful, flexible C# programming language is the foundation of .NET development. " +
                              "Even after two decades of success, it's still getting better! Exciting new features in" +
                              " C# 6 and 7 make it easier than ever to take on big data applications, cloud-centric " +
                              "web development, and cross-platform software using .NET Core. There's never been a " +
                              "better time to learn C# in depth.",
                Language = new Language() { Name = "english" },
                Publisher = context.Publishers.Where(x => x.Name.Equals("Gummerus kustannus")).FirstOrDefault(),
                PrintingHouse = new PrintingHouse() { 
                    Name = "Gummerus kirjapaino", 
                    City = "Jyväskylä"
                },
                Topics = new List<Topic>()
                {
                    new Topic { Description = "programming" },
                    new Topic { Description = "c#" },
                    new Topic { Description = "computers" }
                },
                Authors = new List<Author>()
                {
                    context.Authors.Where(x => x.Person.FirstName.Equals("Jon")).FirstOrDefault()
                },
                PublishingYear = "2009",
                Isbn = "49834758934573"
            };

            context.Add(cSharpBook);
            context.SaveChanges();

            var flyBook = new Book() {
                Name = "Flycasting Scandinavian Style",
                Description = "World-famous guide and instructor Henrik Mortensen's version of Scandinavian casting " +
                              "was designed to catch fish no matter where the caster is on the river—it is the most " +
                              "adaptable and flexible casting technique, giving the flyfisher the ability to handle " +
                              "any situation he encounters effortlessly.",
                Language = context.Languages.Where(x => x.Name.Equals("english")).FirstOrDefault(),
                OriginalLanguage = new Language() { Name = "danish" },
                Topics = new List<Topic>()
                {
                    new Topic { Description = "flyfishing"},
                    new Topic { Description = "outdoors" }
                },
                Authors = new List<Author>()
                {
                    context.Authors.Where(x => x.Person.FirstName.Equals("Henrik")).FirstOrDefault()
                },
                    
                Publisher = context.Publishers.Where(x => x.Name.Equals("Stackpole Books")).FirstOrDefault(),
                PrintingHouse = new PrintingHouse()
                {
                    Name = "Graphicom",
                    City = "Vicenza"
                },
                PublishingYear = "2010",
                Isbn = "9780811705097"
            };

            var mathBook = new Book()
            {
                Name = "Essential Computer Mathematics",
                Description = "Fortunately for you, there's Schaum's Outlines. More than 40 million students " +
                              "have trusted Schaum's to help them succeed in the classroom and on exams. " +
                              "Schaum's is the key to faster learning and higher grades in every subject. Each " +
                              "Outline presents all the essential course information in an easy-to-follow, " +
                              "topic-by-topic format. You also get hundreds of examples, solved problems, and " +
                              "practice exercises to test your skills.",
                Language = context.Languages.Where(x => x.Name.Equals("english")).FirstOrDefault(),
                Topics = new List<Topic>()
                {
                    
                    new Topic { Description = "mathematics" }
                },
                Authors = new List<Author>()
                {
                    context.Authors.Where(x => x.Person.FirstName.Equals("Henrik")).FirstOrDefault()
                },
                Publisher = context.Publishers.Where(x => x.Name.Equals("Stackpole Books")).FirstOrDefault(),
                PublishingYear = "1982",
                Isbn = "9780070379909"
            };

            context.Add(flyBook);
            context.Add(mathBook);

            var flyBookQuantity = new BookCollection()
            {
                Book = flyBook,
                Quantity = 5
            };

            var mathBookQuantity = new BookCollection()
            {
                Book = mathBook,
                Quantity = 2
            };
            var cSharpBookQuantity = new BookCollection()
            {
                Book = cSharpBook,
                Quantity = 10
            };
            context.Add(flyBookQuantity);
            context.Add(mathBookQuantity);
            context.Add(cSharpBookQuantity);
            context.SaveChanges();
        }

        /// <summary>
        /// For testing purposes only, method adds people, authors and customers to database
        /// </summary>
        /// <param name="context">Database context</param>
        public static void AddPeopleToDatabase(LibraryContext context)
        {
            if(context.People.Any())
            {
                return;
            }

            var mortensenPerson = new Person()
            {
                FirstName = "Henrik",
                LastName = "Mortensen",
                BirthDate = new DateTime(1960, 7, 1)
            };

            var mortensenAuthor = new Author()
            {
                Person = mortensenPerson,
                PersonId = mortensenPerson.Id
            };

            var skeetPerson = new Person()
            {
                FirstName = "Jon",
                LastName = "Skeet",
                BirthDate = new DateTime(1965, 3, 25)
            };

            var skeetAuthor = new Author()
            {
                Person = skeetPerson,
                PersonId = skeetPerson.Id
            };

            var customerPerson1 = new Person()
            {
                FirstName = "James",
                LastName = "Potkukelkka",
                BirthDate = new DateTime(1980, 6, 12),
            };

            var newCustomer1 = new Customer()
            {
                Person = customerPerson1,
                PersonId = customerPerson1.Id,
                Address = "Pöllölaaksontie 1, 00100, Helsinki",
                PhoneNumber = "0650656406",
                Email = "James@fasdf.com"
            };

            var customerPerson2 = new Person()
            {
                FirstName = "Palle",
                LastName = "Runqvist",
                BirthDate = new DateTime(1979, 4, 5)
            };

            var newCustomer2 = new Customer()
            {
                Person = customerPerson2,
                PersonId = customerPerson2.Id,
                Address = "Takoraudantie 4, 00700, Helsinki",
                PhoneNumber = "0650656406",
                Email = "James@fasdf.com"
            };

            var customerPerson3 = new Person()
            {
                FirstName = "Helmut",
                LastName = "Pallit",
                BirthDate = new DateTime(1970, 4, 12),
            };
            var newCustomer3 = new Customer()
            {
                Person = customerPerson3,
                PersonId = customerPerson3.Id,
                Address = "Teerisuontie 5, 00700, Helsinki",
                PhoneNumber = "0650656406",
                Email = "Helmut@fasdf.com"
            };

            context.People.Add(mortensenPerson);
            context.People.Add(skeetPerson);
            context.People.Add(customerPerson3);
            context.People.Add(customerPerson1);
            context.People.Add(customerPerson2);
            context.Customers.Add(newCustomer1);
            context.Customers.Add(newCustomer2);
            context.Customers.Add(newCustomer3);
            context.Authors.Add(mortensenAuthor);
            context.Authors.Add(skeetAuthor);
            context.SaveChanges();
        }

        /// <summary>
        /// For testing purposes only, method adds several Publishers to database
        /// </summary>
        /// <param name="context">Database context</param>
        public static void AddPublishersToDatabase(LibraryContext context)
        {
            if(context.Publishers.Any())
            {
                return;
            }

            var publisherOtava = new Publisher()
            {
                Name = "Otava kustannus"
            };

            var publisherGummerus = new Publisher()
            {
                Name = "Gummerus kustannus"
            };

            var publisherStackpole = new Publisher()
            {
                Name = "Stackpole Books"
            };

            context.Publishers.Add(publisherOtava);
            context.Publishers.Add(publisherGummerus);
            context.Publishers.Add(publisherStackpole);
            context.SaveChanges();
        }

        /// <summary>
        /// For testing purposes only, method adds new loan to database
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="bookId">Id of the book to be loaned</param>
        /// <param name="customerId">Id of the customer</param>
        public static void AddLoanToDatabase(LibraryContext context, long bookId, long customerId)
        {
            if(context.Loans.Any())
            {
                return;
            }

            var newLoan = new Loan()
            {
                BookId = bookId,
                CustomerId = customerId,
                DueDate = DateTime.Now.AddDays(30)
            };

            context.Loans.Add(newLoan);
            context.SaveChanges();
        }    

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

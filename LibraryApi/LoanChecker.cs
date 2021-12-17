using LibraryApi.DataTransferObjects.Outgoing;
using LibraryApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi
{
    public class LoanChecker
    {
        /// <summary>
        /// Method returns a list of customerId's with overdue loans 
        /// </summary>
        /// <param name="context"></param>
        public async static void CheckDueLoans(LibraryContext context)
        {
            DateTime oneWeekAhead = DateTime.Now.AddDays(7);
            DateTime currentDate = DateTime.Now;

            var customerIdWithDueLoans = await context.Loans
                .Where(x => x.DueDate.CompareTo(oneWeekAhead) <= 0 && 
                            x.DueDate.CompareTo(currentDate) >= 0 && 
                            x.Returned == false)
                .AsNoTracking()
                .Select(x => x.CustomerId)
                .ToArrayAsync();

            var allCustomers = await context.Customers
                .Include(x => x.Person)
                .AsNoTracking()
                .ToArrayAsync();

            foreach (Customer customer in allCustomers)
            {
                foreach(long id in customerIdWithDueLoans)
                {
                    if(customer.Id == id)
                    {
                        System.Diagnostics.Debug.Print($"Remainder has been sent to a customer {customer.Person.FirstName} {customer.Person.LastName}");
                    }
                }
            }

        }

        /// <summary>
        /// Boolean method that returns value true if customerId has overdued loans.
        /// </summary>
        /// <param name="customerId">Customer id of chosen customer</param>
        /// <param name="context">Database context</param>
        /// <returns></returns>
        public async static Task<bool> HasCustomerDueLoans(float customerId, LibraryContext context)
        {
            DateTime currentDate = DateTime.Now;

            var passedDueDateLoans = await context.Loans
                .Where(x => x.DueDate.CompareTo(currentDate) <= 0 && x.CustomerId == customerId)
                .AsNoTracking()
                .ToArrayAsync();

            if(passedDueDateLoans.Length > 0)
            {
                System.Diagnostics.Debug.Print("Loan denied, customer has overdue loans.");
                return true;
            } 

            return false;
        }

        /// <summary>
        /// Boolean method returns value true, if there is 1 or more copies available of the book with id number bookId in the database
        /// </summary>
        /// <param name="bookId">Id number of desired book</param>
        /// <param name="context">Database context</param>
        /// <returns></returns>
        public async static Task<bool> IsThereAvailableCopies(long bookId, LibraryContext context)
        {
            long numberOfBookCopies = await context.BookCollection
                .Where(x => x.Book.Id == bookId)
                .AsNoTracking()
                .Select(x => x.Quantity)
                .SingleOrDefaultAsync();
            
            if(numberOfBookCopies > 0)
            {
                return true;
            } 
            else
            {
                System.Diagnostics.Debug.Print("All the book copies are already loaned out");
                return false;
            } 
        }

        /// <summary>
        /// Boolean method returns value true, if there's a customer with id number customerId in the database
        /// </summary>
        /// <param name="customerId">Id number of desired customer</param>
        /// <param name="context">Database context</param>
        /// <returns></returns>
        public async static Task<bool> IsCustomerDeniedToLoan(long customerId, LibraryContext context)
        {
            bool isDeniedToLoan = await context.Customers
                .Where(x => x.Id == customerId)
                .AsNoTracking()
                .Select(x => x.IsDeniedToLoan)
                .SingleOrDefaultAsync();

            if(isDeniedToLoan)
            {
                System.Diagnostics.Debug.Print("Customer has status: denied to loan");
                return true;
            }

            return false;
        }
    }
}

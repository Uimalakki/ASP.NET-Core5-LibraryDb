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

            var dueLoans = await context.Loans
                .Where(x => x.DueDate.CompareTo(oneWeekAhead) <= 0 &&
                            x.Returned == false &&
                            x.DueDateRemindedAt.Day < currentDate.Day)
                .ToArrayAsync();

            foreach (Loan loan in dueLoans)
            {
                var customer = await context.Customers.FindAsync(loan.CustomerId);
                var person = await context.People.FindAsync(customer.PersonId);
                loan.DueDateRemindedAt = currentDate;
                if (loan.DueDate.Day < currentDate.Day && !loan.LastReminderSent)
                {
                    System.Diagnostics.Debug.Print($"Laina on umpeutunut, muistutus lähetetty asiakkaalle: {person.FirstName} {person.LastName}");
                    loan.LastReminderSent = true;
                }
                else
                {
                    System.Diagnostics.Debug.Print($"Muistutus lainan lähestyvästä eräpäivästä lähetetty asiakkaalle: {person.FirstName} {person.LastName}");
                }

                context.Entry(loan).State = EntityState.Modified;

            }

            context.SaveChanges();
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

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
        public async static Task<IEnumerable<long>> CheckDueLoans(LibraryContext context)
        {
            DateTime currentDate = DateTime.Now;

            var customersWithDueLoans = await context.Loans
                .Where(x => x.DueDate.CompareTo(currentDate) <= 0 && x.Returned == false)
                .AsNoTracking()
                .Select(x => x.CustomerId)
                .ToArrayAsync();

            return customersWithDueLoans.ToList();
        }

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

        public async static Task<bool> CustomerIsDeniedToLoan(long customerId, LibraryContext context)
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

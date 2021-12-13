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
                .Select(x => x.CustomerId)
                .ToArrayAsync();

            return customersWithDueLoans.ToList();
        }

        public async static Task<bool> HasCustomerDueLoans(float customerId, LibraryContext context)
        {
            DateTime currentDate = DateTime.Now;

            var passedDueDateLoans = await context.Loans.Where(x => x.DueDate.CompareTo(currentDate) <= 0).ToArrayAsync();

            foreach (Loan loan in passedDueDateLoans)
            {
                if (loan.CustomerId == customerId)
                {
                    return true;
                }
            }
            return false;
        }

        public async static Task<bool> isThereAvailableCopies(long bookId, LibraryContext context)
        {
            long numberOfBookCopies = await context.BookCollection.Where(x => x.Book.Id == bookId).Select(x => x.Quantity).SingleOrDefaultAsync();
            
            if(numberOfBookCopies > 1)
            {
                return true;
            } 
            else
            {
                return false;
            } 
        }
    }
}

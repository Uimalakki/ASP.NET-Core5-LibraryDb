using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApi
{
    [AttributeUsage(AttributeTargets.Method)]
    public class LibrarianAuthorization : Attribute, IResourceFilter
    {
        //private const string AuthorizationKey = "ByThePowerOfGreyskull";
        public LibrarianAuthorization()
        {

        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            //throw new NotImplementedException();
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if(!context.HttpContext.Request.Headers.TryGetValue("AuthorizationKey", out var jwtToken))
            {
                context.Result = new UnauthorizedResult();
            }
            if(jwtToken != "ByThePowerOfGreyskull")
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}

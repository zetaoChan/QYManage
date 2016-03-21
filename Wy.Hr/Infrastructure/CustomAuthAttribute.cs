using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wy.Hr.Infrastructure
{
    public class CustomAuthAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated == true)
            {
                return true;
            }
            else {
                return false;
            }
        }

    }
}
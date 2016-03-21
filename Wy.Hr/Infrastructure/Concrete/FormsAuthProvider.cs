using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace Wy.Hr.Infrastructure.Abstract
{
    public class FormsAuthProvider : IAuthProvider
    {
        public bool Authenticate(string userName, string password) { 
            bool result = FormsAuthentication.Authenticate(userName, password);
            if(result){
                FormsAuthentication.SetAuthCookie(userName, false);
            }
            return result;
        }

    }
}

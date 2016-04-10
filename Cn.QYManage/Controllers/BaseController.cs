using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Cn.QYManage.Attribute;

namespace Cn.QYManage
{
    
    public class BaseController : Controller
    {
       
        public string Username
        {
            get
            {
                return User.Identity.Name;
            }
        }
    }

}

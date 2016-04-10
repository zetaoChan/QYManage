using System.Web.Mvc;
using System.Web.Security;
using Cn.QYManage.Attribute;

namespace Cn.QYManage.Controllers
{
    public class HomeController : BaseController
    {
        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

    }
}

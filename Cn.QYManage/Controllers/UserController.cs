using System.Web.Mvc;
using Cn.QYManage.Attribute;

namespace Cn.QYManage.Controllers
{
    public class UserController : BaseController
    {

        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

        [PermissionFilter]
        public ActionResult ChangePassword()
        {
            return View();
        }
        
    }
}
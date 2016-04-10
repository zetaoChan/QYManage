using System.Web.Mvc;
using Cn.QYManage.Attribute;

namespace Cn.QYManage.Controllers
{
    public class StaffController : BaseController
    {
        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
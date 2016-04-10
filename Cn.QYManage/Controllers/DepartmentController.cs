using System.Web.Mvc;
using Cn.QYManage.Attribute;

namespace Cn.QYManage.Controllers
{
    public class DepartmentController : BaseController
    {
        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
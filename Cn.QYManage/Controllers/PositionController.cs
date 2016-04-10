using System.Web.Mvc;
using Cn.QYManage.Attribute;

namespace Cn.QYManage.Controllers
{
    public class PositionController : BaseController
    {
        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
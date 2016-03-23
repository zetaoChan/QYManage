using System.Web.Mvc;
using Wy.Hr.Attribute;

namespace Wy.Hr.Controllers
{
    public class TaskController : BaseController
    {

        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

        [PermissionFilter]
        public ActionResult MyTask()
        {
            return View();
        }

    }
}
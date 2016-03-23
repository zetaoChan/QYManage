using System.Web.Mvc;
using Wy.Hr.Attribute;

namespace Wy.Hr.Controllers
{
    public class PermissionController : BaseController
    {
        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
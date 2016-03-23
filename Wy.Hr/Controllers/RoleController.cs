using System.Web.Mvc;
using Wy.Hr.Attribute;
using Wy.Hr.Infrastructure;

namespace Wy.Hr.Controllers
{
    public class RoleController : BaseController
    {
        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

    }
}
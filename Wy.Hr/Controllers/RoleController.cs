using System.Web.Mvc;
using Wy.Hr.Attribute;
using Wy.Hr.Infrastructure;

namespace Wy.Hr.Controllers
{
    public class RoleController : BaseController
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
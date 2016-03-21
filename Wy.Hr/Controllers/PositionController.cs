using System.Web.Mvc;

namespace Wy.Hr.Controllers
{
    public class PositionController : BaseController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
using System.Web.Mvc;

namespace Wy.Hr.Controllers
{
    public class MessageController : BaseController
    {

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
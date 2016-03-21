using System.Web.Mvc;

namespace Wy.Hr.Controllers
{
    public class TaskController : BaseController
    {

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult List()
        {
            return View();
        }

        [Authorize]
        public ActionResult Detail(int id)
        {
            ViewData.Model = id;
            return View();
        }

    }
}
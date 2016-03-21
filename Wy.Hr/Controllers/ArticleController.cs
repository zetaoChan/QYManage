using System.Web.Mvc;

namespace Wy.Hr.Controllers
{
    public class ArticleController : BaseController
    {

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult List(string type)
        {
            ViewData.Model = type;
            return View();
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            ViewData.Model = id;
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
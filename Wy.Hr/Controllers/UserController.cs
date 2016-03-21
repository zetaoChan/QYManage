using System.Web.Mvc;

namespace Wy.Hr.Controllers
{
    public class UserController : BaseController
    {

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Info()
        {
            return View();
        }

        [Authorize]
        public ActionResult UserProfile()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        
    }
}
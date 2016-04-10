using System.Web.Mvc;
using Cn.QYManage.Attribute;

namespace Cn.QYManage.Controllers
{
    public class ArticleController : BaseController
    {

        [PermissionFilter]
        public ActionResult Index()
        {
            return View();
        }

        [PermissionFilter]
        public ActionResult List(string type)
        {
            ViewData.Model = type;
            return View();
        }

        [PermissionFilter]
        public ActionResult Add()
        {
            return View();
        }

        [PermissionFilter]
        public ActionResult Edit(int id)
        {
            ViewData.Model = id;
            return View();
        }

        [PermissionFilter]
        public ActionResult Detail(int id)
        {
            ViewData.Model = id;
            return View();
        }

    }
}
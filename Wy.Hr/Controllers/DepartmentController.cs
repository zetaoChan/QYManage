using System;
using System.Web.Mvc;
using Wy.Hr.Common;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class DepartmentController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        //  新增部门页面
        public ActionResult Add()
        {
            return View();
        }

        //  修改部门页面
        public ActionResult Edit(int id)
        {
                var result = DepartmentAPIController.Instance.GetDetail(new SingleModelArgs
                {
                    Id = id
                });
                if (!result.Success) {
                    throw new Exception(result.Message);
                }
                ViewData.Model = result.Content;
                return View();
        }

        //  保存部门页面
        public ActionResult Save(DepartmentModel model)
        {
                var result = new APIResult();
                if(model.Id.Equals(0)){
                    result = DepartmentAPIController.Instance.Add(model);
                }
                else{
                    result = DepartmentAPIController.Instance.Update(model);
                }
                if (!result.Success)
                {
                    throw new Exception(result.Message);
                }
                return View();
        }

    }
}
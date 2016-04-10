using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Cn.QYManage.Common;
using Cn.QYManage.Models;

namespace Cn.QYManage.Controllers
{
    public class FileController : BaseController
    {
        // GET: File
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return Content("没有文件！", "text/plain");
            }
            var no = DateTime.Now.ToLocalTime().ToString("yyyyMMdd") + CommonUtil.CreateIntNoncestr(4);
            var fileName = Path.Combine(Request.MapPath("~/Upload"), Path.GetFileName(no + file.FileName.Split('.')[1]));
            try
            {
                file.SaveAs(fileName);
                FileAPIController.Instance.Add(new FilesModels
                {
                    Name = file.FileName,
                    No = no
                });
                return Content("上传成功！");
            }
            catch
            {
                return Content("上传异常 ！", "text/plain");
            }
        }

        public ActionResult DownLoad(string no)
        {
            var fileName = FileAPIController.Instance.GetFileName(no).Content;
            var filePath = CommonUtil.GetMapPath("/Upload/" + no + fileName.Split('.')[1]);
            return File(filePath, "application/octet-stream", fileName);
        }

    }
}
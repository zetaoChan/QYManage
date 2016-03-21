using System;
using System.Collections.Generic;
using System.Web.Http;
using Wy.Hr.Common;
using Wy.Hr.Data;
using Wy.Hr.Models;
using System.Linq;

namespace Wy.Hr.Controllers
{
    public class FileAPIController : ApiBaseController
    {

        public static FileAPIController Instance = new FileAPIController();

        [HttpPost]
        [Authorize]
        public APIResult<List<FilesModels>> GetList()
        {
            try
            {
                using (var db = new DataContext())
                {

                    var list = db.QueryFiles(null)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new FilesModels()
                        {
                            Id = m.Id,
                            No = m.No,
                            Name = m.Name,
                            Uploader = m.Uploader,
                            UploadTime = m.UploadTime
                        })
                        .ToList<FilesModels>();
                    return Success<List<FilesModels>>(list);
                }

            }
            catch (Exception ex)
            {
                return Error<List<FilesModels>>(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult<string> GetFileName(string no)
        {
            try
            {
                using (var db = new DataContext())
                {

                    var files = db.GetFilesByNo(no);
                    var fileName = files == null ? "" : files.Name;
                    return Success<string>(fileName);
                }

            }
            catch (Exception ex)
            {
                return Error<string>(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult Add(FilesModels model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var files = new Files
                    {
                        Name = model.Name,
                        No = model.No,
                        Uploader = User.Identity.Name,
                        UploadTime = DateTime.Now.ToLocalTime()
                    };
                    db.AddToFiles(files);
                    db.SaveChanges();
                    return Success();
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult BatchRemove(BatchModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryFiles(null).Where(m => args.Ids.Contains(m.Id)).ToList();
                    foreach (var item in list)
                    {
                        db.DeleteFiles(item.Id);
                        DeleteFile(item.No);
                    }
                    db.SaveChanges();
                    return Success();
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public void DeleteFile(string fileName)
        {
            var filePath = CommonUtil.GetMapPath("/Upload/" + fileName);
            System.IO.File.Delete(filePath);
        }

    }
}
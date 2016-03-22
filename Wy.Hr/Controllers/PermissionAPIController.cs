using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Wy.Hr.Common;
using Wy.Hr.Data;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class PermissionAPIController : ApiBaseController
    {

        public static PermissionAPIController Instance = new PermissionAPIController();

        #region 信息获取方法
        /// <summary>
        /// 获取职位分页列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult GetPagedList(PermissionPagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var condition = new PermissionQueryCondition { Name = args.Name };
                    var result = db.QueryPermission(condition)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new PermissionModel()
                        {
                            Id = m.Id,
                            Url = m.Url,
                            Name = m.Name
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<PermissionModel>()
                    {
                        Items = result.Items,
                        PageIndex = result.CurrentPageIndex,
                        PageSize = result.PageSize,
                        TotalCount = result.TotalItemCount,
                        TotalPageCount = result.TotalPageCount
                    });
                }

            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }

        
        [HttpPost]
        [Authorize]
        public APIResult GetAllPermission()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryPermission(null)
                        .OrderBy(m => m.Url)
                        .Select(m => new PermissionModel()
                        {
                            Id = m.Id,
                            Url = m.Url,
                            Name = m.Name
                        })
                        .ToList();
                    return Success(list);
                }

            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }
        #endregion

        #region 数据操作方法
        /// <summary>
        /// 新增职位操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult Add(PermissionModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<PermissionModel, Permission>();
                    var entity = Mapper.Map<PermissionModel, Permission>(model);
                    db.AddToPermission(entity);
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }

        /// <summary>
        /// 更新职位操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult Update(PermissionModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSinglePermission(model.Id);
                    if (entity == null) throw new Exception("职位不存在");
                    Mapper.CreateMap<PermissionModel, Permission>();
                    Mapper.Map<PermissionModel, Permission>(model, entity);
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }

        /// <summary>
        /// 批量删除职位操作
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult BatchRemove(BatchModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    db.BatchDeletePermission(args.Ids);
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }
        #endregion

    }
}
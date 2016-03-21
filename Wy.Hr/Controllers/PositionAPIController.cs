using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Wy.Hr.Common;
using Wy.Hr.Data;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class PositionAPIController : ApiBaseController
    {

        public static PositionAPIController Instance = new PositionAPIController();

        #region 信息获取方法
        /// <summary>
        /// 获取职位分页列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult GetPagedList(PositionPagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var condition = new PositionQueryCondition {Name = args.Name };
                    var result = db.QueryPosition(condition)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new PositionModel()
                        {
                            Id = m.Id,
                            No = m.No,
                            Name = m.Name,
                            Grade = m.Grade
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<PositionModel>()
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
        public APIResult GetSelList()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryPosition(null)
                        .OrderByDescending(m => m.Name)
                        .Select(m => new PositionModel()
                        {
                            Id = m.Id,
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
        public APIResult Add(PositionModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<PositionModel, Position>();
                    var entity = Mapper.Map<PositionModel, Position>(model);
                    db.AddToPosition(entity);
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
        public APIResult Update(PositionModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSinglePosition(model.Id);
                    if (entity == null) throw new Exception("职位不存在");
                    Mapper.CreateMap<PositionModel, Position>();
                    Mapper.Map<PositionModel, Position>(model, entity);
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
                    db.BatchDeletePosition(args.Ids);
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
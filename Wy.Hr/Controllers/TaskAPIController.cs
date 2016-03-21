using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Wy.Hr.Common;
using Wy.Hr.Data;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class TaskAPIController : ApiBaseController
    {

        public static TaskAPIController Instance = new TaskAPIController();

        [Authorize]
        [HttpPost]
        public APIResult GetPagedList(TaskPagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var result = db.QueryTask(null)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new TaskModel()
                        {
                            Id = m.Id,
                            AddTime = m.AddTime,
                            AddUser = m.AddUser,
                            Contents = m.Contents,
                            Title = m.Title,
                            Status = m.Status,
                            Executor = m.Executor,
                            FinishedTime = m.FinishedTime,
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<TaskModel>()
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
                while(e.InnerException != null){
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }

        [HttpPost]
        public APIResult<TaskModel> GetDetail(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    //  检验参数是否为空
                    if (args == null) { throw new Exception("请求参数异常"); }
                    //  查找部门详情
                    var entity = db.GetSingleTask(args.Id);
                    if (entity == null) { throw new Exception("任务不存在"); }
                    //  转换为部门模型
                    Mapper.CreateMap<Task, TaskModel>();
                    var model = Mapper.Map<Task, TaskModel>(entity);
                    return Success<TaskModel>(model);
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
                return Error<TaskModel>(error);
            }
        }

        [Authorize]
        [HttpPost]
        public APIResult Publish(TaskModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<TaskModel, Task>();
                    var entity = Mapper.Map<TaskModel, Task>(model);
                    entity.AddTime = DateTime.Now.ToLocalTime();
                    entity.AddUser = User.Identity.Name;
                    entity.Status = TaskStatus.未开始;
                    db.AddToTask(entity);
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
                return Error<TaskModel>(error);
            }
        }

        [Authorize]
        [HttpPost]
        public APIResult Update(TaskModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleTask(model.Id);
                    Mapper.CreateMap<TaskModel, Task>();
                    entity = Mapper.Map<TaskModel, Task>(model);
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
                return Error<TaskModel>(error);
            }
        }

        [Authorize]
        [HttpPost]
        public APIResult Remove(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleTask(args.Id);
                    if (entity == null) { throw new Exception("用户不存在"); }
                    db.DeleteTask(args.Id);
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
                return Error<TaskModel>(error);
            }
        }

    }
}
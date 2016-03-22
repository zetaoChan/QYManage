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
                        .Where(m => m.AddUser == User.Identity.Name)
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
                            ExpectedTime = m.ExpectedTime
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

        [Authorize]
        [HttpPost]
        public APIResult GetMyTask(TaskPagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var result = db.QueryTask(null)
                        .OrderByDescending(m => m.Id)
                        .Where(m => m.Executor == User.Identity.Name)
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
                            ExpectedTime = m.ExpectedTime
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
                while (e.InnerException != null)
                {
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
        public APIResult ExcuseTask(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleTask(args.Id);
                    if (entity == null) throw new Exception("任务不存在");
                    switch (entity.Status)
                    {
                        case TaskStatus.未开始:
                            entity.Status = TaskStatus.进行中;
                            break;
                        case TaskStatus.进行中:
                            entity.FinishedTime = DateTime.Now.ToLocalTime();
                            entity.Status = TaskStatus.已完成;
                            break;
                    }
                    db.SaveChanges();
                    return Success(entity.Status);
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
        public APIResult Add(TaskModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var user = db.GetSingleUserByUserName(model.Executor);
                    if (user == null) throw new Exception("用户不存在");
                    var entity = new Task { 
                        AddTime = DateTime.Now.ToLocalTime(),
                        AddUser = User.Identity.Name,
                        Contents = model.Contents,
                        Executor = model.Executor,
                        ExpectedTime = model.ExpectedTime,
                        Title = model.Title,
                        Status = TaskStatus.未开始
                    };
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
                    Mapper.Map<TaskModel, Task>(model, entity);
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

        public APIResult BatchRemove(BatchModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    db.BatchDeleteTask(args.Ids);
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
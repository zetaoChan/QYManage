using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Cn.QYManage.Common;
using Cn.QYManage.Data;
using Cn.QYManage.Models;

namespace Cn.QYManage.Controllers
{
    public class MessageAPIController : ApiBaseController
    {

        public static MessageAPIController Instance = new MessageAPIController();

        #region 信息获取方法
        [Authorize]
        [HttpPost]
        public APIResult<List<MessageModel>> GetList(MessagePagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var staff = db.QueryStaff(null).Where(m => m.User.UserName == User.Identity.Name).FirstOrDefault();
                    if (staff == null) return Success<List<MessageModel>>(null);
                    var condition = new MessageQueryCondition { 
                        IsReaded = args.IsReaded
                    };

                    var query = db.QueryMessage(null);

                    if (args.Type == "get") {
                        query = query.Where(m => m.RecipientId == staff.Id);
                    }
                    else {
                        query = query.Where(m => m.SenderId == staff.Id);
                    }

                    var list = query
                        .OrderBy(m => m.SendDate)
                        .Where(m => m.SenderId == staff.Id || m.RecipientId == staff.Id)
                        .Select(m => new MessageModel()
                        {
                            Id = m.Id,
                            Contents = m.Contents,
                            SendDate = m.SendDate,
                            IsReaded = m.IsReaded,
                            RecipientName = m.Recipient.Name,
                            SenderName = m.Sender.Name
                        })
                        .ToList();
                    return Success<List<MessageModel>>(list);
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
                return Error<List<MessageModel>>(error);
            }
        }

        [Authorize]
        [HttpPost]
        public APIResult<MessageModel> GetDetail(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleMessage(args.Id);
                    if (entity == null) throw new Exception("消息不存在");
                    if (!entity.IsReaded)
                    {
                        entity.IsReaded = true;
                        db.SaveChanges();
                    }
                    Mapper.CreateMap<Message, MessageModel>();
                    var model = Mapper.Map<Message, MessageModel>(entity);
                    return Success<MessageModel>(model);
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
                return Error<MessageModel>(error);
            }
        }
        #endregion

        #region 数据操作方法
        [Authorize]
        [HttpPost]
        public APIResult Add(MessageAddModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var staff = db.QueryStaff(null).Where(m => m.User.UserName == User.Identity.Name).FirstOrDefault();
                    foreach(var item in model.RecipientIds){
                        var entity = new Message
                        {
                            Contents = model.Contents,
                            IsReaded = false,
                            RecipientId = item,
                            SendDate = DateTime.Now.ToLocalTime(),
                            SenderId = staff.Id
                        };
                        db.AddToMessage(entity);
                    }
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
                return Error<MessageModel>(error);
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
                    var entity = db.GetSingleMessage(args.Id);
                    if (entity == null) throw new Exception("消息不存在");
                    db.DeleteMessage(args.Id);
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
                return Error<MessageModel>(error);
            }
        }
        #endregion

    }
}
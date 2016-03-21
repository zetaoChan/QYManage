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
    public class MessageAPIController : ApiBaseController
    {

        public static MessageAPIController Instance = new MessageAPIController();

        #region 信息获取方法
        [Authorize]
        [HttpPost]
        public APIResult<PagedResult<MessageModel>> GetPagedList(MessagePagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var condition = new MessageQueryCondition { 
                        MessageType = args.MessageType,
                        IsReaded = args.IsReaded
                    };
                    var result = db.QueryMessage(condition)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new MessageModel()
                        {
                            Id = m.Id,
                            Contents = m.Contents,
                            MessageDate = m.MessageDate,
                            IsReaded = m.IsReaded,
                            MessageType = m.MessageType,
                            //RecipientName = m.Recipient.Name,
                            //SenderName = m.Sender.Name,
                            Title = m.Title
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success<PagedResult<MessageModel>>(new PagedResult<MessageModel>()
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
                return Error<PagedResult<MessageModel>>(error);
            }
        }

        [Authorize]
        [HttpPost]
        public APIResult<List<MessageModel>> GetList(MessagePagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryMessage(null)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new MessageModel()
                        {
                            Id = m.Id,
                            Contents = m.Contents,
                            MessageDate = m.MessageDate,
                            IsReaded = m.IsReaded,
                            MessageType = m.MessageType,
                            //RecipientName = m.Recipient.Name,
                            //SenderName = m.Sender.Name,
                            Title = m.Title
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
        public APIResult Add(MessageModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<MessageModel, Message>();
                    var entity = Mapper.Map<MessageModel, Message>(model);
                    db.AddToMessage(entity);
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
        public APIResult Update(MessageModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleMessage(model.Id);
                    if (entity == null) throw new Exception("消息不存在");
                    Mapper.CreateMap<MessageModel, Message>()
                        .ForMember("Id", opt => opt.Ignore());
                    Mapper.Map<MessageModel, Message>(model, entity);
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
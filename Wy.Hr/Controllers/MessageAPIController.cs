using System;
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
                        IsReaded = args.IsReaded
                    };
                    var result = db.QueryMessage(condition)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new MessageModel()
                        {
                            Id = m.Id,
                            Contents = m.Contents,
                            SendDate = m.SendDate,
                            IsReaded = m.IsReaded,
                            RecipientName = m.RecipientName,
                            SenderName = m.SenderName
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
                    var entity = new Message { 
                        Contents = model.Contents,
                        IsReaded = false,
                        RecipientName = model.RecipientName,
                        SendDate = DateTime.Now.ToLocalTime(),
                        SenderName = User.Identity.Name
                    };
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
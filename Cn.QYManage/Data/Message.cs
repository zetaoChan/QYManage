using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Cn.QYManage.Common;

namespace Cn.QYManage.Data
{
    [Table("Message")]
    public class Message
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "发送人Id")]
        public virtual int SenderId { get; set; }
        [ForeignKey("SenderId")]
        public virtual Staff Sender { get; set; }
        [LogFiled(Name = "接收人Id")]
        public virtual int RecipientId { get; set; }
        [ForeignKey("RecipientId")]
        public virtual Staff Recipient { get; set; }
        [LogFiled(Name = "是否已读")]
        public virtual bool IsReaded { get; set; }
        [LogFiled(Name = "发送日期")]
        public virtual DateTime SendDate { get; set; }
        [LogFiled(Name = "消息内容")]
        public virtual string Contents { get; set; }
    }

    public class MessageQueryCondition
    {
        public bool? IsReaded { get; set; }
    }

    public static class MessageDbContextExtention
    {
        public static Message AddToMessage(this DbContext context, Message model)
        {
            context.Set<Message>().Add(model);
            return model;
        }

        public static Message GetSingleMessage(this DbContext context, int id)
        {
            return context.Set<Message>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Message DeleteMessage(this DbContext context, int id)
        {
            var model = context.GetSingleMessage(id);
            if (model != null)
            {
                return context.Set<Message>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static IQueryable<Message> QueryMessage(this DbContext context, MessageQueryCondition condition)
        {
            var query = context.Set<Message>().AsQueryable();
            if(condition != null){
                if (condition.IsReaded.HasValue)
                {
                    query = query.Where(m => m.IsReaded == condition.IsReaded.Value);
                }
            }
            return query;
        }

        public static DbSet<Message> MessageDbSet(this DbContext context)
        {
            return context.Set<Message>();
        }

    }

}
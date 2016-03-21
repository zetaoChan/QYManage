using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Comment")]
    public class Comment
    {
        public virtual int Id { get; set; }
        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; }
        [LogFiled(Name = "员工Id")]
        public virtual int StaffId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        [LogFiled(Name = "职位Id")]
        public virtual int PositionId { get; set; }
        [LogFiled(Name = "邮箱")]
        public virtual string Email { get; set; }
        [LogFiled(Name = "联系电话")]
        public virtual string Tel { get; set; }
    }

    public class CommentQueryCondition
    {
    }

    public static class CommentDbContextExtention
    {
        public static Comment AddToComment(this DbContext context, Comment model)
        {
            context.Set<Comment>().Add(model);
            return model;
        }

        public static Comment GetSingleComment(this DbContext context, int id)
        {
            return context.Set<Comment>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Comment DeleteComment(this DbContext context, int id)
        {
            var model = context.GetSingleComment(id);
            if (model != null)
            {
                return context.Set<Comment>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static IQueryable<Comment> QueryComment(this DbContext context, CommentQueryCondition condition)
        {
            var query = context.Set<Comment>().AsQueryable();
            if(condition != null){
            }
            return query;
        }

        public static DbSet<Comment> CommentDbSet(this DbContext context)
        {
            return context.Set<Comment>();
        }

    }

}
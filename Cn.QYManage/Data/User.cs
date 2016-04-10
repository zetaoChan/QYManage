using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Cn.QYManage.Common;

namespace Cn.QYManage.Data
{
    [Table("User")]
    public class User
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "用户名")]
        public virtual string UserName { get; set; }
        [LogFiled(Name = "密码")]
        public virtual string Password { get; set; }
        [LogFiled(Name = "角色Ids")]
        public virtual string RoleIds { get; set; }

       
    }

    public class UserQueryCondition
    {
    }

    public static class UserDbContextExtention
    {
        public static User AddToUser(this DbContext context, User model)
        {
            context.Set<User>().Add(model);
            return model;
        }

        public static User GetSingleUser(this DbContext context, int id)
        {
            return context.Set<User>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static User GetSingleUserByUserName(this DbContext context, string userName)
        {
            return context.Set<User>().Where(m => m.UserName == userName).FirstOrDefault();
        }

        public static User DeleteUser(this DbContext context, int id)
        {
            var model = context.GetSingleUser(id);
            if (model != null)
            {
                return context.Set<User>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeleteUser(this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeleteUser(id);
            }
        }

        public static IQueryable<User> QueryUser(this DbContext context, UserQueryCondition condition)
        {
            var query = context.Set<User>().AsQueryable();
            if(condition != null){
            }
            return query;
        }

        public static DbSet<User> UserDbSet(this DbContext context)
        {
            return context.Set<User>();
        }

    }

}
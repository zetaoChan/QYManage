using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Role")]
    public class Role
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "角色名")]
        public virtual string Name { get; set; }
        [LogFiled(Name = "系统角色名")]
        public virtual string SysName { get; set; }
        [LogFiled(Name = "权限Id数组")]
        public virtual string PermissionIds { get; set; }
    }

    public class RoleQueryCondition
    {
    }

    public static class RoleDbContextExtention
    {
        public static Role AddToRole(this DbContext context, Role model)
        {
            context.Set<Role>().Add(model);
            return model;
        }

        public static Role GetSingleRole(this DbContext context, int id)
        {
            return context.Set<Role>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Role DeleteRole(this DbContext context, int id)
        {
            var model = context.GetSingleRole(id);
            if (model != null)
            {
                return context.Set<Role>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeleteRole (this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeleteRole(id);
            }
        }

        public static IQueryable<Role> QueryRole(this DbContext context, RoleQueryCondition condition)
        {
            var query = context.Set<Role>().AsQueryable();
            if(condition != null){
            }
            return query;
        }

        public static DbSet<Role> RoleDbSet(this DbContext context)
        {
            return context.Set<Role>();
        }

    }

}
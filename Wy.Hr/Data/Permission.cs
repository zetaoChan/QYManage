using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Permission")]
    public class Permission
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "权限名称")]
        public virtual string Name { get; set; }
        [LogFiled(Name = "Url")]
        public virtual string Url { get; set; }
    }

    public class PermissionQueryCondition
    {
        public string Name { get; set; }
    }

    public static class PermissionDbContextExtention
    {
        public static Permission AddToPermission(this DbContext context, Permission model)
        {
            context.Set<Permission>().Add(model);
            return model;
        }

        public static Permission GetSinglePermission(this DbContext context, int id)
        {
            return context.Set<Permission>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Permission DeletePermission(this DbContext context, int id)
        {
            var model = context.GetSinglePermission(id);
            if (model != null)
            {
                return context.Set<Permission>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeletePermission(this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeletePermission(id);
            }
        }

        public static IQueryable<Permission> QueryPermission(this DbContext context, PermissionQueryCondition condition)
        {
            var query = context.Set<Permission>().AsQueryable();
            if(condition != null){
                if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(m => m.Name.Contains(condition.Name));
                }
            }
            return query;
        }

        public static DbSet<Permission> PermissionDbSet(this DbContext context)
        {
            return context.Set<Permission>();
        }

    }

}
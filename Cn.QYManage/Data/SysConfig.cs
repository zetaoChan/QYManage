using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Cn.QYManage.Common;

namespace Cn.QYManage.Data
{
    [Table("SysConfig")]
    public class SysConfig
    {
        [Key]
        [LogFiled(Name = "参数系统名称")]
        public virtual string SysName { get; set; }
        [LogFiled(Name = "参数名称")]
        public virtual string Name { get; set; }
        [LogFiled(Name = "参数值")]
        public virtual string Value { get; set; }
    }

    public class SysConfigQueryCondition
    {
    }

    public static class SysConfigDbContextExtention
    {
        public static SysConfig AddToSysConfig(this DbContext context, SysConfig model)
        {
            context.Set<SysConfig>().Add(model);
            return model;
        }

        public static SysConfig GetSingleSysConfig(this DbContext context, string sysName)
        {
            return context.Set<SysConfig>().Where(m => m.SysName.Equals(sysName)).FirstOrDefault();
        }

        public static SysConfig DeleteSysConfig(this DbContext context, string sysName)
        {
            var model = context.GetSingleSysConfig(sysName);
            if (model != null)
            {
                return context.Set<SysConfig>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static IQueryable<SysConfig> QuerySysConfig(this DbContext context, SysConfigQueryCondition condition)
        {
            IQueryable<SysConfig> query = context.Set<SysConfig>().AsQueryable();
            if (condition != null)
            {
            }
            return query;
        }

        public static DbSet<SysConfig> SysConfigDbSet(this DbContext context)
        {
            return context.Set<SysConfig>();
        }
    }

}
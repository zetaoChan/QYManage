using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Cn.QYManage.Common;

namespace Cn.QYManage.Data
{
    [Table("Department")]
    public class Department
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "部门编号")]
        public virtual string No { get; set; }
        [LogFiled(Name = "部门名称")]
        public virtual string Name { get; set; }
        [ForeignKey("ParentId")]
        public virtual Department ParentDepartment { get; set; }
        [LogFiled(Name = "上级部门ID")]
        public virtual int? ParentId { get; set; }
        [InverseProperty("ParentDepartment")]
        public virtual IList<Department> ChildrenDepartments { get; set; }
        [LogFiled(Name = "部门级别")]
        public virtual DepartmentGrade Grade { get; set; }
    }

    public class DepartmentQueryCondition
    {
        public int? ParentId { get; set; }
        public string DepartmentName { get; set; }
        public string ChiefName { get; set; }
    }

    public static class DepartmentDbContextExtention
    {
        public static Department AddToDepartment(this DbContext context, Department model)
        {
            context.Set<Department>().Add(model);
            return model;
        }

        public static Department GetSingleDepartment(this DbContext context, int id)
        {
            return context.Set<Department>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Department DeleteDepartment(this DbContext context, int id)
        {
            var model = context.GetSingleDepartment(id);
            if (model != null)
            {
                return context.Set<Department>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeleteDepartment(this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeleteDepartment(id);
            }
        }

        public static IQueryable<Department> QueryDepartment(this DbContext context, DepartmentQueryCondition condition)
        {
            IQueryable<Department> query = context.Set<Department>().AsQueryable();
            if (condition != null)
            {
                if (condition.ParentId.HasValue) { 
                    query = query.Where(m => m.ParentId == condition.ParentId.Value); 
                }
                if (!string.IsNullOrEmpty(condition.DepartmentName)) { 
                    query = query.Where(m => m.Name.Contains(condition.DepartmentName)); 
                }
            }
            return query;
        }

        public static DbSet<Department> DepartmentDbSet(this DbContext context)
        {
            return context.Set<Department>();
        }
    }

}
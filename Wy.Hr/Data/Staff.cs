using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Staff")]
    public class Staff
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "员工姓名")]
        public virtual string Name { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
        [LogFiled(Name = "所属部门ID")]
        public virtual int? DepartmentId { get; set; }
        [ForeignKey("PositionId")]
        public virtual Position Position { get; set; }
        [LogFiled(Name = "职位ID")]
        public virtual int? PositionId { get; set; }
        [LogFiled(Name = "性别")]
        public virtual Sex Sex { get; set; }
        [LogFiled(Name = "身份证号码")]
        public virtual string IDCardNum { get; set; }
        [LogFiled(Name = "出生日期")]
        public virtual DateTime BirthDate { get; set; }
        [LogFiled(Name = "籍贯")]
        public virtual NativePlace NativePlace { get; set; }
        [LogFiled(Name = "民族")]
        public virtual Nation Nation { get; set; }
        [LogFiled(Name = "学历")]
        public virtual Education Education { get; set; }
        [LogFiled(Name = "毕业院校")]
        public virtual string GraduatingAcademy { get; set; }
        [LogFiled(Name = "婚姻状况")]
        public virtual MaritalStatus MaritalStatus { get; set; }
        [LogFiled(Name = "入职日期")]
        public virtual DateTime EntryTime { get; set; }
        [LogFiled(Name = "邮箱")]
        public virtual string Email { get; set; }
        [LogFiled(Name = "手机")]
        public virtual string Mobile { get; set; }
        [LogFiled(Name = "现住地址")]
        public virtual string Address { get; set; }
    }

    public class StaffQueryCondition
    {
        public string Name { get; set; }
        public int? DepartmentId { get; set; }
    }

    public static class StaffDbContextExtention
    {
        public static Staff AddToStaff(this DbContext context, Staff model)
        {
            context.Set<Staff>().Add(model);
            return model;
        }

        public static Staff GetSingleStaff(this DbContext context, int id)
        {
            return context.Set<Staff>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Staff DeleteStaff(this DbContext context, int id)
        {
            var model = context.GetSingleStaff(id);
            if (model != null)
            {
                return context.Set<Staff>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeleteStaff(this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeleteStaff(id);
            }
        }

        public static IQueryable<Staff> QueryStaff(this DbContext context, StaffQueryCondition condition)
        {
            var query = context.Set<Staff>().AsQueryable();
            if(condition != null){
                if (!string.IsNullOrEmpty(condition.Name)) {
                    query = query.Where(m => m.Name.Contains(condition.Name));
                }
                //  部门
                if (condition.DepartmentId.HasValue)
                {
                    //  搜索部门及以下的
                    var department = context.Set<Department>().AsQueryable().Where(m => m.Id == condition.DepartmentId.Value).FirstOrDefault();
                    var ids = GetIDS(department);
                    query = query.Where(m => ids.Contains(m.DepartmentId.Value));
                }
            }
            return query;
        }

        public static DbSet<Staff> StaffDbSet(this DbContext context)
        {
            return context.Set<Staff>();
        }

        public static Int32[] GetIDS(Department department)
        {
            Int32[] ids = new Int32[] { };
            var idsList = ids.ToList();
            idsList.Add(department.Id);
            var list = department.ChildrenDepartments.ToList<Department>();
            if (list.Count() != 0)
            {
                for (var i = 0; i < list.Count(); i++)
                {
                    var cids = GetIDS(list[i]);
                    idsList.AddRange(cids.ToList());
                }
            }
            return idsList.ToArray();
        }

    }

}
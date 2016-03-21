using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Contact")]
    public class Contact
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

    public class ContactQueryCondition
    {
    }

    public static class ContactDbContextExtention
    {
        public static Contact AddToContact(this DbContext context, Contact model)
        {
            context.Set<Contact>().Add(model);
            return model;
        }

        public static Contact GetSingleContact(this DbContext context, int id)
        {
            return context.Set<Contact>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Contact DeleteContact(this DbContext context, int id)
        {
            var model = context.GetSingleContact(id);
            if (model != null)
            {
                return context.Set<Contact>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static IQueryable<Contact> QueryContact(this DbContext context, ContactQueryCondition condition)
        {
            var query = context.Set<Contact>().AsQueryable();
            if(condition != null){
            }
            return query;
        }

        public static DbSet<Contact> ContactDbSet(this DbContext context)
        {
            return context.Set<Contact>();
        }

    }

}
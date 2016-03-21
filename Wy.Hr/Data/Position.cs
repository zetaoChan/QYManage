using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Position")]
    public class Position
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "职位编号")]
        public virtual string No { get; set; }
        [LogFiled(Name = "职位名称")]
        public virtual string Name { get; set; }
        [LogFiled(Name = "职位等级")]
        public virtual PositionGrade Grade { get; set; }
    }

    public class PositionQueryCondition
    {
        public string Name { get; set; }
    }

    public static class PositionDbContextExtention
    {
        public static Position AddToPosition(this DbContext context, Position model)
        {
            context.Set<Position>().Add(model);
            return model;
        }

        public static Position GetSinglePosition(this DbContext context, int id)
        {
            return context.Set<Position>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Position DeletePosition(this DbContext context, int id)
        {
            var model = context.GetSinglePosition(id);
            if (model != null)
            {
                return context.Set<Position>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeletePosition(this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeletePosition(id);
            }
        }

        public static IQueryable<Position> QueryPosition(this DbContext context, PositionQueryCondition condition)
        {
            var query = context.Set<Position>().AsQueryable();
            if(condition != null){
                if (!string.IsNullOrEmpty(condition.Name))
                {
                    query = query.Where(m => m.Name.Contains(condition.Name));
                }
            }
            return query;
        }

        public static DbSet<Position> PositionDbSet(this DbContext context)
        {
            return context.Set<Position>();
        }

    }

}
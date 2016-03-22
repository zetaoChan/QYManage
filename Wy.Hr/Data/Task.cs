using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Task")]
    public class Task
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "任务标题")]
        public virtual string Title { get; set; }
        [LogFiled(Name = "任务内容")]
        public virtual string Contents { get; set; }
        [LogFiled(Name = "任务状态")]
        public virtual TaskStatus Status { get; set; }
        [LogFiled(Name = "发布时间")]
        public virtual DateTime AddTime { get; set; }
        [LogFiled(Name = "发布者")]
        public virtual string AddUser { get; set; }
        [LogFiled(Name = "执行者")]
        public virtual string Executor { get; set; }
        [LogFiled(Name = "期望完成时间")]
        public virtual DateTime ExpectedTime { get; set; }
        [LogFiled(Name = "完成时间")]
        public virtual DateTime? FinishedTime { get; set; }
    }

    public class TaskQueryCondition
    {
    }

    public static class TaskDbContextExtention
    {
        public static Task AddToTask(this DbContext context, Task model)
        {
            context.Set<Task>().Add(model);
            return model;
        }

        public static Task GetSingleTask(this DbContext context, int id)
        {
            return context.Set<Task>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Task DeleteTask(this DbContext context, int id)
        {
            var model = context.GetSingleTask(id);
            if (model != null)
            {
                return context.Set<Task>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeleteTask(this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeleteTask(id);
            }
        }



        public static IQueryable<Task> QueryTask(this DbContext context, TaskQueryCondition condition)
        {
            var query = context.Set<Task>().AsQueryable();
            if(condition != null){
            }
            return query;
        }

        public static DbSet<Task> TaskDbSet(this DbContext context)
        {
            return context.Set<Task>();
        }

    }

}
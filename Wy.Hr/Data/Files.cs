using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Data
{
    [Table("Files")]
    public class Files
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "文件编号")]
        public virtual string No { get; set; }
        [LogFiled(Name = "文件名称")]
        public virtual string Name { get; set; }
        [LogFiled(Name = "上传者")]
        public virtual string Uploader { get; set; }
        [LogFiled(Name = "上传时间")]
        public virtual DateTime UploadTime { get; set; }
    }

    public class FilesQueryCondition
    {
    }

    public static class FilesDbContextExtention
    {
        public static Files AddToFiles(this DbContext context, Files model)
        {
            context.Set<Files>().Add(model);
            return model;
        }

        public static Files GetSingleFiles(this DbContext context, int id)
        {
            return context.Set<Files>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Files GetFilesByNo(this DbContext context, string no)
        {
            return context.Set<Files>().Where(m => m.No == no).FirstOrDefault();
        }

        public static Files DeleteFiles(this DbContext context, int id)
        {
            var model = context.GetSingleFiles(id);
            if (model != null)
            {
                return context.Set<Files>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeleteFiles (this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeleteFiles(id);
            }
        }

        public static IQueryable<Files> QueryFiles(this DbContext context, FilesQueryCondition condition)
        {
            var query = context.Set<Files>().AsQueryable();
            if(condition != null){
            }
            return query;
        }

        public static DbSet<Files> FilesDbSet(this DbContext context)
        {
            return context.Set<Files>();
        }

    }

}
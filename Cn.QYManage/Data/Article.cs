using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using Cn.QYManage.Common;

namespace Cn.QYManage.Data
{
    [Table("Article")]
    public class Article
    {
        public virtual int Id { get; set; }
        [LogFiled(Name = "文章标题")]
        public virtual string Title { get; set; }
        [LogFiled(Name = "文章类型")]
        public virtual ArticleType Type { get; set; }
        [LogFiled(Name = "Article内容")]
        public virtual string Contents { get; set; }
        [LogFiled(Name = "添加时间")]
        public virtual DateTime AddTime { get; set; }
        [LogFiled(Name = "创建人")]
        public virtual string AddUser { get; set; }
    }

    public class ArticleQueryCondition
    {
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public static class ArticleDbContextExtention
    {
        public static Article AddToArticle(this DbContext context, Article model)
        {
            context.Set<Article>().Add(model);
            return model;
        }

        public static Article GetSingleArticle(this DbContext context, int id)
        {
            return context.Set<Article>().Where(m => m.Id == id).FirstOrDefault();
        }

        public static Article DeleteArticle(this DbContext context, int id)
        {
            var model = context.GetSingleArticle(id);
            if (model != null)
            {
                return context.Set<Article>().Remove(model);
            }
            else
            {
                return null;
            }
        }

        public static void BatchDeleteArticle(this DbContext context, int[] ids)
        {
            foreach (var id in ids)
            {
                context.DeleteArticle(id);
            }
        }

        public static IQueryable<Article> QueryArticle(this DbContext context, ArticleQueryCondition condition)
        {
            var query = context.Set<Article>().AsQueryable();
            if(condition != null){
                if(!string.IsNullOrEmpty(condition.Title)){
                    query = query.Where(m => m.Title.Contains(condition.Title));
                }
                if (condition.Type != "all")
                {
                    var type = ArticleType.新闻;
                    if(condition.Type == "notice"){
                        type = ArticleType.通知;
                    }
                    query = query.Where(m => m.Type == type);
                }
            }
            return query;
        }

        public static DbSet<Article> ArticleDbSet(this DbContext context)
        {
            return context.Set<Article>();
        }

    }

}
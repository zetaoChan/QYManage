using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Wy.Hr.Common;
using Wy.Hr.Data;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class ArticleAPIController : ApiBaseController
    {

        public static ArticleAPIController Instance = new ArticleAPIController();

        #region 数据获取方法
        /// <summary>
        /// 获得用户分页列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult GetPagedList(ArticlePagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var condition = new ArticleQueryCondition
                    {
                        Title = args.Title,
                        Type = args.Type
                    };
                    var result = db.QueryArticle(condition)
                        .OrderByDescending(m => m.AddTime)
                        .Select(m => new ArticleModel()
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Type = m.Type,
                            AddTime = m.AddTime,
                            AddUser = m.AddUser
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<ArticleModel>()
                    {
                        Items = result.Items,
                        PageIndex = result.CurrentPageIndex,
                        PageSize = result.PageSize,
                        TotalCount = result.TotalItemCount,
                        TotalPageCount = result.TotalPageCount
                    });
                }

            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while(e.InnerException != null){
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }

        /// <summary>
        /// 获得前n条文章
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult GetTopArticles(TopArticleArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryArticle(null)
                        .Where(m => m.Type == args.Type)
                        .OrderByDescending(m => m.AddTime)
                        .Select(m => new ArticleModel()
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Type = m.Type,
                            Contents = m.Contents,
                            AddTime = m.AddTime,
                            AddUser = m.AddUser
                        })
                        .Take(args.Top)
                        .ToList();
                    return Success(list);
                }

            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }

        /// <summary>
        /// 获得文章详情
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult<ArticleModel> GetDetail(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (args == null) throw new Exception("请求参数异常");
                    var entity = db.GetSingleArticle(args.Id);
                    if (entity == null) throw new Exception("用户不存在");
                    Mapper.CreateMap<Article, ArticleModel>();
                    var model = Mapper.Map<Article, ArticleModel>(entity);
                    return Success<ArticleModel>(model);
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error<ArticleModel>(error);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult GetDetailAndList(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (args == null) throw new Exception("请求参数异常");
                    var entity = db.GetSingleArticle(args.Id);
                    if (entity == null) throw new Exception("用户不存在");
                    Mapper.CreateMap<Article, ArticleModel>();
                    var model = Mapper.Map<Article, ArticleModel>(entity);
                    var list = db.QueryArticle(null)
                        .Where(m => m.Type == model.Type)
                        .Select(m => new ArticleModel()
                        {
                            Id = m.Id,
                            Title = m.Title,
                            AddTime = m.AddTime
                        })
                        .ToList();
                    return Success(new { 
                        article = model,
                        items = list
                    });
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }
        #endregion

        #region 数据操作方法
        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult Add(ArticleAddModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<ArticleAddModel, Article>();
                    var entity = Mapper.Map<ArticleAddModel, Article>(model);
                    entity.AddUser = User.Identity.Name;
                    entity.AddTime = DateTime.Now.ToLocalTime();
                    db.AddToArticle(entity);
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error<UserModel>(error);
            }
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult Update(ArticleEditModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleArticle(model.Id);
                    Mapper.CreateMap<ArticleEditModel, Article>();
                    Mapper.Map<ArticleEditModel, Article>(model, entity);
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error<UserModel>(error);
            }
        }

        /// <summary>
        /// 批量删除文章
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult BatchRemove(BatchModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    db.BatchDeleteArticle(args.Ids);
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                var e = ex;
                var error = e.Message;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error<UserModel>(error);
            }
        }
        #endregion

    }
}
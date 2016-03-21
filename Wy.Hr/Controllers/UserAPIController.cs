using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using AutoMapper;
using Wy.Hr.Common;
using Wy.Hr.Data;
using System.Web.Configuration;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class UserAPIController : ApiBaseController
    {

        public static UserAPIController Instance = new UserAPIController();

        #region 数据获取方法
        /// <summary>
        /// 获得用户分页列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult GetPagedList(UserPagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var result = db.QueryUser(null)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new UserModel()
                        {
                            Id = m.Id,
                            UserName = m.UserName,
                            Email = m.Email,
                            CreateTime = m.CreateTime,
                            Creator = m.Creator,
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<UserModel>()
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

        [HttpPost]
        public APIResult<UserModel> GetDetail(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    if (args == null) throw new Exception("请求参数异常");
                    var entity = db.GetSingleUser(args.Id);
                    if (entity == null) throw new Exception("用户不存在");
                    Mapper.CreateMap<User, UserModel>();
                    var model = Mapper.Map<User, UserModel>(entity);
                    return Success<UserModel>(model);
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

        #region 数据操作方法
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult Add(UserAddModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<UserAddModel, User>();
                    var entity = Mapper.Map<UserAddModel, User>(model);
                    entity.Password = CommonUtil.MD5(entity.Password, Encoding.GetEncoding("UTF-8"));
                    entity.Creator = User.Identity.Name;
                    entity.CreateTime = DateTime.Now.ToLocalTime();
                    db.AddToUser(entity);
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
        /// 修改用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult Update(UserEditModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleUser(model.Id);
                    Mapper.CreateMap<UserEditModel, User>();
                    Mapper.Map<UserEditModel, User>(model, entity);
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
        /// 批量删除用户
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult BatchRemove(UserBatchDelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    db.BatchDeleteUser(args.Ids);
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
        /// 修改用户密码
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult ChangePassword(UserChangePswArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var userName = User.Identity.Name;
                    var user = db.GetSingleUserByUserName(userName);
                    var md5OldPassword = CommonUtil.MD5(args.OldPassword, Encoding.GetEncoding("UTF-8"));
                    if (md5OldPassword != user.Password) throw new Exception("原密码不正确");
                    var md5NewPassword = CommonUtil.MD5(args.NewPassword, Encoding.GetEncoding("UTF-8"));
                    user.Password = md5NewPassword;
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
                return Error(error);
            }
        }

        /// <summary>
        /// 初始化用户密码
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public APIResult ResetPassword(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var user = db.GetSingleUser(args.Id);
                    var defaultPassword = db.GetSingleSysConfig("defaultPassword").Value;
                    var md5Password = CommonUtil.MD5(defaultPassword, Encoding.GetEncoding("UTF-8"));
                    user.Password = md5Password;
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
                return Error(error);
            }
        }
        #endregion

    }
}
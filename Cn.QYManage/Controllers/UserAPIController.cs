using System;
using System.Linq;
using System.Text;
using System.Web.Http;
using AutoMapper;
using Cn.QYManage.Common;
using Cn.QYManage.Data;
using Cn.QYManage.Models;
using System.Collections.Generic;

namespace Cn.QYManage.Controllers
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
                            RoleIds = m.RoleIds
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
        public APIResult GetSelUsers()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryUser(null)
                        .Select(m => new UserModel{
                            Id = m.Id,
                            UserName = m.UserName
                        })
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

        [HttpPost]
        public string[] GetPermissionUrls()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var currentUser = db.GetSingleUserByUserName(User.Identity.Name);
                    string[] roles = currentUser.RoleIds.Split(',');
                    var roleList = db.QueryRole(null).Where(m => roles.Contains(m.Id.ToString())).ToList();
                    List<Permission> resultList = new List<Permission>();
                    foreach (var item in roleList)
                    {
                        string[] permissions = item.PermissionIds.Split(',');
                        var permissionList = db.QueryPermission(null).Where(m => permissions.Contains(m.Id.ToString())).ToList();
                        if (permissionList != null)
                        {
                            resultList.AddRange(permissionList);
                        }
                    }
                    string[] permissionUrls = resultList.Distinct().Select(m => m.Url).ToArray();
                    return permissionUrls;
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
                return new string[1];
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
                    var entity = new User { 
                        UserName = model.UserName,
                        RoleIds = model.RoleIds
                    };
                    entity.Password = CommonUtil.MD5(entity.Password, Encoding.GetEncoding("UTF-8"));
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

        public static bool AddStaffUser(int staffId)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var staff = db.GetSingleStaff(staffId);
                    var defaultPsw = CommonUtil.MD5("111111", Encoding.GetEncoding("UTF-8"));
                    var entity = new User
                    {
                        UserName = staff.Email,
                        Password = defaultPsw,
                        RoleIds = "7"
                    };
                    db.AddToUser(entity);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
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
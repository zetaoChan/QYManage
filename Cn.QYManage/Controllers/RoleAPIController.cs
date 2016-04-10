using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Cn.QYManage.Common;
using Cn.QYManage.Data;
using Cn.QYManage.Models;

namespace Cn.QYManage.Controllers
{
    public class RoleAPIController : ApiBaseController
    {

        public static RoleAPIController Instance = new RoleAPIController();

        [Authorize]
        [HttpPost]
        public APIResult GetPagedList(RolePagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var result = db.QueryRole(null)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new RoleModel()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            SysName = m.SysName,
                            PermissionIds = m.PermissionIds
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<RoleModel>()
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

        [Authorize]
        [HttpPost]
        public APIResult<List<RoleModel>> GetList(RolePagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryRole(null)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new RoleModel()
                        {
                            Id = m.Id,
                        })
                        .ToList();
                    return Success<List<RoleModel>>(list);
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
                return Error<List<RoleModel>>(error);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult<RoleModel> GetDetail(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    //  检验参数是否为空
                    if (args == null) { throw new Exception("请求参数异常"); }
                    //  查找部门详情
                    var entity = db.GetSingleRole(args.Id);
                    if (entity == null) { throw new Exception("用户不存在"); }
                    //  转换为部门模型
                    Mapper.CreateMap<Role, RoleModel>();
                    var model = Mapper.Map<Role, RoleModel>(entity);
                    return Success<RoleModel>(model);
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
                return Error<RoleModel>(error);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult GetAllRole()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = db.QueryRole(null)
                        .OrderBy(m => m.SysName)
                        .Select(m => new RoleModel()
                        {
                            Id = m.Id,
                            SysName = m.SysName,
                            Name = m.Name
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

        #region 数据操作方法
        [HttpPost]
        [Authorize]
        public APIResult Add(RoleModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = new Role { 
                        Name = model.Name,
                        SysName = model.SysName,
                        PermissionIds = model.PermissionIds
                    };
                    db.AddToRole(entity);
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
                return Error<RoleModel>(error);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult Update(RoleModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleRole(model.Id);
                    entity.Name = model.Name;
                    entity.SysName = model.SysName;
                    entity.PermissionIds = model.PermissionIds;
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
                return Error<RoleModel>(error);
            }
        }

        /// <summary>
        /// 批量删除角色
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult BatchRemove(RoleBatchDelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    db.BatchDeleteRole(args.Ids);
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
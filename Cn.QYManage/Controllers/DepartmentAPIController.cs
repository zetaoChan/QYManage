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
    public class DepartmentAPIController : ApiBaseController
    {

        public static DepartmentAPIController Instance = new DepartmentAPIController();

        /// <summary>
        /// 获取部门分页列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult GetPagedList(DepartmentPagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var conidtion = new DepartmentQueryCondition { 
                        DepartmentName = args.DepartmentName,
                        ParentId = args.ParentId
                    };

                    var result = db.QueryDepartment(conidtion)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new DepartmentModel()
                        {
                            Id = m.Id,
                            No = m.No,
                            Name = m.Name,
                            ParentId = m.ParentId,
                            ParentName = m.ParentDepartment == null ? "无" : m.ParentDepartment.Name,
                            ParentDept = m.ParentDepartment,
                            Grade = m.Grade
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<DepartmentModel>()
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
                return Error(ex.Message);
            }
        }

        [HttpPost]
        public APIResult GetSelList()
        {
            try
            {
                using (var db = new DataContext())
                {

                    var list = db.QueryDepartment(null)
                        .OrderBy(m => m.ParentId)
                        .Select(m => new DepartmentModel()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            ParentId = m.ParentId
                        })
                        .ToList();
                    var treeList = new List<DepartmentModel>();
                    //var tempLevel = 0;
                    //while(list.Count != 0){
                    //    var tempList = 
                    //    foreach(var item in list){
                    //        item.Name = "　　" + item.Name;
                    //        treeList.Add(item);
                    //    }
                    //}
                    return Success(list);
                }

            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取部门详情
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult<DepartmentModel> GetDetail(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    //  检验参数是否为空
                    if (args == null) { throw new Exception("请求参数异常！"); }
                    //  查找部门详情
                    var entity = db.GetSingleDepartment(args.Id);
                    if (entity == null) { throw new Exception("部门不存在！"); }
                    //  转换为部门模型
                    Mapper.CreateMap<Department, DepartmentModel>();
                    var model = Mapper.Map<Department, DepartmentModel>(entity);
                    model.ParentName = entity.ParentDepartment == null ? "" : entity.ParentDepartment.Name;
                    return Success<DepartmentModel>(model);
                }
            }
            catch (Exception ex)
            {
                return Error<DepartmentModel>(ex.Message);
            }
        }

        /// <summary>
        /// 新增部门操作
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult Add(DepartmentModel args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<DepartmentModel, Department>();
                    var model = Mapper.Map<DepartmentModel, Department>(args);
                    db.AddToDepartment(model);
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        /// <summary>
        /// 更新部门操作
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult Update(DepartmentModel args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var model = db.GetSingleDepartment(args.Id);
                    if (model == null) { throw new Exception("部门不存在！"); }
                    model.No = args.No;
                    model.Name = args.Name;
                    model.Grade = args.Grade;
                    model.ParentId = args.ParentId;
                    db.SaveChanges();
                    return Success();
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public APIResult BatchRemove(BatchModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    db.BatchDeleteDepartment(args.Ids);
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
        /// 获得选择部门列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult GetSelTreeList()
        {
            try
            {
                using (var db = new DataContext())
                {
                    var list = new List<TreeItem>();
                    var treeList = GetDepartmentTree().Content;
                    foreach (var item in treeList)
                    {
                        list.AddRange(GetChildList(item));
                    }
                    return Success(list);
                }
            }
            catch (Exception ex)
            {
                var error = ex.Message;
                var e = ex;
                while (e.InnerException != null)
                {
                    e = e.InnerException;
                    error += "|" + e.Message;
                }
                return Error(error);
            }
        }

        private List<TreeItem> GetChildList(TreeItem item)
        {
            var list = new List<TreeItem>();
            list.Add(item);
            if (item.Children != null && item.Children.Count > 0)
            {
                foreach (var cItem in item.Children)
                {
                    var cList = GetChildList(cItem);
                    foreach (var c in cList)
                    {
                        c.Label = "　　" + c.Label;
                    }
                    list.AddRange(cList);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得部门树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public APIResult<List<TreeItem>> GetDepartmentTree()
        {
            try
            {
                using (var db = new DataContext())
                {
                    //  获取所有部门
                    var list = db.QueryDepartment(null)
                        .Select(m => new TreeItem()
                        {
                            Id = m.Id,
                            Label = m.Name,
                            ParentId = m.ParentId
                        })
                        .ToList();

                    //  构造树结构
                    var tree = BuildTree(list);
                    return Success<List<TreeItem>>(tree);
                }

            }
            catch (Exception ex)
            {
                return Error<List<TreeItem>>(ex.Message);
            }
        }

        /// <summary>
        /// list转tree方法
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private List<TreeItem> BuildTree(List<TreeItem> list)
        {
            var tree = new List<TreeItem>();
            foreach (var item in list)
            {
                if (!item.ParentId.HasValue)
                {
                    tree.Add(item);
                }
                foreach (var t in list)
                {
                    if (t.ParentId == item.Id)
                    {
                        if (item.Children == null)
                        {
                            var children = new List<TreeItem>();
                            children.Add(t);
                            item.Children = children;
                        }
                        else
                        {
                            item.Children.Add(t);
                        }
                    }
                }
            }
            return tree;
        }

        /// <summary>
        /// 获取部门字段参数
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<ParameterModel> GetParameterList()
        {
            var list = new List<ParameterModel>() { 
                new ParameterModel () { Key = "No", Name = "部门编号" },
                new ParameterModel() { Key = "Name", Name = "部门名称" },
                new ParameterModel() { Key = "Grade", Name = "等级" },
                new ParameterModel() { Key = "Area", Name = "地区" },
                new ParameterModel() { Key = "ParentName", Name = "上级部门" },
                new ParameterModel() { Key = "ChiefName", Name = "负责人" },
                new ParameterModel() { Key = "NumOfPreparation", Name = "编制人数" },
                new ParameterModel() { Key = "StaffNum", Name = "在职人数" },
                new ParameterModel() { Key = "VerifyStatus", Name = "审核状态" },
                new ParameterModel() { Key = "CreateDate", Name = "创建日期" },
                new ParameterModel() { Key = "Creator", Name = "创建人" },
                new ParameterModel() { Key = "LastUpdateDate", Name = "最近修改日期" },
                new ParameterModel() { Key = "Updater", Name = "修改人" },
                new ParameterModel() { Key = "Remark", Name = "备注" }
            };
            return list;
        }

    }
}
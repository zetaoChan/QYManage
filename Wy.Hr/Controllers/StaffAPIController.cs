using System;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Wy.Hr.Common;
using Wy.Hr.Data;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class StaffAPIController : ApiBaseController
    {

        public static StaffAPIController Instance = new StaffAPIController();

        /// <summary>
        /// 获取员工分页列表
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult GetPagedList(StaffPagedArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var conidtion = new StaffQueryCondition { 
                        Name = args.Name,
                        DepartmentId = args.DepartmentId
                    };

                    var result = db.QueryStaff(conidtion)
                        .OrderByDescending(m => m.Id)
                        .Select(m => new StaffModel()
                        {
                            Id = m.Id,
                            Name = m.Name,
                            DepartmentName = m.Department == null ? "" : m.Department.Name,
                            PositionName = m.Position == null ? "" : m.Position.Name,
                            DepartmentId = m.DepartmentId,
                            PositionId = m.PositionId,
                            IDCardNum = m.IDCardNum,
                            Nation = m.Nation,
                            NativePlace = m.NativePlace,
                            GraduatingAcademy = m.GraduatingAcademy,
                            BirthDate = m.BirthDate,
                            EntryTime = m.EntryTime,
                            Mobile = m.Mobile,
                            Sex = m.Sex,
                            Email = m.Email,
                            MaritalStatus = m.MaritalStatus,
                            Address = m.Address,
                            PositionGrade = m.Position == null ? PositionGrade.Z1 : m.Position.Grade,
                            Education = m.Education
                        })
                        .ToPagedList(args.PageIndex, args.PageSize);
                    return Success(new PagedResult<StaffModel>()
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

        /// <summary>
        /// 获取员工详情
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult<StaffModel> GetDetail(SingleModelArgs args)
        {
            try
            {
                using (var db = new DataContext())
                {
                    //  查找部门详情
                    var entity = db.GetSingleStaff(args.Id);
                    if (entity == null) { throw new Exception("员工不存在！"); }
                    //  转换为部门模型
                    Mapper.CreateMap<Staff, StaffModel>();
                    var model = Mapper.Map<Staff, StaffModel>(entity);
                    return Success<StaffModel>(model);
                }
            }
            catch (Exception ex)
            {
                return Error<StaffModel>(ex.Message);
            }
        }

        /// <summary>
        /// 新增员工操作
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult Add(StaffModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    Mapper.CreateMap<StaffModel, Staff>();
                    var obj = Mapper.Map<StaffModel, Staff>(model);
                    db.AddToStaff(obj);
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
        /// 更新员工操作
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [HttpPost]
        public APIResult Update(StaffModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var obj = db.GetSingleStaff(model.Id);
                    Mapper.CreateMap<StaffModel, Staff>();
                    Mapper.Map<StaffModel, Staff>(model, obj);
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
                    db.BatchDeleteStaff(args.Ids);
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

    }
}
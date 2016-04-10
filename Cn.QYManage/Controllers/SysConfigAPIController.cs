using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cn.QYManage.Common;
using Cn.QYManage.Data;
using Cn.QYManage.Models;

namespace Cn.QYManage.Controllers
{
    public class SysConfigAPIController : ApiBaseController
    {

        public static SysConfigAPIController Instance = new SysConfigAPIController();

        #region 信息获取方法
        /// <summary>
        /// 获取系统参数列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult<List<SysConfigModel>> GetList()
        {
            try
            {
                using (var db = new DataContext())
                {

                    var list = db.QuerySysConfig(null)
                        .OrderBy(m => m.SysName)
                        .Select(m => new SysConfigModel()
                        {
                            Name = m.Name,
                            SysName = m.SysName,
                            Value = m.Value
                        })
                        .ToList();
                    return Success<List<SysConfigModel>>(list);
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
                return Error<List<SysConfigModel>>(error);
            }
        }
        #endregion

        #region 数据操作方法
        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public APIResult SaveConfig(SysConfigModel model)
        {
            try
            {
                using (var db = new DataContext())
                {
                    var entity = db.GetSingleSysConfig(model.SysName);
                    if (entity == null) throw new Exception("系统参数不存在");
                    entity.Name = model.Name;
                    entity.Value = model.Value;
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
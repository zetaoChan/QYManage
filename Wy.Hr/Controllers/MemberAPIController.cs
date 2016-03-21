using System;
using System.Text;
using System.Web.Http;
using System.Web.Security;
using Wy.Hr.Common;
using Wy.Hr.Data;
using Wy.Hr.Models;

namespace Wy.Hr.Controllers
{
    public class MemberAPIController : ApiBaseController
    {
        [HttpPost]
        public APIResult Login(LoginArgsModel args)
        {
            try
            {
                using(var db = new DataContext()){
                    if (string.IsNullOrEmpty(args.Username)) throw new Exception("账号不能为空");
                    if (string.IsNullOrEmpty(args.Password)) throw new Exception("密码不能为空");
                    var user = db.GetSingleUserByUserName(args.Username);
                    if (user == null) throw new Exception("账号不存在");
                    var md5Password = CommonUtil.MD5(args.Password, Encoding.GetEncoding("UTF-8"));
                    if (user.Password != md5Password) throw new Exception("密码不正确");
                    FormsAuthentication.SetAuthCookie(args.Username, false);
                    return Success();
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

    }
}
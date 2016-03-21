using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using Wy.Hr.Common;

namespace Wy.Hr.Controllers
{
    public class ApiBaseController : ApiController
    {
        internal APIResult Success()
        {
            return APIResult.CreateSuccess();
        }

        internal APIResult Success(string message)
        {
            return APIResult.CreateSuccess(message);
        }

        internal APIResult<T> Success<T>(T content, string message)
        {
            return APIResult.CreateSuccess<T>(content, message);
        }

        internal APIResult<T> Success<T>(T content)
        {
            return APIResult.CreateSuccess<T>(content);
        }

        internal APIResult Error(int errorCode, string message)
        {
            return APIResult.CreateError(errorCode, message);
        }

        internal APIResult Error(string message)
        {
            return APIResult.CreateError(0, message);
        }

        internal APIResult<T> Error<T>(int errorCode, string message)
        {
            return APIResult.CreateError<T>(errorCode, message);
        }

        internal APIResult<T> Error<T>(string message)
        {
            return APIResult.CreateError<T>(0, message);
        }


        #region tools
        protected string MemberPasswordToMD5(string password)
        {
            return CommonUtil.MD5(password + "cscoder.cn.2015", System.Text.Encoding.Default);
        }
        #endregion

        

        public string datadbConnectionString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["datadb:connectionString"];
            }
        }

        public string datadbDbName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["datadb:dbName"];
            }
        }
    }
}

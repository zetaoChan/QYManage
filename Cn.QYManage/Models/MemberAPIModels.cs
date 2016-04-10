using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Cn.QYManage.Data;

namespace Cn.QYManage.Models
{

    /// <summary>
    /// 登录模型
    /// </summary>
    public class LoginArgsModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

}
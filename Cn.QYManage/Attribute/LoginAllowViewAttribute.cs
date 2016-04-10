using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cn.QYManage.Attribute
{
    /// <summary>
    /// 用户登录可访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class LoginAllowViewAttribute : System.Attribute
    {

    }
}
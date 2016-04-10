using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cn.QYManage.Attribute
{
    /// <summary>
    /// 用户登录可访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DefaultPageAttribute : System.Attribute
    {
    }
}
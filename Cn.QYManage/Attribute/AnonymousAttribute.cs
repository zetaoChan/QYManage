using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cn.QYManage.Attribute
{
    /// <summary>
    /// 匿名可访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AnonymousAttribute : System.Attribute
    {


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wy.Hr.Attribute
{
    /// <summary>
    /// 匿名可访问
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AnonymousAttribute : System.Attribute
    {


    }
}
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Wy.Hr.Data;

namespace Wy.Hr.Attribute
{
    /// <summary>
    /// 权限拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class PermissionFilterAttribute : ActionFilterAttribute
    {
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //  权限拦截是否忽略
            if (filterContext == null) throw new ArgumentNullException("filterContext");

            if (!AuthorizeCore(filterContext))
            {
                filterContext.RequestContext.HttpContext.Response.Redirect("~/home/login");
            }
            else {
                //  用户权限验证
                var controllerName = filterContext.RouteData.Values["controller"].ToString();
                var actionName = filterContext.RouteData.Values["action"].ToString();
                User currentUser = null;
                Role role = null;
                using (var db = new DataContext())
                {
                    currentUser = db.GetSingleUserByUserName(filterContext.HttpContext.User.Identity.Name);
                    role = db.GetSingleRole(1);
                }

                if (currentUser.Roles.Contains(role))
                {
                    //filterContext.RequestContext.HttpContext.Response.Redirect("~/home/Login");
                }
                else
                {
                    filterContext.RequestContext.HttpContext.Response.Redirect("~/Error");
                }
            }
            
        }

        /// <summary>
        /// [Anonymous标记]验证是否匿名访问
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckAnonymous(ActionExecutingContext filterContext)
        {
            object[] attrsAnonymous = filterContext.ActionDescriptor.GetCustomAttributes(typeof(AnonymousAttribute), true);
            var Anonymous = attrsAnonymous.Length == 1;
            return Anonymous;
        }

        /// <summary>
        /// [LoginAllowView标记]验证是否登录就可以访问(如果已经登陆,那么不对于标识了LoginAllowView的方法就不需要验证了
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public bool CheckLoginAllowView(ActionExecutingContext filterContext)
        {
            object[] attrs = filterContext.ActionDescriptor.GetCustomAttributes(typeof(LoginAllowViewAttribute), true);
            var viewMethod = attrs.Length == 1;
            return viewMethod;
        }

        /// <summary>
        /// 权限判断业务逻辑
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="isViewPage">是否是页面</param>
        /// <returns></returns>
        protected virtual bool AuthorizeCore(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext == null) throw new ArgumentNullException("httpContext");

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return true;
            }
            else {
                return false;
            }
        }

    }
}
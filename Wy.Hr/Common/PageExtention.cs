using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wy.Hr.Common
{
    public static class PageExtention
    {
        public static bool HasQueryString(this System.Web.UI.Page page, string qname)
        {
            return !string.IsNullOrEmpty(page.Request.QueryString[qname]);
        }

        public static T QueryString<T>(this System.Web.UI.Page page, string qname)
        {

            object v = page.Request.QueryString[qname];
            
                return (T)Convert.ChangeType(v, typeof(T));
        }

        public static string GetIP(this System.Web.UI.Page page)
        {
            return CommonUtil.GetRealIP();
        }
    }
}

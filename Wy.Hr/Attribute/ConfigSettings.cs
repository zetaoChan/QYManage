using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Wy.Hr.Common;

namespace Wy.Hr.Attribute
{
    public class ConfigSettings
    {
        private static string xmlPath = CommonUtil.GetMapPath("/AllowPageSettings.xml");

        public static int GetAdminUserID()
        {
            int value = 0;
            if (HttpContext.Current.Application["AdminUserID"] == null)
            {
                using (var reader = new XmlTextReader(xmlPath))
                {
                    value = Convert.ToInt32(reader.GetAttribute("value", "/pages/Admin/AdminUserID"));
                    HttpContext.Current.Application["AdminUserID"] = value;
                }
            }
            else
            {
                value = (int)HttpContext.Current.Application["AdminUserID"];
            }
            return value;
        }

        public static int GetAdminUserRoleID()
        {
            int value = 0;
            if (HttpContext.Current.Application["AdminUserRoleID"] == null)
            {
                using (var reader = new XmlTextReader(xmlPath))
                {
                    value = Convert.ToInt32(reader.GetAttribute("value", "/pages/Admin/AdminUserID"));
                    HttpContext.Current.Application["AdminUserID"] = value;
                }
            }
            else
            {
                value = (int)HttpContext.Current.Application["AdminUserID"];
            }
            return value;
        }

        /// <summary>
        /// 读取所有允许访问的路径（主要针对登录，不进行权限验证）
        /// </summary>
        /// <returns></returns>
        public static IList<string> GetAllAllowPages()
        {
            List<string> pages = new List<string>();
            if (HttpContext.Current.Application["AllowPage"] == null)
            {
                XDocument xDoc = XDocument.Load(xmlPath);
                var pageList = xDoc.Root.Descendants("page");
                foreach (var page in pageList)
                {
                    pages.Add(page.Value.ToString());
                }
                HttpContext.Current.Application["AllowPage"] = pages;
            }
            else
            {
                pages = (List<string>)HttpContext.Current.Application["AllowPage"];
            }
            return pages;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Web;
using System.Net;

namespace Cn.QYManage.Common
{
    public class CommonUtil
    {
        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str, Encoding encoding)
        {
            byte[] b = encoding.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        public static String CreateIntNoncestr(int length)
        {
            String chars = "0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static String CreateNoncestr(int length)
        {
            String chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static String CreateNoncestr()
        {
            String chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static int ToTimestamp(DateTime value)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(value - startTime).TotalSeconds;
        }

        //public static string FormatQueryParaMap(Dictionary<string, string> parameters)
        //{

        //    string buff = "";
        //    try
        //    {

        //        var result = from pair in parameters orderby pair.Key select pair;
        //        foreach (KeyValuePair<string, string> pair in result)
        //        {
        //            if (pair.Key != "")
        //            {
        //                buff += pair.Key + "="
        //                        + System.Web.HttpUtility.UrlEncode(pair.Value) + "&";
        //            }
        //        }
        //        if (buff.Length == 0 == false)
        //        {
        //            buff = buff.Substring(0, (buff.Length - 1) - (0));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new SDKRuntimeException(e.Message);
        //    }

        //    return buff;
        //}

        //public static string FormatBizQueryParaMap(Dictionary<string, string> paraMap,
        //        bool urlencode)
        //{

        //    string buff = "";
        //    try
        //    {
        //        var result = from pair in paraMap orderby pair.Key select pair;
        //        foreach (KeyValuePair<string, string> pair in result)
        //        {
        //            if (!string.IsNullOrEmpty(pair.Key))
        //            {

        //                string key = pair.Key;
        //                string val = pair.Value;
        //                if (urlencode)
        //                {
        //                    val = System.Web.HttpUtility.UrlEncode(val);
        //                }
        //                buff += key + "=" + val + "&";

        //            }
        //        }

        //        if (buff.Length == 0 == false)
        //        {
        //            buff = buff.Substring(0, (buff.Length - 1) - (0));
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new SDKRuntimeException(e.Message);
        //    }
        //    return buff;
        //}

        public static bool IsNumeric(String str)
        {
            try
            {
                int.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ArrayToXml(Dictionary<string, string> arr)
        {
            String xml = "<xml>";

            foreach (KeyValuePair<string, string> pair in arr)
            {
                String key = pair.Key;
                String val = pair.Value;
                if (IsNumeric(val))
                {
                    xml += "<" + key + ">" + val + "</" + key + ">";

                }
                else
                    xml += "<" + key + "><![CDATA[" + val + "]]></" + key + ">";
            }

            xml += "</xml>";
            return xml;
        }

        public static Dictionary<string, string> XmlToArray(string xml)
        {
            XDocument xdoc = XDocument.Parse(xml);
            var arr = new Dictionary<string, string>();
            foreach (var item in xdoc.Root.Elements())
            {
                arr.Add(item.Name.LocalName, item.Value);
            }

            return arr;
        }

        public static string GetRealIP()
        {
            string result = String.Empty;

            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (null == result || result == String.Empty || !RegexUtil.IsIPv4(result))
            {
                return "0.0.0.0";
            }

            return result;
        }

        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        }

        public static long ChangeToIpNum(string ip)
        {
            var ipaddr = IPAddress.Parse(ip);
            return ipaddr.Address;
            //string[] sitem = ip.Split('.');
            //if (sitem.Length != 4) return 0;
            //Byte[] item = new Byte[4];
            //for (int i = 0; i < sitem.Length; i++)
            //{
            //    item[i] = Byte.Parse(sitem[i]);
            //}
            //long ipNum = item[3];//	 ip|item[1]<<16|item[2]<<8|item[0];   
            //ipNum |= (long)item[2] << 8;
            //ipNum |= (long)item[1] << 16;
            //ipNum |= (long)item[0] << 24;
            //return ipNum;
        }
        public static string ChangeToIp(long ipNum)
        {
            IPAddress b = new IPAddress(ipNum);
            return b.ToString();
            //string[] arrip = b.ToString().Split('.');
            //string numtoip = "";
            //for (int i = arrip.Length - 1; i > -1; i--)
            //{
            //    if (i == arrip.Length - 1)
            //    {
            //        numtoip = arrip[i].ToString();
            //    }
            //    else
            //    {
            //        numtoip = numtoip + "." + arrip[i].ToString();
            //    }
            //}
            //return numtoip;
        }

        public static string RateFormat(double number)
        {

            if (number > 1024 * 1024)
            {
                return string.Format("{0:f2} G", number / 1024 / 1024);
            }
            else if (number > 1024)
            {
                return string.Format("{0:f2} M", number / 1024);
            }
            else
            {
                return number + " K";
            }
        }

        public static Dictionary<string, int> EnumToList(Enum em)
        {
            Dictionary<string, int> listItems = new Dictionary<string, int>();
            Array array = Enum.GetValues(em.GetType());
            foreach (int val in array)
            {
                listItems.Add(Enum.GetName(em.GetType(), val), val);
            }

            return listItems;
        }

        public static List<long> FormatIps(string ipStr)
        {
            var ips = new List<long>();
            //var s = ipStr.Split(new Char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var ipSection in ipStr.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var ipStrSplit = ipSection.Split(new Char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                var tmp = ipStrSplit[0].Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var ipPrix = string.Format("{0}.{1}.{2}.", tmp[0], tmp[1], tmp[2]);
                var ipStart = ipStrSplit[0].Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[3];
                var ipEnd = ipStrSplit[1].Split(new Char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[3];
                for (int i = int.Parse(ipStart); i <= int.Parse(ipEnd); i++)
                {
                    ips.Add(ChangeToIpNum(ipPrix + i));
                }
            }



            return ips;
        }
    }
}

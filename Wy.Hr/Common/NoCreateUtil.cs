using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Web;
using System.Net;

namespace Wy.Hr.Common
{
    public class NoCreateUtil
    {

        public static String CreateStaffNo(string maxNo)
        {
            String staffNo = "WY-";
            if(!string.IsNullOrEmpty(maxNo)){
                var str = maxNo.Substring(3);
                int no = Convert.ToInt32(str) + 1;
                staffNo += no.ToString().PadLeft(4, '0');
            }
            else{
                staffNo += "0001";
            }
            return staffNo;
        }

        public static String CreatePositionNo(string maxNo)
        {
            //  默认为001
            var positionNo = "001";
            if (!string.IsNullOrEmpty(maxNo))
            {
                int no = Convert.ToInt32(maxNo) + 1;
                positionNo = no.ToString().PadLeft(3, '0');
            }
            return positionNo;
        }

        public static String CreateQuitOrderNo()
        {
            String quitNo = "WY";
            quitNo += DateTime.Now.ToLocalTime().ToString("yyyyMMdd");
            quitNo += CommonUtil.CreateIntNoncestr(4);
            return quitNo;
        }

    }
}

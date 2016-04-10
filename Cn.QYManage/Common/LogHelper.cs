using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Cn.QYManage.Common
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LogFiledAttribute : System.Attribute
    {
        public string Name { get; set; }
        public object OriginalValue { get; set; }
        public object NewValue { get; set; }

        //public LogFiledAttribute(string name)
        //{
        //    this.Name = name;
        //}
    }

    public class LogHelper
    {
        Dictionary<PropertyInfo, LogFiledAttribute> fileds;
        object model;
        public LogHelper(object model)
        {
            this.model = model;
            fileds = new Dictionary<PropertyInfo, LogFiledAttribute>();
            Type t = model.GetType();
            var properties = t.GetProperties();
            foreach (var propertie in properties)
            {
                object[] logFileds = propertie.GetCustomAttributes(typeof(LogFiledAttribute), false);
                if (logFileds.Length > 0)
                {
                    var logFiled = (LogFiledAttribute)logFileds[0];
                    logFiled.OriginalValue = propertie.GetValue(model, null);
                    fileds.Add(propertie, logFiled);
                }
            }
        }

        public string GetChangeLog()
        {
            foreach (var filed in fileds)
            {
                filed.Value.NewValue = filed.Key.GetValue(model, null);
            }
            StringBuilder sb = new StringBuilder();
            foreach (var filed in fileds)
            {
                if (!object.Equals(filed.Value.OriginalValue, filed.Value.NewValue))
                {
                    sb.AppendLine(string.Format("<备注>{0}<原>{1}</原><现>{2}</现></备注> ", filed.Value.Name, filed.Value.OriginalValue, filed.Value.NewValue));
                }
            }

            return sb.ToString();
        }


    }
}
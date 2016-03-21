using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Net.Mail;
using System.Net;

namespace Wy.Hr.Common
{
    /// <summary>    
    /// 发送Email类    
    /// </summary>
    public class EmailHelper
    {
        #region 私有成员
        private static object lockHelper = new object();
        private string _FromEmail;
        private string _Subject;
        private string _Body;
        private string _SmtpServer;
        private string _SmtpPort = "25";
        private string _SmtpUserName;
        private string _SmtpPassword;
        private System.Web.Mail.MailFormat _Format = System.Web.Mail.MailFormat.Html;
        private System.Text.Encoding _Encoding = System.Text.Encoding.Default;
        #endregion
        #region 属性
        /// <summary>        
        /// 正文内容类型        
        /// </summary>        
        public System.Web.Mail.MailFormat Format { set { _Format = value; } }
        /// <summary>        
        /// 正文内容编码        
        /// </summary>        
        public System.Text.Encoding Encoding { set { _Encoding = value; } }
        /// <summary>        
        /// FromEmail 发送方地址(如test@163.com)         
        /// </summary>        
        public string FromEmail { set { _FromEmail = value; } }      
        /// <summary>        
        /// 主题       
        /// </summary>        
        public string Subject { set { _Subject = value; } }
        /// <summary>        
        /// 内容        
        /// </summary>        
        public string Body { set { _Body = value; } }
        /// <summary>        
        /// SmtpServer        
        /// </summary>        
        public string SmtpServer { set { _SmtpServer = value; } }
        /// <summary>        
        /// SmtpPort        
        /// </summary>        
        public string SmtpPort { set { _SmtpPort = value; } }
        /// <summary>        
        /// SmtpUserName        
        /// </summary>        
        public string SmtpUserName { set { _SmtpUserName = value; } }
        /// <summary>        
        /// SmtpPassword        
        /// </summary>        
        public string SmtpPassword { set { _SmtpPassword = value; } }

        public bool EnableSsl { get; set; }
        #endregion
        #region 构造器
        /// <summary>        
        /// 构造器        
        /// </summary>        
        public EmailHelper() { }
        #endregion
        #region Send
        /// <summary>        
        /// 发送EMAIL        
        /// </summary>        
        /// <example>        
        /// <code>        
        ///     Email _Email = new Email();        
        ///     _Email.FromEmail = "test@163.com";        
        ///     _Email.Subject = "&lt;div>aaaa&lt;/div>";        
        ///     _Email.Body = "aaaaaaaaaaaaa";        
        ///     _Email.SmtpServer = "smtp.163.com";        
        ///     _Email.SmtpUserName = "aaa";        
        ///     _Email.SmtpPassword = "aaa";        
        ///     _Email.Send("test@163.com");        
        /// </code>        
        /// </example>        
        /// <param name="toEmail">收信人 接收方地址</param>        
        /// <returns>成功否</returns>        
        public bool SmtpMailSend(string toEmail)
        {
            lock (lockHelper)
            {
                System.Web.Mail.MailMessage msg = new System.Web.Mail.MailMessage();
                try
                {
                    msg.From = _FromEmail;//发送方地址(如test@163.com)
                    msg.To = toEmail;//接收方地址                   
                    msg.BodyFormat = _Format;//正文内容类型    
                    msg.BodyEncoding = _Encoding;//正文内容编码      
                    msg.Subject = _Subject;//主题              
                    msg.Body = _Body;//内容                 
                    msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");//设置为需要用户验证           
                    if (!_SmtpPort.Equals("25")) msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", _SmtpPort);//设置端口     
                    msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", _SmtpUserName);//设置验证用户名                
                    msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", _SmtpPassword);//设置验证密码                
                    if (EnableSsl)
                    {
                        msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
                    }
                    System.Web.Mail.SmtpMail.SmtpServer = _SmtpServer;//邮件服务器地址(如smtp.163.com)                
                    System.Web.Mail.SmtpMail.Send(msg);//发送        
                    return true;
                }
                catch { }
                finally { }
            }
            return false;
        }
        /// <summary>        
        /// 发送EMAIL        
        /// </summary>        
        /// <param name="toEmail">Email</param>        
        /// <returns>是否成功</returns>        
        public bool CDOMessageSend(string toEmail)
        {
            lock (lockHelper)
            {
                CDO.Message objMail = new CDO.Message();
                try
                {
                    objMail.To = toEmail;
                    objMail.From = _FromEmail;
                    objMail.Subject = _Subject;
                    if (_Format.Equals(System.Web.Mail.MailFormat.Html))
                        objMail.HTMLBody = _Body;
                    else objMail.TextBody = _Body;
                    
                    if (!_SmtpPort.Equals("25")) objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"].Value = _SmtpPort;
                    //设置端口                   
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value = _SmtpServer;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendemailaddress"].Value = _FromEmail;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpuserreplyemailaddress"].Value = _FromEmail;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpaccountname"].Value = _SmtpUserName;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value = _SmtpUserName;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value = _SmtpPassword;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value = 2;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value = 1;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"].Value = EnableSsl;
                    objMail.Configuration.Fields.Update();
                    objMail.Send();
                    return true;
                }
                catch (Exception ex) 
                { 
                    //throw ex; 
                }
                finally { }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objMail);
                objMail = null;
            }
            return false;
        }
        /// <summary>        
        /// CDOMessageSend        
        /// </summary>        
        /// <param name="toEmail"></param>        
        /// <param name="sendusing"></param>        
        /// <returns></returns>        
        public bool CDOMessageSend(string toEmail, int sendusing)
        {
            lock (lockHelper)
            {
                CDO.Message objMail = new CDO.Message();
                try
                {
                    objMail.To = toEmail;
                    objMail.From = _FromEmail;
                    objMail.Subject = _Subject;
                    if (_Format.Equals(System.Web.Mail.MailFormat.Html))
                        objMail.HTMLBody = _Body;
                    else objMail.TextBody = _Body;
                    if (!_SmtpPort.Equals("25")) objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"].Value = _SmtpPort;
                    //设置端口                   
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"].Value = _SmtpServer;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"].Value = sendusing;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendemailaddress"].Value = _FromEmail;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpuserreplyemailaddress"].Value = _FromEmail;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpaccountname"].Value = _SmtpUserName;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"].Value = _SmtpUserName;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"].Value = _SmtpPassword;
                    objMail.Configuration.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"].Value = 1;
                    objMail.Configuration.Fields.Update();
                    objMail.Send();
                    return true;
                }
                catch { }
                finally
                {
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objMail);
                objMail = null;
            }
            return false;
        }
        /// <summary>        
        /// SmtpClientSend        
        /// </summary>        
        /// <param name="toEmail"></param>        
        /// <returns></returns>        
        public bool SmtpClientSend(string toEmail)
        {
            lock (lockHelper)
            {
                System.Net.Mail.MailMessage message = new MailMessage(_FromEmail, toEmail, _Subject, _Body);
                message.SubjectEncoding = _Encoding;
                message.BodyEncoding = _Encoding;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient(_SmtpServer);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_SmtpUserName, _SmtpPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    client.Port = Convert.ToInt32(_SmtpPort);
                }
                catch
                {
                    client.Port = 587;
                }
                client.EnableSsl = true;
                try
                {
                    client.Send(message);
                }
                catch
                {
                    return false;
                }
                return true;
            }
        }
        #endregion


    }

    
}

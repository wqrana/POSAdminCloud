using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSA_AdminPortal.Models
{
    public class LoginInfo
    {
        public LoginInfo(LoginModel model)
        {
            Authenticate(model);
        }

        public static RegistrationService.LoginInfo ClientLogonInfo
        {
            get
            {
                return (HttpContext.Current.Session["LoginInfo"] as RegistrationService.LoginInfo);
            }
            set
            {
                HttpContext.Current.Session["LoginInfo"] = value;
            }
        }

        public static RegistrationService.LoginInfo Authenticate(LoginModel model)
        {
            string url = null;

            url = ConfigurationManager.AppSettings["ServiceUrl"];
            /* Removed
            if (ConfigurationManager.AppSettings["liveServer"] != null && ConfigurationManager.AppSettings["liveServer"].ToString() == "1")
            {
                url = ConfigurationManager.AppSettings["liveServiceUrl"];
            }
            else
            {
               url = ConfigurationManager.AppSettings["devServiceUrl"];
               //url = "http://localhost:53679/Registration.svc?wsdl";
            }
             * */

            var client = new RegistrationService.Registration();

            if (!string.IsNullOrWhiteSpace(url))
            {
                client.Url = url;
            }

            var info = client.ClientLogonInfo(model.UserName, model.Password);//"development", "Dev!4400"

            if (info == null || !info.Authorized)
            {
                HttpContext.Current.Session.Abandon();

                return null;
            }
            //set it to reuse again
            ClientLogonInfo = info;

            if (model.RememberMe)
            {
                RememberMe(model);
            }
            else
            {
                UnRememberMe();
            }

            return info;
        }

        public static void LogOff()
        {
            HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// The remember me.
        /// </summary>
        private static void RememberMe(LoginModel model)
        {
            HttpContext.Current.Response.Cookies["userName"].Value = Encryption.Encrypt(model.UserName);
            HttpContext.Current.Response.Cookies["password"].Value = Encryption.Encrypt(model.Password);
            HttpContext.Current.Response.Cookies["userName"].Expires = DateTime.Today.AddMonths(1);
        }

        /// <summary>
        /// The un Remember me.
        /// </summary>
        private static void UnRememberMe()
        {
            HttpContext.Current.Response.Cookies["userName"].Expires = DateTime.Today.AddMonths(-1);
            HttpContext.Current.Response.Cookies["password"].Expires = DateTime.Today.AddMonths(-1);
        }
    }
}
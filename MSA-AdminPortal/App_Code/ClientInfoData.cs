using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MSA_AdminPortal;
using Repository.Helpers;
using Repository;

namespace MSA_AdminPortal
{
    public static class ClientInfoData
    {
        public static long GetClientID()
        {
            if (HttpContext.Current.Session["LoginInfo"] != null)
            {
                RegistrationService.LoginInfo info = (RegistrationService.LoginInfo)HttpContext.Current.Session["LoginInfo"];
                return info.Client.ClientId;
            }
            else
            {
                return 44;
            }
        }
        public static int GetCustomerID()
        {
            if (HttpContext.Current.Session["LoginInfo"] != null)
            {
                RegistrationService.LoginInfo info = (RegistrationService.LoginInfo)HttpContext.Current.Session["LoginInfo"];
                return (int)info.User.CustomerId;
            }
            else
            {
                return 0;
            }
        }
        public static IList<string> GetStates()
        {
            return FSS.FSS.stateList;
        }
        /// <summary>
        /// Create dynamic connection string
        /// </summary>
        /// <returns></returns>
        public static string getConectionString()
        {

            try
            {
                if (HttpContext.Current.Session["LoginInfo"] != null)
                {
                    RegistrationService.LoginInfo info = (RegistrationService.LoginInfo)HttpContext.Current.Session["LoginInfo"];

                    string serverName = info.Connection.ServerName;// "fuf15yfj5p.database.windows.net";
                    string initialCatalog = info.Connection.DatabaseName;// "FSS_POS_TEST_2";
                    string userId = info.Connection.DbUserName; // "Fss_Development";
                    string password = info.Connection.DbPassword; // "4400Developer!";
                    object[] strData = { serverName, initialCatalog, userId, password };

                    StringBuilder connStringsb = new StringBuilder();
                    connStringsb.AppendFormat("data source={0}; initial catalog={1}; persist security info=True; user id={2}; password={3}; MultipleActiveResultSets=True; App=EntityFramework", strData);
                    return connStringsb.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "getConectionString", "Error : get connection string :: " + ex.Message, CommonClasses.getCustomerID(), "getConectionString");
                return "";
            }


        }

        /// <summary>
        /// Create dynamic connection string from clientID
        /// </summary>
        /// <returns></returns>
        public static string getConectionStringByClientID(long clientID)
        {
            //connectionString="data source=fuf15yfj5p.database.windows.net;initial catalog=FSS_POS_TEST_2;persist security info=True;user id=Fss_Development;password=4400Developer!;"

            try
            {
                if (clientID != -1)
                {
                    var client = new RegistrationService.Registration();

                    RegistrationService.ClientConnectionInfo clientConnectionInfo = client.GetConnStringByClientID(clientID, true);

                    string serverName = clientConnectionInfo.DbConnectInfo.ServerName;// "fuf15yfj5p.database.windows.net";
                    string initialCatalog = clientConnectionInfo.DbConnectInfo.DatabaseName;// "FSS_POS_TEST_2";
                    string userId = clientConnectionInfo.DbConnectInfo.DbUserName; // "Fss_Development";
                    string password = clientConnectionInfo.DbConnectInfo.DbPassword; // "4400Developer!";
                    object[] strData = { serverName, initialCatalog, userId, password };

                    //string tempValue = "data source={0}; initial catalog={1}; persist security info=True user id={2}; password={3}";
                    StringBuilder connStringsb = new StringBuilder();
                    connStringsb.AppendFormat("data source={0}; initial catalog={1}; persist security info=True; user id={2}; password={3}; MultipleActiveResultSets=True; App=EntityFramework", strData);
                    return connStringsb.ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "getConectionStringByClientID", "Error : get connection string :: " + ex.Message, CommonClasses.getCustomerID(), "getConectionStringByClientID");
                return "";
            }

        }

        public static Dictionary<int, string> getModulePermissionsList()
        {
            if (HttpContext.Current.Session["PermissionList"] != null)
            {
                return (Dictionary<int, string>)(HttpContext.Current.Session["PermissionList"]);
            }
            else
            {
                if (HttpContext.Current.Session["LoginInfo"] != null)
                {
                    RegistrationService.LoginInfo info = (RegistrationService.LoginInfo)HttpContext.Current.Session["LoginInfo"];
                    var DictData = info.SecurityInfo.ModulePermissions;
                    Dictionary<int, string> newDict = new Dictionary<int, string>();
                    if (DictData != null)
                    {
                        foreach (var item in DictData)
                        {
                            newDict.Add(item.Key, item.Value);
                        }
                    }

                    HttpContext.Current.Session["PermissionList"] = newDict;
                    return (Dictionary<int, string>)(HttpContext.Current.Session["PermissionList"]);
                }
                else
                {
                    return new Dictionary<int, string>();
                }
            }
        }

        public static bool GetIsPrimary()
        {
            if (HttpContext.Current.Session["LoginInfo"] != null)
            {
                var info = (RegistrationService.LoginInfo)HttpContext.Current.Session["LoginInfo"];
                return info.User.IsPrimary;
            }
            else
            {
                return false;
            }
        }
    }
}
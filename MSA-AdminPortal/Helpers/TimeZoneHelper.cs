using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Repository.Helpers;

namespace MSA_AdminPortal.Helpers
{
    public static class TimeZoneHelper
    {
        /// <summary>
        /// Get User Current Time with with user Offset value
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset GetUserCurrentTimeWithOffsetValue(string UserTimeZone)
        {
            TimeSpan clientTimeZone = TimeSpan.Parse(UserTimeZone);
            DateTime utcDateTime = DateTime.UtcNow.Add(clientTimeZone);
            DateTime currentTime = new DateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, utcDateTime.Hour, utcDateTime.Minute, utcDateTime.Second, utcDateTime.Millisecond);
            DateTimeOffset dateTimeOffset = new DateTimeOffset(currentTime, clientTimeZone);
            return dateTimeOffset;
        }

        public static DateTime GetClientTimeZoneLocalDateTime()
            
        {
            DateTime serverDateTime = DateTime.Now;
            DateTime localClientDateTime;
            long clientId = ClientInfoData.GetClientID();
            var service = new RegistrationService.Registration();
            string clientTimeZone = service.ClientTimeZoneID(clientId, true);

            TimeZoneInfo sourceTimeZone = TimeZoneInfo.Local;
            TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(clientTimeZone);
            localClientDateTime = TimeZoneInfo.ConvertTime(serverDateTime, sourceTimeZone, destinationTimeZone);

            return localClientDateTime;

        }


        /// <summary>
        /// Public method to get time zone ID by ClientID
        /// </summary>
        /// <param name="ClientID"></param>
        /// <returns></returns>
        public static string GetClientTimeZoneID(Int64 ClientID) 
        {
            try
            {
                string url = null;

                url = ConfigurationManager.AppSettings["ServiceUrl"];

                //if (ConfigurationManager.AppSettings["liveServer"] != null && ConfigurationManager.AppSettings["liveServer"].ToString() == "1")
                //{
                //    url = ConfigurationManager.AppSettings["liveServiceUrl"];
                //}
                //else
                //{
                //    url = ConfigurationManager.AppSettings["devServiceUrl"];
                //    //url = "http://localhost:53679/Registration.svc?wsdl";
                //}

                var client = new RegistrationService.Registration();

                if (!string.IsNullOrWhiteSpace(url))
                {
                    client.Url = url;
                }

                string TimeZoneID = client.ClientTimeZoneID(ClientID, true);

                if (string.IsNullOrEmpty(TimeZoneID))
                {
                    return "";
                }

                return TimeZoneID;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TimeZoneHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetClientTimeZoneID");
                return "";
            }
        }

        public static DateTime ConvertDateTimeToClientTime(DateTime TimeToConvert, Int64 ClientID)
        {
            try
            {
                string TimeZoneID = string.Empty;
                if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["TimeZoneID"] != null)
                {
                    TimeZoneID = System.Web.HttpContext.Current.Session["TimeZoneID"].ToString();
                }
                else
                {
                    TimeZoneID = GetClientTimeZoneID(ClientID);
                    HttpContext.Current.Session["TimeZoneID"] = TimeZoneID;
                }

                if (!string.IsNullOrEmpty(TimeZoneID))
                {
                    TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
                    DateTime newTime = TimeZoneInfo.ConvertTimeFromUtc(TimeToConvert, timezone);
                    return newTime;
                }
                else
                    return TimeToConvert;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TimeZoneHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ConvertDateTimeToClientTime");
                return TimeToConvert;
            }
        }

        public static string ConvertLongDateToClientTime(DateTime TimeToConvert, Int64 ClientID)
        {
            DateTime convertedDateTime = ConvertDateTimeToClientTime(TimeToConvert, ClientID);
            return convertedDateTime.ToString("MMMM dd, yyyy");
        }

        public static string ConvertTimeToClientTime(DateTime TimeToConvert, Int64 ClientID)
        {
            DateTime convertedDateTime = ConvertDateTimeToClientTime(TimeToConvert, ClientID);
            return convertedDateTime.ToString("hh:mm tt");
        }


    }
}
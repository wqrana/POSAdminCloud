using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace Repository.Helpers
{
    public class TimeZoneSettings
    {
        /// <summary>
        /// Variables used for Time Zone Settings
        /// </summary>
        private static TimeZoneSettings objTimeZone;
        private string timezone = string.Empty;

        /// <summary>
        /// Default Constructor
        /// </summary>
        private TimeZoneSettings()
        {

            this.timezone = ConfigurationManager.AppSettings["localtimezone"].ToString();

        }

        /// <summary>
        /// Static method which returns the Singleton class object
        /// </summary>
        public static TimeZoneSettings Instance
        {
            get
            {
                if (objTimeZone == null)
                {
                    objTimeZone = new TimeZoneSettings();
                }
                return objTimeZone;
            }
        }

        /// <summary>
        /// This method returns the local Date Time by using Local Time Zone Standard Name
        /// </summary>
        /// <returns></returns>
        public DateTime GetLocalTime()
        {
            DateTime timeUtc = DateTime.UtcNow;
            DateTime localTime = DateTime.UtcNow;
            try
            {
                TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                localTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, estZone);
            }
            catch (TimeZoneNotFoundException)
            {

            }
            catch (InvalidTimeZoneException)
            {

            }
            return localTime;
        }

    }
}

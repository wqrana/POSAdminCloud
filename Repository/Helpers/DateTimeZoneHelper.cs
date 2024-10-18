using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Repository.Helpers
{
    public static class DateTimeZoneHelper
    {
        /// <summary>
        /// Get User Current Time with with user Offset value
        /// </summary>
        /// <returns></returns>
        public static DateTimeOffset GetUserCurrentTimeWithOffSetValue(string UserTimeZone)
        {
            TimeSpan clientTimeZone = TimeSpan.Parse(UserTimeZone);
            DateTime utcDateTime = DateTime.UtcNow.Add(clientTimeZone);
            DateTime currentTime = new DateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, utcDateTime.Hour, utcDateTime.Minute, utcDateTime.Second, utcDateTime.Millisecond);
            DateTimeOffset dateTimeOffset = new DateTimeOffset(currentTime, clientTimeZone);
            return dateTimeOffset;
        }

    }
}

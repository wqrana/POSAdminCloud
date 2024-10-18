using AdminPortalModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AdminPortalModels.ViewModels
{
    class PreorderModels
    {

    }


    public class WeblunchCalendar : ErrorModel
    {

        public Int32 WebCalID { get; set; }

        public string CalendarName { get; set; }
        public string CalendarType { get; set; }
        public Int32 DistrictID { get; set; }
        public string fullSchoolsList { get; set; }

        public string SchoolsListID
        {
            get
            {
                return "schoolsList" + WebCalID;

            }

            set
            {
                string temp = value;

            }
        }

        public Int32 CutOffValue { get; set; }
        public Int32 CutOffType { get; set; }
        public Int32 CutOffSelection { get; set; }
        public string SameDayOrderTime { get; set; }
        public Int32 OrderingOption { get; set; }
        public string CalendarTypeName { get; set; }

        public List<PreorderSchool> AssignedSchoolsList { get; set; }


    }

    public class WeblunchCalendarCreateModel : WeblunchCalendar
    {
        public override string Title { get { return "Create New Calendar"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new calendar."; } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }

        public List<CalType> CalTypeList
        {
            get
            {
                List<CalType> returnList = new List<CalType>();
                returnList.Add(new CalType { Id = 0, Type = "...Select..." });
                returnList.Add(new CalType { Id = 1, Type = "Breakfast" });
                returnList.Add(new CalType { Id = 2, Type = "Lunch" });
                returnList.Add(new CalType { Id = 3, Type = "Dinner" });
                returnList.Add(new CalType { Id = 4, Type = "Snack" });
                returnList.Add(new CalType { Id = 5, Type = "Combined" });
                returnList.Add(new CalType { Id = 6, Type = "Other" });
                return returnList;

            }

        }

    }

    public class WeblunchCalendarUpdateModel : WeblunchCalendar
    {
        public override string Title { get { return string.Format("Edit: {0}", CalendarName.Trim()); } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} calendar.", CalendarName); } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save Changes");
            }
        }
    }



    public class PreorderSchool
    {
        public string schoolID { get; set; }
        public string selectedstring { get; set; }
        public string schoolName { get; set; }
        public bool isSelected { get; set; }

    }



    public class CalType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class WebCalItem
    {
        public int menus_id { get; set; }
        public DateTime calItemDate { get; set; }
        public int webCalID { get; set; }

        public int mealType { get; set; }

        public int useOrder { get; set; }

    }

    public class WebCalItemStatus
    {
        public int WebCalID { get; set; }
        public DateTime SelectedDate { get; set; }
        public string Status { get; set; }

    }

    public class ItemScheduler
    {
        public int WebCalID { get; set; }
        public DateTime SourceDate { get; set; }
        public string dateList { get; set; }

    }

    public class OrderingOptionsModel : WeblunchCalendar
    {
        public string useSameDayOrdering { get; set; }
        public bool noneChecked
        {
            get
            {
                if (OrderingOption == 0) return true;
                else return false;

            }
            set
            {
                bool temp = value;
            }
        }
        public bool cutoffsettings
        {
            get
            {
                if (OrderingOption == 1) return true;
                else return false;

            }
            set
            {
                bool temp = value;
            }
        }
        public bool sameDaySettings
        {
            get
            {
                if (OrderingOption == 2) return true;
                else return false;

            }
            set
            {
                bool temp = value;
            }
        }

        public string sameDayHours { get; set; }
        public string sameDayMinutes { get; set; }
        public string sameDayAmPm { get; set; }
        public string CalendarType { get; set; }

        public ICollection<DataItem> SinglesDayList { get; set; }

        public ICollection<DataItem> MultipleDayList { get; set; }


        public ICollection<DataItemStr> HousrList { get; set; }
        public ICollection<DataItemStr> MinutesList { get; set; }
        public ICollection<DataItemStr> AMPMList { get; set; }


    }

    public class DataItem
    {
        public Int64 value { get; set; }
        public string data { get; set; }

    }

    public class DataItemStr
    {
        public string value { get; set; }
        public string data { get; set; }

    }

    public class CalSettingsData
    {
        public string useFiveDayWeekCutOff { get; set; }
        public DateTime CutOffDate { get; set; }
        public string overrideCutOff { get; set; }
        public int Cutoffvalue { get; set; }
        public bool hasCutOffvalue
        {
            get
            {
                return Cutoffvalue != 0;
            }
        }
        public bool isOverriddentCutOff
        {
            get
            {
                return overrideCutOff != "" && overrideCutOff != "0";
            }
        }

        public bool orderingClosed { get; set; }


    }

    public class OverrideCutOffData
    {
        public int cutOffType { get; set; }
        public int cutOffValue { get; set; }
        public int pWebcalid { get; set; }

        public DateTime pDate { get; set; }


    }

    public class SearchModel
    {
        public int CategoryID { get; set; }
        public string searchStr { get; set; }

        public int searchType { get; set; }



    }

    //public class TimeZoneSettings
    //{
    //    /// <summary>
    //    /// Variables used for Time Zone Settings
    //    /// </summary>
    //    private static TimeZoneSettings objTimeZone;
    //    private string timezone = string.Empty;

    //    /// <summary>
    //    /// Default Constructor
    //    /// </summary>
    //    private TimeZoneSettings()
    //    {
    //        this.timezone = "Eastern Standard Time";
    //    }

    //    /// <summary>
    //    /// Static method which returns the Singleton class object
    //    /// </summary>
    //    public static TimeZoneSettings Instance
    //    {
    //        get
    //        {
    //            if (objTimeZone == null)
    //            {
    //                objTimeZone = new TimeZoneSettings();
    //            }
    //            return objTimeZone;
    //        }
    //    }

    //    /// <summary>
    //    /// This method returns the local Date Time by using Local Time Zone Standard Name
    //    /// </summary>
    //    /// <returns></returns>
    //    public DateTime GetLocalTime()
    //    {
    //        DateTime timeUtc = DateTime.UtcNow;
    //        DateTime localTime = DateTime.UtcNow;
    //        try
    //        {
    //            TimeZoneInfo estZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
    //            localTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, estZone);
    //        }
    //        catch (TimeZoneNotFoundException)
    //        {

    //        }
    //        catch (InvalidTimeZoneException)
    //        {

    //        }
    //        return localTime;
    //    }

    //}

}

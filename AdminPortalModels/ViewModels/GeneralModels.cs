using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{

    public class Gender
    {

        public string value { get; set; }
        public string data { get; set; }
        public Gender(string val, string dat)
        {
            value = val;
            data = dat;
        }
    }
    public class Grade
    {

        public Int64 value { get; set; }
        public string data { get; set; }
    }

    public class Language
    {
        public Int64 value { get; set; }
        public string data { get; set; }
    }

    public class Ethnicity
    {
        public Int64 value { get; set; }
        public string data { get; set; }
    }

    public class State
    {
        public string value { get; set; }
        public string data { get; set; }
    }


    public class DistrictItem
    {
        public Int64 value { get; set; }
        public string data { get; set; }
    }

    public class SchoolItem
    {
        public string data { get; set; }
        public Int64 value { get; set; }
        public string stvalue { get; set; }
        public Int64 DistrictID { get; set; }
        public string DistrictName { get; set; }

    }

    
    public class HomeRoomModel
    {
        public Int64 value { get; set; }
        public string data { get; set; }
    }
    
}

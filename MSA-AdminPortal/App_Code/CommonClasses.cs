using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Repository.edmx;
using System.Configuration;

namespace MSA_AdminPortal.App_Code
{


    public class DistricAndSchoolsCount
    {
        public District dist { get; set; }
        public string DistrictName { get; set; }
        public string Phone1 { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public int SchoolCount { get; set; }

    }
    public class SchoolAndPOSCount
    {
        public long ID { get; set; }
        public string SchoolName { get; set; }
        public string DistrictName { get; set; }
        public Int32 POSCount { get; set; }
        public string POSCountStr { get; set; }
        public string idstr { get; set; }
    }

}
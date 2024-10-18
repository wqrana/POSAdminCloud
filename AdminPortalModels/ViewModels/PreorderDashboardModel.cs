using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.Models;


namespace AdminPortalModels.ViewModels
{
  public class PreorderDashboardModel
    {
        public int PeriodTypeID { get; set; }
        public IEnumerable<PeriodType> PeriodTypeList { get; set; }

        public IEnumerable<CurrentPreorder> CurrentPreorderStatList { get; set; }

        public IEnumerable<AvgInPreorder> AvgInPreorderStatList { get; set; }

        public IEnumerable<TopSellingItem> TopSellingItemStatList { get; set; }
                
    }

    public class PeriodType{

        public int id { get; set; }
        public string name { get; set; }
    }
    public class CurrentPreorder
    {
        public int PeriodTypeID { get; set; }
        public string PeriodTypeName { get; set; }
        public int openCount { get; set; }
        public int closedCount { get; set; }


    }

    public class AvgInPreorder
    {
        public int PeriodTypeID { get; set; }
        public string PeriodTypeName { get; set; }
        public int AvgCount { get; set; }

    }

    public class TopSellingItem
    {
        public string ItemName { get; set; }
        public Nullable<int> qtySold { get; set; }

    }

}

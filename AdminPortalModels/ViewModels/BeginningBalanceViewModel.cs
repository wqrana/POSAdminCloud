using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{
    public class BeginningBalanceViewModel
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public string CustomerName { get; set; }
        public string Grade { get; set; }
        public double? MealPlan { get; set; }
        public double? AlaCartePlan { get; set; }

        public double? PrevMealPlanBalance { get; set; }
        public double? PrevAlaCartePlanBalance { get; set; }
        public double? Balance { get; set; }
    }

    public class BeginningBalanceFilters
    {
        public string SearchString { get; set; }
        public string SchoolFilter { get; set; }
        public string GradeFilter { get; set; }
        public string HomeRoomFilter { get; set; }
        public string DistrictFilter { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }
    }

    public class GraduateSeniorsDistinctHomeRoomViewModel
    {
        public long? HomeRoomId { get; set; }
        public string HomeRoomName { get; set; }
    }

    public class BeginningBalancePaymentData
    {
        public int CustomerId { get; set; }
        public double MPAmount { get; set; }
        public double ACAmount { get; set; }
    }
}

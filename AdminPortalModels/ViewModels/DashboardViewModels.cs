using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{
    public class DashboardViewModels
    {
    }

    public class RSSFeedModels
    {
        public SyndicationFeed BlogFeed { get; set; }
    }

        public class PaymentsGraph
        {
            public string PaymentDate { get; set; }
            public Nullable<double> TotalPayment { get; set; }

        }

        public class SalesDashboardGraph
        {
            public string PaymentDate { get; set; }
            public Nullable<double> TotalPayment { get; set; }

        }

        public class TotalSalesForDashboard
        {
            public Nullable<double> TodaySales { get; set; }
            public double YesterdaySales { get; set; }
            public double LastWeekSales { get; set; }
        }


        public class ParticipationPercentageDashboard
        {

            public double TodayParticipation { get; set; }
            public double YesterdayParticipation { get; set; }
            public double LastWeekParticipation { get; set; }
        }

        public class AccountInfoDashboard
        {
            public int CountOfPositiveAccounts { get; set; }
            public double PositiveAmount { get; set; }
            public int CountOfNegativeAccounts { get; set; }
            public double NegativeAmount { get; set; }
            public int CountOfZeroAccounts { get; set; }
            public double ZeroAmount { get; set; }
        }

        public class POSDashboardVM
        {
            public string POS_Name { get; set; }
            public string POS_Open_Cashier { get; set; }
            public string POS_Open_Session { get; set; }
            public Nullable<System.DateTime> POS_Open_Session_Date { get; set; }

        }
    
}

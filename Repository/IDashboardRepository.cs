using AdminPortalModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IDashboardRepository : IDisposable
    {
        IEnumerable<PaymentsGraph> GetPaymentsForGraph(System.DateTime fromDate, System.DateTime toDate, long clientID);
        IEnumerable<SalesDashboardGraph> GetSalesForGraph(System.DateTime fromDate, System.DateTime toDate, long clientID);
        TotalSalesForDashboard GetTotalSalesForDashboard(long clientID);
        ParticipationPercentageDashboard GetParticipationPercentageForDashboard(long clientID);
        AccountInfoDashboard GetAccountInfoForDashboard(long clientID);
        IEnumerable<POSDashboardVM> GetDashboardOpenCashierSession(Nullable<long> clientID, Nullable<long> schoolID);

        Task<AccountInfoDashboard> GetAccountInfoForDashboardAsync(long clientID);
    }
}

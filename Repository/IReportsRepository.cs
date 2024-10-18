using AdminPortalModels.ViewModels;
using Repository.edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IReportsRepository : IDisposable
    {
        IEnumerable<CustomersList> getReportsCustomersList(long clientID);
        IEnumerable<DepositTicket> getDepositTicket(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<DailyCashier> getDailyCashier(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<CustomerRoster> getCustomerRoster(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<CashierPayments> getPaymentsByCashier(long clientID, int reportID, ReportsFilters rptFilters);
        
        //for cafeteria report
        IEnumerable<DetailCafeCategory> getDetailCafeCategories(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<SalesTaxOnOrder> getSalesTaxOnOrders(long clientID);
        IEnumerable<DetailCafeCashier> getDetailCafeCashiers(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<DetailCafeSummary> getDetailCafeSummaries(long clientID, int reportID, ReportsFilters rptFilters);

        IEnumerable<AccountBalanceInfo_Result> getAccountInfoes(long clientID, ReportsFilters rptFilters);
        IEnumerable<DetailCafeNetChange> getDetailCafeNetChanges(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<CustomerBalanceData_Result> getCustomerBalanceData(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<SoldItems> getSoldItems(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<VoidsD> getVoidItems(long clientID, int reportID, ReportsFilters rptFilters);
         
        IEnumerable<Reporting_CustomerRosterSummary_Result> getCustomerRosterSummary(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<Reporting_StatementOrdersSummary_Result> getStatementOrdersSummary(long clientID, int reportID, ReportsFilters rptFilters);

        IEnumerable<Reporting_StatementDetailedOrders_Result> getStatementDetailedOrders(long clientID, int reportID, ReportsFilters rptFilters);

        IEnumerable<Reporting_StatementDetailedItems_Result> getStatementDetailedItems(long clientID, int reportID, ReportsFilters rptFilters);
        List<string> getSortingList(string sortingList);

        IEnumerable<Reporting_SoldItemsByCategory_Result> getSoldItemsByCategory(long clientID, int reportID, ReportsFilters rptFilters);
        IEnumerable<Reporting_CustomerBySoldItems_Result> getCustomerBySoldItems(long clientID, int reportID, ReportsFilters rptFilters);

        IEnumerable<Reporting_DetailedTransactions_Result> getDetailedTransactions(long clientID, int reportID, ReportsFilters rptFilters);
    }
}

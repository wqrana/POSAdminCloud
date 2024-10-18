using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.edmx;
using AdminPortalModels.ViewModels;
using Repository.Helpers;

namespace Repository
{
    public class DashboardRepository:IDashboardRepository,IDisposable
    {
           private PortalContext context;

        public DashboardRepository(PortalContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// This function returns List of payments for dashboard graph 
        /// </summary>
        /// <param name="fromDare"></param>
        /// <param name="toDate"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<PaymentsGraph> GetPaymentsForGraph(System.DateTime fromDate, System.DateTime toDate, long clientID)
        {
            try
            {
                return this.context.Admin_Dashboard_Payments(fromDate, toDate, clientID).Select(p => new PaymentsGraph
                {
                    PaymentDate = p.PaymentDate,
                    TotalPayment = p.TotalPayment

                }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DashboardRepository", "Error : " + ex.Message, clientID.ToString(), "GetPaymentsForGraph");
                return null;
            }
        }

        /// <summary>
        /// This function returns List of Sales for dashboard graph 
        /// </summary>
        /// <param name="fromDare"></param>
        /// <param name="toDate"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<SalesDashboardGraph> GetSalesForGraph(System.DateTime fromDate, System.DateTime toDate, long clientID)
        {
            try
            {
                return this.context.Admin_Dashboard_Sales(fromDate, toDate, clientID).Select(s => new SalesDashboardGraph
                {
                    PaymentDate = s.SaleDate,
                    TotalPayment = s.TotalSales

                }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DashboardRepository", "Error : " + ex.Message, clientID.ToString(), "GetSalesForGraph");
                return null;
            }
        }

        /// <summary>
        /// This function returns Total Sales for dashboard
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public TotalSalesForDashboard GetTotalSalesForDashboard(long clientID)
        {
            try
            {
                return this.context.Admin_Dashboard_TotalSales(clientID).Select(ts => new TotalSalesForDashboard
                {
                    TodaySales = ts.TodaySales,
                    YesterdaySales = ts.YesterdaySales,
                    LastWeekSales = ts.LastWeekSales

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DashboardRepository", "Error : " + ex.Message, clientID.ToString(), "GetTotalSalesForDashboard");
                return null;
            }
        }

        /// <summary>
        /// This function Participation Percentage for dashboard
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public ParticipationPercentageDashboard GetParticipationPercentageForDashboard(long clientID)
        {
            try
            {
                return this.context.Admin_Dashboard_Participation_Percentage(clientID).Select(pp => new ParticipationPercentageDashboard
                {
                    LastWeekParticipation = pp.LastWeekParticipation,
                    TodayParticipation = pp.TodayParticipation,
                    YesterdayParticipation = pp.YesterdayParticipation
                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DashboardRepository", "Error : " + ex.Message, clientID.ToString(), "GetParticipationPercentageForDashboard");
                return null;
            }
        }

        /// <summary>
        /// This function Account information for dashboard
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        async public Task<AccountInfoDashboard> GetAccountInfoForDashboardAsync(long clientID)
        {
            try
            {
                return this.context.Admin_Dashboard_AccountInformation(clientID).Select(ai => new AccountInfoDashboard
                {
                    CountOfNegativeAccounts = ai.CountOfNegativeAccounts,
                    CountOfPositiveAccounts = ai.CountOfPositiveAccounts,
                    NegativeAmount = ai.NegativeAmount,
                    PositiveAmount = ai.PositiveAmount,
                    CountOfZeroAccounts = ai.CountOfZeroAccounts,
                    ZeroAmount = ai.ZeroAmount

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DashboardRepository", "Error : " + ex.Message, clientID.ToString(), "GetAccountInfoForDashboard");
                return null;
            }
        }


        public AccountInfoDashboard GetAccountInfoForDashboard(long clientID)
        {
            try
            {
                return this.context.Admin_Dashboard_AccountInformation(clientID).Select(ai => new AccountInfoDashboard
                {
                    CountOfNegativeAccounts = ai.CountOfNegativeAccounts,
                    CountOfPositiveAccounts = ai.CountOfPositiveAccounts,
                    NegativeAmount = ai.NegativeAmount,
                    PositiveAmount = ai.PositiveAmount,
                    CountOfZeroAccounts = ai.CountOfZeroAccounts,
                    ZeroAmount = ai.ZeroAmount

                }).FirstOrDefault();

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DashboardRepository", "Error : " + ex.Message, clientID.ToString(), "GetAccountInfoForDashboard");
                return null;
            }
        }

        /// <summary>
        /// This function Open Cashier Session for dashboard
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<POSDashboardVM> GetDashboardOpenCashierSession(Nullable<long> ClientID, Nullable<long> SchoolID)
        {
            try
            {
                return this.context.Admin_POS_List(ClientID, null).Select(p1 => new POSDashboardVM { POS_Name = p1.POS_Name, POS_Open_Cashier = p1.POS_Open_Cashier, POS_Open_Session_Date = p1.POS_Open_Session_Date, POS_Open_Session = p1.POS_Open_Session }).AsEnumerable<POSDashboardVM>();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DashboardRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDashboardPOSbySchoolID");
                return null;
            }
        }

        /// <summary>
        /// This function disposes all the memory occupied by this object
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

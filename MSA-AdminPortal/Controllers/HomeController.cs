using AdminPortalModels.ViewModels;
using Repository;
using Repository.Helpers;
using MSA_AdminPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

using System.Xml;
using System.ServiceModel.Syndication;
using System.Configuration;
using System.Threading.Tasks;



namespace MSA_AdminPortal.Controllers
{
    public class HomeController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        private IDashboardRepository dashboardRepository;
        

        /// <summary>
        /// Constructer for class
        /// </summary>
        public HomeController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
            this.dashboardRepository = unitOfWork.dashboardRepository;
        }

        /// <summary>
        /// To get RSS FEED
        /// </summary>
        /// <returns>List of RSS FEEDs</returns>
        [MSA_Authorize]
        public ActionResult Index()
        {
            try
            {

           
            var rssModel = new AdminPortalModels.ViewModels.RSSFeedModels();

            var feed = ConfigurationManager.AppSettings["RSSLink"].ToString(); 

            using (XmlReader reader = XmlReader.Create(feed))
            {
                SyndicationFeed rssData = SyndicationFeed.Load(reader);
                rssModel.BlogFeed = rssData;

                return View(rssModel);

              
            }
            }
            catch (Exception ex)
            {

                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, "", "Index");
                return View();
            }

        }

     
        /// <summary>
        /// Populate Payment Graph
        /// </summary>
        /// <returns>List of Payments in form of Json</returns>
        [HttpGet]
        public JsonResult GetPayments()
        {
            var clientID = Convert.ToInt64(ClientInfoData.GetClientID());
            try
            {
            
            DateTime fromDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
            fromDate = fromDate.AddDays(-15);
            DateTime toDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));


            IEnumerable<PaymentsGraph> list = dashboardRepository.GetPaymentsForGraph(fromDate, toDate, clientID);
           
            var json = JsonConvert.SerializeObject(list);
           
            return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, clientID.ToString(), "GetPayments");
                return Json(new { result = "-1" });
            }
           
        }

        /// <summary>
        /// Populate Sales Graph
        /// </summary>
        /// <returns>List of Sales in form of Json</returns>
        [HttpGet]
        public JsonResult GetSales()
        {
            var clientID = Convert.ToInt64(ClientInfoData.GetClientID());
            try
            {

                DateTime fromDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                fromDate = fromDate.AddDays(-15);
                DateTime toDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));


                IEnumerable<SalesDashboardGraph> list = dashboardRepository.GetSalesForGraph(fromDate, toDate, clientID);

                var json = JsonConvert.SerializeObject(list);

                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, clientID.ToString(), "GetSales");
                return Json(new { result = "-1" });
            }

        }

        /// <summary>
        /// Get Total Sales
        /// </summary>
        /// <returns>Total Sales in form of Json</returns>
        [HttpGet]
        public JsonResult GetTotalSales()
        {
            var clientID = Convert.ToInt64(ClientInfoData.GetClientID());
            try
            {

                TotalSalesForDashboard totalSales = dashboardRepository.GetTotalSalesForDashboard(clientID);

                return Json(new 
                { 
                    todaySales = totalSales.TodaySales,
                    yesterdatSales = totalSales.YesterdaySales,
                    lastWeekSales = totalSales.LastWeekSales
                }, 
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, clientID.ToString(), "GetTotalSales");
                return Json(new { result = "-1" });
            }

        }

        /// <summary>
        /// Get Participation Percentage
        /// </summary>
        /// <returns>Participation Percentage in form of Json</returns>
        [HttpGet]
        public JsonResult GetParticipationPercentage()
        {
            var clientID = Convert.ToInt64(ClientInfoData.GetClientID());
            try
            {

                ParticipationPercentageDashboard participationPercentage = dashboardRepository.GetParticipationPercentageForDashboard(clientID);


                return Json(new
                {
                    todayParticipation = participationPercentage.TodayParticipation,
                    yesterdayParticipation = participationPercentage.YesterdayParticipation,
                    lastWeekParticipation = participationPercentage.LastWeekParticipation
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, clientID.ToString(), "GetParticipationPercentage");
                return Json(new { result = "-1" });
            }

        }


        /// <summary>
        /// Get Account Information
        /// </summary>
        /// <returns>Account information in form of Json</returns>
        [HttpGet]
        public JsonResult GetAccountInfo()
        {
            var clientID = Convert.ToInt64(ClientInfoData.GetClientID());
            try
            {

                AccountInfoDashboard accountInfo = dashboardRepository.GetAccountInfoForDashboard(clientID);

                var json = JsonConvert.SerializeObject(accountInfo);

                return Json(new
                {
                    countOfNegativeAccounts = accountInfo.CountOfNegativeAccounts,
                    negativeAmount = accountInfo.NegativeAmount,
                    countOfPositiveAccounts = accountInfo.CountOfPositiveAccounts,
                    positiveAmount = accountInfo.PositiveAmount,
                    countOfZeroAccounts = accountInfo.CountOfZeroAccounts,
                    zeroAmount = accountInfo.ZeroAmount
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, clientID.ToString(), "GetAccountInfo");
                return Json(new { result = "-1" });
            }

        }

        [HttpGet]
        public JsonResult GetCasheirSessions()
        {
       
            long ClientID = ClientInfoData.GetClientID();
            try
            {

                var POSDashboardVM = dashboardRepository.GetDashboardOpenCashierSession(ClientID, null).Where(p => p.POS_Open_Session == "Open").ToList();

                var json = JsonConvert.SerializeObject(POSDashboardVM);

                return Json(json, JsonRequestBehavior.AllowGet);
             
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, ClientID.ToString(), "GetCasheirSessions");
                return Json(new { result = "-1" });
            }

        }

        async public Task<JsonResult> CombineResult()
        {

            long ClientID = ClientInfoData.GetClientID();
            try
            {
                Task<AccountInfoDashboard> accountInfo = dashboardRepository.GetAccountInfoForDashboardAsync(ClientID); //await Task.Run(() => dashboardRepository.GetAccountInfoForDashboard(ClientID));
                var POSDashboardVM = dashboardRepository.GetDashboardOpenCashierSession(ClientID, null).Where(p => p.POS_Open_Session == "Open").ToList();
                ParticipationPercentageDashboard participationPercentage = dashboardRepository.GetParticipationPercentageForDashboard(ClientID);
                DateTime fromDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                fromDate = fromDate.AddDays(-15);
                DateTime toDate = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));


                IEnumerable<PaymentsGraph> listpayment = dashboardRepository.GetPaymentsForGraph(fromDate, toDate, ClientID);
                IEnumerable<SalesDashboardGraph> listsales = dashboardRepository.GetSalesForGraph(fromDate, toDate, ClientID);

                var jsonpayments = JsonConvert.SerializeObject(listpayment);
                var jsonsales = JsonConvert.SerializeObject(listsales);
                var json = JsonConvert.SerializeObject(POSDashboardVM);
                var jsonpartici = JsonConvert.SerializeObject(participationPercentage);
               var jsonAccountInfo = JsonConvert.SerializeObject(await accountInfo);




               return Json(new
               {
                   Payments =jsonpayments,
                   Sales = jsonsales,
                   CashierSession = json,
                   ParticipationPercentage = jsonpartici,
                   AccountInfo = jsonAccountInfo
               },
               JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeController", "Error : " + ex.Message, ClientID.ToString(), "GetCasheirSessions");
                return Json(new { result = "-1" });
            }

        }

        

        public ActionResult KeepSessionAlive(JQueryDataTableParamModel param)
        {
            //UserRolesHelper urh = new UserRolesHelper();
           return Json(new
            {
                message="i Am aLiVe!!"
            },
            JsonRequestBehavior.AllowGet);
        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MSA_AdminPortal.Helpers;
using Repository;
using Repository.Helpers;
using System.Globalization;
using AdminPortalModels.ViewModels;

namespace MSA_AdminPortal.Controllers
{
    public class PaymentController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        private HomeRoomHelper homeroomHelper = new HomeRoomHelper();
        //
        // GET: /Payment/

        public PaymentController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

        public ActionResult Payment(int id = 0, string searchBy = "")
        {
            if (!SecurityManager.AllowNewPayments) return RedirectToAction("NoAccess", "Security", new { id = "nopayments" }); 
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
                ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
                ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == ClientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);
               
                SetViewBagValues(ClientId,id,searchBy);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PaymentController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Payment");
            }

            return View();
        }

        public ActionResult Adjustment(int id = 0,string searchBy="")
        {
            if (!SecurityManager.AllowAccountAdjustments) return RedirectToAction("NoAccess", "Security", new { id = "noadjustments" }); 
            try
            {
                long ClientId = ClientInfoData.GetClientID();

                ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
                ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
                ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == ClientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);

                SetViewBagValues(ClientId, id, searchBy);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PaymentController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Adjustment");                               
            }

            return View();
        }

        public ActionResult Refund(int id = 0, string searchBy = "")
        {

            if (!SecurityManager.AllowRefunding) return RedirectToAction("NoAccess", "Security", new { id = "norefunds" });
            try
            {
                long ClientId = ClientInfoData.GetClientID();

                ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
                ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
                ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == ClientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);

                SetViewBagValues(ClientId, id, searchBy);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PaymentController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Adjustment");                               
            }

            return View();
        }

        private void SetViewBagValues(long ClientId, int id,string searchBy)
        {
            ViewBag.searchBy = searchBy;
            if (id != 0)
            {
                var customer = unitOfWork.generalRepository.GetCustomerDetailForPayment(ClientId, id);
                ViewBag.CustomerId = id;
                ViewBag.CustomerName = customer.FirstName+ customer.LastName;
                ViewBag.UserID = customer.UserID;
                ViewBag.MealPlanBalance = "$" + customer.MealPlanBalance.ToString("F", CultureInfo.InvariantCulture);
                ViewBag.AlaCarteBalance = "$" + customer.AlaCarteBalance.ToString("F", CultureInfo.InvariantCulture);
            }
            else
            {
                ViewBag.CustomerName = "";
                ViewBag.CustomerId = 0;
                ViewBag.UserID = "";
                ViewBag.MealPlanBalance = "";
                ViewBag.AlaCarteBalance = "";
            }
        }

        [HttpPost]
        public JsonResult ApplyPayment(PaymentData PayData)
        {
            try
            {
                string res;
                double? mDebit = null;
                double? aDebit = null;
                int? CheckNum = null;
                int TransType = 1100;

                long ClientId = ClientInfoData.GetClientID();
                int CustomerId = ClientInfoData.GetCustomerID();
                if (PayData.mealPlan == true)
                    mDebit = PayData.Amount;
                if (PayData.alaCarte == true)
                    aDebit = PayData.Amount;
                if (PayData.CheckNum > 0)
                {
                    TransType += 100;
                    CheckNum = PayData.CheckNum;
                }

               DateTime clientLocalDateTime= TimeZoneHelper.GetClientTimeZoneLocalDateTime();


               res = unitOfWork.generalRepository.Save_Order(ClientId, -1, -3, CustomerId, PayData.CustomerId, TransType, mDebit, aDebit, -1, -1, -1, -1, clientLocalDateTime, null, CheckNum, false, false, null, null);

                return Json(new { result = res });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PaymentController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ApplyPayment");
                return null;   
            }
        }

        [HttpPost]
        public JsonResult ApplyAdjustment(AdjustmentData AdjData)
        {
            try
            {
                string res;
                double? mDebit = null;
                double? aDebit = null;
                int? LogID = null;
                string LogNotes = null;
                int Adj = 1;

                if (AdjData.negative == true)
                    Adj = -1;

                long ClientId = ClientInfoData.GetClientID();
                int CustomerId = ClientInfoData.GetCustomerID();
                if (AdjData.mealPlan == true)
                    mDebit = AdjData.Amount * Adj;
                if (AdjData.alaCarte == true)
                    aDebit = AdjData.Amount * Adj;

                if (AdjData.LogNotes != string.Empty)
                {
                    LogID = -1;
                    LogNotes = AdjData.LogNotes;
                }

                DateTime clientLocalDateTime = TimeZoneHelper.GetClientTimeZoneLocalDateTime();
                res = unitOfWork.generalRepository.Save_Order(ClientId, -1, -3, CustomerId, AdjData.CustomerId, 1500, mDebit, aDebit, -1, -1, -1, -1, clientLocalDateTime, null, null, false, false, LogID, LogNotes);

                return Json(new { result = res });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PaymentController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ApplyAdjustment");
                return null;   
            }
        }

        [HttpPost]
        public JsonResult ApplyRefund(RefundData RefData)
        {
            try
            {
                string res;
                double? mDebit = null;
                double? aDebit = null;

                long ClientId = ClientInfoData.GetClientID();
                int CustomerId = ClientInfoData.GetCustomerID();
                if (RefData.mealPlan == true)
                    mDebit = RefData.Amount * -1;
                if (RefData.alaCarte == true)
                    aDebit = RefData.Amount * -1;

                DateTime clientLocalDateTime = TimeZoneHelper.GetClientTimeZoneLocalDateTime();
                res = unitOfWork.generalRepository.Save_Order(ClientId, -1, -3, CustomerId, RefData.CustomerId, 1400, mDebit, aDebit, -1, -1, -1, -1, clientLocalDateTime, null, null, false, true, null, null);

                return Json(new { result = res });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PaymentController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ApplyRefund");
                return null;   
            }
        }

        public class PaymentData
        {
            public int CustomerId { get; set; }
            public double Amount { get; set; }
            public int CheckNum { get; set; }
            public bool mealPlan { get; set; }
            public bool alaCarte { get; set; }
        }

        public class RefundData
        {
            public int CustomerId { get; set; }
            public double Amount { get; set; }
            public bool mealPlan { get; set; }
            public bool alaCarte { get; set; }
        }

        public class AdjustmentData
        {
            public int CustomerId { get; set; }
            public double Amount { get; set; }
            public bool positive { get; set; }
            public bool negative { get; set; }
            public bool mealPlan { get; set; }
            public bool alaCarte { get; set; }
            public string LogNotes { get; set; }
        }

    }
}

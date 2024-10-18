using AdminPortalModels.ViewModels;
using Repository;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using AdminPortalModels.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using MSA_AdminPortal.Helpers;
using System.Configuration;

namespace MSA_AdminPortal.Controllers
{
    public class BeginningBalanceController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        private IBeginningBalanceRepository beginningBalanceRepository;
        private IGraduateSeniorsRepository graduateSeniorsRepository;
        private IGeneralRepository generalRepository;
        public BeginningBalanceController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
            this.beginningBalanceRepository = unitOfWork.beginningBalanceRepository;
            this.graduateSeniorsRepository = unitOfWork.GraduateSeniorsRepository;
            this.generalRepository = unitOfWork.generalRepository;
        }

        public ActionResult Index()
        {
            try
            {
                var clientId = ClientInfoData.GetClientID();

                //ViewBag.GradeList = unitOfWork.generalRepository.getGrades(clientId).ToList();
                //ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId && (!x.isDeleted)).Select(x => new { value = x.ID, data = x.SchoolName }).OrderBy(x => x.data).ToList();
                ViewBag.DistrictList = unitOfWork.DistrictRepository.GetQuery(x => x.ClientID == clientId && (!x.isDeleted)).Select(x => new { value = x.ID, data = x.DistrictName }).OrderBy(x => x.data).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "BeginningBalanceController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Index");
                return null;
            }
            return View();
        }

        public ActionResult AjaxSearchHandler(JQueryDataTableParamModel param, BeginningBalanceFilters filters)
        {
            try
            {
                //if (Convert.ToInt32(param.sEcho) > 1)
                //{
                long ClientId = ClientInfoData.GetClientID();
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc

                filters.SortColumn = sortColumnIndex.ToString();
                filters.SortOrder = sortDirection;

                //due to bug#1761

                //var beginnnigBalanceData = beginningBalanceRepository.GetFilteredSortedBeginningBalance(ClientId
                //    , filters.SearchString == null ? "" : filters.SearchString
                //    , (filters.SchoolFilter == null || filters.SchoolFilter == "0") ? "" : filters.SchoolFilter
                //    , (filters.GradeFilter == null || filters.GradeFilter == "0") ? "" : filters.GradeFilter
                //    , (filters.HomeRoomFilter == null || filters.HomeRoomFilter == "0") ? "" : filters.HomeRoomFilter
                //    , (filters.DistrictFilter == null || filters.DistrictFilter == "0") ? "" : filters.DistrictFilter
                //    , filters.SortColumn == null ? "" : filters.SortColumn
                //    , filters.SortOrder == null ? "" : filters.SortOrder);

                //var beginnnigBalanceData = beginningBalanceRepository.GetFilteredSortedBeginningBalance(ClientId
                //    , filters.SearchString == null ? "" : filters.SearchString
                //    , (filters.SchoolFilter == null || filters.SchoolFilter == "0") ? "" : filters.SchoolFilter
                //    , (filters.GradeFilter == null || filters.GradeFilter == "0") ? "" : filters.GradeFilter
                //    , (filters.HomeRoomFilter == null || filters.HomeRoomFilter == "0") ? "" : filters.HomeRoomFilter
                //    , (filters.DistrictFilter == null || filters.DistrictFilter == "0") ? "" : filters.DistrictFilter
                //    , filters.SortColumn == null ? "" : filters.SortColumn
                //    , filters.SortOrder == null ? "" : filters.SortOrder);

                var beginnnigBalanceData = beginningBalanceRepository.GetFilteredSortedBeginningBalance(ClientId
                    , filters.SearchString == null ? "" : filters.SearchString
                    , (filters.SchoolFilter == null ) ? "" : filters.SchoolFilter
                    , (filters.GradeFilter == null ) ? "" : filters.GradeFilter
                    , (filters.HomeRoomFilter == null ) ? "" : filters.HomeRoomFilter
                    , (filters.DistrictFilter == null ) ? "" : filters.DistrictFilter
                    , filters.SortColumn == null ? "" : filters.SortColumn
                    , filters.SortOrder == null ? "" : filters.SortOrder);

                var result = from c in beginnnigBalanceData
                             select new[] { c.Id.ToString(), c.UserId, c.CustomerName, c.Grade, c.MealPlan.ToString(), c.AlaCartePlan.ToString(), c.Balance.ToString(), c.PrevMealPlanBalance.ToString(), c.PrevAlaCartePlanBalance.ToString() };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = beginnnigBalanceData.Count(),
                    iTotalDisplayRecords = beginnnigBalanceData.Count(),
                    aaData = result
                },
            JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "BeginningBalanceController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxSearchHandler");
                return null;
            }
        }

        public ActionResult GetDistinctGrades(string schoolId, string districtId)
        {
            try
            {
                int totalRecords;
                long ClientId = ClientInfoData.GetClientID();

                var distinctGrade = graduateSeniorsRepository.FetchDistinctGradesForGraduateSeniors(ClientId, Convert.ToInt64(schoolId), Convert.ToInt64(districtId));

                return Json(new
                {
                    Data = distinctGrade
                },
            JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "BeginningBalanceController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistinctGrades");
                return null;
            }
        }

        public ActionResult GetDistinctHomeRoom(string schoolId, string gradeId, string districtId)
        {
            try
            {
                int totalRecords;
                long ClientId = ClientInfoData.GetClientID();

                var beginningBalancesDistinctHomeRoom = beginningBalanceRepository.FetchDistinctHomeRoomForBeginningBalance(ClientId, Convert.ToInt64(schoolId), Convert.ToInt64(gradeId), Convert.ToInt64(districtId));

                return Json(new
                {
                    Data = beginningBalancesDistinctHomeRoom
                },
            JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "BeginningBalanceController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistinctHomeRoom");
                return null;
            }
        }


        public ActionResult SavePayment(List<BeginningBalancePaymentData> olstBeginningBalancePaymentData)
        {
            try
            {
                string res = "";
                int? CheckNum = null;
                int TransType = 1700;

                long ClientId = ClientInfoData.GetClientID();
                int CustomerId = ClientInfoData.GetCustomerID();

                foreach (var bbPayments in olstBeginningBalancePaymentData)
                {
                    double? mDebit = null;
                    double? aDebit = null;

                    aDebit = bbPayments.ACAmount;
                    mDebit = bbPayments.MPAmount;



                    DateTime clientLocalDateTime = TimeZoneHelper.GetClientTimeZoneLocalDateTime();


                    res = unitOfWork.generalRepository.Save_Order(ClientId, -1, -3, CustomerId, bbPayments.CustomerId, TransType, mDebit, aDebit, -1, -1, -1, -1, clientLocalDateTime, null, CheckNum, false, false, null, null);

                    if (res != "-1")
                        break;
                }

                return Json(new { result = res });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "BeginningBalanceController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistinctHomeRoom");
                return Json(new { result = "0" });
            }
        }

        public ActionResult ConfirmDialog(int id = 0)
        {
            return PartialView("_ConfirmDialog");
        }

        public ActionResult GetDistinctSchools(string districtId)
        {
            try
            {
                int totalRecords;
                long ClientId = ClientInfoData.GetClientID();

                var graduateSeniorsDistinctGrade = graduateSeniorsRepository.FetchDistinctSchoolsForGraduateSeniors(ClientId, Convert.ToInt64(districtId));

                return Json(new
                {
                    Data = graduateSeniorsDistinctGrade
                },
            JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GraduateSeniorsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistinctGrades");
                return null;
            }
        }
    }


}

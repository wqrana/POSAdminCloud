using AdminPortalModels.ViewModels;
using Repository;
using Repository.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using MSA_AdminPortal.Helpers;

namespace MSA_AdminPortal.Controllers
{
    public class OrdersMgtController : BaseAuthorizedController
    {

        UnitOfWork unitOfWork = null;

        public OrdersMgtController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

        public ActionResult Index()
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();

                
                ViewBag.SearchByList = GetSearchDDLItems();
                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "OrdersMgt");
            }
            return View();
        }

        public ActionResult Activity(bool search = false, string searchBy = "")
        {
            try
            {
                if (!SecurityManager.viewActivity) return RedirectToAction("NoAccess", "Security", new { id = "noactivity" });
                long ClientId = ClientInfoData.GetClientID();
                ViewBag.SearchByList = GetSearchDDLItems();
                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
                ViewBag.searchRequired = search;
                ViewBag.searchBy = searchBy.Replace("\"","");
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Activity");
            }
            return View();
        }


        public ActionResult AjaxHandler(JQueryDataTableParamModel param, GroupFilters grpFilters)
        {
            try
            {
                int totalRecords = 0;
                long ClientId = ClientInfoData.GetClientID();

                // by Farrukh on 04/14/16 to fix PA-498
                var endDate = grpFilters.DateEnd.ToString("MM/dd/yyyy");
                endDate = endDate + " 23:59:59 PM";
                grpFilters.DateEnd = Convert.ToDateTime(endDate);
                //-------------------------------------------------

                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc
                var groupData = unitOfWork.orderRepository.GetVoidGroup_List(ClientId, param, grpFilters, out totalRecords);
                int recordCount = groupData.Count();
                string grpID = groupData.Select(o => o.groupID).FirstOrDefault().ToString();
                if (grpID != "")
                {
                    var result = from g in groupData
                                 select new[] { g.groupID.ToString(), g.groupID.ToString(), g.groupName, g.groupName, g.groupID.ToString(), g.UserID, g.CustomerBalance.ToString() };

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = totalRecords,
                        iTotalDisplayRecords = totalRecords,
                        aaData = result
                    },
                JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = ""
                    },
                    JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxHandler");
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetDetail(string allData)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                if (Request["gpFilters"] != null)
                {

                }
                int? groupID = 0;
                DateTime startDate = Convert.ToDateTime("02/02/2000");
                DateTime endDate = Convert.ToDateTime("02/02/2030");
                int searchBy = 0;

                if (!string.IsNullOrEmpty(allData))
                {
                    string[] alldatatemp = allData.Split('*');
                    groupID = Convert.ToInt32(alldatatemp[0]);
                    startDate = Convert.ToDateTime(alldatatemp[1] + " 00:00:00.000");
                    endDate = Convert.ToDateTime(alldatatemp[2] + " 23:59:59.999");
                    searchBy = Convert.ToInt32(alldatatemp[3]);
                }

                var detailData = GetCustomerOrdersDetail(ClientId, groupID, startDate, endDate, searchBy);
                DateTime minDate = detailData.Min(m => m.OrderDate).GetValueOrDefault();
                DateTime maxDate = detailData.Max(m => m.OrderDate).GetValueOrDefault();

                bool isVoidbool = detailData.Select(m => m.isVoid).FirstOrDefault() == null ? false : true;

                //Added by Waqar Q. 
                var detailDataReturn = detailData.Select(x => new
                {
                    check = x.Check,
                    isVoid = x.isVoid,
                    Item = x.Item,
                    Name = x.Name,
                    OrderDate = String.Format("{0:MM/dd/yyyy @ hh:mm tt}", x.OrderDate),
                    OrderDateLocal = String.Format("{0:MM/dd/yyyy @ hh:mm tt}", x.OrderDateLocal),
                    OrderID = x.OrderID,
                    OrderLogID = x.OrderLogID,
                    OrderType = x.OrderType,
                    Payment = x.Payment,
                    POS = x.POS,
                    SalesTax = x.SalesTax,
                    Total = x.Total,
                    Type = x.Type

                });


                string outputOfdetails = JsonConvert.SerializeObject(detailDataReturn);


                if (outputOfdetails == "" || detailData.Count() == 0)
                {
                    return Json(new { result = "-1" });
                }
                else
                {
                    var jsonResult = Json(new { result = outputOfdetails, startDate = minDate, endDate = maxDate, isVoid = isVoidbool });
                    jsonResult.MaxJsonLength = Int32.MaxValue;
                    return jsonResult;
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDetail");
                return Json(new { result = "-1" });
            }
        }

        private DateTime CheckIfDateRangeIsSmall(DateTime minDate, DateTime maxDate)
        {
            TimeSpan difference = maxDate - minDate;
            double days = difference.TotalDays;
            if (days < 30)
            {
                return maxDate.AddDays(-30);
            }
            else
            {
                return minDate;
            }
        }

        [HttpPost]
        public JsonResult GetActivityCount(string allData)
        {
            try
            {
                string customerId = allData;
                long ClientId = ClientInfoData.GetClientID();
                // change by farrukh on 7-april-2016 in order to fix JIRA item PA-498
                int activityCount = unitOfWork.orderRepository.GetCustomerActivityCount(ClientId, Convert.ToInt64(customerId));  //unitOfWork.orderRepository.GetCustomerActivityCount(ClientId, customerId);
                if (activityCount > 0)
                {
                    return Json(new { result = "yes" });
                }
                else
                {
                    return Json(new { result = "no" });
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetActivityCount");
                return null;
            }

        }

        [HttpPost]
        public JsonResult VoidOrder(string allData)
        {
            try
            {
                if (!SecurityManager.viewActivity) return Json(new { result = "-3", vtype = "order" }); ;
                long ClientId = ClientInfoData.GetClientID();
                if (Request["gpFilters"] != null)
                {

                }
                long orderID = 0;
                long empllyeeID = 0;
                int orderType = 0;
                long? orderLogId = null;
                string orderLogNote = "";
                string iTEMIDs = "";
                string voidtype = "";
                bool voidPayment = false;


                if (!string.IsNullOrEmpty(allData))
                {
                    string[] alldatatemp = allData.Split('*');
                    orderID = Convert.ToInt64(alldatatemp[0]);
                    orderType = Convert.ToInt16(alldatatemp[1]);
                    iTEMIDs = Convert.ToString(alldatatemp[2]);
                    empllyeeID = Convert.ToInt64(alldatatemp[3]);
                    if (alldatatemp[4] == "")
                    {
                        orderLogId = -1; // changed from "null" to "-1", by farrukh m (allshore) to fix order log issue. (PA-519)
                    }
                    else
                    {
                        orderLogId = Convert.ToInt64(alldatatemp[4]);
                    }
                    orderLogNote = Convert.ToString(alldatatemp[5]);
                    voidtype = Convert.ToString(alldatatemp[6]);
                    voidPayment = Convert.ToBoolean(alldatatemp[7]);

                }
                bool success = false;
                if (voidtype != "")
                {
                    if (voidtype == "items")
                    {
                        success = VoidItems(ClientId, iTEMIDs, empllyeeID, orderType, orderLogId, orderLogNote);
                    }
                    else if (voidtype == "order")
                    {
                        success = VoidFullOrder(ClientId, empllyeeID, orderID, orderType, voidPayment, orderLogId, orderLogNote);
                    }
                }
                else
                {
                    success = false;

                }

                if (!success)
                {
                    return Json(new { result = "-1", vtype = voidtype });
                }
                else
                {
                    return Json(new { result = "-2", vtype = voidtype });
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "VoidOrder");
                return null;
            }
        }

        private bool VoidItems(Nullable<long> clientID, string iTEMIDs, Nullable<long> eMPLOYEEID, Nullable<int> oRDERTYPE, Nullable<long> oRDLOGID, string oRDLOGNOTE)
        {
            try
            {
                bool success = true;
                string[] itemIdList = iTEMIDs.Split(',');
                if (itemIdList.Length > 0 && itemIdList[0] != "")
                {
                    for (int i = 0; i < itemIdList.Length; i++)
                    {
                        long tempItemID = Convert.ToInt64(itemIdList[i]);
                        success = unitOfWork.orderRepository.VoidItem(clientID, tempItemID, eMPLOYEEID, oRDERTYPE, oRDLOGID, oRDLOGNOTE);
                        if (!success)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "VoidItems");
                return false;
            }
        }

        private bool VoidFullOrder(Nullable<long> clientID, Nullable<long> eMPLOYEEID, Nullable<long> oRDERID, Nullable<int> oRDERTYPE, Nullable<bool> vOIDPAYMENT, Nullable<long> oRDLOGID, string oRDLOGNOTE)
        {
            bool success = true;
            success = unitOfWork.orderRepository.VoidAllOrder(clientID, eMPLOYEEID, oRDERID, oRDERTYPE, vOIDPAYMENT, oRDLOGID, oRDLOGNOTE);
            return success;
        }

        public ActionResult Popup()
        {
            return PartialView("_voidPopup"); //, model
        }


        public JsonResult PopupData()
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                int ordertype = 0;
                if (Request["ordertype"] != null)
                {
                    ordertype = Convert.ToInt16(Request["ordertype"]);
                }
                Int32 OrderId = 0;
                if (Request["orderid"] != null)
                {
                    OrderId = Convert.ToInt32(Request["orderid"]);
                }

                //bind model
                var model = unitOfWork.orderRepository.OrderInfo(ClientId, OrderId, ordertype);
                //if (model != null)
                //{
                //    model.OrderDateString = GetConvertedDateTimeString(model.OrderDate, ClientId);
                //}
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "PopupData");
                return null;
            }
        }

        public string ConvertVoidStatus(string s)
        {
            if (s == "False") return "No"; else return "Yes";
        }

        public ActionResult ItemDetailsAjaxHandler(JQueryDataTableParamModel param, OrderParam op)
        {
            try
            {
                int totalRecords = 12;
                long ClientId = ClientInfoData.GetClientID();
                var orderDetail = unitOfWork.orderRepository.OrderDetailsPopup(ClientId, Convert.ToInt32(op.orderId), Convert.ToInt32(op.ordertype));
                var result = from od in orderDetail
                             select new[] { od.id.ToString(), od.Qty.ToString(), od.ItemName, '$' + (od.PaidPrice * od.Qty).ToString(), ConvertVoidStatus(od.Void.ToString()), od.ServingDate.ToString(), od.PickedupDate.ToString() };
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = result
                },
            JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ItemDetailsAjaxHandler");
                return null;
            }
        }

        private List<SearchBy> GetSearchDDLItems()
        {
            List<SearchBy> list = new List<SearchBy>();
            list.Add(new SearchBy { id = "2", name = "Customer" });
            list.Add(new SearchBy { id = "0", name = "Cashier" });
            list.Add(new SearchBy { id = "1", name = "POS" });
            list.Add(new SearchBy { id = "3", name = "School Name" });
            return list;
        }

        private IEnumerable<CustomerOrderDetails> GetCustomerOrdersDetail(Nullable<long> clientID, Int32? GroupID, DateTime stDate, DateTime endDate, int searchBy)
        {
            try
            {
                IEnumerable<CustomerOrderDetails> customerOrderDetails = unitOfWork.orderRepository.GetVoidGroup_Details(clientID, GroupID, stDate, endDate, searchBy);

                //foreach (var item in customerOrderDetails)
                //{
                //    if (item.OrderDate != null)
                //    {
                //        item.OrderDate = TimeZoneHelper.ConvertDateTimeToClientTime(Convert.ToDateTime(item.OrderDate), (long)clientID);
                //    }
                //}

                return customerOrderDetails;

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrdersMgtController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerOrdersDetail");
                return null;
            }
        }

        //public string GetConvertedDateTimeString(DateTime orderDate, long ClientID)
        //{
        //    return TimeZoneHelper.ConvertDateTimeToClientTime(orderDate, ClientID).ToString("MM/dd/yyyy @ hh:mm tt");
        //}
    }
}

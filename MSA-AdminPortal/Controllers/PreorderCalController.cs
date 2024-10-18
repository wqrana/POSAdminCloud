using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repository;
using Repository.edmx;
using Repository.Helpers;
using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using MSA_AdminPortal.Helpers;
using System.Data.Entity.SqlServer;
using MSA_ADMIN.DAL.Factories;
using MSA_ADMIN.DAL.Models;
using System.Collections.ObjectModel;
using System.Web.Script.Serialization;
using Newtonsoft.Json;


namespace MSA_AdminPortal.Controllers
{


    public class PreorderCalController : BaseAuthorizedController
    {
        PreorderCalHelper helper = new PreorderCalHelper();
        PreorderCalListHelper helper2 = new PreorderCalListHelper();

        public ActionResult Index(int? id)
        {
            int CalID = 0;
            if (id.HasValue)
            {
                CalID = id.Value;
            }

            var CalData = helper2.Get(CalID);

            ViewBag.CalName = CalData.CalendarName;
            ViewBag.CalTypeName = CalData.CalendarTypeName;
            ViewBag.CatTypeList = helper.getCategoryList();
            var model = helper.GetOrderingOptionsModel(CalID);

            return View(model);
            //return RedirectToAction("PreorderCal", new { calid = id });

        }


        public JsonResult AjaxHandler(int? CalId, string start, string end)
        {

            int WebCalId = 0;
            DateTime CurrentDate = DateTime.Now;

            DateTime StarttDate = Convert.ToDateTime(start);

            DateTime endDate = Convert.ToDateTime(end);


            if (CalId.HasValue)
            {
                WebCalId = CalId.Value;
            }

            //WebCalId = 930;
            var CalItemsData = helper.CalendarItems(WebCalId, StarttDate, endDate);

            var eventList = from e in CalItemsData
                            select new
                            {
                                id = e.id,
                                title = e.title,
                                start = e.start,
                                end = e.end,
                                color = e.color,
                                textColor = e.textColor,
                                showOrder = e.showOrder,
                                userOrder = e.userOrder,
                                allDay = true,
                                webCalID = e.WebCalID,
                                menus_id = e.menus_id,
                                tooltip = e.AltDescription,
                                price = e.StudentFullPrice,
                            };
            var rows = eventList.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AjaxHandlerMenuItems(SearchModel searchModel)
        {
            var Allamenuitems = helper.SearchCategories(searchModel);
            //var itemsList = from e in Allamenuitems
            //                select new
            //                {
            //                    id = e.MenuID,
            //                    title = e.ItemName,
            //                };
            var rows = Allamenuitems.ToList();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteCalItem(WebCalItem WebCalItem)
        {
            // Int32 calItemID,DateTime calItemDate, Int32 webCalID
            try
            {

                long ClientId = ClientInfoData.GetClientID();
                Int32 menus_id = WebCalItem.menus_id;
                DateTime CalItemDate = WebCalItem.calItemDate;
                Int32 WebCalID = WebCalItem.webCalID;

                CalFactory.DelWebLunchMenuItemsInCal(WebCalID, menus_id, CalItemDate);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteCalItem");
                return Json(new { result = "notOkay" });
            }
        }

        public JsonResult SaveCalMenuItem(WebCalItem WebCalItem)
        {
            // Int32 calItemID,DateTime calItemDate, Int32 webCalID
            try
            {

                long ClientId = ClientInfoData.GetClientID();
                Int32 menus_id = WebCalItem.menus_id;
                DateTime CalItemDate = WebCalItem.calItemDate;
                Int32 WebCalID = WebCalItem.webCalID;
                Int32 mealType = WebCalItem.mealType;

                Int32 inserted = CalFactory.AddWebLunchSchedule(CalItemDate, mealType, menus_id, 1, false, WebCalID, false, 0);
                //Int32 inserted = 99999;
                return Json(new { result = "ok", recordID = inserted.ToString() });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SaveMenuItem");
                return Json(new { result = "notOkay" });
            }
        }


        [HttpPost]
        public JsonResult UpdateCalItemStatus(WebCalItemStatus WebCalItemStatus)
        {
            try
            {

                string Status = WebCalItemStatus.Status;
                DateTime SelectedDate = WebCalItemStatus.SelectedDate;
                Int32 WebCalID = WebCalItemStatus.WebCalID;

                CalFactory.UpdateCalByOrderStatusForMonth(WebCalID, SelectedDate, Status);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UpdateCalItemStatus");
                return Json(new { result = "notOkay" });
            }
        }

        [HttpPost]
        public JsonResult UpdateCalItemStatusDay(WebCalItemStatus WebCalItemStatus)
        {
            try
            {

                string Status = WebCalItemStatus.Status;
                DateTime SelectedDate = WebCalItemStatus.SelectedDate;
                Int32 WebCalID = WebCalItemStatus.WebCalID;

                CalFactory.UpdateCalByOrderStatusForDay(WebCalID, SelectedDate, Status);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UpdateCalItemStatus");
                return Json(new { result = "notOkay" });
            }
        }



        [HttpPost]
        public JsonResult UpdateOrderingOptions(OrderingOptionsModel orderingOptionsModel)
        {
            try
            {
                helper.SaveOrderingOptions(orderingOptionsModel);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UpdateOrderingOptions");
                return Json(new { result = "notOkay" });
            }
        }


        [HttpPost]
        public JsonResult UpdateMenuItems(List<WebCalItem> items)
        {


            try
            {
                helper.SaveMenuItems(items);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UpdateMenuItems");
                return Json(new { result = "notOkay" });
            }

        }

        [HttpPost]
        public JsonResult UpdateCalForSelectedDates(ItemScheduler items)
        {

            try
            {
                helper.LoopforSelecteddates(items);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UpdateMenuItems");
                return Json(new { result = "notOkay" });
            }

        }

        [HttpPost]
        public JsonResult GetWebLunchCutoffSettings(WebCalItemStatus webCalItemStatus)
        {


            try
            {
                var settings = helper.GetWebLunchCutoffSettings(webCalItemStatus.SelectedDate, webCalItemStatus.WebCalID);
                return Json(new { result = "ok", fiveday = settings.useFiveDayWeekCutOff.ToLower(), cutOFFdate = settings.CutOffDate.ToString("MM/dd/yyyy"), OverrideCutOffValue = settings.overrideCutOff, Cutoffvalue = settings.Cutoffvalue, hasCutOffvalue = settings.hasCutOffvalue.ToString().ToLower(), isOverriddentCutOff = settings.isOverriddentCutOff.ToString().ToLower(), AcceptOrderDate = TimeZoneSettings.Instance.GetLocalTime().Date.ToString("MM/dd/yyyy") });
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UpdateMenuItems");
                return Json(new { result = "notOkay" });
            }

        }

        [HttpPost]
        public JsonResult OverrideCutOffDate(OverrideCutOffData overrideCutOffData)
        {


            try
            {
                helper.UpdateOverrideCutOffDate(overrideCutOffData.cutOffType, overrideCutOffData.cutOffValue, overrideCutOffData.pWebcalid, overrideCutOffData.pDate);
                return Json(new { result = "ok" });
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UpdateMenuItems");
                return Json(new { result = "notOkay" });
            }

        }












    }









}

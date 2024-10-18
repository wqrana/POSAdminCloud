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


namespace MSA_AdminPortal.Controllers
{
    public class PreorderCalListController : BaseAuthorizedController
    {
        //

        public PreorderCalListHelper helper = new PreorderCalListHelper();


        //public ActionResult Index()
        //{
        //    return RedirectToAction("Table");
        //}

        public ActionResult Table()
        {
            var model = helper.CalendarList();
            WeblunchCalendarCreateModel CreateObj = new WeblunchCalendarCreateModel();
            ViewBag.CalTypeList = CreateObj.CalTypeList;
            return View(model);
        }

        [HttpPost]
        public JsonResult updateSchoolsList(string allData)
        {
            try
            {
                helper.UpdateWeblunchSchools(allData);
                string disdata = "-1";
                return Json(new { result = disdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalListController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditCalendar");
                return null;
            }
        }

        public ActionResult Delete(int id = 0)
        {
            var model = helper.GetDeleteModel(id, true);

            return GetActionResult(Request, model);
        }

        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id = 0)
        {
            var model = helper.GetDeleteModel(id, false);

            try
            {
                if (!model.IsError)
                {
                    helper.DeleteCalendar(id);
                    model.Message = "The calendar has been deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalListController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }

            return GetActionResult(Request, model, false);
        }

        private ActionResult GetActionResult(HttpRequestBase request, CalendarDeleteModel model, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            if (model.IsError || isGetAction)
            {
                return PartialView("_Delete", model);
            }

            return RedirectToAction("Index");
        }


        private ActionResult GetActionResult(HttpRequestBase request, WeblunchCalendarUpdateModel viewModel, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            if (viewModel.IsError || isGetAction)
            {
                return PartialView("Popup", viewModel);
            }

            return RedirectToAction("Index");
        }
        private ActionResult GetActionResult(HttpRequestBase request, WeblunchCalendarCreateModel viewModel, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            if (viewModel.IsError || isGetAction)
            {
                return PartialView("Popup", viewModel);
            }

            return RedirectToAction("Index");
        }

        //[ChildActionOnly]
        public ActionResult CreateCalendar()
        {
            var model = helper.GetCreateModel();
            ViewBag.CalTypeList = model.CalTypeList;

            return GetActionResult(Request, model);
        }

        ////[ChildActionOnly]
        //public ActionResult Create()
        //{
        //    var model = helper.GetCreateModel();

        //    return GetActionResult(Request, model);
        //}

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult CreateCalendar(WeblunchCalendarCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int retValue = -999;
                    try
                    {
                        retValue = helper.CreateNewCalendar(model);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        helper.SetErrors(model, ViewData);
                    }

                    model.WebCalID = retValue;
                }
                else
                {
                    helper.SetErrors(model, ViewData);
                }

                return GetActionResult(Request, model, false);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCallListController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "CreateCalendar");
                return null;
            }
        }





        public ActionResult EditCalendar(int id = 0)
        {
            var model = helper.GetEditModel(id);

            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult EditCalendar(WeblunchCalendarUpdateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = helper.Get(model.WebCalID);
                    int retValue = -999;


                    if (entity == null)
                    {
                        return GetActionResult(Request, helper.GetEditModelOnError(), false);
                    }

                    try
                    {
                        retValue = helper.UpdateCalenderName(model);
                    }
                    catch (Exception ex)
                    {
                        //Error logging in cloud tables 
                        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalListController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditCalendar");
                        helper.SetErrors(model, ViewData);
                    }
                    model.WebCalID = retValue;
                }
                else
                {
                    helper.SetErrors(model, ViewData);
                }

                return GetActionResult(Request, model, false);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCallListController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditCalendar");
                return null;
            }
        }



    }

    public class PreorderCalListHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        public PreorderCalHelper helper2 = new PreorderCalHelper();
        public CalendarDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new CalendarDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }

        public void SetErrors(CalendarDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        // for create/update
        public void SetErrors(WeblunchCalendar model, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    model.ErrorMessage2 += error.ErrorMessage + "<br />";
                }
            }

            model.IsError = true;
        }
        public void DeleteCalendar(int id = 0)
        {

            var entity = Get(id);
            CalFactory.DeleteCalendar(id, clientId);
            //SoftDelete(entity);
        }

        public CalendarDeleteModel GetDeleteModelOnError()
        {
            return new CalendarDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }



        public CalendarDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            try
            {
                var entity = Get(id);

                if (entity == null)
                {
                    return GetDeleteModelOnError();
                }


                return new CalendarDeleteModel
                {
                    Id = entity.WebCalID,
                    Name = entity.CalendarName,
                    Message = errorMessage,
                    IsError = !string.IsNullOrWhiteSpace(errorMessage),
                };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderCalListController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDeleteModel");
                return null;
            }
        }

        public WeblunchCalendar Get(int id)
        {
            var cal = CalFactory.GetPreorderCalendar(id); //new WebLunchCalendar { CalendarName = "temp", CalendarType = 1, DistrictID = 44, WebCalID = 5 };
            return cal; // GetAll().Where(x => x.ID == id).FirstOrDefault();
        }

        public IList<WeblunchCalendar> CalendarList()
        {
            var List = CalFactory.GetPreorderCalendarList(clientId);
            return List;

        }

        public void UpdateWeblunchSchools(string dataStr)
        {

            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] actualData = dataStr.Split('*');

                if (actualData.Length == 2)
                {
                    string calID = actualData[0].ToString();
                    calID = calID.Replace("schoolsList", "").Trim();
                    int intcalID = Convert.ToInt32(calID);
                    string schoolList = actualData[1].ToString();

                    CalFactory.SaveAssignedSchools(intcalID, clientId, schoolList);
                }


            }

        }
        public WeblunchCalendarUpdateModel GetEditModel(int id)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetEditModelOnError();
            }

            return new WeblunchCalendarUpdateModel
            {
                WebCalID = entity.WebCalID,
                CalendarName = entity.CalendarName,
                CalendarType = entity.CalendarType,
                DistrictID = entity.DistrictID
            };
        }

        public WeblunchCalendarUpdateModel GetEditModelOnError()
        {
            return new WeblunchCalendarUpdateModel
            {
                WebCalID = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public WeblunchCalendarCreateModel GetCreateModel()
        {
            return new WeblunchCalendarCreateModel { };
        }

        public int UpdateCalenderName(WeblunchCalendar wc)
        {
            Int32 retValue = -999;
            // Changed to allow for testing - NAH (11/6/2017)
            //if (!MenuFactory.CalendarNameExists(wc.CalendarName, wc.DistrictID.ToString(), true))
            if (!MenuFactory.CalendarNameExists(wc.CalendarName, wc.DistrictID.ToString()))
            {
                helper2.ChangeCalendarName(wc.WebCalID, wc.DistrictID, wc.CalendarName);
                retValue = wc.WebCalID;
            }
            return retValue;
        }

        public Int32 CreateNewCalendar(WeblunchCalendar wc)
        {

            if (wc.DistrictID == 0)
            {
                wc.DistrictID = Convert.ToInt16(clientId);
            }
            Int32 retValue = -999;
            if (!MenuFactory.CalendarNameExists(wc.CalendarName, wc.DistrictID.ToString()))
            {
                retValue = MenuFactory.AddWebLunchCalendarData(Convert.ToInt32(wc.CalendarType), wc.CalendarName, wc.DistrictID, 0);
            }
            return retValue;

        }





        ///////////
    }
}

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


namespace MSA_AdminPortal.Controllers
{
    public class POSNotificationsController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        public POSNotificationsHelper helper = new POSNotificationsHelper();
        private HomeRoomHelper homeroomHelper = new HomeRoomHelper();

        public POSNotificationsController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }
        public ActionResult Index()
        {
            IList<POSNotificationsViewModel> POSNotificationsViewModelList = new List<POSNotificationsViewModel>();
            try
            {
                long ClientID = ClientInfoData.GetClientID();
                POSNotificationsViewModelList = helper.GetPOSNotifications(ClientID);

                ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientID).ToList();
                ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
                ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == ClientID && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Index");
            }
            return View(POSNotificationsViewModelList);
        }
        public ActionResult CreatePOSNotification()
        {
            var model = helper.GetCreateModel();
            //ViewBag.CalTypeList = model.CalTypeList;

            return GetActionResult(Request, model);
        }

        [HttpPost]
        public ActionResult CreatePOSNotification(POSNotificationsCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long retValue = -999;
                    try
                    {
                        model.ClientID = ClientInfoData.GetClientID();
                        if (unitOfWork.posNotificationsRepository.GetPOSNotificationByClientIdAndName(model.ClientID, model.Name))
                        {
                            //model.ErrorMessage = "Record already exist with the same name for the same client.";
                            model.ErrorMessage2 = "POS Notification record already exists with the same name or it is marked as deleted.";
                            helper.SetErrors(model, ViewData);
                        }
                        else
                        {
                            retValue = helper.CreateNewPOSNotification(model);
                            model.Id = retValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        helper.SetErrors(model, ViewData);
                    }
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "CreatePOSNotification");
                return null;
            }
        }

        public ActionResult EditPOSNotification(int id = 0)
        {
            var model = helper.GetEditModel(id);

            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult EditPOSNotification(POSNotificationsUpdateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = helper.Get(model.Id);
                    long retValue = -999;


                    if (entity == null)
                    {
                        return GetActionResult(Request, helper.GetEditModelOnError(), false);
                    }

                    try
                    {
                        model.ClientID = ClientInfoData.GetClientID();
                        if (unitOfWork.posNotificationsRepository.GetPOSNotificationByIdClientIdAndName(model.Id, model.ClientID, model.Name))
                        {
                            //model.ErrorMessage = "Record already exist with the same name for the same client.";
                            model.ErrorMessage2 = "POS Notification record already exists with the same name or it is marked as deleted.";
                            helper.SetErrors(model, ViewData);
                        }
                        else
                        {
                            retValue = helper.UpdatePOSNotification(model);
                            model.Id = retValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Error logging in cloud tables 
                        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditPOSNotification");
                        helper.SetErrors(model, ViewData);
                    }
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditPOSNotification");
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
                    long ClientID = ClientInfoData.GetClientID();
                    var olst = unitOfWork.posNotificationsRepository.GetCustomerPOSNotificationByClientandNotificationID(ClientID, id);

                    if (olst != null && olst.Count > 0)
                    {
                        helper.SetErrors(model, "Customer records are binded to this Notification.");
                    }
                    else
                    {
                        helper.DeletePosNotification(id);
                        model.Message = "The POS Notification has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }

            return GetActionResult(Request, model, false);
        }

        [HttpPost]
        public JsonResult AddNotificationToCustomer(string allData)
        {
            try
            {
                helper.AddNotificationToCustomer(allData);
                string disdata = "-1";
                return Json(new { result = disdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updateCustomerNotification");
                return null;
            }
        }

        [HttpPost]
        public JsonResult AddCustomerToNotificaion(string allData)
        {
            try
            {
                helper.AddCustomerToNotificaion(allData);
                string disdata = "-1";
                return Json(new { result = disdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updateCustomerNotification");
                return null;
            }
        }

        private ActionResult GetActionResult(HttpRequestBase request, POSNotificationsDeleteModel model, bool isGetAction = true)
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

        private ActionResult GetActionResult(HttpRequestBase request, POSNotificationsCreateModel viewModel, bool isGetAction = true)
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

        private ActionResult GetActionResult(HttpRequestBase request, POSNotificationsUpdateModel viewModel, bool isGetAction = true)
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
    }
}
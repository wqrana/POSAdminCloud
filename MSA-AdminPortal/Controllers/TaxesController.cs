using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPortalModels.ViewModels;
using Repository;
using Repository.edmx;
using Repository.Helpers;
using MSA_AdminPortal.Helpers;
using MSA_ADMIN.DAL.Factories;
using System.Web.Script.Serialization;
using MSA_ADMIN.DAL.Models;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using AdminPortalModels.Models;


namespace MSA_AdminPortal.Controllers
{
    
    public class TaxesController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        public TaxesListHelper helper= new TaxesListHelper();

        public TaxesController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

        public ActionResult Index()
        {
            IList<TaxListViewModel> taxesList = new List<TaxListViewModel>();
            try
            {
                long ClientID = ClientInfoData.GetClientID();
                taxesList = unitOfWork.taxRepository.GetTaxes(ClientID);
                
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxesController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }
            return View(taxesList);
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
                    var olst = unitOfWork.taxRepository.GetSchoolTaxByClientTaxID(ClientID, id);

                    if (olst != null && olst.Count > 0)
                    {
                        helper.SetErrors(model, "This Tax can't be deleted, some School(s) are referencing it.");
                    }
                    else
                    {
                        helper.DeleteTax(id);
                        model.Message = "The tax has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxesController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }

            return GetActionResult(Request, model, false);
        }

        private ActionResult GetActionResult(HttpRequestBase request, TaxesDeleteModel model, bool isGetAction = true)
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

        public ActionResult CreateTax()
        {
            var model = helper.GetCreateModel();
            //ViewBag.CalTypeList = model.CalTypeList;

            return GetActionResult(Request, model);
        }

        [HttpPost]
        public ActionResult CreateTax(TaxCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    long retValue = -999;
                    try
                    {
                        model.ClientId = ClientInfoData.GetClientID();
                        if (unitOfWork.taxRepository.GetTaxByClientIdAndName(model.ClientId, model.Name))
                        {
                            //model.ErrorMessage = "Record already exist with the same name for the same client.";
                            model.ErrorMessage2 = "Tax record already exist or it is marked as deleted.";
                            helper.SetErrors(model, ViewData);
                        }
                        else
                        {
                            retValue = helper.CreateNewTax(model);
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxesController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "CreateCalendar");
                return null;
            }
        }

        public ActionResult EditTax(int id = 0)
        {
            var model = helper.GetEditModel(id);

            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult EditTax(TaxUpdateModel model)
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
                        model.ClientId = ClientInfoData.GetClientID();
                        if (unitOfWork.taxRepository.GetTaxByIdClientIdAndName(model.Id,model.ClientId, model.Name))
                        {
                            //model.ErrorMessage = "Record already exist with the same name for the same client.";
                            model.ErrorMessage2 = "Tax record already exist or it is marked as deleted.";
                            helper.SetErrors(model, ViewData);
                        }
                        else
                        {
                            retValue = helper.UpdateTax(model);
                            model.Id = retValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Error logging in cloud tables 
                        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxesController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditCalendar");
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxesController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditCalendar");
                return null;
            }
        }
        private ActionResult GetActionResult(HttpRequestBase request, TaxCreateModel viewModel, bool isGetAction = true)
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

        private ActionResult GetActionResult(HttpRequestBase request, TaxUpdateModel viewModel, bool isGetAction = true)
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

        [HttpPost]
        public JsonResult updateSchoolsList(string allData)
        {
            try
            {
                helper.UpdateTaxSchools(allData);
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
    }

    public class TaxesListHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork;

        public TaxesListHelper()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }
        public TaxesDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new TaxesDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }

        public void SetErrors(TaxesDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        // for create/update
        public void SetErrors(Taxes model, ViewDataDictionary viewData)
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
        public void DeleteTax(long id = 0)
        {

            //var entity = Get(id);
            unitOfWork.taxRepository.DeleteTax(id);
            //SoftDelete(entity);
        }

        public TaxesDeleteModel GetDeleteModelOnError()
        {
            return new TaxesDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }



        public TaxesDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            try
            {
                var entity = Get(id);

                if (entity == null)
                {
                    return GetDeleteModelOnError();
                }


                return new TaxesDeleteModel
                {
                    Id = entity.Id,
                    Name = entity.Name,
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

        public Taxes Get(long id)
        {
            var cal = unitOfWork.taxRepository.GetTaxByTaxId(id); //new WebLunchCalendar { CalendarName = "temp", CalendarType = 1, DistrictID = 44, WebCalID = 5 };
            return cal; // GetAll().Where(x => x.ID == id).FirstOrDefault();
        }

        public IList<WeblunchCalendar> CalendarList()
        {
            var List = CalFactory.GetPreorderCalendarList(clientId);
            return List;

        }

        public void UpdateTaxSchools(string dataStr)
        {

            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] actualData = dataStr.Split('*');
                string[] schoolIds = actualData[1].Split(",".ToCharArray());

                if (actualData.Length == 2)
                {
                    string taxId = actualData[0].ToString();
                    taxId = taxId.Replace("schoolsList", "").Trim();
                    long inttaxId = Convert.ToInt64(taxId);
                    string schoolList = actualData[1].ToString();

                    //Fetch already assigned schools
                    List<SchoolTaxes> olstSchoolTaxes=unitOfWork.taxRepository.GetSchoolTaxByClientTaxID(clientId, inttaxId);

                    List<SchoolTaxes> olstSchoolTaxesToRemove = olstSchoolTaxes.Where(x => !schoolIds.Contains(x.SchoolId.ToString())).ToList();


                    //remove the deleted ones
                    for (int i = 0; i < olstSchoolTaxesToRemove.Count; i++)
                    {
                        unitOfWork.taxRepository.DeleteSchoolTaxByTaxId(olstSchoolTaxesToRemove[i].Id);
                    }

                    for (int i = 0; i < schoolIds.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(schoolIds[i]))
                        {
                            List<SchoolTaxes> olstSchoolTaxesToAdd = olstSchoolTaxes.Where(x => x.SchoolId.ToString() == schoolIds[i]).ToList();
                            if (olstSchoolTaxesToAdd != null && olstSchoolTaxesToAdd.Count <= 0)
                            {
                                unitOfWork.taxRepository.AddSchoolToTax(inttaxId, Convert.ToInt64(schoolIds[i]), clientId);
                            }
                        }
                    }
                }


            }

        }


        public void UpdateSchoolsTax(string dataStr, string schoolID)
        {

            //if (!string.IsNullOrEmpty(dataStr))
            //{
            //string[] actualData = dataStr.Split('*');
            string[] taxIds = dataStr.Split(",".ToCharArray());

            //if (actualData.Length == 2)
            //{
            //string taxId = actualData[0].ToString();
            //taxId = taxId.Replace("schoolsList", "").Trim();
            long lschoolID = Convert.ToInt64(schoolID);
            //string schoolList = actualData[1].ToString();

            //Fetch already assigned schools
            List<SchoolTaxes> olstSchoolTaxes = unitOfWork.taxRepository.GetSchoolTaxByClientSchoolID(clientId, lschoolID);

            List<SchoolTaxes> olstSchoolTaxesToRemove = olstSchoolTaxes.Where(x => !taxIds.Contains(x.TaxId.ToString())).ToList();


            //remove the deleted ones
            for (int i = 0; i < olstSchoolTaxesToRemove.Count; i++)
            {
                unitOfWork.taxRepository.DeleteSchoolTaxByTaxId(olstSchoolTaxesToRemove[i].Id);
            }

            for (int i = 0; i < taxIds.Length; i++)
            {
                if (!string.IsNullOrEmpty(taxIds[i]))
                {
                    List<SchoolTaxes> olstSchoolTaxesToAdd = olstSchoolTaxes.Where(x => x.TaxId.ToString() == taxIds[i]).ToList();
                    if (olstSchoolTaxesToAdd != null && olstSchoolTaxesToAdd.Count <= 0)
                    {
                        unitOfWork.taxRepository.AddSchoolToTax(Convert.ToInt64(taxIds[i]), lschoolID, clientId);
                    }
                }
            }
            //}


            //}

        }

        public TaxUpdateModel GetEditModel(int id)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetEditModelOnError();
            }

            return new TaxUpdateModel
            {
                Id = entity.Id,
                Name = entity.Name,
                TaxRate = entity.TaxRate,
                ClientId = entity.ClientId
            };
        }

        public TaxUpdateModel GetEditModelOnError()
        {
            return new TaxUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public TaxCreateModel GetCreateModel()
        {
            return new TaxCreateModel { };
        }

        public long UpdateTax(Taxes tax)
        {
            long retValue = -999;
            tax.ClientId = clientId;

            Taxes taxes = new Taxes();

            taxes = unitOfWork.taxRepository.EditTax(tax);
            //// Changed to allow for testing - NAH (11/6/2017)
            ////if (!MenuFactory.CalendarNameExists(wc.CalendarName, wc.DistrictID.ToString(), true))
            //if (!MenuFactory.CalendarNameExists(wc.CalendarName, wc.DistrictID.ToString()))
            //{
            //    helper2.ChangeCalendarName(wc.WebCalID, wc.DistrictID, wc.CalendarName);
            retValue = tax.Id;
            //}
            return retValue;
        }

        public long CreateNewTax(Taxes tax)
        {
            tax.ClientId = clientId;

            Taxes taxes = new Taxes();

            taxes = unitOfWork.taxRepository.AddTax(tax);
            //if (wc.DistrictID == 0)
            //{
            //    wc.DistrictID = Convert.ToInt16(clientId);
            //}
            long retValue = -999;
            //if (!MenuFactory.CalendarNameExists(wc.CalendarName, wc.DistrictID.ToString()))
            //{
            retValue = taxes.Id;
            //}
            //return retValue;
            return 0;

        }
    }
}
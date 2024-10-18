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
    public class HomeRoomController : BaseAuthorizedController
    {
        private HomeRoomHelper helper = new HomeRoomHelper();
        private SchoolHelper schoolHelper = new SchoolHelper();

        // Inayat [7-Sep-2016] - modified the flow. now both table and tile views won't load together, but on view type selection change.
        // GET: /Homeroom/
        public ActionResult Index()
        {
            if (Request.Cookies["homeroomView"] != null && Encryption.Decrypt(Request.Cookies["homeroomView"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table");
            }
            else
            {
                return RedirectToAction("Tile");
            }
        }

        // Inayat [7-Sep-2016] - modified the flow. now both table and tile views won't load together, but on view type selection change.
        public ActionResult Table()
        {
            if (!SecurityManager.viewHomerooms) return RedirectToAction("NoAccess", "Security", new { id = "nohomerooms" });
            Response.Cookies["homeroomView"].Value = Encryption.Encrypt("Table");

            return View();
        }

        // Inayat [7-Sep-2016] - modified the flow. now both table and tile views won't load together, but on view type selection change.
        public ActionResult Tile()
        {
            if (!SecurityManager.viewHomerooms) return RedirectToAction("NoAccess", "Security", new { id = "nohomerooms" });
            Response.Cookies["homeroomView"].Value = Encryption.Encrypt("Tile");

            ViewBag.ClientID = ClientInfoData.GetClientID();

            var vm = helper.GetIndexModel().OrderBy(x=> x.Name);
            return View(vm);
        }

        // GET: /Homeroom/Create
        public ActionResult Create()
        {
            var model = helper.GetCreateModel();

            return View("Create", model);
        }

        // POST: /Homeroom/Create
        [HttpPost]
        public ActionResult CreateHomeroom(string homeRoomName, string SchoolID)
        {
            string responseMessage = string.Empty;
            try
            {
                if (!helper.IsHomeroomExist(homeRoomName, 0))
                {
                    var entity = new Homeroom();
                   // entity.ID = -1;
                    entity.Name = homeRoomName;
                    entity.School_Id = Convert.ToInt32(SchoolID);
                    helper.Insert(entity);
                    responseMessage = "success";
                }
                else
                {
                    responseMessage = "duplicate";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "CreateHomeroom");
                responseMessage = "error";
            }

            return Json(responseMessage);
        }

        // GET: /Homeroom/Edit
        [HttpGet]
        public ActionResult Edit(int? id = 0)
        {
            var model = helper.GetEditModel(id.Value);

            if (id <= 0)
            {
                return new EmptyResult();
            }
            else
            {
                return View("Edit", model);
            }
        }

        // POST: /Homeroom/Edit
        [HttpPost]
        public ActionResult EditHomeroom(string homeroomId, string homeroomName, string schoolId)
        {
            string responseMessage = string.Empty;

            var entity = helper.Get(Convert.ToInt32(homeroomId));

            if (entity == null)
            {
                responseMessage = "error";
            }
            else
            {
                TryUpdateModel<Homeroom>(entity);
                try
                {
                    if (!helper.IsHomeroomExist(homeroomName, Convert.ToInt16(homeroomId)))
                    {
                        entity.Name = homeroomName;
                        entity.School_Id = Convert.ToInt32(schoolId);
                        helper.Update(entity);
                        responseMessage = "success";
                    }
                    else
                    {
                        responseMessage = "duplicate";
                    }
                }
                catch (Exception ex)
                {
                    //Error logging in cloud tables 
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditHomeroom");
                    responseMessage = "error";
                }
            }


            return Json(responseMessage);
        }

        // Load Delete Model
        public ActionResult Delete(int id = 0)
        {
            var hrDeletemodel = helper.GetDeleteModel(id);

            if (Request.IsAjaxRequest())
            {
                return Json(hrDeletemodel, JsonRequestBehavior.AllowGet);
            }

            if (hrDeletemodel.IsError)
            {
                return PartialView("_Delete", hrDeletemodel);
            }

            return RedirectToAction("Index");
        }

        // Delete Entity
        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id = 0)
        {
            var model = helper.GetDeleteModel(id);

            try
            {
                if (!model.IsError)
                {
                    string returnMessage = helper.Delete(id);
                    if (returnMessage.Equals("customerExist"))
                    {
                        model.Message = "This Homeroom can’t be deleted, some customers are referencing it.";
                        model.IsError = true;
                    }
                    else
                    {
                        model.Message = "The Homeroom record has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                model.IsError = true;
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }
            return Json(model, JsonRequestBehavior.AllowGet);
            //return new EmptyResult();
        }

        // ajax load
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            return GetGridJson(param);
        }

        // Homeroom Grid
        private ActionResult GetGridJson(JQueryDataTableParamModel param)
        {
            try
            {
                //var homerooms = helper.GetAll();
                var homerooms = helper.GetIndexModel();

                int totalRecordsCount = homerooms.Count();

                var sortDirection = Request["sSortDir_0"];
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                var filteredHomerooms = homerooms;
                if (param.iDisplayLength == -1)
                {
                    param.iDisplayLength = totalRecordsCount;
                }
                if (sortColumnIndex == 2)
                {
                    if (sortDirection == "desc")
                    {
                        filteredHomerooms = filteredHomerooms.OrderByDescending(x => x.SchoolName)
                        .Skip(param.iDisplayStart)
                        .Take(param.iDisplayLength);
                    }
                    else
                    {
                        filteredHomerooms = filteredHomerooms.OrderBy(x => x.SchoolName)
                                           .Skip(param.iDisplayStart)
                                           .Take(param.iDisplayLength);
                    }
                }
                else
                {
                    if (sortDirection == "desc")
                    {
                        filteredHomerooms = filteredHomerooms.OrderByDescending(x => x.Name)
                        .Skip(param.iDisplayStart)
                        .Take(param.iDisplayLength);
                    }
                    else
                    {
                        filteredHomerooms = filteredHomerooms.OrderBy(x => x.Name)
                                           .Skip(param.iDisplayStart)
                                           .Take(param.iDisplayLength);
                    }
                }


                var finalQuery = filteredHomerooms.Select(x => new { Id = SqlFunctions.StringConvert((double)x.Id).Trim(), Name = x.Name, SchoolName = x.SchoolName });
                // Inayat [19-Oct-2016] IQueryable cannot convert data into string array, therefore I need to convert the
                // result first into IEnumerable and the string array, because grid requires string array.
                var result = finalQuery.AsEnumerable().Select(x => new string[] { x.Id, x.Name, x.SchoolName });
                
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecordsCount,
                    iTotalDisplayRecords = totalRecordsCount,
                    aaData = result
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetGridJson");
                return null;
            }
        }
    }
}

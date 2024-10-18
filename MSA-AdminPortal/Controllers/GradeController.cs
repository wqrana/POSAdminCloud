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
    public class GradeController : BaseAuthorizedController
    {
        //
        // GET: /Grade/

        private GradesHelper helper = new GradesHelper();

        public ActionResult Index()
        {
            if (Request.Cookies["gradesView"] != null && Encryption.Decrypt(Request.Cookies["gradesView"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table");
            }
            else
            {
                return RedirectToAction("Tile");
            }
        }

        public ActionResult Table()
        {
            if (!SecurityManager.viewGrades) return RedirectToAction("NoAccess", "Security", new { id = "nogrades" });
            Response.Cookies["gradesView"].Value = Encryption.Encrypt("Table");

            return View();
        }

        public ActionResult Tile()
        {
            if (!SecurityManager.viewGrades) return RedirectToAction("NoAccess", "Security", new { id = "nogrades" });
            Response.Cookies["gradesView"].Value = Encryption.Encrypt("Tile");

            ViewBag.ClientID = ClientInfoData.GetClientID();
            var vm = helper.GetIndexModel();
            return View(vm);
        }

        // ajax load Grade Grid
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            return GetGridJson(param);
        }

        // Grade Grid
        private ActionResult GetGridJson(JQueryDataTableParamModel param)
        {
            try
            {
                var grades = helper.GetAllByQuery();
                int totalRecords = grades.Count();
                var filteredGrades = grades;
                var sortDirection = Request["sSortDir_0"];
                IQueryable<Repository.edmx.Grade> displayedGrades;
                if (param.iDisplayLength != -1)
                {
                    if (sortDirection == "desc")
                    {
                        displayedGrades = filteredGrades.OrderByDescending(x => x.Name)
                            .Skip(param.iDisplayStart)
                            .Take(param.iDisplayLength);
                    }
                    else
                    {
                        displayedGrades = filteredGrades.OrderBy(x => x.Name)
                                           .Skip(param.iDisplayStart)
                                           .Take(param.iDisplayLength);
                    }
                }
                else
                {
                    if (sortDirection == "desc")
                        displayedGrades = filteredGrades.OrderByDescending(x => x.Name);
                    else
                        displayedGrades = filteredGrades.OrderBy(x => x.Name);
                }

                var finalQuery = displayedGrades.Select(x => new { ID = SqlFunctions.StringConvert((double)x.ID).Trim(), Name = x.Name });
                var result = finalQuery.AsEnumerable().Select(x => new string[] { x.ID, x.Name });

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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeController", "Error : Garde data :: " + ex.Message, CommonClasses.getCustomerID(), "AjaxHandler");
                return null;
            }
        }

        // GET: /Grade/Create
        public ActionResult Create()
        {
            if (!SecurityManager.CreateGrades) return RedirectToAction("NoAccess", "Security", new { id = "nograde" });

                var model = helper.GetGradeCreateModel();
                return View("Create", model);

        }

        // POST: /Grare/Create
        [HttpPost]
        public ActionResult Create(string gradeName)
        {
            string responseMessage = string.Empty;
            var entity = new Repository.edmx.Grade();
            if (entity == null)
            {
                responseMessage = "error";
            }
            else
            {
                try
                {
                    if (!helper.IsGradeExist(gradeName, 0))
                    {
                        entity.Name = gradeName;
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
                    // Error = true;
                    //Error logging in cloud tables 
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeController", "Error : Garde create :: " + ex.Message, CommonClasses.getCustomerID(), "Create");
                    responseMessage = "error";
                }
            }

            return Json(responseMessage);
        }

        // GET: /Grade/Edit
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

        // POST: /Grade/Edit
        [HttpPost]
        public ActionResult Edit(string gradeId, string gradeName)
        {
            string responseMessage = string.Empty;

            var entity = helper.Get(Convert.ToInt32(gradeId));

            if (entity == null)
            {
                responseMessage = "error";
            }
            else
            {
                TryUpdateModel<Repository.edmx.Grade>(entity);
                try
                {
                    if(!helper.IsGradeExist(gradeName,Convert.ToInt32(gradeId)))
                    {
                        entity.Name = gradeName;
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
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeController", "Error : Garde edit :: " + ex.Message, CommonClasses.getCustomerID(), "Edit");
                    responseMessage = "error";
                }
            }


            return Json(responseMessage);
        }

        // Load Delete Model for grade
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

        // Delete Grade Entity
        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id = 0)
        {
            var model = helper.GetDeleteModel(id);

            try
            {
                if (!model.IsError)
                {
                    string msg = helper.Delete(id);
                    if (msg == "customeralreadyexist")
                    {
                        string message = "This grade can’t be deleted, some customers are referencing it.";
                        helper.SetErrors(model, message);
                    }
                    else
                    {
                        model.Message = "This grade record has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                model.IsError = true;
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeController", "Error : Garde delete :: " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

    }
}

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
using MSA_AdminPortal.App_Code;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MSA_AdminPortal.Controllers
{
    public class SchoolController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        private SchoolHelper schoolHelper = new SchoolHelper();
        private SchoolOptionHelper schoolOptionHelper = new SchoolOptionHelper();
        private DistrictHelper districtHelper = new DistrictHelper();


        public SchoolController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }


        // Inayat [2-Sep-2016] - modified the flow. now both table and tile views won't load together, but on view type selection change.
        // GET: /School/
        public ActionResult Index()
        {
            if (Request.Cookies["schoolView"] != null && MSA_AdminPortal.Encryption.Decrypt(Request.Cookies["schoolView"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table");
            }
            else
            {
                return RedirectToAction("Tile");
            }
        }

        // Inayat [2-Sep-2016] - modified the flow. now both table and tile views won't load together, but on view type selection change.
        // GET: /School/
        public ActionResult Table()
        {
            if (!SecurityManager.viewSchools) return RedirectToAction("NoAccess", "Security", new { id = "noschool" });
            Response.Cookies["schoolView"].Value = Encryption.Encrypt("Table");

            return View();
        }

        // Inayat [2-Sep-2016] - modified the flow. now both table and tile views won't load together, but on view type selection change.
        // GET: /School/
        public ActionResult Tile()
        {
            if (!SecurityManager.viewSchools) return RedirectToAction("NoAccess", "Security", new { id = "noschool" });
            Response.Cookies["schoolView"].Value = Encryption.Encrypt("Tile");

            ViewBag.ClientID = ClientInfoData.GetClientID();
            Session["DistrictIdSChool"] = 0;

            var vm = schoolHelper.GetIndexModel();
            return View(vm);
        }

        public ActionResult SchoolByDistrict(int DistrictId)
        {

            DistrictController dc = new DistrictController();
            string DistrictName = dc.helper.GetDistrictByName(DistrictId);

            Session["DistrictIdSChool"] = DistrictId;

            if (!SecurityManager.viewSchools) return RedirectToAction("NoAccess", "Security", new { id = "noschool" });
            if (Request.Cookies["schoolView"] != null && Encryption.Decrypt(Request.Cookies["schoolView"].Value).ToLower() == "table")
            {
                ViewBag.ViewType = "Table";

                return View("Table");
            }
            else
            {
                ViewBag.ViewType = "Tile";

                ViewBag.ClientID = ClientInfoData.GetClientID();
                Session["DistrictIdSChool"] = 0;

                var vm = schoolHelper.GetIndexModelByDistrict(DistrictId);
                return View("Tile", vm);

            }
        }



        // GET: /School/Create       
        public ActionResult Create()
        {
            var model = schoolHelper.GetEditModel(0);

            ViewBag.Title = "Create School";

            return View("Create", model);
        }

        // Load Delete Model
        public ActionResult Delete(int id = 0)
        {
            var schlDeletemodel = schoolHelper.GetDeleteModel(id);

            if (Request.IsAjaxRequest())
            {
                return Json(schlDeletemodel, JsonRequestBehavior.AllowGet);
            }

            if (schlDeletemodel.IsError)
            {
                return PartialView("_Delete", schlDeletemodel);
            }

            return RedirectToAction("Index");
        }

        // Delete Entity
        [HttpDelete]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id = 0)
        {
            var model = schoolHelper.GetDeleteModel(id);

            try
            {
                if (!model.IsError)
                {
                    if (model.customersExists)
                    {
                        schoolHelper.SetErrors(model, "This school currently has one or more customers associated with it. Reassign the customers to a different school before attempting to delete this one.");
                    }
                    else
                    {
                        unitOfWork.customSchoolRepository.DeleteSchool(id, ClientInfoData.GetClientID());
                        model.Message = "The school record has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                schoolHelper.SetErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }

            return Json(model, JsonRequestBehavior.AllowGet);
            //return new EmptyResult();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,District_Id,Emp_Director_Id,Emp_Administrator_Id,SchoolID,SchoolName,Address1,Address2,City,State,Zip,Phone1,Phone2,Comment,isSevereNeed, " +
            "UseDistDirAdmin,SchoolYearStartDate,SchoolYearEndDate,PhotoLogging,StripZeros,PinPreFix,DoPinPreFix,AlaCarteLimit,MealPlanLimit,BarCodeLength,TaxesList")] SchoolUpdateModel SchoolUpdate)
        {

            try
            {
                var isDeleted = false;
                if (ModelState.IsValid)
                {
                    if (ModelState.IsValid)
                    {

                        if (!schoolHelper.isSchoolExist(SchoolUpdate.SchoolName, 0))
                        {
                            if (!schoolHelper.isSchoolIDExist(SchoolUpdate.SchoolID, 0))
                            {


                                SchoolUpdateModel schoolUpdateModel = new SchoolUpdateModel();


                                schoolUpdateModel.ClientID = ClientInfoData.GetClientID();
                                schoolUpdateModel.Id = -1;


                                schoolUpdateModel.District_Id = SchoolUpdate.District_Id;
                                schoolUpdateModel.Emp_Director_Id = SchoolUpdate.Emp_Director_Id;
                                schoolUpdateModel.Emp_Administrator_Id = SchoolUpdate.Emp_Administrator_Id;
                                schoolUpdateModel.SchoolID = SchoolUpdate.SchoolID;
                                schoolUpdateModel.SchoolName = SchoolUpdate.SchoolName;
                                schoolUpdateModel.Address1 = SchoolUpdate.Address1;
                                schoolUpdateModel.Address2 = SchoolUpdate.Address2;
                                schoolUpdateModel.City = SchoolUpdate.City;
                                schoolUpdateModel.State = SchoolUpdate.State;
                                schoolUpdateModel.Zip = SchoolUpdate.Zip;
                                schoolUpdateModel.Phone1 = SchoolUpdate.Phone1;
                                schoolUpdateModel.Phone2 = SchoolUpdate.Phone2;
                                schoolUpdateModel.Comment = SchoolUpdate.Comment;
                                schoolUpdateModel.isSevereNeed = SchoolUpdate.isSevereNeed;
                                schoolUpdateModel.UseDistDirAdmin = SchoolUpdate.UseDistDirAdmin;

                                schoolUpdateModel.AlaCarteLimit = SchoolUpdate.AlaCarteLimit.HasValue ? SchoolUpdate.AlaCarteLimit : 0;
                                schoolUpdateModel.MealPlanLimit = (double)SchoolUpdate.MealPlanLimit;
                                schoolUpdateModel.PinPreFix = string.IsNullOrEmpty(SchoolUpdate.PinPreFix) ? string.Empty : SchoolUpdate.PinPreFix;
                                schoolUpdateModel.DoPinPreFix = SchoolUpdate.DoPinPreFix;
                                schoolUpdateModel.BarCodeLength = SchoolUpdate.BarCodeLength.HasValue ? SchoolUpdate.BarCodeLength : 0;
                                schoolUpdateModel.SchoolYearStartDate = SchoolUpdate.SchoolYearStartDate;
                                schoolUpdateModel.SchoolYearEndDate = SchoolUpdate.SchoolYearEndDate;
                                schoolUpdateModel.StripZeros = (bool)SchoolUpdate.StripZeros;
                                schoolUpdateModel.PhotoLogging = SchoolUpdate.PhotoLogging;


                                var result = schoolHelper.SaveScoolData(schoolUpdateModel);
                                if (result == "-1")
                                {
                                    schoolHelper.SetErrors(SchoolUpdate, ViewData);
                                }
                                else
                                {
                                    //save tax list
                                    SchoolUpdate.TaxesList = SchoolUpdate.TaxesList == null ? "" : SchoolUpdate.TaxesList;
                                    TaxesListHelper helper = new TaxesListHelper();
                                    helper.UpdateSchoolsTax(SchoolUpdate.TaxesList, result);
                                }

                                //SchoolUpdate.Id = entity.ID;
                            }
                            else
                            {
                                isDeleted = schoolHelper.isSchoolDeletedBySchoolId(SchoolUpdate.SchoolID, 0);
                                if (isDeleted == false)
                                {
                                    SchoolUpdate.ErrorMessage2 = "This School ID already exists in the system. Please try another School ID to create a school.";
                                }
                                else
                                {
                                    SchoolUpdate.ErrorMessage2 = "This School ID already exists in the system, however the status of this school is deleted. Please try another School ID to create a school.";
                                }
                                SchoolUpdate.IsError = true;
                            }
                        }
                        else
                        {
                            isDeleted = schoolHelper.isSchoolDeletedBySchoolName(SchoolUpdate.SchoolName, 0);
                            if (isDeleted == false)
                            {
                                SchoolUpdate.ErrorMessage2 = "This school name already exists in the system. Please use the name of a different school.";
                            }
                            else
                            {
                                SchoolUpdate.ErrorMessage2 = "This school name already exists in the system, however the status of this school is deleted. Please use the name of a different school.";
                            }
                            SchoolUpdate.IsError = true;
                        }
                    }
                    else
                    {
                        schoolHelper.SetErrors(SchoolUpdate, ViewData);
                    }
                }
                else
                {
                    schoolHelper.SetErrors(SchoolUpdate, ViewData);
                }

                return Json(SchoolUpdate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
                return null;
            }
        }

        // GET: /School/Edit
        public ActionResult Edit(int? id = 0)
        {
            var model = schoolHelper.GetEditModel(id.Value);

            if (id.Value == 0)
            {
                ViewBag.Title = "Create School";
            }
            else
            {
                ViewBag.Title = "Edit School: " + model.SchoolName;
            }

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,District_Id,Emp_Director_Id,Emp_Administrator_Id,SchoolID,SchoolName,Address1,Address2,City,State,Zip,Phone1,Phone2,Comment,isSevereNeed, " +
            "UseDistDirAdmin,SchoolYearStartDate,SchoolYearEndDate,PhotoLogging,StripZeros,PinPreFix,DoPinPreFix,AlaCarteLimit,MealPlanLimit,BarCodeLength,TaxesList")] SchoolUpdateModel SchoolUpdate)
        {
            try
            {
                var isDeleted = false;

                if (ModelState.IsValid)
                {
                    if (!schoolHelper.isSchoolExist(SchoolUpdate.SchoolName, SchoolUpdate.Id))
                    {
                        if (!schoolHelper.isSchoolIDExist(SchoolUpdate.SchoolID, SchoolUpdate.Id))
                        {
                            //var model = schoolHelper.Get(SchoolUpdate.Id);
                            //if (model == null)
                            //{
                            //    return View("Edit", schoolHelper.GetEditModelOnError());
                            //}
                            var model = new SchoolUpdateModel();
                            model.ClientID = ClientInfoData.GetClientID();
                            model.Id = SchoolUpdate.Id;
                            model.District_Id = SchoolUpdate.District_Id;
                            model.Emp_Director_Id = SchoolUpdate.Emp_Director_Id;
                            model.Emp_Administrator_Id = SchoolUpdate.Emp_Administrator_Id;
                            model.SchoolID = SchoolUpdate.SchoolID;
                            model.SchoolName = SchoolUpdate.SchoolName;
                            model.Address1 = SchoolUpdate.Address1;
                            model.Address2 = SchoolUpdate.Address2;
                            model.City = SchoolUpdate.City;
                            model.State = SchoolUpdate.State;
                            model.Zip = SchoolUpdate.Zip;
                            model.Phone1 = SchoolUpdate.Phone1;
                            model.Phone2 = SchoolUpdate.Phone2;
                            model.Comment = SchoolUpdate.Comment;
                            model.isSevereNeed = SchoolUpdate.isSevereNeed;
                            model.UseDistDirAdmin = SchoolUpdate.UseDistDirAdmin;
                            model.SchoolYearStartDate = SchoolUpdate.SchoolYearStartDate;
                            model.SchoolYearEndDate = SchoolUpdate.SchoolYearEndDate;
                            model.PhotoLogging = SchoolUpdate.PhotoLogging;
                            model.StripZeros = SchoolUpdate.StripZeros;
                            model.PinPreFix = SchoolUpdate.PinPreFix;
                            model.DoPinPreFix = SchoolUpdate.DoPinPreFix;
                            model.AlaCarteLimit = SchoolUpdate.AlaCarteLimit;
                            model.MealPlanLimit = SchoolUpdate.MealPlanLimit;
                            model.BarCodeLength = SchoolUpdate.BarCodeLength;

                            //TryUpdateModel<School>(model);

                            var result = schoolHelper.SaveScoolData(model);
                            if (result == "-1")
                            {
                                schoolHelper.SetErrors(SchoolUpdate, ViewData);
                            }
                            else
                            {
                                //save tax list
                                SchoolUpdate.TaxesList = SchoolUpdate.TaxesList == null ? "" : SchoolUpdate.TaxesList;
                                TaxesListHelper helper = new TaxesListHelper();
                                helper.UpdateSchoolsTax(SchoolUpdate.TaxesList, model.Id.ToString());
                                return Json(SchoolUpdate, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            isDeleted = schoolHelper.isSchoolDeletedBySchoolId(SchoolUpdate.SchoolID, SchoolUpdate.Id);
                            if (isDeleted == false)
                            {
                                SchoolUpdate.ErrorMessage2 = "This School ID already exists in the system. Please try another School ID to edit a school.";
                            }
                            else
                            {
                                SchoolUpdate.ErrorMessage2 = "This School ID already exists in the system, however the status of this school is deleted. Please try another School ID to edit a school.";
                            }
                            SchoolUpdate.IsError = true;
                        }
                    }
                    else
                    {
                        isDeleted = schoolHelper.isSchoolDeletedBySchoolName(SchoolUpdate.SchoolName, SchoolUpdate.Id);
                        if (isDeleted == false)
                        {
                            SchoolUpdate.ErrorMessage2 = "This school name already exists in the system. Please use the name of a different school.";
                        }
                        else
                        {
                            SchoolUpdate.ErrorMessage2 = "This school name already exists in the system, however the status of this school is deleted. Please use the name of a different school.";
                        }
                        SchoolUpdate.IsError = true;
                    }
                }
                else
                {
                    schoolHelper.SetErrors(SchoolUpdate, ViewData);
                }

                return Json(SchoolUpdate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
                return null;
            }
        }

        public string SetCookie(string viewType)
        {
            Response.Cookies["schoolView"].Value = Encryption.Encrypt(viewType);

            return "Success";
        }

        public ActionResult GetDistrictData(int id)
        {
            try
            {
                var Dist = districtHelper.Get(id);
                DistrictOption distOption = unitOfWork.DistrictOptionRepository.GetQuery(d => d.District_Id == id).FirstOrDefault();
                var stuff = new DistrictData();
                CustomerHelper customerHelper = new CustomerHelper();
                if (Dist.Emp_Director_Id.HasValue && Dist.Emp_Director_Id != null)
                {
                    var director = unitOfWork.CustomCustomerRepository.GetCustomer(ClientInfoData.GetClientID(), Dist.Emp_Director_Id);
                    stuff.DirectorId = Dist.Emp_Director_Id.ToString();
                    stuff.DirectorName = director.FirstName + " " + director.LastName;
                }
                else
                {
                    stuff.DirectorId = "";
                    stuff.DirectorName = "";
                }
                if (Dist.Emp_Administrator_Id.HasValue && Dist.Emp_Administrator_Id != null)
                {
                    var admin = unitOfWork.CustomCustomerRepository.GetCustomer(ClientInfoData.GetClientID(), Dist.Emp_Administrator_Id);
                    stuff.AdminId = Dist.Emp_Administrator_Id.ToString();
                    stuff.AdminName = admin.FirstName + " " + admin.LastName;
                }
                else
                {
                    stuff.AdminId = "";
                    stuff.AdminName = "";
                }

                if (distOption != null)
                {
                    stuff.StartDate = distOption.StartSchoolYear;
                    stuff.StartDateString = distOption.StartSchoolYear.Value.ToString("MM/dd/yyyy");
                }

                if (distOption != null)
                {
                    stuff.EndDate = distOption.EndSchoolYear;
                    stuff.EndDateString = distOption.EndSchoolYear.Value.ToString("MM/dd/yyyy");
                }

                //update Admins and Directors dropdownlist
                stuff.Admins = customerHelper.GetSelectListForDistrict(id, 0);
                stuff.Directors = stuff.Admins;

                return Json(stuff, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistrictData");
                return null;
            }
        }


        // ajax load
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            return GetGridJson(param);
        }

        // School Grid
        private ActionResult GetGridJson(JQueryDataTableParamModel param)
        {
            try
            {
                long clientId = ClientInfoData.GetClientID();
                //var schools = schoolHelper.GetAll().AsEnumerable();
                //var filteredSchools = schools;

                long districtID = 0;

                if (Session["DistrictIdSChool"] != null)
                {
                    districtID = Convert.ToInt64(Session["DistrictIdSChool"].ToString());
                }

                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"];
                string sortColumnName = getColmnName(sortColumnIndex);
                //IEnumerable<School> SchoolResult = null;
                IQueryable<School> Schools = null;
                if (districtID == 0)
                {
                    Schools = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId && (x.isDeleted == null || x.isDeleted == false));
                }
                else
                {
                    Schools = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId && x.District_Id == districtID && (x.isDeleted == null || x.isDeleted == false));
                }

                int totalRecords = Schools.Count();

                IQueryable<POS> POS = unitOfWork.POSRepository.GetQuery(x => x.ClientID == clientId);
                IQueryable<District> dist = unitOfWork.DistrictRepository.GetQuery(x => x.ClientID == clientId);

                IQueryable<SchoolAndPOSCount> SchoolResult = from s in Schools
                                                             join p in POS
                                                             on s.ID
                                                             equals (long?)p.School_Id into SchoolGroup
                                                             select new SchoolAndPOSCount
                                                             {
                                                                 ID = s.ID,
                                                                 idstr = SqlFunctions.StringConvert((double)s.ID),
                                                                 SchoolName = s.SchoolName,
                                                                 DistrictName = dist.Where(d => d.ID == s.District_Id).FirstOrDefault().DistrictName,
                                                                 POSCount = SchoolGroup.Count(),
                                                                 POSCountStr = SqlFunctions.StringConvert((double)SchoolGroup.Count())
                                                             };

                if (param.iDisplayLength != -1)
                {
                    if (sortDirection == "asc")
                    {
                        SchoolResult = SchoolResult.OrderBy(x => sortColumnIndex == 3 ? x.POSCountStr : sortColumnIndex == 2 ? x.DistrictName : x.SchoolName)
                                  .Skip(param.iDisplayStart)
                                  .Take(param.iDisplayLength);
                    }
                    else
                    {
                        SchoolResult = SchoolResult.OrderByDescending(x => sortColumnIndex == 3 ? x.POSCountStr : sortColumnIndex == 2 ? x.DistrictName : x.SchoolName)
                                  .Skip(param.iDisplayStart)
                                  .Take(param.iDisplayLength);
                    }

                }
                else
                {
                    if (sortDirection == "asc")
                    {
                        SchoolResult = SchoolResult.OrderBy(x => sortColumnIndex == 3 ? x.POSCountStr : sortColumnIndex == 2 ? x.DistrictName : x.SchoolName);
                    }
                    else
                    {
                        SchoolResult = SchoolResult.OrderByDescending(x => sortColumnIndex == 3 ? x.POSCountStr : sortColumnIndex == 2 ? x.DistrictName : x.SchoolName);
                    }
                }

                if (SchoolResult != null)
                {
                    var finalQuery = SchoolResult.Select(x => new { ID = SqlFunctions.StringConvert((double)x.ID).Trim(), SchoolName = x.SchoolName, DistrictName = x.DistrictName, POSCount = SqlFunctions.StringConvert((double)x.POSCount).Trim() });
                    // Inayat [19-Oct-2016] IQueryable cannot convert data into string array, therefore I need to convert the
                    // result first into IEnumerable and the string array, because grid requires string array.
                    var result = finalQuery.AsEnumerable().Select(x => new string[] { x.ID.ToString(), x.SchoolName, x.DistrictName, x.POSCount.ToString() });

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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetGridJson");
                return null;
            }


        }
        //Get Column name for sorting
        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 1:
                    retVal = "SchoolName";
                    break;
                case 2:
                    retVal = "DistrictName";
                    break;
                case 3:
                    retVal = "Count";
                    break;
                default:
                    retVal = "SchoolName";
                    break;
            }

            return retVal;
        }

    }

    public class DistrictData
    {
        public string DirectorId { get; set; }
        public string AdminId { get; set; }
        public string DirectorName { get; set; }
        public string AdminName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public IEnumerable<SelectListItem> Directors { get; set; }
        public IEnumerable<SelectListItem> Admins { get; set; }
    }


}

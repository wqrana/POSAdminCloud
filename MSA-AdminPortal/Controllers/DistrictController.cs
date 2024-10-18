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
using MSA_AdminPortal.App_Code;
using MSA_AdminPortal.Helpers;

namespace MSA_AdminPortal.Controllers
{
    public class DistrictController : BaseAuthorizedController
    {
        public DistrictHelper helper = new DistrictHelper();

        //
        // GET: /District/
        public ActionResult Index()
        {
            if (Request.Cookies["districtView"] != null && Encryption.Decrypt(Request.Cookies["districtView"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table");
            }

            return RedirectToAction("Tile");
        }

        public ActionResult Table()
        {
            if (!SecurityManager.viewDistricts) return RedirectToAction("NoAccess", "Security", new { id = "nodistrict" });
            Response.Cookies["districtView"].Value = Encryption.Encrypt("Table");

            return View();
        }

        //
        // GET: /District/
        public ActionResult Tile()
        {
            if (!SecurityManager.viewDistricts) return RedirectToAction("NoAccess", "Security", new { id = "nodistrict" });
            Response.Cookies["districtView"].Value = Encryption.Encrypt("Tile");

            var model = helper.GetIndexModel();
            return View(model);
        }

        // ajax load
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            return GetGridJson(param);
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
                    if (model.schoolsExists)
                    {
                        helper.SetErrors(model, "This district currently has one or more schools assigned to it. Reassign the schools to a different district before attempting to delete this one.");
                    }
                    else
                    {
                        helper.SoftDelete(id);
                        model.Message = "The district record has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
            }

            return GetActionResult(Request, model, false);
        }

        private ActionResult GetGridJson(JQueryDataTableParamModel param)
        {
            try
            {
                int totalRecords;
                long ClientId = ClientInfoData.GetClientID();
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc

                IEnumerable<DistricAndSchoolsCount> displayedDistricts = null;
                displayedDistricts = helper.GetDistrictPage(param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, out totalRecords);
                var result = displayedDistricts.Select(x => new string[] { x.dist.ID.ToString(), x.dist.ID.ToString(), x.dist.DistrictName, x.dist.Phone1, x.dist.Address1, x.dist.City, x.dist.State, "<a style='cursor:pointer; color:#000;' href=/School/SchoolByDistrict?DistrictId=" + x.dist.ID.ToString() + ">" + x.SchoolCount.ToString() + "</a>" });
                
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetGridJson");
                return null;
            }
        }

        private ActionResult GetActionResult(HttpRequestBase request, DistrictDeleteModel model, bool isGetAction = true)
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
    }

    public class DistrictHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        // for delete
        public void SetErrors(DistrictDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetErrors(DistrictDeleteModel model, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    model.Message += error.ErrorMessage + "<br />";
                }
            }

            model.IsError = true;
        }

        public IQueryable<District> GetAll()
        {
            try
            {
                //var disct = unitOfWork.DistrictRepository.GetQuery(x => (x.isDeleted.Equals(null) || !x.isDeleted.Value) && x.ClientID == clientId);
                //IEnumerable<District> res = disct.ToList();
                //return disct;
                return unitOfWork.DistrictRepository.GetQuery(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public bool AnySchoolsAttached(int id)
        {
            return unitOfWork.SchoolRepository.Get(s => s.District_Id == id && s.ClientID == clientId).Any();
        }


        public District Get(int id)
        {
            return GetAll().Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<DistrictIndexModel> GetIndexModel()
        {

            try
            {
                //IEnumerable<DistricAndSchoolsCount> districts = null;

                IQueryable<District> District = unitOfWork.DistrictRepository.GetQuery(d => d.ClientID == clientId);
                IQueryable<School> School = unitOfWork.SchoolRepository.GetQuery(s => s.ClientID == clientId);

                IQueryable<DistricAndSchoolsCount> districts = null;
                districts = from dist in District
                            join s in School
                            on dist.ID
                            equals s.District_Id into DistrictGroup
                            where dist.isDeleted==false
                            select new DistricAndSchoolsCount
                            {
                                dist = dist,
                                SchoolCount = DistrictGroup.Where(x=>x.isDeleted==false).Count()
                            };


                //districts = districts.Select(dis => new DistricAndSchoolsCount { dist = dis, SchoolCount = dis.Schools.Count });

                //return GetAll().OrderBy(o => o.DistrictName).ToList()
                var tempDistrict = districts
                 .Select(x =>
                      new DistrictIndexModel
                      {
                          Id = x.dist.ID,
                          Name = x.dist.DistrictName.Length <= 23 ? x.dist.DistrictName : x.dist.DistrictName.Substring(0, 23) + "...",
                          Phone = x.dist.Phone1,
                          Address = x.dist.Address1 != null ? x.dist.Address1.Length <= 23 ? x.dist.Address1 : x.dist.Address1.Substring(0, 23) + "..." : "",
                          Address2 = x.dist.Address2 != null ? x.dist.Address2.Length <= 23 ? x.dist.Address2 : x.dist.Address2.Substring(0, 23) + "..." : "",
                          City = x.dist.City != null ? x.dist.City.Length <= 23 ? x.dist.City : x.dist.City.Substring(0, 23) + "..." : "",
                          State = x.dist.State != null ? x.dist.State.Length <= 23 ? x.dist.State : x.dist.State.Substring(0, 23) + "..." : "",
                          ZipCode = x.dist.Zip != null ? x.dist.Zip : "",
                          schoolCount = x.SchoolCount
                      }).OrderBy(o => o.Name);

                return tempDistrict;
            }

            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }

        public DistrictDeleteModel GetDeleteModelOnError()
        {
            return new DistrictDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        public string GetMessageOnSchoolsAttached()
        {
            return "This district currently has one or more schools assigned to it. Reassign the schools to a different district before attempting to delete this one.";
        }


        // get
        public DistrictDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new DistrictDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }
        // post
        public DistrictDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            try
            {
                bool schoolsAttached = false;

                schoolsAttached = AnySchoolsAttached(id);

                var entity = Get(id);

                if (entity == null)
                {
                    return GetDeleteModelOnError();
                }


                return new DistrictDeleteModel
                {
                    Id = entity.ID,
                    Name = entity.DistrictName,
                    Message = errorMessage,
                    IsError = !string.IsNullOrWhiteSpace(errorMessage),
                    schoolsExists = schoolsAttached
                };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDeleteModel");
                return null;
            }
        }

        public void Update(District entity)
        {
            try
            {
                unitOfWork.DistrictRepository.Update(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Update");
            }
        }

        public void SoftDelete(int id = 0)
        {
            var entity = Get(id);

            SoftDelete(entity);
        }

        public void SoftDelete(District entity)
        {
            entity.isDeleted = true;

            Update(entity);
        }
        public IEnumerable<DistricAndSchoolsCount> GetDistrictPage(int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, out int totalRecords)
        {

            try
            {
                totalRecords = GetAll().Count();
                string columnName = getColmnName(sortColumnIndex);

                IQueryable<District> District = unitOfWork.DistrictRepository.GetQuery(d => d.ClientID == clientId);
                IQueryable<School> School = unitOfWork.SchoolRepository.GetQuery(s => s.ClientID == clientId);

                IQueryable<DistricAndSchoolsCount> districts = null;
                districts = from district in District
                            join s in School
                            on district.ID equals s.District_Id 
                            into DistrictGroup
                            where district.isDeleted == false
                            select new DistricAndSchoolsCount
                            {
                                dist = district,
                                DistrictName = district.DistrictName,
                                Phone1 = district.Phone1,
                                Address1 = district.Address1,
                                City = district.City,
                                State = district.State,

                                SchoolCount = DistrictGroup.Where(x=>x.isDeleted==false).Count()
                            };



                if (iDisplayLength == -1)
                {
                    iDisplayStart = 0;
                    iDisplayLength = 10000;

                }


                if (sortDirection == "asc")
                {
                    if (columnName == "SchoolCount")
                    {
                        districts = districts.OrderBy(d => d.SchoolCount).Skip(iDisplayStart).Take(iDisplayLength);
                    }
                    else
                    {
                        districts = districts.OrderBy(columnName).Skip(iDisplayStart).Take(iDisplayLength);
                    }
                }
                else
                {
                    if (columnName == "SchoolCount")
                    {
                        districts = districts.OrderByDescending(d => d.SchoolCount).Skip(iDisplayStart).Take(iDisplayLength);
                    }
                    else
                    {
                        districts = districts.OrderByDescending(columnName).Skip(iDisplayStart).Take(iDisplayLength);
                    }
                }


                return districts;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistrictPage");
                totalRecords = 0;
                return null;
            }
        }

        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 1:
                    retVal = "DistrictName";
                    break;
                case 2:
                    retVal = "Phone1";
                    break;
                case 3:
                    retVal = "Address1";
                    break;
                case 4:
                    retVal = "City";
                    break;
                case 5:
                    retVal = "State";
                    break;
                case 6:
                    retVal = "SchoolCount";
                    break;
                case 7:
                    retVal = "SchoolCount";
                    break;

                default:
                    retVal = "DistrictName";
                    break;
            }

            return retVal;
        }

        public string GetDistrictByName(int id)
        {
            string retStr = "";
            var dsname = GetAll().Where(x => x.ID == id).Select(x => x.DistrictName).FirstOrDefault();
            if (dsname != null)
            {
                retStr = dsname.ToString();
            }
            return retStr;
        }
    }
}
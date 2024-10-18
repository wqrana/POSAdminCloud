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
using System.Data.SqlTypes;
using MSA_AdminPortal.App_Code;

namespace MSA_AdminPortal.Helpers
{
    public class SchoolHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());


        public School Get(long id)
        {
            try
            {
                return GetAll().Where(x => x.ID == id && x.ClientID == clientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public School Get()
        {
            return new School { ClientID = clientId, isDeleted = false };
        }

        public IQueryable<School> GetAll()
        {
            try
            {

                //return unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId & x.isDeleted == false & x.District.isDeleted != true );
                return unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId);

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public bool AnyCustomersAttached(int id)
        {
            try
            {
                return unitOfWork.CustomersSchoolsRepository.Get(s => s.School_Id == id && s.ClientID == clientId).Any();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AnyCustomersAttached");
                return false;
            }
        }

        public IEnumerable<SelectListItem> GetSelectList(long id = 0)
        {
            try
            {
                return GetAll().AsEnumerable()
                       .Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.SchoolName, Selected = (x.ID == id) });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSelectList");
                return null;
            }
        }

        public IEnumerable<SchoolIndexModel> GetIndexModel()
        {
            try
            {
                IQueryable<School> schools = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId && (x.isDeleted == null || x.isDeleted == false));
                IQueryable<POS> pos = unitOfWork.POSRepository.GetQuery(x => x.ClientID == clientId);
                IQueryable<District> dist = unitOfWork.DistrictRepository.GetQuery(x => x.ClientID == clientId);

                var query = from s in schools
                            join p in pos
                            on s.ID
                            equals (long?)p.School_Id into schoolgroup
                            //where (s.District_Id == DistrictId && (s.isDeleted == true || s.isDeleted == null))
                            //where (s.isDeleted ==null || s.isDeleted == false)
                            select new SchoolIndexModel
                            {
                                Id = s.ID,
                                SchoolName = s.SchoolName,
                                POSCount = schoolgroup.Count(),
                                DistrictName = dist.Where(d => d.ID == s.District_Id).FirstOrDefault().DistrictName
                            };

                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }

        // Returns Error Edit Screen.
        public SchoolUpdateModel GetEditModelOnError()
        {
            return new SchoolUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        public SchoolUpdateModel GetEditModel(int id)
        {
            try
            {
                var districtHelper = new DistrictHelper();
                var customerHelper = new CustomerHelper();

                if (id == 0)
                {
                    var um = new SchoolUpdateModel();

                    um.States = unitOfWork.generalRepository.getStates().ToList();
                    um.Directors = customerHelper.GetSelectListForAdults();
                    um.Admins = customerHelper.GetSelectListForAdults();
                    um.Districts = districtHelper.GetSelectList().OrderBy(o => o.Text);
                    um.districtEmpDirectorName = "";
                    um.districtEmpAdminName = "";
                    um.UseDistDirAdmin = true;
                    um.BarCodeLengthList = getBarCodeLengthList().ToList();
                    um.districtEndDate = DateTime.Now.AddYears(-100);
                    um.districtStartDate = DateTime.Now.AddYears(-100);
                    um.Taxes = unitOfWork.taxRepository.GetTaxesByClientSchoolID(clientId, id);

                    return um;
                }
                else
                {

                    var entity = Get(id);

                    if (entity == null)
                    {
                        return GetEditModelOnError();
                    }
                    else
                    {
                        var SchoolOptions = unitOfWork.ScholOptionRepository.GetQuery(o => o.School_Id == id).FirstOrDefault();
                        var district = unitOfWork.DistrictRepository.GetQuery().Where(o => o.ID == entity.District_Id).FirstOrDefault();
                        var districtOptions = unitOfWork.DistrictOptionRepository.GetQuery().Where(o => o.ID == district.ID).FirstOrDefault();
                        var SchoolUpdatemodel = new SchoolUpdateModel();
                        SchoolUpdatemodel.ClientID = entity.ClientID;
                        SchoolUpdatemodel.Id = entity.ID;
                        SchoolUpdatemodel.District_Id = entity.District_Id;
                        SchoolUpdatemodel.Emp_Director_Id = (entity.Emp_Director_Id.HasValue ? 0 : entity.Emp_Director_Id);
                        SchoolUpdatemodel.Emp_Administrator_Id = (entity.Emp_Administrator_Id.HasValue ? 0 : entity.Emp_Administrator_Id);
                        SchoolUpdatemodel.SchoolID = entity.SchoolID;
                        SchoolUpdatemodel.SchoolName = entity.SchoolName;
                        SchoolUpdatemodel.DistrictName = district.DistrictName;
                        SchoolUpdatemodel.Address1 = entity.Address1;
                        SchoolUpdatemodel.Address2 = entity.Address2;
                        SchoolUpdatemodel.City = entity.City;
                        SchoolUpdatemodel.State = entity.State;
                        SchoolUpdatemodel.Zip = entity.Zip;
                        SchoolUpdatemodel.Phone1 = entity.Phone1;
                        SchoolUpdatemodel.Phone2 = entity.Phone2;
                        SchoolUpdatemodel.Comment = entity.Comment;
                        SchoolUpdatemodel.isSevereNeed = entity.isSevereNeed ?? false;
                        SchoolUpdatemodel.isDeleted = entity.isDeleted;
                        SchoolUpdatemodel.UseDistDirAdmin = entity.UseDistDirAdmin;
                        SchoolUpdatemodel.SchoolYearStartDate = SchoolOptions != null ? SchoolOptions.StartSchoolYear ?? DateTime.Now.AddYears(-50) : DateTime.Now.AddYears(-50);
                        SchoolUpdatemodel.SchoolYearEndDate = SchoolOptions != null ? SchoolOptions.EndSchoolYear ?? DateTime.Now.AddYears(-50) : DateTime.Now.AddYears(-50);
                        SchoolUpdatemodel.districtStartDate = districtOptions != null ? districtOptions.StartSchoolYear ?? DateTime.Now.AddYears(-50) : DateTime.Now.AddYears(-50);
                        SchoolUpdatemodel.districtEndDate = districtOptions != null ? districtOptions.EndSchoolYear ?? DateTime.Now.AddYears(-50) : DateTime.Now.AddYears(-50);
                        SchoolUpdatemodel.PhotoLogging = SchoolOptions != null && (SchoolOptions.PhotoLogging ?? false);
                        SchoolUpdatemodel.StripZeros = SchoolOptions != null && (SchoolOptions.StripZeros ?? false);
                        SchoolUpdatemodel.PinPreFix = SchoolOptions != null && (SchoolOptions.PinPreFix != null && SchoolOptions.PinPreFix!="") ? Convert.ToInt32(SchoolOptions.PinPreFix).ToString() : ""; // ignore left side zeros if any
                        SchoolUpdatemodel.DoPinPreFix = SchoolOptions != null && (SchoolOptions.DoPinPreFix ?? false);
                        SchoolUpdatemodel.AlaCarteLimit = SchoolOptions != null ? SchoolOptions.AlaCarteLimit : null;
                        SchoolUpdatemodel.MealPlanLimit = SchoolOptions != null ? (SchoolOptions.MealPlanLimit.HasValue) ? Convert.ToDouble(SchoolOptions.MealPlanLimit) : 0 : 0;
                        SchoolUpdatemodel.BarCodeLength = SchoolOptions != null ? SchoolOptions.BarCodeLength : null;
                        SchoolUpdatemodel.IsPinEnable = SchoolOptions != null && (SchoolOptions.BarCodeLength > 0);

                        SchoolUpdatemodel.districtEmpDirectorName = district.Emp_Director_Id == null ? string.Empty :
                        unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Director_Id).FirstName + " " + unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Director_Id).LastName;
                        SchoolUpdatemodel.districtEmpAdminName = district.Emp_Administrator_Id == null ? string.Empty :
                        unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Administrator_Id).FirstName + " " + unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Administrator_Id).LastName;

                        SchoolUpdatemodel.Districts = districtHelper.GetSelectList(entity.District_Id).OrderBy(o => o.Text);
                        SchoolUpdatemodel.States = unitOfWork.generalRepository.getStates().ToList();
                        SchoolUpdatemodel.Directors = customerHelper.GetSelectListForDistrict(entity.District_Id, entity.Emp_Director_Id == null ? 0 : (int)entity.Emp_Director_Id);
                        SchoolUpdatemodel.Admins = customerHelper.GetSelectListForDistrict(entity.District_Id, entity.Emp_Administrator_Id == null ? 0 : (int)entity.Emp_Administrator_Id);
                        SchoolUpdatemodel.BarCodeLengthList = getBarCodeLengthList().ToList();
                        SchoolUpdatemodel.Taxes = unitOfWork.taxRepository.GetTaxesByClientSchoolID(clientId, id);

                        //new SchoolUpdateModel
                        //{

                        //    ClientID = entity.ClientID,
                        //    Id = entity.ID,
                        //    District_Id = entity.District_Id,
                        //    Emp_Director_Id = (entity.Emp_Director_Id.HasValue ? 0 : entity.Emp_Director_Id),
                        //    Emp_Administrator_Id = (entity.Emp_Administrator_Id.HasValue ? 0 : entity.Emp_Administrator_Id),
                        //    SchoolID = entity.SchoolID,
                        //    SchoolName = entity.SchoolName,
                        //    DistrictName = district.DistrictName,
                        //    Address1 = entity.Address1,
                        //    Address2 = entity.Address2,
                        //    City = entity.City,
                        //    State = entity.State,
                        //    Zip = entity.Zip,
                        //    Phone1 = entity.Phone1,
                        //    Phone2 = entity.Phone2,
                        //    Comment = entity.Comment,
                        //    isSevereNeed = entity.isSevereNeed ?? false,
                        //    isDeleted = entity.isDeleted,
                        //    UseDistDirAdmin = entity.UseDistDirAdmin,
                        //    SchoolYearStartDate = SchoolOptions.StartSchoolYear == null ? DateTime.Now.AddYears(-50) : SchoolOptions.StartSchoolYear,
                        //    SchoolYearEndDate = SchoolOptions.EndSchoolYear == null ? DateTime.Now.AddYears(-50) : SchoolOptions.EndSchoolYear,
                        //    districtStartDate = (districtOptions.StartSchoolYear.HasValue) ? Convert.ToDateTime(districtOptions.StartSchoolYear) : DateTime.MinValue,
                        //    districtEndDate = (districtOptions.EndSchoolYear.HasValue) ? Convert.ToDateTime(districtOptions.EndSchoolYear) : DateTime.MinValue,
                        //    PhotoLogging = SchoolOptions.PhotoLogging ?? false,
                        //    StripZeros = SchoolOptions.StripZeros ?? false,
                        //    PinPreFix = SchoolOptions.PinPreFix,
                        //    DoPinPreFix = SchoolOptions.DoPinPreFix ?? false,
                        //    AlaCarteLimit = SchoolOptions.AlaCarteLimit,
                        //    MealPlanLimit = (double)SchoolOptions.MealPlanLimit,
                        //    BarCodeLength = SchoolOptions.BarCodeLength,
                        //    IsPinEnable = SchoolOptions.BarCodeLength > 0 ? true : false,

                        //    districtEmpDirectorName = district.Emp_Director_Id == null ? string.Empty :
                        //    unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Director_Id).FirstName + " " + unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Director_Id).LastName,
                        //    districtEmpAdminName = district.Emp_Administrator_Id == null ? string.Empty :
                        //    unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Administrator_Id).FirstName + " " + unitOfWork.CustomCustomerRepository.GetCustomer(clientId, district.Emp_Administrator_Id).LastName,

                        //    Districts = districtHelper.GetSelectList(entity.District_Id).OrderBy(o => o.Text),
                        //    States = unitOfWork.generalRepository.getStates().ToList(),
                        //    Directors = customerHelper.GetSelectListForDistrict(entity.District_Id, entity.Emp_Director_Id == null ? 0 : (int)entity.Emp_Director_Id),
                        //    Admins = customerHelper.GetSelectListForDistrict(entity.District_Id, entity.Emp_Administrator_Id == null ? 0 : (int)entity.Emp_Administrator_Id),
                        //    BarCodeLengthList = getBarCodeLengthList().ToList()
                        //};
                        return SchoolUpdatemodel;
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetEditModel");
                return null;
            }
        }

        public IEnumerable<SelectListItem> getBarCodeLengthList()
        {
            var BarCodeList = new List<SelectListItem>();
            for (int i = 1; i < 16; i++)
            {
                string tempStr = i.ToString();
                BarCodeList.Add(new SelectListItem { Text = tempStr, Value = tempStr });
            }

            return BarCodeList;
        }

        public string SaveScoolData(SchoolUpdateModel schooUpdateModel)
        {
            return unitOfWork.customSchoolRepository.SaveSchool(schooUpdateModel);
        }

        public void SetErrors(SchoolUpdateModel model, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    model.ErrorMessage2 += error.ErrorMessage + "\r\n";
                }
            }

            model.IsError = true;
        }

        public long GetNextId()
        {

            try
            {
                return unitOfWork.SchoolRepository.Get(x => x.ClientID == clientId).DefaultIfEmpty().Max(x => x == null ? 0 : x.ID) + 1;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetNextId");
                return 0;

            }
        }

        public bool isSchoolExist(string SchoolName, long schoolId)
        {
            try
            {
                if (unitOfWork.SchoolRepository.Get(e => e.ClientID == clientId && e.SchoolName.ToLower().Trim() == SchoolName.ToLower().Trim() && e.ID != schoolId).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "isSchoolExist");
                return false;
            }
        }

        public bool isSchoolIDExist(string schoolId, long autoSchoolId)
        {
            try
            {
                if (unitOfWork.SchoolRepository.Get(e => e.ClientID == clientId && e.SchoolID.ToLower().Trim() == schoolId.ToLower().Trim() && e.ID != autoSchoolId).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "isSchoolIDExist");
                return false;
            }
        }

        public bool isSchoolDeletedBySchoolId(string schoolId, long autoSchoolId)
        {
            try
            {
                if (unitOfWork.SchoolRepository.Get(e => e.ClientID == clientId && e.SchoolID.ToLower().Trim() == schoolId.ToLower().Trim() && e.isDeleted==true && e.ID != autoSchoolId).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "isSchoolDeletedBySchoolId");
                return false;
            }
        }

        public bool isSchoolDeletedBySchoolName(string schoolName, long autoSchoolId)
        {
            try
            {
                if (unitOfWork.SchoolRepository.Get(e => e.ClientID == clientId && e.SchoolName.ToLower().Trim() == schoolName.ToLower().Trim() && e.isDeleted == true && e.ID != autoSchoolId).Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "isSchoolDeletedBySchoolId");
                return false;
            }
        }


        public SchoolDeleteModel GetDeleteModelOnError()
        {
            return new SchoolDeleteModel
            {
                Id = -1,
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        public SchoolDeleteModel GetDeleteModel(int id = 0, string errorMessage = null)
        {

            try
            {
                bool customersAttached = false;

                if (id == 0)
                {
                    return GetDeleteModelOnError();
                }

                customersAttached = AnyCustomersAttached(id);

                var entity = Get(id);

                if (entity == null)
                {
                    return GetDeleteModelOnError();
                }

                return new SchoolDeleteModel
                {
                    Id = entity.ID,
                    Name = entity.SchoolName,
                    Message = errorMessage,
                    IsError = !string.IsNullOrWhiteSpace(errorMessage),
                    customersExists = customersAttached
                };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDeleteModel");
                return null;
            }
        }

        public void SetErrors(SchoolDeleteModel model, string p)
        {
            model.Message += p + "\r\n";
            model.IsError = true;
        }

        public bool Update(School entity)
        {
            try
            {
                unitOfWork.SchoolRepository.Update(entity);
                unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Update");
                return false;
            }
        }

        public bool Create(School entity)
        {
            try
            {
                //entity.District = unitOfWork.DistrictRepository.Get(x => x.ID == entity.District_Id && x.ClientID == entity.SchoolOption.ClientID).FirstOrDefault();
                unitOfWork.SchoolRepository.Insert(entity);
                unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
                return false;
            }
        }

        #region Munawar
        public IEnumerable<SchoolIndexModel> GetIndexModelByDistrict(int DistrictId)
        {
            try
            {
                IQueryable<School> Schools = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId && (x.isDeleted == null || x.isDeleted == false));
                IQueryable<POS> POS = unitOfWork.POSRepository.GetQuery(x => x.ClientID == clientId);
                IQueryable<District> dist = unitOfWork.DistrictRepository.GetQuery(x => x.ClientID == clientId);

                IQueryable<SchoolIndexModel> SchoolResult = from s in Schools
                                                            join p in POS
                                                            on s.ID
                                                            equals (long?)p.School_Id into SchoolGroup
                                                            where (s.District_Id == DistrictId)
                                                            select new SchoolIndexModel
                                                             {
                                                                 Id = s.ID,
                                                                 SchoolName = s.SchoolName,
                                                                 DistrictName = dist.Where(d => d.ID == s.District_Id).FirstOrDefault().DistrictName,
                                                                 POSCount = SchoolGroup.Count(),
                                                                 District_Id = s.District_Id
                                                             };
                return SchoolResult;

                //return GetAll()
                //   .OrderBy(o => o.SchoolName)
                //     .Select(x =>
                //         new SchoolIndexModel
                //         {
                //             Id = x.ID,
                //             SchoolName = x.SchoolName,
                //             POSCount = x.POS.Count,
                //             DistrictName = x.District.DistrictName,
                //             District_Id=x.District_Id
                //}).Where(x => x.District_Id == DistrictId);


            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }
        #endregion
    }
}
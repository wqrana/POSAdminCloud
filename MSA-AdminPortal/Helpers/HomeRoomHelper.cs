using Repository;
using Repository.edmx;
using Repository.Helpers;
using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.SqlServer;




namespace MSA_AdminPortal.Helpers
{
    public class HomeRoomHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        public Homeroom Get(int id)
        {
            return GetAll().Where(x => x.ID == id && x.ClientID == clientId).FirstOrDefault();
        }

        public IEnumerable<Homeroom> GetAll()
        {
            try
            {
                var HomeRoomData = unitOfWork.HomeroomRepository.Get(x => x.ClientID == clientId);
                return HomeRoomData;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public IQueryable<Homeroom> GetAllByQuery()
        {
            try
            {
                var HomeRoomData = unitOfWork.HomeroomRepository.GetQuery(x => x.ClientID == clientId);
                return HomeRoomData;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAllByQuery");
                return null;
            }
        }

        public bool IsHomeroomExist(string HomeroomName, int homeroomId)
        {
            try
            {
                if (unitOfWork.HomeroomRepository.Get(e => e.ClientID == clientId && e.Name == HomeroomName && e.ID != homeroomId).Count() > 0)
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "IsHomeroomExist");
                return false;
            }
        }

        public IEnumerable<Homeroom> GetAllBySchool(Int64 School_Id)
        {
            try
            {
                if (School_Id != 0)
                {
                    return unitOfWork.HomeroomRepository.Get(x => (x.ClientID == clientId && x.School_Id == School_Id));
                }
                else
                {
                    return unitOfWork.HomeroomRepository.Get(x => (x.ClientID == clientId));
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }

        public IEnumerable<SelectListItem> GetSelectList(long homeroomSelected, long School_Id)
        {
            try
            {
                return GetAllBySchool(School_Id)
                        .OrderBy(o => o.Name)
                        .Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name, Selected = (x.ID == homeroomSelected) });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }

        public IEnumerable<SelectListItem> GetSelectList(int homeroomSelected)
        {
            try
            {
                return GetAllByQuery()
                        .OrderBy(o => o.Name)
                        .Select(x => new SelectListItem { Value = SqlFunctions.StringConvert((double)x.ID).Trim(), Text = x.Name, Selected = (x.ID == homeroomSelected) });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSelectList");
                return null;
            }
        }

        public IQueryable<HomeRoomIndexModel> GetIndexModel()
        {
            try
            {
                IQueryable<School> school = unitOfWork.SchoolRepository.GetQuery(o => o.ClientID == clientId);
                IQueryable<Homeroom> homeroom = unitOfWork.HomeroomRepository.GetQuery(o => o.ClientID == clientId);

                var query = from s in school
                            join h in homeroom on new {s.ID } equals new { ID = h.School_Id }
                            select (
                           new HomeRoomIndexModel
                           {
                               Id = h.ID,
                               School_Id = s.ID,
                               Name = h.Name,
                               SchoolName = (s.SchoolName == null) ? "" : s.SchoolName,
                           });

                return query;
                
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }

        // returns Create Screen
        public HomeRoomCreateModel GetCreateModel()
        {
            var schoolHelper = new SchoolHelper();

            return new HomeRoomCreateModel
            {
                Schools = schoolHelper.GetSelectList().OrderBy(o => o.Text),
            };
        }

        // Returns Error Edit Screen.
        public HomeRoomUpdateModel GetEditModelOnError()
        {
            return new HomeRoomUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // Checks and then returns the Edit Screen for Homeroom with id.
        public HomeRoomUpdateModel GetEditModel(int id)
        {
            var schoolHelper = new SchoolHelper();

            if (id == 0)
            {
                var um = new HomeRoomUpdateModel();

                um.Schools = schoolHelper.GetSelectList().OrderBy(o => o.Text);
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
                    return new HomeRoomUpdateModel
                    {
                        Id = entity.ID,
                        Name = entity.Name,
                        School_Id = entity.School_Id,
                        Schools = schoolHelper.GetSelectList(entity.School_Id).OrderBy(o => o.Text),
                    };
                }
            }
        }

        public HomeRoomDeleteModel GetDeleteModelOnError()
        {
            return new HomeRoomDeleteModel
            {
                Id = -1,
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        public HomeRoomDeleteModel GetDeleteModel(int id = 0, string errorMessage = null)
        {
            if (id == 0)
            {
                return GetDeleteModelOnError();
            }

            var entity = Get(id);

            if (entity == null)
            {
                return GetDeleteModelOnError();
            }

            return new HomeRoomDeleteModel
            {
                Id = entity.ID,
                Name = entity.Name,
                Message = errorMessage,
                IsError = !string.IsNullOrWhiteSpace(errorMessage),
            };
        }

        public void SetErrors(HomeRoomCreateModel hrModel, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    hrModel.Message += error.ErrorMessage + "\r\n";
                }
            }

            hrModel.IsError = true;
        }

        public void SetErrors(HomeRoomUpdateModel hrModel, System.Web.Mvc.ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    hrModel.Message += error.ErrorMessage + "\r\n";
                }
            }

            hrModel.IsError = true;
        }

        public void SetErrors(HomeRoomDeleteModel model, string p)
        {
            model.Message += p + "\r\n";
        }

        public void Insert(Homeroom entity)
        {
            try
            {
                entity.ClientID = clientId;
                unitOfWork.HomeroomRepository.Insert(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Insert");
            }
        }

        public void Update(Homeroom entity)
        {
            try
            {
                entity.ClientID = clientId;
                unitOfWork.HomeroomRepository.Update(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Update");
            }
        }

        public string Delete(int id)
        {
            try
            {
                var entity = Get(id);
                IQueryable<Customer> Customer = unitOfWork.CustomerRepository.GetQuery( x=> x.ClientID == clientId);
                Int32 CustomerCount = Customer.Where(c => c.Homeroom_Id == id).Count();
                if (CustomerCount > 0)
                {
                    return "customerExist";
                }
                else
                {
                    unitOfWork.HomeroomRepository.Delete(entity);
                    unitOfWork.Save();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeRoomHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Delete");
                return "";
            }
        }
    }
}
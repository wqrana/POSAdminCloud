using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Repository;
using Repository.edmx;
using Repository.Helpers;
using AdminPortalModels.Models;
using System.Web.Mvc;

namespace MSA_AdminPortal.Helpers
{
    public class GradesHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        /// <summary>
        /// Method for get single Grade Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Grade Get(int id)
        {
            try
            {
                return GetAll().Where(x => x.ID == id && x.ClientID == clientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;     
            }
        }

        /// <summary>
        /// Method for get all GradeList 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Grade> GetAll()
        {
            try
            {
                return unitOfWork.GradeRepository.Get(x => x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");             
                return null;
            }
        }

        /// <summary>
        /// Method returns query for getting all GradeList 
        /// </summary>
        /// <returns></returns>
        public IQueryable<Grade> GetAllByQuery()
        {
            try
            {
                return unitOfWork.GradeRepository.GetQuery(x => x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAllByQuery");
                return null;
            }
        }

        /// <summary>
        /// Get Index page Model
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GradeModels> GetIndexModel()
        {
            try
            {
                return GetAllByQuery()
                        .OrderBy(o => o.Name)
                        .Select(x =>
                            new GradeModels
                            {
                                ClientID = x.ClientID,
                                Id = x.ID,
                                Name = x.Name,
                            });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                 ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                 return null;
            }
        }

        /// <summary>
        /// Method for get Next Grade ID
        /// </summary>
        /// <returns></returns>
        public long GetGradeNextID()
        {
            try
            {
                int GradesCount = unitOfWork.GradeRepository.Get(x => x.ClientID == clientId).Count();
                if (GradesCount > 0)
                    return unitOfWork.GradeRepository.Get(x => x.ClientID == clientId).Max(x => x.ID) + 1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetGradeNextID");
                return 0;
            }

        }

        /// <summary>
        /// Method for get Grade Create Model
        /// </summary>
        /// <returns></returns>
        public GradeCreateModel GetGradeCreateModel()
        {
            return new GradeCreateModel();
        }

        /// <summary>
        /// Method to insert Grade
        /// </summary>
        /// <param name="entity"></param>
        public void Insert(Grade entity)
        {
            try
            {
                entity.ClientID = clientId;
               // entity.ID = GetGradeNextID(); by Farrukh (03-mar-2016)
                unitOfWork.GradeRepository.Insert(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Insert");               
            }
        }

        /// <summary>
        /// Update the Grade item
        /// </summary>
        /// <param name="entity"></param>
        public void Update(Grade entity)
        {
            try
            {
                entity.ClientID = clientId;
                unitOfWork.GradeRepository.Update(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                 ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Update");
            }
        }

        /// <summary>
        /// Method to set errors to create model
        /// </summary>
        /// <param name="gradeModel"></param>
        /// <param name="viewData"></param>
        public void SetErrors(GradeCreateModel gradeModel, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    gradeModel.Message += error.ErrorMessage + "\r\n";
                }
            }

            gradeModel.IsError = true;
        }

        /// <summary>
        /// Set errors for grades helper
        /// </summary>
        /// <param name="gradeModel"></param>
        /// <param name="viewData"></param>
        public void SetErrors(GradeUpdateModel gradeModel, System.Web.Mvc.ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    gradeModel.Message += error.ErrorMessage + "\r\n";
                }
            }

            gradeModel.IsError = true;
        }

        /// <summary>
        /// Set Error to model for Grade Delete model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="p"></param>
        public void SetErrors(GradeDeleteModel model, string p)
        {
            model.Message += p + "\r\n";
            model.IsError = true;
        }


        /// <summary>
        /// Checks and then returns the Edit Screen for Grades with id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GradeUpdateModel GetEditModel(int id)
        {
            try
            {
                if (id == 0)
                {
                    var um = new GradeUpdateModel();
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
                        return new GradeUpdateModel
                        {
                            Id = entity.ID,
                            Name = entity.Name,
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDeleteModel");
                return null;
            }
        }

        /// <summary>
        /// Returns Error Edit Screen. 
        /// </summary>
        /// <returns></returns>
        public GradeUpdateModel GetEditModelOnError()
        {
            return new GradeUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        /// <summary>
        /// Get delete error model
        /// </summary>
        /// <returns></returns>
        public GradeDeleteModel GetDeleteModelOnError()
        {
            return new GradeDeleteModel
            {
                Id = -1,
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        /// <summary>
        /// Method to get delete model
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public GradeDeleteModel GetDeleteModel(int id = 0, string errorMessage = null)
        {
            try
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

                return new GradeDeleteModel
                {
                    Id = entity.ID,
                    Name = entity.Name,
                    Message = errorMessage,
                    IsError = !string.IsNullOrWhiteSpace(errorMessage),
                };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDeleteModel");
                return null;
            }
        }

        /// <summary>
        /// Method to delete grade by id 
        /// </summary>
        /// <param name="id"></param>
        public string Delete(int id)
        {
            try
            {
                int CustomersCount = unitOfWork.CustomerRepository.Get(c => c.Grade_Id == id && c.isDeleted != true && c.ClientID == clientId).Count();
                if (CustomersCount > 0)
                {
                    return "customeralreadyexist";
                }
                else
                {
                    var entity = Get(id);
                    unitOfWork.GradeRepository.Delete(entity);
                    unitOfWork.Save();
                    return "deleted";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GradeHelper\\Delete", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Delete");
                return "";
            }

        }
        /// <summary>
        /// Method to check if grade already exist ()
        /// </summary>
        /// <param name="gradeName">Name of Grade</param>
        /// /// <param name="gradeName">Grade Id</param>
        public bool IsGradeExist(string gradeName, int gradeId)
        {
            try
            {
                if (unitOfWork.GradeRepository.Get(e => e.ClientID == clientId && e.Name == gradeName && e.ID != gradeId).Count() > 0)
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


    }
}
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

namespace MSA_AdminPortal.Helpers
{
    public class POSNotificationsHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork;

        public POSNotificationsHelper()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

        public IList<POSNotificationsViewModel> GetPOSNotifications(long ClientId)
        {
            return unitOfWork.posNotificationsRepository.GetAllPOSNotifications(clientId);
        }
        public POSNotificationsDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new POSNotificationsDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }

        public void SetErrors(POSNotificationsDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        // for create/update
        public void SetErrors(POSNotificationsViewModel model, ViewDataDictionary viewData)
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
        public void DeletePosNotification(long id = 0)
        {

            //var entity = Get(id);
            unitOfWork.posNotificationsRepository.DeletePOSNotification(id);
            //SoftDelete(entity);
        }

        public POSNotificationsDeleteModel GetDeleteModelOnError()
        {
            return new POSNotificationsDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }



        public POSNotificationsDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            try
            {
                var entity = Get(id);

                if (entity == null)
                {
                    return GetDeleteModelOnError();
                }


                return new POSNotificationsDeleteModel
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

        public POSNotificationsViewModel Get(long id)
        {
            var pOSNotificationsViewModel = unitOfWork.posNotificationsRepository.GetPOSNotificationById(id);
            return pOSNotificationsViewModel;
        }

        public POSNotificationsUpdateModel GetEditModel(int id)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetEditModelOnError();
            }

            return new POSNotificationsUpdateModel
            {
                Id = entity.Id,
                Name = entity.Name,
                ClientID = entity.ClientID,
                Code = entity.Code,
                Description = entity.Description,
                BackColor = entity.BackColor,
                TextColor = entity.TextColor
            };
        }

        public POSNotificationsUpdateModel GetEditModelOnError()
        {
            return new POSNotificationsUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public POSNotificationsCreateModel GetCreateModel()
        {
            return new POSNotificationsCreateModel { };
        }

        public long UpdatePOSNotification(POSNotificationsViewModel posNotification)
        {
            long retValue = -999;
            posNotification.ClientID = clientId;

            posNotification = unitOfWork.posNotificationsRepository.EditPOSNotification(posNotification);

            retValue = posNotification.Id;

            return retValue;
        }

        public long CreateNewPOSNotification(POSNotificationsViewModel posNotification)
        {
            posNotification.ClientID = clientId;


            posNotification = unitOfWork.posNotificationsRepository.AddPOSNotification(posNotification);
            //if (wc.DistrictID == 0)
            //{
            //    wc.DistrictID = Convert.ToInt16(clientId);
            //}
            long retValue = -999;
            //if (!MenuFactory.CalendarNameExists(wc.CalendarName, wc.DistrictID.ToString()))
            //{
            retValue = posNotification.Id;
            //}
            //return retValue;
            return 0;

        }


        public void AddNotificationToCustomer(string dataStr)
        {

            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] actualData = dataStr.Split('*');
                string[] customerIds = actualData[1].Split(",".ToCharArray());

                if (actualData.Length == 2)
                {
                    string posNotificationId = actualData[0].ToString();
                    long intposNotificationId = Convert.ToInt64(posNotificationId);
                    string schoolList = actualData[1].ToString();

                    //Fetch already assigned notifications
                    List<CustomerPOSNotificationViewModel> olstCustomerPOSNotificationViewModel = unitOfWork.posNotificationsRepository.GetCustomerPOSNotificationByClientandNotificationID(clientId, intposNotificationId);

                    List<CustomerPOSNotificationViewModel> olstCustomerPOSNotificationViewModelToRemove = olstCustomerPOSNotificationViewModel.Where(x => !customerIds.Contains(x.Customer_Id.ToString())).ToList();


                    //remove the deleted ones
                    for (int i = 0; i < olstCustomerPOSNotificationViewModelToRemove.Count; i++)
                    {
                        unitOfWork.posNotificationsRepository.DeleteCustomerNotificationById(olstCustomerPOSNotificationViewModelToRemove[i].ID);
                    }

                    for (int i = 0; i < customerIds.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(customerIds[i]))
                        {
                            List<CustomerPOSNotificationViewModel> olstCustomerPOSNotificationViewModelToAdd = olstCustomerPOSNotificationViewModel.Where(x => x.Customer_Id.ToString() == customerIds[i]).ToList();
                            if (olstCustomerPOSNotificationViewModelToAdd != null && olstCustomerPOSNotificationViewModelToAdd.Count <= 0)
                            {
                                unitOfWork.posNotificationsRepository.AddCustomerNotification(intposNotificationId, Convert.ToInt64(customerIds[i]), clientId);
                            }
                        }
                    }
                }
            }
        }

        public void AddCustomerToNotificaion(string dataStr)
        {

            if (!string.IsNullOrEmpty(dataStr))
            {
                string[] actualData = dataStr.Split('*');
                string[] posNotificationIds = actualData[1].Split(",".ToCharArray());

                if (actualData.Length == 2)
                {
                    string customerId = actualData[0].ToString();
                    long intCustomerId = Convert.ToInt64(customerId);
                    string schoolList = actualData[1].ToString();

                    //Fetch already assigned notifications
                    List<CustomerPOSNotificationViewModel> olstCustomerPOSNotificationViewModel = unitOfWork.posNotificationsRepository.GetCustomerPOSNotificationByClientandCustomerID(clientId, intCustomerId);

                    List<CustomerPOSNotificationViewModel> olstCustomerPOSNotificationViewModelToRemove = olstCustomerPOSNotificationViewModel.Where(x => !posNotificationIds.Contains(x.POSNotification_Id.ToString())).ToList();


                    //remove the deleted ones
                    for (int i = 0; i < olstCustomerPOSNotificationViewModelToRemove.Count; i++)
                    {
                        unitOfWork.posNotificationsRepository.DeleteCustomerNotificationById(olstCustomerPOSNotificationViewModelToRemove[i].ID);
                    }

                    for (int i = 0; i < posNotificationIds.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(posNotificationIds[i]))
                        {
                            List<CustomerPOSNotificationViewModel> olstCustomerPOSNotificationViewModelToAdd = olstCustomerPOSNotificationViewModel.Where(x => x.POSNotification_Id.ToString() == posNotificationIds[i]).ToList();
                            if (olstCustomerPOSNotificationViewModelToAdd != null && olstCustomerPOSNotificationViewModelToAdd.Count <= 0)
                            {
                                unitOfWork.posNotificationsRepository.AddCustomerNotification(Convert.ToInt64(posNotificationIds[i]), intCustomerId, clientId);
                            }
                        }
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.edmx;
using Repository.Helpers;
using AdminPortalModels.ViewModels;
using System.Data.Entity;

namespace Repository
{
    public class POSNotificationsRepository : IPOSNotificationsRepository, IDisposable
    {
        private PortalContext context;
        private bool disposed = false;

        public POSNotificationsRepository(PortalContext context)
        {
            this.context = context;


        }
        public List<POSNotificationsViewModel> GetAllPOSNotifications(long ClientId)
        {
            List<POSNotificationsViewModel> olstPOSNotificationsViewModel = new List<POSNotificationsViewModel>();
            try
            {
                var olstPOSNotifications = context.POSNotifications.Where(x => x.ClientID == ClientId && x.IsDeleted == false);


                foreach (var item in olstPOSNotifications)
                {
                    POSNotificationsViewModel oPOSNotificationsViewModel = new POSNotificationsViewModel();

                    oPOSNotificationsViewModel.ClientID = item.ClientID;
                    oPOSNotificationsViewModel.BackColor = item.BackColorS;
                    oPOSNotificationsViewModel.TextColor = item.TextColorS;
                    oPOSNotificationsViewModel.Description = item.Description;
                    oPOSNotificationsViewModel.Code = item.Code;
                    oPOSNotificationsViewModel.Id = item.Id;
                    oPOSNotificationsViewModel.Name = item.Name;
                    oPOSNotificationsViewModel.CustomerPOSNotificationPSV = context.Customer_POSNotification.Where(x => x.POSNotification_Id == item.Id && x.ClientID == item.ClientID) != null ? string.Join("|", context.Customer_POSNotification.Where(x => x.POSNotification_Id == item.Id && x.ClientID == item.ClientID).Select(y => y.Customer_Id).ToArray()) : "";

                    olstPOSNotificationsViewModel.Add(oPOSNotificationsViewModel);
                }


                return olstPOSNotificationsViewModel;

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public POSNotificationsViewModel AddPOSNotification(POSNotificationsViewModel pOSNotificationsViewModel)
        {
            try
            {
                POSNotifications pOSNotification = new POSNotifications();

                pOSNotification.ClientID = pOSNotificationsViewModel.ClientID;
                pOSNotification.BackColorS = pOSNotificationsViewModel.BackColor;
                pOSNotification.Code = pOSNotificationsViewModel.Code;
                pOSNotification.Description = pOSNotificationsViewModel.Description;
                pOSNotification.Name = pOSNotificationsViewModel.Name;
                pOSNotification.TextColorS = pOSNotificationsViewModel.TextColor;
                pOSNotification.TextColor = string.IsNullOrEmpty(pOSNotificationsViewModel.TextColor) ? (int?)null : int.Parse(pOSNotificationsViewModel.TextColor.TrimStart('#'), System.Globalization.NumberStyles.HexNumber);
                pOSNotification.BackColor = string.IsNullOrEmpty(pOSNotificationsViewModel.BackColor) ? (int?)null : int.Parse(pOSNotificationsViewModel.BackColor.TrimStart('#'), System.Globalization.NumberStyles.HexNumber);
                pOSNotification.LastUpdatedUTC = DateTime.UtcNow;

                context.POSNotifications.Add(pOSNotification);
                
                context.SaveChanges();
                

                pOSNotificationsViewModel.Id = pOSNotification.Id;

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AddPOSNotification");
                return null;
            }
            return pOSNotificationsViewModel;

        }

        public POSNotificationsViewModel EditPOSNotification(POSNotificationsViewModel pOSNotificationsViewModel)
        {
            try
            {
                POSNotifications oPOSNotifications = context.POSNotifications.FirstOrDefault(x => x.Id == pOSNotificationsViewModel.Id);

                if (oPOSNotifications != null)
                {
                    oPOSNotifications.Name = pOSNotificationsViewModel.Name;
                    oPOSNotifications.Code = pOSNotificationsViewModel.Code;
                    oPOSNotifications.Description = pOSNotificationsViewModel.Description;
                    oPOSNotifications.TextColorS = pOSNotificationsViewModel.TextColor;
                    oPOSNotifications.BackColorS = pOSNotificationsViewModel.BackColor;
                    oPOSNotifications.TextColor = string.IsNullOrEmpty(pOSNotificationsViewModel.TextColor) ? (int?)null : int.Parse(pOSNotificationsViewModel.TextColor.TrimStart('#'), System.Globalization.NumberStyles.HexNumber);
                    oPOSNotifications.BackColor = string.IsNullOrEmpty(pOSNotificationsViewModel.BackColor) ? (int?)null : int.Parse(pOSNotificationsViewModel.BackColor.TrimStart('#'), System.Globalization.NumberStyles.HexNumber);
                    oPOSNotifications.LastUpdatedUTC = DateTime.UtcNow;


                    context.Entry(oPOSNotifications).State = EntityState.Modified;

                    context.SaveChanges();

                    pOSNotificationsViewModel.Id = oPOSNotifications.Id;
                    pOSNotificationsViewModel.Code = oPOSNotifications.Code;
                    pOSNotificationsViewModel.Description = oPOSNotifications.Description;
                    pOSNotificationsViewModel.TextColor = oPOSNotifications.TextColorS;
                    pOSNotificationsViewModel.BackColor = oPOSNotifications.BackColorS;
                    pOSNotificationsViewModel.ClientID = oPOSNotifications.ClientID;
                }
                else
                {
                    throw new Exception("Record Not Found");
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditPOSNotification");
                return null;
            }
            return pOSNotificationsViewModel;
        }

        public bool DeletePOSNotification(long Id)
        {
            try
            {
                POSNotifications oPOSNotifications = context.POSNotifications.FirstOrDefault(x => x.Id == Id);

                if (oPOSNotifications != null)
                {
                    oPOSNotifications.IsDeleted = true;
                    oPOSNotifications.LastUpdatedUTC = DateTime.UtcNow;


                    context.Entry(oPOSNotifications).State = EntityState.Modified;

                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Record Not Found");
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeletePOSNotification");
                return false;
            }
            return true;
        }

        public POSNotificationsViewModel GetPOSNotificationById(long Id)
        {
            var pOSNotification = context.POSNotifications.FirstOrDefault(x => x.Id == Id);
            var POSNotifications = new POSNotificationsViewModel();
            try
            {
                if (pOSNotification != null)
                {
                    POSNotifications.Id = pOSNotification.Id;
                    POSNotifications.ClientID = pOSNotification.ClientID;
                    POSNotifications.Name = pOSNotification.Name;
                    POSNotifications.Code = pOSNotification.Code;
                    POSNotifications.Description = pOSNotification.Description;
                    POSNotifications.BackColor = pOSNotification.BackColorS;
                    POSNotifications.TextColor = pOSNotification.TextColorS;

                    return POSNotifications;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetPOSNotificationById");
                return null;
            }
        }

        public bool GetPOSNotificationByClientIdAndName(long clientId, string name)
        {
            POSNotifications oPOSNotifications = context.POSNotifications.FirstOrDefault(x => x.ClientID == clientId && x.Name == name);
            try
            {
                if (oPOSNotifications != null)
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetPOSNotificationByClientIdAndName");
                return false;
            }
        }

        public bool GetPOSNotificationByIdClientIdAndName(long posNotificationId, long clientId, string name)
        {
            POSNotifications oPOSNotifications = context.POSNotifications.FirstOrDefault(x => x.ClientID == clientId && x.Name == name && x.Id == posNotificationId);
            try
            {
                if (oPOSNotifications != null)
                {
                    return false;
                }
                else
                {
                    return GetPOSNotificationByClientIdAndName(clientId, name);
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetPOSNotificationByClientIdAndName");
                return false;
            }
        }
        public List<CustomerPOSNotificationViewModel> GetCustomerPOSNotificationByClientandNotificationID(long clientId, long posNotificationId)
        {
            try
            {
                List<CustomerPOSNotificationViewModel> olstCustomerPOSNotification = context.Customer_POSNotification.Where(x => x.ClientID == clientId && x.POSNotification_Id == posNotificationId).Select(p1 => new CustomerPOSNotificationViewModel { ID = p1.ID, ClientID = p1.ClientID, POSNotification_Id = p1.POSNotification_Id, Customer_Id = p1.Customer_Id }).ToList<CustomerPOSNotificationViewModel>();

                if (olstCustomerPOSNotification != null && olstCustomerPOSNotification.Count > 0)
                {
                    return olstCustomerPOSNotification;
                }
                else
                {
                    return new List<CustomerPOSNotificationViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerPOSNotificationByClienandNotificationID");
                return new List<CustomerPOSNotificationViewModel>();
            }
        }

        public bool DeleteCustomerNotificationById(long id)
        {
            try
            {
                Customer_POSNotification oCustomer_POSNotification = context.Customer_POSNotification.FirstOrDefault(x => x.ID == id);

                if (oCustomer_POSNotification != null)
                {
                    context.Entry(oCustomer_POSNotification).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Record Not Found");

                }

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteCustomerNotificationById");
                return false;
            }
            return true;
        }

        public bool AddCustomerNotification(long posNotificationId, long CustomerID, long clientID)
        {
            Customer_POSNotification oCustomer_POSNotification = new Customer_POSNotification();
            try
            {
                oCustomer_POSNotification.POSNotification_Id = posNotificationId;
                oCustomer_POSNotification.ClientID = clientID;
                oCustomer_POSNotification.Customer_Id = CustomerID;
                oCustomer_POSNotification.LastUpdatedUTC = DateTime.UtcNow;

                context.Customer_POSNotification.Add(oCustomer_POSNotification);

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AddCustomerToNotification");
                return false;
            }
            return true;
        }

        public List<POSNotificationsViewModel> GetPOSNotificationByClientandCustomerID(long clientId, long customerId)
        {
            try
            {
                List<CustomerPOSNotificationViewModel> olstCustomerPOSNotification = context.Customer_POSNotification.Where(x => x.ClientID == clientId && x.Customer_Id == customerId).Select(p1 => new CustomerPOSNotificationViewModel { ID = p1.ID, ClientID = p1.ClientID, POSNotification_Id = p1.POSNotification_Id, Customer_Id = p1.Customer_Id, }).ToList<CustomerPOSNotificationViewModel>();
                List<POSNotificationsViewModel> olstPOSNotificationsViewModel = GetAllPOSNotifications(clientId);
                List<POSNotificationsViewModel> olstPOSNotificationsViewModelreturn = new List<POSNotificationsViewModel>();


                foreach (var obj in olstPOSNotificationsViewModel)
                {
                    if (olstCustomerPOSNotification != null && olstCustomerPOSNotification.Count > 0)
                    {
                        var CustomerPOSNotification = olstCustomerPOSNotification.FirstOrDefault(x => x.ClientID == obj.ClientID && x.POSNotification_Id == obj.Id);
                        if (CustomerPOSNotification != null)
                        {
                            obj.IsSelected = true;
                        }
                        else
                        {
                            obj.IsSelected = false;
                        }
                        olstPOSNotificationsViewModelreturn.Add(obj);

                    }
                    else
                    {
                        olstPOSNotificationsViewModelreturn.Add(obj);
                    }
                }
                return olstPOSNotificationsViewModelreturn;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetPOSNotificationByClientandCustomerID");
                return new List<POSNotificationsViewModel>();
            }
        }

        public List<CustomerPOSNotificationViewModel> GetCustomerPOSNotificationByClientandCustomerID(long clientId, long customerId)
        {
            try
            {
                List<CustomerPOSNotificationViewModel> olstCustomerPOSNotification = context.Customer_POSNotification.Where(x => x.ClientID == clientId && x.Customer_Id == customerId).Select(p1 => new CustomerPOSNotificationViewModel { ID = p1.ID, ClientID = p1.ClientID, POSNotification_Id = p1.POSNotification_Id, Customer_Id = p1.Customer_Id }).ToList<CustomerPOSNotificationViewModel>();

                if (olstCustomerPOSNotification != null && olstCustomerPOSNotification.Count > 0)
                {
                    return olstCustomerPOSNotification;
                }
                else
                {
                    return new List<CustomerPOSNotificationViewModel>();
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "POSNotificationsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerPOSNotificationByClientandCustomerID");
                return new List<CustomerPOSNotificationViewModel>();
            }
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}

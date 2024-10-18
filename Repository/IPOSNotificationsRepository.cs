using AdminPortalModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IPOSNotificationsRepository
    {
        List<POSNotificationsViewModel> GetAllPOSNotifications(long ClientId);

        POSNotificationsViewModel AddPOSNotification(POSNotificationsViewModel pOSNotificationsViewModel);
        POSNotificationsViewModel EditPOSNotification(POSNotificationsViewModel pOSNotificationsViewModel);
        bool DeletePOSNotification(long Id);
        POSNotificationsViewModel GetPOSNotificationById(long Id);
        bool GetPOSNotificationByClientIdAndName(long clientId, string name);
        bool GetPOSNotificationByIdClientIdAndName(long posNotificationId, long clientId, string name);
        List<CustomerPOSNotificationViewModel> GetCustomerPOSNotificationByClientandNotificationID(long clientId, long posNotificationId);
        bool DeleteCustomerNotificationById(long id);
        bool AddCustomerNotification(long posNotificationId, long CustomerID, long clientID);

        List<POSNotificationsViewModel> GetPOSNotificationByClientandCustomerID(long clientId, long customerId);

        List<CustomerPOSNotificationViewModel> GetCustomerPOSNotificationByClientandCustomerID(long clientId, long customerId);
    }
}

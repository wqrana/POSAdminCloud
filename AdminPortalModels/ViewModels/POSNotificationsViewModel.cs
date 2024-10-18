using AdminPortalModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{
    public class POSNotificationsViewModel : ErrorModel
    {
        public long ClientID { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string TextColor { get; set; }
        public string BackColor { get; set; }

        public string CustomerPOSNotificationPSV { get; set; }
        public bool IsSelected { get; set; }
    }

    public class CustomerPOSNotificationViewModel
    {
        public long ClientID { get; set; }
        public long Customer_Id { get; set; }
        public long POSNotification_Id { get; set; }
        public long ID { get; set; }
    }

    public class POSNotificationsCreateModel : POSNotificationsViewModel
    {
        public override string Title { get { return "Create New"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new POS Notification."; } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }

    public class POSNotificationsUpdateModel : POSNotificationsViewModel
    {
        public override string Title { get { return string.Format("Edit: {0}", Name.Trim()); } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} POS Notification.", Name); } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save Changes");
            }
        }
    }
}

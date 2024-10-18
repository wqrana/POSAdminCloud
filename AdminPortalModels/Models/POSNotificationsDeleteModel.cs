using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.Models
{
    public class POSNotificationsDeleteModel : DeleteModel
    {
        public override string Title { get { return "POS Notification"; } }
        public override string DeleteUrl { get { return "/POSNotifications/Delete"; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using AdminPortalModels.Models;

namespace AdminPortalModels.ViewModels
{
    public class HomeroomDetailViewModel
    {
        public HomeRoomUpdateModel HomeroomItem { get; set; }
        public IEnumerable<SelectListItem> SchoolList { get; set; }
        //public virtual SchoolsVM School { get; set; }
    }
}

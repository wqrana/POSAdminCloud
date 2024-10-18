using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdminPortalModels.Models
{
    // for index page
    public class HomeRoomIndexModel : HomeRoomModel
    {
        public string SchoolName { get; set; }
    }

    // for details page
    public class HomeRoomModel : ErrorModel
    {
        [Key]
        [HiddenInput]
        public long ClientID { get; set; }

        [Key]
        [HiddenInput]
        public long Id { get; set; }

        [Required]
        [HiddenInput]
        public long School_Id { get; set; }

        [Required]
        [Display(Name="Homeroom")]
        public string Name { get; set; }
    }

    // for creating
    public class HomeRoomCreateModel : HomeRoomModel
    {
        public string Action { get { return "Create"; } }
        public string Menu { get { return "Customer Data"; } }
        public string SubMenu { get { return "Homeroom"; } }
        public override string Title { get { return "Create Homeroom"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while creating {0} Homeroom.", Name); } }

        public IEnumerable<SelectListItem> Schools { get; set; }
    }

    //for updating
    public class HomeRoomUpdateModel : HomeRoomModel
    {
        public string Action { get { return "Update"; } }
        public override string Title { get { return "Edit Item: "; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} Homeroom.", Name); } }

        public IEnumerable<SelectListItem> Schools { get; set; }
    }

    // for Deleting
    public class HomeRoomDeleteModel : DeleteModel
    {
        public override string Title { get { return "Homeroom"; } }
        public override string DeleteUrl { get { return "/Homeroom/Delete"; } }
    }
}

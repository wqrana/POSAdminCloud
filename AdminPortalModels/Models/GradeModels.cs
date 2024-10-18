using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AdminPortalModels.Models
{
    public class GradeModels : ErrorModel
    {
        [Key]
        [HiddenInput]
        public long ClientID { get; set; }

        [Key]
        [HiddenInput]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Grade")]
        public string Name { get; set; }
    }

    // for creating
    public class GradeCreateModel : GradeModels
    {
        public string Action { get { return "Create"; } }
        public string Menu { get { return "Customer Data"; } }
        public string SubMenu { get { return "Grade"; } }
        public override string Title { get { return "Create Grade"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while creating {0} Grade.", Name); } }
    }

    //for updating
    public class GradeUpdateModel : GradeModels
    {
        public string Action { get { return "Update"; } }
        public override string Title { get { return "Edit Item: "; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} Grade.", Name); } }

    }

    // for Deleting
    public class GradeDeleteModel : DeleteModel
    {
        public override string Title { get { return "Grade"; } }
        public override string DeleteUrl { get { return "/Grade/Delete"; } }
    }
}

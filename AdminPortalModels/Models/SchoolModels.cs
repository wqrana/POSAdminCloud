using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AdminPortalModels.ViewModels;

namespace AdminPortalModels.Models
{

    //for index page
    public class SchoolIndexModel
    {
        public long Id { get; set; }
        [Display(Name = "School")]
        public string SchoolName { get; set; }
        [Display(Name = "POS Stations")]
        public int POSCount { get; set; }
        [Display(Name = "District")]
        public string DistrictName { get; set; }
        public string shortSchoolName
        {
            get {
                if (SchoolName.Length > 25)
                    return SchoolName.Substring(0, 19) + "...";
                else
                {
                    return SchoolName;
                }
            }
        }
        public string toolTip
        {
            get {
                if (SchoolName.Length > 25)
                    return SchoolName;
                else
                    return "";
            }
        
        }
        public long District_Id { get; set; }
       
        
    }

    // Standard
    public class SchoolModel : ErrorModel
    {
        [Key]
        [HiddenInput]
        public long ClientID { get; set; }

        [Key]
        [HiddenInput]
        public long Id { get; set; }

        [Required]
        [HiddenInput]
        public long District_Id { get; set; }

        public Nullable<long> Emp_Director_Id { get; set; }
        public Nullable<long> Emp_Administrator_Id { get; set; }

        [Required]
        [Display(Name="School ID")]
        public string SchoolID { get; set; }

        [Required]
        [Display(Name="School")]
        public string SchoolName { get; set; }

        [Display(Name = "District")]
        public string DistrictName { get; set; }

        [Display(Name="Address")]
        public string Address1 { get; set; }

        public string Address2 { get; set; }

        [Display(Name="City")]
        public string City { get; set; }

        [Display(Name="State")]
        public string State { get; set; }

        [Display(Name="Zip Code")]
        public string Zip { get; set; }

        [Display(Name="Phone")]
        public string Phone1 { get; set; }

        [Display(Name="Fax")]
        public string Phone2 { get; set; }

        [Display(Name="Notes")]
        public string Comment { get; set; }

        [Display(Name="Severe Need School")]
        public Nullable<bool> isSevereNeed { get; set; }

        [Display(Name="Deleted")]
        public Nullable<bool> isDeleted { get; set; }

        //[HiddenInput]
        //public Nullable<int> Forms_Director_Id { get; set; }

        //[HiddenInput]
        //public Nullable<int> Forms_Admin_Id { get; set; }

        [Display(Name="Use District's Director/Admin")]
        public Nullable<bool> UseDistDirAdmin { get; set; }

        //[Display(Name="FORMS Administrator Title")]
        //public string Forms_Admin_Title { get; set; }

        //[Display(Name="FORMS Administrator Phone")]
        //public string Forms_Admin_Phone { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? SchoolYearStartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime? SchoolYearEndDate { get; set; }

    }

    public class SchoolUpdateModel : SchoolModel
    {
        public override string Title { get { return string.Format("{0} Settings", SchoolName); } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0}.", SchoolName); } }

        [Display(Name = "Display Photos on POS")]
        public bool PhotoLogging { get; set; }
        [Display(Name = "Remove Leading Zeros")]
        public bool StripZeros { get; set; }
        public string PinPreFix { get; set; }
        [Display(Name = "PIN Prefix")]
        public bool DoPinPreFix { get; set; }
        [Display(Name = "PIN Enable")]
        public bool IsPinEnable { get; set; }
        [Display(Name = "Ala Carte Limit")]
        public double? AlaCarteLimit { get; set; }
        [Display(Name = "Meal Plan Limit")]
        public double MealPlanLimit { get; set; }
        [Display(Name = "PIN Length")]
        public int? BarCodeLength { get; set; }

        public string TaxesList { get; set; }
        public DateTime? districtStartDate { get; set; }
        public DateTime? districtEndDate { get; set; }
        public string districtEmpDirectorName { get; set; }
        public string districtEmpAdminName { get; set; }

        public IEnumerable<SelectListItem> Districts { get; set; }
        public ICollection<State> States { get; set; }
        public IEnumerable<SelectListItem> Directors { get; set; }
        public IEnumerable<SelectListItem> Admins { get; set; }

        public IEnumerable<SelectListItem> BarCodeLengthList { get; set; }

        public List<Taxes> Taxes { get; set; }
    }

    // for Deleting
    public class SchoolDeleteModel : DeleteModel
    {
        public override string Title { get { return "School"; } }
        public override string DeleteUrl { get { return "/School/Delete"; } }
        public bool customersExists { get; set; }

    }
}

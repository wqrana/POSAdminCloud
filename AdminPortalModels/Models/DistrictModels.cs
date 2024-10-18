using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminPortalModels.Models
{
    // tile view
    public class DistrictIndexModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public int schoolCount { get; set; }
    }

    //////// popup
    //////public class DistrictModel : ErrorModel
    //////{
    //////    [HiddenInput]
    //////    public int Id { get; set; }

    //////    // General
    //////    [Required]
    //////    [MaxLength(30, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Name")]
    //////    public string Name { get; set; }
       
    //////    //[Required]
    //////    [MaxLength(30, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Address")]
    //////    public string Address1 { get; set; }
        
    //////    [MaxLength(30, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Address")]
    //////    public string Address2 { get; set; }
        
    //////    [MaxLength(30, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    //[Display(Name = "City")]
    //////    public string City { get; set; }
        
    //////    [MaxLength(2, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    //[Display(Name = "State")]
    //////    public string State { get; set; }
        
    //////    [MaxLength(10, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Zip")]
    //////    public string Zip { get; set; }
        
    //////    [MaxLength(14, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Phone")]
    //////    public string Phone { get; set; }
        
    //////    [MaxLength(14, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Fax")]
    //////    public string Fax { get; set; }

    //////    // Options
    //////    [Display(Name = "Paid Student")]
    //////    public bool? IsStudentPaidTaxable { get; set; }
    //////    [Display(Name = "Reduced Student")]
    //////    public bool? IsStudentRedTaxable { get; set; }
    //////    [Display(Name = "Free Student ")]
    //////    public bool? IsStudentFreeTaxable { get; set; }
    //////    [Display(Name = "Employee")]
    //////    public bool? IsEmployeeTaxable { get; set; }
    //////    [Display(Name = "Meal Plan Person")]
    //////    public bool? IsMealPlanTaxable { get; set; }
    //////    [Display(Name = "Guest (Cash Sale)")]
    //////    public bool? IsGuestTaxable { get; set; }
    //////    [Display(Name = "Student (Cash Sale) ")]
    //////    public bool? IsStudentCashTaxable { get; set; }

    //////    [Display(Name = "Start Date")]
    //////    public DateTime? SchoolStartYear { get; set; }
    //////    [Display(Name = "End Date")]
    //////    public DateTime? SchoolEndYear { get; set; }

    //////    // BankInfo
    //////    [MaxLength(15, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Routing Number")]
    //////    public string RoutingNumber { get; set; }
       
    //////    [MaxLength(15, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Account Number")]
    //////    public string AccountNumber { get; set; }

    //////    [MaxLength(30, ErrorMessage = "{0} can be {1} characters or less.")]
    //////    [Display(Name = "Bank Name")]
    //////    public string BankName { get; set; }

    //////    [MaxLength(30, ErrorMessage = "Bank {0} can be {1} characters or less.")]
    //////    [Display(Name = "Address")]
    //////    public string BankAddress { get; set; }

    //////    [MaxLength(30, ErrorMessage = "Bank {0} can be {1} characters or less.")]
    //////    [Display(Name = "Address 2")]
    //////    public string BankAddress { get; set; }

    //////    [MaxLength(30, ErrorMessage = "Bank {0} can be {1} characters or less.")]
    //////    [Display(Name = "City")]
    //////    public string BankCity { get; set; }

    //////    [MaxLength(2, ErrorMessage = "Bank {0} can be {1} characters or less.")]
    //////    [Display(Name = "State")]
    //////    public string BankState { get; set; }
        
    //////    [MaxLength(10, ErrorMessage = "Bank {0} can be {1} characters or less.")]
    //////    [Display(Name = "Zip")]
    //////    public string BankZip { get; set; }
    //////}

    //////public class DistrictCreateModel : DistrictModel
    //////{
    //////    public override string Title { get { return "Create New Item"; } }
    //////    public override string SuccessMessage { get { return "A new item created successfully."; } }
    //////    public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new item."; } }
    //////    public override string LoadSuccessMessage { get { return "Initialized successfully."; } }
    //////    public override string LoadErrorMessage { get { return "Error initializing."; } }
    //////}

    //////public class DistrictUpdateModel : DistrictModel
    //////{
    //////    public override string Title { get { return "Edit Item: "; } }
    //////    public override string SuccessMessage { get { return string.Format("{0} item updated successfully.", ItemName); } }
    //////    public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} item.", ItemName); } }
    //////    public override string LoadSuccessMessage { get { return "Data Loaded successfully."; } }
    //////    public override string LoadErrorMessage { get { return "Error loading data."; } }
    //////}

    // delete
    public class DistrictDeleteModel : DeleteModel
    {
        public override string Title { get { return "District"; } }
        public override string DeleteUrl { get { return "/District/Delete"; } }
        public bool schoolsExists { get; set; }
    }

    



}
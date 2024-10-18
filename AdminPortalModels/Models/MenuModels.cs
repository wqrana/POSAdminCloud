using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminPortalModels.Models
{
    public enum ItemType
    {
        NA, LunchItem, Breakfast
    }

    // index page
    public class MenuIndexModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public string UPCCode { get; set; }
        public string Color { get; set; }
        public string HexaColor { get { return Color; } }

        //Added by Waqar Q. bug 1546
        public bool IsDeleted { get; set; }
    }

    public class MenuIndexSearchModel
    {
        public string SearchBy { get; set; }
        public int? SearchBy_Id { get; set; }
        public int? Category_Id { get; set; }
        public int? CategoryType_Id { get; set; }
        public int? Taxable_Id { get; set; }
        public int? KitchenItem_Id { get; set; }
        public int? ScaleItem_Id { get; set; }
        public int? hdCategory_Id { get; set; }
    }

    // popup
    public class MenuModel : ErrorModel
    {
        [HiddenInput]
        public long Id { get; set; }
        
        [Required]
        //[Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        [Display(Name = "Category")]
        public long Category_Id { get; set; }
        
        [Required]
        [MaxLength(75, ErrorMessage = "{0} can be {1} characters or less.")]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Student Full Price")]
        public double? StudentFullPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Student Reduced Price")]
        public double? StudentRedPrice { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Employee Price")]
        public double? EmployeePrice { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Guest Price")]
        public double? GuestPrice { get; set; }
       
        [Display(Name = "Item Type")]
        public ItemType ItemType { get; set; }

        [Display(Name = "Taxable Item")]
        public bool? isTaxable { get; set; }

        [Display(Name = "Electronic Scale Item")]
        public bool? isScaleItem { get; set; }

        [Display(Name = "Once Per Day Item")]
        public bool? isOnceDay { get; set; }
        
        [Display(Name = "Kitchen Printer Item")]
        public bool? KitchenItem { get; set; }

        [MaxLength(25, ErrorMessage = "{0} can be {1} characters or less.")]
        [Display(Name = "UPC Code")]
        public string UPC { get; set; }

        [MaxLength(30, ErrorMessage = "{0} can be {1} characters or less.")]
        [Display(Name = "Shortcut Button Caption")]
        public string ButtonCaption { get; set; }

        [MaxLength(80, ErrorMessage = "{0} can be {1} characters or less.")]
        [Display(Name = "Preorder Description")]
        public string PreOrderDesc { get; set; }
    }

    public class MenuCreateModel : MenuModel
    {
        public override string Title { get { return "Create New Item"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new item."; } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }
   
    public class MenuUpdateModel : MenuModel
    {
        public override string Title { get { return "Edit Item: "; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} item.", ItemName); } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
        public bool displayReducedPrice { get; set; }
 
    }

    // delete
    public class MenuDeleteModel : DeleteModel
    {
        public override string Title { get { return "Menu Item"; } }
        public override string DeleteUrl { get { return "/Menu/Delete"; } }
    }
}
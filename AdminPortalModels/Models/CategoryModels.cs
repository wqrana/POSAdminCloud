using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminPortalModels.Models
{
    // for index page
    public class CategoryIndexModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Category Type")]
        public string CategoryType { get; set; }
        public string Color { get; set; }
        public string HexaColor { get { return Color; } }
        [Display(Name = "Item Count")]
        public int ItemCount { get; set; }
    }

    // delete
    public class CategoryDeleteModel : DeleteModel
    {
        public override string Title { get { return "Category"; } }
        public override string DeleteUrl { get { return "/Category/Delete"; } }
    }

    // for popup
    public class CategoryModel : ErrorModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Category Type")]
        public long? CategoryType_Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "{0} can be {1} characters or less.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public string Color { get; set; }
        public string HexaColor { get { return Color; } }
        [Display(Name = "Currently Active")]
        public bool IsActive { get; set; }
    }

    public class CategoryCreateModel : CategoryModel
    {
        public override string Title { get { return "Create New"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new category."; } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }

    public class CategoryUpdateModel : CategoryModel
    {
        public override string Title { get { return string.Format("{0} Settings", Name); } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} category.", Name); } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }
}
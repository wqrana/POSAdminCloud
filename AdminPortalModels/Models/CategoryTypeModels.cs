using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminPortalModels.Models
{
    // for index page
    public class CategoryTypeIndexModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Categories { get; set; }
        public int Items { get; set; }
    }

    // delete
    public class CategoryTypeDeleteModel : DeleteModel
    {
        public override string Title { get { return "Category Type"; } }
        public override string DeleteUrl { get { return "/CategoryType/Delete"; } }
    }

    // for popup
    public class CategoryTypeModel : ErrorModel
    {
        [HiddenInput]
        public long Id { get; set; }
        [Required]
        [MaxLength(15, ErrorMessage = "{0} can be {1} characters or less.")]
        [Display(Name = "Category Type Name")]
        public string Name { get; set; }
        [Display(Name = "Can be free")]
        public bool CanFree { get; set; }
        [Display(Name = "Can be reduced")]
        public bool CanReduce { get; set; }
        [Display(Name = "Meal Plan Items")]
        public bool IsMealPlan { get; set; }
        [Display(Name = "Meal Equivalency")]
        public bool IsMealEquiv { get; set; }
        

    }

    public class CategoryTypeCreateModel : CategoryTypeModel
    {
        public override string Title { get { return "Create New"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new category type."; } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }

    public class CategoryTypeUpdateModel : CategoryTypeModel
    {
        public override string Title { get { return string.Format("{0} Settings", Name); } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} category type.", Name); } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }
}
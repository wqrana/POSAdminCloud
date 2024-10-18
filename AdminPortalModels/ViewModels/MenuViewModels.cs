using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using AdminPortalModels.Models;

namespace AdminPortalModels.ViewModels
{
    public class MenuIndexViewModel : MenuIndexSearchModel
    {
        public IEnumerable<MenuIndexModel> IndexModel { get; set; }
        public IEnumerable<SelectListItem> SearchByList { get; set; }
        [Display(Name = "Category Type")]
        public IEnumerable<SelectListItem> CategoryTypeList { get; set; }
        [Display(Name = "Category")]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
        [Display(Name = "Taxable")]
        public IEnumerable<SelectListItem> TaxableList { get; set; }
        [Display(Name = "Kitchen Item")]
        public IEnumerable<SelectListItem> KitchenItemList { get; set; }
        [Display(Name = "Scale Item")]
        public IEnumerable<SelectListItem> ScaleItemList { get; set; }
    }

    public class MenuViewModel
    {
        public MenuModel MenuItem { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}

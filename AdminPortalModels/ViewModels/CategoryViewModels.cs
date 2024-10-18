using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using AdminPortalModels.Models;

namespace AdminPortalModels.ViewModels
{
    public class CategoryViewModel
    {
        public CategoryModel CategoryModel { get; set; }
        public IEnumerable<SelectListItem> CategoryTypeList { get; set; }
    }
}

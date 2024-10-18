using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Services.Client;

namespace Repository.Helpers
{
    public class CommonHelper
    {
        public const string RecordNotFoundMsg = "Record not found or deleted by another user.";

        //public static class HtmlHelperExtensions
        //{
        //    public static string CurrentViewName(this HtmlHelper html)
        //    {
        //        return System.IO.Path.GetFileNameWithoutExtension(
        //            ((RazorView)html.ViewContext.View).ViewPath
        //        );
        //    }
        //}

        

        public static SelectList AddFirstItem(SelectList origList, SelectListItem firstItem)
        {
            List<SelectListItem> newList = origList.ToList();
            newList.Insert(0, firstItem);

            var selectedItem = newList.FirstOrDefault(item => item.Selected);
            var selectedItemValue = String.Empty;
            if (selectedItem != null)
            {
                selectedItemValue = selectedItem.Value;
            }

            return new SelectList(newList, "Value", "Text", selectedItemValue);
        }
    }

}

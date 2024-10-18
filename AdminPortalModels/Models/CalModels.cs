using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminPortalModels.Models
{

    //public class WebLunchCalendar
    //{
    //    public Int32 WebCalID { get; set; }
    //    public string CalendarName { get; set; }
    //    public int CalendarType { get; set; }
    //    public Int32 DistrictID { get; set; }
    //}
    public class CalendarDeleteModel : DeleteModel
    {
        public override string Title { get { return "Calendar"; } }
        public override string DeleteUrl { get { return "/PreorderCalList/Delete"; } }
    }





}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;


namespace AdminPortalModels.ViewModels
{
   public class PreorderSettingModel
    {
        public long Id { get; set; }

        public int? POPickMode { get; set; }

        public string POPickModeDescription { get; set; }

    }
}

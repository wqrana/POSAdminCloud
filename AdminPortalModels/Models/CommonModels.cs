using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace AdminPortalModels.Models
{
    public class ErrorModel
    {
        public virtual string Title { get { return "Title"; } }
        public string Message { get; set; }
        public bool IsError { get; set; }
        public virtual string savebtnCaption { get { return "SAVE"; } }

        // will remove in future and just use Message property
        public string ErrorMessage2 { get; set; }
        public virtual string ErrorMessage { get { return "Error!"; } }
        
    }

    // delete
    public class DeleteModel
    {
        [HiddenInput]
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual string Title { get { return ""; } }
        public virtual string DeleteUrl { get { return ""; } }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }

    // activate/deactivate
    public class ActivateModel
    {
        [HiddenInput]
        public long Id { get; set; }
        public bool IsActive { get; set; }
        public string ActiveString { get { return !IsActive ? "Activate" : "Deactivate"; } }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ActivateUrl { get; set; }
        public string Message { get; set; }
        public bool IsError { get; set; }
    }
}

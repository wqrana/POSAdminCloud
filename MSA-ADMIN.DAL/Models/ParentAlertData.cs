using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class ParentAlertData
    {
        public int ID { get; set; }
        public int District_Id { get; set; }
        public string MessageCreated { get; set; }
        public DateTime MessageCreatedDate { get; set; } // MessageCreated
        public int? USERID { get; set; } // Created_DistrictUsers_ID
        public string MessageName { get; set; }
        public string MessageText { get; set; }
        public string MessageStart { get; set; }
        public string MessageEnd { get; set; }
        public bool? SendEmailNotification { get; set; }
        public string Status { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? LastEdited { get; set; }
        public int? Edited_DistrictUsers_ID { get; set; }
        public string userid2 { get; set; }
        public string DistrictGroup { get; set; }
    }
}

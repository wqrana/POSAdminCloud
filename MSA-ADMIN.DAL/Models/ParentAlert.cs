using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class ParentAlert
    {
        public int Id { get; set; }
        public int District_Id { get; set; }
        public DateTime MessageCreated { get; set; }
        public string MessageName { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageStart { get; set; }
        public DateTime? MessageEnd { get; set; }
        public bool? SendEmailNotification { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? LastEdited { get; set; }
        public int? Created_DistrictUsers_ID { get; set; }
        public int? Edited_DistrictUsers_ID { get; set; }
        public string DistrictGroup { get; set; }

    }
}

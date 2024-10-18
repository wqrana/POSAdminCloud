using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class School
    {
        public int Id { get; set; }
        public int DistSchool_Id { get; set; }
        public int District_Id { get; set; }
        public string DistrictName { get; set; }
        public int? Emp_Director_Id { get; set; }
        public int? Emp_Administrator_Id { get; set; }
        public string SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Comment { get; set; }
        public bool? isSevereNeed { get; set; }
        public bool? isDeleted { get; set; }
        public bool? UseDistDirAdmin { get; set; }
        public int? Forms_Director_Id { get; set; }
        public int? Forms_Admin_Id { get; set; }
        public bool? UseDistFormsDirAdmin { get; set; }
        public bool? UseDistNameDirector { get; set; }
        public bool? UseDistNameAdmin { get; set; }
        public string Forms_Admin_Title { get; set; }
        public string Forms_Admin_Phone { get; set; }
        public string Forms_Dir_Title { get; set; }
        public string Forms_Dir_Phone { get; set; }
        public bool? isPreorderTaxable { get; set; }
        public bool? isEasyPayTaxable { get; set; }
        public int CloudPOS_Id { get; set; }
    }
}

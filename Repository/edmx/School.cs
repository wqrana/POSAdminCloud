//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository.edmx
{
    using System;
    using System.Collections.Generic;
    
    public partial class School
    {
        public long ClientID { get; set; }
        public long ID { get; set; }
        public long District_Id { get; set; }
        public Nullable<long> Emp_Director_Id { get; set; }
        public Nullable<long> Emp_Administrator_Id { get; set; }
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
        public Nullable<bool> isSevereNeed { get; set; }
        public bool isDeleted { get; set; }
        public Nullable<bool> UseDistDirAdmin { get; set; }
        public Nullable<long> Forms_Director_Id { get; set; }
        public Nullable<long> Forms_Admin_Id { get; set; }
        public Nullable<bool> UseDistFormsDirAdmin { get; set; }
        public Nullable<bool> UseDistNameDirector { get; set; }
        public Nullable<bool> UseDistNameAdmin { get; set; }
        public string Forms_Admin_Title { get; set; }
        public string Forms_Admin_Phone { get; set; }
        public string Forms_Dir_Title { get; set; }
        public string Forms_Dir_Phone { get; set; }
        public Nullable<System.DateTime> LastUpdatedUTC { get; set; }
        public Nullable<bool> UpdatedBySync { get; set; }
        public Nullable<int> Local_ID { get; set; }
        public bool CloudIDSync { get; set; }
    }
}

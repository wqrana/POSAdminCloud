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
    
    public partial class SchoolOption
    {
        public long ClientID { get; set; }
        public long ID { get; set; }
        public long School_Id { get; set; }
        public Nullable<System.DateTime> ChangedDate { get; set; }
        public Nullable<double> AlaCarteLimit { get; set; }
        public Nullable<double> MealPlanLimit { get; set; }
        public Nullable<bool> DoPinPreFix { get; set; }
        public string PinPreFix { get; set; }
        public Nullable<bool> PhotoLogging { get; set; }
        public Nullable<bool> FingerPrinting { get; set; }
        public Nullable<int> BarCodeLength { get; set; }
        public Nullable<System.DateTime> StartSchoolYear { get; set; }
        public Nullable<System.DateTime> EndSchoolYear { get; set; }
        public Nullable<bool> StripZeros { get; set; }
        public Nullable<System.DateTime> ChangedDateLocal { get; set; }
        public Nullable<System.DateTime> LastUpdatedUTC { get; set; }
        public Nullable<bool> UpdatedBySync { get; set; }
        public Nullable<int> Local_ID { get; set; }
        public bool CloudIDSync { get; set; }
    }
}
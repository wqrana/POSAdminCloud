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
    
    public partial class App_Member_Incomes
    {
        public long ClientID { get; set; }
        public long Id { get; set; }
        public long App_Member_Id { get; set; }
        public int Income_Type_Id { get; set; }
        public Nullable<double> Income { get; set; }
        public Nullable<int> Frequency_Id { get; set; }
        public Nullable<System.DateTime> LastUpdatedUTC { get; set; }
        public Nullable<bool> UpdatedBySync { get; set; }
        public Nullable<int> Local_ID { get; set; }
        public bool CloudIDSync { get; set; }
    }
}
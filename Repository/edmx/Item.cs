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
    
    public partial class Item
    {
        public long ClientID { get; set; }
        public long Id { get; set; }
        public long Order_Id { get; set; }
        public long Menu_Id { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<double> FullPrice { get; set; }
        public Nullable<double> PaidPrice { get; set; }
        public Nullable<double> TaxPrice { get; set; }
        public Nullable<bool> isVoid { get; set; }
        public Nullable<int> SoldType { get; set; }
        public Nullable<long> PreOrderItem_Id { get; set; }
        public Nullable<System.DateTime> LastUpdatedUTC { get; set; }
        public Nullable<bool> UpdatedBySync { get; set; }
        public Nullable<int> Local_ID { get; set; }
        public bool CloudIDSync { get; set; }
    }
}
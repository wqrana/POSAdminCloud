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
    
    public partial class SalesTaxOnOrder
    {
        public long ClientID { get; set; }
        public long ORDID { get; set; }
        public Nullable<double> CashSaleSalesTax { get; set; }
        public Nullable<double> AccountSaleSalesTax { get; set; }
        public Nullable<double> SalesTax { get; set; }
    }
}
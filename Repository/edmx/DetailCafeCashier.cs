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
    
    public partial class DetailCafeCashier
    {
        public long ClientID { get; set; }
        public long CASHRESID { get; set; }
        public Nullable<long> POSID { get; set; }
        public Nullable<long> SCHID { get; set; }
        public Nullable<bool> Finished { get; set; }
        public string POSName { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Middle { get; set; }
        public long Emp_Cashier_Id { get; set; }
        public Nullable<System.DateTime> OpenDate { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public string OpenMessage { get; set; }
        public string CloseMessage { get; set; }
    }
}
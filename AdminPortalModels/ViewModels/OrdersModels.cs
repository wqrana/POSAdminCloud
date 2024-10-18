using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{
    public class VoidsOrdersGroup
    {
        public string groupName { get; set; }
        public int? groupID { get; set; }
        public string UserID { get; set; }
        public double? CustomerBalance { get; set; }
    }

    public class CustomerOrderDetails
    {
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> OrderLogID { get; set; }
        public Nullable<int> OrderType { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> OrderDateLocal { get; set; }
        public Nullable<double> Item { get; set; }
        public Nullable<double> SalesTax { get; set; }
        public Nullable<double> Total { get; set; }
        public Nullable<double> Payment { get; set; }
        public string Type { get; set; }
        public string Check { get; set; }
        public string POS { get; set; }
        public Nullable<bool> isVoid { get; set; }

        //public string customerName { get; set; }
        //public string orderDate { get; set; }
        //public string item { get; set; }
    }
    public class GroupFilters
    {
        public int? SearchBy_Id { get; set; }
        public string SearchStr { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public int CustomerID { get; set; }
        public bool? IsActive { get; set; }
    }

    public class OrderInfo
    {
        public long OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public Nullable<System.DateTime> OrderDateLocal { get; set; }
        public string OrderDateString { get; set; }
        public Nullable<double> OrderTotal { get; set; }
        public Nullable<double> Payment { get; set; }
        public string POSName { get; set; }
        public string CashierName { get; set; }
        public long CustomerID { get; set; }
        public int POSID { get; set; }
        public Nullable<long> CashierID { get; set; }
        public Nullable<int> SchoolID { get; set; }
        public Nullable<int> PrimarySchoolID { get; set; }
        public Nullable<long> OrdersLogID { get; set; }
        public string OrderNote { get; set; }
       
    }

    public class OrderDetailsPopup
    {
        public Nullable<int> id { get; set; }
        public Nullable<int> Qty { get; set; }
        public string ItemName { get; set; }
        public Nullable<decimal> PaidPrice { get; set; }
        public Nullable<bool> Void { get; set; }
        public Nullable<System.DateTime> ServingDate { get; set; }
        public Nullable<System.DateTime> PickedupDate { get; set; }
    }

    public class OrderParam
    {
        public string orderId { get; set; }
        public string ordertype { get; set; }
    }

    public class ApiOrderParam
    {
        [Required]
        public long clientID { get; set; }
        [Required]
        public int customerID { get; set; }
        [Required]
        public DateTime startDate { get; set; }
        [Required]
        public DateTime endDate { get; set; }
    }

}
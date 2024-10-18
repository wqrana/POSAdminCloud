using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.Models
{
    public class ActivityModel
    {
        
    }

    public class DetailOrdersModel 
    {
        public long ClientID { get; set; }
        public long CustomerID { get; set; }
        public long SchoolID { get; set; }
        public long OrderID { get; set; }
        public string OrderTypeName { get; set; }
        public int OrderType { get; set; }
        public Nullable<System.DateTime> GDate { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<int> TransType { get; set; }
        public string PaymentType { get; set; }
        public Nullable<double> TotalAccount { get; set; }
        public Nullable<double> TotalPaid { get; set; }
        public Nullable<double> SalesTax { get; set; }
        public Nullable<double> BalanceChange { get; set; }

        public IEnumerable<DetailItemsModel> DetailItemsList { get; set; }
    }

    public class DetailItemsModel 
    {
        public long ClientID { get; set; }
        public long CustomerID { get; set; }
        public long SchoolID { get; set; }
        public long OrderID { get; set; }
        public Nullable<System.DateTime> GDate { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<int> TransType { get; set; }
        public string OrderTypeName { get; set; }
        public int OrderType { get; set; }
        public string ItemName { get; set; }
        public Nullable<double> PaidPrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> ExtendedPrice { get; set; }
    }
}

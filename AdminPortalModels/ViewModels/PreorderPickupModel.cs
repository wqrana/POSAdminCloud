using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.Models;


namespace AdminPortalModels.ViewModels
{
   
    public class PreorderPickupModel
    {
        public long ClientID { get; set; }
        public IEnumerable<SchoolItem> locationList { get; set; }
        public IEnumerable<HomeRoomModel> homeRoomList { get; set; }
        public IEnumerable<Grade> gradesList { get; set; }
        public IEnumerable<CustomersList> customersList { get; set; }
        public IEnumerable<DateRangeType> dateRangeTypesList { get; set; } //Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
        public IEnumerable<ItemSelectionType> itemSelectionTypeList { get; set; }
        public IEnumerable<ItemStatus> itemStatusList { get; set; }
        public IEnumerable<dynamic> categoryTypeList { get; set; }
        public IEnumerable<dynamic> categoryList { get; set; }
        public IEnumerable<dynamic> itemList { get; set; }
            
    }

    public class PreorderPickupList
    {

        public long PreOrderItemId { get; set; }
        public long preOrderId { get; set; }
        public int transactionId { get; set; }
        public string Grade { get; set; }
        public string customerName { get; set; }
        public string userId { get; set; }
        public Nullable<long> CategoryType_Id { get; set; }
        public long Category_Id { get; set; }
        public string itemName { get; set; }
        public System.DateTime datePurchased { get; set; }
        public Nullable<System.DateTime> dateToServe { get; set; }
        public Nullable<System.DateTime> datePickedUp { get; set; }
        public int received { get; set; }
        public bool itemVoid { get; set; }
        public int qty { get; set; }
        public bool orderVoid { get; set; }
        public string @void { get; set; }

    }

    public class LoadItemVoidList{
      public long Id { get; set; }
        public Nullable<long> PreorderId { get; set; }
        public string CustomerName { get; set; }
        public Nullable<long> customerId { get; set; }
        public string UserID { get; set; }
        public string Grade { get; set; }
        public Nullable<int> PreSaleTrans_Id { get; set; }
        public Nullable<long> orderId { get; set; }
        public string ItemName { get; set; }
        public System.DateTime purchasedDate { get; set; }
        public Nullable<System.DateTime> ServingDate { get; set; }
        public Nullable<int> Qty { get; set; }
        public string CanVoid { get; set; }
        public string isVoid { get; set; }
    }


    public class LoadOrderVoidList
    {

        public long Id { get; set; }
        public Nullable<int> PreSaleTrans_Id { get; set; }
        public Nullable<long> OrderId { get; set; }
        public string Grade { get; set; }
        public string CustomerName { get; set; }
        public string UserID { get; set; }
        public Nullable<System.DateTime> PurchasedDate { get; set; }
        public string Void { get; set; }
        public string CanVoid { get; set; }
        public Nullable<int> HasPayment { get; set; }
    }

    [Serializable]
    public class PreorderPickupFilters
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string customerSelectionType { get; set; }
        public string SelectedCustomersList { get; set; }
        public string location { get; set; }
        public string homeRoom { get; set; }
        public string grade { get; set; }
        public string dateRangeTypes { get; set; }
        public string itemSelectionType { get; set; }
        public string itemStatusType { get; set; }
        public string selectedTypeList { get; set; }
        
         

    }

     [Serializable]
    public class VoidRequestType{

        public string callingType { get; set; }
        public string callingParam { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> orderId { get; set; }
        public Nullable<int> orderLogId { get; set; }
        public Nullable<int> orderType { get; set; }
        public Nullable<int> customerId { get; set; }
        public Nullable<int> clientId { get; set; }   
        public Nullable<bool> voidPayment { get; set;} 

    }

     public class VoidUpdateResult
     {
         public int Result { get; set; }
         public string ErrorMessage { get; set; }
        
     }

     /* Use from ReportModel
     [Serializable]
     
      public class ItemSelectionFilters
      {
          public int selectionType { get; set; }
          public int? categoryType {get;set;}
          public int? category { get; set; }

      }
      */
     public class DateRangeType
    {
       public int id {get;set;}
       public string name { get; set; }


    }

   public class ItemStatus
   {
       public int id { get; set; }
       public string name { get; set; }
   }

   /* Use from ReportModel
   public class ItemSelectionType 
   {
       public int id { get; set; }
       public string name { get; set; }

   }
   */

   public class PreorderPickupItemsList
    {
        public Nullable<long> Id { get; set; }
        public Nullable<long> PreOrderId { get; set; }
        public Nullable<int> PickupCountValid { get; set; }
        public Nullable<int> Selected { get; set; }
        public Nullable<int> TransactionId { get; set; }
        public string Grade { get; set; }
        public string CustomerName { get; set; }
        public string UserId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<long> MealPlan_Id { get; set; }
        public string ItemName { get; set; }
        public Nullable<long> MenuId { get; set; }
        public Nullable<double> StudentFullPrice { get; set; }
        public Nullable<long> Category_Id { get; set; }
        public Nullable<int> KitchenItem { get; set; }
        public Nullable<System.DateTime> DatePurchased { get; set; }
        public Nullable<System.DateTime> DateToServe { get; set; }
        public Nullable<System.DateTime> DatePickedUp { get; set; }
        public Nullable<int> Received { get; set; }
        public Nullable<bool> ItemVoid { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<bool> OrderVoid { get; set; }
        public string Void { get; set; }
        public Nullable<long> SchoolId { get; set; }
        public Nullable<int> PickupQty { get; set; }
    }

     public class PreorderPickupItemsCount
     {

         public Nullable<int> recordCount { get; set; }
         public string recordIdStr { get; set; }

     }

    public class SelectedPreorderItems
    {
        public int Id { get; set; }
        public int Qty { get; set; }
        public Nullable<System.DateTime> DatePickedUp { get; set; }
    }


    public class PreorderPickupReport
    {
        public int transactionId { get; set; }
        public string Grade { get; set; }
        public string customerName { get; set; }
        public string userId { get; set; }
        public string itemName { get; set; }
        public System.DateTime datePurchased { get; set; }
        public Nullable<System.DateTime> datePickedUp { get; set; }
        public int received { get; set; }
        public int Ordered { get; set; }
        public string voidSts { get; set; }

    }

}


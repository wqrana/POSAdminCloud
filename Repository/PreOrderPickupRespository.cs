using AdminPortalModels.ViewModels;
using Repository.edmx;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
namespace Repository
{
    class PreOrderPickupRespository : IPreOrderPickupRespository, IDisposable
    {

        private PortalContext context;
        private bool disposed = false;
        string userIdsCannotBeDuplicated = string.Empty;

        public PreOrderPickupRespository(PortalContext context)
        {
            this.context = context;

            userIdsCannotBeDuplicated = ConfigurationManager.AppSettings["UserIdsCannotBeDuplicated"];

        }

        public PreorderPickupItemsCount GetPreOrderPickupItemsCount(PreorderPickupFilters filters)
        {
            string location = filters.location == null ? "" : filters.location;
            string dateRangeTypes = filters.dateRangeTypes == null ? "" : filters.dateRangeTypes;
            string fromDate = filters.fromDate;
            string toDate = filters.toDate;
            string homeRoom = filters.homeRoom == null ? "" : filters.homeRoom;
            string customerSelectionType = filters.customerSelectionType;
            string SelectedCustomersList = filters.SelectedCustomersList == null ? "" : filters.SelectedCustomersList;
            string grade = filters.grade == null ? "" : filters.grade;
            string itemSelectionType = filters.itemSelectionType;
            string itemStatusType = filters.itemStatusType;
            string selectedTypeList = filters.selectedTypeList;

            PreorderPickupItemsCount result =
                       this.context.Main_PreOrder_Count
                       (
                        location,
                         dateRangeTypes,
                         fromDate,
                         toDate,
                         homeRoom,
                         customerSelectionType,
                         SelectedCustomersList,
                         grade,
                         itemSelectionType,
                         itemStatusType,
                         selectedTypeList

                      ).Select
                      (x => new PreorderPickupItemsCount()
                      {

                          recordCount = x.recordCount,
                          recordIdStr = x.recordIdStr
                      }
                      ).FirstOrDefault();

            return result;

        }

       public IEnumerable<PreorderPickupList> GetPreOrderPickupList(int iDisplayStart, int pageSize, int sortColumnIndex, string sortDirection, PreorderPickupFilters filters, out int totalrecords)
        {

                string location                 = filters.location == null ? "" : filters.location;
                string dateRangeTypes           = filters.dateRangeTypes == null ? "" : filters.dateRangeTypes;
                string fromDate                 = filters.fromDate;
                string toDate                   = filters.toDate;
                string homeRoom                 = filters.homeRoom == null ? "" : filters.homeRoom;
                string customerSelectionType    = filters.customerSelectionType;
                string SelectedCustomersList    = filters.SelectedCustomersList == null ? "" : filters.SelectedCustomersList;
                string grade                    = filters.grade == null ? "" : filters.grade;
                string itemSelectionType        = filters.itemSelectionType;
                string itemStatusType           = filters.itemStatusType;
                string selectedTypeList         = filters.selectedTypeList;
                string sortColumnName           = getColmnName(sortColumnIndex);

                int PageNo = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(iDisplayStart) / Convert.ToDouble(pageSize)) + 1);
           //  int PageNo = iDisplayStart;
           //Count Procedure

                totalrecords = GetPreOrderPickupItemsCount(filters).recordCount.Value;    
            /*
                totalrecords =int.Parse(this.context.Main_PreOrder_Count
                    (
                      location,
                       dateRangeTypes,
                       fromDate,
                       toDate,
                       homeRoom,
                       customerSelectionType,
                       SelectedCustomersList,
                       grade,
                       itemSelectionType,
                       itemStatusType,
                       selectedTypeList

                    ).FirstOrDefault().Value.ToString());

              */ 
 

           //List Procedure
            var dataSet = this.context.Main_PreOrder_List(
                  location,
                  dateRangeTypes,
                  fromDate,
                  toDate,
                  homeRoom,
                  customerSelectionType,
                  SelectedCustomersList,
                  grade,
                  itemSelectionType,
                  itemStatusType,
                  selectedTypeList,
                  PageNo,
                  pageSize,
                  sortColumnName,
                  sortDirection).
                  
                  Select(x => new PreorderPickupList()
                  {
                      preOrderId = x.preOrderId,
                      PreOrderItemId = x.PreOrderItemId,
                      
                      transactionId     = x.transactionId,
                      Grade             = x.Grade,
                      customerName      = x.customerName,
                      userId            = x.userId,
                      CategoryType_Id   = x.CategoryType_Id,
                      Category_Id       = x.Category_Id,
                      itemName          = x.itemName,
                      datePurchased     = x.datePurchased,
                      dateToServe       = x.dateToServe,
                      datePickedUp      = x.datePickedUp,
                      received          = x.received,
                      itemVoid          = x.itemVoid,
                      qty               = x.qty,
                      orderVoid         = x.orderVoid,
                      @void             = x.@void

                  }

                  );
                       


            return dataSet;

        }

        //Get Order For Void List
     public  IEnumerable<LoadOrderVoidList> GetOrderForVoidList(int iDisplayStart, int pageSize, int sortColumnIndex, string sortDirection, string parm, out int totalrecords)
       {

           var dataSet = this.context.Main_LoadOrder_Void_List(parm).
               Select(
                       x => new LoadOrderVoidList()
                       {

                           Id = x.Id,
                           PreSaleTrans_Id = x.PreSaleTrans_Id,
                           OrderId = x.OrderId,
                           Grade = x.Grade,
                           CustomerName = x.CustomerName,
                           UserID = x.UserID,
                           PurchasedDate = x.PurchasedDate,
                           Void = x.Void,
                           CanVoid = x.CanVoid,
                           HasPayment = x.HasPayment

                       }).ToList<LoadOrderVoidList>();
          

           totalrecords = dataSet.Count();

           return dataSet;    
              
            }

       //Get Item For Void List
     public IEnumerable<LoadItemVoidList> GetItemForVoidList(int iDisplayStart, int pageSize, int sortColumnIndex, string sortDirection, string parm, out int totalrecords)
     {

       var dataSet =  this.context.Main_LoadItem_Void_List(parm).Select(

             x => new LoadItemVoidList()
             {

                Id                  =   x.Id,
                PreorderId          =   x.PreorderId,
                CustomerName        =   x.CustomerName,
                customerId          =   x.customerId,
                UserID              =   x.UserID,
                Grade               =   x.Grade,
                PreSaleTrans_Id     =   x.PreSaleTrans_Id,
                orderId             =   x.orderId,
                ItemName            =   x.ItemName, 
                purchasedDate       =   x.purchasedDate,
                ServingDate         =   x.ServingDate,
                Qty                 =   x.Qty,
                CanVoid             =   x.CanVoid,
                isVoid              =   x.isVoid,

             }).ToList<LoadItemVoidList>();

       totalrecords = dataSet.Count();

       return dataSet;
     }

     public VoidUpdateResult UpdateVoidOrder(Nullable<int> clientID, Nullable<int> orderID, Nullable<int> orderLogID, Nullable<int> orderType, Nullable<bool> voidPayment)
     {

         VoidUpdateResult resultSet = this.context.VoidOrder(clientID, orderID, orderLogID, orderType, voidPayment).Select(
             x => new VoidUpdateResult()
             {

                 Result = x.Result,
                 ErrorMessage = x.ErrorMessage

             }).FirstOrDefault();

         return resultSet;
     }
     public VoidUpdateResult UpdateVoidItem(Nullable<int> clientID, Nullable<int> itemID, Nullable<int> orderID, Nullable<int> orderLogID, Nullable<int> CustID, Nullable<int> orderType)
     {
          VoidUpdateResult resultSet = this.context.VoidItem(clientID,itemID,orderID,orderLogID,CustID, orderType).Select(
             x => new VoidUpdateResult()
             {

                 Result = x.Result,
                 ErrorMessage = x.ErrorMessage

             }).FirstOrDefault();

         return resultSet;

     }

     public IEnumerable<PreorderPickupReport> GetPreOrderPickupReportList(string preorderStr)
     {

         var resultSet = this.context.Main_PreOrder_Report(preorderStr).Select
             (x => new PreorderPickupReport()
             {
                 

                 transactionId = x.transactionId,
                 Grade = x.Grade,
                 customerName = x.customerName,
                 userId = x.userId,
                 itemName = x.itemName,
                 datePurchased = x.datePurchased,
                 datePickedUp = x.datePickedUp,
                 Ordered = x.Ordered,
                 received = x.received,
                 voidSts = x.@void

             });

         return resultSet;
     }


     public IEnumerable<PreorderPickupItemsList> GetPreorderPickupItemsList(long clientID, string preorderItemsList, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, out int totalRecords)
     {
         totalRecords = 0;
         try
         {
             string SortColumn = GetPickupItemsColumnName(sortColumnIndex);
             sortDirection = sortDirection == "asc" ? "ASC" : "DESC";
             iDisplayStart = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(iDisplayStart) / Convert.ToDouble(iDisplayLength)) + 1);
             List<Admin_PreorderItems_List_Result> result = this.context.Admin_PreorderItems_List(clientID, preorderItemsList, iDisplayStart, iDisplayLength, SortColumn, sortDirection).ToList();
             var obj = result.FirstOrDefault();
             if (obj != null)
             {
                 totalRecords = obj.AllRecordsCount.HasValue ? obj.AllRecordsCount.Value : 0;
             }

             return
             result.Select(x => new PreorderPickupItemsList()
             {
                 Id = x.Id,
                 Category_Id = x.Category_Id,
                 CustomerId = x.CustomerId,
                 CustomerName = x.CustomerName,
                 DatePickedUp = x.DatePickedUp,
                 DatePurchased = x.DatePurchased,
                 DateToServe = x.DateToServe,
                 Grade = x.Grade,
                 ItemName = x.ItemName,
                 ItemVoid = x.ItemVoid,
                 KitchenItem = x.KitchenItem,
                 MealPlan_Id = x.MealPlan_Id,
                 MenuId = x.MenuId,
                 OrderVoid = x.OrderVoid,
                 PickupCountValid = x.PickupCountValid,
                 PickupQty = x.PickupQty,
                 PreOrderId = x.PreOrderId,
                 Qty = x.Qty,
                 Received = x.Received,
                 SchoolId = x.SchoolId,
                 Selected = x.Selected,
                 StudentFullPrice = x.StudentFullPrice,
                 TransactionId = x.TransactionId,
                 UserId = x.UserId,
                 Void = x.Void
             });


         }
         catch (Exception ex)
         {
             //Error logging in cloud tables
             ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreOrderManagement", "Error : " + ex.Message, null, "GetPreorderPickupItemsList");
             return null;
         }
     }
    //Preorder Dashboard procedure Calling
   public  IEnumerable<CurrentPreorder> GetCurrentPreorderOverviewList()
     {
         
         try
         {
             var resultSet = this.context.Main_PreOrder_CurrentPreorderOverview().
                           Select(x => new CurrentPreorder()
                           {
                               PeriodTypeID = x.PeriodTypeID,
                               PeriodTypeName = x.PeriodTypeName,
                               openCount = x.openCount,
                               closedCount = x.closedCount
                           });

             return resultSet;
              

         }
         catch (Exception ex)
         {
             ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreOrderManagement", "Error : " + ex.Message, null, "GetCurrentPreorderOverviewList");
             return null;
         }

        
     }
    public IEnumerable<AvgInPreorder> GetAvgInPreorderOverviewList()
     {

         try
         {
             var resultSet = this.context.Main_PreOrder_AvgInPreorderOverview().
                 Select(x => new AvgInPreorder()
                 {
                     PeriodTypeID   = x.PeriodTypeID,
                     PeriodTypeName = x.PeriodTypeName,
                     AvgCount       = x.AvgCount

                 });

             return resultSet;

         }
         catch (Exception ex)
         {
             ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreOrderManagement", "Error : " + ex.Message, null, "GetAvgInPreorderOverviewList");
             return null;
         }
       
     }
    public IEnumerable<TopSellingItem> GetTopSellingItemOverviewList(int preiodTypeID)
     {
         try
         {
             var resultSet = this.context.Main_PreOrder_TopSellingItemOverview(preiodTypeID).
                 Select(x => new TopSellingItem()
                 {
                     ItemName = x.ItemName,
                     qtySold = x.qtySold
                 });

             return resultSet;
         }
         catch (Exception ex)
         {
             ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreOrderManagement", "Error : " + ex.Message, null, "GetTopSellingItemOverviewList");
             return null;
         }
         
     }
    //End Dashboard

    public IEnumerable<Menu> GetMenuItems(int? categoryID, int? categoryTypeID)
    {
        try
        {
          var ret= this.context.Main_PreOrder_MenuItem_List(categoryID, categoryTypeID).
                Select(x => new Menu() { ID = x.ID, ItemName = x.ItemName });

          return ret;
        }
        catch (Exception ex)
        {

            ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreOrderManagement", "Error : " + ex.Message, null, "GetMenuItemPreoder");
            return null;
        }

    }   
     private string GetPickupItemsColumnName(int sortColumnIndex)
     {
         string column = "CustomerName";
         switch (sortColumnIndex)
         {
             case 1:
                 column = "TransactionId";
                 break;
             case 2:
                 column = "Grade";
                 break;
             case 3:
                 column = "CustomerName";
                 break;
             case 4:
                 column = "UserId";
                 break;
             case 5:
                 column = "ItemName";
                 break;
             case 6:
                 column = "DatePurchased";
                 break;
             case 7:
                 column = "DateToServe";
                 break;
             case 8:
                 column = "DatePickedUp";
                 break;
             case 9:
                 column = "Qty";
                 break;
             case 10:
                 column = "Received";
                 break;
             default:
                 column = "CustomerName";
                 break;
         }

         return column;
     }

     public int ProcessPickupPreorderItems(long clientID, int cashierId, DateTime localDateTime, List<SelectedPreorderItems> selectedPreorderItems, out int status)
     {
         DataTable dt = new DataTable("SelectedPreorderItems");
         if (selectedPreorderItems != null)
         {
             dt.Columns.Add("Id", typeof(int));
             dt.Columns.Add("Qty", typeof(int));

             foreach (var item in selectedPreorderItems)
             {
                 var row = dt.NewRow();
                 row["Id"] = item.Id;
                 row["Qty"] = item.Qty;

                 dt.Rows.Add(row);
             }
         }

         var ClientID = new SqlParameter("ClientID", SqlDbType.BigInt);
         ClientID.Value = (object)clientID ?? DBNull.Value;

         var CashierId = new SqlParameter("CashierId", SqlDbType.BigInt);
         CashierId.Value = (object)cashierId ?? DBNull.Value;

         var LocalDateTime = new SqlParameter("LocalDateTime", SqlDbType.DateTime2);
         LocalDateTime.Value = (object)localDateTime ?? DBNull.Value;

         var SelectedPreorderItems = new SqlParameter("SelectedPreorderItems", SqlDbType.Structured);
         SelectedPreorderItems.TypeName = "dbo.PreorderPickupItemType";
         SelectedPreorderItems.Value = (object)dt ?? DBNull.Value;

         var Result = new SqlParameter("Result", SqlDbType.Int);
         Result.Direction = ParameterDirection.Output;

         var ErrorMessage = new SqlParameter("ErrorMsg", SqlDbType.VarChar);
         ErrorMessage.Direction = ParameterDirection.Output;
         ErrorMessage.Size = 4000;

         try
         {

             this.context.ExecuteProcessPreorderPickupItems("ProcessPickupPreorderItems", ClientID, CashierId, LocalDateTime, SelectedPreorderItems, Result, ErrorMessage);
            
             if (ErrorMessage.Value.ToString() == "")
             {
                 status = 1;
             }
             else
             {
                 status = 0;
             }
         }
         catch (Exception ex)
         {
             status = 0;
             //Error logging in cloud tables
             ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreOrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ProcessPickupPreorderItems");
         }
         return status;
     }

        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 2:
                    retVal = "transactionId";
                    break;
                case 3:
                    retVal = "Grade";
                    break;
                case 4:
                    retVal = "customerName";
                    break;
                case 5:
                    retVal = "userId";
                    break;
                case 6:
                    retVal = "itemName";
                    break;
                case 7:
                    retVal = "datePurchased";
                    break;
                case 8:
                    retVal = "dateToServe";
                    break;
                case 9:
                    retVal = "datePickedUp";
                    break;
                case 10:
                    retVal = "qty";
                    break;
                case 11:
                    retVal = "received";
                    break;

                case 12:
                    retVal = "void";
                    break;
                default:
                    retVal = "transactionId";
                    break;
            }

            return retVal;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}

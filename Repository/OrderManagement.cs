using AdminPortalModels.ViewModels;
using AdminPortalModels.Models;
using Repository.edmx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Repository.Helpers;


namespace Repository
{
    public class OrderManagement : IOrderManagement
    {
        private PortalContext context;

        public OrderManagement(PortalContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// This function returns group list of search by 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="gpFilters"></param>
        /// <returns></returns>
        public IEnumerable<VoidsOrdersGroup> GetVoidGroup_List(Nullable<long> clientID, JQueryDataTableParamModel param, GroupFilters gpFilters, out Int32 totalRecords) //DateTime DateStart, DateTime DateEnd, string SearchString, Nullable<int> searchBy)
        {
            
            IQueryable<Admin_VoidGroup_List_Result> query = this.context.Admin_VoidGroup_List(clientID, gpFilters.DateStart, gpFilters.DateEnd, gpFilters.SearchStr, gpFilters.SearchBy_Id).ToList().Where(e => !String.IsNullOrEmpty(e.GroupName)).ToList().AsQueryable();

            totalRecords = query.Count();

            if (gpFilters.IsActive != null)
            {
                query = query.Where(e => e.Active == gpFilters.IsActive);
                totalRecords = query.Count();
            }

            IEnumerable<VoidsOrdersGroup> result = new List<VoidsOrdersGroup>();
            if (gpFilters.CustomerID != 0)
            {
                query = query.Where("GroupID=" + gpFilters.CustomerID);
                totalRecords = query.Count();
            }
            try
            {
                result = query.Select(c => new VoidsOrdersGroup
                {
                    groupName = c.GroupName,
                    groupID = c.GroupID,
                    UserID = c.UserID,
                    CustomerBalance = c.CustomerBalance

                }).OrderBy(g => g.groupName).Skip(param.iDisplayStart).Take(param.iDisplayLength) as IEnumerable<VoidsOrdersGroup>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetVoidGroup_List");
                string msg = ex.Message;
            }
            return result;

        }
        /// <summary>
        /// This function checks whether user has activity records
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public int GetCustomerActivityCount(Nullable<long> clientID, string customerID)
        {
            DateTime? startDate = DateTime.Now.AddYears(-10);
            DateTime? endDate = DateTime.Now;
            int tempCount = 0;

            try
            {
                IQueryable<Admin_VoidGroup_List_Result> query = this.context.Admin_VoidGroup_List(clientID, startDate, endDate, "", 2);
                query = query.Where("GroupID=" + customerID);
                tempCount = query.Count();
            }

            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerActivityCount");
                string msg = ex.Message;
            }
            return tempCount;
        }

        /// <summary>
        /// This function checks whether user has activity records
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public int GetCustomerActivityCount(long clientID, long customerID)
        {
            
            int tempCount = 0;

            try
            {

                string sqlQuery = "SELECT [dbo].[GetCustomerActivityCount] ({0},{1})";
                Object[] parameters = { clientID, customerID };
                tempCount = context.Database.SqlQuery<int>(sqlQuery, parameters).FirstOrDefault();

               
            }

            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerActivityCount");
                string msg = ex.Message;
            }
            return tempCount;
        }

        /// <summary>
        /// This function returns detail of a group 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="GroupID"></param>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        /// <param name="searchBy"></param>
        /// <returns></returns>
        public IEnumerable<CustomerOrderDetails> GetVoidGroup_Details(Nullable<long> clientID, Int32? GroupID, DateTime stDate, DateTime endDate, int searchBy)
        {
            try
            {
                return this.context.Admin_VoidGroup_Detail(clientID, GroupID, stDate, endDate, searchBy).Select(l => new CustomerOrderDetails
                    {

                        OrderID = l.OrderID,
                        OrderLogID = l.OrderLogID,
                        OrderType = l.OrderType,
                        Name = l.Name,
                        OrderDate = l.OrderDate,
                        OrderDateLocal = l.OrderDate, 
                        Item = l.Item,
                        SalesTax = l.SalesTax,
                        Total = l.Total,
                        Payment = l.Payment,
                        Type = l.Type,
                        isVoid = l.isVoid == null ? false : l.isVoid,
                        Check = l.Check,
                        POS = l.POS
                    }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetVoidGroup_Details");
                return null;

            }

        }

        /// <summary>
        /// This function returns order information (header information)
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="orderID"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        public OrderInfo OrderInfo(Nullable<long> clientID, Nullable<int> orderID, Nullable<int> orderType)
        {
            try
            {
                var orderInfo = this.context.Main_Order_View(clientID, orderID, orderType).Select(o => new
                    {
                        OrderNumber = o.OrderNumber,
                        OrderDate = o.OrderDate,
                        OrderDateLocal =  o.OrderDateLocal,
                        OrderTotal = o.OrderTotal,
                        Payment = o.Payment,
                        POSName = o.POSName,
                        CashierName = o.CashierName,
                        CashierID = o.CashierID,
                        OrdersLogID = o.OrdersLogID
                    }).FirstOrDefault();

                if (orderInfo != null)
                {
                    // Inayat [31-Aug-2016] Previously OrderDate was being passed as a DateTime to parse in JSon, which resulted in
                    // two different results on local and Azure at the client side. So I replaced Datetime with string to avoid conversion issues by Json at Azure.
                    return new OrderInfo()
                    {
                        OrderNumber = orderInfo.OrderNumber,
                        //OrderDate = orderInfo.OrderDate.ToString(),
                        OrderDate = String.Format("{0:MM/dd/yyyy @ hh:mm tt}", orderInfo.OrderDate),
                        OrderDateLocal = orderInfo.OrderDateLocal,
                        OrderTotal = orderInfo.OrderTotal,
                        Payment = orderInfo.Payment,
                        POSName = orderInfo.POSName,
                        CashierName = orderInfo.CashierName,
                        CashierID = orderInfo.CashierID,
                        OrdersLogID = orderInfo.OrdersLogID
                    };
                }

                return null;

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "OrderInfo");
                return null;
            }
        }

        /// <summary>
        /// This function returns order details information (header information)
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="orderID"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>

        public IEnumerable<OrderDetailsPopup> OrderDetailsPopup(Nullable<long> clientID, Nullable<int> orderID, Nullable<int> orderType)
        {
            try
            {
                return this.context.Main_Items_View(clientID, orderID, orderType).Select(o => new OrderDetailsPopup
                    {
                        id = o.id,
                        Qty = o.Qty,
                        ItemName = o.ItemName,
                        PaidPrice = o.PaidPrice,
                        Void = o.Void,
                        ServingDate = o.ServingDate,
                        PickedupDate = o.PickedupDate
                    });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "OrderDetailsPopup");
                return null;
            }
        }
        /// <summary>
        /// function is used to void single item
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="iTEMID"></param>
        /// <param name="eMPLOYEEID"></param>
        /// <param name="oRDERTYPE"></param>
        /// <param name="oRDLOGID"></param>
        /// <param name="oRDLOGNOTE"></param>
        /// <returns></returns>
        public bool VoidItem(Nullable<long> clientID, Nullable<long> iTEMID, Nullable<long> eMPLOYEEID, Nullable<int> oRDERTYPE, Nullable<long> oRDLOGID, string oRDLOGNOTE)
        {
            bool success = true;

            //Main_Items_Void_Result tempResult = new Main_Items_Void_Result();
            string msg = "";
            try
            {

                msg = this.context.Main_Items_Void(clientID, iTEMID, eMPLOYEEID, oRDERTYPE, oRDLOGID, oRDLOGNOTE).Select(s => s.ErrorMessage).FirstOrDefault();
                if (msg != "")
                {
                    success = false;
                }
                else
                {
                    success = true;
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "VoidItem");
            }
            return success;
        }


        /// <summary>
        /// function is used to void an order
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="eMPLOYEEID"></param>
        /// <param name="oRDERID"></param>
        /// <param name="oRDERTYPE"></param>
        /// <param name="vOIDPAYMENT"></param>
        /// <param name="oRDLOGID"></param>
        /// <param name="oRDLOGNOTE"></param>
        /// <returns></returns>
        public bool VoidAllOrder(Nullable<long> clientID, Nullable<long> eMPLOYEEID, Nullable<long> oRDERID,
            Nullable<int> oRDERTYPE, Nullable<bool> vOIDPAYMENT, Nullable<long> oRDLOGID, string oRDLOGNOTE)
        {
            bool success = true;


            var ClientID = new SqlParameter("ClientID", SqlDbType.BigInt);
            ClientID.Value = (object)clientID ?? DBNull.Value;

            var EMPLOYEEID = new SqlParameter("EMPLOYEEID", SqlDbType.BigInt);
            EMPLOYEEID.Value = (object)eMPLOYEEID ?? DBNull.Value;

            var ORDERID = new SqlParameter("ORDERID", SqlDbType.BigInt);
            ORDERID.Value = (object)oRDERID ?? DBNull.Value;

            var ORDERTYPE = new SqlParameter("ORDERTYPE", SqlDbType.Int);
            ORDERTYPE.Value = (object)oRDERTYPE ?? DBNull.Value;

            var VOIDPAYMENT = new SqlParameter("VOIDPAYMENT", SqlDbType.Bit);
            VOIDPAYMENT.Value = (object)vOIDPAYMENT ?? DBNull.Value;


            var ORDLOGID = new SqlParameter("ORDLOGID", SqlDbType.BigInt);
            ORDLOGID.Value = (object)oRDLOGID ?? DBNull.Value;


            var ORDLOGNOTE = new SqlParameter("ORDLOGNOTE", SqlDbType.VarChar);
            ORDLOGNOTE.Value = (object)oRDLOGNOTE ?? DBNull.Value;

            var ErrorMessage = new SqlParameter("ErrorMessage", SqlDbType.VarChar);

            ErrorMessage.Direction = ParameterDirection.Output;
            ErrorMessage.Size = 4000;


            try
            {

                this.context.ExecuteVoidProcedure("Main_Order_Void", ClientID, EMPLOYEEID, ORDERID, ORDERTYPE, VOIDPAYMENT, ORDLOGID, ORDLOGNOTE, ErrorMessage);
                if (ErrorMessage.Value.ToString() == "")
                {
                    success = true;
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception ex)
            {
                success = false;
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, CommonClasses.getCustomerID(), "VoidAllOrder");
            }
            return success;

        }

        /// <summary>
        /// Get Orders Detail list for POS
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="customerID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<DetailOrdersModel> GetOrdersDetailList(Nullable<long> clientID, int customerID, DateTime startDate, DateTime endDate, out int status)
        {
            try
            {
                IEnumerable<StatementDetailOrder> detailOrdersList = this.context.StatementDetailOrders.AsNoTracking().Where(e => e.ClientID == clientID && e.CSTID == customerID && e.OrderDate >= startDate && e.OrderDate <= endDate).OrderByDescending(e => e.OrderDate).ToList();
                if (detailOrdersList != null && detailOrdersList.Count() > 0)
                {
                    IEnumerable<StatementDetailItem> detailItemList = this.context.StatementDetailItems.AsNoTracking().Where(e => e.ClientID == clientID && e.CSTID == customerID && e.OrderDate >= startDate && e.OrderDate <= endDate).OrderByDescending(e => e.OrderDate).ToList();
                    List<DetailOrdersModel> orderList = new List<DetailOrdersModel>();
                    foreach (var orderItem in detailOrdersList)
                    {
                        DetailOrdersModel order = new DetailOrdersModel();
                        order.ClientID = orderItem.ClientID;
                        order.CustomerID = orderItem.CSTID;
                        order.SchoolID = orderItem.SCHID;
                        order.OrderID = orderItem.ORDID;
                        order.OrderTypeName = orderItem.OrderTypeName;
                        order.OrderType = orderItem.OrderType;
                        order.GDate = orderItem.GDate;
                        order.OrderDate = orderItem.OrderDate;
                        order.TransType = orderItem.TransType;
                        order.PaymentType = orderItem.PaymentType;
                        order.TotalAccount = orderItem.TotalAccount;
                        order.TotalPaid = orderItem.TotalPaid;
                        order.SalesTax = orderItem.SalesTax;
                        order.BalanceChange = orderItem.BalanceChange;
                        order.DetailItemsList = getDetailItemsByOrderID(orderItem.ORDID, customerID, detailItemList);
                        orderList.Add(order);
                    }
                    status = 1;
                    return orderList;
                }
                else
                {
                    status = 0;
                    return null;
                }
            }
            catch (Exception ex)
            {
                status = -1;
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, customerID.ToString(), "GetOrdersDetailList");
                return null;
            }
            //status = 1;
            //return null;
        }
        /// <summary>
        /// Get detail items by orderId
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="detailItemList"></param>
        /// <returns></returns>
        private List<DetailItemsModel> getDetailItemsByOrderID(long OrderID, int customerId, IEnumerable<StatementDetailItem> detailItemList)
        {
            try
            {
                detailItemList = detailItemList.Where(e => e.ORDID == OrderID).ToList();
                if (detailItemList != null && detailItemList.Count() > 0)
                {
                    List<DetailItemsModel> detailItemModelList = new List<DetailItemsModel>();
                    foreach (var detailItem in detailItemList)
                    {
                        DetailItemsModel tempDetailItem = new DetailItemsModel();

                        tempDetailItem.ClientID = detailItem.ClientID;
                        tempDetailItem.CustomerID = detailItem.CSTID;
                        tempDetailItem.SchoolID = detailItem.SCHID;
                        tempDetailItem.OrderID = detailItem.ORDID;
                        tempDetailItem.GDate = detailItem.GDate;
                        tempDetailItem.OrderDate = detailItem.OrderDate;
                        tempDetailItem.TransType = detailItem.TransType;
                        tempDetailItem.OrderTypeName = detailItem.OrderTypeName;
                        tempDetailItem.OrderType = detailItem.OrderType;
                        tempDetailItem.ItemName = detailItem.ItemName;
                        tempDetailItem.PaidPrice = detailItem.PaidPrice;
                        tempDetailItem.Quantity = detailItem.Qty;
                        tempDetailItem.ExtendedPrice = detailItem.ExtendedPrice;
                        detailItemModelList.Add(tempDetailItem);
                    }
                    return detailItemModelList;
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "OrderManagement", "Error : " + ex.Message, customerId.ToString(), "getDetailItemsByOrderID");
            }
            return null;
        }

        private bool disposed = false;

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

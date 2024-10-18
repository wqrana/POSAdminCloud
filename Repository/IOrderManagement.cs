using AdminPortalModels.ViewModels;
using AdminPortalModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.edmx;

namespace Repository
{
    public interface IOrderManagement : IDisposable
    {

        IEnumerable<VoidsOrdersGroup> GetVoidGroup_List(Nullable<long> clientID, JQueryDataTableParamModel param, GroupFilters gpFilters, out Int32 totalRecords);
        int GetCustomerActivityCount(Nullable<long> clientID, string customerID);
        int GetCustomerActivityCount(long clientID, long customerID);

        IEnumerable<CustomerOrderDetails> GetVoidGroup_Details(Nullable<long> clientID, Int32? GroupID, DateTime stDate, DateTime endDate, int searchBy);

        OrderInfo OrderInfo(Nullable<long> clientID, Nullable<int> orderID, Nullable<int> orderType);
        IEnumerable<OrderDetailsPopup> OrderDetailsPopup(Nullable<long> clientID, Nullable<int> orderID, Nullable<int> orderType);

        bool VoidItem(Nullable<long> clientID, Nullable<long> iTEMID, Nullable<long> eMPLOYEEID, Nullable<int> oRDERTYPE, Nullable<long> oRDLOGID, string oRDLOGNOTE);
        bool VoidAllOrder(Nullable<long> clientID, Nullable<long> eMPLOYEEID, Nullable<long> oRDERID, Nullable<int> oRDERTYPE, Nullable<bool> vOIDPAYMENT, Nullable<long> oRDLOGID, string oRDLOGNOTE);

        List<DetailOrdersModel> GetOrdersDetailList(Nullable<long> clientID, int customerID, DateTime startDate, DateTime endDate, out int status);
    }
}

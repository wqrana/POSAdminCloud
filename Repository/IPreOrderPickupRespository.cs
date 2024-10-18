using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.ViewModels;
using Repository.edmx;

namespace Repository
{
  public interface IPreOrderPickupRespository : IDisposable
    {

       IEnumerable<PreorderPickupList> GetPreOrderPickupList(int iDisplayStart, int pageSize, int sortColumnIndex, string sortDirection, PreorderPickupFilters filters, out int totalrecords);
       PreorderPickupItemsCount GetPreOrderPickupItemsCount(PreorderPickupFilters filters);
       IEnumerable<LoadOrderVoidList> GetOrderForVoidList(int iDisplayStart, int pageSize, int sortColumnIndex, string sortDirection, string parm, out int totalrecords);
       IEnumerable<LoadItemVoidList> GetItemForVoidList(int iDisplayStart, int pageSize, int sortColumnIndex, string sortDirection, string parm, out int totalrecords);
       VoidUpdateResult UpdateVoidOrder(Nullable<int> clientID, Nullable<int> orderID, Nullable<int> orderLogID, Nullable<int> orderType, Nullable<bool> voidPayment);
       VoidUpdateResult UpdateVoidItem(Nullable<int> clientID, Nullable<int> itemID, Nullable<int> orderID, Nullable<int> orderLogID, Nullable<int> CustID, Nullable<int> orderType);
       IEnumerable<PreorderPickupItemsList> GetPreorderPickupItemsList(long clientID, string preorderItemsList, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, out int totalRecords);
       int ProcessPickupPreorderItems(long clientID, int cashierId, DateTime localDatetTime, List<SelectedPreorderItems> selectedPreorderItems, out int status);
      IEnumerable<PreorderPickupReport> GetPreOrderPickupReportList(string preorderStr);
      IEnumerable<CurrentPreorder> GetCurrentPreorderOverviewList();
      IEnumerable<AvgInPreorder> GetAvgInPreorderOverviewList();
      IEnumerable<TopSellingItem> GetTopSellingItemOverviewList(int preiodTypeID);
      IEnumerable<Menu> GetMenuItems(int? categoryID, int? categoryTypeID);

    }
}

using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface ICustomerRepository : IDisposable
    {
        IEnumerable<Customer_List> GetCustomers(Nullable<long> clientID, string searchString, Nullable<int> searchBy);
        IEnumerable<Customer_List> GetCustomerPage(Nullable<long> clientID, int iDisplayStart, int pageSize, int sortColumnIndex, string sortDirection, CustomerFilters filters, out int totalrecords);
        Customer_Detail_VM GetCustomer(Nullable<long> clientID, Nullable<long> customerID);
        IEnumerable<EatingSchool> GetEatingSchools(Nullable<long> clientID, Nullable<long> district_Id, Nullable<bool> includeDeleted, long schoolID);
        IEnumerable<AssignedSchool> GetAssignedSchools(Nullable<long> clientID, Nullable<int> CustomerID);
        string SaveCustomerData(CustomerData customerData, out Int32 customerId);
        IEnumerable<Customer_Logs> GetCustomerLogs(Nullable<long> clientID, Nullable<int> CustomerID, out int totalRecords);
        string DeleteCustomer(string CustomerData);
        long GetSchoolsDistrict(Nullable<long> clientID, Nullable<long> SchoolID);
        string GetDistrictName(Nullable<long> clientID, Nullable<long> DistrictID);
        bool UserIdAlreadyExists(long ClientID, string userID, Int32 custId, Int32 distID);
        bool PINAlreadyExists(long ClientID, string userID, Int32 custId, Int32 distID);
        Int32 FARM_AppCount(Nullable<long> clientID, Nullable<int> CustomerID);
        List<POSCustomer> GetPosStudents(long districtID, string ListClientCustids, string ListDistCustids);

        
    }
}

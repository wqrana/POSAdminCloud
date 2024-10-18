using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.ViewModels;
using Repository.edmx;

namespace Repository
{
    public interface IGeneralRepository : IDisposable
    {
        IEnumerable<Gender> getGendersList();
        IEnumerable<Language> getLanguages(long? ClientId);
        IEnumerable<AdminPortalModels.ViewModels.Ethnicity> getEthnicities(long? ClientId);
        IEnumerable<AdminPortalModels.ViewModels.Grade> getGrades(long? ClientId);
        IEnumerable<State> getStates();
        IEnumerable<DistrictItem> getDistricts(long? ClientId);
        IEnumerable<SchoolItem> getSchools(long? ClientId, Nullable<long> district_Id, Nullable<bool> includeDeleted);
        IEnumerable<HomeRoomModel> getHomeRooms(long? ClientId);
        List<SearchBy> GetSearchDDLItems();
        List<string> GetSortOrderList();
        string GetCustomerName(long clientID, int customerID);

        Customer_Detail_VM GetCustomerDetailForPayment(long clientID, int customerID);
        string Save_Order(long ClientID, int? OrderID, int? POSID, int EmpID, int CustID, int TransType, double? mDebit, double? aDebit, int? MealPlan, int? CashRes,
            int? LunchType, int? SchoolID, DateTime? OrdDate, int? CreditAuth, int? CheckNum, bool OverRide, bool Void, int? LogID, string LogNotes);

        int getItemsCountofCategoryType(long ClientID, long categoryID);

        int getItemsCountofCategory(long ClientID, long categoryID);
        IEnumerable<IncomeFrequency> GetIncomeFrequencies();
    }
}

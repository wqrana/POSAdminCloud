using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.edmx;
using AdminPortalModels.ViewModels;

namespace Repository
{
    public interface ISettingsRepository :IDisposable
    {
        IEnumerable<School> GetSchools(long clientid);
        School GetSchollByID(int Schoolid);
        IEnumerable<POSVM> GetPOSbySchoolID(long Schoolid, long ClientID);

        //IEnumerable<POSDashboardVM> GetDashboardOpenCashierSession(Nullable<long> clientID, Nullable<long> schoolID);

        int GetAllPOSCount(long ClientID);
        IEnumerable<POS> GetFullPOSbyID(int ClientID, int posID);
        void Savepos(POS pos);

        long GetFirstDistrictID(long clientid);
        long GetFirstSchoolID(long clientid);
        DistrictVM getDistrict(long clientid, long districtId);
        DistrictOptionsVM getDistrictOptions(long clientid, long districtId);
        IList<DistrictNames> getDistrictNames(long clientid);

        DistrictsData getDistrictsData(long clientid, long districtId);
        string SaveDistrictData(string districtData, out Int32 districtId);

        string ReactivateDistrict(string data);

        string SavePOSData(string posData);

        IEnumerable<District> GetDistricts(long clientid);
        IEnumerable<SchoolIndexData> GetSchoolbyDistrictID(long Districtid, long ClientID);
        SchoolsData getSchoolsData(long clientid, long schoolId);
        string SaveSchoolData(string schoolData, out Int32 schoolId);
        string DeleteSchool(string SchoolIDs);
        string getCustomerName(long Clientid, long? Customerid);
        DistrictExecs getDistrictExecs(long Clientid, long districtId);
        IEnumerable<Customer_List> getAdultCustomers(long clientId);

        Admin_POS_Delete_Result DeletePOS(string posIDs);

        IEnumerable<POSVM> GetPOSListPage(Nullable<long> clientID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, out int totalrecords);
        IQueryable<POSVM> GetPOSList(Nullable<long> clientID, out int totalrecords);
        string SessionStatus(int posID, long ClientID);
    }
}

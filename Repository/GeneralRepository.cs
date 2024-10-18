using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.edmx;
using AdminPortalModels.ViewModels;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Data.SqlClient;
using Repository.Helpers;


namespace Repository
{
    public class GeneralRepository : IGeneralRepository, IDisposable
    {
        private PortalContext context;

        public GeneralRepository(PortalContext context)
        {
            this.context = context;
        }
        public IEnumerable<Gender> getGendersList()
        {
            var list = new List<Gender>();
            list.Add(new Gender("F", "Female"));
            list.Add(new Gender("M", "Male"));
            return list.AsEnumerable<Gender>();
        }
        public IEnumerable<Language> getLanguages(long? ClientId)
        {
            try
            {
                return this.context.Admin_Language_List(ClientId).Select(l => new Language
                    {
                        value = l.Language_Id,
                        data = l.Language_Name
                    }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getLanguages");
                return null;
            }
        }
        public IEnumerable<AdminPortalModels.ViewModels.Ethnicity> getEthnicities(long? ClientId)
        {
            try
            {
                return this.context.Admin_Ethnicity_List(ClientId).Select(e => new AdminPortalModels.ViewModels.Ethnicity
                    {
                        value = e.Ethnicity_Id,
                        data = e.Ethnicity_Name
                    }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getEthnicities");
                return null;
            }
        }

        public IEnumerable<AdminPortalModels.ViewModels.Grade> getGrades(long? ClientId)
        {
            try
            {
                return this.context.Admin_Grade_List(ClientId).Select(g => new AdminPortalModels.ViewModels.Grade
                    {
                        value = g.Grade_Id,
                        data = g.Grade

                    }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getGrades");
                return null;
            }
        }
        public IEnumerable<State> getStates()
        {
            try
            {
                var StateList = new List<State>();
                List<string> districtStates = FSS.FSS.stateList.ToList();
                StateList.Add(new State { value = "", data = ""});
                foreach (var item in districtStates)
                {
                    StateList.Add(new State { value = item, data = item });
                }

                return StateList;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getStates");
                return null;
            }
        }

        public IEnumerable<DistrictItem> getDistricts(long? ClientId)
        {
            try
            {
                return this.context.Admin_District_List(ClientId).Select(d => new DistrictItem
                    {
                        value = d.District_Id,
                        data = d.District_Name
                    }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistricts");
                return null;
            }

        }

        public IEnumerable<SchoolItem> getSchools(long? ClientId, Nullable<long> district_Id, Nullable<bool> includeDeleted)
        {

            try
            {
                //var temp = context.Admin_School_List(ClientId, district_Id, false);
                return this.context.Admin_School_List(ClientId, district_Id, false).Select(s => new SchoolItem
                  {
                      value = s.Id,
                      data = s.School_Name,
                      DistrictID = s.District_Id,
                      DistrictName = s.District_Name
                  }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSchools");
                return null;
            }
        }


        public IEnumerable<HomeRoomModel> getHomeRooms(long? ClientId)
        {
            try
            {
                return this.context.Admin_HomeRoom_List(ClientId).Select(h => new HomeRoomModel
                  {
                      value = h.HomeRoom_Id,
                      data = h.HomeRoom_Name
                  }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getHomeRooms");
                return null;
            }
        }


        public List<SearchBy> GetSearchDDLItems()
        {
            List<SearchBy> list = new List<SearchBy>();
            list.Add(new SearchBy { id = "0", name = "LN, FN, & USERID" });
            list.Add(new SearchBy { id = "1", name = "Last Name" });
            list.Add(new SearchBy { id = "2", name = "First Name" });
            list.Add(new SearchBy { id = "3", name = "User ID" });
            list.Add(new SearchBy { id = "4", name = "Grade" });
            list.Add(new SearchBy { id = "5", name = "Homeroom" });
            return list;
        }
        public List<string> GetSortOrderList()
        {
            return new List<string> { "School", "Grade", "Homeroom", "Customer Name" };
        }

        public string GetCustomerName(long clientID, int customerID)
        {
            string res = string.Empty;

            try
            {
                Customer_Detail_VM cust = this.context.Admin_Customer_Detail(clientID, customerID).Select(c => new Customer_Detail_VM
                  {
                      FirstName = c.FirstName,
                      LastName = c.LastName
                  }).FirstOrDefault();

                res = cust.FirstName + " " + cust.LastName;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerName");
            }

            return res;
        }

        public Customer_Detail_VM GetCustomerDetailForPayment(long clientID, int customerID)
        {
            string res = string.Empty;
            Customer_Detail_VM cust=null;
            try
            {
                cust = this.context.Admin_Customer_Detail_ForPayment(clientID, customerID).Select(c => new Customer_Detail_VM
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    UserID=c.UserID,
                    MealPlanBalance=c.MealPlanBalance,
                    AlaCarteBalance=c.AlaCarteBalance,
                }).FirstOrDefault();

                //res = cust.FirstName + " " + cust.LastName;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerName");
            }

            return cust;
        }

        public string Save_Order(long ClientID, int? OrderID, int? POSID, int EmpID, int CustID, int TransType, double? mDebit, double? aDebit, int? MealPlan, int? CashRes,
            int? LunchType, int? SchoolID, DateTime? OrdDate, int? CreditAuth, int? CheckNum, bool OverRide, bool Void, int? LogID, string LogNotes)
        {
            string res = string.Empty;


            mDebit = (mDebit != null) ? mDebit : 0.0;
            aDebit = (aDebit != null) ? aDebit : 0.0;

            try
            {
                ObjectParameter cashId = new ObjectParameter("CASHRESID", CashRes);
                ObjectParameter orderId = new ObjectParameter("ORDID", OrderID);
                ObjectParameter ORDLOGID = new ObjectParameter("ORDLOGID", DbType.Int32);

                if (string.Empty != LogNotes & LogNotes != null)
                    ORDLOGID.Value = -1;
                else
                    ORDLOGID.Value = DBNull.Value;

                int CASHIERID = EmpID;
                ObjectParameter result = new ObjectParameter("Result", 0);
                ObjectParameter errorMsg = new ObjectParameter("ErrorMsg", string.Empty);

                ObjectParameter aBalance = new ObjectParameter("ABalance", 0.0);
                ObjectParameter mBalance = new ObjectParameter("MBalance", 0.0);
                ObjectParameter bBalance = new ObjectParameter("BonusBalance", 0.0);


              //Order
                DateTime? orderDateUtc = DateTime.UtcNow;
                
                DateTime? OpenDate = OrdDate.Value.AddSeconds(-1);
                DateTime? CloseDate = OrdDate.Value.AddSeconds(1);

                this.context.Admin_Payment_Save(ClientID, cashId, orderId, ORDLOGID, CASHIERID, CustID, aDebit, mDebit, TransType, orderDateUtc, result, errorMsg, aBalance, mBalance, bBalance, CheckNum, OrdDate, null, OpenDate, null, null, CloseDate, null, LogNotes);



                if (Convert.ToInt32(result.Value) != 0)
                    res = Convert.ToString(errorMsg.Value);
                else
                    res = "-1";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GeneralRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Save_Order");
                throw;
            }

            return res;
        }

        public int getItemsCountofCategoryType(long ClientID, long categoryTypeID)
        {
            int retInt = 0;
            retInt = context.sp_ItemCountOfCategoryType(ClientID, categoryTypeID).First().Value;
            return retInt;
        }

        public int getItemsCountofCategory(long ClientID, long categoryID)
        {
            int retInt = 0;
            retInt = context.sp_ItemCountOfCategory(ClientID, categoryID).First().Value;
            return retInt;
        }

        public IEnumerable<IncomeFrequency> GetIncomeFrequencies()
        {
            var list = new List<IncomeFrequency>();
            var fList = context.Income_Frequencies.ToList();
            foreach (var f in fList)
            {
                list.Add(new IncomeFrequency { Id = f.Id, Name = f.Frequency_Name, Multiplier = !f.Multiplier.HasValue ? 0 : f.Multiplier.Value });
            }
            return list;
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

using AdminPortalModels.Models;
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
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private PortalContext context;
        string userIdsCannotBeDuplicated = string.Empty;

        public CustomerRepository(PortalContext context)
        {
            this.context = context;

            userIdsCannotBeDuplicated = ConfigurationManager.AppSettings["UserIdsCannotBeDuplicated"];
        }
        /// <summary>
        /// This function returns  
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="searchString"></param>
        /// <param name="searchBy"></param>
        /// <returns></returns>
        public IEnumerable<Customer_List> GetCustomers(Nullable<long> clientID, string searchString, Nullable<int> searchBy)
        {
            try
            {
                return this.context.Admin_Customer_List(clientID, searchString, searchBy, "", 0).Select(c => new Customer_List
                   {
                       UserID = c.UserID,
                       Last_Name = c.Last_Name,
                       First_Name = c.First_Name,
                       Middle_Initial = c.Middle_Initial,
                       Adult = c.Adult,
                       Active = c.Active,
                       Homeroom = c.Homeroom,
                       School_Name = c.School_Name,
                       Total_Balance = c.Total_Balance
                   }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomers");
                return null;
            }
        }


        /// <summary>
        /// This function returns paged data of a customer 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="iDisplayStart"></param>
        /// <param name="iDisplayLength"></param>
        /// <param name="sortColumnIndex"></param>
        /// <param name="sortDirection"></param>
        /// <param name="filters"></param>
        /// <param name="totalrecords"></param>
        /// <returns></returns>
        //public IEnumerable<Customer_List> GetCustomerPage(Nullable<long> clientID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, CustomerFilters filters, out int totalrecords)
        //{

        //    try
        //    {
        //        string columnName = getColmnName(sortColumnIndex);
        //        IEnumerable<Customer_List> sortedCustomers;
        //        IEnumerable<Admin_Customer_List_Result> adminResult;
        //        string searchString = "";
        //        if (!string.IsNullOrEmpty(filters.SearchBy))
        //        {
        //            searchString = filters.SearchBy;// SessionHelper.SearchStr;
        //        }
        //        int searchBy = 0;
        //        if (filters.SearchBy_Id.HasValue)
        //        {
        //            searchBy = (int)filters.SearchBy_Id;//SessionHelper.SearchBy;
        //        }

        //        if (searchBy != 0 && searchString.Trim() == "" && (sortColumnIndex == 0 || sortColumnIndex == 1))
        //        {
        //            columnName = getSearchColumnName(searchBy);
        //        }


        //        IQueryable<Admin_Customer_List_Result> query = this.context.Admin_Customer_List(clientID, searchString, searchBy, "", 0);

        //        if (!string.IsNullOrEmpty(filters.SchoolStr))//SessionHelper.SchoolSearchStr
        //        {
        //            query = query.Where(c => c.School_Name.Contains(filters.SchoolStr));//SessionHelper.SchoolSearchStr
        //        }
        //        if (!string.IsNullOrEmpty(filters.HomeRoomStr)) //SessionHelper.HomeSearchStr
        //        {
        //            query = query.Where(c => c.Homeroom.Contains(filters.HomeRoomStr));//SessionHelper.HomeSearchStr
        //        }
        //        if (!string.IsNullOrEmpty(filters.GradeStr))//SessionHelper.isGradeFilter
        //        {
        //            query = query.Where(c => c.Grade == filters.GradeStr);//SessionHelper.isGradeFilter
        //        }
        //        if (!string.IsNullOrEmpty(filters.AdultStr)) //SessionHelper.isAdultFilter
        //        {
        //            if (filters.AdultStr == "Yes")
        //            {
        //                query = query.Where(c => c.Adult == true);
        //            }
        //            else
        //            {
        //                query = query.Where(c => c.Adult == false || c.Adult == null);
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(filters.ActiveStr))//SessionHelper.isActiveFilter
        //        {
        //            if (filters.ActiveStr.ToLower() == "active")
        //            {
        //                query = query.Where(c => c.Active == true);
        //            }
        //            else
        //            {
        //                query = query.Where(c => c.Active == false || c.Active == null);
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(filters.NewUser))
        //        {
        //            if (filters.NewUser == "true")
        //            {
        //                query = query.Where(e => e.Last_Name != "Cash Sale");
        //            } 
        //        }

        //        if (columnName != "UserID")
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                adminResult = query.OrderBy(columnName).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_Customer_List_Result>;
        //            }
        //            else
        //            {
        //                adminResult = query.OrderByDescending(columnName).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_Customer_List_Result>;
        //            }
        //        }
        //        else
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                adminResult = query.ToList().AsEnumerable<Admin_Customer_List_Result>().OrderBy(x => x.UserID, new AlphaNumericComparer<string>()).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_Customer_List_Result>;
        //            }
        //            else
        //            {
        //                adminResult = query.ToList().AsEnumerable<Admin_Customer_List_Result>().OrderByDescending(x => x.UserID, new AlphaNumericComparer<string>()).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_Customer_List_Result>;

        //            }
        //        }



        //        //modify searchBy value, if column header was clicked to do sorting abid-h

        //        bool sameColumnsSelected = true;

        //        searchBy = getModifiedSearchID(sortColumnIndex, searchBy, out sameColumnsSelected);

        //        if (searchBy == 1 && searchString.Trim() != "" && sameColumnsSelected)
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                adminResult = adminResult.OrderBy(m => m.Last_Name.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenBy(m => m.Last_Name);
        //            }
        //            else
        //            {
        //                adminResult = adminResult.OrderByDescending(m => m.Last_Name.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenByDescending(m => m.Last_Name);
        //            }
        //        }


        //        if (searchBy == 2 && searchString.Trim() != "" && sameColumnsSelected)
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                adminResult = adminResult.OrderBy(m => m.First_Name.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenBy(m => m.First_Name);
        //            }
        //            else
        //            {
        //                adminResult = adminResult.OrderByDescending(m => m.First_Name.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenByDescending(m => m.First_Name);

        //            }
        //        }


        //        if (searchBy == 3 && searchString.Trim() != "" && sameColumnsSelected)
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                adminResult = adminResult.OrderBy(m => m.UserID.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenBy(m => m.First_Name);
        //            }
        //            else
        //            {
        //                adminResult = adminResult.OrderByDescending(m => m.UserID.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenByDescending(m => m.UserID);

        //            }
        //        }


        //        if (searchBy == 4 && searchString.Trim() != "" && sameColumnsSelected)
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                adminResult = adminResult.OrderBy(m => m.Grade.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenBy(m => m.First_Name);
        //            }
        //            else
        //            {
        //                adminResult = adminResult.OrderByDescending(m => m.Grade.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenByDescending(m => m.Grade);

        //            }
        //        }


        //        if (searchBy == 5 && searchString.Trim() != "" && sameColumnsSelected)
        //        {
        //            if (sortDirection == "asc")
        //            {
        //                adminResult = adminResult.OrderBy(m => m.Homeroom.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenBy(m => m.First_Name);
        //            }
        //            else
        //            {
        //                adminResult = adminResult.OrderByDescending(m => m.Homeroom.ToLower().StartsWith(searchString.ToLower()) ? 0 : 1)
        //                    .ThenByDescending(m => m.Homeroom);

        //            }
        //        }



        //        sortedCustomers = adminResult.Select(c => new Customer_List
        //            {
        //                id = c.Customer_Id,
        //                UserID = c.UserID,
        //                Last_Name = c.Last_Name,
        //                First_Name = c.First_Name,
        //                Middle_Initial = c.Middle_Initial,
        //                Adult = c.Adult,
        //                Grade = c.Grade,
        //                Homeroom = c.Homeroom,
        //                School_Name = c.School_Name,
        //                M_Balance = c.M_Balance,
        //                A_Balance = c.A_Balance,
        //                Total_Balance = c.Total_Balance,
        //                Active = c.Active
        //            });
        //        this.context.Database.CommandTimeout = 180;
        //        totalrecords = query.Count();


        //        return sortedCustomers;
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error logging in cloud tables
        //        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerPage");
        //        totalrecords = 0;
        //        return null;                
        //    }
        //}


        public IEnumerable<Customer_List> GetCustomerPage(Nullable<long> clientID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, CustomerFilters filters, out int totalrecords)
        {

            long ClientID = clientID.HasValue ? clientID.Value : 0;
            string SearchString = "";
            int SearchBy = 0;
            totalrecords = 0;

            /*– Filters */
            bool isAnyFilter = false;
            string SchoolFilter = "";
            string GradeFilter = "";
            string HomeRoomFilter = "";
            bool? OnlyActive = null;
            bool? OnlyAdult = null;
            bool ExcludeCashSaleCustomers = false;
            bool IsBalanceRequired = false;

            /*– Pagination Parameters */
            int PageNo = 1;
            int PageSize = iDisplayLength;

            /*– Sorting Parameters */
            string SortColumn = "";
            string SortOrder = "";

            try
            {

               

                if (!string.IsNullOrEmpty(filters.SearchBy))
                {
                    SearchString = filters.SearchBy;// SessionHelper.SearchStr;
                }
                if (filters.SearchBy_Id.HasValue)
                {
                    SearchBy = (int)filters.SearchBy_Id;//SessionHelper.SearchBy;
                }
                isAnyFilter = getFilterStatus(filters);
                SchoolFilter = filters.SchoolStr == null ? "" : filters.SchoolStr;
                GradeFilter = filters.GradeStr == null ? "" : filters.GradeStr;

                HomeRoomFilter = filters.HomeRoomStr == null ? "" : filters.HomeRoomStr;
                if (filters.ActiveStr != null)
                {
                    OnlyActive = filters.ActiveStr.ToLower() == "active" ? true : false ;
                }
                if (filters.AdultStr != null)
                {
                    OnlyAdult = filters.AdultStr.ToLower() == "yes" ? true : false ;
                }
                
                ExcludeCashSaleCustomers = filters.NewUser == null ? false : filters.NewUser.ToLower() == "true";

                SortColumn = getColmnName(sortColumnIndex);

                SortOrder = sortDirection == "asc" ? "ASC" : "DESC";
                PageNo = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(iDisplayStart) / Convert.ToDouble(iDisplayLength)) + 1);
                IsBalanceRequired = filters.IsBalanceRequired;

              //  IQueryable<Admin_Customers_SortedList_Result> query = this.context.Admin_Customers_SortedList(ClientID, SearchString, SearchBy, isAnyFilter, SchoolFilter, GradeFilter, HomeRoomFilter, OnlyActive, OnlyAdult, ExcludeCashSaleCustomers,IsBalanceRequired, PageNo, PageSize, SortColumn, SortOrder);
                IEnumerable<Admin_Customers_SortedList_Result> dataSet = this.context.Admin_Customers_SortedList(ClientID, SearchString, SearchBy, isAnyFilter, SchoolFilter, GradeFilter, HomeRoomFilter, OnlyActive, OnlyAdult, ExcludeCashSaleCustomers, IsBalanceRequired, PageNo, PageSize, SortColumn, SortOrder);
               // IEnumerable<sp_Admin_Customers_SortedList_Result> NewDataSet = this.context.sp_Admin_Customers_SortedList(ClientID, SearchString, SearchBy, isAnyFilter, SchoolFilter, GradeFilter, HomeRoomFilter, OnlyActive, OnlyAdult, ExcludeCashSaleCustomers, IsBalanceRequired, PageNo, PageSize, SortColumn, SortOrder);
                var query = dataSet.ToList();                                                    
              //  var query = NewDataSet.ToList();                                                    

                //modify searchBy value, if column header was clicked to do sorting abid-h
                
                 IEnumerable<Customer_List> sortedCustomers = query.Select(c => new Customer_List
                {
                    id = c.Customer_Id,
                    UserID = c.UserID,
                    Last_Name = c.Last_Name,
                    First_Name = c.First_Name,
                    Middle_Initial = c.Middle_Initial,
                    Adult = c.Adult,
                    Grade = c.Grade,
                    Homeroom = c.Homeroom,
                    School_Name = c.School_Name,
                    PIN = c.PIN,
                
                    M_Balance = c.M_Balance,
                   
                   A_Balance = c.A_Balance,
                   
                   Total_Balance = c.Total_Balance,
                    Active = c.Active
                }).ToList();
                
                //this.context.Database.CommandTimeout = 180;

                var obj = query.FirstOrDefault();
                if (obj != null)
                    totalrecords = obj.allRecordsCount.HasValue ? obj.allRecordsCount.Value : 0;



                return sortedCustomers;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerPage");
                totalrecords = 0;
                return null;
            }
        }

        private bool getFilterStatus(CustomerFilters filter)
        {
            bool retValue = false;
            if (
                string.IsNullOrEmpty(filter.SchoolStr) &&
                string.IsNullOrEmpty(filter.HomeRoomStr) &&
                string.IsNullOrEmpty(filter.GradeStr) &&
                string.IsNullOrEmpty(filter.ActiveStr) &&
                string.IsNullOrEmpty(filter.AdultStr) &&
                string.IsNullOrEmpty(filter.NewUser)
                )
            {
                retValue = false;
            }
            else
            {
                retValue = true;
            }

            return retValue;

        }


        public List<POSCustomer> GetPosStudents(long districtID, string ListClientCustids, string ListDistCustids)
        {
            long? distId = (long?) districtID;
            ObjectResult<Admin_Connected_Customers_MSA_Result> resultList = context.Admin_Connected_Customers_MSA(distId, ListClientCustids, ListDistCustids);
            List<POSCustomer> posStudentList = new List<POSCustomer>(); 
            foreach(var item in resultList.ToList())
            {
                POSCustomer student = new POSCustomer();
                student.CustomerID = item.Customer_Id;
                student.Balance = item.TotalBalance;
                student.FirstName = item.FirstName;
                student.LastName = item.LastName;
                student.SchoolName = item.School_Name;
                student.Local_ID = item.Local_id;
                student.UserId = item.UserID;
                student.Active = item.isActive;
                student.DOB = item.DOB;
                student.Grade = item.Grade_Id;
                student.HomeRoom = item.HomeroomName == null ? "" : item.HomeroomName;

                posStudentList.Add(student);
            }
            return posStudentList;
        }


        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 2:
                    retVal = "UserID";
                    break;
                case 3:
                    retVal = "Last_Name";
                    break;
                case 4:
                    retVal = "First_Name";
                    break;
                case 5:
                    retVal = "Middle_Initial";
                    break;
                case 6:
                    retVal = "Adult";
                    break;
                case 7:
                    retVal = "Grade";
                    break;
                case 8:
                    retVal = "Homeroom";
                    break;
                case 9:
                    retVal = "School_Name";
                    break;
                case 10:
                    retVal = "PIN";
                    break;
                case 11:
                    retVal = "Total_Balance";
                    break;
                default:
                    retVal = "Last_Name";
                    break;
            }

            return retVal;
        }


        private string getSearchColumnName(int searchColumnIndex)
        {
            int temIndex = searchColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 1:
                    retVal = "Last_Name";
                    break;
                case 2:
                    retVal = "First_Name";
                    break;
                case 3:
                    retVal = "UserID";
                    break;
                case 4:
                    retVal = "Grade";
                    break;
                case 5:
                    retVal = "Homeroom";
                    break;
                default:
                    retVal = "Last_Name, First_Name";
                    break;
            }

            return retVal;
        }

        private int getModifiedSearchID(int searchColumnIndex, int oldSearchID, out bool sameColumnSelected)
        {
            int tempIndex = searchColumnIndex;
            sameColumnSelected = false;
            int retVal = oldSearchID;
            switch (tempIndex)
            {
                case 3://Last name was selected as sorting column from header
                    retVal = 1;
                    break;
                case 4://First name was selected as sorting column from header
                    retVal = 2;
                    break;
                case 2://User ID was selected as sorting column from header
                    retVal = 3;
                    break;
                case 7:// grade was selected as sorting column from header
                    retVal = 4;
                    break;
                case 8://homeroom was selected as sorting column from header
                    retVal = 5;
                    break;
                default:
                    retVal = oldSearchID;
                    sameColumnSelected = true;
                    break;
            }

            if ((oldSearchID == 5 && searchColumnIndex == 8) ||
                (oldSearchID == 4 && searchColumnIndex == 7) ||
                (oldSearchID == 3 && searchColumnIndex == 2) ||
                (oldSearchID == 2 && searchColumnIndex == 4) ||
                (oldSearchID == 1 && searchColumnIndex == 3))
            {
                sameColumnSelected = true;
            }

            return retVal;
        }


        /// <summary>
        /// This function returns a single customer’s data
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer_Detail_VM GetCustomer(Nullable<long> clientID, Nullable<long> customerID)
        {
            try
            {
                if (customerID != 0)
                {
                    return this.context.Admin_Customer_Detail(clientID, customerID).Select(c => new Customer_Detail_VM
                    {
                        Customer_Id = c.Customer_Id,
                        District_Id = c.District_Id,
                        District_Name = c.District_Name,
                        School_Id = c.School_Id,
                        School_Name = c.School_Name,
                        Grade_Id = c.Grade_Id,
                        Grade = c.Grade,
                        Homeroom_Id = c.Homeroom_Id,
                        Homeroom = c.Homeroom,
                        Language_Id = c.Language_Id,
                        Language = c.Language,
                        Ethnicity_Id = c.Ethnicity_Id,
                        Ethnicity = c.Ethnicity,
                        UserID = c.UserID,
                        PIN = c.PIN,
                        LastName = c.LastName,
                        FirstName = c.FirstName,
                        Middle = c.Middle,
                        Gender = c.Gender,
                        SSN = c.SSN,
                        Customer_Addr1 = c.Customer_Addr1,
                        Customer_Addr2 = c.Customer_Addr2,
                        Customer_City = c.Customer_City,
                        Customer_State = c.Customer_State,
                        Customer_Zip = c.Customer_Zip,
                        Customer_Phone = c.Customer_Phone,
                        Email = c.EMail,
                        Customer_Notes = c.Customer_Notes ?? "",
                        Date_Of_Birth = c.Date_Of_Birth,
                        GraduationDate = c.GraduationDate,
                        Active = c.Active,
                        Deleted = c.Deleted,
                        NotInDistrict = c.NotInDistrict,
                        LunchType = c.LunchType,
                        LunchType_String = c.LunchType_String,
                        Student = c.Student,
                        Snack_Participant = c.Snack_Participant,
                        Student_Worker = c.Student_Worker,
                        AllowAlaCarte = c.AllowAlaCarte,
                        No_Credit_On_Account = c.No_Credit_On_Account,
                        MealPlanBalance = c.MealPlanBalance,
                        AlaCarteBalance = c.AlaCarteBalance,
                        BonusBalance = c.BonusBalance,
                        TotalBalance = c.TotalBalance,
                        PictureExtension = c.PictureExtension,
                        StorageAccountName = c.StorageAccountName,
                        ContainerName = c.ContainerName,
                        PictureFileName = c.PictureFileName,
                        CreationDate = c.CreationDate,
                        AllowBiometrics = c.AllowBiometrics,
                        Allow_ACH = c.Allow_ACH,
                        graduationDateSet = c.GraduationDateSet
                    }).FirstOrDefault();
                }
                else
                {
                    return new Customer_Detail_VM(clientID);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomer");
                return new Customer_Detail_VM(clientID);
            }
        }
        /// <summary>
        /// This function returns Admin School List
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="district_Id"></param>
        /// <param name="includeDeleted"></param>
        /// <returns></returns>
        public IEnumerable<EatingSchool> GetEatingSchools(Nullable<long> clientID, Nullable<long> district_Id, Nullable<bool> includeDeleted, long schoolID)
        {
            try
            {
                var tempList = context.Admin_School_List(clientID, district_Id, includeDeleted).Where(s => s.Id != schoolID).ToList();
                var returnList = new List<EatingSchool>();
                foreach (var item in tempList)
                {
                    returnList.Add(new EatingSchool { id = Convert.ToString(item.Id), name = item.School_Name });
                }
                return returnList;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetEatingSchools");
                return null;
            }

        }
        public IEnumerable<AssignedSchool> GetAssignedSchools(Nullable<long> clientID, Nullable<int> CustomerID)
        {
            try
            {
                var tempList = context.Admin_SchoolAssignment_List(clientID, CustomerID).ToList();
                var returnList = new List<AssignedSchool>();
                foreach (var item in tempList)
                {
                    returnList.Add(new AssignedSchool { id = item.School_Id.ToString(), name = item.School_Name });
                }
                return returnList;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAssignedSchools");
                return null;
            }
        }
        /// <summary>
        /// This function returns district of school  
        /// </summary>f
        /// <param name="clientID"></param>
        /// <param name="SchoolID"></param>
        /// <returns></returns>
        public long GetSchoolsDistrict(Nullable<long> clientID, Nullable<long> SchoolID)
        {
            try
            {
                return context.Schools.Where(s => (s.ClientID == clientID && s.ID == SchoolID)).Select(c => c.District_Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchoolsDistrict");
                return 0;
            }
        }
        public string GetDistrictName(Nullable<long> clientID, Nullable<long> DistrictID)
        {
            try
            {
                return context.Districts.Where(s => (s.ClientID == clientID && s.ID == DistrictID)).Select(c => c.DistrictName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistrictName");
                return null;
            }
        }
        /// <summary>
        /// This function saves customer’s data 
        /// </summary>
        /// <param name="customerData"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public string SaveCustomerData(CustomerData customerData, out Int32 customerId)
        {
            int retValue = 1;
            int custId = 0;

            custId = Convert.ToInt16(customerData.custId);
            var ClientID = new SqlParameter("ClientID", Convert.ToInt64(customerData.clientId));

            var CustomerId = new SqlParameter("CustomerID", Convert.ToInt16(customerData.custId));
            CustomerId.Direction = ParameterDirection.InputOutput;

            var Active = new SqlParameter("Active", SqlDbType.Bit);
            Active.Value = (object)customerData.Active ?? DBNull.Value;

            var LastName = new SqlParameter("LastName", SqlDbType.VarChar);
            LastName.Value = (object)customerData.lastname ?? DBNull.Value;

            var FirstName = new SqlParameter("FirstName", SqlDbType.VarChar);
            FirstName.Value = (object)customerData.firstname ?? DBNull.Value;

            var Middle = new SqlParameter("Middle", Convert.ToString(customerData.middle));
            Middle.Value = (object)customerData.middle ?? DBNull.Value;

            var DateOfBirth = new SqlParameter("DateOfBirth", SqlDbType.DateTime);
            DateOfBirth.Value = (object)customerData.dob ?? DBNull.Value;
            if (DateOfBirth.Value.ToString() == "")
            {
                DateOfBirth.Value = DBNull.Value;
            }

            var Gender = new SqlParameter("Gender", SqlDbType.VarChar);
            Gender.Value = (object)customerData.gender ?? DBNull.Value;

            var SSN = new SqlParameter("SSN", SqlDbType.VarChar);
            SSN.Value = (object)customerData.ssn ?? DBNull.Value;

            var Notes = new SqlParameter("Notes", SqlDbType.VarChar);
            Notes.Value = (object)customerData.Customer_Notes ?? DBNull.Value;

            var Addr1 = new SqlParameter("Addr1", SqlDbType.VarChar);
            Addr1.Value = (object)customerData.Addr1 ?? DBNull.Value;

            var Addr2 = new SqlParameter("Addr2", SqlDbType.VarChar);
            Addr2.Value = (object)customerData.Addr2 ?? DBNull.Value;

            var City = new SqlParameter("City", SqlDbType.VarChar);
            City.Value = (object)customerData.city ?? DBNull.Value;

            var State = new SqlParameter("State", SqlDbType.VarChar);
            State.Value = (object)customerData.state ?? DBNull.Value;

            var Zip = new SqlParameter("Zip", SqlDbType.VarChar);
            Zip.Value = (object)customerData.zip ?? DBNull.Value;

            var Phone = new SqlParameter("Phone", SqlDbType.VarChar);
            Phone.Value = (object)customerData.phone ?? DBNull.Value;

            var Email = new SqlParameter("Email", SqlDbType.VarChar);
            Email.Value = (object)customerData.email ?? DBNull.Value;

            var LanguageID = new SqlParameter("LanguageID", SqlDbType.Int);
            LanguageID.Value = (object)customerData.lang ?? DBNull.Value;

            var EthnicityID = new SqlParameter("EthnicityID", SqlDbType.Int);
            EthnicityID.Value = (object)customerData.ethnicity ?? DBNull.Value;

            var UserID = new SqlParameter("UserID", SqlDbType.VarChar);
            UserID.Value = (object)customerData.userid ?? DBNull.Value;

            var PIN = new SqlParameter("PIN", SqlDbType.VarChar);
            PIN.Value = (object)customerData.pin ?? DBNull.Value;

            var SchoolID = new SqlParameter("SchoolID", SqlDbType.Int);
            SchoolID.Value = (object)customerData.schoolid ?? DBNull.Value;

            var DistrictID = new SqlParameter("DistrictID", SqlDbType.Int);
            DistrictID.Value = (object)customerData.distid ?? DBNull.Value;

            var NotInDistrict = new SqlParameter("NotInDistrict", SqlDbType.Bit);
            NotInDistrict.Value = (object)customerData.notinDist ?? DBNull.Value;

            customerData.grad = customerData.grad==""?null:customerData.grad;
            var GradeID = new SqlParameter("GradeID", SqlDbType.Int);
            GradeID.Value = (object)customerData.grad ?? DBNull.Value;

            var HomeroomID = new SqlParameter("HomeroomID", SqlDbType.Int);
            HomeroomID.Value = (object)customerData.homeroom ?? DBNull.Value;
            if (HomeroomID.Value.ToString() == "")
            {
                HomeroomID.Value = DBNull.Value;
            }

            //paidlunch, ReducedLunch, FreeLunch, MealPlanLunch
            var LunchType = new SqlParameter("LunchType", GetLunchType(Convert.ToBoolean(customerData.paidlunch), Convert.ToBoolean(customerData.ReducedLunch), Convert.ToBoolean(customerData.FreeLunch), Convert.ToBoolean(customerData.MealPlanLunch), Convert.ToBoolean(customerData.MealStatuMealPlan)));

            var Student = new SqlParameter("Student", SqlDbType.Bit);
            Student.Value = (object)customerData.Student ?? DBNull.Value;

            var SnackProgram = new SqlParameter("SnackProgram", SqlDbType.Bit);
            SnackProgram.Value = (object)customerData.snackP ?? DBNull.Value;

            var StudentWorker = new SqlParameter("StudentWorker", SqlDbType.Bit);
            StudentWorker.Value = (object)customerData.stuWork ?? DBNull.Value;

            var AllowAlaCarte = new SqlParameter("AllowAlaCarte", SqlDbType.Bit);
            AllowAlaCarte.Value = (object)customerData.alacarte ?? DBNull.Value;

            var NoCreditOnAccount = new SqlParameter("NoCreditOnAccount", SqlDbType.Bit);
            NoCreditOnAccount.Value = (object)customerData.nocredit ?? DBNull.Value;

            var CreationDate = new SqlParameter("CreationDate", SqlDbType.DateTime2);

            CreationDate.Value = DateTime.UtcNow;

            var LocalTime = new SqlParameter("LocalTime", SqlDbType.DateTime2);
            LocalTime.Value = DateTime.Now;

            var dt = new DataTable();
            if (customerData.schoolsList != null && !string.IsNullOrEmpty(customerData.schoolsList))
            {
                string[] AssignedSchools = Convert.ToString(customerData.schoolsList).Split(',');
                dt.Columns.Add("School_Id");
                for (int i = 0; i < AssignedSchools.Count(); i++)
                {
                    dt.Rows.Add(AssignedSchools[i]);
                }

            }
            //Picture Info
            customerData.PictureExtension = customerData.PictureExtension == "" ? null : customerData.PictureExtension;
            var PictureExtension = new SqlParameter("PictureExtension", SqlDbType.VarChar);
            PictureExtension.Value = (object)customerData.PictureExtension ?? DBNull.Value;

            customerData.StorageAccountName = customerData.StorageAccountName == "" ? null : customerData.StorageAccountName;
            var StorageAccountName = new SqlParameter("StorageAccountName", SqlDbType.VarChar);
            StorageAccountName.Value = (object)customerData.StorageAccountName ?? DBNull.Value;

            customerData.ContainerName = customerData.ContainerName == "" ? null : customerData.ContainerName;
            var ContainerName = new SqlParameter("ContainerName", SqlDbType.VarChar);
            ContainerName.Value = (object)customerData.ContainerName ?? DBNull.Value;

            customerData.PictureFileName = customerData.PictureFileName=="" ? null : customerData.PictureFileName;
            var PictureFileName = new SqlParameter("PictureFileName", SqlDbType.VarChar);
            PictureFileName.Value = (object)customerData.PictureFileName ?? DBNull.Value;

            var EmployeeID = new SqlParameter("EmployeeID", SqlDbType.Int);

            int Employee_ID = 0;
            if (System.Web.HttpContext.Current.Session["Cust_ID"] != null)
            {
                Employee_ID = Convert.ToInt32(System.Web.HttpContext.Current.Session["Cust_ID"].ToString());
            }
            EmployeeID.Value = Employee_ID;

            var GraduationDate = new SqlParameter("GraduationDate", SqlDbType.DateTime);
            GraduationDate.Value = (object)customerData.GraduationDate ?? DBNull.Value;
            if (GraduationDate.Value.ToString() == "")
            {
                GraduationDate.Value = DBNull.Value;
            }

            var graduationDateSet = new SqlParameter("graduationDateSet", SqlDbType.Bit);
            graduationDateSet.Value = (object)customerData.graduationDateSet ?? DBNull.Value;

            var AllowACH = new SqlParameter("AllowACH", false);


            var ResultCode = new SqlParameter("ResultCode", SqlDbType.Int);
            ResultCode.Direction = ParameterDirection.Output;

            var ErrorMessage = new SqlParameter("ErrorMessage", SqlDbType.VarChar);
            ErrorMessage.Direction = ParameterDirection.Output;
            ErrorMessage.Size = 4000;



            var EatingAssignments = new SqlParameter("EatingAssignments", SqlDbType.Structured);
            if (dt.Rows.Count > 0)
            {
                EatingAssignments.Value = dt;
            }
            else
            {
                EatingAssignments.Value = null;
            }
            EatingAssignments.TypeName = "dbo.CustomerSchoolAssignments";

            if (custId == 0)
            {
                CustomerId.Value = -1;
            }


            try
            {
                if (!UserIdAlreadyExists(Convert.ToInt64(customerData.clientId), customerData.userid, custId, Convert.ToInt32(customerData.distid)))
                {
                    if (!PINAlreadyExists(Convert.ToInt64(customerData.clientId), customerData.pin, custId, Convert.ToInt32(customerData.distid)))
                    {
                        this.context.ExecuteStoreProcedure("Admin_Customer_Save", ClientID, CustomerId, DistrictID, SchoolID, UserID, PIN, LastName, FirstName, LunchType, Student, EatingAssignments, LanguageID, GradeID, HomeroomID, EthnicityID, Middle, Gender, SSN, Addr1, Addr2, City, State, Zip, Phone, GraduationDate, graduationDateSet, Notes, Email, DateOfBirth, Active, AllowAlaCarte, NoCreditOnAccount, AllowACH, SnackProgram, StudentWorker, NotInDistrict, CreationDate, LocalTime, PictureExtension, StorageAccountName, ContainerName, PictureFileName, EmployeeID, ResultCode, ErrorMessage);
                        retValue = Convert.ToInt32(CustomerId.Value);
                        string error = ErrorMessage.ToString();
                        string code = ResultCode.ToString();


                    }
                    else
                    {
                        retValue = -1000;
                    }
                }
                else
                {
                    retValue = -999;
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                retValue = -1;
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SaveCustomerData");
            }
            customerId = Convert.ToInt32(CustomerId.Value);
            return Convert.ToString(retValue);
        }
        /// <summary>
        /// This function returns lunch type of a customer 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        private int GetLunchType(bool p1, bool p2, bool p3, bool p4, bool p5)
        {
            if (p1) return 1;
            else if (p2) return 2;
            else if (p3) return 3;
            else if (p4) return 5;
            else if (p5) return 4;
            else return 4;
        }

        public bool UserIdAlreadyExists(long ClientID, string userID, Int32 custId, Int32 distID)
        {
            try
            {
                // Inayat [9-Aug-2016] The following piece of code will stop CSG and CSS userid from being duplicated even in different districts for two different users.
                // This work is done against bug# PA-517
                if (!string.IsNullOrEmpty(userIdsCannotBeDuplicated))
                {
                    string[] userIds = userIdsCannotBeDuplicated.Split(',');
                    if (userIds.Count() > 0)
                    {
                        if (userIds.Select(x => x.ToLower().Trim()).Contains(userID.ToLower().Trim()))
                        {
                            if (custId == 0)
                            {
                                return context.Customers.Where(c => c.ClientID == ClientID && c.UserID == userID).Count() > 0;
                            }
                            else
                            {
                                return context.Customers.Where(c => c.ClientID == ClientID && c.UserID == userID && c.ID != custId).Count() > 0;
                            }
                        }
                    }
                }

                if (custId == 0)
                {
                    return context.Customers.Where(c => c.ClientID == ClientID && c.UserID == userID && c.District_Id == distID).Any();
                }
                else
                {
                    if (context.Customers.Where(c => c.ClientID == ClientID && c.UserID == userID && c.District_Id == distID && c.ID == custId).Count() == 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (context.Customers.Where(c => c.ClientID == ClientID && c.UserID == userID && c.District_Id == distID && c.ID != custId).Any())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UserIdAlreadyExists");
                return false;
            }
        }

        public bool PINAlreadyExists(long ClientID, string PIN, Int32 custId, Int32 distID)
        {
            try
            {
                if (custId == 0)
                {
                    return context.Customers.Where(c => c.ClientID == ClientID && c.PIN == PIN && c.District_Id == distID).Any();
                }
                else
                {
                    if (context.Customers.Where(c => c.ClientID == ClientID && c.PIN == PIN && c.District_Id == distID && c.ID == custId).Count() == 1)
                    {
                        return false;
                    }
                    else
                    {
                        if (context.Customers.Where(c => c.ClientID == ClientID && c.PIN == PIN && c.District_Id == distID && c.ID != custId).Any())
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "PINAlreadyExists");
                return false;
            }
        }


        /// <summary>
        /// This function gets customer’s log from database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="CustomerID"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public IEnumerable<Customer_Logs> GetCustomerLogs(Nullable<long> clientID, Nullable<int> CustomerID, out int totalRecords)
        {
            try
            {
                IEnumerable<Customer_Logs> tempCustomer_Logs = this.context.Admin_Customer_Logs(clientID, CustomerID).Select(l => new Customer_Logs
                   {
                       ChangedDate = l.ChangedDateLocal,
                       Employee_Name = l.Employee_Name,
                       Note = l.Note
                   }).ToList();

                totalRecords = tempCustomer_Logs.Count();
                return tempCustomer_Logs;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerLogs");
                totalRecords = 0;
                return null;
            }
        }


        /// <summary>
        /// This function deletes a single customer’s data 
        /// </summary>
        /// <param name="CustomerData"></param>
        /// <returns></returns>
        public string DeleteCustomer(string CustomerData)
        {
            string retValue = "1";
            string[] fildsToUpdate = CustomerData.Split('*');
            if (fildsToUpdate.Length == 8)
            {
                long ClientID = Convert.ToInt32(fildsToUpdate[0]);
                int CustomerID = Convert.ToInt32(fildsToUpdate[1]);
                bool OverrideBalance = Convert.ToBoolean(fildsToUpdate[2]);
                bool OverrideEmployee = Convert.ToBoolean(fildsToUpdate[3]);
                bool OverrideOrders = Convert.ToBoolean(fildsToUpdate[4]);
                bool OverrideBonusPayments = Convert.ToBoolean(fildsToUpdate[5]);
                bool OverridePreorders = Convert.ToBoolean(fildsToUpdate[6]);
                bool OverrideStudentApp = Convert.ToBoolean(fildsToUpdate[7]);

                //call db function to delete
                try
                {
                    Admin_Customer_Delete_Result tempResult = new Admin_Customer_Delete_Result();
                    tempResult = this.context.Admin_Customer_Delete(ClientID, CustomerID, OverrideBalance, OverrideEmployee, OverrideOrders, OverrideBonusPayments, OverridePreorders, OverrideStudentApp).FirstOrDefault();
                    int cId = (int)tempResult.Customer_Id;
                    retValue = tempResult.ErrorMessage;
                }
                catch (Exception ex)
                {
                    //Error logging in cloud tables
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteCustomer");
                }
            }
            return retValue;
        }

        //Is Customer Applied for Free and Reduced Meal Application 
        public Int32 FARM_AppCount(Nullable<long> clientID, Nullable<int> CustomerID)
        {
            Int32 countResult = context.Admin_Customer_IsFreeReducedApplied(clientID, CustomerID).FirstOrDefault() ?? 0;
            return countResult;
        }


        /// <summary>
        /// This function disposes all the memory occupied by this object
        /// </summary>
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


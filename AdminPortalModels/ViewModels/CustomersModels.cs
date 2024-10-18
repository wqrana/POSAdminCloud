using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AdminPortalModels.ViewModels
{
    class CustomersModels
    {

    }

    public class Customer_List
    {

        public int? id { get; set; }
        public string UserID { get; set; }
        public string Last_Name { get; set; }
        public string First_Name { get; set; }
        public string Middle_Initial { get; set; }
        public Nullable<bool> Adult { get; set; }
        public Nullable<bool> Active { get; set; }
        public string Grade { get; set; }
        public string Homeroom { get; set; }
        public string School_Name { get; set; }
        public string PIN { get; set; }
        public Nullable<decimal> M_Balance { get; set; }
        public Nullable<decimal> A_Balance { get; set; }
        public Nullable<decimal> Total_Balance { get; set; }
    }
    public class PagedCustomers
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public IEnumerable<Customer_List> Customers { get; set; }
        public int totalRecords { get; set; }
    }
    public class JQueryDataTableParamModel
    {
        /// <summary>
        /// Request sequence number sent by DataTable, same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// Index of column which is being sorted
        /// </summary>
        public int iSortCol_0 { get; set; }

        /// <summary>
        /// Sort direction of column "asc" or "desc"
        /// </summary>
        public string sSortDir_0 { get; set; }
    }
    //public class AssignedSchools
    //{
    //    public IEnumerable<EatingSchool> SelectedSchoolList { get; set; }
    //    public IEnumerable<EatingSchool> SchoolsList { get; set; }
    //}
    public class EatingSchool
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class AssignedSchool
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Customer_Detail_VM
    {
        public Customer_Detail_VM()
        {
        }
        public Customer_Detail_VM(Nullable<long> ClientId)
        {
            Customer_Id = 0;
            District_Id = 0;
            District_Name = "";
            School_Id = null;
            School_Name = "";
            Grade_Id = 0;
            Grade_Id = 0;
            Homeroom_Id = 0;
            Language_Id = 0;
            Ethnicity_Id = 0;
            UserID = "";
            PIN = "";
            LastName = "";
            FirstName = "";
            Middle = "";
            Gender = "";
            SSN = "";
            Customer_Addr1 = "";
            Customer_Addr2 = "";
            Customer_City = "";
            Customer_State = "";
            Customer_Zip = "";
            Customer_Phone = "";
            Customer_Notes = "";
            Email = "";
            Active = true;
            NotInDistrict = false;
            LunchType = 6;
            Student = true;
            Snack_Participant = false;
            Student_Worker = false;
            AllowAlaCarte = true;
            No_Credit_On_Account = false;
            graduationDateSet = false;
        }

        public Int64 Customer_Id { get; set; }
        public long District_Id { get; set; }
        public string District_Name { get; set; }
        public Nullable<long> School_Id { get; set; }
        public string School_Name { get; set; }
        public Nullable<long> Grade_Id { get; set; }
        public string Grade { get; set; }
        public Nullable<long> Homeroom_Id { get; set; }
        public string Homeroom { get; set; }

        public Nullable<bool> graduationDateSet { get; set; }

        public Nullable<long> Language_Id { get; set; }
        public string Language { get; set; }
        public Nullable<int> Ethnicity_Id { get; set; }
        public string Ethnicity { get; set; }
        public string UserID { get; set; }
        public string PIN { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Middle { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string Customer_Addr1 { get; set; }
        public string Customer_Addr2 { get; set; }
        public string Customer_City { get; set; }
        public string Customer_State { get; set; }
        public string Customer_Zip { get; set; }
        public string Customer_Phone { get; set; }
        public string Customer_Notes { get; set; }
        public string Email { get; set; }

        public Nullable<System.DateTime> Date_Of_Birth { get; set; }
        public Nullable<System.DateTime> GraduationDate { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<bool> NotInDistrict { get; set; }
        public Nullable<int> LunchType { get; set; }
        public string LunchType_String { get; set; }
        public Nullable<bool> Student { get; set; }
        public Nullable<bool> Snack_Participant { get; set; }
        public Nullable<bool> Student_Worker { get; set; }
        public Nullable<bool> AllowAlaCarte { get; set; }
        public Nullable<bool> No_Credit_On_Account { get; set; }

        public double MealPlanBalance { get; set; }
        public double AlaCarteBalance { get; set; }
        public double BonusBalance { get; set; }
        public double TotalBalance { get; set; }
        public string PictureExtension { get; set; }
        public string StorageAccountName { get; set; }
        public string ContainerName { get; set; }
        public string PictureFileName { get; set; }

        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> CreationDateLocal { get; set; }
        public int AllowBiometrics { get; set; }
        public Nullable<bool> Allow_ACH { get; set; }
        public string ConvertedCreationDate { get; set; } 
        //CASE c.LunchType WHEN 1 THEN 'Paid' WHEN 2 THEN 'Reduced' WHEN 3 THEN 'Free' WHEN 5 THEN 'Meal Plan' ELSE 'Adult' END as LunchType_String,
        //extras
        public bool MealStatuPaid
        {
            get
            {
                if (LunchType == 1) return true; else return false;
            }
        }
        public bool MealStatuReduced
        {
            get
            {
                if (LunchType == 2) return true; else return false;
            }
        }
        public bool MealStatuFree
        {
            get
            {
                if (LunchType == 3) return true; else return false;
            }
        }
        public bool MealStatuMealPlan
        {
            get
            {
                if (LunchType == 5) return true; else return false;
            }
        }

        public bool MealStatusEmployeeAdult
        {
            get
            {
                if (LunchType == 4) return true; else return false;
            }
        }
        public string CustomerDOB
        {
            get
            {
                if (Date_Of_Birth != null)
                {
                    return Date_Of_Birth.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    return "";
                }
            }
        }
        public string FormatGraduationDate
        {
            get
            {
                if (GraduationDate != null)
                {
                    return GraduationDate.Value.ToString("MM/dd/yyyy");
                }
                else
                {
                    return "";
                }
            }
        }
    }
    public class SingleCustomer
    {
        public string pictureStoragepath { get; set; }
        public Int64 ClientID { get; set; }
        public Uri uri
        {
            get
            {
                Random rand = new Random();
                return new Uri(pictureStoragepath + ClientID.ToString() + "/" + Customer.Customer_Id.ToString() + ".jpg?rand=" + rand.Next());
            }
        }
        public Customer_Detail_VM Customer { get; set; }
        public ICollection<Gender> GendersList { get; set; }
        public ICollection<State> StatesList { get; set; }
        public ICollection<Language> Languages { get; set; }
        public ICollection<Ethnicity> Ethnicities { get; set; }
        public ICollection<SchoolItem> Schools { get; set; }
        public ICollection<DistrictItem> Districts { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public IEnumerable<SelectListItem> HomeRooms { get; set; }
        //public ICollection<HomeRoom> HomeRooms { get; set; }
        public ICollection<AssignedSchool> assignedSchools { get; set; }
        public ICollection<EatingSchool> eatingSchools { get; set; }

        public int FreeReducedAppCount { get; set; }
        
    }
    public class popUpCustomer
    {
        
        public Int64 ClientID { get; set; }
        public string pictureStoragepath { get; set; }
        public Uri uri
        {
            get
            {
                Random rand = new Random();
                return new Uri(pictureStoragepath + ClientID.ToString() + "/" + Customer.Customer_Id.ToString() + ".jpg?rand=" + rand.Next());
            }
        }
        public string FullAddress
        {
            get
            {
                return Customer.Customer_Addr1 + " " + Customer.Customer_Addr2 + " " + Customer.Customer_City + " " + Customer.Customer_State + " " + Customer.Customer_Zip;
            }

        }
        public string FullGender
        {
            get
            {
                if (Customer.Gender != null)
                    return (Convert.ToString((Customer.Gender.ToLower())) == "f") ? "Female" : "Male";
                else
                    return string.Empty;
            }
        }
        public string cOptions
        {
            get
            {
                return getCustomerOptions(Customer.Student, Customer.Snack_Participant, Customer.Student_Worker);
            }
        }
        public string cRestrictions
        {
            get
            {
                return getCustomerRestr(Customer.AllowAlaCarte, Customer.No_Credit_On_Account);
            }

        }
        public Customer_Detail_VM Customer { get; set; }
        public bool IsError { get; set; }
        public ICollection<AssignedSchool> assignedSchools { get; set; }
        public string CustomerAssignedSchools
        {
            get
            {
                return getCustomerAssignedSchools();
            }
        }
        public string CustomerDOB
        {
            get
            {
                if (Customer.Date_Of_Birth != null)
                {
                    return Customer.Date_Of_Birth.Value.ToString("MM/dd/yyyy");
            }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// /methods
        /// </summary>
        /// <returns></returns>
        private string getCustomerAssignedSchools()
        {

            string retStr = "";
            foreach (var school in assignedSchools)
            {
                retStr = retStr + school.name + " <br /> ";

            }
            return retStr;
        }

        private string getCustomerRestr(bool? AllowAlaCarte, bool? No_Credit_On_Account)
        {
            string retStr = "";
            int count = 0;
            if (AllowAlaCarte.HasValue)
            {
                if ((bool)AllowAlaCarte)
                {
                    retStr = retStr + "Allow Ala Carte";
                    count = count + 1;
                }
            }
            if (No_Credit_On_Account.HasValue)
            {
                if ((bool)No_Credit_On_Account)
                {
                    if (count > 0) retStr = retStr + " <br /> ";
                    retStr = retStr + "No Credit on Account";
                    count = count + 1;
                }
            }
            return retStr;
        }
        private string getCustomerOptions(bool? isStudent, bool? isSnack, bool? isStudentWorker)
        {
            string retStr = "";
            int count = 0;
            if (isStudent.HasValue)
            {
                if ((bool)isStudent)
                {
                    retStr = retStr + "Student";
                    count = count + 1;
                }
            }
            if (isSnack.HasValue)
            {
                if ((bool)isSnack)
                {
                    if (count > 0) retStr = retStr + " <br /> ";
                    retStr = retStr + "Snack Program";
                    count = count + 1;
                }
            }
            if (isStudentWorker.HasValue)
            {
                if ((bool)isStudentWorker)
                {
                    if (count > 0) retStr = retStr + " <br /> "; ;
                    retStr = retStr + " Student Worker";
                    count = count + 1;
                }
            }

            return retStr;
        }
        public List<POSNotificationsViewModel> POSNotifications { get; set; }

    }



    public class CData
    {
        public string data { get; set; }
    }
    public class CustomerData
    {
        public string clientId { get; set; }
        public string custId { get; set; }
        public string Active { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string middle { get; set; }
        public string dob { get; set; }
        public string gender { get; set; }
        public string ssn { get; set; }
        public string Customer_Notes { get; set; }
        public string Addr1 { get; set; }
        public string Addr2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string lang { get; set; }
        public string ethnicity { get; set; }
        public string userid { get; set; }
        public string pin { get; set; }
        public string schoolid { get; set; }
        public string distid { get; set; }
        public string notinDist { get; set; }
        public string grad { get; set; }
        public string homeroom { get; set; }
        public string graduationDateSet { get; set; }
        public string GraduationDate { get; set; }
        public string paidlunch { get; set; }
        public string ReducedLunch { get; set; }
        public string FreeLunch { get; set; }
        public string MealPlanLunch { get; set; }
        public string MealStatuMealPlan { get; set; }
        public string Student { get; set; }
        public string snackP { get; set; }
        public string stuWork { get; set; }
        public string alacarte { get; set; }
        public string nocredit { get; set; }
        public string schoolsList { get; set; }

        public string PictureExtension { get; set; }
        public string StorageAccountName { get; set; }
        public string ContainerName { get; set; }
        public string PictureFileName { get; set; }
    }
    public class Customer_Logs
    {
        public System.DateTime? ChangedDate { get; set; }
        public string CDate
        {
            get
            {
                if (ChangedDate != null)
                {
                    return Convert.ToDateTime(ChangedDate).ToShortDateString();
                }
                else {
                    return "";
                }
                
            }
        }
        public string ChangedTime
        {
            get
            {
                if (ChangedDate != null)
                {
                    return Convert.ToDateTime(ChangedDate).ToLongTimeString();
                }
                else { 
                    return "";
                }
            }
        }
        public string Note { get; set; }
        public string Employee_Name { get; set; }
    }

    [Serializable]
    public class CustomerFilters
    {
        public string SearchBy { get; set; }
        public int? SearchBy_Id { get; set; }
        public string SchoolStr { get; set; }
        public string HomeRoomStr { get; set; }
        public string GradeStr { get; set; }
        public string ActiveStr { get; set; }
        public string AdultStr { get; set; }
        public string NewUser { get; set; }
        public bool IsBalanceRequired { get; set; }
    }
    public class SearchBy
    {
        public string id { get; set; }
        public string name { get; set; }
    }

}

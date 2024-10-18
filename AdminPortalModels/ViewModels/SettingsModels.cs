using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.Models;
using System.Web.Mvc;

namespace AdminPortalModels.ViewModels
{
    class SettingsModels
    {
    }
    public class SchoolVM
    {
        public SchoolVM()
        {
            POSVM = new List<POSVM>();
        }

        public long ClientID { get; set; }
        public long Id { get; set; }
        public string SchoolName { get; set; }
        public int SchoolsCount
        {
            get
            {
                return POSVM.Count;
            }
        }
        public string schoolDisplayClass
        {
            get
            {
                if (SchoolsCount > 0)
                    return "ShowSchool";
                else
                    return "HideSchool";
            }
        }

        public virtual ICollection<POSVM> POSVM { get; set; }

    }


    public class POSPageVM
    {
        public IEnumerable<SchoolVM> SchoolsList { get; set; }
        public int allPOSCount { get; set; }

    }

    // delete
    public class POSDeleteModel : DeleteModel
    {
        public override string Title { get { return "POS"; } }
        public override string DeleteUrl { get { return "/Settings/DeletePOS"; } }
        public string sessionStatus { get; set; }
    }


    public class POSVM : ErrorModel
    {
        public long ClientID { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string SessionStatus { get; set; }
        public bool? EnableCCProcessing { get; set; }
        public string VeriFoneUserId { get; set; }
        public string VeriFonePassword { get; set; }
        public string hrefid { get { return Convert.ToString(ClientID.ToString() + "_" + Id.ToString()); } }
        public long School_Id { get; set; }
        public bool enbCCCCProcessing
        {
            get
            {
                if (EnableCCProcessing.HasValue)
                {
                    if ((bool)EnableCCProcessing)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
        }
        public string SchoolName { get; set; }

    }

    public class POSCreateModel : POSVM
    {
        public override string Title { get { return "Create New"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new category."; } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }

    public class POSUpdateModel : POSVM
    {
        public override string Title { get { return string.Format("{0} Settings", Name); } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} category.", Name); } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save Changes");
            }
        }
    }
    /// <summary>
    /// ///////////////////
    /// </summary>

    public class DistrictVM
    {
        private long _ClientID;
        private long _Id;
        private string _DistrictName;
           private string  _Address1;
            private string _Address2;
           private string  _City;
           private string  _State ;
           private string  _Zip;
          private string   _Phone1;
          private string   _Phone2;
          private string   _BankRoute;
          private string   _BankAccount;
          private string   _BankName;
           private string  _BankAddr1;
           private string  _BankAddr2;
          private string   _BankCity;
          private string   _BankState;
          private string _BankZip;
          private long? _empDirector;
          private long? _empAdmin;

        public DistrictVM()
        {

        }
        public DistrictVM(long clientId)
        {
            ClientID = clientId;
            DistrictName = "";
            Address1 = "";
            Address2 = "";
            City = "";
            State = "";
            Zip = "";
            Phone1 = "";
            Phone2 = "";
            BankRoute = "";
            BankAccount = "";
            BankName = "";
            BankAddr1 = "";
            BankAddr2 = "";
            BankCity = "";
            BankState = "";
            BankZip = "";
            _empDirector = null;
            _empAdmin = null;
        }

        public long ClientID
        {
            get
            {
                return _ClientID ;

            }
            set
            {
                _ClientID = value;
            }
        }

        public long Id
        {
            get
            {
                return _Id;

            }
            set
            {
                _Id = value;
            }
        }
        public string DistrictName
        {
            get
            {
                return _DistrictName ?? "";

            }
            set
            {
                _DistrictName = value ?? "";
            }
        }
        public string Address1
        {
            get
            {
                return _Address1 ?? "";

            }
            set
            {
                _Address1 = value ?? "";
            }
        }
        public string Address2
        {
            get
            {
                return _Address2 ?? "";

            }
            set
            {
                _Address2 = value ?? "";
            }
        }
        public string City
        {
            get
            {
                return _City ?? "";

            }
            set
            {
                _City = value ?? "";
            }
        }
        public string State
        {
            get
            {
                return _State ?? "";

            }
            set
            {
                _State = value ?? "";
            }
        }
        public string Zip
        {
            get
            {
                return _Zip ?? "";

            }
            set
            {
                _Zip = value ?? "";
            }
        }
        public string Phone1
        {
            get
            {
                return _Phone1 ?? "";

            }
            set 
            {
                _Phone1 = value ?? ""; 
            }
        }
        public string Phone2
        {
            get
            {
                return _Phone2 ?? "";

            }
            set
            {
                _Phone2 = value ?? "";
            }
        }

        public string BankRoute
        {
            get
            {
                return _BankRoute ?? "";

            }
            set
            {
                _BankRoute = value ?? "";
            }
        }
        public string BankAccount
        {
            get
            {
                return _BankAccount ?? "";

            }
            set
            {
                _BankAccount = value ?? "";
            }
        }
        public string BankName
        {
            get
            {
                return _BankName ?? "";

            }
            set
            {
                _BankName = value ?? "";
            }
        }

        public string BankAddr1
        {
            get
            {
                return _BankAddr1 ?? "";

            }
            set
            {
                _BankAddr1 = value ?? "";
            }
        }
        public string BankAddr2
        {
            get
            {
                return _BankAddr2 ?? "";

            }
            set
            {
                _BankAddr2 = value ?? "";
            }
        }


        public string BankCity
        {
            get
            {
                return _BankCity ?? "";

            }
            set
            {
                _BankCity = value ?? "";
            }
        }
        public string BankState
        {
            get
            {
                return _BankState ?? "";

            }
            set
            {
                _BankState = value ?? "";
            }
        }
        public string BankZip
        {
            get
            {
                return _BankZip ?? "";

            }
            set
            {
                _BankZip = value ?? "";
            }
        }

        public long? empAdmin
        {
            get
            {
                return _empAdmin;  //?? 0;

            }
            set
            {
                _empAdmin = value; //?? 0;
            }
        }
        public long? empDirector
        {
            get
            {
                return _empDirector; //?? 0;

            }
            set
            {
                _empDirector = value; //?? 0;
            }
        }
    }

    public class DistrictOptionsVM
    {
        public DistrictOptionsVM()
        {
            isStudentPaidTaxable = false;
            isStudentRedTaxable = false;
            isStudentFreeTaxable = false;
            isEmployeeTaxable = false;
            isMealPlanTaxable = false;
            isGuestTaxable = false;
            isStudCashTaxable = false;
            StartSchoolYear = DateTime.Now.AddYears(-100);
            EndSchoolYear = DateTime.Now.AddYears(-99);
        }

        public Nullable<bool> isStudentPaidTaxable { get; set; }
        public Nullable<bool> isStudentRedTaxable { get; set; }
        public Nullable<bool> isStudentFreeTaxable { get; set; }
        public Nullable<bool> isEmployeeTaxable { get; set; }
        public Nullable<bool> isMealPlanTaxable { get; set; }
        public Nullable<bool> isGuestTaxable { get; set; }
        public Nullable<bool> isStudCashTaxable { get; set; }


        public Nullable<System.DateTime> StartSchoolYear { get; set; }
        public Nullable<System.DateTime> EndSchoolYear { get; set; }


    }

    public class DistrictNames
    {
        public long districtId { get; set; }
        public string districtName { get; set; }
    }



    public class DistrictsData
    {

        public DistrictVM DistrictVM { get; set; }
        public DistrictOptionsVM DistrictOptionsVM { get; set; }
        public ICollection<DistrictNames> DistrictList { get; set; }
        public string DirectorName { get; set; }
        public string AdminName { get; set; }
    }

    ////////districts page methods
    public class Result
    {
        public int result { get; set; }
    }

    /// <summary>
    /// School Page Data methods
    /// </summary>

    public class SchoolIndexVM
    {
        public long ClientID { get; set; }
        public long Id { get; set; }
        public string DistrictName { get; set; }
        public virtual ICollection<SchoolIndexData> SchoolIndexData { get; set; }

    }

    public class SchoolIndexData
    {
        public long Id { get; set; }
        public long District_Id { get; set; }
        public string School_ID { get; set; }
        public string School_Name { get; set; }
        public string District_Name { get; set; }
        public int POSNum { get; set; }
    }

    public class SchoolsVM
    {
        public SchoolsVM()
        {

        }
        public SchoolsVM(long clientId)
        {
            ClientID = clientId;
            SchoolName = "";
            SchoolID = "";
            Address1 = "";
            Address2 = "";
            City = "";
            State = "";
            Zip = "";
            Phone1 = "";
            Phone2 = "";
            Comments = "";
            isSevereNeed = false;
            UseDistDirAdmin = false;
        }

        public long ClientID { get; set; }
        public long Id { get; set; }
        public string SchoolID { get; set; }
        public string SchoolName { get; set; }
        public long District_Id { get; set; }
        public long? Emp_Director_Id { get; set; }
        public long? Emp_Administrator_Id { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }

        public string Comments { get; set; }

        public Nullable<bool> isSevereNeed { get; set; }
        public Nullable<bool> UseDistDirAdmin { get; set; }
        public string hrefid { get { return Convert.ToString(ClientID.ToString() + "_" + Id.ToString()); } }
    }

    public class SchoolOptionsVM
    {
        public SchoolOptionsVM()
        {
            DoPinPreFix = false;
            PhotoLogging = false;
            StripZeros = false;
            StartSchoolYear = null;
            EndSchoolYear = null;
            PinPreFix = "";
        }

        public Nullable<bool> DoPinPreFix { get; set; }
        public Nullable<bool> PhotoLogging { get; set; }
        public Nullable<bool> StripZeros { get; set; }
        public double AlaCarteLimit { get; set; }
        public double MealPlanLimit { get; set; }
        public DateTime? StartSchoolYear { get; set; }
        public DateTime? EndSchoolYear { get; set; }

        public string PinPreFix { get; set; }
        public Nullable<int> BarCodeLength { get; set; }
    }

    public class SchoolNames
    {
        public long schoolId { get; set; }
        public string schoolName { get; set; }
    }

    public class DistrictExecs
    {
        public DistrictExecs()
        {
            Emp_Administrator_Id = 0;
            Emp_Director_Id = 0;
        }

        public Nullable<long> Emp_Administrator_Id { get; set; }
        public Nullable<long> Emp_Director_Id { get; set; }
    }

    public class DistrictDates
    {
        public DistrictDates()
        {
            StartSchoolYear = DateTime.MinValue;
            EndSchoolYear = DateTime.MinValue;
        }

        public Nullable<DateTime> StartSchoolYear { get; set; }
        public Nullable<DateTime> EndSchoolYear { get; set; }
    }

    public class SchoolsData
    {

        public SchoolsVM SchoolVM { get; set; }
        public SchoolOptionsVM SchoolOptionsVM { get; set; }
        public ICollection<SchoolNames> SchoolList { get; set; }
        public DistrictDates DistrictDates { get; set; }
    }
}

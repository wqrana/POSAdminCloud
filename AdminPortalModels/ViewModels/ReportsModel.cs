using AdminPortalModels.Models;
using System;
using System.Collections.Generic;

namespace AdminPortalModels.ViewModels
{
    public class ReportsModel
    {
        public long ClientID { get; set; }
        public IEnumerable<SchoolItem> locationList { get; set; }
        public IEnumerable<SchoolItem> msa_schoolList { get; set; }
        public IEnumerable<HomeRoomModel> homeRoomList { get; set; }
        public IEnumerable<Grade> gradesList { get; set; }
        public IEnumerable<CustomersList> customersList { get; set; }
        public IEnumerable<AccountStatus> AccountStatus { get; set; }
        public IEnumerable<ItemSelectionType> ItemSelectionTypeList { get; set; }
        public IEnumerable<dynamic> CategoryTypeList { get; set; }
        public IEnumerable<dynamic> CategoryList { get; set; }
        public IEnumerable<dynamic> ItemList { get; set; }
        public IList<string> reportAccountStatusList { get; set; }
        public IList<string> reportQualificationTypesList { get; set; }
        public IList<string> reportZeroBalanceList { get; set; }
        public IList<string> reportBalanceTypesList { get; set; }
        public IList<string> reportDateRangeTypesList { get; set; } //Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
        public IList<string> reportAccountTypeList { get; set; }
        public IList<string> ReportQtyRangeList { get; set; }
        public IList<string> reportNameFormatOptions { get; set; }
        public IList<SortingColumn> reportSortOrder { get; set; }
        public IList<SortingColumn> reportSingleColumnSorting { get; set; }
        public IList<string> reportSessionTypes { get; set; } 
        public IList<string> reportDepositType { get; set; }

        public ShowHideReportsFilters showHideReportsFilters { get; set; }

    }
    public abstract class valueDataClass
    {
        public int? value { get; set; }
        public string data { get; set; }
    }
    public class Location : valueDataClass
    {
    }
    /*
    public class Homerooms : valueDataClass
    {
    }
     * */

    public class CustomersList : valueDataClass
    {
    }
    public class AccountStatus : valueDataClass
    {
    }

    [Serializable]
    public class ReportsFilters
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string SelectedCustomersList { get; set; }
        public string location { get; set; }
        public bool allCustomers { get; set; }
        public bool specificCustomers { get; set; }
        public string homeRoom { get; set; }
        public string grade { get; set; }
        public string[] accountStatus { get; set; }
        public string[] qualificationTypes { get; set; }
        public string balanceActTypes { get; set; }
        public string dateRangeTypes { get; set; }
        public string sortOrder { get; set; }

        public string[] formatName { get; set; }
        public string SessionTypeList { get; set; }
        public string DepositTypeList { get; set; }
        public string MsaSchool { get; set; }
        public string MsaSchoolMultiSelect { get; set; }

        public string Date { get; set; }
        public string DateEnd { get; set; }
        public bool IncludeSchool { get; set; }
        public string SingleColumnSorting { get; set; }

        public string accountype { get; set; }
        public string range_slider_input_start { get; set; }
        public string range_slider_input_end { get; set; }
        public string slider_range_max_amount { get; set; }
        public string slider_range_min_amount { get; set; }
        public string selectedQtyType { get; set; }
        public int? minQty { get; set; }
        public int? maxQty { get; set; }

        public int? itemSelectionType { get; set; }
        public string[] selectedTypeList { get; set; }

        public bool insertPageBreak { get; set; }

    }

    public class ReportsMenu
    {
        //public IDictionary<string, Dictionary<string, List<string>>> reportList { get; set; }

        public IDictionary<FSS.Reports.FSS_REPORT_GROUPS, Dictionary<FSS.Reports.FSS_REPORT_SECTIONS, Dictionary<FSS.Reports.FSS_REPORTS, bool>>> reportList { get; set; }
    }



    public class ShowHideReportsFilters
    {
        private bool isAll()
        {
            if (ReportID == -1)
            {
                return true;
            }
            else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_ALL))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool isNone()
        {
            if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_NONE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ShowHideReportsFilters(int? RptID)
        {
            if (RptID == null)
            {
                ReportID = -1;
            }
            else if (RptID.HasValue)
            {
                ReportID = (int)RptID;
            }
            else
            {
                ReportID = -1;
            }
        }

        private int ReportID { get; set; }
        public bool ShowDateRange
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_DATE_RANGE))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
        public bool ShowDateRangeTypes
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_DATE_RANGE_TYPES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowCustomers
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_CUSTOMERS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowLocactions
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_LOCATIONS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowMsaSchools
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_MSA_SCHOOLS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowMsaSchoolsMultiSelect
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_MSA_SCHOOLS_MULTISELECT))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowDate
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_DATE))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowDateEnd
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_DATE_END))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowIncludeSchool
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_INCLUDE_SCHOOL))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowSingleColumnSorting
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_SINGLE_COLUMN_SORTING))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowHomeRooms
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_HOMEROOMS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowGrade
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_GRADES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowAccountStatus
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_ACCOUNT_STATUS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowQualificationTypes
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_QUALIFICATION_TYPES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowBalanceActTypes
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_BALANCE_TYPES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowAccountType
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_Account_Type))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowsortOrder
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_SORT_ORDER))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowformatName
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_NAME_FORMATS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        //
        public bool ShowSessionType
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_Session_Type))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowDepositType
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_Deposit_Type))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowSelectionTypes
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_SELECTION_TYPE))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowCategoryTypes
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_CATEGORY_TYPES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowCategories
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_CATEGORIES))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowItems
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_ITEMS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool ShowQuantityRange
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_QUANTITY_RANGE))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ShowPagebreakCheck
        {
            get
            {
                if (isAll())
                {
                    return true;
                }
                else if (isNone())
                {
                    return false;
                }
                else if (FSS.Reports.FSSReports.ReportFilterAssignments[(FSS.Reports.FSS_REPORTS)ReportID].Exists(element => element == FSS.Reports.FSS_REPORT_FILTERS.RF_PageBreak))
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

    public class CashierPayments
    {
        public long ClientID { get; set; }
        public Nullable<long> DISTID { get; set; }
        public Nullable<long> SCHID { get; set; }
        public Nullable<long> CSTID { get; set; }
        public Nullable<long> EMPID { get; set; }
        public long POSID { get; set; }
        public string DistrictName { get; set; }
        public string SchoolID { get; set; }
        public string SchoolName { get; set; }
        public string EmpUserID { get; set; }
        public string CashierName { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string CustUserID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Middle { get; set; }
        public Nullable<int> TransType { get; set; }
        public Nullable<double> PaymentAmount { get; set; }
        public string Comments { get; set; }
        public Nullable<double> TotalCashPayments { get; set; }
        public Nullable<double> TotalChecks { get; set; }
        public Nullable<double> TotalCredits { get; set; }
        public Nullable<double> OnlineCreditPayments { get; set; }
        public Nullable<double> OnlineACHPayments { get; set; }
        public Nullable<double> OnlineACHReturns { get; set; }
        public Nullable<double> Refunds { get; set; }
        public Nullable<double> AccountPayments { get; set; }
        public Nullable<double> CashSaleMonies { get; set; }
        public Nullable<double> TotalMonies { get; set; }
        public Nullable<double> TotalDeposit { get; set; }
        public Nullable<int> LineCount { get; set; }
        public int GroupType { get; set; }
        public string FullName { get; set; }
    }

    public class SoldItems
    {
        public long clientid { get; set; }
        public Nullable<long> SCHID { get; set; }
        public System.DateTime GDate { get; set; }
        public string MenuItem { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<double> Price { get; set; }
    }

    public class SortingColumn
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ItemSelectionType
    {
        public int id { get; set; }
        public string name { get; set; }

    }

    [Serializable]
    public class ItemSelectionFilters
    {
        public int selectionType { get; set; }
        public int? categoryType { get; set; }
        public int? category { get; set; }

    }

}


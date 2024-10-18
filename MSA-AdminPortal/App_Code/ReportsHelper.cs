using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using AdminPortalModels.ViewModels;
using FSS.Reports;

namespace MSA_AdminPortal.App_Code
{
    public static class ReportsHelper
    {
        public static IList<string> reportAccountStatus()
        {
            return FSSReports.AccountStatuses;
        }
        public static IList<string> reportQualificationTypes()
        {
            return FSSReports.QualificationTypes;
        }
        public static IList<string> reportZeroBalance()
        {
            return FSSReports.BalanceTypes;
        }
        public static IList<string> reportBalanceTypes()
        {
            return FSSReports.BalanceTypes;
        }
        public static IList<string> reportAccountTypes()
        {
            return FSSReports.AccountTypes;
        }
        public static IList<string> reportQtyRanges()
        {
            return FSSReports.QtyRanges;
        }

        public static IList<string> reportNameFormatOptions()
        {
            return FSSReports.NameFormatOptions;
        }

        public static IList<string> reportSessionTypes()
        {
            return FSSReports.SessionTypes;
        }

        public static IList<string> reportDepositType()
        {
            return FSSReports.DepositType;
        }

        //Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
        public static IList<string> reportDateRangeTypes()
        {
            return FSSReports.DateRangeTypes;
        }

        public static IList<SortingColumn> SortingColumnsList(int? id)
        {
            int ReportID = 0;
            if (id.HasValue)
            {
                ReportID = id.Value;
            }
            FSS.Reports.FSS_REPORTS Report = ((FSS.Reports.FSS_REPORTS)ReportID);
            IList<SortingColumn> list = new List<SortingColumn>();
            var tempList = FSSReports.SortingColumns;
            foreach (var item in tempList)
            {
                list.Add(new SortingColumn { id = item, name = item });
            }


            if (Report == FSS.Reports.FSS_REPORTS.REP_CUSTOMER_ROSTER)
            {
                list.Add(new SortingColumn { id = "PIN", name = "PIN" });
            }
            else if (Report == FSS.Reports.FSS_REPORTS.REP_PREORDER_DISTRIBUTION)
            {
                list.Clear();
                list.Add(new SortingColumn { id = "Name", name = "Name" });
                list.Add(new SortingColumn { id = "User Id", name = "User Id" });
                list.Add(new SortingColumn { id = "Grade", name = "Grade" });
                list.Add(new SortingColumn { id = "Homeroom", name = "Homeroom" });
            }

            return list;

        }

        public static string getReportName(int reportId)
        {
            string fileName = ""; // "DepositTicket.rpt";
            FSS.Reports.FSS_REPORTS key = ((FSS.Reports.FSS_REPORTS)reportId);
            fileName = FSS.Reports.FSSReports.ReportFileNames[key];
            return fileName;
        }

        public static string getDataTabletName(int reportId)
        {
            string retStr = ""; //"DepositTicket";
            FSS.Reports.FSS_REPORTS key = ((FSS.Reports.FSS_REPORTS)reportId);
            retStr = FSS.Reports.FSSReports.ReportDataTableNames[key];
            return retStr;
        }

        public static string getGroupName(FSS_REPORT_GROUPS gp)
        {
            string retStr = "";
            if (FSS.Reports.FSSReports.ReportGroups.Keys.Contains(gp))
            {
                retStr = FSS.Reports.FSSReports.ReportGroups[gp].ToString();
            }
            else
            {
                retStr = "";
            }
            return retStr;
        }



        //public static IDictionary<string, Dictionary<string, List<string>>> GetreportNames()
        public static IDictionary<FSS.Reports.FSS_REPORT_GROUPS, Dictionary<FSS.Reports.FSS_REPORT_SECTIONS, Dictionary<FSS.Reports.FSS_REPORTS, bool>>> GetreportNames()
        {
            return FSS.Reports.FSSReports.ReportMenu;
        }
    }
}
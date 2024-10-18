using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using MSA_AdminPortal.Helpers;

using AdminPortalModels.ViewModels;
using Repository;
using Repository.Helpers;
using MSA_AdminPortal.App_Code;
using CrystalDecisions.ReportAppServer.ClientDoc;
using CrystalDecisions.ReportAppServer.Controllers;
using CrRas = CrystalDecisions.ReportAppServer.DataDefModel;
using System.Globalization;
using MSA_AdminPortal.ADODataSets;
using MSA_ADMIN.DAL.Factories;
using AdminPortalModels.Models;



namespace MSA_AdminPortal.Controllers
{
    public class ReportsController : BaseAuthorizedController
    {
        private IReportGenerator reportGenerator;
        UnitOfWork unitOfWork = null;
        private CategoryTypeHelper categoryTypeHelper = null;
        private CategoryHelper categoryHelper = null;
        private MenuHelper menuHelper = null;
        private HomeRoomHelper homeroomHelper = new HomeRoomHelper();

        public ReportsController()
        {
            this.reportGenerator = new ReportGenerator();
            categoryTypeHelper = new CategoryTypeHelper();
            categoryHelper = new CategoryHelper();
            menuHelper = new MenuHelper();
        }

        public ActionResult Index(int? id)
        {
            try
            {
                string str = "";
                long ClientId = ClientInfoData.GetClientID();

                if (!CheckAccess(id, out str)) return RedirectToAction("NoAccess", "Security", new { id = str });
                unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
                ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
                ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
                ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == ClientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);


                if (id.HasValue)
                {
                    FSS.Reports.FSS_REPORTS myReportID = (FSS.Reports.FSS_REPORTS)id.Value;
                    ViewBag.ReportName = FSS.Reports.FSSReports.ReportNames[myReportID];
                    ViewBag.ReportGroup = FSS.Reports.FSSReports.ReportGroupAssignments[myReportID].ToString();
                    ViewBag.ReportGroupName = FSS.Reports.FSSReports.ReportGroups[FSS.Reports.FSSReports.ReportGroupAssignments[myReportID]].ToString();
                    ViewBag.ReportDescription = "";

                    if (id.Value == 64) //reorder Distribution Labels Report
                    {
                        ViewBag.ReportDescription = "Avery 5160";
                    }
                    else if (id.Value == 65) //Preorder Distribution Labels(school) Report
                    {
                        ViewBag.ReportDescription = "Avery 5163";
                    }



                }

                var model = new ReportsModel();


                //var model = new ReportsModel
                //{
                model.ClientID = ClientId;
                model.customersList = unitOfWork.reportsRepository.getReportsCustomersList(ClientId);
                model.locationList = unitOfWork.generalRepository.getSchools(ClientId, null, false).ToList();
                model.msa_schoolList = ConvertToEnumerable(ReportFactory.GetSchoolsByDistrict(ClientId));
                model.reportAccountStatusList = ReportsHelper.reportAccountStatus();
                model.reportQualificationTypesList = ReportsHelper.reportQualificationTypes();

                model.gradesList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
                model.homeRoomList = unitOfWork.generalRepository.getHomeRooms(ClientId).ToList();

                model.reportZeroBalanceList = ReportsHelper.reportZeroBalance();
                model.reportBalanceTypesList = ReportsHelper.reportBalanceTypes();
                model.reportAccountTypeList = ReportsHelper.reportAccountTypes();
                model.ReportQtyRangeList = ReportsHelper.reportQtyRanges();
                model.reportNameFormatOptions = ReportsHelper.reportNameFormatOptions();
                model.reportSortOrder = ReportsHelper.SortingColumnsList(id); //unitOfWork.generalRepository.GetSortOrderList();
                model.reportSingleColumnSorting = ReportsHelper.SortingColumnsList(id);
                model.showHideReportsFilters = new ShowHideReportsFilters(id);
                model.reportSessionTypes = ReportsHelper.reportSessionTypes();
                model.reportDepositType = ReportsHelper.reportDepositType();
                model.reportDateRangeTypesList = ReportsHelper.reportDateRangeTypes();
                model.CategoryTypeList = categoryTypeHelper.GetSelectList();
                model.ItemList = menuHelper.GetAll();
                model.CategoryList = new List<SelectListItem>();
                model.ItemSelectionTypeList = new List<ItemSelectionType>() 
                {

                    new ItemSelectionType() {id=1,name= "Category Type"}, 
                    new ItemSelectionType() {id=2,name= "Category"},
                    new ItemSelectionType() {id=3,name= "Item"}
                                                                    
                };

                //};
                return View(model);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Index");
                return View();
            }
        }



        IEnumerable<SchoolItem> ConvertToEnumerable(DataTable dt)
        {
            List<SchoolItem> schoolList = new List<SchoolItem>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    SchoolItem schoolItem = new SchoolItem();
                    schoolItem.data = Convert.ToString(row["SchoolName"]);
                    schoolItem.value = Convert.ToInt64(row["CloudPOS_Id"]);
                    schoolItem.DistrictID = Convert.ToInt64(row["District_Id"]);
                    schoolItem.DistrictName = Convert.ToString(row["DistrictName"]);

                    schoolList.Add(schoolItem);
                }
            }
            return schoolList;
        }

        public ActionResult ReportView()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ShowReport(ReportsFilters ReportsFilters, string fromDate)
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            ReportsFilters RptFilters = new ReportsFilters();
            if (Request["filterData"] != null)
            {
                RptFilters = new JavaScriptSerializer().Deserialize<ReportsFilters>(Request["filterData"]);
                System.Web.HttpContext.Current.Session["filterData"] = RptFilters;
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["filterData"] != null)
                {
                    RptFilters = (ReportsFilters)System.Web.HttpContext.Current.Session["filterData"];
                }
                else
                {
                    jsonErrorCode = "-1";
                }
            }
            int reportId = -1;
            bool converted = false;
            bool firstExe = false;
            if (Request["id"] != null)
            {
                converted = int.TryParse(Convert.ToString(Request["id"]), out reportId);
                if (converted)
                {
                    System.Web.HttpContext.Current.Session["reportId"] = Convert.ToString(reportId);
                }
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["reportId"] != null)
                {
                    firstExe = true;
                    reportId = Convert.ToInt32(System.Web.HttpContext.Current.Session["reportId"]);
                    converted = true;
                }
            }

            //check id is valid
            if (!converted)
            {
                isValid = false;
            }
            string msg = "";

            try
            {
                if (!firstExe)
                {
                    return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
                }

                if (isValid)
                {
                    ReportDocument rd = new ReportDocument();
                    rd = reportGenerator.GetReportDocument(reportId, RptFilters);
                    rd.ExportToHttpResponse(ExportFormatType.PortableDocFormat, System.Web.HttpContext.Current.Response, true, "crReport");
                    ViewBag.reportData = false;
                    if (rd != null)
                    {
                        ViewBag.reportData = true;
                    }
                }
                else
                {
                    Response.Write("<H2>Report not found</H2>");
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                if (msg == "Load report failed.")
                {
                    jsonErrorCode = "-3";
                }
                else
                {
                    jsonErrorCode = "-2";
                }
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ShowReport");
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ReportsMenu(string reportGroup = null)
        {
            var model = new ReportsMenu
            {
                reportList = ReportsHelper.GetreportNames()
            };

            if (!string.IsNullOrWhiteSpace(reportGroup))
            {
                ViewBag.ReportGroup = reportGroup;
            }

            return PartialView("_ReportsMenuPartial", model);
        }

        public ActionResult RefreshReport()
        {
            return PartialView("_ReportsDisplay");
        }

        public bool CheckAccess(int? id, out string str)
        {
            int intID = Convert.ToInt32(id);
            bool retValue = true;
            switch (intID)
            {
                case 0:
                case 1:
                    if (!SecurityManager.ViewCustomerReports)
                    {
                        str = "nocustomerReport";
                        retValue = false;
                        break;
                    }
                    else
                    {
                        str = "";
                        retValue = true;
                        break;
                    }

                case 20:
                case 27:
                case 33:
                case 39:

                    if (!SecurityManager.ViewFinancialReports)
                    {
                        str = "nofinanceReport";
                        retValue = false;
                        break;
                    }
                    else
                    {
                        str = "";
                        retValue = true;
                        break;
                    }

                default:
                    {
                        str = "";
                        retValue = true;
                        break;
                    }
            }
            return retValue;
        }

        #region export to excel
        [HttpGet]
        public ActionResult GetExcel(string exportFormat, bool firstExe)
        {
            string jsonErrorCode = "0";
            string msg = "";
            string contentType = "application/pdf";
            string reportName = "";
            string[] strReportName = null;
            // Swtich b/w standard and Lagency mode
            bool ib_useCsvStdMode = false;

            ib_useCsvStdMode = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["useCsvStdMode"]);
            try
            {
                if (exportFormat == null)
                {
                    throw new Exception("Report format not found");
                }
                int reportId = -1;

                ExportOptions exportOption;
                Stream reportDcoumentStream = null;
                ExportRequestContext exportRequest = new ExportRequestContext();
                ReportsFilters RptFilters = new ReportsFilters();
                ReportDocument rd = new ReportDocument();

                if (System.Web.HttpContext.Current.Session["reportId"] != null)
                {
                    reportId = Convert.ToInt32(System.Web.HttpContext.Current.Session["reportId"]);
                    reportName = ReportsHelper.getReportName(reportId);
                    if (reportName != "")
                    {
                        strReportName = reportName.Split('.');
                    }
                    else
                    {
                        strReportName[0] = "reportName";
                    }
                }
                else
                {
                    throw new Exception("Please run the report first.");
                }


                if (System.Web.HttpContext.Current.Session["filterData"] != null)
                {
                    RptFilters = (ReportsFilters)System.Web.HttpContext.Current.Session["filterData"];
                }
                else
                {
                    throw new Exception("Report filters are not found.");
                }

                if (firstExe == false)
                {
                    rd = reportGenerator.GetReportDocument(reportId, RptFilters);
                }
                else
                {
                    return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
                }



                if (rd != null)
                {
                    // Following piece of code is used to convert report document to stream

                    exportOption = rd.ExportOptions;

                    switch (exportFormat)
                    {
                        case "Excel":
                            exportOption.ExportFormatType = ExportFormatType.Excel;
                            reportName = string.Concat(strReportName[0], ".xls");
                            contentType = "application/vnd.ms-excel";
                            break;
                        case "PortableDocFormat":
                            exportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                            reportName = string.Concat(strReportName[0], ".pdf");
                            contentType = "application/pdf";
                            break;
                        case "CharacterSeparatedValues":

                            FSS.Reports.FSS_REPORTS ReportID = ((FSS.Reports.FSS_REPORTS)reportId);
                            exportOption.ExportFormatType = ExportFormatType.CharacterSeparatedValues;
                            /*
                            exportOption.ExportFormatOptions = new CharacterSeparatedValuesFormatOptions()
                            {
                                ExportMode = CsvExportMode.Standard ,
                               GroupSectionsOption = CsvExportSectionsOption.ExportIsolated,
                                ReportSectionsOption = CsvExportSectionsOption.ExportIsolated
                            };
                            */
                            CharacterSeparatedValuesFormatOptions CSVExportFormatOptionsSTD = new CharacterSeparatedValuesFormatOptions()
                            {
                                ExportMode = CsvExportMode.Standard,
                                GroupSectionsOption = CsvExportSectionsOption.ExportIsolated,
                                ReportSectionsOption = CsvExportSectionsOption.ExportIsolated
                            };

                            CharacterSeparatedValuesFormatOptions CSVExportFormatOptionsLGY = new CharacterSeparatedValuesFormatOptions()
                            {
                                ExportMode = CsvExportMode.Legacy

                            };

                            if (ib_useCsvStdMode)
                            {
                                switch (ReportID)
                                {
                                    // 1482	Fix POS Cloud Admin Site(Report Section)- Presale Student No Orders CSV file issue         
                                    //1485	Fix POS Cloud Admin Site(Report Section)-CSV file issue of Lunch Menu Report
                                    // 1480	Fix POS Cloud Admin Site(Report Section)-CSV File issue of Preorder History Report   
                                    case FSS.Reports.FSS_REPORTS.REP_PRESALE_STUDENT_NOORDERS:
                                        exportOption.ExportFormatOptions = CSVExportFormatOptionsSTD;
                                        break;
                                    case FSS.Reports.FSS_REPORTS.REP_PARENT:
                                        exportOption.ExportFormatOptions = CSVExportFormatOptionsSTD;
                                        break;
                                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_HISTORY:
                                        exportOption.ExportFormatOptions = CSVExportFormatOptionsSTD;
                                        break;
                                    case FSS.Reports.FSS_REPORTS.REP_LUNCH_MENU:
                                        exportOption.ExportFormatOptions = CSVExportFormatOptionsSTD;
                                        break;
                                    case FSS.Reports.FSS_REPORTS.REP_CC_PROCESSING:
                                        exportOption.ExportFormatOptions = CSVExportFormatOptionsSTD;
                                        break;
                                    default:
                                        exportOption.ExportFormatOptions = CSVExportFormatOptionsLGY;
                                        break;

                                }
                            }
                            else
                            {
                                exportOption.ExportFormatOptions = CSVExportFormatOptionsLGY;
                            }

                            reportName = string.Concat(strReportName[0], ".csv");
                            contentType = "application/csv";
                            break;
                        case "WordForWindows":
                            exportOption.ExportFormatType = ExportFormatType.WordForWindows;
                            reportName = string.Concat(strReportName[0], ".doc");
                            contentType = "application/msword";
                            break;
                        default:
                            exportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                            break;
                    }


                    exportRequest.ExportInfo = exportOption;
                    reportDcoumentStream = rd.FormatEngine.ExportToStream(exportRequest);

                    return File(reportDcoumentStream, contentType, reportName);
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                if (msg == "Please Run the Report first.")
                {
                    jsonErrorCode = "-3";
                }
                else
                {
                    jsonErrorCode = "-2";
                }
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ShowReport");
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);

        }

        #endregion

        public JsonResult GetRptFiltersDropdownData()
        {
            dynamic dropdownData = null;
            string[] emptyStringArray = new string[0];

            ItemSelectionFilters filters = new ItemSelectionFilters();
            filters = new JavaScriptSerializer().Deserialize<ItemSelectionFilters>(Request["filterData"]);

            try
            {
                if (filters != null)
                {
                    if (filters.selectionType == 1)
                    {
                        dropdownData = this.GetCategoryTypeList();

                    }
                    else if (filters.selectionType == 2)
                    {
                        dropdownData = this.GetCategoryList(filters.categoryType);

                    }

                    else if (filters.selectionType == 3)
                    {

                        dropdownData = this.GetItemList(filters.categoryType, filters.category);
                    }
                }

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call");
                return null;
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = dropdownData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }

        public IEnumerable<SelectListItem> GetCategoryTypeList()
        {

            return categoryTypeHelper.GetSelectList();
        }

        public IEnumerable<SelectListItem> GetCategoryList(long? categoryTypeID)
        {

            if (categoryTypeID == null)
            {

                return categoryHelper.GetSelectList();
            }
            else
            {

                return categoryHelper.GetAll().Where(x => x.CategoryType_Id == categoryTypeID).Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name });

            }
        }

        public IEnumerable<SelectListItem> GetItemList(int? categoryTypeID, int? categoryID)
        {

            if (categoryTypeID == null && categoryID == null)
            {
                return menuHelper.GetAll().Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.ItemName });

            }

            else if (categoryTypeID != null && categoryID == null)
            {

                return menuHelper.GetAll(categoryTypeID.Value).Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.ItemName });

            }
            else if (categoryTypeID != null && categoryID != null)
            {

                return menuHelper.GetIndexModelByCategory(categoryID.Value).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name });

            }

            return null;
        }

    }



    /// <summary>
    /// /////
    /// </summary>
    public interface IReportGenerator : IDisposable
    {
        ReportDocument GetReportDocument(int rptId, ReportsFilters ReportsFilters);
    }
    public class ReportGenerator : IReportGenerator, IDisposable
    {
        //private IReportsRepository reportsRepository;
        UnitOfWork unitOfWork = null;
        private DataTable rptDataTable;
        private System.Data.DataSet ReportDataSet;
        public ReportGenerator()
        {
            //this.reportsRepository = new ReportsRepository(new Repository.edmx.PortalContext());
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

        public ReportDocument GetReportDocument(int rptId, ReportsFilters ReportsFilters)
        {
            try
            {
                int reportId = rptId;

                string strReportName = string.Empty;
                string strDataSetName = string.Empty;
                string strRptPath = string.Empty;
                bool upperSelected = false;
                upperSelected = isUpperSelected(ReportsFilters);
                string nameFormat = "";


                //get dataset name
                strDataSetName = ReportsHelper.getDataTabletName(reportId);
                rptDataTable = new DataTable(strDataSetName);

                strReportName = ReportsHelper.getReportName(reportId);

                FSS.Reports.FSS_REPORTS ReportID = ((FSS.Reports.FSS_REPORTS)reportId);
                long ClientId = ClientInfoData.GetClientID();
               
                ReportDataSet = new DataSet();
                // load reports source here, for each report 

                //To get MSA SchoolID based on  POS Cloud School ID
                String MsaSchoolID = "";
                switch (ReportID)
                {
                    case FSS.Reports.FSS_REPORTS.REP_DEPOSIT_TICKET:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getDepositTicket(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;

                    case FSS.Reports.FSS_REPORTS.REP_DAILY_CASHIER:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getDailyCashier(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    //id=0 
                    case FSS.Reports.FSS_REPORTS.REP_CUSTOMER_ROSTER:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getCustomerRoster(ClientId, reportId, ReportsFilters).ToList(), upperSelected);

                        //GetNameFormat is called to determin the fomat of names
                        nameFormat = GetNameFormat(ReportsFilters);
                        if (isUpperSelected(ReportsFilters))
                        {
                            rptDataTable.Columns["ExtraInfo"].Expression = string.Format(nameFormat, "FirstName", "Middle", "LastName");// rptDataSource.Columns["FirstName"].ToString() + rptDataSource.Columns["LastName"].ToString();
                        }
                        else
                        {
                            rptDataTable.Columns["ExtraInfo"].Expression = string.Format(nameFormat, "FirstName", "Middle", "LastName");
                        }
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_MEAL_ROSTER:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getCustomerRoster(ClientId, reportId, ReportsFilters).ToList(), upperSelected);


                        //GetNameFormat is called to determin the fomat of names
                        nameFormat = GetNameFormat(ReportsFilters);
                        if (isUpperSelected(ReportsFilters))
                        {
                            rptDataTable.Columns["ExtraInfo"].Expression = string.Format(nameFormat, "FirstName", "Middle", "LastName");// rptDataSource.Columns["FirstName"].ToString() + rptDataSource.Columns["LastName"].ToString();
                        }
                        else
                        {
                            rptDataTable.Columns["ExtraInfo"].Expression = string.Format(nameFormat, "FirstName", "Middle", "LastName");
                        }
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_ACCOUNT_BALANCE:

                        while (ReportDataSet.Tables.Count > 0)
                        {
                            DataTable table = ReportDataSet.Tables[0];
                            if (ReportDataSet.Tables.CanRemove(table))
                            {
                                ReportDataSet.Tables.Remove(table);
                            }
                        }
                        var temData = unitOfWork.reportsRepository.getCustomerBalanceData(ClientId, reportId, ReportsFilters);
                        var tempData2 = temData.ToList();
                        rptDataTable = ToDataTable(tempData2, upperSelected);
                        rptDataTable.TableName = "CustomerBalance";

                        ReportDataSet.Tables.Add(rptDataTable);
                        break;

                    case FSS.Reports.FSS_REPORTS.REP_STATEMENT:

                        DataTable StatementOrdersTable = new DataTable("StatementOrders");
                        StatementOrdersTable = ToDataTable(unitOfWork.reportsRepository.getStatementOrdersSummary(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        StatementOrdersTable.TableName = "StatementOrders";


                        ReportDataSet.Tables.Add(StatementOrdersTable);


                        DataTable CustomerRosterTable = new DataTable("CustomerRoster");
                        CustomerRosterTable = ToDataTable(unitOfWork.reportsRepository.getCustomerRosterSummary(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        CustomerRosterTable.TableName = "CustomerRoster";

                        ReportDataSet.Tables.Add(CustomerRosterTable);

                        //Add relation
                        DataColumn StatementOrdersCol = ReportDataSet.Tables["StatementOrders"].Columns["SCHID"];
                        DataColumn CustomerRosterCol = ReportDataSet.Tables["CustomerRoster"].Columns["SCHID"];
                        // Create DataRelation.
                        DataRelation relCustOrder;
                        relCustOrder = new DataRelation("CustRosterStatmentOrders", StatementOrdersCol, CustomerRosterCol, false);
                        // Add the relation to the DataSet.
                        ReportDataSet.Relations.Add(relCustOrder);
                        break;

                    case FSS.Reports.FSS_REPORTS.REP_DETAILED_STATEMENT:

                        DataTable CustomerRosterTableDetailed = new DataTable("CustomerRoster");
                        CustomerRosterTableDetailed = ToDataTable(unitOfWork.reportsRepository.getCustomerRosterSummary(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        CustomerRosterTableDetailed.TableName = "CustomerRoster";

                        ReportDataSet.Tables.Add(CustomerRosterTableDetailed);

                        DataTable DetailedOrders = new DataTable("StatementDetailOrders");
                        DetailedOrders = ToDataTable(unitOfWork.reportsRepository.getStatementDetailedOrders(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        DetailedOrders.TableName = "StatementDetailOrders";

                        ReportDataSet.Tables.Add(DetailedOrders);

                        DataTable DetailedItems = new DataTable("StatementDetailItems");
                        DetailedItems = ToDataTable(unitOfWork.reportsRepository.getStatementDetailedItems(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        DetailedItems.TableName = "StatementDetailItems";

                        ReportDataSet.Tables.Add(DetailedItems);


                        break;

                    case FSS.Reports.FSS_REPORTS.REP_PAYMENTS_BY_CASHIER:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getPaymentsByCashier(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        nameFormat = GetNameFormat(ReportsFilters);
                        if (isUpperSelected(ReportsFilters))
                        {
                            rptDataTable.Columns["FullName"].Expression = string.Format(nameFormat, "FirstName", "Middle", "LastName");// rptDataSource.Columns["FirstName"].ToString() + rptDataSource.Columns["LastName"].ToString();
                        }
                        else
                        {
                            rptDataTable.Columns["FullName"].Expression = string.Format(nameFormat, "FirstName", "Middle", "LastName");
                        }
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_CAFETERIA:
                        DataTable CafeCategoriesTable = new DataTable("DetailCafeCategories");

                        CafeCategoriesTable = ToDataTable(unitOfWork.reportsRepository.getDetailCafeCategories(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        CafeCategoriesTable.TableName = "DetailCafeCategories";
                        ReportDataSet.Tables.Add(CafeCategoriesTable);

                        DataTable SalesTaxonOrderTable = new DataTable("SalesTaxonOrder");
                        SalesTaxonOrderTable = ToDataTable(unitOfWork.reportsRepository.getSalesTaxOnOrders(ClientId).ToList(), upperSelected);
                        ReportDataSet.Tables.Add(SalesTaxonOrderTable);

                        //get datasets of subreports
                        DataTable DetailCafeCashiersTable = new DataTable();
                        DetailCafeCashiersTable = ToDataTable(unitOfWork.reportsRepository.getDetailCafeCashiers(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        DetailCafeCashiersTable.TableName = "DetailCafeCashiers";
                        ReportDataSet.Tables.Add(DetailCafeCashiersTable);

                        DataTable DetailcafeSummaryTable = new DataTable();
                        // changed by farrukh m (allshore) on 05/12/16 to fix item PA-522
                        var result = unitOfWork.reportsRepository.getDetailCafeSummaries(ClientId, reportId, ReportsFilters).Distinct().ToList();
                        DetailcafeSummaryTable = ToDataTable(result, upperSelected);
                        //---------------------------------------------------------------------
                        DetailcafeSummaryTable.TableName = "DetailCafeSummary";
                        ReportDataSet.Tables.Add(DetailcafeSummaryTable);

                        break;
                    case FSS.Reports.FSS_REPORTS.REP_SOLD_ITEMS:
                        //DataTable SoldItemsTable = new DataTable();
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getSoldItems(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_SOLD_ITEMS_BY_CATEGORY:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getSoldItemsByCategory(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        rptDataTable.TableName = "SoldItemsByCategory";
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_ITEM_CUSTOMERS:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getCustomerBySoldItems(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        rptDataTable.TableName = "CustomerBySoldItems";
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_VOIDS:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getVoidItems(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        rptDataTable.TableName = "VoidsDS";
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_DETAILED_TRANSACTION:
                        rptDataTable = ToDataTable(unitOfWork.reportsRepository.getDetailedTransactions(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                        rptDataTable.TableName = "DetailedTransaction";
                        ReportDataSet.Tables.Add(rptDataTable);
                        break;

                    case FSS.Reports.FSS_REPORTS.REP_PARENT:
                        {
                            DataTable dtPayemtns = ReportFactory.GetDistrict(ClientId).Copy();
                            dtPayemtns.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtPayemtns);

                            DataTable dtParent = ReportFactory.GetParentsByDistrict(ClientId).Copy();
                            dtParent.TableName = "Parents";
                            ReportDataSet.Tables.Add(dtParent);

                            DataTable dtStudent = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudent.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudent);

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_ACH_DEPOSIT:
                        {
                            DataTable dtTransactions = ReportFactory.GetAch(ClientId).Copy();
                            dtTransactions.TableName = "AchReport";
                            ReportDataSet.Tables.Add(dtTransactions);

                            DataTable dtPayemtns = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtPayemtns.TableName = "Students";
                            ReportDataSet.Tables.Add(dtPayemtns);

                            DataTable dtOldStudents = ReportFactory.GetOldStudentsByDistrictId(ClientId).Copy();
                            dtOldStudents.TableName = "OldStudents";
                            ReportDataSet.Tables.Add(dtOldStudents);

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_CC_DEPOSIT:
                        {
                            DataTable dtCcDeposit = ReportFactory.GetCcDepositReport(ClientId, Convert.ToDateTime(ReportsFilters.fromDate), Convert.ToDateTime(ReportsFilters.toDate)).Copy();
                            dtCcDeposit.TableName = "CcDepositReport";
                            ReportDataSet.Tables.Add(dtCcDeposit);
                            /*
                            DataTable dtTransactions = ReportFactory.GetTransactionsForCcDeposit(ClientId).Copy();
                            dtTransactions.TableName = "Transactions";
                            ReportDataSet.Tables.Add(dtTransactions);

                            DataTable dtPayemtns = ReportFactory.GetPaymentsForCcDeposit(ClientId).Copy();
                            dtPayemtns.TableName = "PaymentsCompleted";
                            ReportDataSet.Tables.Add(dtPayemtns);

                            DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtParents = ReportFactory.GetParentsByDistrict(ClientId).Copy();
                            dtParents.TableName = "Parents";
                            ReportDataSet.Tables.Add(dtParents);

                           // DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            DataTable dtStudents = ReportFactory.GetStudentsForCcDeposit(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            //DataTable dtOldStudents = ReportFactory.GetOldStudentsByDistrictId(ClientId).Copy();
                            DataTable dtOldStudents = ReportFactory.GetOldStudentsForCcDeposit(ClientId).Copy();
                            dtOldStudents.TableName = "OldStudents";
                            ReportDataSet.Tables.Add(dtOldStudents);
                            */
                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_CC_PROCESSING:
                        {
                            DataTable dtTransactions = ReportFactory.GetTransactionsForCcDeposit(ClientId).Copy();
                            dtTransactions.TableName = "Transactions";
                            ReportDataSet.Tables.Add(dtTransactions);

                            DataTable dtParents = ReportFactory.GetParentsByDistrict(ClientId).Copy();
                            dtParents.TableName = "Parents";
                            ReportDataSet.Tables.Add(dtParents);

                            DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtCreditCardHistory = ReportFactory.GetCreditCardHistoryForCcProcessing(ClientId).Copy();
                            dtCreditCardHistory.TableName = "CreditCardHistory";
                            ReportDataSet.Tables.Add(dtCreditCardHistory);


                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PRODUCTION_SUMMARY_MSAADMIN:
                        {
                            /*DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtSchools = ReportFactory.GetSchoolsByDistrict(ClientId).Copy();
                            dtSchools.TableName = "Schools";
                            ReportDataSet.Tables.Add(dtSchools);

                            DataTable dtParents = ReportFactory.GetParentsByDistrict(ClientId).Copy();
                            dtParents.TableName = "Parents";
                            ReportDataSet.Tables.Add(dtParents);

                            DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtCategory = ReportFactory.GetCategoryForProductionSummary(ClientId).Copy();
                            dtCategory.TableName = "Category";
                            ReportDataSet.Tables.Add(dtCategory);

                            DataTable dtPreSaleTransactions = ReportFactory.GetPreSaleTransactionForProductionSummary(ClientId).Copy();
                            dtPreSaleTransactions.TableName = "PreSaleTransactions";
                            ReportDataSet.Tables.Add(dtPreSaleTransactions);*/

                            bool isError;
                            string outMessage;
                            var reportDataSet = POSAPIHepler.GetProductionSummary(ClientId, ReportsFilters.fromDate, ReportsFilters.toDate, out isError, out outMessage);
                            if (isError)
                            {
                                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + outMessage, CommonClasses.getCustomerID(), "GetReportDocument");
                            }

                            ReportDataSet = reportDataSet;

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PRODUCTION_SUMMARY_BY_GRADE_District:
                        {
                            /*DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);
                        
                            DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtCategory = ReportFactory.GetCategoryForProductionSummary(ClientId).Copy();
                            dtCategory.TableName = "Category";
                            ReportDataSet.Tables.Add(dtCategory);

                            DataTable dtPreSaleTransactions = ReportFactory.GetPreSaleTransactionForProductionSummary(ClientId).Copy();
                            dtPreSaleTransactions.TableName = "PreSaleTransactions";
                            ReportDataSet.Tables.Add(dtPreSaleTransactions);*/

                            bool isError;
                            string outMessage;
                            var reportDataSet = POSAPIHepler.GetProductionSummaryPerDistrictsByGrade(ClientId, ReportsFilters.fromDate, ReportsFilters.toDate, out isError, out outMessage);
                            if (isError)
                            {
                                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + outMessage, CommonClasses.getCustomerID(), "GetReportDocument");
                            }

                            ReportDataSet = reportDataSet;

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PRODUCTION_SUMMARY_BY_GRADE_School:
                        {
                            /*DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtSchools = ReportFactory.GetSchoolsByDistrict(ClientId).Copy();
                            dtSchools.TableName = "Schools";
                            ReportDataSet.Tables.Add(dtSchools);

                            DataTable dtParents = ReportFactory.GetParentsByDistrict(ClientId).Copy();
                            dtParents.TableName = "Parents";
                            ReportDataSet.Tables.Add(dtParents);

                            DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtCategory = ReportFactory.GetCategoryForProductionSummary(ClientId).Copy();
                            dtCategory.TableName = "Category";
                            ReportDataSet.Tables.Add(dtCategory);

                            DataTable dtPreSaleTransactions = ReportFactory.GetPreSaleTransactionForProductionSummary(ClientId).Copy();
                            dtPreSaleTransactions.TableName = "PreSaleTransactions";
                            ReportDataSet.Tables.Add(dtPreSaleTransactions);*/

                            bool isError;
                            string outMessage;
                            var reportDataSet = POSAPIHepler.GetProductionSummaryByGrade(ClientId, ReportsFilters.fromDate, ReportsFilters.toDate, ReportsFilters.MsaSchool, out isError, out outMessage);
                            if (isError)
                            {
                                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + outMessage, CommonClasses.getCustomerID(), "GetReportDocument");
                            }

                            ReportDataSet = reportDataSet;

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_DISTRIBUTION:
                        {
                            /*DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtPreSaleTransactions = ReportFactory.GetPreSaleTransactionForProductionSummary(ClientId).Copy();
                            dtPreSaleTransactions.TableName = "PreSaleTransactions";
                            ReportDataSet.Tables.Add(dtPreSaleTransactions);*/

                            bool isError;
                            string outMessage;
                            var reportDataSet = POSAPIHepler.GetPreorderDistribution(ClientId, ReportsFilters.fromDate, out isError, out outMessage);
                            if (isError)
                            {
                                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + outMessage, CommonClasses.getCustomerID(), "GetReportDocument");
                            }

                            ReportDataSet = reportDataSet;

                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_DISTRIBUTION_LABELS:
                        {
                            /*DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtSchools = ReportFactory.GetSchoolsByDistrict(ClientId).Copy();
                            dtSchools.TableName = "Schools";
                            ReportDataSet.Tables.Add(dtSchools);

                            DataTable dtPreSaleTransactions = ReportFactory.GetPreSaleTransactionForProductionSummary(ClientId).Copy();
                            dtPreSaleTransactions.TableName = "PreSaleTransactions";
                            ReportDataSet.Tables.Add(dtPreSaleTransactions);*/

                            bool isError;
                            string outMessage;
                            var reportDataSet = POSAPIHepler.GetPreorderDistributionLabels(ClientId, ReportsFilters.fromDate, ReportsFilters.toDate, out isError, out outMessage);
                            if (isError)
                            {
                                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + outMessage, CommonClasses.getCustomerID(), "GetReportDocument");
                            }

                            ReportDataSet = reportDataSet;

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_DISTRIBUTION_LABELS_SCHOOLS:
                        {
                            /*DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtSchools = ReportFactory.GetSchoolsByDistrict(ClientId).Copy();
                            dtSchools.TableName = "Schools";
                            ReportDataSet.Tables.Add(dtSchools);

                            DataTable dtPreSaleTransactions = ReportFactory.GetPreSaleTransactionForProductionSummary(ClientId).Copy();
                            dtPreSaleTransactions.TableName = "PreSaleTransactions";
                            ReportDataSet.Tables.Add(dtPreSaleTransactions);*/

                            bool isError;
                            string outMessage;
                            var reportDataSet = POSAPIHepler.GetPreorderDistributionLabelsBySchools(ClientId, ReportsFilters.fromDate, ReportsFilters.toDate, ReportsFilters.MsaSchoolMultiSelect, out isError, out outMessage);
                            if (isError)
                            {
                                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + outMessage, CommonClasses.getCustomerID(), "GetReportDocument");
                            }

                            ReportDataSet = reportDataSet;

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_HISTORY:
                        {
                            DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            dtDistrict.TableName = "Districts";
                            ReportDataSet.Tables.Add(dtDistrict);

                            DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            dtStudents.TableName = "Students";
                            ReportDataSet.Tables.Add(dtStudents);

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtPreSaleTransactions = ReportFactory.GetPreSaleTransactionForProductionSummary(ClientId).Copy();
                            dtPreSaleTransactions.TableName = "PreSaleTransactions";
                            ReportDataSet.Tables.Add(dtPreSaleTransactions);

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PRESALE_STUDENT_NOORDERS:
                        {
                            //DataTable dtDistrict = ReportFactory.GetDistrict(ClientId).Copy();
                            //dtDistrict.TableName = "Districts";
                            //ReportDataSet.Tables.Add(dtDistrict);

                            //DataTable dtStudents = ReportFactory.GetStudentsByDistrict(ClientId).Copy();
                            //dtStudents.TableName = "Students";
                            //ReportDataSet.Tables.Add(dtStudents);

                            //DataTable dtParents = ReportFactory.GetParentsByDistrict(ClientId).Copy();
                            //dtParents.TableName = "Parents";
                            //ReportDataSet.Tables.Add(dtParents);

                            //DataTable dtPreSale = ReportFactory.GetPreSale(ClientId).Copy();
                            //dtPreSale.TableName = "PreSale";
                            //ReportDataSet.Tables.Add(dtPreSale);

                            //DataTable dtPreSaleCompleted = ReportFactory.GetPreSaleCompleted(ClientId).Copy();
                            //dtPreSaleCompleted.TableName = "PreSaleCompleted";
                            //ReportDataSet.Tables.Add(dtPreSaleCompleted);

                            DataTable dtPresaleStudentNoOrders = ReportFactory.GetPresaleStudentNoOrders(ClientId).Copy();
                            dtPresaleStudentNoOrders.TableName = "PresaleStudentsWithNoOrders";
                            ReportDataSet.Tables.Add(dtPresaleStudentNoOrders);

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_LUNCH_MENU:
                        {


                            string searchExpression = "CloudPOS_id = " + ReportsFilters.MsaSchool;
                            MsaSchoolID = ReportsFilters.MsaSchool;
                            DataTable dtSchools = ReportFactory.GetSchoolsByDistrict(ClientId).Copy();
                            dtSchools.TableName = "Schools";
                            ReportDataSet.Tables.Add(dtSchools);

                            DataRow[] foundRows = dtSchools.Select(searchExpression);
                            if (foundRows.Length > 0)
                            {
                                DataRow dr = foundRows[0];
                                MsaSchoolID = dr["Id"].ToString();
                            }

                            DataTable dtMenu = ReportFactory.GetMenuForProductionSummary(ClientId).Copy();
                            dtMenu.TableName = "Menu";
                            ReportDataSet.Tables.Add(dtMenu);

                            DataTable dtWebLunchCalendar = ReportFactory.GetWebLunchCalendarByDistrict(ClientId).Copy();
                            dtWebLunchCalendar.TableName = "WebLunchCalendar";
                            ReportDataSet.Tables.Add(dtWebLunchCalendar);

                            DataTable dtWebLunchSchools = ReportFactory.GetWebLunchSchoolsByDistrict(ClientId).Copy();
                            dtWebLunchSchools.TableName = "WebLunchSchools";
                            ReportDataSet.Tables.Add(dtWebLunchSchools);

                            DataTable dtCal = ReportFactory.GetCal(ClientId).Copy();
                            dtCal.TableName = "Cal";
                            ReportDataSet.Tables.Add(dtCal);

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PURCHASE_REPORT:
                        {
                            DataSet ds = ReportFactory.GetPurchaseReportData(ClientId);
                            DataTable parentReportDataTable = new DataTable("ShoppingCartReport");
                            DataTable subReportDataTable = new DataTable("ac_OrderItemInputs");
                            if (ds != null && ds.Tables.Count > 0)
                            {
                                parentReportDataTable = ds.Tables[0].Copy();
                                subReportDataTable = ds.Tables[1].Copy();
                            }
                            ReportDataSet.Tables.Add(parentReportDataTable);
                            ReportDataSet.Tables.Add(subReportDataTable);
                            ReportDataSet.Tables[0].TableName = "ShoppingCartReport";
                            ReportDataSet.Tables[1].TableName = "ac_OrderItemInputs";

                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PROCESSING_REPORT:
                        {
                            DataTable dtProcessingReportData = ReportFactory.GetProcessingReportData(ClientId).Copy();
                            dtProcessingReportData.TableName = "ViewOrders";
                            ReportDataSet.Tables.Add(dtProcessingReportData);

                            break;
                        }

                    default:

                        break;
                }
                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "CrystalReports//" + strReportName;

                ReportDocument rptDocument = new ReportDocument();
                rptDocument.Load(strRptPath);

                if (ReportDataSet != null && ReportDataSet.GetType().ToString() != "System.String")
                {
                    rptDocument.SetDataSource(ReportDataSet);
                    switch (ReportID)
                    {
                        //only cafeteria reports have subreports 
                        case FSS.Reports.FSS_REPORTS.REP_CAFETERIA:
                            DataSet InfoSection = new DataSet();

                            DataTable AccountInfoTable = new DataTable();
                            AccountInfoTable = ToDataTable(unitOfWork.reportsRepository.getAccountInfoes(ClientId, ReportsFilters).ToList(), upperSelected);
                            AccountInfoTable.TableName = "AccountInfo";
                            InfoSection.Tables.Add(AccountInfoTable);

                            DataTable DetailcafenetChangeTable = new DataTable();
                            DetailcafenetChangeTable = ToDataTable(unitOfWork.reportsRepository.getDetailCafeNetChanges(ClientId, reportId, ReportsFilters).ToList(), upperSelected);
                            DetailcafenetChangeTable.TableName = "DetailCafeNetChange";
                            InfoSection.Tables.Add(DetailcafenetChangeTable);

                            rptDocument.OpenSubreport("Cafeteria Account Info Section.rpt").SetDataSource(InfoSection);

                            rptDocument.OpenSubreport("Cafeteria Payments Section.rpt").SetDataSource(ReportDataSet.Tables["DetailCafeSummary"]);

                            rptDocument.OpenSubreport("Cafeteria Open Sessions Section.rpt").SetDataSource(ReportDataSet.Tables["DetailCafeCashiers"]);


                            break;
                        default:
                            break;
                    }
                }

                //add parameters for all reports here

                switch (ReportID)
                {
                    case FSS.Reports.FSS_REPORTS.REP_ACCOUNT_BALANCE:

                        rptDocument = addNewSortingToReport(rptDocument, ReportsFilters.sortOrder, true);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_CUSTOMER_ROSTER:

                        rptDocument = addNewSortingToReport(rptDocument, ReportsFilters.sortOrder, true);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_MEAL_ROSTER:

                        rptDocument = addNewSortingToReport(rptDocument, ReportsFilters.sortOrder, true);
                        break;

                    case FSS.Reports.FSS_REPORTS.REP_DEPOSIT_TICKET:
                        //add parameteres if needed
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_DAILY_CASHIER:
                        ParameterFields paramFields = new ParameterFields();
                        //add parameteres if needed
                        break;

                    case FSS.Reports.FSS_REPORTS.REP_STATEMENT:

                        rptDocument.SetParameterValue("StartDate", ReportsFilters.fromDate);
                        string toDate = getOnlyDate(ReportsFilters.toDate);// dt.ToString("MM/dd/yyyy");
                        rptDocument.SetParameterValue("EndDate", toDate);

                        rptDocument = addGroupSortingStatment(rptDocument, ReportsFilters.sortOrder);

                        break;
                    case FSS.Reports.FSS_REPORTS.REP_DETAILED_STATEMENT:
                        //add sorting 
                        rptDocument = addNewSortingToReport(rptDocument, ReportsFilters.sortOrder, true);

                        rptDocument.SetParameterValue("StartDate", ReportsFilters.fromDate);
                        string toDate2 = getOnlyDate(ReportsFilters.toDate);// dt.ToString("MM/dd/yyyy");

                        rptDocument.SetParameterValue("EndDate", toDate2);
                        rptDocument = addGroupSortingStatment(rptDocument, ReportsFilters.sortOrder);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_SOLD_ITEMS_BY_CATEGORY:

                        string StartDatecat = ReportsFilters.fromDate;// dt.ToString("MM/dd/yyyy");
                        rptDocument.SetParameterValue("StartDate", StartDatecat);

                        string toDatecat = getOnlyDate(ReportsFilters.toDate);// dt.ToString("MM/dd/yyyy");
                        rptDocument.SetParameterValue("EndDate", toDatecat);

                        break;

                    case FSS.Reports.FSS_REPORTS.REP_ITEM_CUSTOMERS:
                        //add sorting 
                        rptDocument = addNewSortingToReport(rptDocument, ReportsFilters.sortOrder, false);
                        string StartDateCust = ReportsFilters.fromDate;// dt.ToString("MM/dd/yyyy");
                        rptDocument.SetParameterValue("StartDate", StartDateCust);

                        string toDateCust = getOnlyDate(ReportsFilters.toDate);// dt.ToString("MM/dd/yyyy");
                        rptDocument.SetParameterValue("EndDate", toDateCust);


                        break;

                    case FSS.Reports.FSS_REPORTS.REP_VOIDS:  // Added by farrukh m (allshore) on 05/10/16 to fix item PA-519

                        rptDocument.SetParameterValue("StartDate", ReportsFilters.fromDate);

                        string endDate = getOnlyDate(ReportsFilters.toDate);
                        rptDocument.SetParameterValue("EndDate", endDate);

                        break;
                    case FSS.Reports.FSS_REPORTS.REP_DETAILED_TRANSACTION:
                        string[] schoolIds = string.IsNullOrEmpty(ReportsFilters.location) ? new string[0] : ReportsFilters.location.Split(',');
                        List<SchoolItem> schools = unitOfWork.generalRepository.getSchools(ClientId, null, false).ToList();
                        //string schoolsNames = string.Join("/", schools.Where(x => schoolIds.Contains(x.value.ToString())).Select(x => x.data));

                        List<string> namesList = new List<string>();
                        var districts = schools.Select(x => new { x.DistrictID, x.DistrictName }).Distinct();
                        if (string.IsNullOrEmpty(ReportsFilters.location))
                        {
                            namesList.AddRange(districts.Select(x => x.DistrictName));
                        }
                        else
                        {
                            foreach (var dist in districts)
                            {
                                var distSchools = schools.Where(x => x.DistrictID == dist.DistrictID);
                                if (distSchools.All(x => schoolIds.Contains(x.value.ToString())))
                                {
                                    namesList.Add(dist.DistrictName);
                                }
                                else
                                {
                                    namesList.AddRange(distSchools.Where(x => schoolIds.Contains(x.value.ToString())).Select(x => x.data));
                                }
                            }
                        }

                        string schoolsNames = string.Join(" / ", namesList);

                        string end = getOnlyDate(ReportsFilters.toDate);

                        rptDocument.SetParameterValue("IsSingleSchoolSelected", schoolIds.Length == 1);
                        rptDocument.SetParameterValue("SchoolsList", schoolsNames);
                        rptDocument.SetParameterValue("StartDate", ReportsFilters.fromDate);
                        rptDocument.SetParameterValue("EndDate", end);
                        break;
                    case FSS.Reports.FSS_REPORTS.REP_PARENT:
                        rptDocument.SetParameterValue("DistrictID", ClientInfoData.GetClientID());
                        break;

                    case FSS.Reports.FSS_REPORTS.REP_ACH_DEPOSIT:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_CC_DEPOSIT:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_CC_PROCESSING:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            rptDocument.SetParameterValue("StartDate", Convert.ToDateTime(ReportsFilters.fromDate));
                            rptDocument.SetParameterValue("EndDate", Convert.ToDateTime(ReportsFilters.toDate));
                            break;
                        }

                    case FSS.Reports.FSS_REPORTS.REP_PRODUCTION_SUMMARY_MSAADMIN:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PRODUCTION_SUMMARY_BY_GRADE_District:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PRODUCTION_SUMMARY_BY_GRADE_School:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            rptDocument.SetParameterValue("SchoolID", Convert.ToInt32(ReportsFilters.MsaSchool));
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_DISTRIBUTION:
                        {

                            string servingDate = ReportsFilters.fromDate;
                            string sortColumn = ReportsFilters.SingleColumnSorting;
                            rptDocument.SetParameterValue("ServingDate", Convert.ToDateTime(servingDate));
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            rptDocument.SetParameterValue("SortOption", sortColumn);
                            rptDocument.SetParameterValue("insertPageBreak", ReportsFilters.insertPageBreak);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_DISTRIBUTION_LABELS:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_DISTRIBUTION_LABELS_SCHOOLS:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            rptDocument.SetParameterValue("SchoolIDList", ReportsFilters.MsaSchoolMultiSelect);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PREORDER_HISTORY:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PRESALE_STUDENT_NOORDERS:
                        {
                            //rptDocument.SetParameterValue("StartDate", Convert.ToDateTime(ReportsFilters.Date));
                            //rptDocument.SetParameterValue("EndDate", Convert.ToDateTime(ReportsFilters.DateEnd));
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("GroupBySchool", ReportsFilters.IncludeSchool ? "1" : "0");
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_LUNCH_MENU:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("SchoolID", Convert.ToInt32(MsaSchoolID));
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PURCHASE_REPORT:
                        {
                            rptDocument.SetDataSource(ReportDataSet);
                            rptDocument.OpenSubreport("ACPurchaseReport_ProductTemplateValies_v2").SetDataSource(ReportDataSet.Tables["ac_OrderItemInputs"]);

                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            break;
                        }
                    case FSS.Reports.FSS_REPORTS.REP_PROCESSING_REPORT:
                        {
                            ParameterRangeValue dateRange = new ParameterRangeValue();
                            dateRange.StartValue = Convert.ToDateTime(ReportsFilters.fromDate);
                            dateRange.EndValue = Convert.ToDateTime(ReportsFilters.toDate);
                            rptDocument.SetParameterValue("DateRange", dateRange);
                            rptDocument.SetParameterValue("DistrictID", ClientId);
                            rptDocument.SetParameterValue("SortOption", "Order Number");
                            break;
                        }

                    default:
                        break;

                }


                return rptDocument;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetReportDocument");
                return null;
            }

        }

        private string getColumnName(string inputName)
        {
            string outStr = "SchoolName";
            switch (inputName)
            {
                case "school":
                    outStr = "SchoolName";
                    break;
                case "homeroom":
                    outStr = "Homeroom";
                    break;
                case "grade":
                    outStr = "Grade";
                    break;
                case "customername":
                    outStr = "LastName";
                    break;
                case "pin":
                    outStr = "PIN";
                    break;

                default:
                    break;
            }
            return outStr;

        }

        //This function converts IEnumerable table to Datatable
        public static DataTable ToDataTable<T>(List<T> l_oItems, bool isupperSelected)
        {
            try
            {
                DataTable oReturn = new DataTable(typeof(T).Name);
                object[] a_oValues;
                int i;
                PropertyInfo[] a_oProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (PropertyInfo oProperty in a_oProperties)
                {
                    oReturn.Columns.Add(oProperty.Name, BaseType(oProperty.PropertyType));
                }

                foreach (T oItem in l_oItems)
                {
                    a_oValues = new object[a_oProperties.Length];

                    for (i = 0; i < a_oProperties.Length; i++)
                    {
                        a_oValues[i] = a_oProperties[i].GetValue(oItem, null);
                        if (isupperSelected)
                        {
                            if (a_oProperties[i].Name == "FirstName")
                            {
                                a_oValues[i] = Convert.ToString(a_oValues[i]).ToUpper();
                            }
                            if (a_oProperties[i].Name == "Middle")
                            {
                                a_oValues[i] = Convert.ToString(a_oValues[i]).ToUpper();
                            }
                            if (a_oProperties[i].Name == "LastName")
                            {
                                a_oValues[i] = Convert.ToString(a_oValues[i]).ToUpper();
                            }
                        }


                    }

                    oReturn.Rows.Add(a_oValues);
                }

                return oReturn;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ToDataTable");
                return null;
            }
        }

        public static Type BaseType(Type oType)
        {
            if (oType != null && oType.IsValueType &&
                oType.IsGenericType && oType.GetGenericTypeDefinition() == typeof(Nullable<>)
            )
            {
                return Nullable.GetUnderlyingType(oType);
            }
            else
            {
                return oType;
            }
        }
        /// <summary>
        /// This function will return name fromat string
        /// </summary>
        /// <param name="rptFilters"></param>
        /// <returns></returns>
        private string GetNameFormat(ReportsFilters rptFilters)
        {
            string retStr = "{2} + ', '+{0}+' '+{1}";
            if (rptFilters.formatName.Any("Last, First M.".Contains))
            {
                retStr = "{2} + ', '+{0}+' '+{1}";
            }
            else if (rptFilters.formatName.Any("First M. Last".Contains))
            {
                retStr = "{0} + ' '+{1}+' '+{2}";
            }
            return retStr;
        }
        //////////
        public static ReportDocument addNewSortingToReport(ReportDocument rd, string inputSortingList, bool removeGroupSorting)//List<ReportSortingGrouping> sortByParams
        {

            var sortByParamsList = getSortingParams(inputSortingList);
            if (sortByParamsList != null && sortByParamsList.Count > 0)
            {
                ISCDReportClientDocument rcd = rd.ReportClientDocument;
                DataDefController dataDefController = rcd.DataDefController;
                CrRas.Sorts sorts = dataDefController.DataDefinition.Sorts;
                SortController sController = dataDefController.SortController;


                //First of all remove all those 'RecordSortField' sorting that are in the report otherwise there will be conflict
                if (removeGroupSorting)
                {
                    int sIndex = 0;

                    foreach (SortField sField in rd.DataDefinition.SortFields)
                    {
                        if (sField.SortType == SortFieldType.RecordSortField)
                        {
                            CrRas.Sort sort = (CrRas.Sort)sorts[sIndex];
                            sController.Remove(sort);
                            sIndex--;
                        }
                        sIndex++;
                    }
                }
                //Add new sorting
                CrRas.Fields resultFields = dataDefController.DataDefinition.FormulaFields;
                foreach (ReportSortingGrouping item in sortByParamsList)
                {

                    CrRas.Field sortField = (CrRas.Field)resultFields.FindField("{@" + item.FieldName + "}", CrRas.CrFieldDisplayNameTypeEnum.crFieldDisplayNameFormula, CrRas.CeLocale.ceLocaleEnglishUS);

                    SortController sortController = dataDefController.SortController;

                    if (sortController.CanSortOn(sortField))
                    {

                        CrRas.Sort newSort = new CrRas.SortClass();

                        newSort.SortField = sortField;

                        newSort.Direction = item.SortDirection == CrystalDecisions.Shared.SortDirection.AscendingOrder ? CrRas.CrSortDirectionEnum.crSortDirectionAscendingOrder : CrRas.CrSortDirectionEnum.crSortDirectionDescendingOrder;

                        int index = rd.DataDefinition.SortFields.Count;

                        sortController.Add(index, newSort);

                    }

                }

            }

            return rd;

        }

        private ReportDocument addGroupSortingStatment(ReportDocument rd, string inputSortingList)
        {
            List<string> sortinglist = inputSortingList.Split(',').Select(x => x.Trim().ToLower().Replace(" ", "")).ToList();
            int tempCount = 0;
            string sortColName = "SchoolName";
            foreach (var columnname in sortinglist)
            {
                tempCount++;
                sortColName = getColumnName(columnname);
                switch (tempCount)
                {
                    case 1:
                        rd.SetParameterValue("SortedField", sortColName);
                        break;
                    case 2:
                        rd.SetParameterValue("SortedField2", sortColName);
                        break;
                    case 3:
                        rd.SetParameterValue("SortedField3", sortColName);
                        break;
                    case 4:
                        rd.SetParameterValue("SortedField4", sortColName);
                        break;

                    default:
                        break;
                }
            }
            if (tempCount < 5)
            {
                switch (tempCount)
                {
                    case 0:
                        rd.SetParameterValue("SortedField", sortColName);
                        rd.SetParameterValue("SortedField2", sortColName);
                        rd.SetParameterValue("SortedField3", sortColName);
                        rd.SetParameterValue("SortedField4", sortColName);
                        break;
                    case 1:
                        rd.SetParameterValue("SortedField2", sortColName);
                        rd.SetParameterValue("SortedField3", sortColName);
                        rd.SetParameterValue("SortedField4", sortColName);
                        break;
                    case 2:
                        rd.SetParameterValue("SortedField3", sortColName);
                        rd.SetParameterValue("SortedField4", sortColName);

                        break;
                    case 3:
                        rd.SetParameterValue("SortedField4", sortColName);
                        break;
                    case 4:
                        break;

                    default:
                        break;
                }
            }

            return rd;
        }


        public static List<ReportSortingGrouping> getSortingParams(string sortingOrder)
        {

            List<string> sortinglist = sortingOrder.Split(',').Select(x => x.Trim().ToLower().Replace(" ", "")).ToList();
            List<ReportSortingGrouping> sortByParams = new List<ReportSortingGrouping>();
            foreach (var columnname in sortinglist)
            {
                switch (columnname)
                {
                    case "school":
                        sortByParams.Add(new ReportSortingGrouping { Index = 0, FieldName = "SchoolSort", SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder });
                        break;
                    case "homeroom":
                        sortByParams.Add(new ReportSortingGrouping { Index = 0, FieldName = "HomeRoomSort", SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder });
                        break;
                    case "grade":
                        sortByParams.Add(new ReportSortingGrouping { Index = 0, FieldName = "GardeSort", SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder });
                        break;
                    case "customername":
                        sortByParams.Add(new ReportSortingGrouping { Index = 0, FieldName = "CustNameSort", SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder });
                        break;
                    case "pin":
                        sortByParams.Add(new ReportSortingGrouping { Index = 0, FieldName = "PINSort", SortDirection = CrystalDecisions.Shared.SortDirection.AscendingOrder });
                        break;

                    default:
                        break;
                }
            }

            return sortByParams;


        }






        /// <summary>
        /// This function will check if user selected "All Upper Case"
        /// </summary>
        /// <param name="rptFilters"></param>
        /// <returns></returns>
        private bool isUpperSelected(ReportsFilters rptFilters)
        {
            bool isUpper = false;
            if (rptFilters.formatName.Any("All Upper Case".Contains))
            {
                isUpper = true;
            }
            else
            {
                isUpper = false;
            }
            return isUpper;
        }

        private string getOnlyDate(string inputdatetime)
        {
            string outDate = DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
            if (inputdatetime != "")
            {
                DateTime dt = DateTime.ParseExact(inputdatetime, "MM/dd/yyyy HH:mm:ss.fff", CultureInfo.InvariantCulture);
                outDate = dt.ToString("MM/dd/yyyy");
            }
            return outDate;
        }
        /// <summary>
        /// dispose method will be used to dispose object after using that
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    rptDataTable.Dispose();
                    ReportDataSet.Dispose();
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


    public class ReportSortingGrouping
    {
        public int Index { get; set; }
        public string FieldName { get; set; }
        public SortDirection SortDirection { get; set; }
    }

}


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

namespace MSA_AdminPortal.Controllers
{
    public class PreorderPickupReportController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork = null;
        private IPreorderPickupReportGenerator preorderPickupReportGenerator;
        public PreorderPickupReportController()
        {
            preorderPickupReportGenerator = new PreorderPickupReportGenerator();
        }

        [HttpGet]
        public ActionResult ShowReport()
        {
            bool isValid = true;
            string jsonErrorCode = "0";
            ReportsFilters RptFilters = new ReportsFilters();
            if (Request["dataFilterString"] != null)
            {
                RptFilters = new JavaScriptSerializer().Deserialize<ReportsFilters>(Request["dataFilterString"]);
                System.Web.HttpContext.Current.Session["dataFilterString"] = RptFilters;
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["dataFilterString"] != null)
                {
                    RptFilters = (ReportsFilters)System.Web.HttpContext.Current.Session["dataFilterString"];
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
                   rd = preorderPickupReportGenerator.GetReportDocument(reportId, RptFilters);
                   
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupReportsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ShowReport");
            }

            return Json(new { result = jsonErrorCode, err = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RefreshReport()
        {
            //Return shared partial view
            return PartialView("_PreorderPickupReportDisplay");
        }

        public ActionResult PreorderPickupReportView()
        {
            return View();
        }

        public ActionResult GetExcel(string exportFormat, bool firstExe)
        {
            string jsonErrorCode = "0";
            string msg = "";
            string contentType = "application/pdf";
            string reportName = "";
            string[] strReportName = null;

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



                if (System.Web.HttpContext.Current.Session["dataFilterString"] != null)
                {
                    RptFilters = (ReportsFilters)System.Web.HttpContext.Current.Session["dataFilterString"];
                }
                else
                {
                    throw new Exception("Report filters are not found.");
                }

                if (firstExe == false)
                {
                    rd = preorderPickupReportGenerator.GetReportDocument(reportId, RptFilters);
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
                            reportName = "PreorderPickup.xls";
                            contentType = "application/vnd.ms-excel";
                            break;
                        case "PortableDocFormat":
                            exportOption.ExportFormatType = ExportFormatType.PortableDocFormat;
                            reportName = "PreorderPickup.pdf";
                            contentType = "application/pdf";
                            break;
                        case "CharacterSeparatedValues":
                            exportOption.ExportFormatType = ExportFormatType.CharacterSeparatedValues;
                            reportName = "PreorderPickup.csv";
                            contentType = "application/csv";
                            break;
                        case "WordForWindows":
                            exportOption.ExportFormatType = ExportFormatType.WordForWindows;
                            reportName = "PreorderPickup..doc";
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

       

    }
               

    public interface IPreorderPickupReportGenerator : IDisposable
    {
        ReportDocument GetReportDocument(int rptId, ReportsFilters ReportsFilters);
    }

    public class PreorderPickupReportGenerator : IPreorderPickupReportGenerator, IDisposable
    {

        UnitOfWork unitOfWork = null;
        private DataTable rptDataTable;
        private System.Data.DataSet ReportDataSet;
        private bool disposed = false;
        public PreorderPickupReportGenerator()
        {

            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

           public ReportDocument GetReportDocument(int rptId, ReportsFilters ReportsFilters)
        {
            try
            {
                string strRptPath = string.Empty;
                ReportDataSet = new DataSet();
                string preorderStr = ReportsFilters.location;

                var resultSet = unitOfWork.customPreOrderPickupRespository.GetPreOrderPickupReportList(preorderStr).ToList();

                rptDataTable = new DataTable("PreorderPickup");

                rptDataTable = ToDataTable(resultSet, false);
               
                ReportDataSet.Tables.Add(rptDataTable);

                strRptPath = System.Web.HttpContext.Current.Server.MapPath("~/") + "CrystalReports//" + "PreorderPickup.rpt";

                ReportDocument rptDocument = new ReportDocument();
                rptDocument.Load(strRptPath);
               

                if (ReportDataSet != null && ReportDataSet.GetType().ToString() != "System.String")
                {
                  rptDocument.SetDataSource(ReportDataSet);
                                                      
                }
                rptDocument.SetParameterValue("startDate", ReportsFilters.fromDate);
               // string toDate = getOnlyDate(ReportsFilters.toDate);// dt.ToString("MM/dd/yyyy");
                rptDocument.SetParameterValue("endDate", ReportsFilters.toDate);

                return rptDocument;
                
            }
             
            
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupReportController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetReportDocument");
                return null;
            }

        }

           //This function converts IEnumerable table to Datatable
           public static DataTable ToDataTable<T>(List<T> l_oItems, bool isupperSelected)
           {
               try
               {
                   DataTable oReturn = new DataTable("PreorderPickup");
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


}

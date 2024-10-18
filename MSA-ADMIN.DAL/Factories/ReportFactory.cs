using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Factories
{
    public class ReportFactory
    {

        public static DataTable GetDistrict(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetDistricts]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetParentsByDistrict(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetParentsByDistrictId]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetStudentsByDistrict(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetStudentsByDistrictId]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetTransactionsForCcDeposit(long districtId)
          {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetTransactionsForCcDeposit]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetCcDepositReport(long districtId, DateTime startDate, DateTime endDate)
        {
            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);
                dataPortal.AddDateParameter("@StartDate", startDate);
                dataPortal.AddDateParameter("@EndDate", endDate);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetCcDepositReportData]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }

        }
      
        public static DataTable GetOldStudentsByDistrictId(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetOldStudentsByDistrictId]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

       
        public static DataTable GetSchoolsByDistrict(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetSchoolsByDistrictId]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }
        
        public static DataTable GetAch(long districtId)
        {
            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetAchReport]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

       
       
        public static DataTable GetCreditCardHistoryForCcProcessing(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetCreditCardHistoryForCcProcessing]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetMenuForProductionSummary(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetMenuForProductionSummary]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetCategoryForProductionSummary(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetCategoryForProductionSummary]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetPreSaleTransactionForProductionSummary(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetPreSaleTransactionForProductionSummary]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetWebLunchCalendarByDistrict(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetWebLunchCalendarByDistrict]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetWebLunchSchoolsByDistrict(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetWebLunchSchoolsByDistrict]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetCal(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetCal]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetPresaleStudentNoOrders(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetPresaleStudentNoOrders]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetPreSale(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetPreSale]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetPreSaleCompleted(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                DataSet ds = new DataSet();
                dataPortal.FillDataSet("[msa_reports_GetPreSaleCompleted]", DataPortal.QueryType.StoredProc, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataSet GetPurchaseReportData(long districtId)
        {

            DataPortal dataPortal = new DataPortal(connectionStringName: "AbleCommerceConnectionString");
            try
            {
                string query = @"SELECT 
                                    ViewOrderItems.ProductName,
                                    ViewOrderItems.Price, 
                                    ViewOrders.OrderDate,
                                    ViewOrders.LastName, 
                                    ViewOrders.FirstName, 
                                    ViewOrders.OrderNumber, 
                                    ViewOrderItems.Quantity, 
                                    ViewOrders.DistrictID, 
                                    ViewOrderItems.StudentName, 
                                    ViewOrders.DistrictName, 
                                    ViewOrderItems.Discount, 
                                    ViewOrderItems.OrderItemId, 
                                    ViewOrderItems.SchoolName, 
                                    ViewOrderItems.OrderId,
                                    ac_OptionChoices.Name as OptionChoicesName, ac_Options.Name as OptionsName, ac_OptionChoices.PriceModifier, ac_OptionChoices.OptionChoiceId, 
                                    ViewOrderItems.InputValue
                                    FROM   (((AbleCommerce.dbo.ac_OptionChoices ac_OptionChoices INNER JOIN 
                                    AbleCommerce.dbo.VariantLink2OI VariantLink2OI ON ac_OptionChoices.OptionChoiceId=VariantLink2OI.OptionChoiceID) 
                                    INNER JOIN AbleCommerce.dbo.ac_Options ac_Options ON ac_OptionChoices.OptionId=ac_Options.OptionId) 
                                    RIGHT OUTER JOIN AbleCommerce.dbo.ViewOrderItems ViewOrderItems ON VariantLink2OI.OrderItemID=ViewOrderItems.OrderItemId) 
                                    INNER JOIN AbleCommerce.dbo.ViewOrders ViewOrders ON ViewOrderItems.OrderId=ViewOrders.OrderId
                                    where ViewOrders.DistrictID  = " + districtId +
                                        " ORDER BY ViewOrders.DistrictID, ViewOrderItems.SchoolName, ViewOrders.OrderNumber, ViewOrderItems.OrderItemId; select * from ac_OrderItemInputs";


                DataSet ds = new DataSet();
                dataPortal.FillDataSet(query, DataPortal.QueryType.QueryString, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static DataTable GetProcessingReportData(long districtId)
        {

            DataPortal dataPortal = new DataPortal(connectionStringName: "AbleCommerceConnectionString");
            try
            {
                string query = @"SELECT OrderDate,
                                        LastName,
                                        FirstName ,
                                        OrderNumber,
                                        DistrictID,
                                        DistrictName ,
                                        PaymentMethodName,
                                        OrderTotal,
                                        InterchangeFee
                                    FROM  ViewOrders
                                    WHERE DistrictID = " + districtId + @"
                                    ORDER BY DistrictID, OrderNumber;";

                DataSet ds = new DataSet();
                dataPortal.FillDataSet(query, DataPortal.QueryType.QueryString, ds);
                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

    }
}
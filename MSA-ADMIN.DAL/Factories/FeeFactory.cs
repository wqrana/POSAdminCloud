using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MSA_ADMIN.DAL.Common;

namespace MSA_ADMIN.DAL.Factories
{
    public sealed class FeeFactory
    {
        #region Public Function

        public static DataSet ListFeeCatalogDataByDistrict(int pPageIndex, int pPageSize, int pDistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                data.AddIntParameter("@DistrictID", pDistrictID);
                data.FillDataSet("usp_MNU_GetFeeCatalogByDistrictID", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int GetCatalogFeeCountByCatalogId(int CatalogId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                data.AddIntParameter("@arg_CatalogId", CatalogId);
                reader = data.GetDataReader("usp_MNU_GetCatalogFeeCountByCatalogId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetFeeCatalogCountByDistrictID(int DistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                reader = data.GetDataReader("usp_MNU_GetFeeCatalogCountByDistrictID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetFeeCustomGroupCountByDistrictID(int pDistrictID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                data.AddIntParameter("@arg_DistrictID", pDistrictID);
                reader = data.GetDataReader("usp_MNU_GetFeeCustomGroupCountByDistrictID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCategoryTypeCountByDistrictId(int DistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                reader = data.GetDataReader("usp_MNU_GetCategoryTypeCountByDistrictId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCategoryCountByDistrictId(int DistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                reader = data.GetDataReader("usp_MNU_GetCategoryCountByDistrictId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetFeeCountByDistrictId(int DistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                reader = data.GetDataReader("usp_MNU_GetFeeCountByDistrictId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetCategoryByFeeId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string CategoryValue = "";
            try
            {
                data.AddIntParameter("@arg_FeeId", FeeId);
                reader = data.GetDataReader("usp_MNU_GetCategoryByFeeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryValue = reader.GetInt32("Category_Id").ToString();
                }
                return CategoryValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetCategoryTypeByCategoryId(int CategoryId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string CategoryValue = "";
            try
            {
                data.AddIntParameter("@arg_CategoryId", CategoryId);
                reader = data.GetDataReader("usp_MNU_GetCategoryTypeByCategoryId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryValue = reader.GetInt32("CategoryType_Id").ToString();
                }
                return CategoryValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetCategoryNameByCategoryId(int CategoryId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string CategoryValue = "";
            try
            {
                data.AddIntParameter("@arg_CategoryId", CategoryId);
                reader = data.GetDataReader("usp_MNU_GetCategoryNameByCategoryId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryValue = reader.GetString("Name");
                }
                return CategoryValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetFeeNameByFeeId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string FeeName = "";
            try
            {
                data.AddIntParameter("@arg_FeeId", FeeId);
                reader = data.GetDataReader("usp_MNU_GetFeeNameByFeeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    FeeName = reader.GetString("FeeName");
                }
                return FeeName;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetFeeDescriptionByFeeId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string FeeDescription = "";
            try
            {
                data.AddIntParameter("@arg_FeeId", FeeId);
                reader = data.GetDataReader("usp_MNU_GetFeeDescriptionByFeeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    FeeDescription = reader.GetString("FeeDescription");
                }
                return FeeDescription;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetFeeFullPriceByFeeId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string FullPrice = "";
            try
            {
                data.AddIntParameter("@arg_FeeId", FeeId);
                reader = data.GetDataReader("usp_MNU_GetFeeFullPriceByFeeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    FullPrice = reader.GetDecimal("FullPrice").ToString("F2");
                }
                return FullPrice;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetFeeReducedPriceByFeeId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;

            string ReducedPrice = "";
            try
            {
                data.AddIntParameter("@arg_FeeId", FeeId);
                reader = data.GetDataReader("usp_MNU_GetFeeReducedPriceByFeeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    ReducedPrice = reader.GetDecimal("ReducedPrice").ToString("F2");
                }
                return ReducedPrice;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static Boolean GetIsFeeTaxableByFeeId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Boolean Taxable = false;
            try
            {
                data.AddIntParameter("@arg_FeeId", FeeId);
                reader = data.GetDataReader("usp_MNU_GetFeeTaxableByFeeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    Taxable = reader.GetBoolean("FeeTaxable");
                }
                return Taxable;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetCatTypeNameByCatTypeId(int CatTypeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string Name = "";
            try
            {
                data.AddIntParameter("@arg_CatTypeId", CatTypeId);
                reader = data.GetDataReader("usp_MNU_GetCatTypeNameByCatTypeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    Name = reader.GetString("Name");
                }
                return Name;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static Boolean GetIsReducedByCatTypeId(int CatTypeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Boolean IsRed = false;
            try
            {
                data.AddIntParameter("@arg_CatTypeId", CatTypeId);
                reader = data.GetDataReader("usp_MNU_GetCatTypeIsRedByCatTypeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    IsRed = reader.GetBoolean("canReduce");
                }
                return IsRed;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static Boolean GetIsFreeByCatTypeId(int CatTypeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Boolean IsFree = false;
            try
            {
                data.AddIntParameter("@arg_CatTypeId", CatTypeId);
                reader = data.GetDataReader("usp_MNU_GetCatTypeIsFreeByCatTypeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    IsFree = reader.GetBoolean("canFree");
                }
                return IsFree;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static DataSet ListFeeCatalogSetupDataByCategoriesDistrict(int CategoryID, int FeeCatalogID, int pDistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_CategoryID", CategoryID);
                data.AddIntParameter("@arg_FeeCatalogID", FeeCatalogID);
                data.AddIntParameter("@arg_DistrictID", pDistrictID);
                data.FillDataSet("usp_MNU_ListFeeCatalogSetupDataByCategories", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet ListCatalogFeeDetailsByCatalogId(int PageIndex, int PageSize, int FeeCatalogID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_PageIndex", PageIndex);
                data.AddIntParameter("@arg_PageSize", PageSize);
                data.AddIntParameter("@arg_FeeCatalogID", FeeCatalogID);
                data.FillDataSet("usp_MNU_ListFeeCatalogSetupDataByCatalogId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetFeeCategoriesByDistricId(int DistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.FillDataSet("usp_MNU_ListFeeCatagoriesByDistrictId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetCategoryTypeAndIdByDistricId(int DistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.FillDataSet("usp_MNU_GetCategoryTypesAndIdByDistrictId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetCategoryTypeByDistricId(int pageIndex, int PageSize, int DistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.AddIntParameter("@arg_PageIndex", pageIndex);
                data.AddIntParameter("@arg_PageSize", PageSize);
                data.FillDataSet("usp_MNU_ListCategoryTypesByDistrictId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetCategoriesByDistricId(int pageIndex, int PageSize, int DistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.AddIntParameter("@arg_PageIndex", pageIndex);
                data.AddIntParameter("@arg_PageSize", PageSize);
                data.FillDataSet("usp_MNU_ListCatagoriesByDistrictId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetFeesByDistricId(int pageIndex, int PageSize, int DistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.AddIntParameter("@arg_PageIndex", pageIndex);
                data.AddIntParameter("@arg_PageSize", PageSize);
                data.FillDataSet("usp_MNU_ListFeesByDistrictId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetFeesByDistricIdCategoryId(int DistrictID, int CategoryId)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.AddIntParameter("@arg_CategoryID", CategoryId);
                data.FillDataSet("usp_MNU_ListFeesByDistrictIdCategoryId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetCustomGroupsByDistricId(int DistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.FillDataSet("usp_MNU_ListCustomGroupsByDistrictId", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DateTime getEndDateByFeeDetailsId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            DateTime EndDate = System.DateTime.Today;
            try
            {
                string myQuery = "Select EndDate From FeeCatalogDetails Where Id = " + FeeId.ToString();
                reader = data.GetDataReader(myQuery, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    EndDate = reader.GetDateTime("EndDate");
                }
                return EndDate;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static DateTime getStartDateByFeeDetailsId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            DateTime StartDate = System.DateTime.Today;
            try
            {
                string myQuery = "Select StartDate From FeeCatalogDetails Where Id = " + FeeId.ToString();
                reader = data.GetDataReader(myQuery, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    StartDate = reader.GetDateTime("StartDate");
                }
                return StartDate;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetCustomGroupIdByFeeDetailsId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string CustomId = "";
            try
            {
                string myQuery = "Select CustomGroup_Id From FeeCatalogDetails Where Id = " + FeeId.ToString();
                reader = data.GetDataReader(myQuery, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    CustomId = reader.GetInt32("CustomGroup_Id").ToString();
                }
                return CustomId;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetFeeIdByFeeDetailsId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string MenuId = "";
            try
            {
                string myQuery = "Select Menus_Id From FeeCatalogDetails Where Id = " + FeeId.ToString();
                reader = data.GetDataReader(myQuery, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    MenuId = reader.GetInt32("Menus_Id").ToString();
                }
                return MenuId;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetCategoryIdByFeeDetailsId(int FeeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string CategoryValue = "";
            try
            {
                string myQuery = "Select EasyPayCategory_Id from EasyPayItems EPI inner join FeeCatalogDetails fcd on fcd.Menus_Id = EPI.Id Where fcd.Id = " + FeeId.ToString();
                reader = data.GetDataReader(myQuery, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    CategoryValue = reader.GetInt32("EasyPayCategory_Id").ToString();
                }
                return CategoryValue;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static string GetCatalogNameByCatalogId(int CatalogId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            string CatalogName = "";
            try
            {
                string myQuery = "Select CatalogName from FeeCatalog Where Id = " + CatalogId.ToString();
                reader = data.GetDataReader(myQuery, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    CatalogName = reader.GetString("CatalogName");
                }
                return CatalogName;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int AddCatalog(int CatalogType, string CatalogName, int DistrictID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CatalogId", true);
                if (CatalogType != 0)
                    data.AddIntParameter("@arg_CatalogType", CatalogType);
                if (CatalogName != "")
                    data.AddStringParameter("@arg_CatalogName", CatalogName);
                data.AddIntParameter("@arg_DistrictID", DistrictID);

                data.SubmitData("usp_MNU_AddCatalog", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_CatalogId");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static int AddCustomGroup(string GroupName, int DistrictID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_GroupId", true);
                data.AddStringParameter("@arg_GroupName", GroupName);
                data.AddIntParameter("@arg_DistrictID", DistrictID);

                data.SubmitData("usp_MNU_AddCustomGroup", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_GroupId");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static int AddFeeCatalogDetails(DateTime StartDate, DateTime EndDate, int FeeId, int CatalogId, int CustomGroupId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Exists", true);
                data.AddDateParameter("@arg_StartDate", StartDate);
                data.AddDateParameter("@arg_EndDate", EndDate);
                data.AddIntParameter("@arg_FeeID", FeeId);
                data.AddIntParameter("@arg_CatalogID", CatalogId);
                if (CustomGroupId != 0)
                    data.AddIntParameter("@arg_GroupID", CustomGroupId);

                data.SubmitData("usp_MNU_AddFeeCatalogDetail", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Exists");
                
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddFeeToMenu(int CategoryId, string FeeName, string FeeDescription, string FullPrice, string RedPrice, Boolean Taxable, int DistrictId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Category_Id", CategoryId);
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                data.AddStringParameter("@arg_ItemName", FeeName);
                data.AddDecimalParameter("@arg_FullPrice", Convert.ToDecimal(FullPrice));
                if (FeeDescription != "")
                    data.AddStringParameter("@arg_AltDescription", FeeDescription);
                if (RedPrice != "")
                    data.AddDecimalParameter("@arg_RedPrice", Convert.ToDecimal(RedPrice));
                data.AddBoolParameter("@arg_Taxable", Taxable);

                data.SubmitData("usp_MNU_AddFeeToMenu", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddCategoryType(string CatTypeName, Boolean Reduced, Boolean Free, int DistrictId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                data.AddStringParameter("@arg_Name", CatTypeName);
                data.AddBoolParameter("@arg_CanRed", Reduced);
                data.AddBoolParameter("@arg_CanFree", Free);

                data.SubmitData("usp_MNU_AddCategoryType", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddCategory(int CatTypeId, string CategoryName, int DistrictId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                data.AddStringParameter("@arg_Name", CategoryName);
                data.AddIntParameter("@arg_CategoryTypeId", CatTypeId);

                data.SubmitData("usp_MNU_AddCategory", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddFeeCatalogSchool(int CatalogID, int DistrictID, int School_ID)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (CatalogID != 0)
                    data.AddIntParameter("@arg_FeeCatalogID", CatalogID);
                if (DistrictID != 0)
                    data.AddIntParameter("@arg_DistrictID", DistrictID);
                if (School_ID != 0)
                    data.AddIntParameter("@arg_SchoolID", School_ID);

                data.SubmitData("usp_MNU_AddFeeCatalogSchool", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddCustomGroupDisplayOrder(int DisplayOrder, int CustomGroupID, int DistrictID, int CatalogID)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (CatalogID != 0)
                    data.AddIntParameter("@arg_FeeCatalogID", CatalogID);
                if (DistrictID != 0)
                    data.AddIntParameter("@arg_DistrictID", DistrictID);
                if (CustomGroupID != 0)
                    data.AddIntParameter("@arg_CustomGroupID", CustomGroupID);
                if (DisplayOrder != 0)
                    data.AddIntParameter("@arg_DisplayOrder", DisplayOrder);

                data.SubmitData("usp_MNU_EditGroupDisplayOrder", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static int UpdateFeeCatalogDetails(DateTime StartDate, DateTime EndDate, int FeeId, int CustomGroupId, int FeeDetailId, int CatalogId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Exists", true);
                data.AddIntParameter("@arg_FeeDetailId", FeeDetailId);
                data.AddDateParameter("@arg_StartDate", StartDate);
                data.AddDateParameter("@arg_EndDate", EndDate);
                data.AddIntParameter("@arg_FeeId", FeeId);
                data.AddIntParameter("@arg_CatalogID", CatalogId);
                if (CustomGroupId !=0 )
                    data.AddIntParameter("@arg_CustomGroupId", CustomGroupId);

                data.SubmitData("usp_MNU_UpdateFeeCatalogDetail", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Exists");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void UpdateFeeOnMenu(int CategoryId, string FeeName, string FeeDescription, string FullPrice, string RedPrice, Boolean Taxable, int FeeId, int DistrictId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Category_Id", CategoryId);
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                data.AddIntParameter("@arg_FeeId", FeeId);
                data.AddStringParameter("@arg_ItemName", FeeName);
                data.AddDecimalParameter("@arg_FullPrice", Convert.ToDecimal(FullPrice));
                if (FeeDescription != "")
                    data.AddStringParameter("@arg_AltDescription", FeeDescription);
                if (RedPrice != "")
                    data.AddDecimalParameter("@arg_RedPrice", Convert.ToDecimal(RedPrice));
                data.AddBoolParameter("@arg_Taxable", Taxable);
                data.AddDateParameter("@arg_UpdateDate", DateTime.Today);

                data.SubmitData("usp_MNU_UpdateFeeOnMenu", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void UpdateCategoryType(string CatTypeName, Boolean Reduce, Boolean Free, int CatTypeId, int DistrictId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CatTypeId", CatTypeId);
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                data.AddStringParameter("@arg_Name", CatTypeName);
                data.AddBoolParameter("@arg_CanRed", Reduce);
                data.AddBoolParameter("@arg_CanFree", Free);

                data.SubmitData("usp_MNU_UpdateCategoryType", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void UpdateCategory(int CatTypeId, string CategoryName, int CategoryId, int DistrictId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CategoryTypeId", CatTypeId);
                data.AddIntParameter("@arg_DistrictId", DistrictId);
                data.AddIntParameter("@arg_CategoryId", CategoryId);
                data.AddStringParameter("@arg_Name", CategoryName);

                data.SubmitData("usp_MNU_UpdateCategory", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetFeeCatalogSchoolCountByFeeCatalogID(int CatalogID, int pSchoolId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;

                reader = data.GetDataReader("select count(*) as Count from FeeCatalogSchools where FeeCatalog_Id = " + CatalogID + " and School_Id = " + pSchoolId + "", DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("Count");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelCustomOrderingByCatalogId(int CatalogID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CatalogId", CatalogID);
                data.SubmitData("usp_MNU_DelCustomOrderingByCatalogId", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelFeeCatalogDataByCatalogId(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_FeeCatalgId", Id);
                data.SubmitData("usp_MNU_DelFeeCatalogDetailByCatalogId", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelFeeCatalogDetailData(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_FeeCatalgDetailId", Id);
                data.SubmitData("usp_MNU_DelFeeCatalogDetailById", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelFeeCatalog(int CatalogId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CatalogID", CatalogId);
                data.SubmitData("usp_MNU_DelFeeCatalogById", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelFeeSchoolsByCatalogID(int CatalogId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CatalogID", CatalogId);
                data.SubmitData("usp_MNU_DelFeeCatalogSchoolByCatalogId", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelFeeCatalogSchool(int SchoolID, int WebCallID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_FeeCatalogID", WebCallID);
                data.AddIntParameter("@arg_SchoolID", SchoolID);
                data.SubmitData("usp_MNU_DelFeeCatalogSchools", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DeleteByRequestType(int DeleteType, int ID, int DistrictId)
        {
            DataPortal data = new DataPortal();

            data.AddIntParameter("@arg_DistrictId", DistrictId);

            // 1 = Category Type, 2 = Category, 3 = Item
            if (DeleteType == 1)
            {
                data.AddIntParameter("@arg_CatTypeId", ID);
                data.GetDataReader("usp_MNU_DelCatTypeById", DataPortal.QueryType.StoredProc);

            }
            else if (DeleteType == 2)
            {
                data.AddIntParameter("@arg_CategoryId", ID);
                data.GetDataReader("usp_MNU_DelCategoryById", DataPortal.QueryType.StoredProc);
            }
            else if (DeleteType == 3)
            {
                data.AddIntParameter("@arg_FeeId", ID);
                data.GetDataReader("usp_MNU_DelFeeOnMenuById", DataPortal.QueryType.StoredProc);
            }

        }

        public static DataSet GetFeeCategories(int DistrictID)
        {
            DataSet ret = new DataSet();
            DataPortal data = new DataPortal();

            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.FillDataSet("usp_MNU_GetFeeCatalogCategories", DataPortal.QueryType.StoredProc, ret);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetFeeGroupNames(int DistrictID)
        {
            DataSet ret = new DataSet();
            DataPortal data = new DataPortal();

            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.FillDataSet("usp_MNU_GetFeeCustomGroupsAndIDByDistrictID", DataPortal.QueryType.StoredProc, ret);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static IList<Order> GetOrders(int CatalogID, int DistrictID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            IList<FeeFactory.Order> results = new List<Order>();
            try
            {
                data.AddIntParameter("@arg_DistrictID", DistrictID);
                data.AddIntParameter("@arg_CatalogId", CatalogID);
                reader = data.GetDataReader("usp_MNU_getCatalogGroupDisplayOrder", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    int displayOrder = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("DisplayOrder")));
                    int customGroupId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CustomGroup_Id")));
                    //int feeCustomOrderId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id")));
                    string groupName = (!reader.IsDBNull(reader.GetOrdinal("GroupName")))
                                             ? (string)reader.GetValue(reader.GetOrdinal("GroupName"))
                                             : string.Empty;
                    //results.Add(new Order(displayOrder, customGroupId, groupName, feeCustomOrderId));
                    results.Add(new Order(displayOrder, customGroupId, groupName));
                }
                return results;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion //End public function region

        #region Nested type: Order

        public class Order
        {
            private string _groupName;
            private int _displayOrder;
            private int _customGroupId;

            public Order(int displayOrder, int customGroupId, string groupName)
            {
                _displayOrder = displayOrder;
                _customGroupId = customGroupId;
                _groupName = groupName;
            }

            public int DisplayOrder
            {
                get { return _displayOrder; }
            }

            public int CustomGroupId
            {
                get { return _customGroupId; }
            }

            public string GroupName
            {
                get { return _groupName; }
            }
        }

        #endregion
    }
}

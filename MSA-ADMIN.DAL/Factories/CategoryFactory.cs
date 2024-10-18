using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using MSA_ADMIN.DAL.Models;
using MSA_ADMIN.DAL.Common;
//using FSSAdmin.Data;
//using Common;

namespace MSA_ADMIN.DAL.Factories
{
    public class CategoryFactory
    {
        #region Static Function

        public static Collection<CategoryData> ListCategory(int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<CategoryData> cdlist = new Collection<CategoryData>();
            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetCategory", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryData cd = PopulateCategoryDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist;
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

        public static Collection<CategoryData> ListCategoryByKeyword(string pKeyword, int pPageIndex, int pPageSize, int pDistrict)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<CategoryData> cdlist = new Collection<CategoryData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@arg_DistrictID", pDistrict);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetCategoriesByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryData cd = PopulateCategoryDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist;
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

        public static Collection<CategoryData> GetCategoryByCategoryID(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<CategoryData> cdlist = new Collection<CategoryData>();
            try
            {
                data.AddIntParameter("@arg_CategoryID", Id);
                reader = data.GetDataReader("usp_MNU_GetCategoryByCategoryID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    CategoryData cd = PopulateCategoryDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist;
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

        public static Collection<CategoryData> GetQualifiedCategory()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<CategoryData> cdlist = new Collection<CategoryData>();
            try
            {
                reader = data.GetDataReader("usp_MNU_GetQualifiedCategory", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryData cd = PopulateCategoryDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist;
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

        public static DataSet GetCategoryCount(string Name)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select count(*) from Category where name='" + Name + "'", DataPortal.QueryType.QueryString, ds);
                data.AddStringParameter("@Name", Name);
                data.FillDataSet("usp_MENU_GetCategoryCount", DataPortal.QueryType.StoredProc, ds);
                return ds;
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

        public static DataSet getCategoryExistCount(int Id)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select count(*) from Menu where Category_Id='" + Id + "'", DataPortal.QueryType.QueryString, ds);
                data.AddIntParameter("@Id", Id);
                data.FillDataSet("usp_MENU_getCategoryExistCount", DataPortal.QueryType.StoredProc, ds);
                return ds;
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
        public static int GetCategoriesCountByKeyword(string pKeyword, int pDistrict)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@arg_DistrictID", pDistrict);
                reader = data.GetDataReader("usp_MNU_GetCategoriesCountByKeyword", DataPortal.QueryType.StoredProc);
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

        public static int AddCategory(int CategoryType_Id, string Name, bool isActive, bool isDeleted, int Color, string AccountNumber)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (CategoryType_Id != 0)
                    data.AddIntParameter("@arg_CategoryType_Id", CategoryType_Id);
                if (Name != "")
                    data.AddStringParameter("@arg_Name", Name);
                //if (isActive != false)
                data.AddBoolParameter("@arg_isActive", isActive);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (Color != 0)
                    data.AddIntParameter("@arg_Color", Color);
                if (AccountNumber != "")
                    data.AddStringParameter("@arg_AccountNumber", AccountNumber);



                data.SubmitData("usp_MNU_AddCategory", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
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

        public static void ChgCategory(int Id, int CategoryType_Id, string Name, bool isActive, bool isDeleted, int Color, string AccountNumber)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (CategoryType_Id != 0)
                    data.AddIntParameter("@arg_CategoryType_Id", CategoryType_Id);
                if (Name != "")
                    data.AddStringParameter("@arg_Name", Name);
                //if (isActive != false)
                data.AddBoolParameter("@arg_isActive", isActive);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (Color != 0)
                    data.AddIntParameter("@arg_Color", Color);
                if (AccountNumber != "")
                    data.AddStringParameter("@arg_AccountNumber", AccountNumber);

                data.SubmitData("usp_MNU_ChgCategory", DataPortal.QueryType.StoredProc);


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

        public static void DelCategory(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_MNU_DelCategory", DataPortal.QueryType.StoredProc);
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

        public static CategoryData PopulateCategoryDataFromReader(SafeDataReader reader)
        {
            CategoryData cd = new CategoryData();

            cd.CategoryID = reader.GetInt32("Id");
            //cd.CategoryType = new NameValuePair(reader.GetString("CategoryTypeName"), reader.GetInt32("CategoryType_Id"));
            NameValuePair[] nvpcon = new NameValuePair[3];
            nvpcon[0].Value = reader.GetInt32("CategoryType_Id");
            nvpcon[0].Name = reader.GetString("CategoryTypeName").ToString().Trim();
            nvpcon[1].Value = reader.GetInt32("CategoryType_Id");
            nvpcon[1].Name = reader.GetBoolean("canFree").ToString();
            nvpcon[2].Value = reader.GetInt32("CategoryType_Id");
            nvpcon[2].Name = reader.GetBoolean("canReduce").ToString();
            cd.CategoryType = new NameValuePairCollection(nvpcon);
            cd.Name = reader.GetString("Name").Trim();
            cd.isActive = reader.GetBoolean("isActive");
            cd.isDeleted = reader.GetBoolean("isDeleted");
            cd.Color = reader.GetInt32("Color");
            cd.AccountNumber = reader.GetString("AccountNumber").Trim();
            return cd;
        }

        #endregion
    }
}

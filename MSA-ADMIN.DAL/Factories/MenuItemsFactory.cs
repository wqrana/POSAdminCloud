using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using MSA_ADMIN.DAL.Models;
using MSA_ADMIN.DAL.Common;

namespace MSA_ADMIN.DAL.Factories
{
    public class MenuItemsFactory
    {
        #region Static Function

        public static Collection<MenuItemData> ListMenu(int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<MenuItemData> mdlist = new Collection<MenuItemData>();
            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetMenu", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    MenuData md = MenuFactory.PopulateMenuDataFromReader(reader);
                    mdlist.Add(new MenuItemData(md));
                }
                return mdlist;
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

        public static Collection<MenuItemData> ListMenuByKeyword(string pKeyword, int pPageIndex, int pPageSize, int pDistrict)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<MenuItemData> mdlist = new Collection<MenuItemData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@arg_DistrictID", pDistrict);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetMenuItemsByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    MenuData md = MenuFactory.PopulateMenuDataFromReader(reader);
                    mdlist.Add(new MenuItemData(md));
                }
                return mdlist;
            }
            catch (Exception Ex)
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

        public static DataSet GetMenuItemsCount(string Name)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select count(*) from Menu where ItemName='" + Name + "'", DataPortal.QueryType.QueryString, ds);
                data.AddStringParameter("@Name", Name);
                data.FillDataSet("usp_MENU_GetMenuItemsCount", DataPortal.QueryType.StoredProc, ds);
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

        public static DataSet GetWebLunchMenuItemsCount(string Name)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {

                //data.FillDataSet("select count(*) from WebLunchMenu where Abbreviation='" + Name + "'", DataPortal.QueryType.QueryString, ds);
                data.AddStringParameter("@Name", Name);
                data.FillDataSet("usp_MENU_GetWebLunchMenuItemsCount", DataPortal.QueryType.StoredProc, ds);

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

        public static DataSet GetWebLunchMenuScheduleDateCount(SmartDate Sdate, int Smenuid)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select id,date,menus_id from Cal where date='" + Sdate + "' and menus_id=" + Smenuid, DataPortal.QueryType.QueryString, ds);
                data.AddIntParameter("@Smenuid", Smenuid);
                data.AddDateParameter("@Sdate", Sdate);
                data.FillDataSet("usp_MENU_GetWebLunchMenuScheduleDateCount", DataPortal.QueryType.StoredProc, ds);
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

        public static DataSet getMenuItemExistCount(int Id)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.FillDataSet("select count(*) from Items where Menu_Id='" + Id + "'", DataPortal.QueryType.QueryString, ds);
                //data.AddIntParameter("@Id", Id);
                //data.FillDataSet("usp_MENU_getMenuItemExistCount", DataPortal.QueryType.StoredProc, ds);
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

        public static int GetMenuItemsCountByKeyword(string pKeyword, int pDistrict)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                else
                    data.AddStringParameter("@arg_Keyword", "%");
                data.AddIntParameter("@arg_DistrictID", pDistrict);
                reader = data.GetDataReader("usp_MNU_GetMenuItemsCountByKeyword", DataPortal.QueryType.StoredProc);
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

        public static int GetCountForAvailableMenuItemsByCategoryID(string pKeyword, int pCategoryID, int pDistrict)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@arg_CategoryID", pCategoryID);
                data.AddIntParameter("@arg_DistrictID", pDistrict);
                reader = data.GetDataReader("usp_MNU_GetCountForAvailableMenuItemsByCategoryID", DataPortal.QueryType.StoredProc);
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
        
        public static Collection<MenuItemData> GetMenuByMenuID(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<MenuItemData> mdlist = new Collection<MenuItemData>();
            try
            {
                data.AddIntParameter("@arg_MenuID", Id);
                reader = data.GetDataReader("usp_MNU_GetMenuByMenuID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    MenuData md = MenuFactory.PopulateMenuDataFromReader(reader);
                    mdlist.Add(new MenuItemData(md));
                }
                return mdlist;
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

        public static Collection<MenuItemData> getAvailableItemsByCategoryId(int CateId, string pKeyword, int pPageIndex, int pPageSize, int pDistrict)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<MenuItemData> mdlist = new Collection<MenuItemData>();
            try
            {
                data.AddIntParameter("@arg_CategoryID", CateId);
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@arg_DistrictID", pDistrict);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetAvailableMenuItemsByCategoryID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    MenuData md = MenuFactory.PopulateMenuDataFromReader(reader);
                    mdlist.Add(new MenuItemData(md));
                }
                return mdlist;
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

        public static int AddMenu(int Category_Id, string ItemName, string M_F6_Code, decimal StudentFullPrice, decimal StudentRedPrice, decimal EmployeePrice, decimal GuestPrice, bool isTaxable, bool isDeleted, string AltDescription)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Category_Id != 0)
                    data.AddIntParameter("@arg_Category_Id", Category_Id);
                if (ItemName != "")
                    data.AddStringParameter("@arg_ItemName", ItemName);
                if (M_F6_Code != "")
                    data.AddStringParameter("@arg_M_F6_Code", M_F6_Code);
                if (StudentFullPrice != 0)
                    data.AddDecimalParameter("@arg_StudentFullPrice", StudentFullPrice);
                if (StudentRedPrice != 0)
                    data.AddDecimalParameter("@arg_StudentRedPrice", StudentRedPrice);
                if (EmployeePrice != 0)
                    data.AddDecimalParameter("@arg_EmployeePrice", EmployeePrice);
                if (GuestPrice != 0)
                    data.AddDecimalParameter("@arg_GuestPrice", GuestPrice);
                //if (isTaxable != false)
                data.AddBoolParameter("@arg_isTaxable", isTaxable);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (AltDescription != "")
                    data.AddStringParameter("@arg_AltDescription", AltDescription);

                data.SubmitData("usp_MNU_AddMenu", DataPortal.QueryType.StoredProc);
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

        public static void ChgMenu(int Id, int Category_Id, string ItemName, string M_F6_Code, decimal StudentFullPrice, decimal StudentRedPrice, decimal EmployeePrice, decimal GuestPrice, bool isTaxable, bool isDeleted, string AltDescription)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Category_Id != 0)
                    data.AddIntParameter("@arg_Category_Id", Category_Id);
                if (ItemName != "")
                    data.AddStringParameter("@arg_ItemName", ItemName);
                if (M_F6_Code != "")
                    data.AddStringParameter("@arg_M_F6_Code", M_F6_Code);
                if (StudentFullPrice != 0)
                    data.AddDecimalParameter("@arg_StudentFullPrice", StudentFullPrice);
                if (StudentRedPrice != 0)
                    data.AddDecimalParameter("@arg_StudentRedPrice", StudentRedPrice);
                if (EmployeePrice != 0)
                    data.AddDecimalParameter("@arg_EmployeePrice", EmployeePrice);
                if (GuestPrice != 0)
                    data.AddDecimalParameter("@arg_GuestPrice", GuestPrice);
                //if (isTaxable != false)
                data.AddBoolParameter("@arg_isTaxable", isTaxable);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (AltDescription != "")
                    data.AddStringParameter("@arg_AltDescription", AltDescription);

                data.SubmitData("usp_MNU_ChgMenu", DataPortal.QueryType.StoredProc);


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

        public static void DelMenu(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_MNU_DelMenu", DataPortal.QueryType.StoredProc);
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

        #endregion
    }
}

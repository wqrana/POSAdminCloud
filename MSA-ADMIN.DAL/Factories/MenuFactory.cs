using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using Common;
using System.Data.SqlClient;
using System.Configuration;
using MSA_ADMIN.DAL.Models;
using MSA_ADMIN.DAL.Common;
using System.Collections.ObjectModel;

namespace MSA_ADMIN.DAL.Factories
{
    public sealed class MenuFactory
    {
        static SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MSAAdminConnectionString"].ToString());
        SqlCommand cm;

        #region Private Function

        private static CategoryTypesData PopulateCategoryTypeDataFromReader(SafeDataReader reader)
        {
            CategoryTypesData ctd = new CategoryTypesData();
            ctd.CategoryTypesID = reader.GetInt32("Id");
            ctd.Name = reader.GetString("Name").Trim();
            ctd.canFree = reader.GetBoolean("canFree");
            ctd.canReduce = reader.GetBoolean("canReduce");
            ctd.isDeleted = reader.GetBoolean("isDeleted");
            //dd.BankMICR = reader.geto
            return ctd;
        }

        public static MenuData PopulateMenuDataFromReader(SafeDataReader reader)
        {
            MenuData md = new MenuData();

            md.MenuID = reader.GetInt32("Id");
            md.Category = new NameValuePair(reader.GetString("CategoryName").Trim(), reader.GetInt32("Category_Id"));
            md.ItemName = reader.GetString("ItemName").Trim();
            md.M_F6_Code = reader.GetString("M_F6_Code").Trim();
            md.StudentFullPrice = reader.GetDecimal("StudentFullPrice");
            md.StudentRedPrice = reader.GetDecimal("StudentRedPrice");
            md.EmployeePrice = reader.GetDecimal("EmployeePrice");
            md.GuestPrice = reader.GetDecimal("GuestPrice");
            md.isTaxable = reader.GetBoolean("isTaxable");
            md.isDeleted = reader.GetBoolean("isDeleted");
            md.AltDescription = reader.GetString("AltDescription");

            //dd.BankMICR = reader.geto
            return md;
        }
        #endregion

        #region Public Function

        #region CategoryType Functions

        public static CategoryTypesData[] ListCategoryTypes(int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CategoryTypesData> ctdlist = new List<CategoryTypesData>();
            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetCategoryType", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryTypesData ctd = PopulateCategoryTypeDataFromReader(reader);
                    ctdlist.Add(ctd);
                }
                return ctdlist.ToArray();
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

        public static CategoryTypesData[] ListCategoryTypesByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CategoryTypesData> ctdlist = new List<CategoryTypesData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetCategoryTypesByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CategoryTypesData ctd = PopulateCategoryTypeDataFromReader(reader);
                    ctdlist.Add(ctd);
                }
                return ctdlist.ToArray();
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

        public static CategoryTypesData[] GetCategoryTypesByCategoryTypesID(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CategoryTypesData> ctdlist = new List<CategoryTypesData>();
            try
            {
                data.AddIntParameter("@arg_CategoryTypesID", Id);
                reader = data.GetDataReader("usp_MNU_GetCategoryTypesByCategoryTypesID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    CategoryTypesData ctd = PopulateCategoryTypeDataFromReader(reader);
                    ctdlist.Add(ctd);
                }
                return ctdlist.ToArray();
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

        public static DataSet GetCategoryTypeCount(string Name)//tested
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select count(*) from CategoryTypes where name='" + Name + "'", DataPortal.QueryType.QueryString, ds);
                data.AddStringParameter("@Name", Name);
                data.FillDataSet("usp_MENU_getCategoryExistCountName", DataPortal.QueryType.StoredProc, ds);

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
        public static DataSet getCategoryTypeExistCount(int Id)//tested
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select count(*) from Category where CategoryType_Id='" + Id + "'", DataPortal.QueryType.QueryString, ds);
                data.AddIntParameter("@Id", Id);
                data.FillDataSet("usp_MENU_getCategoryTypeExistCount", DataPortal.QueryType.StoredProc, ds);

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

        #region Del,Add,Chg

        public static int AddCategoryType(string Name, bool canFree, bool canReduce, bool isDeleted)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Name != "")
                    data.AddStringParameter("@arg_Name", Name);
                //if (canFree != false)
                data.AddBoolParameter("@arg_canFree", canFree);
                //if (canReduce != false)
                data.AddBoolParameter("@arg_canReduce", canReduce);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);


                data.SubmitData("usp_MNU_AddCategoryType", DataPortal.QueryType.StoredProc);
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

        public static void ChgCategoryType(int Id, string Name, bool canFree, bool canReduce, bool isDeleted)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Name != "")
                    data.AddStringParameter("@arg_Name", Name);
                //if (canFree != false)
                data.AddBoolParameter("@arg_canFree", canFree);
                //if (canReduce != false)
                data.AddBoolParameter("@arg_canReduce", canReduce);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);

                data.SubmitData("usp_MNU_ChgCategoryType", DataPortal.QueryType.StoredProc);


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

        public static void DelCategoryType(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_MNU_DelCategoryType", DataPortal.QueryType.StoredProc);
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

        #endregion

        #region Count

        public static int GetCategoryTypesCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_MNU_GetCategoryTypesCountByKeyword", DataPortal.QueryType.StoredProc);
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

        #endregion

        #endregion


        #region WebLunchMenuData
        #region Private Function
        private static WebLunchMenuData PopulateWebLunchMenuDataFromReader(SafeDataReader reader)
        {
            WebLunchMenuData wm = new WebLunchMenuData();

            wm.Id = reader.GetInt32("Id");
            wm.District = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("DistrictID"));
            //wm.DistrictID = reader.GetInt32("DistrictID");
            wm.Abbreviation = reader.GetString("Abbreviation");
            wm.Description = reader.GetString("Description");
            wm.Calories = reader.GetString("Calories");
            wm.Price1 = reader.GetDecimal("Price1");
            wm.Price2 = reader.GetDecimal("Price2");
            wm.Price3 = reader.GetDecimal("Price3");
            wm.ReducedPrice = reader.GetDecimal("ReducedPrice");
            wm.QualifiedMeal = reader.GetBoolean("QualifiedMeal");
            wm.ALaCarteSelection = reader.GetBoolean("ALaCarteSelection");
            wm.FoodServItemNumber = reader.GetString("FoodServItemNumber");

            return wm;

        }
        #endregion

        #region Public Function
        public static WebLunchMenuData[] ListWebLunchMenu()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<WebLunchMenuData> wmlist = new List<WebLunchMenuData>();
            try
            {
                reader = data.GetDataReader("usp_MNU_GetWebLunchMenu", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    WebLunchMenuData wm = PopulateWebLunchMenuDataFromReader(reader);
                    wmlist.Add(wm);
                }
                return wmlist.ToArray();
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



        public static WebLunchMenuData[] GetWebLunchMenuByWebLunchID(int pId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<WebLunchMenuData> wdlist = new List<WebLunchMenuData>();
            try
            {
                data.AddIntParameter("@arg_Id", pId);
                reader = data.GetDataReader("usp_MNU_GetWebLunchMenuByWebLunchID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    WebLunchMenuData md = PopulateWebLunchMenuDataFromReader(reader);
                    wdlist.Add(md);
                }
                return wdlist.ToArray();
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


        public static int GetWebLunchMenuCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_MNU_GetWebLunchMenuItemsCountByKeyword", DataPortal.QueryType.StoredProc);
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

        public static WebLunchMenuData[] ListWebLunchMenuByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<WebLunchMenuData> wdlist = new List<WebLunchMenuData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_MNU_GetWebLunchMenuItemsByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    WebLunchMenuData wd = PopulateWebLunchMenuDataFromReader(reader);
                    wdlist.Add(wd);
                }
                return wdlist.ToArray();
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

        public static int AddWebLunchMenu(int pDistrictID, string pAbbreviation, string pDescription, string pCalories, decimal pPrice1, decimal pPrice2, decimal pPrice3, decimal pReducedPrice, Boolean pQualifiedMeal, Boolean pALaCarteSelection, string pFoodServItemNumber)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (pDistrictID != 0)
                    data.AddIntParameter("@arg_DistrictID", pDistrictID);
                if (pAbbreviation != "")
                    data.AddStringParameter("@arg_Abbreviation", pAbbreviation);
                if (pDescription != "")
                    data.AddStringParameter("@arg_Description", pDescription);
                if (pCalories != "")
                    data.AddStringParameter("@arg_Calories ", pCalories);
                if (pPrice1 != 0)
                    data.AddDecimalParameter("@arg_Price1", pPrice1);
                if (pPrice2 != 0)
                    data.AddDecimalParameter("@arg_Price2", pPrice2);
                if (pPrice3 != 0)
                    data.AddDecimalParameter("@arg_Price3", pPrice3);
                if (pReducedPrice != 0)
                    data.AddDecimalParameter("@arg_ReducedPrice", pReducedPrice);
                // if (pQualifiedMeal != false)
                data.AddBoolParameter("@arg_QualifiedMeal", pQualifiedMeal);
                // if (pALaCarteSelection != false)
                data.AddBoolParameter("@arg_ALaCarteSelection", pALaCarteSelection);
                if (pFoodServItemNumber != "")
                    data.AddStringParameter("@arg_FoodServItemNumber", pFoodServItemNumber);

                data.SubmitData("usp_MNU_AddWebLunchMenu", DataPortal.QueryType.StoredProc);
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

        public static void ChgWebLunchMenu(int pId, int pDistrictID, string pAbbreviation, string pDescription, string pCalories, decimal pPrice1, decimal pPrice2, decimal pPrice3, decimal pReducedPrice, Boolean pQualifiedMeal, Boolean pALaCarteSelection, string pFoodServItemNumber)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (pId != 0)
                    data.AddIntParameter("@arg_Id", pId);
                if (pDistrictID != 0)
                    data.AddIntParameter("@arg_DistrictID", pDistrictID);
                if (pAbbreviation != "")
                    data.AddStringParameter("@arg_Abbreviation", pAbbreviation);
                if (pDescription != "")
                    data.AddStringParameter("@arg_Description", pDescription);
                if (pCalories != "")
                    data.AddStringParameter("@arg_Calories ", pCalories);
                if (pPrice1 != 0)
                    data.AddDecimalParameter("@arg_Price1", pPrice1);
                if (pPrice2 != 0)
                    data.AddDecimalParameter("@arg_Price2", pPrice2);
                if (pPrice3 != 0)
                    data.AddDecimalParameter("@arg_Price3", pPrice3);
                if (pReducedPrice != 0)
                    data.AddDecimalParameter("@arg_ReducedPrice", pReducedPrice);
                //  if (pQualifiedMeal != )
                data.AddBoolParameter("@arg_QualifiedMeal", pQualifiedMeal);
                // if (pALaCarteSelection != false)
                data.AddBoolParameter("@arg_ALaCarteSelection", pALaCarteSelection);

                if (pFoodServItemNumber != "")
                    data.AddStringParameter("@arg_FoodServItemNumber", pFoodServItemNumber);

                data.SubmitData("usp_MNU_ChgWebLunchMenu", DataPortal.QueryType.StoredProc);

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


        public static void DelWebLunchMenu(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_MNU_DelWebLunchMenu", DataPortal.QueryType.StoredProc);
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
        #endregion


        #region WebLunchScheduleData
        #region Public Function

        // ---------------------------------WebSchdule Dataset----------------------//

        //public static int AddWebLunchSchedule(SmartDate pdate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, bool ppaid, int pshoworder, DateTime pCutoffDate)
        //{

        //    DataPortal data = new DataPortal();
        //    try
        //    {
        //        data.AddIntParameter("@arg_id", true);
        //        if (pdate != "")
        //            data.AddDateParameter("@arg_date", pdate);
        //        if (pmeal != 0)
        //            data.AddIntParameter("@arg_meal", pmeal);
        //        if (pmenus_id != 0)
        //            data.AddIntParameter("@arg_menus_id", pmenus_id);
        //        if (pQuantity != 0)
        //            data.AddIntParameter("@arg_Quantity", pQuantity);
        //        if (pWebcalid != 0)
        //            data.AddIntParameter("@arg_WebCalID", pWebcalid);
        //        data.AddBoolParameter("@arg_Status", pStatus);

        //        data.AddBoolParameter("@arg_Paid", ppaid);
        //        data.AddIntParameter("@arg_showorder", pshoworder);

        //        data.AddDateParameter("@arg_CutoffDate", pCutoffDate);

        //        data.SubmitData("usp_MNU_AddWebLunchSchedule", DataPortal.QueryType.StoredProc);
        //        return (int)data.GetParameterValue("@arg_id");
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}

        //No need because CutOffDate column is not present

        //public static void UpdateWebLunchScheduleOverriCutOff(SmartDate pDate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, int pshoworder, DateTime pCutoffDate, int CutOffType, int CutOffValue)
        //{
        //    DataPortal data = new DataPortal();
        //    try
        //    {
        //        //Set the command text as name of the stored procedure

        //        string query = "update Cal set CutOffDate='" + pCutoffDate + "',OverrideCutOffType=" + CutOffType + ",OverrideCutOffValue=" + CutOffValue + "  where webcalid=" + pWebcalid + " and date='" + pDate + "' and meal=" + pmeal + " and menus_id=" + pmenus_id + " and Quantity=" + pQuantity + " and Status='" + pStatus + "' ";

        //        data.SubmitData(query, DataPortal.QueryType.QueryString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (data != null)
        //            data.Dispose();
        //    }

        //}

        #endregion
        #endregion


        #region WebLunchCalendar
        #region Private Function
        private static WebLunchCalendarData PopulateWebLunchCalendarDataFromReader(SafeDataReader reader)
        {
            WebLunchCalendarData wc = new WebLunchCalendarData();

            wc.WebCalID = reader.GetInt32("WebCalID");
            wc.CalendarType = reader.GetInt32("CalendarType");
            wc.CalendarName = reader.GetString("CalendarName");
            return wc;

        }
        #endregion

        #region Public Function

        //public static WebLunchCalendarData[] ListWebLunchCalendarData()
        //{
        //    DataPortal data = new DataPortal();
        //    SafeDataReader reader = null;
        //    List<WebLunchCalendarData> wclist = new List<WebLunchCalendarData>();
        //    try
        //    {
        //        reader = data.GetDataReader("usp_MNU_GetWebLunchCalendar", DataPortal.QueryType.StoredProc);
        //        while (reader.Read())
        //        {
        //            WebLunchCalendarData wc = PopulateWebLunchCalendarDataFromReader(reader);
        //            wclist.Add(wc);
        //        }
        //        return wclist.ToArray();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}


        public static DataSet ListWebLunchCalendarDataByKeywordwithDistrict(string pKeyword, int pPageIndex, int pPageSize, int pDistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                data.AddIntParameter("@DistrictID", pDistrictID);
                data.FillDataSet("usp_MNU_GetWebLunchCalendarByKeywordwithDistrict", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet ListWebLunchCalendarDataByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                // data.AddIntParameter("@DistrictID", pDistrictID);
                data.FillDataSet("usp_MNU_GetWebLunchCalendarByKeyword", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static int GetWebLunchCalanderCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_MNU_GetWebLunchCalendarCountByKeyword", DataPortal.QueryType.StoredProc);
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

        public static int GetWebLunchCalanderCountByKeywordandDistrictID(string pKeyword, int pDistrictID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@arg_DistrictID", pDistrictID);
                reader = data.GetDataReader("usp_MNU_GetWebLunchCalendarCountByKeywordandDistrictID", DataPortal.QueryType.StoredProc);
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

        public static int AddWebLunchCalendarData(int pCalendarType, string pCalendarName, int pDistrictID, int pCutoffday)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_WebCalID", true);
                if (pCalendarType != 0)
                    data.AddIntParameter("@arg_CalendarType", pCalendarType);
                if (pCalendarName != "")
                    data.AddStringParameter("@arg_CalendarName", pCalendarName);
                data.AddIntParameter("@arg_DistrictID", pDistrictID);

                if (pCutoffday != 0)
                    data.AddIntParameter("@arg_CalCutOffDays", pCutoffday);

                data.SubmitData("usp_MNU_AddWebLunchCalendar", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_WebCalID");
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

        public static void ChgWebLunchCalendarData(int pWebCalID, int pCalendarType, string pCalendarName)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (pWebCalID != 0)
                    data.AddIntParameter("@arg_WebCalID", pWebCalID);
                if (pCalendarType != 0)
                    data.AddIntParameter("@arg_CalendarType", pCalendarType);
                if (pCalendarName != "")
                    data.AddStringParameter("@arg_CalendarName", pCalendarName);


                data.SubmitData("usp_MNU_ChgWebLunchCalendar", DataPortal.QueryType.StoredProc);

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


        public static void DelWebLunchCalendarData(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_WebCalID", Id);
                data.SubmitData("usp_MNU_DelWebLunchCalendar", DataPortal.QueryType.StoredProc);
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

        public static DataSet GetWebLunchCalendarItemsCount(string Name, string districtId)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                int district_id = 0;
                int.TryParse(districtId, out district_id);
                //data.FillDataSet("select count(*) from WebLunchCalendar where CalendarName='" + Name + "'", DataPortal.QueryType.QueryString, ds);
                data.AddStringParameter("@Name", Name);
                data.AddIntParameter("@districtId", district_id);
                data.FillDataSet("usp_MENU_GetWebLunchCalendarItemsCount", DataPortal.QueryType.StoredProc, ds);
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

        public static bool CalendarNameExists(string Name, string districtId, bool editmode=false)
        {
            bool retvalue = false;
            DataSet dsWeCalName = new DataSet();
            dsWeCalName = MenuFactory.GetWebLunchCalendarItemsCount(Name, districtId);
            if (dsWeCalName.Tables.Count > 0)
            {
                Int32 tempCount = Convert.ToInt32(Convert.ToString(dsWeCalName.Tables[0].Rows[0]["Column1"]));
                if ((editmode == true && tempCount == 1) || (editmode == false && tempCount == 0))
                {
                    retvalue = false;
                }
                else
                {
                    if (editmode == false && tempCount > 0)
                    {
                        retvalue = true;
                    }
                
                }
            }
            return retvalue;

        }



        public static DataSet GetWebLunchCalendarDetails(int pWebCalID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select * from WebLunchCalendar where WebCalID=" + pWebCalID, DataPortal.QueryType.QueryString, ds);
                data.AddIntParameter("@pWebCalID", pWebCalID);
                data.FillDataSet("usp_MENU_GetWebLunchCalendarDetails", DataPortal.QueryType.StoredProc, ds);

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
        public static DataSet GetAssignedMenuItemsByWebCalID(int pWebCalId)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {

                data.AddIntParameter("@arg_WebCalID", pWebCalId);

                data.FillDataSet("usp_ADM_GetAssignedMenuItemsByWebCalID", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #endregion


        #region WebLunchSchools
        #region Private Function

        private static WebLunchSchoolsData PopulateWebLunchSchoolsData(SafeDataReader reader)
        {
            WebLunchSchoolsData wsd = new WebLunchSchoolsData();
            wsd.ID = reader.GetInt32("Id");
            NameValuePair[] nvpcon = new NameValuePair[2];
            nvpcon[0].Value = reader.GetInt32("WebCalID");
            nvpcon[0].Name = reader.GetString("CalendarName");
            nvpcon[1].Value = reader.GetInt32("WebCalID");
            nvpcon[1].Name = reader.GetInt32("CalendarType").ToString();

            wsd.WebCal = new NameValuePairCollection(nvpcon);
            wsd.District = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("DistrictID"));
            wsd.School = new NameValuePair(reader.GetString("SchoolName"), reader.GetInt32("School_ID"));

            return wsd;
        }
        #endregion

        #region Public Function

        public static WebLunchSchoolsData[] GetWebLunchSchoolsByWebCalID(int pWebcalId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<WebLunchSchoolsData> wcallist = new List<WebLunchSchoolsData>();
            try
            {
                data.AddIntParameter("@arg_WebCalID", pWebcalId);
                reader = data.GetDataReader("usp_MNU_GetWebLunchSchoolsByWebCalID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    WebLunchSchoolsData wsd = PopulateWebLunchSchoolsData(reader);
                    wcallist.Add(wsd);
                }
                return wcallist.ToArray();
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

        public static int AddWebLunchSchool(int pWebCalID, int pDistrictID, int pSchool_ID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_ID", true);
                if (pWebCalID != 0)
                    data.AddIntParameter("@arg_WebCalID", pWebCalID);
                if (pDistrictID != 0)
                    data.AddIntParameter("@arg_DistrictID", pDistrictID);
                if (pSchool_ID != 0)
                    data.AddIntParameter("@arg_School_ID", pSchool_ID);


                data.SubmitData("usp_MNU_AddWebLunchSchool", DataPortal.QueryType.StoredProc);
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

        public static void ChgWebLunchSchool(int pWebCalID, int pDistrictId, int pSchool_ID)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (pWebCalID != 0)
                    data.AddIntParameter("@arg_WebCalID", pWebCalID);
                if (pDistrictId != 0)
                    data.AddIntParameter("@arg_DistrictID", pDistrictId);
                if (pSchool_ID != 0)
                    data.AddIntParameter("@arg_School_ID", pSchool_ID);


                data.SubmitData("usp_MNU_ChgWebLunchSchool", DataPortal.QueryType.StoredProc);


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

        public static void DelWebLunchSchool(int SchoolID, int WebCallID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_WebCalID", WebCallID);
                data.AddIntParameter("@arg_School_ID", SchoolID);
                data.SubmitData("usp_MNU_DelWebLunchSchools", DataPortal.QueryType.StoredProc);
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

        public static void DelWebLunchSchoolsBYWebCallID(int WebCallID, int DistrictID)
        {
            DataPortal data = new DataPortal();
            try
            {

                //string query = "delete from WebLunchSchools where WebCalID=" + WebCallID + " and DistrictID=" + DistrictID + "";
                //data.SubmitData(query, DataPortal.QueryType.QueryString);


                data.AddIntParameter("@WebCallID", WebCallID);
                data.AddIntParameter("@DistrictID", DistrictID);
                data.SubmitData("usp_MENU_DelWebLunchSchoolsBYWebCallID", DataPortal.QueryType.StoredProc);



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

        public static int GetWebLunchSchoolCountByWebCalID(int pWebcalId, int pSchoolId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;

                //reader = data.GetDataReader("select count(*) as Count from WebLunchSchools where webcalid=" + pWebcalId + " and school_Id=" + pSchoolId + "", DataPortal.QueryType.QueryString);
                data.AddIntParameter("@pWebcalId", pWebcalId);
                data.AddIntParameter("@pSchoolId", pSchoolId);

                reader = data.GetDataReader("usp_MENU_GetWebLunchSchoolCountByWebCalID", DataPortal.QueryType.StoredProc);
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

        #endregion

        #endregion
    }
}

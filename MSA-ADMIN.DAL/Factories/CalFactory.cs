using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using MSA_ADMIN.DAL.Models;
using MSA_ADMIN.DAL.Common;
using System.Collections.Generic;
using AdminPortalModels.ViewModels;
//using FSSAdmin.Data;
//using Common;

namespace MSA_ADMIN.DAL.Factories
{
    public class CalFactory
    {
        #region Static Function
        public static Collection<CalData> GetWebLunchScheduleByWebcalID(int pWebcalId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<CalData> cdlist = new Collection<CalData>();
            try
            {
                data.AddIntParameter("@arg_WebCalID", pWebcalId);

                reader = data.GetDataReader("usp_MNU_GetWebLunchScheduleByWebcalID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CalData cd = PopulateCalDataFromReader(reader);
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

        public static Collection<CalEvent> GetWebLunchEventsByWebcalID(int pWebcalId, DateTime StartDate, DateTime EndDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<CalEvent> itemslist = new Collection<CalEvent>();
            try
            {
                data.AddIntParameter("@arg_WebCalID", pWebcalId);
                data.AddDateParameter("@arg_StartDate", StartDate);
                data.AddDateParameter("@arg_EndDate", EndDate);


                reader = data.GetDataReader("pos_GetWebLunchScheduleByWebcalID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CalEvent ce = PopulateCalEvenDataFromReader(reader);
                    itemslist.Add(ce);
                }
                return itemslist;
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


        // ---------------------------------WebSchdule Dataset----------------------//
        public static DataSet GetWebLunchScheduleByWebcalIDDS(int pWebcalId)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select id as Id, description as Description, startdate as StartDate,enddate as EndDate from WebSchduleTest", DataPortal.QueryType.QueryString, ds);
                data.FillDataSet("usp_MENU_GetWebLunchScheduleByWebcalIDDS", DataPortal.QueryType.StoredProc, ds);
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

        public static int AddWebLunchSchedule(DateTime pdate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, bool ppaid, int pshoworder)
        {

            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", true);
                if (pdate != null)
                    data.AddDateParameter("@arg_date", pdate);
                if (pmeal != 0)
                    data.AddIntParameter("@arg_meal", pmeal);
                if (pmenus_id != 0)
                    data.AddIntParameter("@arg_menus_id", pmenus_id);
                if (pQuantity != 0)
                    data.AddIntParameter("@arg_Quantity", pQuantity);
                if (pWebcalid != 0)
                    data.AddIntParameter("@arg_WebCalID", pWebcalid);
                data.AddBoolParameter("@arg_Status", pStatus);

                data.AddBoolParameter("@arg_Paid", ppaid);
                data.AddIntParameter("@arg_showorder", pshoworder);

                // data.AddDateParameter("@arg_CutoffDate", pCutoffDate);

                //data.SubmitData("usp_MNU_AddWebLunchSchedule", DataPortal.QueryType.StoredProc);
                data.SubmitData("pos_AddCalItem", DataPortal.QueryType.StoredProc);

                return (int)data.GetParameterValue("@arg_id");
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
        //public static int AddWebLunchSchedule(SmartDate pdate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, bool ppaid, int pshoworder, DateTime pCutoffDate)
        //{
        //    int pid = MenuFactory.AddWebLunchSchedule(pdate, pmeal, pmenus_id, pQuantity, pStatus, pWebcalid, ppaid, pshoworder, pCutoffDate);
        //    return pid;
        //}

        public static void ChgWebLunchSchedule(int pid, SmartDate pdate, int pmeal, int pmenus_id, int pschools_id, int pseq, Boolean pdef)
        {

            DataPortal data = new DataPortal();
            try
            {
                if (pid != 0)
                    data.AddIntParameter("@arg_id", pid);
                if (pdate != "")
                    data.AddDateParameter("@arg_date", pdate);
                if (pmeal != 0)
                    data.AddIntParameter("@arg_meal", pmeal);
                if (pmenus_id != 0)
                    data.AddIntParameter("@arg_menus_id", pmenus_id);
                if (pschools_id != 0)
                    data.AddIntParameter("@arg_schools_id ", pschools_id);
                if (pseq != 0)
                    data.AddIntParameter("@arg_seq", pseq);
                if (pdef != false)
                    data.AddBoolParameter("@arg_def", pdef);

                data.SubmitData("usp_MNU_ChgWebLunchSehedule", DataPortal.QueryType.StoredProc);

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

        public static void DelWebLunchSchedule(int pWebCalid)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_WebCalID", pWebCalid);
                data.SubmitData("usp_MNU_DelWebLunchSchedule", DataPortal.QueryType.StoredProc);
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

        public static void DelWebLunchMenuItemsInCal(int pWebCalid, int pMenuID, DateTime PDate)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_WebCalID", pWebCalid);
                data.AddDateParameter("@arg_Date", PDate);
                data.AddIntParameter("@arg_MenuId", pMenuID);
                data.SubmitData("usp_MNU_DelWebLunchMenuItemsInCal", DataPortal.QueryType.StoredProc);
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



        public static void DelWebLunchScheduleByWebcalID(int pWebcalid)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_WebCalID", pWebcalid);
                data.SubmitData("usp_MNU_DelWebLunchScheduleByWebcalID", DataPortal.QueryType.StoredProc);
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

        public static int GetWebLunchMenuCount(int pMenuId, int pWebCalId, DateTime pDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;

                //reader = data.GetDataReader("select count(*) as Count from cal where menus_id=" + pMenuId + " and WebCalID=" + pWebCalId + " and date='" + pDate + "'", DataPortal.QueryType.QueryString);
                data.AddIntParameter("@pMenuId", pMenuId);
                data.AddIntParameter("@pWebCalId", pWebCalId);
                data.AddDateParameter("@pDate", pDate);
                reader = data.GetDataReader("usp_MENU_GetWebLunchMenuCountReader", DataPortal.QueryType.StoredProc);
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

        public static void UpdateCalByOrderStatus(int pCalID, int pShowOrder, DateTime pDate)
        {
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure
                data.AddIntParameter("@pShowOrder", pShowOrder);
                data.AddIntParameter("@pCalID", pCalID);
                data.AddDateParameter("@pDate", pDate);

                data.SubmitData("usp_MNU_UpdateCalByOrderStatus", DataPortal.QueryType.StoredProc);

                //string query = "update Cal set showorder=" + pShowOrder + " where webcalid=" + pCalID + " and date='" + pDate + "'";
                //data.SubmitData(query, DataPortal.QueryType.QueryString);
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

        public static void UpdateWebLunchSchedule(SmartDate pDate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, int pshoworder, DateTime pCutoffDate)
        {// this method is not being used anymore.
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure

                string query = "update Cal set CutOffDate='" + pCutoffDate + "' where webcalid=" + pWebcalid + " and date='" + pDate + "' and meal=" + pmeal + " and menus_id=" + pmenus_id + " and Quantity=" + pQuantity + " and Status='" + pStatus + "' and ShowOrder=" + pshoworder;

                data.SubmitData(query, DataPortal.QueryType.QueryString);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        //public static void UpdateWebLunchScheduleOverriCutOff(SmartDate pdate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, int pshoworder, DateTime pCutoffDate, int CutOffType, int CutOffValue)
        //{
        //    MenuFactory.UpdateWebLunchScheduleOverriCutOff(pdate, pmeal, pmenus_id, pQuantity, pStatus, pWebcalid, pshoworder, pCutoffDate, CutOffType, CutOffValue);
        //}

        public static void UpdateWebLunchScheduleOverriCutOff(SmartDate pDate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, int pshoworder, int CutOffType, int CutOffValue)
        {
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure

                //string query = "update Cal set OverrideCutOffType=" + CutOffType + ",OverrideCutOffValue=" + CutOffValue + "  where webcalid=" + pWebcalid + " and date='" + pDate + "' and meal=" + pmeal + " and menus_id=" + pmenus_id + " and Quantity=" + pQuantity + " and Status='" + pStatus + "' ";
                //data.SubmitData(query, DataPortal.QueryType.QueryString);

                data.AddIntParameter("@CutOffType", CutOffType);
                data.AddIntParameter("@CutOffValue", CutOffValue);
                data.AddIntParameter("@pWebcalid", pWebcalid);
                data.AddIntParameter("@pmeal", pmeal);
                data.AddIntParameter("@pmenus_id", pmenus_id);
                data.AddIntParameter("@pQuantity", pQuantity);
                data.AddBoolParameter("@pStatus", pStatus);

                data.AddDateParameter("@pDate", pDate);
                data.SubmitData("usp_MENU_UpdateWebLunchScheduleOverriCutOff", DataPortal.QueryType.StoredProc);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        // this method is not being used anymore.
        public static void UpdateWebLunchScheduleWithUserValues(SmartDate pDate, int pmeal, int pmenus_id, int pQuantity, Boolean pStatus, int pWebcalid, int pshoworder, DateTime pCutoffDate, int CutOffType, int CutOffValue, int cutoffselection)
        {
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure

                string query = "update Cal set CutOffSelection=" + cutoffselection + ",CutOffDate='" + pCutoffDate + "',CutOffType=" + CutOffType + ",CutOffValue=" + CutOffValue + "  where webcalid=" + pWebcalid + " and date='" + pDate + "' and meal=" + pmeal + " and menus_id=" + pmenus_id + " and Quantity=" + pQuantity + " and Status='" + pStatus + "'";

                data.SubmitData(query, DataPortal.QueryType.QueryString);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void UpdateWebLunchCalendarForCutOffValue(int webCalId, int CutOffType, int CutOffValue, int cutoffselection)
        {
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure

                //string query = "Update WebLunchCalendar Set CutOffValue = '" + CutOffValue + "', CutOffType = '" + CutOffType + "', CutOffSelection = '" + cutoffselection + "' Where WebCalId = " + webCalId;
                //data.SubmitData(query, DataPortal.QueryType.QueryString);

                data.AddIntParameter("@webCalId", webCalId);
                data.AddIntParameter("@CutOffType", CutOffType);
                data.AddIntParameter("@CutOffValue", CutOffValue);
                data.AddIntParameter("@cutoffselection", cutoffselection);


                data.SubmitData("usp_MENU_UpdateWebLunchCalendarForCutOffValue", DataPortal.QueryType.StoredProc);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void UpdateWebLunchCalendarForSameDayOrder(int webCalId, int OrderingOption, string SameDayOrderTime)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@webCalId", webCalId);
                data.AddIntParameter("@OrderingOption", OrderingOption);
                data.AddTimeParameter("@SameDayOrderTime", SameDayOrderTime);


                data.SubmitData("usp_MENU_UpdateWebLunchCalendarForSameDayOrdering", DataPortal.QueryType.StoredProc);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void UpdateWebLunchCalendarOrderingOption(int webCalId, int OrderingOption)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@webCalId", webCalId);
                data.AddIntParameter("@OrderingOption", OrderingOption);


                data.SubmitData("usp_MENU_UpdateOrderingOption", DataPortal.QueryType.StoredProc);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        //Get the records by webcalID and Date
        public static DataSet GetCalByWebcalIDandDate(int pWebcalId, DateTime pDate)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select * from cal where webcalid=" + pWebcalId + "and date='" + pDate + "'", DataPortal.QueryType.QueryString, ds);

                data.AddIntParameter("@pWebcalId", pWebcalId);
                data.AddDateParameter("@pDate", pDate);

                data.FillDataSet("usp_MENU_GetCalByWebcalIDandDate", DataPortal.QueryType.StoredProc, ds);
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

        //Get the records from cal by  Date
        public static DataSet GetCalByDate(DateTime pDate, string pWebcalID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("Declare @WebCalCutoff int set @WebCalCutoff = (Select CutOffValue from WebLunchCalendar where WebCalId = " + pWebcalID + ") select *,@WebCalCutoff as Cutoffval from cal inner join menu on cal.menus_id = menu.id where date='" + pDate + "' and webcalid= '" + pWebcalID + "' order by userOrder asc", DataPortal.QueryType.QueryString, ds);
                data.AddIntParameter("@pWebcalID", Convert.ToInt32(pWebcalID));
                data.AddDateParameter("@pDate", pDate);
                data.FillDataSet("usp_MENU_GetCalByDate", DataPortal.QueryType.StoredProc, ds);

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

        //Get The records from cal table 
        //Get the records by webcalID and Date and menuid
        public static DataSet GetWebLunchMenuShowOrder(int pMenuId, int pWebcalId, DateTime pDate)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select * from cal where menus_id =" + pMenuId + " and webcalid=" + pWebcalId + " and date='" + pDate + "'", DataPortal.QueryType.QueryString, ds);

                data.AddIntParameter("@pMenuId", pMenuId);
                data.AddIntParameter("@pWebcalId", pWebcalId);
                data.AddDateParameter("@pDate", pDate);
                data.FillDataSet("usp_MENU_GetWebLunchMenuShowOrder", DataPortal.QueryType.StoredProc, ds);

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

        public static DataSet getCategories(int DistrictID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select * from Category where DistrictID=" + DistrictID, DataPortal.QueryType.QueryString, ds);
                data.AddIntParameter("@DistrictID", DistrictID);
                data.FillDataSet("usp_MENU_getCategories", DataPortal.QueryType.StoredProc, ds);

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

        public static void RemoveItemByMenuID(string MenuID, string date, int WebCalID)
        {
            DataPortal data = new DataPortal();
            try
            {

                //string query = "delete from Cal where menus_id=" + MenuID + " and date='" + date + "'";

                data.AddIntParameter("@MenuID", Convert.ToInt32(MenuID));
                data.AddDateParameter("@pDate", Convert.ToDateTime(date));
                data.AddIntParameter("@WebCalID", WebCalID);

                data.SubmitData("usp_MENU_RemoveItemByMenuID", DataPortal.QueryType.StoredProc);
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

        public static void UpdateUserOrderByMenuId(int userOrder, int Id)
        {
            DataPortal data = new DataPortal();
            try
            {

                //string query = "update Cal set  userOrder=" + userOrder + " where id=" + Id + "";
                //data.SubmitData(query, DataPortal.QueryType.QueryString);

                data.AddIntParameter("@userOrder", userOrder);
                data.AddIntParameter("@Id", Id);
                data.SubmitData("usp_MENU_UpdateUserOrderByMenuId", DataPortal.QueryType.StoredProc);

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

        public static DataSet GetWebCalDetails(int webCallId, string selectedDate)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();

            try
            {
                // data.FillDataSet("select count(*) from Menu where ItemName='" + Name + "'", DataPortal.QueryType.QueryString, ds);
                //data.FillDataSet("select * from Cal where WebCalID=" + webCallId + " and date='" + DateTime.Parse(selectedDate) + "'", DataPortal.QueryType.QueryString, ds);

                data.AddDateParameter("@selectedDate", DateTime.Parse(selectedDate));
                data.AddIntParameter("@webCallId", webCallId);

                data.FillDataSet("usp_MENU_GetWebCalDetails", DataPortal.QueryType.StoredProc, ds);

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

        public static DataSet GetWebCalDetailsOnScheduler(int webCallId)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                //data.FillDataSet("select * from Cal where date between '" + StartDate + "' and '" + EndDate + "' and WebCalID=" + webCallId + " and OverrideCutOffType >= 0", DataPortal.QueryType.QueryString, ds);
                //data.FillDataSet("select * from WebLunchCalendar where WebCalID=" + webCallId + " and CutOffType >= 0", DataPortal.QueryType.QueryString, ds);
                data.AddIntParameter("@webCallId", webCallId);
                data.FillDataSet("usp_MENU_GetWebCalDetailsOnScheduler", DataPortal.QueryType.StoredProc, ds);

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

        public static CalData PopulateCalDataFromReader(SafeDataReader reader)
        {
            CalData cl = new CalData();

            cl.id = reader.GetInt32("id");
            cl.date = reader.GetSmartDate("date");
            cl.meal = reader.GetInt32("meal");
            cl.menus_id = reader.GetInt32("menus_id");
            cl.MenuName = reader.GetString("MenuName");
            cl.ShowOrder = reader.GetInt32("ShowOrder");
            cl.Webcall = new NameValuePair(reader.GetString("CalendarName"), reader.GetInt32("WebCalID"));
            cl.AltDescription = reader.GetString("AltDescription");
            cl.CutOffDate = reader.GetSmartDate("CutOffDate");
            cl.WebLunchCutOffValue = Convert.ToString(reader.GetInt32("WebLunchCutOffValue"));
            cl.OverrideCutOffValue = Convert.ToString(reader.GetInt32("OverrideCutOffValue"));
            cl.OverrideStartDate = reader.GetSmartDate("OverrideStartDate");
            cl.CutOffDateStr = reader.GetString("CutOffDateStr");

            return cl;
        }

        public static CalEvent PopulateCalEvenDataFromReader(SafeDataReader reader)
        {
            CalEvent cl = new CalEvent();

            cl.id = reader.GetInt32("id");
            cl.title = reader.GetString("MenuName");
            cl.start = reader.GetSmartDate("date").ToString("yyy-MM-dd");
            cl.end = reader.GetSmartDate("date").ToString("yyy-MM-dd");
            cl.color = "#d0ecbb";
            cl.showOrder = reader.GetInt32("ShowOrder");

            cl.menus_id = reader.GetInt32("menus_id");

            cl.WebLunchCutOffValue = Convert.ToString(reader.GetInt32("WebLunchCutOffValue"));
            cl.OverrideCutOffValue = Convert.ToString(reader.GetInt32("OverrideCutOffValue"));
            cl.OverrideStartDate = reader.GetSmartDate("OverrideStartDate").ToString("yyy-MM-dd");
            cl.CutOffDateStr = reader.GetString("CutOffDateStr");

            cl.date = reader.GetSmartDate("date").ToString("yyy-MM-dd");

            //cl.meal = reader.GetInt32("meal");


            cl.WebCalID = reader.GetInt32("WebCalID");
            cl.AltDescription = reader.GetString("AltDescription");
            //cl.CutOffDate = reader.GetSmartDate("CutOffDate");
            cl.textColor = "#121212";
            cl.userOrder = reader.GetInt32("userOrder");
            cl.StudentFullPrice = reader.GetDecimal("StudentFullPrice");

            return cl;
        }
        public static IList<WeblunchCalendar> GetPreorderCalendarList(Int64 disstrictId)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            IList<WeblunchCalendar> retValue = null;
            try
            {
                //data.FillDataSet("select * from cal where webcalid=" + pWebcalId + "and date='" + pDate + "'", DataPortal.QueryType.QueryString, ds);

                data.AddLongParameter("@DistrictID", disstrictId);
                data.FillDataSet("pos_getPreorderCalendarList", DataPortal.QueryType.StoredProc, ds);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    retValue = new List<WeblunchCalendar>();
                    if (dt.Rows.Count > 0)
                    {
                       
                        foreach (DataRow row in dt.Rows)
                        {
                            WeblunchCalendar wc = new WeblunchCalendar();
                            wc.WebCalID = Convert.ToInt32(row["WebCalID"].ToString());
                            wc.CalendarName = row["CalendarName"].ToString();
                            wc.CalendarType = row["CalendarType"].ToString();
                            wc.fullSchoolsList = row["AssignedSchools"].ToString();
                            wc.DistrictID = Convert.ToInt32(row["DistrictID"].ToString());
                            List<PreorderSchool> list = new List<PreorderSchool>();
                            if (wc.fullSchoolsList != "")
                            {
                                string[] sList = wc.fullSchoolsList.Split(':');
                                if (sList.Length > 0)
                                {

                                    for (int i = 0; i < sList.Length; i++)
                                    {
                                        PreorderSchool ps = new PreorderSchool();
                                        string[] innerList = sList[i].Split(',');
                                        if (innerList.Length == 3)
                                        {
                                            ps.schoolID = innerList[0];
                                            ps.schoolName = innerList[1];
                                            ps.selectedstring = innerList[2];
                                            ps.isSelected = innerList[2].Trim().ToLower() == "selected";
                                        }

                                        list.Add(ps);
                                    }
                                    list.Sort((x, y) => x.schoolName.CompareTo(y.schoolName));
                                }
                            }
                            wc.AssignedSchoolsList = list;
                            retValue.Add(wc);

                        }
                    }
                }
                return retValue;
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
        public static WeblunchCalendar GetPreorderCalendar(int WebCalID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            WeblunchCalendar retValue = null;
            try
            {
                data.AddIntParameter("@WebCalID", WebCalID);
                data.FillDataSet("pos_getPreorderCalendar", DataPortal.QueryType.StoredProc, ds);
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        retValue = new WeblunchCalendar();
                        WeblunchCalendar wc = new WeblunchCalendar();
                        wc.WebCalID = Convert.ToInt32(dt.Rows[0]["WebCalID"].ToString());
                        wc.CalendarName = dt.Rows[0]["CalendarName"].ToString();
                        wc.CalendarType = dt.Rows[0]["CalendarType"].ToString();
                        wc.DistrictID = Convert.ToInt32(dt.Rows[0]["DistrictID"].ToString());
                        wc.CalendarTypeName = dt.Rows[0]["CalendarTypeName"].ToString();

                        wc.CutOffValue = Convert.ToInt32(dt.Rows[0]["CutOffValue"].ToString());
                        wc.CutOffType = Convert.ToInt32(dt.Rows[0]["CutOffType"].ToString());
                        wc.CutOffSelection = Convert.ToInt32(dt.Rows[0]["CutOffSelection"].ToString());
                        wc.SameDayOrderTime = dt.Rows[0]["SameDayOrderTime"].ToString();
                        wc.OrderingOption = Convert.ToInt32(dt.Rows[0]["OrderingOption"].ToString());
                        retValue = wc;
                    }
                }
                return retValue;
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



        public static void DeleteCalendar(int pWebCalid, Int64 pDistrictID)
        {

            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@WebCalID", pWebCalid);
                data.AddLongParameter("@DistrictID", pDistrictID);
                data.SubmitData("Pos_DeleteCalendar", DataPortal.QueryType.StoredProc);
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


        public static int SaveAssignedSchools(int WebCalID, Int64 districtID, string schoolsList)
        {
            int retValue = 0;

            DataTable dt = null;

            DataPortal data = new DataPortal();

            if (!string.IsNullOrEmpty(schoolsList))
            {

                string[] schoolsListArray = schoolsList.Split(',');
                if (schoolsListArray.Length > 0)
                {

                    dt = new DataTable("CalAssignedSchoolsList");

                    dt.Columns.Add("WebCalID", typeof(int));
                    dt.Columns.Add("DistrictID", typeof(Int64));
                    dt.Columns.Add("School_ID", typeof(int));

                    for (int i = 0; i < schoolsListArray.Length; i++)
                    {

                        DataRow row = dt.NewRow();
                        row["WebCalID"] = WebCalID;
                        row["DistrictID"] = districtID;
                        row["School_ID"] = schoolsListArray[i];
                        dt.Rows.Add(row);
                    }

                }

                try
                {
                    data.AddIntParameter("@arg_WebCalID", WebCalID);
                    data.AddLongParameter("@arg_DistrictID", districtID);
                    data.AddAssignedSchsDataTableParameter("CalAssignedSchoolsList", dt);
                    data.SubmitData("pos_UpdateCalAssignedSchools", DataPortal.QueryType.StoredProc);
                    retValue = 1;
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
            else
            {
                try
                {
                    data.AddIntParameter("@arg_WebCalID", WebCalID);
                    data.AddLongParameter("@arg_DistrictID", districtID);
                    data.SubmitData("pos_DeleteAllAssignedSchools", DataPortal.QueryType.StoredProc);
                    retValue = 1;
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
            return retValue;
        }

        public static int SavePreorderItemsForSelectedDates(int WebCalID, DateTime sourceDate, string dateList)
        {
            int retValue = 0;

            DataTable dt = null;

            DataPortal data = new DataPortal();

            if (!string.IsNullOrEmpty(dateList))
            {

                string[] dateListArray = dateList.Split(',');
                if (dateListArray.Length > 0)
                {

                    dt = new DataTable("SelectedCalDates");

                    dt.Columns.Add("targetdate", typeof(DateTime));

                    for (int i = 0; i < dateListArray.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(dateListArray[i]))
                        {
                            DataRow row = dt.NewRow();
                            row["targetdate"] = dateListArray[i];
                            dt.Rows.Add(row);
                        }
                    }

                }

                try
                {
                    data.AddIntParameter("@arg_WebCalID", WebCalID);
                    data.AddDateParameter("@arg_SourceDate", sourceDate);
                    data.AddSelecteddatesDataTable("CalDatesList", dt);
                    data.SubmitData("pos_PreorderCalItemScheduler", DataPortal.QueryType.StoredProc);
                    retValue = 1;
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
            return retValue;
        }




        public static void UpdateCalByOrderStatusForMonth(int pCalID, DateTime pDate, string pShowOrderStatus)
        {
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure
                data.AddIntParameter("@WebCalID", pCalID);
                data.AddStringParameter("@Status", pShowOrderStatus);
                data.AddDateParameter("@SelectedDate", pDate);

                data.SubmitData("pos_UpdateCalByOrderStatusMonth", DataPortal.QueryType.StoredProc);
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

        public static void UpdateCalByOrderStatusForDay(int pCalID, DateTime pDate, string pShowOrderStatus)
        {
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure
                data.AddIntParameter("@WebCalID", pCalID);
                data.AddStringParameter("@Status", pShowOrderStatus);
                data.AddDateParameter("@SelectedDate", pDate);

                data.SubmitData("pos_UpdateCalByOrderStatusDay", DataPortal.QueryType.StoredProc);
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

        public static int SaveMenuItems(List<WebCalItem> WebCalItemList)
        {
            int retValue = 0;

            DataTable dt = null;

            DataPortal data = new DataPortal();

            if (WebCalItemList.Count > 0)
            {


                dt = new DataTable("WebCalItemsList");

                dt.Columns.Add("WebCalID", typeof(int));
                dt.Columns.Add("calItemDate", typeof(DateTime));
                dt.Columns.Add("menus_id", typeof(int));
                dt.Columns.Add("useOrder", typeof(int));

                foreach (var item in WebCalItemList)
                {
                    DataRow row = dt.NewRow();
                    row["WebCalID"] = item.webCalID;
                    row["calItemDate"] = item.calItemDate;
                    row["menus_id"] = item.menus_id;
                    row["useOrder"] = item.useOrder;
                    dt.Rows.Add(row);
                }


                try
                {
                    data.AddDataTableParameterWebCalItem("WebCalItemsList", dt);
                    data.SubmitData("pos_UpdateCalItems", DataPortal.QueryType.StoredProc);
                    retValue = 1;
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
            return retValue;
        }

        public static DataSet GetWebLunchCutoffSettings(DateTime selectedDate, int pWebCalId, Int32 districtID)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddDateParameter("@arg_CalDate", selectedDate);
                data.AddIntParameter("@arg_WebCalID", pWebCalId);
                data.AddIntParameter("@arg_districtID", districtID);


                data.FillDataSet("pos_GetWebLunchCutoffSettings", DataPortal.QueryType.StoredProc, ds);
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

        public static void UpdateOverrideCutOff(int CutOffType, int CutOffValue,   int pWEbCalID, DateTime pDate)
        {
            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure
                data.AddIntParameter("@CutOffType", CutOffType);
                data.AddIntParameter("@CutOffValue", CutOffValue);
                data.AddIntParameter("@pWebcalid", pWEbCalID);

                data.AddDateParameter("@pDate", pDate);

                data.SubmitData("pos_OverrideCutOffDay", DataPortal.QueryType.StoredProc);
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

        public static void ChangeCalendarName(int CalID, int DistrictId, string CalName)
        {

            DataPortal data = new DataPortal();
            try
            {
                //Set the command text as name of the stored procedure
                data.AddIntParameter("@CalID", CalID);
                data.AddIntParameter("@DistrictId", DistrictId);
                data.AddStringParameter("@CalName", CalName);


                data.SubmitData("usp_ADMIN_ChangeCalendarName", DataPortal.QueryType.StoredProc);
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

            /////////////////
        }


    }
}

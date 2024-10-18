using AdminPortalModels.ViewModels;
using MSA_ADMIN.DAL.Factories;
using MSA_ADMIN.DAL.Models;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;

namespace MSA_AdminPortal.Helpers
{
    public class PreorderCalHelper
    {
        private long clientId = ClientInfoData.GetClientID();

        public List<CalEvent> CalendarItems(int pWebcalId, DateTime StartDate, DateTime EndDate)
        {
            Collection<CalEvent> CalendarItems = CalFactory.GetWebLunchEventsByWebcalID(pWebcalId, StartDate, EndDate);

            ///////////
            string hdColor = "0";
            List<CalEvent> CalendarItemsModified = new List<CalEvent>();
            string CutOffDateStr = "";

            foreach (CalEvent appointment in CalendarItems)
            {
                string showorder = appointment.showOrder.ToString();

                CalEvent newEvent = new CalEvent();
                newEvent = appointment;

                DateTime CutOffDate = DateTime.Now.AddDays(-1);
                DateTime OrderDate = DateTime.Now;

                string WebLunchCutOffValue = appointment.WebLunchCutOffValue;
                string OverrideCutOffValue = appointment.OverrideCutOffValue;
                string OverrideStartDate = appointment.OverrideStartDate;

                DateTime OverrideStartDte = Convert.ToDateTime(OverrideStartDate);


                bool isWebLunchCutOff = WebLunchCutOffValue != "" && WebLunchCutOffValue != "0";
                bool isOverriddentCutOff = OverrideCutOffValue != "" && OverrideCutOffValue != "0";


                if (!string.IsNullOrEmpty(appointment.CutOffDateStr))
                {
                    CutOffDateStr = appointment.CutOffDateStr;
                    CutOffDate = Convert.ToDateTime(CutOffDateStr);

                }
                else
                {
                    isWebLunchCutOff = false;
                }

                if (!string.IsNullOrEmpty(Convert.ToString(appointment.date)))
                {
                    string OrderDateStr = appointment.date;
                    OrderDate = Convert.ToDateTime(appointment.date);
                }




                DateTime AcceptOrderDate =  TimeZoneSettings.Instance.GetLocalTime();
                bool orderingClosed = AcceptOrderDate >= CutOffDate;
                bool OverrideAllowOrder = false;

                if (OverrideStartDte.AddDays(Convert.ToDouble(OverrideCutOffValue)) <= OrderDate && !orderingClosed && isOverriddentCutOff)
                {
                    OverrideAllowOrder = true;
                }

                if (orderingClosed && !OverrideAllowOrder)
                {
                    newEvent.color = CalItemsColors.ColorsDict[ShowOrders.CutoffItems];
                    newEvent.textColor = "#ffffff";
                    hdColor = "red";
                }

                else if (isWebLunchCutOff)
                {
                    if (isOverriddentCutOff && OverrideAllowOrder)
                    {
                        newEvent.color = CalItemsColors.ColorsDict[ShowOrders.OverrideCutoffItems]; //"overrideCutoffItems";//order open due to override

                        hdColor = "blue";
                    }
                    else
                    {
                        if (showorder == "1")
                        {
                            newEvent.color = CalItemsColors.ColorsDict[ShowOrders.ViewOnly]; //"MyCustomAppointmentStyle";
                        }
                        else
                        {
                            if (showorder == "2")
                            {
                                if (isOverriddentCutOff && OverrideAllowOrder && hdColor == "red")
                                {
                                    newEvent.color = CalItemsColors.ColorsDict[ShowOrders.OverrideCutoffItems];// "overrideCutoffItems";//order open due to override

                                }
                                else
                                {
                                    if (hdColor == "green" || hdColor == "0" || !isOverriddentCutOff || !OverrideAllowOrder)
                                    {
                                        newEvent.color = CalItemsColors.ColorsDict[ShowOrders.AllowOrders]; //"MyCustomAppointmentStyle1";
                                        hdColor = "green";
                                    }
                                }
                            }
                            // Added by farrukh m (allshore) to resolve MA-1.
                            else if (showorder == "0" && isOverriddentCutOff == false)
                            {
                                newEvent.color = CalItemsColors.ColorsDict[ShowOrders.AdminOnly];// "MyCustomAppointmentStyle2";
                            }
                            //---------- end ------------------------------------
                        }
                    }

                }

                else if (showorder == "1")
                    newEvent.color = CalItemsColors.ColorsDict[ShowOrders.ViewOnly];//"MyCustomAppointmentStyle";
                else if (showorder == "2")
                    newEvent.color = CalItemsColors.ColorsDict[ShowOrders.AllowOrders]; //"MyCustomAppointmentStyle1";
                // Added by farrukh m (allshore) to resolve MA-1.
                else if (showorder == "0" && isOverriddentCutOff == false)
                {
                    newEvent.color = CalItemsColors.ColorsDict[ShowOrders.AdminOnly]; // "MyCustomAppointmentStyle2";
                }
                //---------- end ------------------------------------
                string pAltDescription = appointment.AltDescription;
                if (pAltDescription == null)
                    newEvent.AltDescription = "";
                else
                    newEvent.AltDescription = pAltDescription;


                string pSub = appointment.Subject;// e.Container.Appointment.Attributes["Subject"];
                if (pSub == null)
                    newEvent.Subject = "";
                else
                {
                    if (pSub.Trim().Length > 15)
                    {
                        newEvent.Subject = pSub.Substring(0, 15) + "...";
                    }
                }


                CalendarItemsModified.Add(newEvent);
            }

            return CalendarItemsModified;
        }

        public bool GetSameDayOrdering(int districtID)
        {
            bool retValue = false;
            try
            {
                if (DistrictFactory.DistrictOptionInSession != null)
                {
                    bool? tempVal = DistrictFactory.DistrictOptionInSession.useSameDayOrdering;
                    if (tempVal.HasValue)
                    {
                        retValue = tempVal.Value;
                    }
                }

            }
            catch (Exception)
            {

                //throw;
            }
            return retValue;
        }

        public OrderingOptionsModel GetOrderingOptionsModel(Int32 webCallId)
        {

            bool useSameDayOrdering = GetSameDayOrdering(Convert.ToInt32(clientId));
            OrderingOptionsModel orderingOptionsModel = new OrderingOptionsModel();
            orderingOptionsModel.WebCalID = webCallId;

            orderingOptionsModel.useSameDayOrdering = useSameDayOrdering == true ? "true" : "false";

            DataSet tblWebCal = CalFactory.GetWebCalDetailsOnScheduler(webCallId);

            if (tblWebCal.Tables[0].Rows.Count > 0 && tblWebCal.Tables[0].Rows[0]["CutOffType"].ToString().Length > 0)
            {
                orderingOptionsModel.CutOffSelection = Convert.ToInt32(tblWebCal.Tables[0].Rows[0]["CutOffSelection"].ToString());
                orderingOptionsModel.CutOffType = Convert.ToInt32(tblWebCal.Tables[0].Rows[0]["CutOffType"].ToString());
                orderingOptionsModel.CutOffValue = Convert.ToInt32(tblWebCal.Tables[0].Rows[0]["CutOffValue"].ToString());
            }

            if (tblWebCal.Tables.Count > 0 && tblWebCal.Tables[0].Rows.Count > 0)
            {
                Int32 orderingOption = 0;
                if (tblWebCal.Tables[0].Rows[0]["OrderingOption"].ToString().Length > 0)
                {
                    orderingOption = Convert.ToInt32(tblWebCal.Tables[0].Rows[0]["OrderingOption"].ToString());
                }

                //Non live districts cannot use same day ordering option
                if (!useSameDayOrdering)
                {
                    if (orderingOption == 2)
                    {
                        orderingOption = 0;
                    }
                }

                orderingOptionsModel.OrderingOption = orderingOption;

                orderingOptionsModel.DistrictID = Convert.ToInt32(tblWebCal.Tables[0].Rows[0]["DistrictID"].ToString());
                orderingOptionsModel.CalendarType = tblWebCal.Tables[0].Rows[0]["CalendarType"].ToString();


                //HDOrderingOption.Value = orderingOption;

                string SameDayOrderTime = tblWebCal.Tables[0].Rows[0]["SameDayOrderTime"].ToString();
                if (!string.IsNullOrEmpty(SameDayOrderTime))
                {
                    string[] SameDayOrderTimeSplit = SameDayOrderTime.Split(':');
                    if (SameDayOrderTimeSplit.Length > 0)
                    {
                        string hours = SameDayOrderTimeSplit[0];
                        string AMPMStr = "AM";
                        int hoursInt = Convert.ToInt16(SameDayOrderTimeSplit[0].ToString());
                        if (hoursInt >= 12)
                        {
                            AMPMStr = "PM";
                        }

                        if (hoursInt > 12)
                        {
                            hours = Convert.ToString(hoursInt - 12);
                            hoursInt = Convert.ToInt16(hours);
                        }
                        orderingOptionsModel.sameDayHours = Convert.ToString(hoursInt);
                        orderingOptionsModel.sameDayMinutes = SameDayOrderTimeSplit[1].ToString();
                        orderingOptionsModel.sameDayAmPm = AMPMStr;
                    }
                }
            }

            /////
            List<DataItem> SinglesDayList = new List<DataItem>();
            SinglesDayList.Add(new DataItem { data = "Day", value = 0 });
            SinglesDayList.Add(new DataItem { data = "Month", value = 1 });
            SinglesDayList.Add(new DataItem { data = "Week", value = 2 });
            orderingOptionsModel.SinglesDayList = SinglesDayList;
            ////////

            List<DataItem> MultipleDayList = new List<DataItem>();
            MultipleDayList.Add(new DataItem { data = "Day(s)", value = 0 });
            MultipleDayList.Add(new DataItem { data = "Month(s)", value = 1 });
            MultipleDayList.Add(new DataItem { data = "Week(s)", value = 2 });
            orderingOptionsModel.MultipleDayList = MultipleDayList;

            ////////
            List<DataItemStr> HousrList = new List<DataItemStr>();
            HousrList.Add(new DataItemStr { data = "00", value = "0" });
            HousrList.Add(new DataItemStr { data = "01", value = "1" });
            HousrList.Add(new DataItemStr { data = "02", value = "2" });
            HousrList.Add(new DataItemStr { data = "03", value = "3" });
            HousrList.Add(new DataItemStr { data = "04", value = "4" });
            HousrList.Add(new DataItemStr { data = "05", value = "5" });
            HousrList.Add(new DataItemStr { data = "06", value = "6" });
            HousrList.Add(new DataItemStr { data = "07", value = "7" });
            HousrList.Add(new DataItemStr { data = "08", value = "8" });
            HousrList.Add(new DataItemStr { data = "09", value = "9" });
            HousrList.Add(new DataItemStr { data = "10", value = "10" });
            HousrList.Add(new DataItemStr { data = "11", value = "11" });
            HousrList.Add(new DataItemStr { data = "12", value = "12" });
            orderingOptionsModel.HousrList = HousrList;


            //////
            List<DataItemStr> MinutesList = new List<DataItemStr>();
            MinutesList.Add(new DataItemStr { data = "00", value = "0" });
            MinutesList.Add(new DataItemStr { data = "15", value = "15" });
            MinutesList.Add(new DataItemStr { data = "30", value = "30" });
            MinutesList.Add(new DataItemStr { data = "45", value = "45" });
            orderingOptionsModel.MinutesList = MinutesList;

            //////////////
            List<DataItemStr> AMPMList = new List<DataItemStr>();
            AMPMList.Add(new DataItemStr { data = "AM", value = "AM" });
            AMPMList.Add(new DataItemStr { data = "PM", value = "PM" });
            orderingOptionsModel.AMPMList = AMPMList;

            return orderingOptionsModel;

        }

        public void SaveOrderingOptions(OrderingOptionsModel orderingOptionsModel)
        {
            Int32 cutofftype = orderingOptionsModel.CutOffType;
            Int32 cutoffdayvalue = orderingOptionsModel.CutOffValue;
            int cutoffselection = orderingOptionsModel.CutOffSelection;
            string orderingoption = orderingOptionsModel.OrderingOption.ToString();
            int Webcalid = orderingOptionsModel.WebCalID;
            int orderingOptionInt = 0;

            switch (orderingoption)
            {
                case "0": //"none":
                    orderingOptionInt = 0;
                    CalFactory.UpdateWebLunchCalendarOrderingOption(Webcalid, orderingOptionInt);
                    break;

                case "1": //"cutoffsettings":
                    orderingOptionInt = 1;

                    CalFactory.UpdateWebLunchCalendarForCutOffValue(Webcalid, cutofftype, cutoffdayvalue, cutoffselection);
                    break;

                case "2": //"sameDaySettings":
                    orderingOptionInt = 2;
                    string hours = orderingOptionsModel.sameDayHours;
                    string minutes = orderingOptionsModel.sameDayMinutes;
                    string ampm = orderingOptionsModel.sameDayAmPm;

                    if (ampm == "PM" && Convert.ToInt32(hours) != 12)
                    {
                        hours = Convert.ToString(Convert.ToInt32(hours) + 12);
                    }

                    CalFactory.UpdateWebLunchCalendarForSameDayOrder(Webcalid, orderingOptionInt, string.Format("{0}:{1}", hours, minutes));

                    break;


                default:
                    break;
            }




        }

        public void SaveMenuItems(List<WebCalItem> list)
        {
            CalFactory.SaveMenuItems(list);
        }

        public void LoopforSelecteddates(ItemScheduler itemScheduler)
        {
            CalFactory.SavePreorderItemsForSelectedDates(itemScheduler.WebCalID, itemScheduler.SourceDate, itemScheduler.dateList);
        }

        public Collection<CategoryData> getCategoryList()
        {

            Collection<CategoryData> cate = CategoryFactory.ListCategoryByKeyword("", 0, 10000, Convert.ToInt32(clientId));
            return cate;

        }

        public CalSettingsData GetWebLunchCutoffSettings(DateTime selecteddate,  Int32 WebCalID)
        {
            CalSettingsData calSettingsData = new CalSettingsData();
            DataSet ds = CalFactory.GetWebLunchCutoffSettings(selecteddate, WebCalID, Convert.ToInt32(clientId));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    calSettingsData.useFiveDayWeekCutOff = Convert.ToString(ds.Tables[0].Rows[0]["useFiveDayWeekCutOff"]);
                    calSettingsData.CutOffDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["CutOffDate"].ToString());
                    calSettingsData.overrideCutOff = Convert.ToString(ds.Tables[0].Rows[0]["OverrideCutOffValue"]);
                    calSettingsData.Cutoffvalue = Convert.ToInt32(ds.Tables[0].Rows[0]["CutOffValue"]);
                
                }
            }

            return calSettingsData;
        }

        public void UpdateOverrideCutOffDate(int CutOffType, int CutOffValue,   int pWEbCalID, DateTime pDate)
        {
            CalFactory.UpdateOverrideCutOff(CutOffType, CutOffValue, pWEbCalID, pDate);
        
        }

        public Collection<MenuItemData> SearchCategories(SearchModel searchModel)
        {

            string pKeyword = "";

            if (searchModel.searchStr != "")
                pKeyword = "%" + searchModel.searchStr + "%";
            else
                pKeyword = "%";

            
            Int32 pPageIndex = 0;
            Int32 pMenuCount = 14784;
            Int32 ClientID = Convert.ToInt32(clientId);
            Collection<MenuItemData> Allamenuitems  = null;

            switch (searchModel.searchType)
            {
                case -1:
                    Allamenuitems = MenuItemsFactory.ListMenuByKeyword(pKeyword, pPageIndex, pMenuCount, ClientID);
                    break;
                case 1:
                    Allamenuitems = MenuItemsFactory.getAvailableItemsByCategoryId(searchModel.CategoryID, pKeyword, pPageIndex, pMenuCount, ClientID);
                    break;

                default:
                    break;
            }


            return Allamenuitems;
            

        
        }

        public void ChangeCalendarName(int CalID, int DistrictId, string CalName)
        {
            CalFactory.ChangeCalendarName(CalID, DistrictId, CalName);
        }

    }
}

using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable()]
    public struct CalData
    {
        #region Private Constants and Variables
        private int pid;
        private SmartDate pdate;
        private int pmeal;
        private int pmenus_id;
        private string pMenuName;
        private NameValuePair pWebcall;
        private int pShowOrder;
        private string pAltDescription;
        private SmartDate pCutOffDate;
        private string pWebLunchCutOffValue;
        private string pOverrideCutOffValue;
        private SmartDate pOverrideStartDate;
        private string pCutOffDateStr;
        




        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        public int id
        {
            get { return pid; }
            set { pid = value; }
        }
        public SmartDate date
        {
            get { return pdate; }
            set { pdate = value; }
        }
        public int meal
        {
            get { return pmeal; }
            set { pmeal = value; }
        }
        public int menus_id
        {
            get { return pmenus_id; }
            set { pmenus_id = value; }
        }
        public string MenuName
        {
            get { return pMenuName; }
            set { pMenuName = value; }
        }
        public string CalendarName
        {
            get { return this.Webcall.Name; }
        }
        public int WecalID
        {
            get { return this.Webcall.Value; }
        }
        public string Description
        {
            get { return this.Webcall.Name; }
        }
        public DateTime StartDate
        {
            get { return Convert.ToDateTime(this.date.ToString()); }
        }
        public DateTime EndDate
        {
            get { return Convert.ToDateTime(this.date.Date.AddMinutes(10)); }
        }
        public int ShowOrder
        {
            get { return pShowOrder; }
            set { pShowOrder = value; }
        }
        public string AltDescription
        {
            get { return pAltDescription; }
            set { pAltDescription = value; }
        }

        public SmartDate CutOffDate
        {
            get { return pCutOffDate; }
            set { pCutOffDate = value; }
        }

        public string WebLunchCutOffValue
        {
            get { return pWebLunchCutOffValue; }
            set { pWebLunchCutOffValue = value; }
        }

        public string OverrideCutOffValue
        {
            get { return pOverrideCutOffValue; }
            set { pOverrideCutOffValue = value; }
        }

        public SmartDate OverrideStartDate
        {
            get { return pOverrideStartDate; }
            set { pOverrideStartDate = value; }
        }
        public string CutOffDateStr
        {
            get { return pCutOffDateStr; }
            set { pCutOffDateStr = value; }
        }



        public NameValuePair Webcall
        {
            get { return pWebcall; }
            set { pWebcall = value; }
        }

        public ObjectHistoryData ObjectHistory
        {
            get { return this.pObjectHistory; }
            set { this.pObjectHistory = value; }
        }


        public BusinessObjectState ObjectState
        {
            get { return this.pObjectState; }
            set { this.pObjectState = value; }
        }

        #endregion

        #region Object Overrides


        public override string ToString()
        {
            return this.id.ToString();
        }

        #endregion
    }

    [Serializable()]
    public class CalEvent 
    {
        private int pid;
        private string ptitle;
        private string pstart;
        private string pend;
        private string pcolor;
        private int pShowOrder;
        private int pmenus_id;
        private string pWebLunchCutOffValue;
        private string pOverrideCutOffValue;
        private string pOverrideStartDate;
        private string pCutOffDateStr;
        private string pdate;
        private string pAltDescription;
        private string pSubject;
        private string ptextColor;
        private int puserOrder;

        private int pWebCalID;
        private decimal studentFullPrice;

        public int id
        {
            get { return pid; }
            set { pid = value; }
        }
        public string title
        {
            get { return ptitle; }
            set { ptitle = value; }
        }
        public string start
        {
            get { return pstart; }
            set { pstart = value; }
        }
        public string end
        {
            get { return pend; }
            set { pend = value; }
        }
        public string color
        {
            get { return pcolor; }
            set { pcolor = value; }
        }

        public int showOrder
        {
            get { return pShowOrder; }
            set { pShowOrder = value; }
        }
        public int menus_id
        {
            get { return pmenus_id; }
            set { pmenus_id = value; }
        }

        public string WebLunchCutOffValue
        {
            get { return pWebLunchCutOffValue; }
            set { pWebLunchCutOffValue = value; }
        }
        public string OverrideCutOffValue
        {
            get { return pOverrideCutOffValue; }
            set { pOverrideCutOffValue = value; }
        }
        public string OverrideStartDate
        {
            get { return pOverrideStartDate; }
            set { pOverrideStartDate = value; }
        }
        public string CutOffDateStr
        {
            get { return pCutOffDateStr; }
            set { pCutOffDateStr = value; }
        }
        public string date
        {
            get { return pdate; }
            set { pdate = value; }
        }

        public string AltDescription
        {
            get { return pAltDescription; }
            set { pAltDescription = value; }
        }


        public string Subject
        {
            get { return pSubject; }
            set { pSubject = value; }
        }

        public string textColor
        {
            get { return ptextColor; }
            set { ptextColor = value; }
        }

        public int userOrder
        {
            get { return puserOrder; }
            set { puserOrder = value; }
        }
        public int WebCalID
        {
            get { return pWebCalID; }
            set { pWebCalID = value; }
        }
        public decimal StudentFullPrice
        {
            get { return studentFullPrice; }
            set { studentFullPrice = value; }
        }

        

        

    }

    public enum ShowOrders { ViewOnly = 1, AllowOrders, AdminOnly, CutoffItems, OverrideCutoffItems };

    public static class CalItemsColors
    {
        public static readonly IDictionary<ShowOrders, string> ColorsDict = new ReadOnlyDictionary<ShowOrders, string>(new Dictionary<ShowOrders, string>
        {
            {ShowOrders.ViewOnly, "#fff8be"},
            {ShowOrders.AllowOrders, "#d0ecbb"},
            {ShowOrders.AdminOnly, "#dbdbda"},
            {ShowOrders.CutoffItems, "#f23a3a"},
            {ShowOrders.OverrideCutoffItems, "#93ccfa"}
        });


    }

    public static class CalItemsStatus
    {
        public static readonly IDictionary<ShowOrders, string> StatusDict = new ReadOnlyDictionary<ShowOrders, string>(new Dictionary<ShowOrders, string>
        {
            {ShowOrders.ViewOnly, "ViewOnly"},
            {ShowOrders.AllowOrders, "AllowOrders"},
            {ShowOrders.AdminOnly, "AdminOnly"},
        });


    }

}

using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct WebLunchCalendarData
    {
        #region Private Constants and Variables
        private int pWebCalID;
        private string pCalendarName;
        private int pCalendarType;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        public int WebCalID
        {
            get { return pWebCalID; }
            set { pWebCalID = value; }
        }
        

        public string CalendarName
        {
            get { return pCalendarName; }
            set { pCalendarName = value; }
        }

        public int CalendarType
        {
            get { return pCalendarType; }
            set { pCalendarType = value; }
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
            return this.pWebCalID.ToString();
        }

        #endregion
    }
}

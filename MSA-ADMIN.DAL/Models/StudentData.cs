using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable()]
    public struct StudentData
    {
        #region Private Constants and Variables


        private int pId;
        private string pUserID;
        private string pRace;
        private SmartDate pAppDate;
        private bool pFosterChild;
        private float pFosterIncome;
        private string pApprovalStatus;
        private SmartDate pDateEntered;
        private SmartDate pDateChanged;
        private string pHouseholdID;
        private NameValuePair pDistrict;
        private NameValuePairCollection pCustomer;
        private bool pAppLetterSent;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 


        public int Id
        {
            get { return pId; }
            set { pId = value; }
        }
        public string UserID
        {
            get { return pUserID; }
            set { pUserID = value; }
        }
        public string Race
        {
            get { return pRace; }
            set { pRace = value; }
        }
        public SmartDate AppDate
        {
            get { return pAppDate; }
            set { pAppDate = value; }
        }
        public bool FosterChild
        {
            get { return pFosterChild; }
            set { pFosterChild = value; }
        }
        public float FosterIncome
        {
            get { return pFosterIncome; }
            set { pFosterIncome = value; }
        }
        public string ApprovalStatus
        {
            get { return pApprovalStatus; }
            set { pApprovalStatus = value; }
        }
        public SmartDate DateEntered
        {
            get { return pDateEntered; }
            set { pDateEntered = value; }
        }
        public SmartDate DateChanged
        {
            get { return pDateChanged; }
            set { pDateChanged = value; }
        }
        public string HouseholdID
        {
            get { return pHouseholdID; }
            set { pHouseholdID = value; }
        }
        public NameValuePair District
        {
            get { return pDistrict; }
            set { pDistrict = value; }
        }
        public NameValuePairCollection Customer
        {
            get { return pCustomer; }
            set { pCustomer = value; }
        }
        public bool AppLetterSent
        {
            get { return pAppLetterSent; }
            set { pAppLetterSent = value; }
        }
        

        public ObjectHistoryData ObjectHistory
        {
            get { return this.pObjectHistory; }
            set { this.pObjectHistory = value; }
        }
        /// <summary>
        /// Indicates the state of the Order's data (new, deleted, modified, or 
        /// unmodified).
        /// </summary>
        public BusinessObjectState ObjectState
        {
            get { return this.pObjectState; }
            set { this.pObjectState = value; }
        }

        #endregion

        #region Object Overrides

        /// <summary>
        /// Returns the Department's ID.
        /// </summary>
        /// <returns>The Department's ID.</returns>
        public override string ToString()
        {
            return this.pId.ToString();
        }

        #endregion

    }
}

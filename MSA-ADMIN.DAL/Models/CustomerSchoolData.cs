using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct CustomerSchoolData
    {
        #region Private Constants and Variables

        private int pCustID;
        private NameValuePairCollection pCustomer;
        private NameValuePair pSchool;
        private NameValuePair pGrade;
        private NameValuePair pHomeroom;
        private bool pisPrimary;
       
        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int CustID
        {
            get { return pCustID; }
            set { pCustID = value; }
        }
        public NameValuePairCollection Customer
        {
            get { return pCustomer; }
            set { pCustomer = value; }
        }
        public NameValuePair School
        {
            get { return pSchool; }
            set { pSchool = value; }
        }
        public NameValuePair Grades
        {
            get { return pGrade; }
            set { pGrade = value; }
        }
        public NameValuePair Homeroom
        {
            get { return pHomeroom; }
            set { pHomeroom = value; }
        }
        public bool isPrimary
        {
            get { return pisPrimary; }
            set { pisPrimary = value; }
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
            return this.pCustID.ToString();
        }

        #endregion
    }
}

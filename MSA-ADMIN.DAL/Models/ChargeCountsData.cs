using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct ChargeCountsData
    {
        #region Private Constants and Variables

        private int pId;
        private NameValuePairCollection pCustomer;
        private SmartDate pSDate;
        private SmartDate pEDate;
        private SmartDate pWLetter1;
        private SmartDate pWLetter2;
        private SmartDate pWLetter3;

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
        public NameValuePairCollection Customer
        {
            get { return pCustomer; }
            set { pCustomer = value; }
        }
        public SmartDate SDate
        {
            get { return pSDate; }
            set { pSDate = value; }
        }
        public SmartDate EDate
        {
            get { return pEDate; }
            set { pEDate = value; }
        }
        public SmartDate WLetter1
        {
            get { return pWLetter1; }
            set { pWLetter1 = value; }
        }
        public SmartDate WLetter2
        {
            get { return pWLetter2; }
            set { pWLetter2 = value; }
        }
        public SmartDate WLetter3
        {
            get { return pWLetter3; }
            set { pWLetter3 = value; }
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

using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]

    public struct EditCheckEligAssignedData
    {
        #region Private Constants and Variables

        private SmartDate pDate;
        private int pSchool;
        private int pFreeEligA;
        private int pRedEligA;
        private int pPaidEligA;
        private int pFreeClaimedA;
        private int pRedClaimedA;
        private int pPaidClaimedA;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public SmartDate Date
        {
            get { return pDate; }
            set { pDate = value; }
        }
        public int School
        {
            get { return pSchool; }
            set { pSchool = value; }
        }
        public int FreeEligA
        {
            get { return pFreeEligA; }
            set { pFreeEligA = value; }
        }
        public int RedEligA
        {
            get { return pRedEligA; }
            set { pRedEligA = value; }
        }
        public int PaidEligA
        {
            get { return pPaidEligA; }
            set { pPaidEligA = value; }
        }
        public int FreeClaimedA
        {
            get { return pFreeClaimedA; }
            set { pFreeClaimedA = value; }
        }
        public int RedClaimedA
        {
            get { return pRedClaimedA; }
            set { pRedClaimedA = value; }
        }
        public int PaidClaimedA
        {
            get { return pPaidClaimedA; }
            set { pPaidClaimedA = value; }
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
    }
}

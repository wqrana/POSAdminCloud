using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct EditCheckEligData
    {
        #region Private Constants and Variables

        private SmartDate pDate;
        private int pSchool;
        private int pFreeElig;
        private int pRedElig;
        private int pPaidElig;
        private int pFreeClaimed;
        private int pRedClaimed;
        private int pPaidClaimed;

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
        public int FreeElig
        {
            get { return pFreeElig; }
            set { pFreeElig = value; }
        }
        public int RedElig
        {
            get { return pRedElig; }
            set { pRedElig = value; }
        }
        public int PaidElig
        {
            get { return pPaidElig; }
            set { pPaidElig = value; }
        }
        public int FreeClaimed
        {
            get { return pFreeClaimed; }
            set { pFreeClaimed = value; }
        }
        public int RedClaimed
        {
            get { return pRedClaimed; }
            set { pRedClaimed = value; }
        }
        public int PaidClaimed
        {
            get { return pPaidClaimed; }
            set { pPaidClaimed = value; }
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

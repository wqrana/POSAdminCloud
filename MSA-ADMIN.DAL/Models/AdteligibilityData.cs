using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct AdteligibilityData
    {
        #region Private Constants and Variables

        private int pId;
        private float pAFAnnual;
        private float pAFMonthly;
        private float pAFWeekly;
        private float pARAnnual;
        private float pARMonthly;
        private float pARWeekly;
        private int pDistrict_Id;

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
        public float AFAnnual
        {
            get { return pAFAnnual; }
            set { pAFAnnual = value; }
        }
        public float AFMonthly
        {
            get { return pAFMonthly; }
            set { pAFMonthly = value; }
        }
        public float AFWeekly
        {
            get { return pAFWeekly; }
            set { pAFWeekly = value; }
        }
        public float ARAnnual
        {
            get { return pARAnnual; }
            set { pARAnnual = value; }
        }
        public float ARMonthly
        {
            get { return pARMonthly; }
            set { pARMonthly = value; }
        }
        public float ARWeekly
        {
            get { return pARWeekly; }
            set { pARWeekly = value; }
        }
        public int District_Id
        {
            get { return pDistrict_Id; }
            set { pDistrict_Id = value; }
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

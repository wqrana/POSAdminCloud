using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct EligibilityData
    {
        #region Private Constants and Variables

        private int pId;
        private int pFamSize;
        private float pFAnnual;
        private float pFMonthly;
        private float pFWeekly;
        private float pRAnnual;
        private float pRMonthly;
        private float pRWeekly;
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
        public int FamSize
        {
            get { return pFamSize; }
            set { pFamSize = value; }
        }
        public float FAnnual
        {
            get { return pFAnnual; }
            set { pFAnnual = value; }
        }
        public float FMonthly
        {
            get { return pFMonthly; }
            set { pFMonthly = value; }
        }
        public float FWeekly
        {
            get { return pFWeekly; }
            set { pFWeekly = value; }
        }
        public float RAnnual
        {
            get { return pRAnnual; }
            set { pRAnnual = value; }
        }
        public float RMonthly
        {
            get { return pRMonthly; }
            set { pRMonthly = value; }
        }
        public float RWeekly
        {
            get { return pRWeekly; }
            set { pRWeekly = value; }
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

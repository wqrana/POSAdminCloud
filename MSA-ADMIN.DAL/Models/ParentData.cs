using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct ParentData
    {
        #region Private Constants and Variables


        private int pId;
        private string pHouseholdID;
        private string pLName;
        private string pFName;
        private string pMI;
        private string pSSN;
        private string pParentalStatus;
        private float pIncome1;
        private float pIncome2;
        private float pIncome3;
        private float pIncome4;
        private float pIncome5;
        private float pTotalIncome;
        private string pFrequency1;
        private string pFrequency2;
        private string pFrequency3;
        private string pFrequency4;
        private string pFrequency5;
        private NameValuePair pDistrict;

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
        public string HouseholdID
        {
            get { return pHouseholdID; }
            set { pHouseholdID = value; }
        }
        public string LName
        {
            get { return pLName; }
            set { pLName = value; }
        }
        public string FName
        {
            get { return pFName; }
            set { pFName = value; }
        }
        public string MI
        {
            get { return pMI; }
            set { pMI = value; }
        }
        public string SSN
        {
            get { return pSSN; }
            set { pSSN = value; }
        }
        public string ParentalStatus
        {
            get { return pParentalStatus; }
            set { pParentalStatus = value; }
        }
        public float Income1
        {
            get { return pIncome1; }
            set { pIncome1 = value; }
        }
        public float Income2
        {
            get { return pIncome2; }
            set { pIncome2 = value; }
        }
        public float Income3
        {
            get { return pIncome3; }
            set { pIncome3 = value; }
        }
        public float Income4
        {
            get { return pIncome4; }
            set { pIncome4 = value; }
        }
        public float Income5
        {
            get { return pIncome5; }
            set { pIncome5 = value; }
        }
        public float TotalIncome
        {
            get { return pTotalIncome; }
            set { pTotalIncome = value; }
        }
        public string Frequency1
        {
            get { return pFrequency1; }
            set { pFrequency1 = value; }
        }
        public string Frequency2
        {
            get { return pFrequency2; }
            set { pFrequency2 = value; }
        }
        public string Frequency3
        {
            get { return pFrequency3; }
            set { pFrequency3 = value; }
        }
        public string Frequency4
        {
            get { return pFrequency4; }
            set { pFrequency4 = value; }
        }
        public string Frequency5
        {
            get { return pFrequency5; }
            set { pFrequency5 = value; }
        }
        public NameValuePair Districtd
        {
            get { return pDistrict; }
            set { pDistrict = value; }
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

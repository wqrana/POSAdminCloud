using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct MenuData
    {
        #region Private Constants and Variables

        private int pMenuID;
        private NameValuePair pCategory;
        private string pItemName;
        private string pM_F6_Code;
        private decimal pStudentFullPrice;
        private decimal pStudentRedPrice;
        private decimal pEmployeePrice;
        private decimal pGuestPrice;
        private bool pisTaxable;
        private bool pisDeleted;
        private string pAltDescription;
        private int pCalId;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int MenuID
        {
            get { return pMenuID; }
            set { pMenuID = value; }
        }
        public int CalId
        {
            get { return pCalId; }
            set { pCalId = value; }
        }
        public NameValuePair Category
        {
            get { return pCategory; }
            set { pCategory = value; }
        }
        public string ItemName
        {
            get { return pItemName; }
            set { pItemName = value; }
        }
        public string M_F6_Code
        {
            get { return pM_F6_Code; }
            set { pM_F6_Code = value; }
        }
        public decimal StudentFullPrice
        {
            get { return pStudentFullPrice; }
            set { pStudentFullPrice = value; }
        }
        public decimal StudentRedPrice
        {
            get { return pStudentRedPrice; }
            set { pStudentRedPrice = value; }
        }
        public decimal EmployeePrice
        {
            get { return pEmployeePrice; }
            set { pEmployeePrice = value; }
        }
        public decimal GuestPrice
        {
            get { return pGuestPrice; }
            set { pGuestPrice = value; }
        }
        public bool isTaxable
        {
            get { return pisTaxable; }
            set { pisTaxable = value; }
        }
        public bool isDeleted
        {
            get { return pisDeleted; }
            set { pisDeleted = value; }
        }
        public string AltDescription
        {
            get { return pAltDescription; }
            set { pAltDescription = value; }
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
            return this.pMenuID.ToString();
        }

        #endregion
    }
}

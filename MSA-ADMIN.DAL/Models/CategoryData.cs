using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct CategoryData
    {
        #region Private Constants and Variables

        private int pCategoryID;
        //private NameValuePair pCategoryType;     
        private NameValuePairCollection pCategoryType; 
        private string pName;
        private bool pisActive;
        private bool pisDeleted;
        private int pColor;
        private string pAccountNumber;
       
        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int CategoryID
        {
            get { return pCategoryID; }
            set { pCategoryID = value; }
        }
        public NameValuePairCollection CategoryType
        {
            get { return pCategoryType; }
            set { pCategoryType = value; }
        }
        public int CategoryTypeID
        {
            get { return this.CategoryType[0].Value; }
            //set { pData.CategoryType.Value = value; }
        }
        public string CategoryTypeName
        {
            get { return this.CategoryType[0].Name; }
            //set { pData.CategoryType.Name = value; }
        }
        public bool canFree
        {
            get { return Convert.ToBoolean(this.CategoryType[1].Name); }
            //set { pData.CategoryType.Name = value; }
        }
        public bool canReduce
        {
            get { return Convert.ToBoolean(this.CategoryType[2].Name); }
            //set { pData.CategoryType.Name = value; }
        }
        public string Name
        {
            get { return pName; }
            set { pName = value; }
        }
        public bool isActive
        {
            get { return pisActive; }
            set { pisActive = value; }
        }
        public bool isDeleted
        {
            get { return pisDeleted; }
            set { pisDeleted = value; }
        }  
        public int Color
        {
            get { return pColor; }
            set { pColor = value; }
        }
        public string AccountNumber
        {
            get { return pAccountNumber; }
            set { pAccountNumber = value; }
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
            return this.pCategoryID.ToString();
        }

        #endregion
    }
}

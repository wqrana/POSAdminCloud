using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct CategoryTypesData
    {
        #region Private Constants and Variables

        private int pCategoryTypesID;       
        private string pName;
        private bool pcanFree;
        private bool pcanReduce;
        private bool pisDeleted;       

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int CategoryTypesID
        {
            get { return pCategoryTypesID; }
            set { pCategoryTypesID = value; }
        }

        public string Name
        {
            get { return pName; }
            set { pName = value; }
        }
        public bool canFree
        {
            get { return pcanFree; }
            set { pcanFree = value; }
        }
        public bool canReduce
        {
            get { return pcanReduce; }
            set { pcanReduce = value; }
        }
        public bool isDeleted
        {
            get { return pisDeleted; }
            set { pisDeleted = value; }
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
            return this.pCategoryTypesID.ToString();
        }

        #endregion
    }
}

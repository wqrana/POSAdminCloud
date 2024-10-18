using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable()]
    public struct CustomerActivityData
    {

        #region Private Constants and Variables

        private int pID;
        private int pCustomer_Id;
        private NameValuePairCollection pActivityItem;
        

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int ID
        {
            get { return pID; }
            set { pID = value; }
        }
        public int Customer_Id
        {
            get { return pCustomer_Id; }
            set { pCustomer_Id = value; }
        }
        public NameValuePairCollection ActivityItem
        {
            get { return pActivityItem; }
            set { pActivityItem = value; }
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
            return this.pID.ToString();
        }

        #endregion


    }
}

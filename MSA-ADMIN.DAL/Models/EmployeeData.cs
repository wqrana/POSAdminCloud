using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct EmployeeData
    {
        #region Private Constants and Variables

        private int pEmployeeID;
        private NameValuePairCollection pCustomer;
        private NameValuePair pSecurityGroup;       
        private string pLoginName;
        private string pPassword;
        
        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int EmployeeID
        {
            get { return pEmployeeID; }
            set { pEmployeeID = value; }
        }
        public NameValuePairCollection Customer
        {
            get { return pCustomer; }
            set { pCustomer = value; }
        }
        public NameValuePair SecurityGroup
        {
            get { return pSecurityGroup; }
            set { pSecurityGroup = value; }
        }

        public string LoginName
        {
            get { return pLoginName; }
            set { pLoginName = value; }
        }
        public string Password
        {
            get { return pPassword; }
            set { pPassword = value; }
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
            return this.pEmployeeID.ToString();
        }

        #endregion
    }
}

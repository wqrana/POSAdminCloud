using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct AccessRightsData
    {
        #region Private Constants and Variables

        private int pId;
        private NameValuePair pObject;
        private NameValuePair pSecurityGroup;
        private bool pcanInsert;
        private bool pcanDelete;
        private bool pcanView;
        private bool pcanEdit;       

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
        public NameValuePair ObjectP
        {
            get { return pObject; }
            set { pObject = value; }
        }
        public NameValuePair SecurityGroup
        {
            get { return pSecurityGroup; }
            set { pSecurityGroup = value; }
        }
        
        public bool canInsert
        {
            get { return pcanInsert; }
            set { pcanInsert = value; }
        }
        public bool canDelete
        {
            get { return pcanDelete; }
            set { pcanDelete = value; }
        }
        public bool canView
        {
            get { return pcanView; }
            set { pcanView = value; }
        }
        public bool canEdit
        {
            get { return pcanEdit; }
            set { pcanEdit = value; }
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

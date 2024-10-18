using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct PicturesData
    {
        #region Private Constants and Variables

        private int pId;
        private NameValuePairCollection pCustomer;
        private Object pPicture;
        private string pPictureType;
     
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
        public NameValuePairCollection Customer
        {
            get { return pCustomer; }
            set { pCustomer = value; }
        }
        public Object Picture
        {
            get { return pPicture; }
            set { pPicture = value; }
        }
        public string PictureType
        {
            get { return pPictureType; }
            set { pPictureType = value; }
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

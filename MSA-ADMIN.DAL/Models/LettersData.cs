using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct LettersData
    {
        #region Private Constants and Variables

        private int pId;
        private NameValuePair pDistrict;
        private NameValuePair pLanguage;
        private string pLetter1;
        private string pLetter2;
        private string pLetter3;
        
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
            set { pId= value; }
        }
        public NameValuePair District
        {
            get { return pDistrict; }
            set { pDistrict = value; }
        }
        public NameValuePair Language
        {
            get { return pLanguage; }
            set { pLanguage = value; }
        }
        public string Letter1
        {
            get { return pLetter1; }
            set { pLetter1 = value; }
        }
        public string Letter2
        {
            get { return pLetter2; }
            set { pLetter2 = value; }
        }
        public string Letter3
        {
            get { return pLetter3; }
            set { pLetter3 = value; }
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

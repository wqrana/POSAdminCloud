using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct EditCheckWData
    {
        #region Private Constants and Variables

        private int pId;
        private NameValuePair pEmpPreparedBy;
        private NameValuePair pSchool;
        private SmartDate pReportDate;
        private SmartDate pPreparedDate;
        private double pAttendenceFactor;
      
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
        public NameValuePair EmpPreparedBy
        {
            get { return pEmpPreparedBy; }
            set { pEmpPreparedBy = value; }
        }
        public NameValuePair School
        {
            get { return pSchool; }
            set { pSchool = value; }
        }
        public SmartDate ReportDate
        {
            get { return pReportDate; }
            set { pReportDate = value; }
        }
        public SmartDate PreparedDate
        {
            get { return pPreparedDate; }
            set { pPreparedDate = value; }
        }
        public double AttendenceFactor
        {
            get { return pAttendenceFactor; }
            set { pAttendenceFactor = value; }
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

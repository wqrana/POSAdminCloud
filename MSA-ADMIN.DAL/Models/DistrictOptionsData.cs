using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct DistrictOptionsData
    {
        #region Private Constants and Variables

        private int pID;
        private NameValuePair pDistrict;
        private SmartDate pChangedDate;
        private int pLetterWarning1;
        private int pLetterWarning2;
        private int pLetterWarning3;
        private double pTaxPercent;
        private bool pisEmployeeTaxable;
        private bool pisStudentFreeTaxable;
        private bool pisStudentPaidTaxable;
        private bool pisStudentRedTaxable;
        private SmartDate pStartSchoolYear;
        private SmartDate pEndSchoolYear;
        private SmartDate pStartForms;
        private SmartDate pEndForms;
        private bool pSetFormsDates;
       
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
        public NameValuePair District
        {
            get { return pDistrict; }
            set { pDistrict = value; }
        }
        public SmartDate ChangedDate
        {
            get { return pChangedDate; }
            set { pChangedDate = value; }
        }
        public int LetterWarning1
        {
            get { return pLetterWarning1; }
            set { pLetterWarning1 = value; }
        }
        public int LetterWarning2
        {
            get { return pLetterWarning2; }
            set { pLetterWarning2 = value; }
        }
        public int LetterWarning3
        {
            get { return pLetterWarning3; }
            set { pLetterWarning3 = value; }
        }
        public double TaxPercent
        {
            get { return pTaxPercent; }
            set { pTaxPercent = value; }
        }
        public bool isEmployeeTaxable
        {
            get { return pisEmployeeTaxable; }
            set { pisEmployeeTaxable = value; }
        }
        public bool isStudentFreeTaxable
        {
            get { return pisStudentFreeTaxable; }
            set { pisStudentFreeTaxable = value; }
        }
        public bool isStudentPaidTaxable
        {
            get { return pisStudentPaidTaxable; }
            set { pisStudentPaidTaxable = value; }
        }
        public bool isStudentRedTaxable
        {
            get { return pisStudentRedTaxable; }
            set { pisStudentRedTaxable = value; }
        }
        public SmartDate StartSchoolYear
        {
            get { return pStartSchoolYear; }
            set { pStartSchoolYear = value; }
        }
        public SmartDate EndSchoolYear
        {
            get { return pEndSchoolYear; }
            set { pEndSchoolYear = value; }
        }
        public SmartDate StartForms
        {
            get { return pStartForms; }
            set { pStartForms = value; }
        }
        public SmartDate EndForms
        {
            get { return pEndForms; }
            set { pEndForms = value; }
        }
        public bool SetFormsDates
        {
            get { return pSetFormsDates; }
            set { pSetFormsDates = value; }
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
            return this.ID.ToString();
        }

        #endregion
    }
}

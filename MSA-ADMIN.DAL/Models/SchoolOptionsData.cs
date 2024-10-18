using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct SchoolOptionsData
    {
        #region Private Constants and Variables

        private int pId;
        private NameValuePair pSchool;
        private SmartDate pChangedDate;
        private Double pAlaCarteLimit;
        private Double pMealPlanLimit;
        private bool pDoPinPreFix;
        private string pPinPreFix;
        private bool pPhotoLogging;
        private bool pFingerPrinting;
        private int pBarCodeLength;
        private SmartDate pStartSchoolYear;
        private SmartDate pEndSchoolYear;
        private bool pDistrictExecutive;
        private bool pDistrictDates;       

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
        public NameValuePair School
        {
            get { return pSchool; }
            set { pSchool = value; }
        }
        public SmartDate ChangedDate
        {
            get { return pChangedDate; }
            set { pChangedDate = value; }
        }
        public Double AlaCarteLimit
        {
            get { return pAlaCarteLimit; }
            set { pAlaCarteLimit = value; }
        }
        public Double MealPlanLimit
        {
            get { return pMealPlanLimit; }
            set { pMealPlanLimit = value; }
        }
        public bool DoPinPreFix
        {
            get { return pDoPinPreFix; }
            set { pDoPinPreFix = value; }
        }
        public string PinPreFix
        {
            get { return pPinPreFix; }
            set { pPinPreFix = value; }
        }
        public bool PhotoLogging
        {
            get { return pPhotoLogging; }
            set { pPhotoLogging = value; }
        }
        public bool FingerPrinting
        {
            get { return pFingerPrinting; }
            set { pFingerPrinting = value; }
        }
        public int BarCodeLength
        {
            get { return pBarCodeLength; }
            set { pBarCodeLength = value; }
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
        public bool DistrictExecutive
        {
            get { return pDistrictExecutive; }
            set { pDistrictExecutive = value; }
        }
        public bool DistrictDates
        {
            get { return pDistrictDates; }
            set { pDistrictDates = value; }
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

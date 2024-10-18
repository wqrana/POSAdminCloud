using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct CustomersData
    {
        #region Private Constants and Variables

        private int pCustomerID;
        private NameValuePair pDistrict;
        private NameValuePair pLanguage;
        private NameValuePair pGrade;
        private NameValuePair pHomeRoom;
        private bool pisStudent;
        private string pUserID;
        private string pPIN;
        private string pLastName;
        private string pFirstName;
        private string pMiddle;
        private string pGender;
        private string pSSN;
        private string pAddress1;
        private string pAddress2;
        private string pCity;
        private string pState;
        private string pZip;
        private string pPhone;
        private int pLunchType;
        private bool pAllowAlaCarte;
        private bool pCashOnly;
        private bool pisActive;
        private SmartDate pGraduationDate;
        private string pSchoolDat;
        private bool pisDeleted;
        private string pExtraInfo;
        private string pEMail;
        private SmartDate pDOB;
        private bool pACH;
        private bool pisSnack;
        private bool pisStudentWorker;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int CustomerID
        {
            get { return pCustomerID; }
            set { pCustomerID = value; }
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
        public NameValuePair Grade
        {
            get { return pGrade; }
            set { pGrade = value; }
        }
        public NameValuePair HomeRoom
        {
            get { return pHomeRoom; }
            set { pHomeRoom = value; }
        }
        public bool isStudent
        {
            get { return pisStudent; }
            set { pisStudent = value; }
        }
        public string UserID
        {
            get { return pUserID; }
            set { pUserID = value; }
        }
        public string PIN
        {
            get { return pPIN; }
            set { pPIN = value; }
        }
        public string LastName
        {
            get { return pLastName; }
            set { pLastName = value; }
        }
        public string FirstName
        {
            get { return pFirstName; }
            set { pFirstName = value; }
        }
        public string Middle
        {
            get { return pMiddle; }
            set { pMiddle = value; }
        }
        public string Gender
        {
            get { return pGender; }
            set { pGender = value; }
        }
        public string SSN
        {
            get { return pSSN; }
            set { pSSN = value; }
        }
        public string Address1
        {
            get { return pAddress1; }
            set { pAddress1 = value; }
        }
        public string Address2
        {
            get { return pAddress2; }
            set { pAddress2 = value; }
        }
        public string City
        {
            get { return pCity; }
            set { pCity = value; }
        }
        public string State
        {
            get { return pState; }
            set { pState = value; }
        }
        public string Zip
        {
            get { return pZip; }
            set { pZip = value; }
        }
        public string Phone
        {
            get { return pPhone; }
            set { pPhone = value; }
        }
        public int LunchType
        {
            get { return pLunchType; }
            set { pLunchType = value; }
        }
        public bool AllowAlaCarte
        {
            get { return pAllowAlaCarte; }
            set { pAllowAlaCarte = value; }
        }
        public bool CashOnly
        {
            get { return pCashOnly; }
            set { pCashOnly = value; }
        }
        public bool isActive
        {
            get { return pisActive; }
            set { pisActive = value; }
        }
        public SmartDate GraduationDate
        {
            get { return pGraduationDate; }
            set { pGraduationDate = value; }
        }
        public string SchoolDat
        {
            get { return pSchoolDat; }
            set { pSchoolDat = value; }
        }
        public bool isDeleted
        {
            get { return pisDeleted; }
            set { pisDeleted = value; }
        }
        public string ExtraInfo
        {
            get { return pExtraInfo; }
            set { pExtraInfo = value; }
        }
        public string EMail
        {
            get { return pEMail; }
            set { pEMail = value; }
        }
        public SmartDate DOB
        {
            get { return pDOB; }
            set { pDOB = value; }
        }
        public bool ACH
        {
            get { return pACH; }
            set { pACH = value; }
        }
        public bool isSnack 
        {
            get { return pisSnack; }
            set { pisSnack = value; }
        }
        public bool isStudentWorker 
        {
            get { return pisStudentWorker; }
            set { pisStudentWorker = value; }
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
            return this.pCustomerID.ToString();
        }

        #endregion
    }
}

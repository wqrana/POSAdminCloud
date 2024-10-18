using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct HouseHoldData
    {
        #region Private Constants and Variables

        
        private string pHouseholdID;
        private int pHHSize;
        private int pAdditionalMembers;
        private SmartDate pDateSigned;
        private string pFSNum;
        private string pAddr1;
        private string pAddr2;
        private string pCity;
        private string pState;
        private string pZip;
        private string pPhone1;
        private string pPhone2;
        private string pLanguage;
        private string pCert;
        private string pMilkOnly;
        private string pHealthDept;
        private string pTempCode;
        private SmartDate pTempCodeExpDate;
        private string pComment;
        private string pSigned_SSN;
        private string pSignedLName;
        private string pSignedFName;
        private string pSignedMI;
        private string pHHAreaCode;
        private NameValuePair pDistrict;
        private int pId;
        private string pTANF;
        private int pTempStatus;
        private bool pAppLetterSent;
        private bool pNoIncome;
        private string pMigrant;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public string HouseholdID
        {
            get { return pHouseholdID; }
            set { pHouseholdID = value; }
        }
        public int HHSize
        {
            get { return pHHSize; }
            set { pHHSize = value; }
        }
        public int AdditionalMembers
        {
            get { return pAdditionalMembers; }
            set { pAdditionalMembers = value; }
        }
        public SmartDate DateSigned
        {
            get { return pDateSigned; }
            set { pDateSigned = value; }
        }
        public string FSNum
        {
            get { return pFSNum; }
            set { pFSNum = value; }
        }
        public string Addr1
        {
            get { return pAddr1; }
            set { pAddr1 = value; }
        }
        public string Addr2
        {
            get { return pAddr2; }
            set { pAddr2 = value; }
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
        public string Phone1
        {
            get { return pPhone1; }
            set { pPhone1 = value; }
        }
        public string Phone2
        {
            get { return pPhone2; }
            set { pPhone2 = value; }
        }
        public string Language
        {
            get { return pLanguage; }
            set { pLanguage = value; }
        }
        public string Cert
        {
            get { return pCert; }
            set { pCert = value; }
        }
        public string MilkOnly
        {
            get { return pMilkOnly; }
            set { pMilkOnly = value; }
        }
        public string HealthDept
        {
            get { return pHealthDept; }
            set { pHealthDept = value; }
        }
        public string TempCode
        {
            get { return pTempCode; }
            set { pTempCode = value; }
        }
        public SmartDate TempCodeExpDate
        {
            get { return pTempCodeExpDate; }
            set { pTempCodeExpDate = value; }
        }
        public string Comment
        {
            get { return pComment; }
            set { pComment = value; }
        }
        public string Signed_SSN
        {
            get { return pSigned_SSN; }
            set { pSigned_SSN = value; }
        }
        public string SignedLName
        {
            get { return pSignedLName; }
            set { pSignedLName = value; }
        }
        public string SignedFName
        {
            get { return pSignedFName; }
            set { pSignedFName = value; }
        }
        public string SignedMI
        {
            get { return pSignedMI; }
            set { pSignedMI = value; }
        }
        public string HHAreaCode
        {
            get { return pHHAreaCode; }
            set { pHHAreaCode = value; }
        }
        public NameValuePair District
        {
            get { return pDistrict; }
            set { pDistrict = value; }
        }
        public int Id
        {
            get { return pId; }
            set { pId = value; }
        }
        public string TANF
        {
            get { return pTANF; }
            set { pTANF = value; }
        }
        public int TempStatus
        {
            get { return pTempStatus; }
            set { pTempStatus = value; }
        }
        public bool AppLetterSent
        {
            get { return pAppLetterSent; }
            set { pAppLetterSent = value; }
        }
        public bool NoIncome
        {
            get { return pNoIncome; }
            set { pNoIncome = value; }
        }
        public string Migrant
        {
            get { return pMigrant; }
            set { pMigrant = value; }
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

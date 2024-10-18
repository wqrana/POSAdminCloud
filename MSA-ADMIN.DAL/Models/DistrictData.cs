using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable()]
    public struct DistrictData
    {
        #region Private Constants and Variables
        private int pEmp_Administrator_Id;
        private int pEmp_Director_Id;
        private int pId;
        private string pDistrictName;
        private string pAddress1;
        private string pAddress2;
        private string pCity;
        private string pState;
        private string pZip;
        private string pPhone1;
        private string pPhone2;
        private bool pisDeleted;
        private string pBankCity;
        private string pBankState;
        private string pBankZip;
        private string pBankName;
        private string pBankAddr1;
        private string pBankAddr2;
        private string pBankRoute;
        private string pBankAccount;
        private object pBankMICR;
        private DistrictOptionsData[] pDistrictOptions;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 


        public int Emp_Administrator_Id
        {
            get { return pEmp_Administrator_Id; }
            set { pEmp_Administrator_Id = value; }
        }
        public int Emp_Director_Id
        {
            get { return pEmp_Director_Id; }
            set { pEmp_Director_Id = value; }
        }
        public int Id
        {
            get { return pId; }
            set { pId = value; }
        }
        public string DistrictName
        {
            get { return pDistrictName; }
            set { pDistrictName = value; }
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
        public bool isDeleted
        {
            get { return pisDeleted; }
            set { pisDeleted = value; }
        }
        public string BankCity
        {
            get { return pBankCity; }
            set { pBankCity = value; }
        }
        public string BankState
        {
            get { return pBankState; }
            set { pBankState = value; }
        }
        public string BankZip
        {
            get { return pBankZip; }
            set { pBankZip = value; }
        }
        public string BankName
        {
            get { return pBankName; }
            set { pBankName = value; }
        }
        public string BankAddr1
        {
            get { return pBankAddr1; }
            set { pBankAddr1 = value; }
        }
        public string BankAddr2
        {
            get { return pBankAddr2; }
            set { pBankAddr2 = value; }
        }
        public string BankRoute
        {
            get { return pBankRoute; }
            set { pBankRoute = value; }
        }
        public string BankAccount
        {
            get { return pBankAccount; }
            set { pBankAccount = value; }
        }
        public object BankMICR
        {
            get { return pBankMICR; }
            set { pBankMICR = value; }
        }
        public DistrictOptionsData[] DistrictOptions
        {
            get { return pDistrictOptions; }
            set { pDistrictOptions = value; }
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

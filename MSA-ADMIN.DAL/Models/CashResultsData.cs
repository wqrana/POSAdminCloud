using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct CashResultsData
    {
        #region Private Constants and Variables

        private int pCashResultsID;
        private NameValuePair pPOS;
      //  private NameValuePairCollection pEmpCashier;
        private int pEmpCashierId;
        private SmartDate pOpenDate;
        private SmartDate pCloseDate;
        private object pTotalCash;
        private object pOverShort;
        private object pAdditional;
        private object pPaidOuts;
        private object pOpenAmount;
        private object pCloseAmount;
        private object pSales;
        private bool pFinished;
        private object pOpenBlob;
        private object pCloseBlob;
       
        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public int CashResultsID
        {
            get { return pCashResultsID; }
            set { pCashResultsID = value; }
        }
        public int EmpCashierId
        {
            get { return pEmpCashierId; }
            set { pEmpCashierId = value; }
        }
        public NameValuePair POS
        {
            get { return pPOS; }
            set { pPOS = value; }
        }
        //public NameValuePairCollection EmpCashier
        //{
        //    get { return pEmpCashier; }
        //    set { pEmpCashier = value; }
        //}
        public SmartDate OpenDate
        {
            get { return pOpenDate; }
            set { pOpenDate = value; }
        }
        public SmartDate CloseDate
        {
            get { return pCloseDate; }
            set { pCloseDate = value; }
        }
        public object TotalCash
        {
            get { return pTotalCash; }
            set { pTotalCash = value; }
        }
        public object OverShort
        {
            get { return pOverShort; }
            set { pOverShort = value; }
        }
        public object Additional
        {
            get { return pAdditional; }
            set { pAdditional = value; }
        }
        public object PaidOuts
        {
            get { return pPaidOuts; }
            set { pPaidOuts = value; }
        }
        public object OpenAmount
        {
            get { return pOpenAmount; }
            set { pOpenAmount = value; }
        }
        public object CloseAmount
        {
            get { return pCloseAmount; }
            set { pCloseAmount = value; }
        }
        public object Sales
        {
            get { return pSales; }
            set { pSales = value; }
        }
        public Boolean Finished
        {
            get { return pFinished; }
            set { pFinished = value; }
        }
        public object OpenBlob
        {
            get { return pOpenBlob; }
            set { pOpenBlob = value; }
        }
        public object CloseBlob
        {
            get { return pCloseBlob; }
            set { pCloseBlob = value; }
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
            return this.pCashResultsID.ToString();
        }

        #endregion
    }
}

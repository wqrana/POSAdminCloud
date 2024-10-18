using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable()]
    public struct SchoolsData
    {
        #region Private Variables

        private int pId;
        private NameValuePair pDistrict;
        private int pEmp_Director_Id;
        private int pEmp_Administrator_Id;
        private string pSchoolId;
        private string pSchoolName;
        private string pAddress1;
        private string pAddress2;
        private string pCity;
        private string pState;
        private string pZip;
        private string pPhone1;
        private string pPhone2;
        private string pComment;
        private bool pIsSevereNeed;
        private bool pIsDeleted;

        // WaheedM [26.09.2013] 
        private bool pIsPreorderTaxable;
        private bool pIsEasyPayTaxable;

        #endregion

        #region Public Variables
        public int Id
        {
            get { return this.pId; }
            set { this.pId = value; }
        }
        public NameValuePair District
        {
            get { return this.pDistrict; }
            set { this.pDistrict = value; }
        }
        public int District_Id
        {
            get { return this.District.Value; }
            //set { this.pData.District.Value = value; }
        }
        public string DistrictName
        {
            get { return this.District.Name; }
            //set { this.pData.District.Name = value; }
        }
        public int Emp_Director_Id
        {
            get { return this.pEmp_Director_Id; }
            set { this.pEmp_Director_Id = value; }
        }
        public int Emp_Administrator_Id
        {
            get { return this.pEmp_Administrator_Id; }
            set { this.pEmp_Administrator_Id = value; }
        }
        public string SchoolId
        {
            get { return this.pSchoolId; }
            set { this.pSchoolId = value; }
        }
        public string SchoolName
        {
            get { return this.pSchoolName; }
            set { this.pSchoolName = value; }
        }
        public string Address1
        {
            get { return this.pAddress1; }
            set { this.pAddress1 = value; }
        }
        public string Address2
        {
            get { return this.pAddress2; }
            set { this.pAddress2 = value; }
        }
        public string City
        {
            get { return this.pCity; }
            set { this.pCity = value; }
        }
        public string State
        {
            get { return this.pState; }
            set { this.pState = value; }
        }
        public string Zip
        {
            get { return this.pZip; }
            set { this.pZip = value; }
        }
        public string Phone1
        {
            get { return this.pPhone1; }
            set { this.pPhone1 = value; }
        }
        public string Phone2
        {
            get { return this.pPhone2; }
            set { this.pPhone2 = value; }
        }
        public string Comment
        {
            get { return this.pComment; }
            set { this.pComment = value; }
        }
        public bool IsSevereNeed
        {
            get { return this.pIsSevereNeed; }
            set { this.pIsSevereNeed = value; }
        }
        public bool IsDeleted
        {
            get { return this.pIsDeleted; }
            set { this.pIsDeleted = value; }
        }

        // WaheedM [26.09.2013] 
        public bool IsPreorderTaxable
        {
            get { return this.pIsPreorderTaxable; }
            set { this.pIsPreorderTaxable = value; }
        }
        public bool IsEasyPayTaxable
        {
            get { return this.pIsEasyPayTaxable; }
            set { this.pIsEasyPayTaxable = value; }
        }

        #endregion

    }
}

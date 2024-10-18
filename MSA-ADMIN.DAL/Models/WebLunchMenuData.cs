using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct WebLunchMenuData
    {
        #region Private Constants and Variables
        private int pId;
        //private int pDistrictID;
        private NameValuePair pDistrict;
        private string pAbbreviation;
        private string pDescription;
        private string pCalories;
        private decimal pPrice1;
        private decimal pPrice2;
        private decimal pPrice3;
        private decimal pReducedPrice;
        private Boolean pQualifiedMeal;
        private Boolean pALaCarteSelection;
        private string pFoodServItemNumber;
       
        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        public int Id
        {
            get { return pId; }
            set { pId = value; }
        }
        //public int DistrictID
        //{
        //    get { return pDistrictID; }
        //    set { pDistrictID = value; }
        //}

        public NameValuePair District
        {
            get { return this.pDistrict; }
            set { this.pDistrict = value; }
        }

        public string Abbreviation
        {
            get { return pAbbreviation; }
            set { pAbbreviation = value; }
        }
        public string Description
        {
            get { return pDescription; }
            set { pDescription = value; }
        }
        public string Calories
        {
            get { return pCalories; }
            set { pCalories = value; }
        }
        public decimal Price1
        {
            get { return pPrice1; }
            set { pPrice1 = value; }
        }

        public decimal Price2
        {
            get { return pPrice2; }
            set { pPrice2 = value; }
        }
        public decimal Price3
        {
            get { return pPrice3; }
            set { pPrice3 = value; }
        }
        public decimal ReducedPrice
        {
            get { return pReducedPrice; }
            set { pReducedPrice = value; }
        }
        public Boolean QualifiedMeal
        {
            get { return pQualifiedMeal; }
            set { pQualifiedMeal = value; }
        }
        public Boolean ALaCarteSelection
        {
            get { return pALaCarteSelection; }
            set { pALaCarteSelection = value; }
        }
        public string FoodServItemNumber
        {
            get { return pFoodServItemNumber; }
            set { pFoodServItemNumber = value; }
        }

        public ObjectHistoryData ObjectHistory
        {
            get { return this.pObjectHistory; }
            set { this.pObjectHistory = value; }
        }

      
        public BusinessObjectState ObjectState
        {
            get { return this.pObjectState; }
            set { this.pObjectState = value; }
        }

        #endregion

        #region Object Overrides

      
        public override string ToString()
        {
            return this.Id.ToString();
        }

        #endregion
    }
}

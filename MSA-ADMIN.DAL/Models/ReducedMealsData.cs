using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public struct ReducedMealsData
    {
        #region Private Constants and Variables

        private float pLunch;
        private float pBreakfast;
        private float pSnacks;
        private int pId;
        private NameValuePair pDistrict;

        private BusinessObjectState pObjectState;
        private ObjectHistoryData pObjectHistory;

        #endregion

        #region Public Properties

        /// <summary>
        /// The ID of this property
        /// </summary>
        /// 

        public float Lunch
        {
            get { return pLunch; }
            set { pLunch = value; }
        }
        public float Breakfast
        {
            get { return pBreakfast; }
            set { pBreakfast = value; }
        }
        public float Snacks
        {
            get { return pSnacks; }
            set { pSnacks = value; }
        }
        public int Id
        {
            get { return pId; }
            set { pId = value; }
        }
        public NameValuePair District
        {
            get { return pDistrict; }
            set { pDistrict = value; }
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

using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Common
{
    /// <summary>
    /// Represents a combination of an integer ID and associated name.
    /// </summary>
    [Serializable()]
    public struct NameValuePair
    {
        #region Private Variables and Constants

        private int pValue;
        private string pName;
        private string pDescription;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Dalbey.NameValuePair structure.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The numeric value.</param>
        public NameValuePair(
            string name,
            int value)
        {
            pValue = value;
            pName = name;
            pDescription = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the Dalbey.NameValuePair structure.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The numeric value.</param>
        /// <param name="description">A description of the object.</param>
        public NameValuePair(
            string name,
            int value,
            string description)
        {
            pValue = value;
            pName = name;
            pDescription = description;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a description of the object.
        /// </summary>
        public string Description
        {
            get { return pDescription; }
            set { pDescription = value; }
        }

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        public string Name
        {
            get { return pName; }
            set { pName = value; }
        }

        /// <summary>
        /// Gets or sets the numeric value of the object.
        /// </summary>
        public int Value
        {
            get { return pValue; }
            set { pValue = value; }
        }

        #endregion
    }
}

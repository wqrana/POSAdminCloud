using MSA_ADMIN.DAL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Models
{
    /// <summary>
    /// Contains data on an item's creation and the last time it was modified.
    /// </summary>
    [Serializable()]
    public struct ObjectHistoryData
    {

        #region Private Variables and Constants

        private NameValuePair pCreateUser;
        private SmartDate pCreateDate;
        private NameValuePair pModifyUser;
        private SmartDate pModifyDate;

        #endregion

        #region Constructors

        public ObjectHistoryData(
            int createUserID,
            SmartDate createDate)
        {
            this.pCreateUser = new NameValuePair(string.Empty, createUserID);
            this.pCreateDate = createDate;
            this.pModifyUser = new NameValuePair();
            this.pModifyDate = new SmartDate();
        }

        /// <summary>
        /// Creates an ObjectHistoryData object with User IDs, but no User Names.
        /// </summary>
        /// <param name="createUserID">The ID of the user who created the item.</param>
        /// <param name="createDate">The date that the item was created.</param>
        /// <param name="modifyUserID">The ID of the user who last modified the item.</param>
        /// <param name="modifyDate">The date that the item was last modified.</param>
        public ObjectHistoryData(
            int createUserID,
            SmartDate createDate,
            int modifyUserID,
            SmartDate modifyDate)
        {
            this.pCreateUser = new NameValuePair(string.Empty, createUserID);
            this.pCreateDate = createDate;
            this.pModifyUser = new NameValuePair(string.Empty, modifyUserID);
            this.pModifyDate = modifyDate;
        }

        /// <summary>
        /// Creates an ObjectHistoryData object with User IDs and User Names.
        /// </summary>
        /// <param name="createUserID">The ID of the user who created the item.</param>
        /// <param name="createUserName">The name of the user who created the item.</param>
        /// <param name="createDate">The date that the item was created.</param>
        /// <param name="modifyUserID">The ID of the user who last modified the item.</param>
        /// <param name="modifyUserName">The name of the user who last modified the item.</param>
        /// <param name="modifyDate">The date that the item was last modified.</param>
        public ObjectHistoryData(
            int createUserID,
            string createUserName,
            SmartDate createDate,
            int modifyUserID,
            string modifyUserName,
            SmartDate modifyDate)
        {
            this.pCreateUser = new NameValuePair(createUserName, createUserID);
            this.pCreateDate = createDate;
            this.pModifyUser = new NameValuePair(modifyUserName, modifyUserID);
            this.pModifyDate = modifyDate;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The user who created the item.
        /// </summary>
        public NameValuePair CreateUser
        {
            get { return pCreateUser; }
            set { pCreateUser = value; }
        }

        /// <summary>
        /// The date that the item was created.
        /// </summary>
        public SmartDate CreateDate
        {
            get { return pCreateDate; }
            set { pCreateDate = value; }
        }

        /// <summary>
        /// The user who last modified the item.
        /// </summary>
        public NameValuePair ModifyUser
        {
            get { return pModifyUser; }
            set { pModifyUser = value; }
        }

        /// <summary>
        /// The date that the item was last modified.
        /// </summary>
        public SmartDate ModifyDate
        {
            get { return pModifyDate; }
            set { pModifyDate = value; }
        }

        #endregion

    }
}

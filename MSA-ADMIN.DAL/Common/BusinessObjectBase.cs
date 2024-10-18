using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;

namespace MSA_ADMIN.DAL.Common
{
    /// <summary>
    /// Contains basic functionality shared by all business objects.
    /// </summary>
    [Serializable()]
    public abstract class BusinessObjectBase : INotifyPropertyChanged
    {

        #region Public Properties

        /// <summary>
        /// Returns the object's internal data structure.
        /// </summary>
        public abstract object Data
        {
            get;
        }

        /// <summary>
        /// Indicates whether the object is new, deleted, modified, or unmodified.
        /// </summary>
        public abstract BusinessObjectState ObjectState
        {
            get;
            protected set;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the changed property.</param>
        protected void NotifyEventChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        /// <summary>
        /// If the new value is different the original value, the ObjectState property will  
        /// be set, and the PropertyChanged event will be triggered.
        /// </summary>
        /// <typeparam name="T">The old and new values can be of any time, but they must be the
        /// same type.</typeparam>
        /// <param name="propertyName">The name of the property being updated.</param>
        /// <param name="origValue">The original property value.</param>
        /// <param name="newValue">The new property value.</param>
        protected virtual void ProcessObjectPropertyChange<T>(string propertyName, T origValue, T newValue)
        {
            //Only perform operations if the new value is different than the original value.
            if(!newValue.Equals(origValue))
            {
                //Set the ObjectState (but only if the current state is "Unmodified").
                if (this.ObjectState == BusinessObjectState.Unmodified)
                {
                    this.ObjectState = BusinessObjectState.Modified;
                }

                //Trigger the PropertyChanged event.
                NotifyEventChange(propertyName);
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Notifies databound controls when a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}

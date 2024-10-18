using System;
using System.Collections.Generic;
using System.Text;

namespace MSA_ADMIN.DAL.Common
{
    /// <summary>
    /// Describes the state of a Business Object's data. This value is usually used to determine what kind of 
    /// operation (such as insert or update) should be performed when an object is saved.
    /// </summary>
    public enum BusinessObjectState
    {
        New,
        Unmodified,
        Modified,
        Deleted
    }
}

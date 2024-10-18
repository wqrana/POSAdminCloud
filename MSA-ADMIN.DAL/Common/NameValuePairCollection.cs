using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace MSA_ADMIN.DAL.Common
{
    [Serializable()]
    public class NameValuePairCollection : Collection<NameValuePair>
    {
        public NameValuePairCollection()
        { }

        public NameValuePairCollection(NameValuePair[] values)
        {
            foreach (NameValuePair value in values)
            {
                this.Add(value);
            }
        }
    }
}

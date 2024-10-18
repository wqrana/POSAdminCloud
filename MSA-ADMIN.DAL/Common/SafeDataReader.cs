using System;
using System.Data;
//using Common;

namespace MSA_ADMIN.DAL.Common
{
    /// <summary>
    /// This is a DataReader that 'fixes' any null values before
    /// they are returned to our business code.
    /// </summary>
    public class SafeDataReader : IDataReader
    {

        #region Private Variables and Constants

        private IDataReader pDataReader;
        private bool pDisposedValue; // To detect redundant calls

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes the SafeDataReader object to use data from
        /// the provided DataReader object.
        /// </summary>
        /// <param name="dataReader">The source DataReader object containing the data.</param>
        public SafeDataReader(IDataReader dataReader)
        {
            pDataReader = dataReader;
        }

        #endregion

        #region Finalizer

        /// <summary>
        /// Object finalizer.
        /// </summary>
        ~SafeDataReader()
        {
            Dispose(false);
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// Get a reference to the underlying data reader
        /// object that actually contains the data from
        /// the data source.
        /// </summary>
        protected IDataReader DataReader
        {
            get { return pDataReader; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Closes the datareader.
        /// </summary>
        public void Close()
        {
            pDataReader.Close();
        }

        /// <summary>
        /// Returns the depth property value from the datareader.
        /// </summary>
        public int Depth
        {
            get
            {
                return pDataReader.Depth;
            }
        }

        /// <summary>
        /// Returns the FieldCount property from the datareader.
        /// </summary>
        public int FieldCount
        {
            get
            {
                return pDataReader.FieldCount;
            }
        }

        /// <summary>
        /// Gets a boolean value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false" /> for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public bool GetBoolean(string name)
        {
            return GetBoolean(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a boolean value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns <see langword="false" /> for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual bool GetBoolean(int i)
        {
            if (pDataReader.IsDBNull(i))
                return false;
            else
                return pDataReader.GetBoolean(i);
        }

        /// <summary>
        /// Gets a byte value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public byte GetByte(string name)
        {
            return GetByte(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a byte value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual byte GetByte(int i)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetByte(i);
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public Int64 GetBytes(string name, Int64 fieldOffset,
          byte[] buffer, int bufferOffset, int length)
        {
            return GetBytes(pDataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetBytes method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public virtual Int64 GetBytes(int i, Int64 fieldOffset,
          byte[] buffer, int bufferOffset, int length)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetBytes(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Gets a char value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns Char.MinValue for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public char GetChar(string name)
        {
            return GetChar(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a char value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns Char.MinValue for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual char GetChar(int i)
        {
            if (pDataReader.IsDBNull(i))
                return char.MinValue;
            else
            {
                char[] myChar = new char[1];
                pDataReader.GetChars(i, 0, myChar, 0, 1);
                return myChar[0];
            }
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public Int64 GetChars(string name, Int64 fieldOffset,
          char[] buffer, int bufferOffset, int length)
        {
            return GetChars(pDataReader.GetOrdinal(name), fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetChars method of the underlying datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        /// <param name="buffer">Array containing the data.</param>
        /// <param name="bufferOffset">Offset position within the buffer.</param>
        /// <param name="fieldOffset">Offset position within the field.</param>
        /// <param name="length">Length of data to read.</param>
        public virtual Int64 GetChars(int i, Int64 fieldOffset,
          char[] buffer, int bufferOffset, int length)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetChars(i, fieldOffset, buffer, bufferOffset, length);
        }

        /// <summary>
        /// Invokes the GetData method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public IDataReader GetData(string name)
        {
            return GetData(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Invokes the GetData method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual IDataReader GetData(int i)
        {
            return pDataReader.GetData(i);
        }

        /// <summary>
        /// Invokes the GetDataTypeName method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public string GetDataTypeName(string name)
        {
            return GetDataTypeName(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Invokes the GetDataTypeName method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual string GetDataTypeName(int i)
        {
            return pDataReader.GetDataTypeName(i);
        }

        /// <summary>
        /// Gets a date value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns DateTime.MinValue for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public virtual DateTime GetDateTime(string name)
        {
            return GetDateTime(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a date value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns DateTime.MinValue for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual DateTime GetDateTime(int i)
        {
            if (pDataReader.IsDBNull(i))
                return DateTime.MinValue;
            else
                return pDataReader.GetDateTime(i);
        }

        /// <summary>
        /// Gets a decimal value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public decimal GetDecimal(string name)
        {
            return GetDecimal(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a decimal value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual decimal GetDecimal(int i)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetDecimal(i);
        }

        /// <summary>
        /// Gets a double from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public double GetDouble(string name)
        {
            return GetDouble(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a double from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual double GetDouble(int i)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetDouble(i);
        }

        /// <summary>
        /// Invokes the GetFieldType method of the underlying datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public Type GetFieldType(string name)
        {
            return GetFieldType(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Invokes the GetFieldType method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual Type GetFieldType(int i)
        {
            return pDataReader.GetFieldType(i);
        }

        /// <summary>
        /// Gets a Single value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public float GetFloat(string name)
        {
            return GetFloat(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Single value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual float GetFloat(int i)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetFloat(i);
        }

        /// <summary>
        /// Gets a Guid value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns Guid.Empty for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public System.Guid GetGuid(string name)
        {
            return GetGuid(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Guid value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns Guid.Empty for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual System.Guid GetGuid(int i)
        {
            if (pDataReader.IsDBNull(i))
                return Guid.Empty;
            else
                return pDataReader.GetGuid(i);
        }

        /// <summary>
        /// Gets a Short value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public short GetInt16(string name)
        {
            return GetInt16(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Short value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual short GetInt16(int i)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetInt16(i);
        }

        /// <summary>
        /// Gets an integer from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public int GetInt32(string name)
        {
            return GetInt32(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets an integer from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual int GetInt32(int i)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetInt32(i);
        }

        /// <summary>
        /// Gets a Long value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public Int64 GetInt64(string name)
        {
            return GetInt64(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a Long value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns 0 for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual Int64 GetInt64(int i)
        {
            if (pDataReader.IsDBNull(i))
                return 0;
            else
                return pDataReader.GetInt64(i);
        }

        /// <summary>
        /// Invokes the GetName method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual string GetName(int i)
        {
            return pDataReader.GetName(i);
        }

        /// <summary>
        /// Gets an ordinal value from the datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public int GetOrdinal(string name)
        {
            return pDataReader.GetOrdinal(name);
        }

        /// <summary>
        /// Invokes the GetSchemaTable method of the underlying datareader.
        /// </summary>
        public DataTable GetSchemaTable()
        {
            return pDataReader.GetSchemaTable();
        }

        /// <summary>
        /// Gets a <see cref="SmartDate" /> from the datareader.
        /// </summary>
        /// <remarks>
        /// A null is converted into min possible date
        /// See Chapter 5 for more details on the SmartDate class.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public SmartDate GetSmartDate(string name)
        {
            return GetSmartDate(pDataReader.GetOrdinal(name), true);
        }

        /// <summary>
        /// Gets a <see cref="SmartDate" /> from the datareader.
        /// </summary>
        /// <remarks>
        /// A null is converted into the min possible date
        /// See Chapter 5 for more details on the SmartDate class.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual SmartDate GetSmartDate(int i)
        {
            return GetSmartDate(i, true);
        }

        /// <summary>
        /// Gets a <see cref="SmartDate" /> from the datareader.
        /// </summary>
        /// <remarks>
        /// A null is converted into either the min or max possible date
        /// depending on the MinIsEmpty parameter. See Chapter 5 for more
        /// details on the SmartDate class.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        /// <param name="minIsEmpty">
        /// A flag indicating whether the min or max 
        /// value of a data means an empty date.</param>
        public SmartDate GetSmartDate(string name, bool minIsEmpty)
        {
            return GetSmartDate(pDataReader.GetOrdinal(name), minIsEmpty);
        }

        /// <summary>
        /// Gets a <see cref="SmartDate"/> from the datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        /// <param name="minIsEmpty">
        /// A flag indicating whether the min or max 
        /// value of a data means an empty date.</param>
        public virtual SmartDate GetSmartDate(
          int i, bool minIsEmpty)
        {
            if (pDataReader.IsDBNull(i))
                return new SmartDate();
            else
                return new SmartDate(pDataReader.GetDateTime(i));
        }

        /// <summary>
        /// Gets a string value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns empty string for null.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public string GetString(string name)
        {
            return GetString(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Gets a string value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns empty string for null.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual string GetString(int i)
        {
            if (pDataReader.IsDBNull(i))
                return string.Empty;
            else
                return pDataReader.GetString(i);
        }

        /// <summary>
        /// Gets a Uri value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns null if no value.
        /// </remarks>
        /// <param name="name">Name of the column containing the value.</param>
        public Uri GetUri( string name )
        {
            return GetUri( pDataReader.GetOrdinal( name ) );
        }

        /// <summary>
        /// Gets a Uri value from the datareader.
        /// </summary>
        /// <remarks>
        /// Returns null if no value.
        /// </remarks>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual Uri GetUri( int i )
        {
            if (pDataReader.IsDBNull( i ))
                return null;
            else
                return new Uri( pDataReader.GetString( i ) );
        }

        /// <summary>
        /// Gets a value of type <see cref="System.Object" /> from the datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public object GetValue(string name)
        {
            return GetValue(pDataReader.GetOrdinal(name));
        }

        /// <summary>
        /// Invokes the GetValues method of the underlying datareader.
        /// </summary>
        /// <param name="values">An array of System.Object to
        /// copy the values into.</param>
        public int GetValues(object[] values)
        {
            return pDataReader.GetValues(values);
        }

        /// <summary>
        /// Gets a value of type <see cref="System.Object" /> from the datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual object GetValue(int i)
        {
            if (pDataReader.IsDBNull(i))
                return null;
            else
                return pDataReader.GetValue(i);
        }

        /// <summary>
        /// Returns the IsClosed property value from the datareader.
        /// </summary>
        public bool IsClosed
        {
            get
            {
                return pDataReader.IsClosed;
            }
        }

        /// <summary>
        /// Returns the IsValidField property value from the datareader.
        /// Tried using the GetSchemaTable() method to then check the indexof the colums 
        /// or to loop column captions, the datatable returned does not contain this info...making this unreliable
        /// </summary>
        public bool IsValidField(string name)
        {
            try
            {
                int i = pDataReader.GetOrdinal(name);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Invokes the IsDBNull method of the underlying datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual bool IsDBNull(int i)
        {
            return pDataReader.IsDBNull(i);
        }

        public bool IsDBNull(string name)
        {
            return (pDataReader.IsDBNull(pDataReader.GetOrdinal(name)));
        }

        /// <summary>
        /// Reads the next row of data from the datareader.
        /// </summary>
        public bool Read()
        {
            return pDataReader.Read();
        }

        /// <summary>
        /// Moves to the next result set in the datareader.
        /// </summary>
        public bool NextResult()
        {
            return pDataReader.NextResult();
        }

        /// <summary>
        /// Returns the RecordsAffected property value from the underlying datareader.
        /// </summary>
        public int RecordsAffected
        {
            get
            {
                return pDataReader.RecordsAffected;
            }
        }

        /// <summary>
        /// Returns a value from the datareader.
        /// </summary>
        /// <param name="name">Name of the column containing the value.</param>
        public object this[string name]
        {
            get
            {
                object val = pDataReader[name];
                if (DBNull.Value.Equals(val))
                    return null;
                else
                    return val;
            }
        }

        /// <summary>
        /// Returns a value from the datareader.
        /// </summary>
        /// <param name="i">Ordinal column position of the value.</param>
        public virtual object this[int i]
        {
            get
            {
                if (pDataReader.IsDBNull(i))
                    return null;
                else
                    return pDataReader[i];
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">True if called by
        /// the public Dispose method.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!pDisposedValue)
            {
                if (disposing)
                {
                    // free unmanaged resources when explicitly called
                    pDataReader.Dispose();
                }

                // free shared unmanaged resources
            }
            pDisposedValue = true;
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
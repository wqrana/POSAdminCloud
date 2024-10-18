using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MSA_ADMIN.DAL.Common
{
    /// <summary>
    /// Represents a DateTime that supports database null values.
    /// </summary>
    [Serializable()]
    public struct SmartDate : IComparable
    {

        #region Private Variables and Constants

        private DateTime pDate;
        private bool pInitialized;
        private string pFormat;
        private static string pDefaultFormat = "d";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        /// <remarks>
        /// The SmartDate created will use the min possible
        /// date to represent an empty date.
        /// </remarks>
        /// <param name="value">The initial value of the object.</param>
        public SmartDate(DateTime value)
        {
            pFormat = null;
            pInitialized = false;
            pDate = DateTime.MinValue;
            Date = value;
        }

        /// <summary>
        /// Creates a new SmartDate object.
        /// </summary>
        /// <remarks>
        /// The SmartDate created will use the min possible
        /// date to represent an empty date.
        /// </remarks>
        /// <param name="value">The initial value of the object (as text).</param>
        public SmartDate(string value)
        {
            pFormat = null;
            pInitialized = true;
            pDate = DateTime.MinValue;
            this.Text = value;
        }

        public SmartDate(DataRow row, DataColumn column)
        {
            if (row == null)
            {
                throw new ArgumentNullException("row", @"Argument ""row"" cannot be null.");
            }

            if (column == null)
            {
                throw new ArgumentNullException("column", @"Argument ""column"" cannot be null.");
            }

            pFormat = null;

            if (row.IsNull(column))
            {
                pInitialized = false;
                pDate = DateTime.MinValue;
            }
            else
            {
                pInitialized = true;
                pDate = (DateTime)row[column];
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        public DateTime Date
        {
            get
            {
                if (!pInitialized)
                {
                    pDate = DateTime.MinValue;
                    pInitialized = true;
                }
                return pDate;
            }
            set
            {
                pDate = value;
                pInitialized = true;
            }
        }

        /// <summary>
        /// Gets a database-friendly version of the date value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If the SmartDate contains an empty date, this returns <see cref="DBNull"/>.
        /// Otherwise the actual date value is returned as type Date.
        /// </para><para>
        /// This property is very useful when setting parameter values for
        /// a Command object, since it automatically stores null values into
        /// the database for empty date values.
        /// </para><para>
        /// When you also use the SafeDataReader and its GetSmartDate method,
        /// you can easily read a null value from the database back into a
        /// SmartDate object so it remains considered as an empty date value.
        /// </para>
        /// </remarks>
        public object DBValue
        {
            get
            {
                if (this.IsEmpty)
                    return DBNull.Value;
                else
                    return this.Date;
            }
        }

        /// <summary>
        /// Gets or sets the format string used to format a date
        /// value when it is returned as text.
        /// </summary>
        /// <remarks>
        /// The format string should follow the requirements for the
        /// .NET System.String.Format statement.
        /// </remarks>
        /// <value>A format string.</value>
        public string FormatString
        {
            get
            {
                if (pFormat == null)
                    pFormat = pDefaultFormat;
                return pFormat;
            }
            set
            {
                pFormat = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this object contains an empty date.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.Date.Equals(DateTime.MinValue);
            }
        }

        /// <summary>
        /// Gets or sets the date value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property can be used to set the date value by passing a
        /// text representation of the date. Any text date representation
        /// that can be parsed by the .NET runtime is valid.
        /// </para><para>
        /// When the date value is retrieved via this property, the text
        /// is formatted by using the format specified by the 
        /// <see cref="FormatString" /> property. The default is the
        /// short date format (d).
        /// </para>
        /// </remarks>
        public string Text
        {
            get { return DateToString(this.Date, FormatString); }
            set { this.Date = StringToDate(value); }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a TimeSpan onto the object.
        /// </summary>
        /// <param name="value">Span to add to the date.</param>
        public DateTime Add(TimeSpan value)
        {
            if (IsEmpty)
                return this.Date;
            else
                return this.Date.Add(value);
        }

        /// <summary>
        /// Converts a date value into a text representation.
        /// </summary>
        /// <remarks>
        /// Whether the date value is considered empty is determined by
        /// the EmptyIsMin parameter value. If the date is empty, this
        /// method returns an empty string. Otherwise it returns the date
        /// value formatted based on the FormatString parameter.
        /// </remarks>
        /// <param name="value">The date value to convert.</param>
        /// <param name="formatString">The format string used to format the date into text.</param>
        /// <param name="emptyValue">Indicates whether an empty date is the min or max date value.</param>
        /// <returns>Text representation of the date value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "1#")]
        public static string DateToString(DateTime value, string formatString)
        {
            if (value == DateTime.MinValue)
                return string.Empty;

            return string.Format("{0:" + formatString + "}", value);
        }

        /// <summary>
        /// Converts a string value into a SmartDate.
        /// </summary>
        /// <param name="value">String containing the date value.</param>
        /// <returns>A new SmartDate containing the date value.</returns>
        /// <remarks>
        /// EmptyIsMin will default to <see langword="true"/>.
        /// </remarks>
        public static SmartDate Parse(string value)
        {
            return new SmartDate(value);
        }

        /// <summary>
        /// Sets the global default format string used by all new
        /// SmartDate values going forward.
        /// </summary>
        /// <remarks>
        /// The default global format string is "d" unless this
        /// method is called to change that value. Existing SmartDate
        /// values are unaffected by this method, only SmartDate
        /// values created after calling this method are affected.
        /// </remarks>
        /// <param name="formatString">
        /// The format string should follow the requirements for the
        /// .NET System.String.Format statement.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", MessageId = "0#")]
        public static void SetDefaultFormatString(string formatString)
        {
            pDefaultFormat = formatString;
        }

        /// <summary>
        /// Converts a text date representation into a Date value.
        /// </summary>
        /// <remarks>
        /// An empty string is assumed to represent an empty date. An empty date
        /// is returned as the MinValue or MaxValue of the Date datatype depending
        /// on the EmptyIsMin parameter.
        /// </remarks>
        /// <param name="value">The text representation of the date.</param>
        /// <param name="emptyValue">Indicates whether an empty date is the min or max date value.</param>
        /// <returns>A Date value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation", MessageId = "stack0")]
        public static DateTime StringToDate(string value)
        {
            DateTime tmp;
            if (string.IsNullOrEmpty(value))
            {
                return DateTime.MinValue;
            }
            else if (DateTime.TryParse(value, out tmp))
                return tmp;
            else
            {
                string ldate = value.Trim().ToLower();
                if (ldate == "t" ||
                    ldate == "today" ||
                    ldate == ".")
                    return DateTime.Now;
                if (ldate == "y" ||
                    ldate == "yesterday" ||
                    ldate == "-")
                    return DateTime.Now.AddDays(-1);
                if (ldate == "tom" ||
                    ldate == "tomorrow" ||
                    ldate == "+")
                    return DateTime.Now.AddDays(1);
                throw new ArgumentException("String value can not be converted to a date");
            }
        }

        /// <summary>
        /// Subtracts a TimeSpan from the object.
        /// </summary>
        /// <param name="value">Span to subtract from the date.</param>
        public DateTime Subtract(TimeSpan value)
        {
            if (IsEmpty)
                return this.Date;
            else
                return this.Date.Subtract(value);
        }

        /// <summary>
        /// Subtracts a DateTime from the object.
        /// </summary>
        /// <param name="value">Date to subtract from the date.</param>
        public TimeSpan Subtract(DateTime value)
        {
            if (IsEmpty)
                return TimeSpan.Zero;
            else
                return this.Date.Subtract(value);
        }

        #endregion

        #region Object overrides

        /// <summary>
        /// Returns a text representation of the date value.
        /// </summary>
        public override string ToString()
        {
            return this.Text;
        }

        /// <summary>
        /// Returns a text representation of the date value.
        /// </summary>
        /// <param name="format">
        /// A standard .NET format string.
        /// </param>
        public string ToString(string format)
        {
            return DateToString(this.Date, format);
        }

        /// <summary>
        /// Compares this object to another <see cref="SmartDate"/>
        /// for equality.
        /// </summary>
        /// <param name="obj">Object to compare for equality.</param>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj", @"Argument ""obj"" cannot be null.");
            }

            if (obj is SmartDate)
            {
                SmartDate tmp = (SmartDate)obj;
                if (this.IsEmpty && tmp.IsEmpty)
                    return true;
                else
                    return this.Date.Equals(tmp.Date);
            }
            else if (obj is DateTime)
                return this.Date.Equals((DateTime)obj);
            else if (obj is string)
                return (this.CompareTo(obj.ToString()) == 0);
            else
                return false;
        }

        /// <summary>
        /// Returns a hash code for this object.
        /// </summary>
        public override int GetHashCode()
        {
            return this.Date.GetHashCode();
        }

        #endregion

        #region IComparable Members

        /// <summary>
        /// Compares one SmartDate to another.
        /// </summary>
        /// <remarks>
        /// This method works the same as the DateTime.CompareTo method
        /// on the Date datetype, with the exception that it
        /// understands the concept of empty date values.
        /// </remarks>
        /// <param name="value">The date to which we are being compared.</param>
        /// <returns>A value indicating if the comparison date is less than, equal to or greater than this date.</returns>
        public int CompareTo(SmartDate obj)
        {
            if (this.IsEmpty && obj.IsEmpty)
                return 0;
            else
                return pDate.CompareTo(obj.Date);
        }

        /// <summary>
        /// Compares one SmartDate to another.
        /// </summary>
        /// <remarks>
        /// This method works the same as the DateTime.CompareTo method
        /// on the Date datetype, with the exception that it
        /// understands the concept of empty date values.
        /// </remarks>
        /// <param name="value">The date to which we are being compared.</param>
        /// <returns>A value indicating if the comparison date is less than, equal to or greater than this date.</returns>
        int IComparable.CompareTo(object obj)
        {
            if (obj is SmartDate)
                return CompareTo((SmartDate)obj);
            else
                throw new ArgumentException("Value is not a SmartDate");
        }

        /// <summary>
        /// Compares a SmartDate to a text date value.
        /// </summary>
        /// <param name="value">The date to which we are being compared.</param>
        /// <returns>A value indicating if the comparison date is less than, equal to or greater than this date.</returns>
        public int CompareTo(string value)
        {
            return this.Date.CompareTo(StringToDate(value));
        }

        /// <summary>
        /// Compares a SmartDate to a date value.
        /// </summary>
        /// <param name="value">The date to which we are being compared.</param>
        /// <returns>A value indicating if the comparison date is less than, equal to or greater than this date.</returns>
        public int CompareTo(DateTime value)
        {
            return this.Date.CompareTo(value);
        }

        #endregion

        #region Operators

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator ==(SmartDate obj1, SmartDate obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator !=(SmartDate obj1, SmartDate obj2)
        {
            return !obj1.Equals(obj2);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator ==(SmartDate obj1, DateTime obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator !=(SmartDate obj1, DateTime obj2)
        {
            return !obj1.Equals(obj2);
        }

        /// <summary>
        /// Equality operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator ==(SmartDate obj1, string obj2)
        {
            return obj1.Equals(obj2);
        }

        /// <summary>
        /// Inequality operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator !=(SmartDate obj1, string obj2)
        {
            return !obj1.Equals(obj2);
        }

        /// <summary>
        /// Addition operator
        /// </summary>
        /// <param name="start">Original date/time</param>
        /// <param name="span">Span to add</param>
        /// <returns></returns>
        public static SmartDate operator +(SmartDate start, TimeSpan span)
        {
            return new SmartDate(start.Add(span));
        }

        /// <summary>
        /// Subtraction operator
        /// </summary>
        /// <param name="start">Original date/time</param>
        /// <param name="span">Span to subtract</param>
        /// <returns></returns>
        public static SmartDate operator -(SmartDate start, TimeSpan span)
        {
            return new SmartDate(start.Subtract(span));
        }

        /// <summary>
        /// Subtraction operator
        /// </summary>
        /// <param name="start">Original date/time</param>
        /// <param name="finish">Second date/time</param>
        /// <returns></returns>
        public static TimeSpan operator -(SmartDate start, SmartDate finish)
        {
            return start.Subtract(finish.Date);
        }

        /// <summary>
        /// Greater than operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator >(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator <(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }

        /// <summary>
        /// Greater than operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator >(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator <(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }

        /// <summary>
        /// Greater than operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator >(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) > 0;
        }

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator <(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) < 0;
        }

        /// <summary>
        /// Greater than or equals operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator >=(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }

        /// <summary>
        /// Less than or equals operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator <=(SmartDate obj1, SmartDate obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }

        /// <summary>
        /// Greater than or equals operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator >=(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }

        /// <summary>
        /// Less than or equals operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator <=(SmartDate obj1, DateTime obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }

        /// <summary>
        /// Greater than or equals operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator >=(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) >= 0;
        }

        /// <summary>
        /// Less than or equals operator
        /// </summary>
        /// <param name="obj1">First object</param>
        /// <param name="obj2">Second object</param>
        /// <returns></returns>
        public static bool operator <=(SmartDate obj1, string obj2)
        {
            return obj1.CompareTo(obj2) <= 0;
        }

        #endregion

    }
}

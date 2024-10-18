using System;
using System.Data;
using System.Data.Common;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
//using Common;

//TODO: Add trace logging for performance.
namespace MSA_ADMIN.DAL.Common
{
    /// <summary>
    /// Encapsulates database access functions.
    /// </summary>
    public class DataPortal : IDisposable
    {

        #region Private Variables and Constants

        private string pConnectionString;
        private List<SqlParameter> pParameters = new List<SqlParameter>();
        private Dictionary<string, object> pOutputValues = new Dictionary<string, object>();
        private SqlConnection pConn;
        private static int sqlMaxRetries = 4;
        private static int sqlRetrySleep = 100;
        private static int sqlMaxSleep = 5000;
        private static int sqlMinSleep = 10;

        public enum QueryType { QueryString, StoredProc }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the Dalbey.Data.DataPortal class.
        /// </summary>
        public DataPortal()
        {
            this.pConnectionString = ConfigurationManager.ConnectionStrings["MSAAdminConnectionString"].ToString();
        }

        /// <summary>
        /// Initializes a new instance of the Dalbey.Data.DataPortal class using the 
        /// specified connection string.
        /// </summary>
        public DataPortal(string connectionStringName)
        {
            this.pConnectionString =
                ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        }

        #endregion

        #region Private Methods

        //Adds a parameter to the parameter collection.
        private void AddParameter(string paramName, SqlDbType dataType, object value, bool isOutput)
        {
            paramName = paramName.Trim();

            //Make sure the parameter name begins with "@".
            if (!paramName.StartsWith("@"))
                paramName = "@" + paramName;

            SqlParameter param = new SqlParameter(paramName, dataType);

            //Convert null to DBNull.
            if (value != null)
            {
                param.Value = value;
            }
            else
            {
                param.Value = DBNull.Value;
            }

            //Set the Direction for output parameters.
            if (isOutput)
                param.Direction = ParameterDirection.Output;

            pParameters.Add(param);
        }

        private void AddParameterDataTable(string paramName, DataTable dt)
        {
            paramName = paramName.Trim();


            var AssignedSchools = new SqlParameter(paramName, SqlDbType.Structured);
            if (dt.Rows.Count > 0)
            {
                AssignedSchools.Value = dt;
            }
            else
            {
                AssignedSchools.Value = null;
            }
            AssignedSchools.TypeName = "dbo.CalAssignedSchools";

            pParameters.Add(AssignedSchools);
        }
        private void AddSelecteddatesDataTableParam(string paramName, DataTable dt)
        {
            paramName = paramName.Trim();


            var selectedDates = new SqlParameter(paramName, SqlDbType.Structured);
            if (dt.Rows.Count > 0)
            {
                selectedDates.Value = dt;
            }
            else
            {
                selectedDates.Value = null;
            }
            selectedDates.TypeName = "dbo.SelectedCalDates";

            pParameters.Add(selectedDates);
        }



        private void AddParameterDataTableWebCalItems(string paramName, DataTable dt)
        {
            paramName = paramName.Trim();


            var WebCalItemsList = new SqlParameter(paramName, SqlDbType.Structured);
            if (dt.Rows.Count > 0)
            {
                WebCalItemsList.Value = dt;
            }
            else
            {
                WebCalItemsList.Value = null;
            }
            WebCalItemsList.TypeName = "dbo.WebCalItems";

            pParameters.Add(WebCalItemsList);
        }



        //Saves output parameter values for future use.
        private void SaveOutputValues()
        {
            pOutputValues.Clear();

            foreach (SqlParameter param in pParameters)
            {
                if (param.Direction == ParameterDirection.Output)
                {
                    pOutputValues[param.ParameterName.Trim().ToLower()] = param.Value;
                }
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a bool-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddBoolParameter(string paramName, bool paramValue)
        {
            AddParameter(paramName, SqlDbType.Bit, paramValue, false);
        }

        /// <summary>
        /// Adds a bool-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddBoolParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.Bit, paramValue, false);
        }

        /// <summary>
        /// Adds a bool-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddBoolParameter(string paramName, bool paramValue, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.Bit, paramValue, isOutput);
        }

        /// <summary>
        /// Adds a DateTime-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddDateParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.DateTime, null, isOutput);
        }

        /// <summary>
        /// Adds a DateTime-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddDateParameter(string paramName, DateTime paramValue)
        {
            AddParameter(paramName, SqlDbType.DateTime, paramValue, false);
        }

        /// <summary>
        /// Adds a DateTime-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddDateParameter(string paramName, DateTime? paramValue)
        {
            AddParameter(paramName, SqlDbType.DateTime, paramValue, false);
        }

        /// <summary>
        /// Adds a DateTime-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddDateParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.DateTime, paramValue, false);
        }

        /// <summary>
        /// Adds a DateTime-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">A SmartDate structure containing the value of the parameter.</param>
        public void AddDateParameter(string paramName, SmartDate paramValue)
        {
            AddParameter(paramName, SqlDbType.DateTime, paramValue.DBValue, false);
        }

        /// <summary>
        /// Adds a decimal-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddDecimalParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.Decimal, null, isOutput);
        }

        /// <summary>
        /// Adds a decimal-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddDecimalParameter(string paramName, decimal paramValue)
        {
            AddParameter(paramName, SqlDbType.Decimal, paramValue, false);
        }

        /// <summary>
        /// Adds a decimal-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddDecimalParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.Decimal, paramValue, false);
        }

        /// <summary>
        /// Adds a float-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddFloatParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.Float, null, isOutput);
        }

        /// <summary>
        /// Adds a float-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddFloatParameter(string paramName, float paramValue)
        {
            AddParameter(paramName, SqlDbType.Float, paramValue, false);
        }

        /// <summary>
        /// Adds a float-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddFloatParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.Float, paramValue, false);
        }

        /// <summary>
        /// Adds a Guid-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddGuidParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.UniqueIdentifier, null, isOutput);
        }

        /// <summary>
        /// Adds a Guid-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddGuidParameter(string paramName, Guid paramValue)
        {
            AddParameter(paramName, SqlDbType.UniqueIdentifier, paramValue, false);
        }

        /// <summary>
        /// Adds a Guid-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddGuidParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.UniqueIdentifier, paramValue, false);
        }

        /// <summary>
        /// Adds a binary (Image-type) parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddImageParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.Image, null, isOutput);
        }

        /// <summary>
        /// Adds a binary (Image-type) parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddImageParameter(string paramName, byte[] paramValue)
        {
            AddParameter(paramName, SqlDbType.Image, paramValue, false);
        }

        /// <summary>
        /// Adds a binary (Image-type) parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddImageParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.Image, paramValue, false);
        }

        /// <summary>
        /// Adds a int-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddIntParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.Int, null, isOutput);
        }

        /// <summary>
        /// Adds a int-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddIntParameter(string paramName, int paramValue)
        {
            AddParameter(paramName, SqlDbType.Int, paramValue, false);
        }

        /// <summary>
        /// Adds a int-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddIntParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.Int, paramValue, false);
        }

        /// <summary>
        /// Adds a long-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddLongParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.BigInt, null, isOutput);
        }

        /// <summary>
        /// Adds a long-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddLongParameter(string paramName, long paramValue)
        {
            AddParameter(paramName, SqlDbType.BigInt, paramValue, false);
        }

        /// <summary>
        /// Adds a long-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddLongParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.BigInt, paramValue, false);
        }

        /// <summary>
        /// Adds a string-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddStringParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.VarChar, null, isOutput);
        }

        /// <summary>
        /// Adds a string-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddStringParameter(string paramName, string paramValue)
        {
            AddParameter(paramName, SqlDbType.VarChar, paramValue, false);
        }

        /// <summary>
        /// Adds a string-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddStringParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.VarChar, paramValue, false);
        }

        /// <summary>
        /// Adds a long string-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddTextParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.Text, null, isOutput);
        }

        /// <summary>
        /// Adds a long string-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddTextParameter(string paramName, string paramValue)
        {
            AddParameter(paramName, SqlDbType.Text, paramValue, false);
        }

        /// <summary>
        /// Adds a long string-type parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddTextParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.Text, paramValue, false);
        }

        /// <summary>
        /// Adds a Uri (treated as varchar) parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="isOutput">Indicates whether the parameter is an output parameter.</param>
        public void AddUriParameter(string paramName, bool isOutput)
        {
            AddParameter(paramName, SqlDbType.VarChar, null, isOutput);
        }

        /// <summary>
        /// Adds a Uri (treated as varchar) parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddUriParameter(string paramName, Uri paramValue)
        {
            AddParameter(paramName, SqlDbType.VarChar, paramValue.ToString(), false);
        }

        /// <summary>
        /// Adds a Uri (treated as varchar) parameter to the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="paramValue">The value of the parameter.</param>
        public void AddUriParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.VarChar, paramValue, false);
        }

        public void AddTimeParameter(string paramName, string paramValue)
        {
            AddParameter(paramName, SqlDbType.Time, paramValue, false);
        }
        /// <summary>
        /// Adds datatable as a parameter to the parameters collection.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddDataTableParameter(string paramName, DataTable paramValue)
        {
            AddParameter(paramName, SqlDbType.Structured, paramValue, false);
        }
        /// <summary>
        /// Adds datatable as a parameter to the parameters collection.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="paramValue"></param>
        public void AddDataTableParameter(string paramName, DBNull paramValue)
        {
            AddParameter(paramName, SqlDbType.Structured, paramValue, false);
        }

        public void AddAssignedSchsDataTableParameter(string paramName, DataTable paramValue)
        {
            AddParameterDataTable(paramName, paramValue);
        }
        public void AddSelecteddatesDataTable(string paramName, DataTable paramValue)
        {
            AddSelecteddatesDataTableParam(paramName, paramValue);
        }



        public void AddDataTableParameterWebCalItem(string paramName, DataTable paramValue)
        {
            AddParameterDataTableWebCalItems(paramName, paramValue);
        }

        /// <summary>
        /// Clears the parameter collection.
        /// </summary>
        public void ClearParameters()
        {
            this.pParameters.Clear();
        }

        /// <summary>
        /// Retrieves a value from a column in a DataRow. If the value is DBNull, a default value is returned.
        /// </summary>
        /// <param name="row">The DataRow containing the value to be returned.</param>
        /// <param name="column">The DataColumn containing the value to be returned.</param>
        /// <returns>The value contained in the DataRow, or false if it is DBNull.</returns>
        public static bool DbNullToBool(DataRow row, DataColumn column)
        {
            if (row.IsNull(column))
            {
                return false;
            }
            else
            {
                return (bool)row[column];
            }
        }

        /// <summary>
        /// Retrieves a value from a column in a DataRow. If the value is DBNull, a default value is returned.
        /// </summary>
        /// <param name="row">The DataRow containing the value to be returned.</param>
        /// <param name="column">The DataColumn containing the value to be returned.</param>
        /// <returns>The value contained in the DataRow, or zero if it is DBNull.</returns>
        public static decimal DbNullToDecimal(DataRow row, DataColumn column)
        {
            if (row.IsNull(column))
            {
                return 0;
            }
            else
            {
                return (decimal)row[column];
            }
        }

        /// <summary>
        /// Retrieves a value from a column in a DataRow. If the value is DBNull, a default value is returned.
        /// </summary>
        /// <param name="row">The DataRow containing the value to be returned.</param>
        /// <param name="column">The DataColumn containing the value to be returned.</param>
        /// <returns>The value contained in the DataRow, or zero if it is DBNull.</returns>
        public static int DbNullToInt(DataRow row, DataColumn column)
        {
            if (row.IsNull(column))
            {
                return 0;
            }
            else
            {
                return (int)row[column];
            }
        }

        /// <summary>
        /// Retrieves a value from a column in a DataRow. If the value is DBNull, a default value is returned.
        /// </summary>
        /// <param name="row">The DataRow containing the value to be returned.</param>
        /// <param name="column">The DataColumn containing the value to be returned.</param>
        /// <returns>The value contained in the DataRow, or zero if it is DBNull.</returns>
        public static long DbNullToLong(DataRow row, DataColumn column)
        {
            if (row.IsNull(column))
            {
                return 0;
            }
            else
            {
                return (long)row[column];
            }
        }

        /// <summary>
        /// Retrieves a value from a column in a DataRow. If the value is DBNull, a default value is returned.
        /// </summary>
        /// <param name="row">The DataRow containing the value to be returned.</param>
        /// <param name="column">The DataColumn containing the value to be returned.</param>
        /// <returns>The value contained in the DataRow, or a blank string if it is DBNull.</returns>
        public static string DbNullToString(DataRow row, DataColumn column)
        {
            if (row.IsNull(column))
            {
                return string.Empty;
            }
            else
            {
                return row[column].ToString();
            }
        }

        /// <summary>
        /// Fills a DataSet using a specified query.
        /// </summary>
        /// <param name="queryText">The query to use to fill the DataSet</param>
        /// <param name="queryType">Indicates whether the query string is an SQL query or the name of a 
        /// stored procedure.</param>
        /// <param name="ds">The DataSet to be filled.</param>
        public void FillDataSet(string queryText, QueryType queryType, DataSet ds)
        {
            FillDataSet(queryText, queryType, ds, false);
        }

        /// <summary>
        /// Fills a DataSet using a specified query.
        /// </summary>
        /// <param name="queryText">The query to use to fill the DataSet</param>
        /// <param name="queryType">Indicates whether the query string is an SQL query or the name of a 
        /// stored procedure.</param>
        /// <param name="ds">The DataSet to be filled.</param>
        /// <param name="preserveParameters">Indicates whether the parameter collection should be cleared
        /// after the query is executed.</param>
        public void FillDataSet(string queryText, QueryType queryType, DataSet ds, bool preserveParameters)
        {
            bool retValue = false;
            if (string.IsNullOrEmpty(queryText))
            {
                throw new ArgumentException("Query text must be supplied.", "queryText");
            }

            if (ds == null)
            {
                throw new ArgumentException("An instantiated DataSet must be supplied.", "ds");
            }

            //Initialize the connection, command, and data adapter.
            if (this.pConn == null)
            {
                this.pConn = new SqlConnection(this.pConnectionString);
            }

            SqlCommand cmd = new SqlCommand(queryText, this.pConn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            if (queryType == QueryType.StoredProc)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }

            //Add All SQL Parameters
            if (pParameters.Count > 0)
            {
                cmd.Parameters.AddRange(pParameters.ToArray());
            }

            //Add table mappings for typed datasets.
            if (ds.Tables.Count > 0)
            {
                int tableIndex = 0;

                foreach (DataTable table in ds.Tables)
                {
                    string sourceName;

                    //The first table is called "Table" and subsequent tables add an ascending number.
                    if (tableIndex == 0)
                    {
                        sourceName = "Table";
                    }
                    else
                    {
                        sourceName = "Table" + tableIndex.ToString();
                    }

                    da.TableMappings.Add(sourceName, table.TableName);

                    tableIndex++;
                }
            }

            try
            {

                for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
                {

                    try
                    {
                        this.pConn.Open();
                        //retrieve data or add data into table                
                        da.Fill(ds);
                        retValue = true;

                    }
                    catch (Exception e)
                    {
                        if (retryCount == 1)
                        {
                            System.Data.SqlClient.SqlConnection.ClearPool(this.pConn);
                        }

                        if (retryCount < sqlMaxRetries)
                        {
                            //We can log retryCount at this point;

                            // don't sleep on the first retry
                            // Most SQL Azure retries work on the first retry with no sleep
                            if (retryCount > 1)
                            {
                                // wait longer between each retry
                                int sleep = retryCount * retryCount * sqlRetrySleep;

                                // limit to the min and max retry values
                                if (sleep > sqlMaxSleep)
                                {
                                    sleep = sqlMaxSleep;
                                }
                                else if (sleep < sqlMinSleep)
                                {
                                    sleep = sqlMinSleep;
                                }

                                // sleep
                                System.Threading.Thread.Sleep(sleep);
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(sqlRetrySleep);
                        }

                    }
                    finally
                    {
                        if (this.pConn.State != System.Data.ConnectionState.Closed)
                        {
                            this.pConn.Close();
                        }
                    }
                    if (retValue)
                    {
                        break;
                    }
                }
                //this.pConn.Open();

                ////retrieve data or add data into table                
                //da.Fill(ds);
            }
            catch (SqlException e)
            {
                throw;
                //ds.Clear();
                //throw new DBException("A SQL exception occurred.", "ReturnDataSet", e);
            }
            catch (Exception ex)
            {
                throw;
                //ds.Clear();
                //throw new DBException(ex.Message, ex.Source, ex);
            }
            finally
            {
                //Dispose the command.
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                //Dispose the data adapter.
                if (da != null)
                {
                    da.Dispose();
                    da = null;
                }

                //Close the connection; if 
                if (this.pConn != null)
                {
                    this.pConn.Close();
                    if (!preserveParameters)
                    {
                        this.pConn.Dispose();
                        this.pConn = null;
                    }
                }

                //Clear the parameter collection if preserveParameters is false.
                if (!preserveParameters)
                {
                    this.ClearParameters();
                }
            }
        }

        /// <summary>
        /// Returns a DataReader, using a specified query.
        /// </summary>
        /// <param name="queryText">The query to execute to get the DataReader.</param>
        /// <param name="queryType">Indicates whether the query string is an SQL query or the name of a 
        /// stored procedure.</param>
        /// <returns>The DataReader resulting from the execution of the query.</returns>
        public SafeDataReader GetDataReader(string queryText, QueryType queryType)
        {
            return GetDataReader(queryText, queryType, false);
        }

        /// <summary>
        /// Returns a DataReader, using a specified query.
        /// </summary>
        /// <param name="queryText">The query to execute to get the DataReader.</param>
        /// <param name="queryType">Indicates whether the query string is an SQL query or the name of a 
        /// stored procedure.</param>
        /// <param name="preserveParameters">Indicates whether the parameter collection should be cleared
        /// after the query is executed.</param>
        /// <returns>The DataReader resulting from the execution of the query.</returns>
        public SafeDataReader GetDataReader(
            string queryText,
            QueryType queryType,
            bool preserveParameters)
        {
            //Initialize the connection, command, and data reader.
            SqlConnection con = new SqlConnection(this.pConnectionString);
            SqlCommand cmd = new SqlCommand(queryText, con);
            SqlDataReader reader = null;

            if (queryType == QueryType.StoredProc)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }

            //Add All SQL Parameters
            if (pParameters.Count > 0)
            {
                cmd.Parameters.AddRange(pParameters.ToArray());
            }

            try
            {
                for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
                {

                    try
                    {
                        con.Open();
                        reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                    catch (Exception e)
                    {
                        if (retryCount == 1)
                        {
                            System.Data.SqlClient.SqlConnection.ClearPool(con);
                        }

                        if (retryCount < sqlMaxRetries)
                        {
                            //We can log retryCount at this point;

                            // don't sleep on the first retry
                            // Most SQL Azure retries work on the first retry with no sleep
                            if (retryCount > 1)
                            {
                                // wait longer between each retry
                                int sleep = retryCount * retryCount * sqlRetrySleep;

                                // limit to the min and max retry values
                                if (sleep > sqlMaxSleep)
                                {
                                    sleep = sqlMaxSleep;
                                }
                                else if (sleep < sqlMinSleep)
                                {
                                    sleep = sqlMinSleep;
                                }

                                // sleep
                                System.Threading.Thread.Sleep(sleep);
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(sqlRetrySleep);
                        }

                    }
                    finally
                    {
                        
                    }
                    if (reader != null)
                    {
                        break;
                    }
                }


            }
            catch (Exception ex)
            {
                throw;// new DBException(ex.Message, "ReturnDataReader", ex);
            }
            finally
            {
                //Dispose the command.
                if (cmd != null)
                {
                    cmd.Dispose();
                    cmd = null;
                }

                //Clear the parameter collection if preserveParameters is false.
                if (!preserveParameters)
                {
                    this.ClearParameters();
                }
            }

            return new SafeDataReader(reader);
        }

        /// <summary>
        /// Executes a non-query statement against the database.
        /// </summary>
        /// <param name="queryText">The statement to execute.</param>
        /// <param name="queryType">Indicates whether the statement is an SQL statement or the name of a 
        /// stored procedure.</param>
        /// <returns>The number of rows affected by the query.</returns>
        //public int SubmitData1(string queryText, QueryType queryType)
        //{
        //    return SubmitData1(queryText, queryType, false);
        //}
        public int SubmitData(string queryText, QueryType queryType)
        {
            return SubmitData(queryText, queryType, false);
        }

        /// <summary>
        /// Executes a non-query statement against the database.
        /// </summary>
        /// <param name="queryText">The statement to execute.</param>
        /// <param name="queryType">Indicates whether the statement is an SQL statement or the name of a 
        /// stored procedure.</param>
        /// <param name="preserveParameters">Indicates whether the parameter collection should be cleared
        /// after the query is executed.</param>
        /// <returns>The number of rows affected by the query.</returns>
        /// 
        public int SubmitData(string queryText, QueryType queryType, bool preserveParameters)
        {
            bool CommandExecuted = false;
            //Initialize the connection and command.
            if (this.pConn == null)
            {
                this.pConn = new SqlConnection(this.pConnectionString);
            }

            SqlCommand cmd = new SqlCommand(queryText, this.pConn);

            int rowCount = 0;

            if (queryType == QueryType.StoredProc)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }

            //Add All SQL Parameters.
            if (pParameters.Count > 0)
            {
                cmd.Parameters.AddRange(pParameters.ToArray());
            }

            try
            {
                for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
                {

                    try
                    {
                        this.pConn.Open();
                        rowCount = cmd.ExecuteNonQuery();
                        CommandExecuted = true;
                    }
                    catch (Exception e)
                    {
                        if (retryCount == 1)
                        {
                            System.Data.SqlClient.SqlConnection.ClearPool(this.pConn);
                        }

                        if (retryCount < sqlMaxRetries)
                        {
                            //We can log retryCount at this point;

                            // don't sleep on the first retry
                            // Most SQL Azure retries work on the first retry with no sleep
                            if (retryCount > 1)
                            {
                                // wait longer between each retry
                                int sleep = retryCount * retryCount * sqlRetrySleep;

                                // limit to the min and max retry values
                                if (sleep > sqlMaxSleep)
                                {
                                    sleep = sqlMaxSleep;
                                }
                                else if (sleep < sqlMinSleep)
                                {
                                    sleep = sqlMinSleep;
                                }

                                // sleep
                                System.Threading.Thread.Sleep(sleep);
                            }
                        }
                        else
                        {
                            System.Threading.Thread.Sleep(sqlRetrySleep);
                        }

                    }
                    finally
                    {
                        if (this.pConn.State != System.Data.ConnectionState.Closed)
                        {
                            this.pConn.Close();
                        }
                    }
                    if (CommandExecuted)
                    {
                        break;
                    }
                }



            }
            catch (SqlException e)
            {
                rowCount = 0;
                throw;// new DBException("A SQL exception occurred.", "SubmitData", e);
            }
            catch (Exception Ex)
            {
                rowCount = 0;
                throw;// new DBException(Ex.Message, Ex.Source, Ex);
            }
            finally
            {
                //Dispose the command.
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                    cmd.Dispose();
                    cmd = null;
                }

                //Close the connection; if preserveParameters is false, dispose it.
                if (this.pConn != null)
                {
                    this.pConn.Close();

                    if (!preserveParameters)
                    {
                        this.pConn.Dispose();
                        this.pConn = null;
                    }
                }

                //Save the output parameters for later use.
                SaveOutputValues();

                //Clear the parameter collection if preserveParameters is false.
                if (!preserveParameters)
                {
                    this.ClearParameters();
                }
            }

            return rowCount;
        }




        //public int SubmitData1(string queryText, QueryType queryType, bool preserveParameters)
        //{
        //    //Initialize the connection and command.
        //    if (this.pConn == null)
        //    {
        //        this.pConn = new SqlConnection(this.pConnectionString);
        //    }

        //    SqlCommand cmd = new SqlCommand(queryText, this.pConn);

        //    int rowCount = 0;

        //    if (queryType == QueryType.StoredProc)
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //    }
        //    else
        //    {
        //        cmd.CommandType = CommandType.Text;
        //    }

        //    //Add All SQL Parameters.
        //    if (pParameters.Count > 0)
        //    {
        //        cmd.Parameters.AddRange(pParameters.ToArray());
        //    }

        //    try
        //    {
        //        this.pConn.Open();

        //        //execute SQL Submits
        //        rowCount = cmd.ExecuteNonQuery();
        //        return Convert.ToInt32((pParameters[pParameters.Count - 1].Value));
        //    }
        //    catch (SqlException e)
        //    {
        //        rowCount = 0;
        //        throw new DBException("A SQL exception occurred.", "SubmitData", e);
        //    }
        //    catch (Exception Ex)
        //    {
        //        rowCount = 0;
        //        throw new DBException(Ex.Message, Ex.Source, Ex);
        //    }
        //    finally
        //    {
        //        //Dispose the command.
        //        if (cmd != null)
        //        {
        //            cmd.Parameters.Clear();
        //            cmd.Dispose();
        //            cmd = null;
        //        }

        //        //Close the connection; if preserveParameters is false, dispose it.
        //        if (this.pConn != null)
        //        {
        //            this.pConn.Close();

        //            if (!preserveParameters)
        //            {
        //                this.pConn.Dispose();
        //                this.pConn = null;
        //            }
        //        }

        //        //Save the output parameters for later use.
        //        SaveOutputValues();

        //        //Clear the parameter collection if preserveParameters is false.
        //        if (!preserveParameters)
        //        {
        //            this.ClearParameters();
        //        }
        //    }

        //    return rowCount;
        //}

        /// <summary>
        /// Updates the value of a parameter in the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter to update.</param>
        /// <param name="paramValue">The value to assign to the parameter.</param>
        public void UpdateParameterValue(string paramName, object paramValue)
        {
            paramName = paramName.Trim().ToLower();

            //Find the named parameter.
            foreach (SqlParameter param in pParameters)
            {
                if (param.ParameterName.Trim().ToLower() == paramName)
                {
                    //Set the parameter's value.
                    param.Value = paramValue;
                    break;
                }
            }
        }

        /// <summary>
        /// Returns the value of a specified parameter in the parameter collection.
        /// </summary>
        /// <param name="paramName">The name of the parameter whose value should be returned.</param>
        /// <returns>An Object containing the value of the specified parameter.</returns>
        public object GetParameterValue(string paramName)
        {
            paramName = paramName.Trim();

            if (!paramName.StartsWith("@"))
            {
                paramName = "@" + paramName;
            }

            return pOutputValues[paramName.Trim().ToLower()];
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by the DataPortal.
        /// </summary>
        public void Dispose()
        {
            pParameters = null;

            if (this.pConn != null)
            {
                this.pConn.Close();
                this.pConn.Dispose();
                this.pConn = null;
            }
        }

        #endregion

    }
}

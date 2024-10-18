// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DALHelper.cs" company="">
//   
// </copyright>
// <summary>
//   The dal helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSA_AdminPortal.DataAccess
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;


    /// <summary>
    /// The dal helper.
    /// </summary>
    public class DALHelper : IDataProviderBase
    {
        #region Constants and Fields


        /// <summary>
        /// The dl config.
        /// </summary>
        protected string SqlConString;

        /// <summary>
        /// The obj command.
        /// </summary>
        protected SqlCommand objCommand = new SqlCommand();

        /// <summary>
        /// The service log.
        /// </summary>
        private SqlConnection sqlconn;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DALHelper"/> class. 
        /// Constructor for Request Class
        /// </summary>
        public DALHelper()
        {
                this.SqlConString = "FssConnectionString";
        }

        #endregion

        #region Public Methods and Operators

        // WM - [07.04.2014] 
        /// <summary>
        /// to get LitleSubMerchantID for the given district
        /// </summary>
        /// <param name="district_Id"></param>
        /// <returns></returns>
        public string GetSubMerchant(string district_Id)
        {
            // Set the command text as name of the stored procedure
            var query = string.Format("Select dbo.fn_GetSubMerchant({0})", district_Id);

            try
            {
                var retVal = SQLDataAccess.SQLCommandExecuteScalar1(query);

                if (retVal == null)
                {
                    return null;
                }

                return retVal.ToString();
            }
            catch (Exception Ex)
            {
                //throw Ex;
            }

            return null;
        }

        public DataTable GetDistrictUsageFeeStatus(string DistrictID)
        {
            var command = new SqlCommand();
            var dss = new DataSet();
            var paramDistrictId = new SqlParameter();

            try
            {
                command.CommandText = "SP_CheckStudentUsageFeeApplicable";
                command.CommandType = CommandType.StoredProcedure;

                paramDistrictId = new SqlParameter("@DistrictID", SqlDbType.Int);
                paramDistrictId.Direction = ParameterDirection.Input;
                paramDistrictId.Value = Convert.ToInt32(DistrictID);
                command.Parameters.Add(paramDistrictId);

                dss = SQLDataAccess.ExecuteStoredProcedureDataSet(command); //(DataSet)this.DLCommand.ExecuteQuery(this.DLConfig, ReturnType.DataSetType);

                if (dss != null)
                {
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        return dss.Tables[0];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                //this.ServiceLog.AppendMessage("Error in getting the AllowUsageFee Status in District : " + ex.Message + ", District ID: " + DistrictID);
            }
            finally
            {
                paramDistrictId = null;
                command = null;
            }
            return null;
        }

        #endregion
    }
}
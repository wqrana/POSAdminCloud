// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProvider.cs" company="">
//   
// </copyright>
// <summary>
//   The data provider.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MSA_AdminPortal.DataAccess
{
    using System;
    using System.Data;

    /// <summary>
    /// The data provider.
    /// </summary>
    public class DataProvider
    {
        #region Constants and Fields

        /// <summary>
        /// The dataprovide r_ assembl y_ path.
        /// </summary>
        private const string DATAPROVIDER_ASSEMBLY_PATH = "DataAccess.dll";

        /// <summary>
        /// Returns an instance of the user-specified data provider class.
        /// </summary>
        /// <returns>An instance of the user-specified data provider class.  This class must inherit the
        /// IDataProviderBase interface.</returns> 
        private static IDataProviderBase dp;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The instance.
        /// </summary>
        /// <returns>
        /// </returns>
        public static IDataProviderBase Instance()
        {
            // if (dp == null)
            // {
            dp = new DALHelper();
            return dp;

            // }
            // else
            // return dp;
        }

        #endregion
    }

    /// <summary>
    /// The i data provider base.
    /// </summary>
    public interface IDataProviderBase
    {
        // View Students
        #region Public Methods and Operators
        
        // WM - [07.04.2014] 
        /// <summary>
        /// to get LitleSubMerchantID for the given district
        /// </summary>
        /// <param name="district_Id"></param>
        /// <returns></returns>
        
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Configuration;

namespace MSA_AdminPortal.DataAccess
{
    static class SQLDataAccess
    {
        private static string SqlConString;
        private static string FSSShoppingCartConnString;
        private static int sqlMaxRetries = 4;
        private static int sqlRetrySleep = 100;
        private static int sqlMaxSleep = 5000;
        private static int sqlMinSleep = 10;



        static SQLDataAccess()
        {
            // Reads the connection settings from web.config
            if (RoleEnvironment.IsAvailable)
            {
                SqlConString = RoleEnvironment.GetConfigurationSettingValue("FssConnectionString");
            }
            else
            {
                SqlConString = ConfigurationManager.ConnectionStrings["FSSConnectionString"].ConnectionString;
            }

            // Sql Connection string for getting FSS Shopping Cart Details
            if (RoleEnvironment.IsAvailable)
            {
                FSSShoppingCartConnString = RoleEnvironment.GetConfigurationSettingValue("FSSShopingCartConnString");
            }
            else
            {
                FSSShoppingCartConnString = ConfigurationManager.ConnectionStrings["FSSShopingCartConnString"].ConnectionString;
            }

        }
        public static int SQLCommand(string sql)
        {
            int retVal = -1;

            // start a timer
            TimeSpan ts;
            DateTime dt = DateTime.UtcNow;
            for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
            {
                SqlConnection DBConn = new SqlConnection(SqlConString);
                SqlCommand sc = new SqlCommand();
                try
                {
                    DBConn.Open();
                    sc.Connection = DBConn;
                    sc.CommandType = CommandType.Text;
                    sc.CommandText = sql;
                    retVal = sc.ExecuteNonQuery();
                    ts = DateTime.UtcNow - dt;

                    // log opens that take too long
                    if (ts.TotalMilliseconds >= 75)
                    {
                        //We can log time taken by this operation
                    }
                    break;
                }

                catch (SqlException ex)
                {
                    if (retryCount == 1)
                    {
                        SqlConnection.ClearPool(DBConn);
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
                        // Log the exception
                        //SxpLog.WriteSqlException(ex, conn.DataSource, conn.Database, conn.WorkstationId, "Connect", retryCount, "Final");

                        // we thought about rethrowing the exception, but chose not to
                        // this will give us one more chance to execute the request - we might get lucky ...

                        // sleep
                        System.Threading.Thread.Sleep(sqlRetrySleep);
                        //DBConn.Open();
                    }
                }
                finally
                {
                    sc = null;
                    DBConn.Close();
                    if (DBConn != null)
                    {
                        DBConn.Dispose();
                    }
                }
            }

            return retVal;
        }
        public static DataSet SQLCommandDataSet(string sql)
        {
            // start a timer
            TimeSpan ts;
            DateTime dt = DateTime.UtcNow;
            DataSet ds  = new DataSet();
            for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
            {
                SqlConnection DBConn = new SqlConnection(SqlConString);
                SqlCommand sc = new SqlCommand();
                try
                {
                    DBConn.Open();
                    sc.Connection = DBConn;
                    sc.CommandType = CommandType.Text;
                    sc.CommandText = sql;
                    SqlDataAdapter da = new SqlDataAdapter(sc);
                    da.Fill(ds);
                    ts = DateTime.UtcNow - dt;

                    // log opens that take too long
                    if (ts.TotalMilliseconds >= 75)
                    {
                        //We can log time taken by this operation
                    }
                    break;
                }

                catch (SqlException ex)
                {
                    if (retryCount == 1)
                    {
                        SqlConnection.ClearPool(DBConn);
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
                        // Log the exception
                        //SxpLog.WriteSqlException(ex, conn.DataSource, conn.Database, conn.WorkstationId, "Connect", retryCount, "Final");

                        // we thought about rethrowing the exception, but chose not to
                        // this will give us one more chance to execute the request - we might get lucky ...

                        // sleep
                        System.Threading.Thread.Sleep(sqlRetrySleep);
                        //DBConn.Open();
                    }
                }
                finally
                {
                    sc = null;
                    DBConn.Close();
                    if (DBConn != null)
                    {
                        DBConn.Dispose();
                    }
                }
            }

            return ds;
        }

        // WaheedM - [07.04.2014] 
        public static object SQLCommandExecuteScalar1(string sql)
        {
            object retVal = null;

            // start a timer
            TimeSpan ts;
            DateTime dt = DateTime.UtcNow;
            for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
            {
                SqlConnection DBConn = new SqlConnection(SqlConString);
                SqlCommand sc = new SqlCommand();
                try
                {
                    DBConn.Open();
                    sc.Connection = DBConn;
                    sc.CommandType = CommandType.Text;
                    sc.CommandText = sql;
                    retVal = sc.ExecuteScalar();
                    ts = DateTime.UtcNow - dt;

                    // log opens that take too long
                    if (ts.TotalMilliseconds >= 75)
                    {
                        //We can log time taken by this operation
                    }
                    break;
                }

                catch (SqlException ex)
                {
                    if (retryCount == 1)
                    {
                        SqlConnection.ClearPool(DBConn);
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
                        // Log the exception
                        //SxpLog.WriteSqlException(ex, conn.DataSource, conn.Database, conn.WorkstationId, "Connect", retryCount, "Final");

                        // we thought about rethrowing the exception, but chose not to
                        // this will give us one more chance to execute the request - we might get lucky ...

                        // sleep
                        System.Threading.Thread.Sleep(sqlRetrySleep);
                        //DBConn.Open();
                    }
                }
                finally
                {
                    sc = null;
                    DBConn.Close();
                    if (DBConn != null)
                    {
                        DBConn.Dispose();
                    }
                }
            }

            return retVal;
        }

        public static int SQLCommandExecuteScalar(string sql)
        {
            // Added by WM - [07.04.2014] 

            var retVal = SQLCommandExecuteScalar1(sql);

            if (retVal == null)
            {
                return -1;
            }

            try
            {
                return Convert.ToInt32(retVal);
            }
            catch
            {
                return -1;
            }

            // Commented by WM - [07.04.2014] 
            //int retVal = -1;

            //// start a timer
            //TimeSpan ts;
            //DateTime dt = DateTime.UtcNow;
            //for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
            //{
            //    SqlConnection DBConn = new SqlConnection(SqlConString);
            //    SqlCommand sc = new SqlCommand();
            //    try
            //    {
            //        DBConn.Open();
            //        sc.Connection = DBConn;
            //        sc.CommandType = CommandType.Text;
            //        sc.CommandText = sql;
            //        retVal = Convert.ToInt32(sc.ExecuteScalar());
            //        ts = DateTime.UtcNow - dt;

            //        // log opens that take too long
            //        if (ts.TotalMilliseconds >= 75)
            //        {
            //            //We can log time taken by this operation
            //        }
            //        break;
            //    }

            //    catch (SqlException ex)
            //    {
            //        if (retryCount == 1)
            //        {
            //            SqlConnection.ClearPool(DBConn);
            //        }

            //        if (retryCount < sqlMaxRetries)
            //        {
            //            //We can log retryCount at this point;

            //            // don't sleep on the first retry
            //            // Most SQL Azure retries work on the first retry with no sleep
            //            if (retryCount > 1)
            //            {
            //                // wait longer between each retry
            //                int sleep = retryCount * retryCount * sqlRetrySleep;

            //                // limit to the min and max retry values
            //                if (sleep > sqlMaxSleep)
            //                {
            //                    sleep = sqlMaxSleep;
            //                }
            //                else if (sleep < sqlMinSleep)
            //                {
            //                    sleep = sqlMinSleep;
            //                }

            //                // sleep
            //                System.Threading.Thread.Sleep(sleep);
            //            }
            //        }
            //        else
            //        {
            //            // Log the exception
            //            //SxpLog.WriteSqlException(ex, conn.DataSource, conn.Database, conn.WorkstationId, "Connect", retryCount, "Final");

            //            // we thought about rethrowing the exception, but chose not to
            //            // this will give us one more chance to execute the request - we might get lucky ...

            //            // sleep
            //            System.Threading.Thread.Sleep(sqlRetrySleep);
            //            //DBConn.Open();
            //        }
            //    }
            //    finally
            //    {
            //        sc = null;
            //        DBConn.Close();
            //        if (DBConn != null)
            //        {
            //            DBConn.Dispose();
            //        }
            //    }
            //}

            //return retVal;
        }
        public static int ExecuteStoredProcedure(SqlCommand sc, string outparm)
        {
            int retVal = -1;

            // start a timer
            TimeSpan ts;
            DateTime dt = DateTime.UtcNow;
            for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
            {
                SqlConnection DBConn = new SqlConnection(SqlConString);
                try
                {
                    DBConn.Open();
                    sc.Connection = DBConn;
                    retVal = sc.ExecuteNonQuery();

                    if (outparm != "")
                    {
                        retVal = Convert.ToInt32(sc.Parameters[outparm].Value.ToString());
                    }
                    ts = DateTime.UtcNow - dt;
                    if (ts.TotalMilliseconds >= 75)
                    {
                        //We can log time taken by this operation
                    }
                    break;
                }

                catch (SqlException ex)
                {
                    if (retryCount == 1)
                    {
                        SqlConnection.ClearPool(DBConn);
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

                    DBConn.Close();
                    if (DBConn != null)
                    {
                        DBConn.Dispose();
                    }
                }
            }
            sc = null;
            return retVal;
        }
        //
        public static DataSet ExecuteStoredProcedureDataSet(SqlCommand sc)
        {
            DataSet ds = new DataSet();

            // start a timer
            TimeSpan ts;
            DateTime dt = DateTime.UtcNow;
            for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
            {
                SqlConnection DBConn = new SqlConnection(SqlConString);
                try
                {
                    DBConn.Open();
                    sc.Connection = DBConn;
                    SqlDataAdapter da = new SqlDataAdapter(sc);
                    da.Fill(ds);
                    ts = DateTime.UtcNow - dt;
                    if (ts.TotalMilliseconds >= 75)
                    {
                        //We can log time taken by this operation
                    }
                    break;
                }

                catch (SqlException ex)
                {
                    if (retryCount == 1)
                    {
                        SqlConnection.ClearPool(DBConn);
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

                    DBConn.Close();
                    if (DBConn != null)
                    {
                        DBConn.Dispose();
                    }
                }
            }
            sc = null;
            return ds;
        }

        public static DataSet ExecuteStoredProcedureDataSetCartDb(SqlCommand sc)
        {
            DataSet ds = new DataSet();

            // start a timer
            TimeSpan ts;
            DateTime dt = DateTime.UtcNow;
            for (int retryCount = 1; retryCount <= sqlMaxRetries; retryCount++)
            {
                SqlConnection DBConn = new SqlConnection(FSSShoppingCartConnString);
                try
                {
                    DBConn.Open();
                    sc.Connection = DBConn;
                    SqlDataAdapter da = new SqlDataAdapter(sc);
                    da.Fill(ds);
                    ts = DateTime.UtcNow - dt;
                    if (ts.TotalMilliseconds >= 75)
                    {
                        //We can log time taken by this operation
                    }
                    break;
                }

                catch (SqlException ex)
                {
                    if (retryCount == 1)
                    {
                        SqlConnection.ClearPool(DBConn);
                    }

                    if (retryCount < sqlMaxRetries)
                    {
                        //SxpLog.WriteSqlRetry(5902, ex, conn.DataSource, conn.Database, conn.WorkstationId, "Connect", retryCount);

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

                    DBConn.Close();
                    if (DBConn != null)
                    {
                        DBConn.Dispose();
                    }
                }
            }
            sc = null;
            return ds;
        }

    }

}


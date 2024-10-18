using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;

namespace Repository.Helpers
{
    public static class ErrorLogHelper
    {
        private static string connectionString;
        private static string tableNameDev;
        private static string tableNameLive;
        private static string tableName;
        private static string isUseLiveTable;
        private static CloudStorageAccount storageAccount;
        private static CloudTableClient tableClient;
        private static ListRowsContinuationToken continuationToken = null;
        static ErrorLogHelper()
        {
            //if (RoleEnvironment.IsAvailable)
            //{
            //    connectionString = RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString");
            //    isUseLiveTable = RoleEnvironment.GetConfigurationSettingValue("IsUseLiveErrorTable");
            //    tableNameDev = RoleEnvironment.GetConfigurationSettingValue("ErrorLogTableNameDEV");
            //    tableNameLive = RoleEnvironment.GetConfigurationSettingValue("ErrorLogTableNameLive");
            //}
            //else
            //{
            connectionString = ConfigurationManager.AppSettings["StorageConnectionString"].ToString();
            isUseLiveTable = ConfigurationManager.AppSettings["IsUseLiveErrorTable"].ToString();
            tableNameDev = ConfigurationManager.AppSettings["ErrorLogTableNameDEV"].ToString();
            tableNameLive = ConfigurationManager.AppSettings["ErrorLogTableNameLive"].ToString();
            //}

            // Get connection string and table name from settings.


            // Reference storage account from connection string. 
            storageAccount = CloudStorageAccount.Parse(connectionString);

            // Create Table service client.
            tableClient = storageAccount.CreateCloudTableClient();

            if (Convert.ToBoolean(isUseLiveTable))
                tableName = tableNameLive;
            else
                tableName = tableNameDev;
        }

        // Insert a new Error Log.
        public static void InsertLog(string LogType, DateTime LogEstDate, string ActionClassName, string Message, string CustomerId, string MethodName)
        {
            // Get data context.
            CloudTable logTable = tableClient.GetTableReference(tableName);
            logTable.CreateIfNotExists();

            string IISName = System.Net.Dns.GetHostName();

                ErrorLogEntity entity = new ErrorLogEntity();
                // Partition key is first letter of Error Log's first name.
            entity.PartitionKey = "FSSPOSAPILog";

                // Row key is value of first name, with GUID appended to avoid conflicts in case where two first names are the same.
                entity.RowKey = String.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

                // Populate the other properties.
            entity.LogDate = DateTime.Now;
                entity.LogEstDate = LogEstDate;
                entity.IISName = IISName;
                entity.LogType = LogType;
                entity.ActionClassName = ActionClassName;
                entity.Message = Message;
                entity.CustomerId = CustomerId;
                entity.MethodName = MethodName;

            // Create an operation to add the new customer to the people table. 
            TableOperation tempTable = TableOperation.Insert(entity);

            // Submit the operation to the table service. 
            logTable.Execute(tempTable);

        }

        #region OLD commented code 
        //// Insert a new Error Log.
        //public static void InsertLog(string LogType, DateTime LogEstDate, string ActionClassName, string Message, string CustomerId, string MethodName)
        //{
        //    // Get data context.
        //    TableServiceContext context = tableClient.GetDataServiceContext();

        //    string IISName = System.Net.Dns.GetHostName();
        //    InsertLogInternal(LogType, IISName, context, DateTime.Now, LogEstDate, ActionClassName, Message, GetCustomerId(CustomerId), MethodName);

        //    // Save changes to the service.
        //    context.SaveChanges();
        //}
        //// Insert a new Log.
        //private static void InsertLogInternal(string LogType, string IISName, TableServiceContext context, DateTime LogDate, DateTime LogEstDate, string ActionClassName, string Message, int CustomerId, string MethodName)
        //{
        //    // Create the new entity.
        //    ErrorLogEntity entity = new ErrorLogEntity();

        //    // Partition key is first letter of Error Log's first name.
        //    entity.PartitionKey = "FSSPOSLog";

        //    // Row key is value of first name, with GUID appended to avoid conflicts in case where two first names are the same.
        //    entity.RowKey = String.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

        //    // Populate the other properties.
        //    entity.LogDate = LogDate;
        //    entity.LogEstDate = LogEstDate;
        //    entity.IISName = IISName;
        //    entity.LogType = LogType;
        //    entity.ActionClassName = ActionClassName;
        //    entity.Message = Message;
        //    entity.CustomerId = CustomerId;
        //    entity.MethodName = MethodName;


        //    // Add the entity.
        //    context.AddObject(tableName, entity);
        //}

        //public static List<ErrorLogEntity> GetAllLogs()
        //{
        //    TableServiceContext tableServiceContext = tableClient.GetDataServiceContext();
        //    List<ErrorLogEntity> entities = new List<ErrorLogEntity>();
        //    do
        //    {
        //        var allEntities = tableServiceContext.CreateQuery<ErrorLogEntity>(tableName);
        //        var query = allEntities as DataServiceQuery<ErrorLogEntity>;
        //        if (continuationToken != null)
        //        {
        //            query = query.AddQueryOption("NextPartitionKey", continuationToken.PartitionKey);
        //            if (continuationToken.RowKey != null)
        //            {
        //                query = query.AddQueryOption("NextRowKey", continuationToken.RowKey);
        //            }
        //        }

        //        var response = query.Execute() as QueryOperationResponse;
        //        foreach (ErrorLogEntity e in response)
        //            entities.Add(e);


        //        if (response.Headers.ContainsKey("x-ms-continuation-NextPartitionKey"))
        //        {
        //            continuationToken = new ListRowsContinuationToken();
        //            continuationToken.PartitionKey = response.Headers["x-ms-continuation-NextPartitionKey"];
        //            if (response.Headers.ContainsKey("x-ms-continuation-NextRowKey"))
        //            {
        //                continuationToken.RowKey = response.Headers["x-ms-continuation-NextRowKey"];
        //            }
        //        }
        //        else
        //        {
        //            continuationToken = null;
        //        }
        //    } while (continuationToken != null);

        //    return entities;
        //}

        //public static List<ErrorLogEntity> GetAllLogs(int pageSize, ListRowsContinuationToken token, out ListRowsContinuationToken rowToken)
        //{
        //    TableServiceContext tableServiceContext = tableClient.GetDataServiceContext();
        //    List<ErrorLogEntity> entities = new List<ErrorLogEntity>();

        //    var allEntities = tableServiceContext.CreateQuery<ErrorLogEntity>(tableName).Take(pageSize);
        //    var query = allEntities as DataServiceQuery<ErrorLogEntity>;
        //    if (token != null)
        //    {
        //        query = query.AddQueryOption("NextPartitionKey", token.PartitionKey);
        //        if (token.RowKey != null)
        //        {
        //            query = query.AddQueryOption("NextRowKey", token.RowKey);
        //        }
        //    }

        //    var response = query.Execute() as QueryOperationResponse;
        //    foreach (ErrorLogEntity e in response)
        //        entities.Add(e);


        //    if (response.Headers.ContainsKey("x-ms-continuation-NextPartitionKey"))
        //    {
        //        token = new ListRowsContinuationToken();
        //        token.PartitionKey = response.Headers["x-ms-continuation-NextPartitionKey"];
        //        if (response.Headers.ContainsKey("x-ms-continuation-NextRowKey"))
        //        {
        //            token.RowKey = response.Headers["x-ms-continuation-NextRowKey"];
        //        }
        //    }
        //    rowToken = token;
        //    return entities;
        //}

        //public static List<ErrorLogEntity> GetPagedLogs(int pageSize, int pageIndex)
        //{
        //    TableServiceContext tableServiceContext = tableClient.GetDataServiceContext();
        //    IQueryable<ErrorLogEntity> entities = (from e in tableServiceContext.CreateQuery<ErrorLogEntity>(tableName)
        //                                           select e);
        //    return entities.ToList();
        //}

        //public static void CreateTable()
        //{
        //    tableClient.CreateTableIfNotExist(tableName);
        //}
        #endregion

        private static int GetCustomerId(string CustomerId)
        {
            if (CustomerId != "")
            {
                return Convert.ToInt32(CustomerId);
            }
            else
            {
                return 0;
            }
        }

    }

    public class ListRowsContinuationToken
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }

    }

    public class Constants
    {
        // Error log type constants.
        public const string ERROR = "ERROR";
        public const string INFO = "INFO";
        public const string WARNING = "WARNING";
        public const string TRACE = "TRACE";
    }

    public class ErrorLogEntity : TableEntity
    {

        public string IISName { get; set; }

        public string LogType { get; set; }

        public DateTime LogDate { get; set; }

        public DateTime LogEstDate { get; set; }

        public string ActionClassName { get; set; }

        public string Message { get; set; }

        public string CustomerId { get; set; }

        public string MethodName { get; set; }

    }

    public class CommonClasses
    {
        public static string getCustomerID()
        {
            if (HttpContext.Current.Session["Cust_ID"] != null)
            {
                return HttpContext.Current.Session["Cust_ID"].ToString();
            }
            else
            {
                return "";
            }
        }
    }
}
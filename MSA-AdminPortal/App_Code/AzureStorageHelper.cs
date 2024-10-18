using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;
using MSA_AdminPortal.App_Code;
using MSA_AdminPortal.Helpers;
using Repository.Helpers;
namespace MSA_AdminPortal
{
    public class AzureStorageHelper
    {
        private static string connectionString;
        private static CloudStorageAccount storageAccount;

        static AzureStorageHelper()
        {
            try
            {
                connectionString = ConfigurationManager.AppSettings["StorageConnectionString"].ToString();
                storageAccount = CloudStorageAccount.Parse(connectionString);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "AzureStorageHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AzureStorageHelper");
            
            }
        }
        public static void uploadCutomerPicture(string clieintID, string CustomerID, HttpPostedFileBase file)
        {
            try
            {
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("customerspics");
                container.CreateIfNotExists();
                container.SetPermissions(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });


                CloudBlockBlob blockBlob = container.GetBlockBlobReference(clieintID + "/" + CustomerID + ".jpg");

                // Create or overwrite the "myblob" blob with contents from a local file.
                using (var stream = file.InputStream)
                {
                    blockBlob.UploadFromStream(stream);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "AzureStorageHelper", "Error : upload cusotmer picture :: " + ex.Message, CommonClasses.getCustomerID(), "uploadCutomerPicture");
            }
        }

        public static CloudBlockBlob getCutomerPicture(string azureContainer, string clieintID, string CustomerID)
        {

            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);

                // Retrieve reference to a blob named "photo1.jpg".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(clieintID + "/" + CustomerID + ".jpg");

                // Save blob contents to a file.
                using (var fileStream = System.IO.File.OpenWrite(CustomerID + ".jpg"))
                {
                    blockBlob.DownloadToStream(fileStream);
                }
                return blockBlob;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "AzureStorageHelper", "Error : Get cusotmer picture :: " + ex.Message, CommonClasses.getCustomerID(), "getCutomerPicture");
                return null;
            }
        }


        public static bool removeCutomerPicture(string azureContainer, string clieintID, string CustomerID)
        {

            try
            {
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);

                // Retrieve reference to a blob named "photo1.jpg".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(clieintID + "/" + CustomerID + ".jpg");

                // remove the blob content
                blockBlob.Delete();
                return true;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "AzureStorageHelper", "Error : Remove cusotmer picture :: " + ex.Message, CommonClasses.getCustomerID(), "removeCutomerPicture");
                return false;   
            }
        }

        public static bool checkCutomerPicture(string azureContainer, string clieintID, string CustomerID)
        {

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(azureContainer);

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(clieintID + "/" + CustomerID + ".jpg");
            try
            {
                blockBlob.FetchAttributes();
                return true;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "AzureStorageHelper", "Error : Cusotmer picture :: " + ex.Message, CommonClasses.getCustomerID(), "checkCutomerPicture");
                return false;
            }
        }
    }
}
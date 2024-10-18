using MSA_ADMIN.DAL.Common;
using MSA_ADMIN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Factories
{
    public class CommunicationFactory
    {
        public static List<ParentAlertData> GetParentAlertList(int displayLenght, int displayStart, int sortColumnIndex, string sortDirection, out int totalDisplayRecords, long districtId)
        {
            totalDisplayRecords = 0;

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@DisplayLength", displayLenght);
                dataPortal.AddIntParameter("@DisplayStart", displayStart);
                dataPortal.AddIntParameter("@SortCol", sortColumnIndex);
                dataPortal.AddStringParameter("@SortDir", sortDirection);
                dataPortal.AddLongParameter("@DistrictId", districtId);

                reader = dataPortal.GetDataReader("[msa_GetParentAlerts]", DataPortal.QueryType.StoredProc);
                List<ParentAlertData> parentAlertList = PopulateParentAlertListFromReader(reader, out totalDisplayRecords);
                return parentAlertList;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }

        public static ParentAlert GetParentAlert(int parentAlertId)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@AlertId", parentAlertId);

                reader = dataPortal.GetDataReader("[msa_GetParentAlert]", DataPortal.QueryType.StoredProc);
                ParentAlert parentAlert = PopulateParentAlertFromReader(reader);
                return parentAlert;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }


        public static List<DistrictLookup> GetDistrictList()
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {

                reader = dataPortal.GetDataReader("[usp_ADMIN_getActiveDistrictsList]", DataPortal.QueryType.StoredProc);
                List<DistrictLookup> districtList = PopulateDistrictListFromReader(reader);
                return districtList;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }


        public static int UpdateAlert(string title, string message, DateTime startDate, DateTime? endDate, bool enabled, int id, int edditedId, string districtGroup)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {

                dataPortal.AddStringParameter("@MessageName", title);
                dataPortal.AddStringParameter("@MessageText", message);
                dataPortal.AddDateParameter("@AlertStartDate", startDate);
                dataPortal.AddDateParameter("@AlertEndDate", endDate);
                dataPortal.AddBoolParameter("@IsActive", enabled);
                dataPortal.AddIntParameter("@EditedDistrictUserID", edditedId);
                dataPortal.AddStringParameter("@DistrictGroup", districtGroup);
                dataPortal.AddIntParameter("@AlertId", id);
                return dataPortal.SubmitData("[msa_UpdateParentAlert]", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataPortal != null)
                    dataPortal.Dispose();
            }

            
        }


        public static int AddNewAlert(string title, string message, DateTime startDate, DateTime? endDate, bool enabled, int edditedId, string districtGroup, int districtId)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@DistrcitId", districtId);
                //dataPortal.AddDateParameter("@MessageCreated", title);
                dataPortal.AddStringParameter("@MessageName", title);
                dataPortal.AddStringParameter("@MessageText", message);
                dataPortal.AddDateParameter("@AlerStartDate", startDate);
                
                dataPortal.AddDateParameter("@AlertEndDate", endDate);

                dataPortal.AddBoolParameter("@SendEmailNotification", false);
                dataPortal.AddBoolParameter("@IsActive", enabled);
                //dataPortal.AddDateParameter("@LastEditedDate", edditedId);
                dataPortal.AddIntParameter("@CreatedDistrictUserID", edditedId);
                dataPortal.AddIntParameter("@EditedDistrictUserID", edditedId);
                dataPortal.AddStringParameter("@DistrictGroup", districtGroup);

                return dataPortal.SubmitData("[msa_AddParentsAlert]", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        


        public static int AddAlerts(int District_Id, string MessageName, string MessageText, DateTime MessageStart, DateTime? MessageEnd, bool SendEmailNotification, bool Enabled, string districtGroup)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@District_Id", District_Id);
                dataPortal.AddStringParameter("@MessageName", MessageName);
                dataPortal.AddStringParameter("@MessageText", MessageText);
                dataPortal.AddDateParameter("@MessageStart", MessageStart);
                dataPortal.AddDateParameter("@MessageEnd", MessageEnd);
                dataPortal.AddBoolParameter("@SendEmailNotification", false); // Its hard coded in MSA as well.
                dataPortal.AddBoolParameter("@Enabled", Enabled);
                dataPortal.AddStringParameter("@DistrictGroup", districtGroup);

                return dataPortal.SubmitData("[usp_ADMIN_AddParentsAlert]", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataPortal != null)
                    dataPortal.Dispose();
            }

        }

        public static int DeleteParentAlert(int parentAlertId)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@ID", parentAlertId);
                return dataPortal.SubmitData("[usp_ADMIN_DelParentAlerts]", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }
        






        private static List<ParentAlertData> PopulateParentAlertListFromReader(SafeDataReader reader, out int totalDisplayRecords)
        {
            List<ParentAlertData> parentAlertList = new List<ParentAlertData>();
            totalDisplayRecords = 0;

            while (reader.Read())
            {
                totalDisplayRecords = reader.GetInt32("TotalDisplayCount");

                ParentAlertData parentAlert = new ParentAlertData();

                parentAlert.ID = reader.GetInt32("ID");
                parentAlert.Status = reader.GetString("Status");
                parentAlert.MessageCreated = reader.GetString("MessageCreated");
                parentAlert.MessageName = reader.GetString("MessageName");
                parentAlert.MessageText = reader.GetString("MessageText");
                parentAlert.MessageStart = reader.GetString("MessageStart");
                parentAlert.MessageEnd = reader.GetString("MessageEnd");
                parentAlert.userid2 = reader.GetString("userid2");
                parentAlert.USERID = reader.IsDBNull(reader.GetOrdinal("USERID")) ? (int?)0 : reader.GetInt32("USERID");
                parentAlert.District_Id = reader.GetInt32("District_Id");

                parentAlertList.Add(parentAlert);
            }
            return parentAlertList;
        }

        private static ParentAlert PopulateParentAlertFromReader(SafeDataReader reader)
        {
            ParentAlert parentAlert = new ParentAlert();

            //This reader will return us only one record. Because TOP 1 is used in Select Query
            while (reader.Read())
            {
                parentAlert.Id                       = reader.GetInt32("Id");
                parentAlert.District_Id              = reader.GetInt32("District_Id");
                parentAlert.MessageCreated           = reader.GetDateTime("MessageCreated");
                parentAlert.MessageName              = reader.GetString("MessageName");
                parentAlert.MessageText              = reader.GetString("MessageText");
                parentAlert.MessageStart             = reader.GetDateTime("MessageStart");
                parentAlert.MessageEnd               = reader.IsDBNull(reader.GetOrdinal("MessageEnd")) ? (DateTime?) null : reader.GetDateTime("MessageEnd");
                parentAlert.SendEmailNotification    = reader.IsDBNull(reader.GetOrdinal("SendEmailNotification")) ? (bool?) null : reader.GetBoolean("SendEmailNotification");
                parentAlert.Enabled                  = reader.IsDBNull(reader.GetOrdinal("Enabled")) ? (bool?) null : reader.GetBoolean("Enabled");
                parentAlert.LastEdited               = reader.IsDBNull(reader.GetOrdinal("LastEdited")) ? (DateTime?) null : reader.GetDateTime("LastEdited");
                parentAlert.Created_DistrictUsers_ID = reader.IsDBNull(reader.GetOrdinal("Created_DistrictUsers_ID")) ? (int?) null : reader.GetInt32("Created_DistrictUsers_ID");
                parentAlert.Edited_DistrictUsers_ID  = reader.IsDBNull(reader.GetOrdinal("Edited_DistrictUsers_ID")) ? (int?) null : reader.GetInt32("Edited_DistrictUsers_ID");
                parentAlert.DistrictGroup            = reader.GetString("DistrictGroup");
            }
            return parentAlert;
        }

        private static List<DistrictLookup> PopulateDistrictListFromReader(SafeDataReader reader)
        {
            List<DistrictLookup> districtList = new List<DistrictLookup>();

            while (reader.Read())
            {
                DistrictLookup district = new DistrictLookup();
                district.Id = reader.GetInt32("id");
                district.Name = reader.GetString("name");
                districtList.Add(district);
            }
            return districtList;
        }




        

    
    }

    public class DistrictLookup
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

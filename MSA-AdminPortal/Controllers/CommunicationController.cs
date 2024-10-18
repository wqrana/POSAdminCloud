using AdminPortalModels.ViewModels;
using MSA_ADMIN.DAL.Factories;
using MSA_ADMIN.DAL.Models;
using MSA_AdminPortal.Helpers;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Repository;
using System.Globalization;

namespace MSA_AdminPortal.Controllers
{
    public class CommunicationController : BaseAuthorizedController
    {

        
        public CommunicationController()
        {
        }

        public ActionResult Index()
        {
            if (!SecurityManager.viewMSAAlerts)
            {
                return RedirectToAction("NoAccess", "Security", new { id = "nomsaalerts" });
            }
            ViewBag.MsaAlertCreatePermission = SecurityManager.CreateMSAAlerts;
            return View();
        }

        // ajax handlers
        public string AjaxParentAlertList(JQueryDataTableParamModel param)
        {
            try
            {

                int totalDisplayRecords = 0;
                long ClientId = ClientInfoData.GetClientID();

                List<ParentAlertData> parentAlertList = CommunicationFactory.GetParentAlertList(param.iDisplayLength, param.iDisplayStart, param.iSortCol_0, param.sSortDir_0, out totalDisplayRecords, ClientId);

                var result = new
                {
                    iTotalRecords = totalDisplayRecords,
                    iTotalDisplayRecords = totalDisplayRecords,
                    aaData = parentAlertList
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CommunicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxParentAlertList");
                return null;
            }
        }

        public string AjaxGetParentAlert(int parentAlertId)
        {
            try
            {
                ParentAlert parentAlert = CommunicationFactory.GetParentAlert(parentAlertId);
                
                var result = new
                {
                    aaData = parentAlert
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CommunicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxGetParentAlert");
                return null;
            }
        }

        public string AjaxGetDistrictList()
        {
            try
            {
                List<DistrictLookup> districtList = CommunicationFactory.GetDistrictList();

                var result = new
                {
                    aaData = districtList
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CommunicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxGetDistrictList");
                return null;
            }
        }

        public int AjaxCreateUpdateAlert(string dataToUpload)
        {
            int result = 0;
            string districtID = Convert.ToString(ClientInfoData.GetClientID());
            string[] alertFields = dataToUpload.Split('*');
            if (alertFields.Length == 8)
            {
                string title = alertFields[0].Replace("'", "''");
                string message = alertFields[1].Replace("'", "''");
                DateTime startDate = Convert.ToDateTime(alertFields[2]);// DateTime.ParseExact(, "d/M/yyyy", CultureInfo.InvariantCulture);
                DateTime? endDate = alertFields[3] == "" ? (DateTime?)null : Convert.ToDateTime(alertFields[3]); // DateTime.ParseExact(alertFields[3], "d/M/yyyy", CultureInfo.InvariantCulture).AddDays(1).AddMilliseconds(-1);
                bool enabled = Convert.ToBoolean(alertFields[4]);
                int id =  alertFields[5] == "" ? 0 : Convert.ToInt32(alertFields[5]);
                string districtsList = alertFields[6];
                string districtGroup = alertFields[7];


                // UF Review following comment

                //int edditedId = Convert.ToInt32(DistrictUsers_ID);
                //if (Convert.ToBoolean(HttpContext.Current.Session["IsAdmin"]))
                //{
                //    //edditedId = -1;
                //    edditedId = 0;
                //}

                int edditedId = 0; // UF Review

                if (id != 0)
                {
                    result = CommunicationFactory.UpdateAlert(title, message, startDate, endDate, enabled, id, edditedId, districtGroup);
                }
                else
                {
                  //  if (!districtsList.Contains("all") & !districtsList.Contains(districtID) & districtGroup != "AllDistricts" & !districtsList.Contains(","))
                    if (districtGroup != "AllDistricts" && districtsList == "-1")
                    {
                        result = CommunicationFactory.AddNewAlert(title, message, startDate, endDate, enabled, edditedId, districtGroup, (int)ClientInfoData.GetClientID());
                    }
                    else //if (districtsList != "-1" || districtGroup == "AllDistricts")
                    {
                        //Bug 1834
                        string currDistrict = districtID.ToString();

                        var districtIds = districtsList.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                        var isCurrDistrictExists = districtIds.Count(x => x.ToString() == currDistrict);

                       // if (!districtsList.Contains("-1") & !districtsList.Contains(currDistrict))
                        if (!districtsList.Contains("-1") && isCurrDistrictExists == 0)
                        {
                            districtsList += "," + currDistrict;
                        }
                        //end fixed
                        result = AddNewAlertforDistricts(districtsList, title, message, startDate, endDate, false, enabled, districtGroup);
                    }
                }

            }
            return result;
        }

        public string AjaxDeleteParentAlert(int parentAlerId)
        {
            try
            {
                int value = CommunicationFactory.DeleteParentAlert(parentAlerId);

                var result = new
                {
                    Data = value
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CommunicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxDeleteParentAlert");
                return null;
            }
        }



        public static int AddNewAlertforDistricts(string districtsList, string MessageName, string MessageText, DateTime MessageStart, DateTime? MessageEnd, bool SendEmailNotification, bool enabled, string districtGroup)
        {
            int result = 0;
            var districtList = districtsList;


            //DateTime dtMessageStart = Convert.ToDateTime(MessageStart, new CultureInfo("en-US")); // DateTime.ParseExact(MessageStart.Trim(), "MM/dd/yyyy", null);//, CultureInfo.InvariantCulture); //Convert.ToDateTime(MessageStart);
            //DateTime dtMessageEnd = Convert.ToDateTime(MessageEnd, new CultureInfo("en-US")); // DateTime.ParseExact(MessageEnd, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture); //Convert.ToDateTime(MessageEnd);
            //Int16 sendNotif = Convert.ToInt16(SendEmailNotification);
            //Int16 enabledint = Convert.ToInt16(enabled);


            if (districtsList.Contains("all") || districtGroup == "AllDistricts")
            {
                result = CommunicationFactory.AddAlerts(0, MessageName, MessageText, MessageStart, MessageEnd, SendEmailNotification, enabled, districtGroup);
            }
            else
            {
                var districtIds = districtList.Split(new[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string districtId in districtIds)
                {
                    int districtInt = Convert.ToInt16(districtId);
                    result = CommunicationFactory.AddAlerts(districtInt, MessageName, MessageText, MessageStart, MessageEnd, SendEmailNotification, enabled, districtGroup);
                }
            }

            return result;
        }


    }
}
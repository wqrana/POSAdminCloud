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
using System.Data;
using System.IO;

namespace MSA_AdminPortal.Controllers
{
    public class ParentsController : BaseAuthorizedController
    {

        /// <summary>
        /// Constructer for class
        /// </summary>
        public ParentsController()
        {
        }

        /// <summary>
        /// Initially populates model
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!SecurityManager.viewParent)
            {
                return RedirectToAction("NoAccess", "Security", new { id = "noparents" });
            }

            try
            {
                ViewBag.SearchByList = GetSearchDDLItems();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Index");
            }

            return View();
        }

        // ajax handlers
        public string AjaxParentList(JQueryDataTableParamModel param)
        {
            try
            {
                
                int totalDisplayRecords = 0;
                //int totalRecords = 0;
                long ClientId = ClientInfoData.GetClientID();
                string SearchValue = Request["SearchValue"] == "" ? null : Request["SearchValue"];
                string SearchBy = Request["SearchBy"]; // asc or desc

                //List<Parent> parentList = ParentFactory.GetParentPage(44, "LastName", "TestValue", param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, out totalRecords);
                List<Parent> parentList = ParentFactory.GetParentList(param.iDisplayLength, param.iDisplayStart, param.iSortCol_0, param.sSortDir_0, out totalDisplayRecords, ClientId, SearchValue, SearchBy);

                var result = new
                {
                    iTotalRecords = totalDisplayRecords,
                    iTotalDisplayRecords = totalDisplayRecords,
                    aaData = parentList
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json  = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxParentList");
                return null;
            }
        }

        public string AjaxParent(int parentId)
        {
            try
            {
                Parent parent = ParentFactory.GetParent(parentId);

                var result = new
                {
                    aaData = parent
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxParent");
                return null;
            }
        }

        public string AjaxStudentList(int parentId)
        {
            try
            {
                List<Student> studentListMSA = ParentFactory.GetStudentList(parentId);
                List<Student> studentListPOS = null;
                bool usePosData = ParentFactory.GetDistOptionUseLivePOSDataFlag(ClientInfoData.GetClientID());

                if (usePosData)
                {
                    string customerSearchStr = "";
                    string userIDSearchStr = "";

                    if (studentListMSA != null && studentListMSA.Count > 0)
                    {
                        var list1 = (from Student student in studentListMSA
                                     select (student.ClientCustId != null) ? student.ClientCustId : default(Int64?)
                                     ).ToList();

                        customerSearchStr = String.Join(",", list1.Where(c => !string.IsNullOrEmpty(Convert.ToString(c))));

                        var list2 = (from Student student in studentListMSA
                                     select (student.DistrictCustId != null) ? student.DistrictCustId : default(Int32?)
                                    ).ToList();

                        userIDSearchStr = String.Join(",", list2.Where(c => !string.IsNullOrEmpty(Convert.ToString(c)) && c != 0));

                        UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
                        List<AdminPortalModels.Models.POSCustomer> POSlist = unitOfWork.CustomCustomerRepository.GetPosStudents(ClientInfoData.GetClientID(), customerSearchStr, userIDSearchStr);

                        List<Student> msaADMINStudents = (from Student student in studentListMSA

                                                          select new Student
                                                          {
                                                              StudentId = student.StudentId,
                                                              DistrictCustId = (student.DistrictCustId != null) ? student.DistrictCustId : default(Int32?),
                                                              ClientCustId = (student.ClientCustId != null) ? student.ClientCustId : default(Int32?)
                                                          }).ToList();

                        var joinquery = from POSCustomer in POSlist
                                        join Student in msaADMINStudents on POSCustomer.CustomerID equals Student.ClientCustId
                                        select new Student 
                                        { 
                                            UserId = POSCustomer.UserId, 
                                            LastName = POSCustomer.LastName, 
                                            FirstName = POSCustomer.FirstName, 
                                            SchoolName = POSCustomer.SchoolName, 
                                            Active = POSCustomer.Active, 
                                            Balance =  POSCustomer.Balance.ToString(), 
                                            DOB = POSCustomer.DOB,
                                            Grade = Convert.ToString(POSCustomer.Grade),
                                            HomeRoom = POSCustomer.HomeRoom,
                                            StudentId = Student.StudentId
                                        };

                        studentListPOS = joinquery.ToList();

                    }
                }
                
                
                var result = new
                {
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = (usePosData && studentListPOS != null) ? studentListPOS : studentListMSA
                };
                

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxStudentList");
                return null;
            }
        }

        public string AjaxDepositHistory(JQueryDataTableParamModel param)
        {
            try
            {

                int totalDisplayRecords = 0;
                //int totalRecords = 0;
                int parentId = Convert.ToInt32(Request["ParentId"]);

                List<TransactionHistory> transactionHistoryList = ParentFactory.GetTransactionList(param.iDisplayLength, param.iDisplayStart, param.iSortCol_0, param.sSortDir_0, out totalDisplayRecords, parentId);

                var result = new
                {
                    iTotalRecords = totalDisplayRecords,
                    iTotalDisplayRecords = totalDisplayRecords,
                    aaData = transactionHistoryList
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxDepositHistory");
                return null;
            }
        }

        public string AjaxStudentDetail(int parentId)
        {
            try
            {

                //int totalDisplayRecords = 0;
                //int totalRecords = 0;
                //long ClientId = ClientInfoData.GetClientID();
                //int parentId = Convert.ToInt32(Request["ParentId"]); // asc or desc

                //List<Parent> parentList = ParentFactory.GetParentPage(44, "LastName", "TestValue", param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, out totalRecords);
                List<Student> studentList = ParentFactory.GetActiveStudentList(parentId);

                var result = new
                {
                    iTotalRecords = 0,
                    iTotalDisplayRecords = 0,
                    aaData = studentList
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxStudentList");
                return null;
            }
        }

        public string AjaxDeleteStudent(int studentId, int parentId)
        {
            try
            {
                int value = ParentFactory.DeleteStudent(studentId, parentId);

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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxDeleteStudent");
                return null;
            }
        }

        public string AjaxUpdateParentEmail(int parentId, string email)
        {
            try
            {
                int value = 0;
                if (!string.IsNullOrEmpty(email))
                {
                    value = ParentFactory.UpdateParentEmail(parentId, email);
                }

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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxUpdateParentEmail");
                return null;
            }
        }

        public string AjaxChangePassword(int parentId)
        {
            try
            {
                DateTime dt = DateTime.Now;
                int RandNum, mdt = dt.Millisecond;
                Random RanPass = new Random(mdt);
                RandNum = RanPass.Next(10000000, 99999999);
                string UserID, Email, NewPassword = RandNum.ToString();
                int retValue = 0;

                Parent parent = ParentFactory.GetParent(parentId);

                if (parent != null)
                {
                    UserID = parent.UserID;
                    Email = parent.Email;


                    if (SendNotificationEmail(Email, UserID, NewPassword))
                    {
                        retValue = ParentFactory.UpdateParentPassword(parentId, NewPassword);
                    }
                }

                var result = new
                {
                    Data = retValue
                };

                JavaScriptSerializer js = new JavaScriptSerializer();
                var json = js.Serialize(result);

                return json;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxChangePassword");
                return null;
            }
        }

        private bool SendNotificationEmail(string ParentEmail, string UserID, string NewPassword)
        {
            bool retValue = false;
            try
            {
                SmtpClient myEmail = new SmtpClient();  // ("localhost");
                MailMessage myMsg = new MailMessage();
                MailAddress toAddr = new MailAddress(ParentEmail);
                MailAddress fmAddr = new MailAddress("gatekeeper@myschoolaccount.com", "myschoolaccount.com");

                myEmail.Host = ConfigurationManager.AppSettings["SmtpServer"];
                myEmail.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                myEmail.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserMail"], ConfigurationManager.AppSettings["UserPassword"]);
                myEmail.EnableSsl = true;

                myMsg.To.Add(toAddr);
                myMsg.From = fmAddr;
                myMsg.Subject = "MySchoolAccount Password Reset";
                myMsg.Body = "Parent Account - UserID: " + UserID + "\r\n" +
                                  "Your password has been reset to: " + NewPassword + "\r\n\n" +
                                  "Login to https://www.myschoolaccount.com with the new password\r\n" +
                                  "and click on [Manage My Account] and change your password.";

                myEmail.Send(myMsg);
                retValue = true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return retValue;
        }

        private List<SearchBy> GetSearchDDLItems()
        {
            List<SearchBy> list = new List<SearchBy>();
            list.Add(new SearchBy { id = "1", name = "Last Name" });
            list.Add(new SearchBy { id = "2", name = "User ID" });
            list.Add(new SearchBy { id = "3", name = "Email" });
            return list;
        }

        public ActionResult ParentDetailPopup()
        {
            return PartialView("ParentDetailPopup");
        }

        public ActionResult StudentDetailPopup()
        {
            return PartialView("StudentDetailPopup");
        }

        public JsonResult GetLowBalanceSettings(int parentId)
        {
            try
            {
                var parent = ParentFactory.GetParent(parentId);
                return Json(new
                {
                    BalNotify = parent.BalNotify,
                    PaymentNotify = parent.PaymentNotify,
                    VIPNotify = parent.VIPNotify,
                    PreorderNotify = parent.PreorderNotify,
                    Email = (string.IsNullOrEmpty(parent.NotifyEmail) || parent.NotifyEmail.Length < 6) ? parent.Email : parent.NotifyEmail
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetLowBalanceSettings");
                return null;
            }
        }

        public ActionResult AjaxLowBalStudentsList(int parentId)
        {
            try
            {
                long clientId = ClientInfoData.GetClientID();
                List<LowBalanceStudent> lowbalStudList = new List<LowBalanceStudent>();
                lowbalStudList = ParentFactory.GetLowBalStudentsList(clientId, parentId);

                var result = from s in lowbalStudList
                             select new[] { s.StudentId.ToString(), s.StudentName, s.CurrentBalance.ToString(), s.MinimumBalance.ToString(), s.IsNotifyEnabled.ToString() };
                
                return Json(new
                {
                    aaData = result
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxLowBalStudentsList");
                return null;
            }
        }

        [HttpPost]
        public ActionResult SaveLowBalanceSettings(Parent mainSettings, List<LowBalanceStudent> students)
        {
            try
            {
                int parentId = mainSettings.Id;
                int districtId = Convert.ToInt32(ClientInfoData.GetClientID());
                bool balNotify = mainSettings.BalNotify.HasValue ? mainSettings.BalNotify.Value : false;
                bool paymentNotify = mainSettings.PaymentNotify.HasValue ? mainSettings.PaymentNotify.Value : false;
                bool vipNotify = mainSettings.VIPNotify.HasValue ? mainSettings.VIPNotify.Value : false;
                bool preorderNotify = mainSettings.PreorderNotify.HasValue ? mainSettings.PreorderNotify.Value : false;
                string email = mainSettings.Email;

                DataTable studentsDT = new DataTable();
                studentsDT.Columns.Add("District_Id", typeof(int));
                studentsDT.Columns.Add("Student_Id", typeof(int));
                studentsDT.Columns.Add("MinimumBal", typeof(decimal));
                studentsDT.Columns.Add("LBNEnabled", typeof(bool));

                foreach (var student in students)
                {
                    DataRow newRow = studentsDT.NewRow();
                    newRow["District_Id"] = districtId;
                    newRow["Student_Id"] = student.StudentId;
                    newRow["MinimumBal"] = this.calculateMinimumBalance(student.IsNotifyEnabled, student.MinimumBalance);
                    newRow["LBNEnabled"] = student.IsNotifyEnabled;

                    studentsDT.Rows.Add(newRow);
                }

                var result = AdminFactory.SaveLowBalNotifSettings(districtId, parentId, balNotify, paymentNotify, vipNotify, preorderNotify, email, studentsDT);

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxLowBalStudentsList");
                return null;
            }
        }

        [HttpPost]
        public JsonResult SendLowBalTestEmail(string email)
        {
            try
            {
                string MessageBody = string.Empty;
                var sr = new StreamReader(this.Server.MapPath("~/Views/TestNotificationEmail.htm"));
                sr = System.IO.File.OpenText(this.Server.MapPath("~/Views/TestNotificationEmail.htm"));
                MessageBody = sr.ReadToEnd();
                sr.Close();

                var mAff = new SmtpClient();
                var mAffMessage = new MailMessage();
                var mAffFrom = new MailAddress("administrator@myschoolaccount.com", "myschoolaccount.com");
                string mailTo = email;
                if (mailTo.Contains(";"))
                {
                    string[] tos = mailTo.Split(';');

                    for (int i = 0; i < tos.Length; i++)
                    {
                        mAffMessage.To.Add(new MailAddress(tos[i].Trim()));
                    }
                }
                else
                {
                    mAffMessage.To.Add(new MailAddress(mailTo));
                }

                mAff.Host = ConfigurationManager.AppSettings["SmtpServer"];
                mAff.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                mAff.Credentials = new NetworkCredential(Convert.ToString(ConfigurationManager.AppSettings["UserMail"]), Convert.ToString(ConfigurationManager.AppSettings["UserPassword"]));

                mAffMessage.IsBodyHtml = true;
                mAffMessage.Body = MessageBody;
                mAffMessage.Subject = "MSA TEST EMAIL";
                mAffMessage.From = mAffFrom;
                mAffMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                mAff.EnableSsl = true;
                mAff.Send(mAffMessage);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ParentsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SendLowBalTestEmail");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        private decimal calculateMinimumBalance(bool isNotifyEnabled, decimal balance)
        {
            decimal LBNAmount = balance;
            if (!isNotifyEnabled)
            {
                LBNAmount = 999999.99m; // disables student LBN
            }

            if (LBNAmount < 999999.0m && LBNAmount > -999999.0m)
            {
                return LBNAmount;
            }
            else
            {
                return 999999.99m;
            }
        }
    }
}

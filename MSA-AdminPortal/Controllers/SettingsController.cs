using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdminPortalModels.ViewModels;
using Repository;
using Repository.edmx;
using Repository.Helpers;
using MSA_AdminPortal.Helpers;
using MSA_ADMIN.DAL.Factories;
using System.Web.Script.Serialization;
using MSA_ADMIN.DAL.Models;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace MSA_AdminPortal.Controllers
{
    public class SettingsController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        private HomeRoomHelper homeroomHelper = new HomeRoomHelper();
        private POSHelper helper = new POSHelper();

        public SettingsController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            try
            {
                long ClientID = ClientInfoData.GetClientID();
                int totalRecords;
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc

                var posList = unitOfWork.settingsRepository.GetPOSListPage(ClientID, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, out totalRecords);  // settingsRepository.GetPOSbySchoolID(null, ClientID).ToList();

                var result = posList.Select(x => new string[] { x.Id.ToString(), x.Id.ToString(), x.Name, x.SchoolName, x.SessionStatus, x.VeriFoneUserId, x.enbCCCCProcessing.ToString() });

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = result
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxHandler");
                return null;
            }
        }

        public ActionResult Districts(int? id)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                long districtIdIn = -1;
                if (id.HasValue)
                {
                    districtIdIn = (int)id;
                }
                else
                {
                    districtIdIn = unitOfWork.settingsRepository.GetFirstDistrictID(ClientId);
                }

                if (districtIdIn == -1)
                {
                    return View("Index");
                }
                else
                {
                    CustomerHelper customerHelper = new CustomerHelper();
                    var districtdata = unitOfWork.settingsRepository.getDistrictsData(ClientId, districtIdIn);
                    List<string> districtStates = new List<string>();
                    districtStates.Add("");
                    foreach (var item in ClientInfoData.GetStates())
                    {
                        districtStates.Add(item);
                    }
                    ViewBag.ListOfStates = districtStates;
                    ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

                    ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
                    ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
                    ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == ClientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);

                    districtdata.DirectorName = customerHelper.GetSingleCustomer(districtdata.DistrictVM.empDirector == null ? 0 : Convert.ToInt32(districtdata.DistrictVM.empDirector));
                    districtdata.AdminName = customerHelper.GetSingleCustomer(districtdata.DistrictVM.empAdmin == null ? 0 : Convert.ToInt32(districtdata.DistrictVM.empAdmin));

                    return View(districtdata);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Districts");
                return View("Index");
            }
        }

        [HttpPost]
        public ActionResult Districts(string allData)
        {
            try
            {
                int distId = 0;
                unitOfWork.settingsRepository.SaveDistrictData(allData, out distId);
                List<string> districtStates = new List<string>(ClientInfoData.GetStates());
                ViewBag.ListOfStates = districtStates;
                var districtdata = unitOfWork.settingsRepository.getDistrictsData(ClientInfoData.GetClientID(), distId);
                return View(districtdata);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Districts");
                return null;
            }
        }

        [HttpPost]
        public JsonResult updateDistrict(string allData)
        {
            try
            {
                int distId = 0;
                string disdata = unitOfWork.settingsRepository.SaveDistrictData(allData, out distId);
                return Json(new { result = disdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updateDistrict");
                return null;
            }
        }

        [HttpPost]
        public JsonResult activateDistrict(string allData)
        {
            try
            {
                string disdata = unitOfWork.settingsRepository.ReactivateDistrict(allData);
                return Json(new { result = disdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "activateDistrict");
                return null;
            }
        }

        [HttpPost]
        public JsonResult updatePOS(string allData)
        {
            try
            {
                string disdata = unitOfWork.settingsRepository.SavePOSData(allData);
                return Json(new { result = disdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updatePOS");
                return null;
            }
        }

        public ActionResult Schools_Index(int? id)
        {
            try
            {
                var school = unitOfWork.settingsRepository.GetDistricts(ClientInfoData.GetClientID()).Select(s => new
                   SchoolIndexVM()
                   {
                       DistrictName = s.DistrictName,
                       Id = s.ID,
                       ClientID = s.ClientID,
                       SchoolIndexData = unitOfWork.settingsRepository.GetSchoolbyDistrictID(s.ID, s.ClientID).ToList()
                   }
                   );
                ViewBag.Layout = 0;
                if (id.HasValue)
                    ViewBag.Layout = id;
                return View(school);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Schools_Index");
                return View();
            }
        }

        [HttpPost]
        public JsonResult DeleteSchool(string allData)
        {
            try
            {
                string resultdata = unitOfWork.settingsRepository.DeleteSchool(allData);
                return Json(new { result = resultdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteSchool");
                return null;
            }
        }

        [HttpPost]
        public JsonResult updateSchools(string allData)
        {
            try
            {
                int schoolId = 0;
                string schdata = unitOfWork.settingsRepository.SaveSchoolData(allData, out schoolId);
                return Json(new { result = schdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updateSchools");
                return null;
            }
        }

        public ActionResult Schools(long? id)
        {
            long ClientId = ClientInfoData.GetClientID();
            try
            {
                long schoolIdIn = -1;
                if (id.HasValue)
                {
                    schoolIdIn = (int)id;
                }
                else
                {
                    schoolIdIn = unitOfWork.settingsRepository.GetFirstSchoolID(ClientId);
                }

                if (schoolIdIn == -1)
                {
                    return View("Index");
                }
                else
                {
                    var schoolData = unitOfWork.settingsRepository.getSchoolsData(ClientId, schoolIdIn);
                    if (schoolData.SchoolOptionsVM.StartSchoolYear == null)
                        schoolData.SchoolOptionsVM.StartSchoolYear = schoolData.DistrictDates.StartSchoolYear;
                    if (schoolData.SchoolOptionsVM.EndSchoolYear == null)
                        schoolData.SchoolOptionsVM.EndSchoolYear = schoolData.DistrictDates.EndSchoolYear;
                    List<string> States = new List<string>(ClientInfoData.GetStates());
                    ViewBag.ListOfStates = States;

                    var DistrictList = new List<SelectListItem>();
                    foreach (var dist in unitOfWork.settingsRepository.getDistrictNames(ClientId).ToList())
                    {
                        var item = new SelectListItem();
                        item.Value = dist.districtId.ToString();
                        item.Text = dist.districtName;
                        if (dist.districtId == schoolData.SchoolVM.District_Id)
                            item.Selected = true;
                        DistrictList.Add(item);
                    }

                    var DistExecs = unitOfWork.settingsRepository.getDistrictExecs(schoolData.SchoolVM.ClientID, schoolData.SchoolVM.District_Id);

                    if (DistExecs != null)
                    {
                        ViewBag.DirectorName = unitOfWork.settingsRepository.getCustomerName(schoolData.SchoolVM.ClientID, DistExecs.Emp_Director_Id);
                        ViewBag.AdminName = unitOfWork.settingsRepository.getCustomerName(schoolData.SchoolVM.ClientID, DistExecs.Emp_Administrator_Id);
                    }
                    else
                    {
                        ViewBag.DirectorName = string.Empty;
                        ViewBag.AdminName = string.Empty;
                    }

                    var customers = unitOfWork.settingsRepository.getAdultCustomers(ClientId);

                    ViewBag.AdultCustomers = customers.ToList();

                    ViewBag.ListOfDistricts = DistrictList;
                    return View(schoolData);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Schools");
                return View();
            }
        }

        public ActionResult pos()
        {
            if (Request.Cookies["POSView"] != null && Encryption.Decrypt(Request.Cookies["POSView"].Value).ToLower() == "table")
            {
                return RedirectToAction("POSTable");
            }

            return RedirectToAction("POSTile");
        }

        public ActionResult msa()
        {
            if (!SecurityManager.viewMSASettings)
            {
                return RedirectToAction("NoAccess", "Security", new { id = "nomsasettings" });
            }

            long districtId = ClientInfoData.GetClientID();
            ViewBag.DistrictId = districtId;

            MSA_ADMIN.DAL.Models.DistrictOption districtOption = DistrictFactory.GetDistrictOption(districtId);
            MSA_ADMIN.DAL.Models.District district = DistrictFactory.GetDistrict(districtId);
            if (district != null)
            {
                district.BankAccount = MSAHelper.HideAccountNumber(district.BankAccount);
            }

            DistrictAndDistrictOptionVM districtAndOption = new DistrictAndDistrictOptionVM();
            districtAndOption.District = district;
            districtAndOption.DistrictOption = districtOption;

            // For dropdowns
            List<MSA_ADMIN.DAL.Models.School> schools = SettingsFactory.GetSchools(districtId);
            var preorderTaxableSchools = schools.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.SchoolName, Selected = s.isPreorderTaxable ?? false });
            var easyPayTaxableSchools = schools.Select(s => new SelectListItem { Value = Convert.ToString(s.Id), Text = s.SchoolName, Selected = s.isEasyPayTaxable ?? false });

            ViewBag.PreorderTaxableSchools = preorderTaxableSchools;
            ViewBag.EasyPayTaxableSchools = easyPayTaxableSchools;

            return View("msa", districtAndOption);
        }

        public string AjaxGetDistrict(long districtId)
        {
            try
            {
                MSA_ADMIN.DAL.Models.District district = DistrictFactory.GetDistrict(districtId);

                // Show only last 4 characters of Account Number
                if (district != null)
                {
                    district.BankAccount = MSAHelper.HideAccountNumber(district.BankAccount);
                }

                bool? useVariableCCFee = false;

                MSA_ADMIN.DAL.Models.DistrictOption districtOption = DistrictFactory.GetDistrictOption(districtId);
                if (districtOption != null)
                {
                    useVariableCCFee = districtOption.useVariableCCFee;
                }

                var result = new { dist = district, UseCCFee = useVariableCCFee };

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

        public int AjaxUpdateDistrictSettings(string dataToUpload)
        {
            int result = 0;
            //string districtID = Convert.ToString(ClientInfoData.GetClientID());
            string[] districtFields = dataToUpload.Split('*');
            if (districtFields.Length == 12)
            {
                string contactName = districtFields[0].Replace("'", "''");
                string contactNumber = districtFields[1].Replace("'", "''");
                string contactEmail = districtFields[2].Replace("'", "''");
                bool lowBalanceNotification = Convert.ToBoolean(districtFields[3]);// DateTime.ParseExact(, "d/M/yyyy", CultureInfo.InvariantCulture);
                bool allowStudentTransfers = Convert.ToBoolean(districtFields[4]);
                bool studentAttachment = districtFields[5] == "N/A" ? false : Convert.ToBoolean(districtFields[5]);
                bool displayVoids = Convert.ToBoolean(districtFields[6]);
                bool displayAdjustments = Convert.ToBoolean(districtFields[7]);
                string easyPayTax_SelectedValues = districtFields[8];
                string preorderTax_SelectedValues = districtFields[9];
                bool cutOff_5 = districtFields[10] == "N/A" ? false : Convert.ToBoolean(districtFields[10]);
                bool forcePaymentNegBalance = districtFields[11] == "N/A" ? false : Convert.ToBoolean(districtFields[11]);

                result = SettingsFactory.UpdateDistrictInformation(ClientInfoData.GetClientID(), contactName, contactNumber, contactEmail,
                                                    lowBalanceNotification, allowStudentTransfers, studentAttachment,
                                                    displayVoids, displayAdjustments, easyPayTax_SelectedValues,
                                                    preorderTax_SelectedValues, cutOff_5, forcePaymentNegBalance);
            }
            return result;
        }

        public int AjaxUpdateDistrictOptions(string dataToUpload)
        {
            string[] fildsToUpdate = dataToUpload.Split('*');

            int result = 0;

            if (fildsToUpdate.Length == 2)
            {
                bool validatePreorderItemStatus = Convert.ToBoolean(fildsToUpdate[0]);
                bool allowPreorderNegBalances = Convert.ToBoolean(fildsToUpdate[1]);

                result = SettingsFactory.UpdateDistrictOptions((int)ClientInfoData.GetClientID(), validatePreorderItemStatus, allowPreorderNegBalances);
            }

            return result;
        }

        public int AjaxUpdateCommunicationOptions(string dataToUpload)
        {
            bool msaAlert = Convert.ToBoolean(dataToUpload);

            int result = 0;
            result = SettingsFactory.UpdateCommunicationOptions((int)ClientInfoData.GetClientID(), msaAlert);
            return result;
        }

        public int AjaxRequestAttention(string allData)
        {
            string[] emailinfo = allData.Split('*');
            bool emailSent = false;
            int retValue = 1;
            if (emailinfo.Length == 5)
            {
                string name = emailinfo[0];
                string email = emailinfo[1];
                string attentionType = emailinfo[2];//1 0
                string changeType = emailinfo[3];//1 0
                string changeBank = emailinfo[4];//1 0
                if (attentionType == "payment")
                {
                    emailSent = SendEmailToAdmin(changeType == "1" ? true : false, false, email, name);
                    //SendEmailToAdmin(name);
                }
                else if (attentionType == "bankInfo")
                {
                    emailSent = SendEmailToAdmin(false, changeBank == "1" ? true : false, email, name);
                    //SendEmailToAdmin(name);
                }

                if (emailSent == false)
                {
                    retValue = 0;
                }
            }
            return retValue;
        }

        public bool SendEmailToAdmin(bool chageType, bool changeBank, string email, string name)
        {
            string changePaymentType = "Change Payment Type";
            string changeBankInformation = "Change Bank Information";
            string subject = string.Empty;
            if (chageType && changeBank)
            {
                subject = changePaymentType + "," + changeBankInformation;
            }
            else if (chageType && !changeBank)
            {
                subject = changePaymentType;
            }
            else if (!chageType && changeBank)
            {
                subject = changeBankInformation;
            }

            bool retValue = false;
            try
            {
                SmtpClient myEmail = new SmtpClient();  // ("localhost");
                MailMessage myMsg = new MailMessage();
                MailAddress toAddr = new MailAddress(ConfigurationManager.AppSettings["ToEmailChangeBankInfo"]);
                MailAddress fmAddr = new MailAddress("gatekeeper@myschoolaccount.com", "myschoolaccount.com");

                myEmail.Host = ConfigurationManager.AppSettings["SmtpServer"];
                myEmail.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                myEmail.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserMail"], ConfigurationManager.AppSettings["UserPassword"]);
                myEmail.EnableSsl = true;

                myMsg.To.Add(toAddr);
                myMsg.From = fmAddr;
                myMsg.Subject = subject;
                myMsg.Body = "Dear Admin, \r\n District Account - DistrictID: " + Convert.ToString(ClientInfoData.GetClientID()) +
                                  "requested to " + subject + ". Please contact " + name + " to proceed with the request. ";

                myEmail.Send(myMsg);
                retValue = true;
            }
            finally
            {
                //retValue = false;
            }

            return retValue;
        }

        public ActionResult POSTable()
        {
            if (!SecurityManager.viewPOS) return RedirectToAction("NoAccess", "Security", new { id = "nopos" });
            Response.Cookies["POSView"].Value = Encryption.Encrypt("Table");

            return View();
        }

        public ActionResult POSTile()
        {
            if (!SecurityManager.viewPOS) return RedirectToAction("NoAccess", "Security", new { id = "nopos" });

            Response.Cookies["POSView"].Value = Encryption.Encrypt("Tile");

            try
            {
                long ClientID = ClientInfoData.GetClientID();

                // Inatay [6-Sep-2016] -- Optimized the code in this function. Previously a separate query was executing for every
                // school to load its POS List. So the number of queries hitting database for loading POS list was equal to the number
                // of schools. After optimization I load all the data at once.
                int totalRecords;
                var posList = unitOfWork.settingsRepository.GetPOSList(ClientID, out totalRecords).OrderBy(x => x.SchoolName);
                List<SchoolVM> schools = new List<SchoolVM>();
                foreach (var pos in posList)
                {
                    if (!schools.Any(s => s.SchoolName == pos.SchoolName))
                    {
                        schools.Add(new SchoolVM()
                        {
                            SchoolName = pos.SchoolName,
                            POSVM = posList.Where(x => x.SchoolName == pos.SchoolName).ToList()
                        });
                    };
                }

                var AllSchools = new POSPageVM
                {
                    SchoolsList = schools,
                    allPOSCount = totalRecords
                };
                return View(AllSchools);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "POSTile");
                return View();
            }
        }

        [HttpDelete]
        [ActionName("DeletePOS")]
        public ActionResult DeletePOSConfirm(int id = 0)
        {
            try
            {
                Admin_POS_Delete_Result resultdata = unitOfWork.settingsRepository.DeletePOS(ClientInfoData.GetClientID().ToString() + "*" + id.ToString());
                return Json(new { IsError = resultdata.Result,Message=resultdata.ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeletePOS");
                return null;
            }
        }

        [HttpDelete]
        //[ChildActionOnly]
        [ActionName("Delete")]
        public ActionResult DeleteConfirm(int id = 0)
        {
            var model = helper.GetDeleteModel(id, false);

            try
            {
                if (!model.IsError)
                {
                    long ClientID = ClientInfoData.GetClientID();
                    string sessionStatus = unitOfWork.settingsRepository.SessionStatus(id, ClientID);
                    if (sessionStatus.ToLower() == "open")
                    {
                        helper.SetErrors(model, "This POS is currently in use and cannot be deleted. Please make sure the cashier is cashed out of this POS before attempting to delete it.");
                    }
                    else
                    {
                        helper.SoftDelete(id);
                        model.Message = "The POS has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
            }

            return GetActionResult(Request, model, false);
        }

        public ActionResult EditPOS(int id = 0)
        {
            var model = helper.GetEditModel(id);

            return GetActionResult(Request, model);
        }

        public ActionResult Delete(int id = 0)
        {
            var model = helper.GetDeleteModel(id, true);

            return GetActionResult(Request, model);
        }

        public ActionResult DeletePOS(int id = 0)
        {
            var model = helper.GetDeleteModel(id, true);

            return GetActionResult(Request, model);
        }

        private ActionResult GetActionResult(HttpRequestBase request, POSUpdateModel viewModel, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            if (viewModel.IsError || isGetAction)
            {
                return PartialView("Popup", viewModel);
            }

            return RedirectToAction("Index");
        }

        private ActionResult GetActionResult(HttpRequestBase request, POSDeleteModel model, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            if (model.IsError || isGetAction)
            {
                return PartialView("_Delete", model);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.settingsRepository.Dispose();
            base.Dispose(disposing);
        }
    }

    public class POSHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        // get
        public POSDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new POSDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }

        // post
        public POSDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetDeleteModelOnError();
            }

            return new POSDeleteModel
            {
                Id = entity.ID,
                Name = entity.Name,
                Message = errorMessage,
                IsError = !string.IsNullOrWhiteSpace(errorMessage),
            };
        }

        // get
        public POSUpdateModel GetEditModel(int id)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetEditModelOnError();
            }

            return new POSUpdateModel
            {
                ClientID = entity.ClientID,
                Id = entity.ID,
                Name = entity.Name,
                EnableCCProcessing = entity.EnableCCProcessing,
                VeriFoneUserId = entity.VeriFoneUserId,
                VeriFonePassword = "temppass",
                School_Id = entity.School_Id
            };
        }

        public POSUpdateModel GetEditModelOnError()
        {
            return new POSUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        public POSDeleteModel GetDeleteModelOnError()
        {
            return new POSDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        public POS Get(int id)
        {
            return GetAll().Where(x => x.ID == id).FirstOrDefault();
        }

        public IQueryable<POS> GetAll()
        {
            try
            {
                return unitOfWork.POSRepository.GetQuery(x => (x.isDeleted.Equals(null) || x.isDeleted == false) && x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        // for delete
        public void SetErrors(POSDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SoftDelete(int id = 0)
        {
            var entity = Get(id);

            SoftDelete(entity);
        }

        public void SoftDelete(POS entity)
        {
            try
            {
                entity.isDeleted = true;

                Update(entity);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SoftDelete");
            }
        }

        public void Update(POS entity)
        {
            try
            {
                unitOfWork.POSRepository.Update(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables s
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Update");
            }
        }
    }

    public class MSAHelper
    {
        public static string HideAccountNumber(string orginal)
        {
            string modified = string.Empty;
            modified = orginal;
            if (!string.IsNullOrEmpty(orginal) && orginal.Length > 4)
            {
                modified = orginal.Remove(0, (orginal.Length - 4)).PadLeft(orginal.Length, '*');
            }
            return modified;
        }

        private string GetStatusHtml(bool isAllowed)//string typeAllowed
        {
            string status = "";
            string img = "";

            if (isAllowed)
            {
                status = "ENABLED";
                img = "../Images/circle-green.png";
            }
            else
            {
                status = "DISABLED";
                img = "../Images/circle-gray.png";
            }

            return "<span><img height='10' width='10' style='margin-bottom:2px' src=''" + img + "' /></span> " + status;
        }
    }
}
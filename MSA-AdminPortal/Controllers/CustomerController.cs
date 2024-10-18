using AdminPortalModels.ViewModels;
using Repository;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using AdminPortalModels.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using MSA_AdminPortal.Helpers;
using System.Configuration;


namespace MSA_AdminPortal.Controllers
{
    public class CustomerController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        private ICustomerRepository customerRepository;
        private IGeneralRepository generalRepository;
        private HomeRoomHelper homeroomHelper = new HomeRoomHelper();
        private string[] emptyStringArray = new string[0];

        /// <summary>
        /// Constructer for class
        /// </summary>
        public CustomerController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
            this.customerRepository = unitOfWork.CustomCustomerRepository;
            this.generalRepository = unitOfWork.generalRepository;
        }

        /// <summary>
        /// Initially populates model
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!SecurityManager.viewCustomers) return RedirectToAction("NoAccess", "Security", new { id = "nocustomers" });
            var clientId = ClientInfoData.GetClientID();
            try
            {
                ViewBag.SearchByList = GetSearchDDLItems();
                ViewBag.GradeList = unitOfWork.generalRepository.getGrades(clientId).ToList();
                ViewBag.ActiveList = GetActiveDDLItems();
                ViewBag.AdultList = GetAdultDDLItems();
                ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
                ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Index");
            }
            return View();
        }

        /// <summary>
        /// Populates model in edit mode
        /// </summary>
        /// <param name="id"></param> for customer id
        /// <returns></returns>
        public ActionResult Edit(int? id)
        {
            long ClientId = ClientInfoData.GetClientID();
            try
            {
                int customerId = 0;
                Int64 schID = 0;
                ViewBag.CustomerBreadCrumTitle = "";
                if (id.HasValue)
                {
                    customerId = Convert.ToInt16(id);
                }
                if (customerId == 0)
                {
                    ViewBag.CustomerModeTitle = "Add New Customer";
                    ViewBag.CustomerBreadCrumTitle = "Customers";
                    ViewBag.IsCustomerImageExist = false;

                }
                else
                {
                    ViewBag.CustomerModeTitle = "Edit Customer";
                    ViewBag.CustomerBreadCrumTitle = "Customers";
                    bool IsImageExist = AzureStorageHelper.checkCutomerPicture("customerspics", ClientId.ToString(), id.ToString());
                    if (IsImageExist)
                    {
                        ViewBag.IsCustomerImageExist = true;
                    }
                    else
                    {
                        ViewBag.IsCustomerImageExist = false;
                    }

                }

                var OneCustomer = customerRepository.GetCustomer(ClientId, customerId);
                var GendersList = generalRepository.getGendersList().ToList();
                var StatesList = generalRepository.getStates().ToList();
                var Languages = generalRepository.getLanguages(ClientId).ToList();
                var Ethnicities = generalRepository.getEthnicities(ClientId).ToList();
                var Grades = generalRepository.getGrades(ClientId).ToList();

                ICollection<SchoolItem> Schools = null;
                if (customerId == 0)
                {
                    Schools = generalRepository.getSchools(ClientId, null, false).ToList();
                    OneCustomer.LunchType = 1; // By default Meal Plan is Paid/Standard so therefore set LunchType to 1
                }
                else
                {
                    Schools = generalRepository.getSchools(ClientId, OneCustomer.District_Id, false).ToList();
                }

                if (Schools.Count() > 0)
                {
                    schID = Schools.DefaultIfEmpty().Select(s => s.value).FirstOrDefault();
                }

                var HomeRooms = homeroomHelper.GetSelectList(OneCustomer.Homeroom_Id.HasValue ? OneCustomer.Homeroom_Id.Value : 0, OneCustomer.School_Id.HasValue ? OneCustomer.School_Id.Value : schID);

                long distID = customerRepository.GetSchoolsDistrict(ClientId, OneCustomer.School_Id.HasValue ? OneCustomer.School_Id.Value : schID);
                var Districts = generalRepository.getDistricts(ClientId).ToList();
                var assignedSchools = customerRepository.GetAssignedSchools(ClientId, customerId).ToList();
                var eatingSchools = customerRepository.GetEatingSchools(ClientId, distID, false, OneCustomer.School_Id.HasValue ? OneCustomer.School_Id.Value : schID).ToList();

                // TO DO : Fix it after Abid Check in 
                Int32 FreeRedApp_count = 0;//customerRepository.FARM_AppCount(ClientId,customerId);

                var model = new SingleCustomer
                {
                    ClientID = ClientId,
                    Customer = OneCustomer,
                    GendersList = GendersList,
                    StatesList = StatesList,
                    Languages = Languages,
                    Ethnicities = Ethnicities,
                    Grades = Grades,
                    HomeRooms = HomeRooms,
                    Schools = Schools,
                    Districts = Districts,
                    assignedSchools = assignedSchools,
                    eatingSchools = eatingSchools,
                    pictureStoragepath = ConfigurationManager.AppSettings["CustomerPicturesPath"].ToString(),
                    FreeReducedAppCount = FreeRedApp_count
                };
                return View(model);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
            }
            return View();

        }
        /// <summary>
        ///  delete customer data
        /// </summary>
        /// <param name="allData"></param> customer information
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCustomer(string allData)
        {
            try
            {
                string[] fildsToUpdate = allData.Split('*');
                string customerId = "";

                if (fildsToUpdate.Length == 1)
                {
                    customerId = fildsToUpdate[0];
                }
                long ClientId = ClientInfoData.GetClientID();
                string OverrideBalance = "False";
                string OverrideEmployee = "False";

                string OverrideOrders = "False";
                string OverrideBonusPayments = "False";
                string OverridePreorders = "False";
                string OverrideStudentApp = "False";

                string modifiedData = Convert.ToString(ClientId) + "*" + customerId + "*" + OverrideBalance + "*" + OverrideEmployee + "*" + OverrideOrders + "*" + OverrideBonusPayments + "*" + OverridePreorders + "*" + OverrideStudentApp;
                string resultdata = customerRepository.DeleteCustomer(modifiedData);

                if (resultdata == "")
                {
                    DeleteUsers(Convert.ToInt64(customerId.Trim()));
                    return Json(new { result = "-1" });
                }
                else
                {
                    return Json(new { result = resultdata });
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
                return Json(new { result = "-1" });
            }
        }

        private void DeleteUsers(long customerId)
        {
            var clientId = ClientInfoData.GetClientID();
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();

            int unDeleteUserResult = 0;
            bool unDeleteUserSpecified = false;

            client.DeleteUser(clientId, true, customerId, true, out unDeleteUserResult, out unDeleteUserSpecified);
        }

        /// <summary>
        /// This procedure gets data from ajax post and saves in database 
        /// </summary>
        /// <param name="customerData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult updateCustomer(CustomerData customerData)
        {

            CustomerData customer = new CustomerData();
            var jsSer = new JavaScriptSerializer();
            string cstatus = "old";

            if (Request["customerData"] != null)
            {
                customer = new JavaScriptSerializer().Deserialize<CustomerData>(Request["customerData"]);
            }
            else
            {
                customer = customerData;
            }

            if (customer.custId == "0")
            {
                cstatus = "new";
            }

            if (customer.distid == "0" || customer.distid == null)
            {
                long clientID = Convert.ToInt64(customer.clientId);
                long schoolID = Convert.ToInt32(customer.schoolid);
                long distid = customerRepository.GetSchoolsDistrict(clientID, schoolID);
                customer.distid = distid.ToString();
            }
            //save data
            int customerId = 0;
            string retval = "";
            try
            {
                if (Request.Files.Count > 0)
                {
                    // File we want to resize and save.
                    var file = Request.Files[0];
                    try
                    {
                        retval = customerRepository.SaveCustomerData(customer, out customerId);
                        var filename = UploadFile(file, customer.clientId, Convert.ToString(customerId));

                    }
                    catch (Exception ex)
                    {
                        //Error logging in cloud tables 
                        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updateCustomer");
                        string message = ex.Message;
                    }
                }
                else
                {
                    retval = customerRepository.SaveCustomerData(customer, out customerId);
                }
                if (retval == "-1")
                {
                    cstatus = "error";
                }
                else if (retval == "-999")
                {
                    cstatus = "duplicateUserid";
                }
                else if (retval == "-1000")
                {
                    cstatus = "duplicatePIN";
                }

                //updating the customer id=
                var client = new RegistrationService.Registration();
                int UpdateUserCustomerNameResult = 0;
                bool UpdateUserCustomerNameSpecified = true;
                client.UpdateUserCustomerName(ClientInfoData.GetClientID(), true, Convert.ToInt32(customerData.custId), true, customerData.firstname, customerData.lastname, out UpdateUserCustomerNameResult, out UpdateUserCustomerNameSpecified);

                // Return JSON
                return Json(new
                {
                    CustomerID = customerId,
                    status = cstatus
                }, "text/html");

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updateCustomer");
                // Return JSON
                return Json(new
                {
                    CustomerID = customerId,
                    status = cstatus
                }, "text/html");
            }
        }


        /// <summary>
        /// Dynamically renders data table of customers
        /// </summary>
        /// <param name="param"></param>
        /// <param name="filters"></param>
        /// <returns></returns>

        public ActionResult AjaxHandler(JQueryDataTableParamModel param, CustomerFilters filters)
        {
            try
            {
                // don't load data on first
                if ((string.IsNullOrWhiteSpace(filters.SearchBy)
                    && !filters.SearchBy_Id.HasValue // || (filters.SearchBy_Id.HasValue && filters.SearchBy_Id.Value == 0))
                    && string.IsNullOrWhiteSpace(filters.SchoolStr)
                    && string.IsNullOrWhiteSpace(filters.HomeRoomStr)
                    && string.IsNullOrWhiteSpace(filters.GradeStr)
                    && string.IsNullOrWhiteSpace(filters.ActiveStr)
                    && string.IsNullOrWhiteSpace(filters.AdultStr)
                    ))
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = emptyStringArray
                    },
                        JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //ViewBag.SearchBy_Id = filters.SearchBy_Id;
                    Session["CustomerFilters"] = filters;
                }

                int totalRecords;
                long ClientId = ClientInfoData.GetClientID();
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc
                filters.IsBalanceRequired = true;
                var customerData = customerRepository.GetCustomerPage(ClientId, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, filters, out totalRecords);

            

                if (customerData != null)
                {

                                       
                    var result = from c in customerData
                                 select new[] { (!(c.Active.HasValue && c.Active.Value) ? "<i  title=\"Activate\" class=\"fa fa-user-plus fasize faActiveColr\"></i>" : "<i  title=\"Deactivate\" class=\"fa fa-user-times fasize fadeactiveColr \"></i>"), c.id.ToString(), c.UserID, c.Last_Name, c.First_Name, c.Middle_Initial, c.Adult.ToString(), c.Grade, c.Homeroom, c.School_Name, c.PIN, string.Format("{0:C}", c.Total_Balance), c.id.ToString() };

                    
                    
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = totalRecords,
                        iTotalDisplayRecords = totalRecords,
                        aaData = result
                    },
                    JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = totalRecords,
                        iTotalDisplayRecords = totalRecords,
                        aaData = emptyStringArray
                    },
                       JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "updateCustomer");
                return null;
            }
        }

        /// <summary>
        /// Dynamically renders data table of logs of a customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult LogAjaxHandler(int? id, JQueryDataTableParamModel param)
        {
            try
            {
                int totalRecords;
                if (Request["id"] != null)
                {
                    string idd = Request["id"].ToString();
                    long ClientId = ClientInfoData.GetClientID();
                    var customerLogData = customerRepository.GetCustomerLogs(ClientId, id, out totalRecords);

                    var result = from cL in customerLogData
                                 select new[] { 
                             cL.CDate ,cL.ChangedTime, (cL.Note == null) ? "" : cL.Note, cL.Employee_Name 

                         };
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = totalRecords,
                        iTotalDisplayRecords = totalRecords,
                        aaData = result
                    },
                JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = emptyStringArray
                    },
                    JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "LogAjaxHandler");
                return null;
            }
        }

        public ActionResult AjaxSearchHandler(JQueryDataTableParamModel param, CustomerFilters filters)
        {
            try
            {
                if (Convert.ToInt32(param.sEcho) > 1)
                {
                    int totalRecords;
                    long ClientId = ClientInfoData.GetClientID();
                    int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                    string sortDirection = Request["sSortDir_0"]; // asc or desc
                    filters.IsBalanceRequired = false;

                    //due to bug#1761
                    filters.ActiveStr = "active";

                    var customerData = customerRepository.GetCustomerPage(ClientId, param.iDisplayStart, param.iDisplayLength, sortColumnIndex + 1, sortDirection, filters, out totalRecords);

                    var result = from c in customerData
                                 select new[] { c.id.ToString(), c.UserID, c.Last_Name, c.First_Name, c.Middle_Initial, c.Adult.ToString(), c.Grade, c.Homeroom, c.School_Name, c.Total_Balance.ToString(), c.A_Balance.ToString(), c.M_Balance.ToString() };
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = totalRecords,
                        iTotalDisplayRecords = totalRecords,
                        aaData = result
                    },
                JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = emptyStringArray
                    },
                        JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxSearchHandler");
                return null;
            }
        }

        public ActionResult RemovePhoto(string CustomerID)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                bool isPhotoRemoved = AzureStorageHelper.removeCutomerPicture("customerspics", ClientId.ToString(), CustomerID);
                return Json(isPhotoRemoved, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "RemovePhoto");
                return null;
            }
        }

        private List<SearchBy> GetSearchDDLItems()
        {
            List<SearchBy> list = new List<SearchBy>();
            list.Add(new SearchBy { id = "0", name = "LN, FN, & USERID" });
            list.Add(new SearchBy { id = "1", name = "Last Name" });
            list.Add(new SearchBy { id = "2", name = "First Name" });
            list.Add(new SearchBy { id = "3", name = "User ID" });
            list.Add(new SearchBy { id = "4", name = "Grade" });
            list.Add(new SearchBy { id = "5", name = "Homeroom" });
            list.Add(new SearchBy { id = "6", name = "PIN" });
            return list;
        }
        private List<string> GetActiveDDLItems()
        {
            List<string> list = new List<string>();
            list.Add("Active");
            list.Add("Inactive");
            return list;
        }
        private List<string> GetAdultDDLItems()
        {
            List<string> list = new List<string>();
            list.Add("Yes");
            list.Add("No");
            return list;
        }




        /// <summary>
        /// Persist the file to disk.
        /// </summary>
        private string UploadFile(HttpPostedFileBase file, string ClienID, string CustomerID)
        {
            try
            {
                AzureStorageHelper.uploadCutomerPicture(ClienID, CustomerID, file);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "UploadFile");
                string message = ex.Message;
            }

            return "success";
        }

        private string SaveImageToDataBase(HttpPostedFileBase file, string ClienID, string CustomerID)
        {
            return "success";

        }
        /// <summary>
        /// Renders partial view named “Popup”
        /// </summary>
        /// <param name="request"></param>
        /// <param name="model"></param>
        /// <param name="isGetAction"></param>
        /// <returns></returns>
        private ActionResult GetActionResult(HttpRequestBase request, popUpCustomer model, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            if (model.IsError || isGetAction)
            {
                return PartialView("Popup", model);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Renders partial view named “Popup”
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [OutputCache(Duration = 1)]
        public ActionResult Popup(int? id)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                int customerId = 0;

                if (id.HasValue)
                {
                    customerId = Convert.ToInt16(id);

                }

                var model = new popUpCustomer
                {
                    ClientID = ClientId,
                    Customer = GetCustomerData(ClientId, customerId),
                    assignedSchools = customerRepository.GetAssignedSchools(ClientId, customerId).ToList(),
                    pictureStoragepath = ConfigurationManager.AppSettings["CustomerPicturesPath"].ToString(),
                    POSNotifications = unitOfWork.posNotificationsRepository.GetPOSNotificationByClientandCustomerID(ClientId, customerId),
                };


                if (Request.IsAjaxRequest())
                {
                    return Json(model, JsonRequestBehavior.AllowGet);
                }

                return PartialView("Popup", model);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Popup");
                return PartialView("Popup");
            }
        }

        [HttpPost]
        public JsonResult getDistrict(string allData)
        {
            try
            {
                string[] schoolData = allData.Split('*');
                string SchoolId = "";

                if (schoolData.Length == 1)
                {
                    SchoolId = schoolData[0];
                }
                long ClientId = ClientInfoData.GetClientID();

                long distid = customerRepository.GetSchoolsDistrict(ClientId, Convert.ToInt16(SchoolId));

                string distName = customerRepository.GetDistrictName(ClientId, distid);
                return Json(new { result = distid, districtName = distName });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistrict");
                return null;
            }
        }

        [HttpPost]
        public JsonResult CheckUserID(string allData)
        {
            try
            {
                string[] custData = allData.Split('*');
                int custId = 0;
                string userId = "";
                int distID = 0;

                if (custData.Length == 3)
                {
                    userId = custData[0];
                    custId = Convert.ToInt32(custData[1]);
                    distID = Convert.ToInt32(custData[2]);

                }
                long ClientId = ClientInfoData.GetClientID();

                bool alreadyExists = customerRepository.UserIdAlreadyExists(ClientId, userId, custId, distID);

                string returnResult = "yes";
                if (alreadyExists)
                {
                    returnResult = "yes";
                }
                else
                {
                    returnResult = "no";
                }
                return Json(new { result = returnResult });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "CheckUserID");
                return Json(new { result = "yes" });
            }
        }

        [HttpPost]
        public JsonResult CheckPINID(string allData)
        {
            try
            {
                string[] custData = allData.Split('*');
                int custId = 0;
                string PINId = "";
                int distID = 0;
                if (custData.Length == 3)
                {
                    PINId = custData[0];
                    custId = Convert.ToInt32(custData[1]);
                    distID = Convert.ToInt32(custData[2]);

                }
                long ClientId = ClientInfoData.GetClientID();

                bool alreadyExists = customerRepository.PINAlreadyExists(ClientId, PINId, custId, distID);

                string returnResult = "yes";
                if (alreadyExists)
                {
                    returnResult = "yes";
                }
                else
                {
                    returnResult = "no";
                }
                return Json(new { result = returnResult });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "CheckPINID");
                return Json(new { result = "yes" });
            }

        }

        [HttpPost]
        public JsonResult getHomeRooms(string allData)
        {
            try
            {
                string[] schoolData = allData.Split('*');
                string SchoolId = "0";

                if (schoolData.Length == 1)
                {
                    SchoolId = schoolData[0];
                }
                long ClientId = ClientInfoData.GetClientID();

                var homeRoomList = homeroomHelper.GetSelectList(0, Convert.ToInt32(SchoolId)).ToList();
                long distid = customerRepository.GetSchoolsDistrict(ClientId, Convert.ToInt16(SchoolId));
                var SchoolsList = customerRepository.GetEatingSchools(ClientId, distid, false, Convert.ToInt32(SchoolId)).ToList();

                if (homeRoomList == null)
                {
                    return Json(new { result = "-1" });
                }
                else
                {
                    return Json(new { result = homeRoomList, Schoollist = SchoolsList, DistrictID = distid.ToString() });
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getHomeRooms");
                return Json(new { result = "-1" });
            }
        }

        [HttpPost]
        public JsonResult getHomeRoomsList(string SchoolId)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                IEnumerable<SelectListItem> homeRoomList;
                if (string.IsNullOrEmpty(SchoolId))
                    homeRoomList = homeroomHelper.GetSelectList(0).ToList();
                else
                    homeRoomList = homeroomHelper.GetSelectList(0, Convert.ToInt32(SchoolId)).ToList();

                if (homeRoomList == null)
                {
                    return Json(new { result = "-1" });
                }
                else
                {
                    return Json(new { result = homeRoomList });
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getHomeRoomsList");
                return Json(new { result = "-1" });
            }
        }

        [HttpPost]
        public JsonResult getSchools(string allData)
        {
            try
            {
                string[] distData = allData.Split('*');
                string DistId = "0";

                if (distData.Length == 1)
                {
                    DistId = distData[0];
                }
                long ClientId = ClientInfoData.GetClientID();

                var schoolsList = generalRepository.getSchools(ClientId, Convert.ToInt32(DistId), false).ToList();

                if (schoolsList == null)
                {
                    return Json(new { result = "-1" });
                }
                else
                {
                    return Json(new { result = schoolsList, count = schoolsList.Count });
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSchools");
                return Json(new { result = "-1" });
            }
        }

        public ActionResult InitActivate()
        {
            var model = new ActivateModel { Title = "Customer", ActivateUrl = "/Customer/Activate" };

            return PartialView("_Activate", model);
        }

        public ActionResult Activate(int id = 0)
        {
            try
            {
                var clientId = ClientInfoData.GetClientID();
                var model = new ActivateModel();
                var entity = unitOfWork.CustomerRepository.Get(x => x.ClientID == clientId && x.ID == id).FirstOrDefault();

                if (entity == null)
                {
                    model.Message = "Record not found or deleted by another user.";
                    model.IsError = true;
                }
                else
                {
                    model.Id = entity.ID;
                    model.Name = entity.LastName + ", " + entity.FirstName;
                    model.IsActive = (bool)entity.isActive;
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(model, JsonRequestBehavior.AllowGet);
                }

                if (model.IsError)
                {
                    return PartialView("_Activate", model);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Activate");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ActionName("Activate")]
        public ActionResult ActivateConfirm(int id = 0, bool isActive = false)
        {
            var clientId = ClientInfoData.GetClientID();
            var model = new ActivateModel { IsError = true };
            var entity = unitOfWork.CustomerRepository.Get(x => x.ClientID == clientId && x.ID == id).FirstOrDefault();

            if (entity == null)
            {
                model.Message = "Record not found or deleted by another user.";
            }
            else
            {
                if (Convert.ToBoolean(entity.isActive))
                    entity.DeactiveDateLocal = DateTime.Now;
                else
                    entity.ReactiveDateLocal = DateTime.Now;

                if (Convert.ToBoolean(entity.isActive))
                    entity.DeactiveDate = DateTime.UtcNow;
                else
                    entity.ReactiveDate = DateTime.UtcNow;

                entity.isActive = !isActive;
                unitOfWork.CustomerRepository.Update(entity);

                try
                {
                    unitOfWork.Save();
                    model.Message = string.Format("The customer record has been {0} successfully.", !isActive ? "activated" : "deactivated");
                    model.IsError = false;

                    // delete or un-delete the user on the basis of customer
                    ActivateDeacvtivateUsers(entity.ID, entity.isActive);
                }
                catch (Exception ex)
                {
                    model.Message = ex.Message;
                    //Error logging in cloud tables 
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ActivateConfirm");
                }
            }

            if (Request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            if (model.IsError)
            {
                return PartialView("_Activate", model);
            }

            return RedirectToAction("Index");
        }

        private void ActivateDeacvtivateUsers(long customerId, bool isActive)
        {
            var clientId = ClientInfoData.GetClientID();
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();

            int unDeleteUserResult = 0;
            bool unDeleteUserSpecified = false;

            if (isActive == false)
                client.DeactivateUser(clientId, true, customerId, true, out unDeleteUserResult, out unDeleteUserSpecified);
            else
                client.ActivateUser(clientId, true, customerId, true, out unDeleteUserResult, out unDeleteUserSpecified);
        }

        public string getServiceUrl()
        {
            string url = "";
            url = ConfigurationManager.AppSettings["ServiceUrl"];
            return url;
        }

        /// <summary>
        /// Get Customer Data
        /// </summary>
        /// <param name="ClientID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public Customer_Detail_VM GetCustomerData(Int64 ClientID, int customerID)
        {
            var customerData = customerRepository.GetCustomer(ClientID, customerID);
            try
            {
                if (customerData.CreationDateLocal != null)
                {
                    customerData.ConvertedCreationDate = Convert.ToDateTime(customerData.CreationDateLocal).ToString("MMMM dd, yyyy hh:mm tt");
                }

                customerData.Customer_Phone = string.IsNullOrEmpty(customerData.Customer_Phone) ? "" : customerData.Customer_Phone;
                customerData.SSN = string.IsNullOrEmpty(customerData.SSN) ? "" : customerData.SSN;
                customerData.Email = string.IsNullOrEmpty(customerData.Email) ? "" : customerData.Email;


                return customerData;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCustomerData");
                return customerData;
            }
        }


    }

}

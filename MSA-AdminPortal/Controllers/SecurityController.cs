using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using MSA_AdminPortal.Helpers;
using Repository;
using Repository.edmx;
using Repository.Helpers;
using System.Data;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Text;

namespace MSA_AdminPortal.Controllers
{
    public class SecurityController : BaseAuthorizedController
    {
        private UnitOfWork unitOfWork;
        private string[] emptyStringArray = new string[0];
        // private ISecurityRepository securityRepository;
        // private IGeneralRepository generalRepository;
        UserRolesHelper helper = null;
        private HomeRoomHelper homeroomHelper = new HomeRoomHelper();

        public SecurityController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
            helper = new UserRolesHelper();
        }

        public ActionResult NoAccess(string id)
        {
            ErrorModel model = new ErrorModel();
            getErrorModel(id, model);
            return View(model);
        }

        private void getErrorModel(string id, ErrorModel Model)
        {
            string erroid = (id == null) ? "" : Convert.ToString(id);

            switch (erroid)
            {
                case "nodistrict":
                    Model.IsError = true;
                    Model.Message = "Access to districts page denied.";
                    break;
                case "noschool":
                    Model.IsError = true;
                    Model.Message = "Access to school page denied.";
                    break;
                case "nopos":
                    Model.IsError = true;
                    Model.Message = "Access to POS page denied.";
                    break;
                case "nousers":
                    Model.IsError = true;
                    Model.Message = "Access to users page denied.";
                    break;

                case "nouserroles":
                    Model.IsError = true;
                    Model.Message = "Access to user roles page denied.";
                    break;

                case "nocustomers":
                    Model.IsError = true;
                    Model.Message = "Access to customer page denied.";
                    break;

                case "nohomerooms":
                    Model.IsError = true;
                    Model.Message = "Access to homeroom page denied.";
                    break;

                case "nogrades":
                    Model.IsError = true;
                    Model.Message = "Access to grades page denied.";
                    break;

                case "noactivity":
                    Model.IsError = true;
                    Model.Message = "Access to Activity page denied.";
                    break;
                case "nocustomerReport":
                    Model.IsError = true;
                    Model.Message = "Access to customer report is denied.";
                    break;
                case "nofinanceReport":
                    Model.IsError = true;
                    Model.Message = "Access to Finance report is denied.";
                    break;
                case "noparents":
                    Model.IsError = true;
                    Model.Message = "Access to parents page is denied.";
                    break;
                case "nomsaalerts":
                    Model.IsError = true;
                    Model.Message = "Access to MSA Alert page is denied.";
                    break;
                case "nomsasettings":
                    Model.IsError = true;
                    Model.Message = "Access to MSA Settings is denied.";
                    break;
                case "nodashboard":
                    Model.IsError = true;
                    Model.Message = "Access to Dashboard is denied.";
                    break;
                case "noapplications":
                    Model.IsError = true;
                    Model.Message = "Access to Applications Page is denied.";
                    break;
                default:
                    Model.IsError = true;
                    Model.Message = "Access denied";
                    break;

            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Users(int? id)
        {
            if (!SecurityManager.viewUsers) return RedirectToAction("NoAccess", "Security", new { id = "nousers" });
            try
            {
                long ClientId = ClientInfoData.GetClientID();
               
                var userList = helper.getUsersList(ClientId, id); //unitOfWork.securityRepository.getUsers(ClientId, id);
                //ViewBag.SecurityList = unitOfWork.securityRepository.getSecurityGroup(ClientId);

                ViewBag.ActiveList = GetActiveDDLItems();
                ViewBag.PrimaryList = GetPrimaryDDLItems();
                ViewBag.userRolesList = helper.getUserRolesNamesList(ClientId);
                ViewBag.selectedRoleUsers = id.HasValue ? id.Value : 0;
                return View(userList);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Users");
                return View();
            }
        }

        private List<string> GetActiveDDLItems()
        {
            List<string> list = new List<string>();
            list.Add("Active");
            list.Add("Inactive");
            return list;
        }

        private List<string> GetPrimaryDDLItems()
        {
            List<string> list = new List<string>();
            list.Add("Primary");
            list.Add("Secondary");
            return list;
        }

        public ActionResult UserRolesList()
        {
            if (!SecurityManager.viewUserRoles) return RedirectToAction("NoAccess", "Security", new { id = "nouserroles" });

            if (Request.Cookies["UserRolesView"] != null && Encryption.Decrypt(Request.Cookies["UserRolesView"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table");
            }

            return RedirectToAction("Table");

        }

        public ActionResult UserRoles(int? id)
        {
            long ClientId = ClientInfoData.GetClientID();


            UserRoleModel model = new UserRoleModel();

            userRoleItem userRoleItem = helper.getSingleUserRole(ClientId, id);


            model.ClientID = userRoleItem.ClientId;
            model.ID = userRoleItem.Id;

            model.RoleName = userRoleItem.RoleName;
            model.SelectedHQSystem = Enum.GetName(typeof(FSSSecurity.SecurityLists.FSS_Systems), userRoleItem.AdminHQSystem == 0 ? (int)FSSSecurity.SecurityLists.FSS_Systems.POSADMIN : userRoleItem.AdminHQSystem);
            model.ModulesWithPermissions = helper.getModulesAndPermissions(ClientId, id);
            model.SystemsList = unitOfWork.securityRepository.getSystemList();
            model.ModulesList = unitOfWork.securityRepository.getModulesList(FSSSecurity.SecurityLists.FSS_Systems.POSADMIN);
            if (userRoleItem.AdminHQSystem == (int)FSSSecurity.SecurityLists.FSS_Systems.POS)
            {
                model.ModulesList = model.ModulesList.Where(x => x.POS_ObjectID != -1).ToList();

            }
            return View(model);
        }

        public ActionResult Table()
        {
            Response.Cookies["UserRolesView"].Value = Encryption.Encrypt("Table");

            return View();
        }

        public ActionResult Tile()
        {
            Response.Cookies["UserRolesView"].Value = Encryption.Encrypt("Tile");
            return View();
        }

        public ActionResult InitActivate()
        {
            var model = new ActivateModel { Title = "User", ActivateUrl = "/Security/Activate" };

            return PartialView("_Activate", model);
        }


        public ActionResult AjaxHandlerUser(JQueryDataTableParamModel param, UserFilters filters)
        {
            try
            {


                long ClientId = ClientInfoData.GetClientID();
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc
                //var customerData = customerRepository.GetCustomerPage(ClientId, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, filters, out totalRecords);
                var userData = helper.getUsersListFiltered(ClientId, filters.UserRoleId, filters.SearchBy, filters.ActiveStr, filters.PrimaryStr, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection);



                if (userData != null)
                {
                    var result = from u in userData
                                 select new[] { u.CustomerId.ToString(), u.UserName, u.LoginName, u.UserRoleName, u.isActive.ToString().ToLower(), u.isPrimary.ToString().ToLower(), u.CustomerId.ToString(), u.IsDeleted.ToString().ToLower() };
                    //select new[] { (!(c.Active.HasValue && c.Active.Value) ? "<i  title=\"Activate\" class=\"fa fa-user-plus fasize faActiveColr\"></i>" : "<i  title=\"Deactivate\" class=\"fa fa-user-times fasize fadeactiveColr \"></i>"), c.id.ToString(), c.UserID, c.Last_Name, c.First_Name, c.Middle_Initial, c.Adult.ToString(), c.Grade, c.Homeroom, c.School_Name, c.PIN, string.Format("{0:C}", c.Total_Balance), c.id.ToString() };

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = userData[0].AllRecordsCount,
                        iTotalDisplayRecords = userData[0].AllRecordsCount,
                        aaData = result,
                        recordsFiltered = userData[0].AllRecordsCount,
                        recordsTotal = userData[0].AllRecordsCount
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
                        aaData = emptyStringArray,
                        recordsFiltered = 0,
                        recordsTotal = 0
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

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            //UserRolesHelper urh = new UserRolesHelper();
            long ClientID = ClientInfoData.GetClientID();
            int totalRecords = 0;
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"]; // asc or desc
            string iDisplayLength = Request["iDisplayLength"];
            string iDisplayStart = Request["iDisplayStart"];
            int pageSize = helper.getPageSize(iDisplayLength);
            int pageNumber = helper.getPageNumber(iDisplayStart, iDisplayLength);

            totalRecords = helper.getTotalOfUserRoles(ClientID);
            var result = helper.getUserRolesList(ClientID, sortColumnIndex, sortDirection.ToUpper(), pageSize, pageNumber);// posList.Select(x => new string[] { x.id.ToString(), x.id.ToString(), FSSSecurity.SecurityLists.getSystemName(), x.UserRoleName, x.UserRolePermissions_IDS, x.usersCount.ToString() });

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = result
            },
            JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditUser(int id = 0)
        {
            long ClientId = ClientInfoData.GetClientID();

            var userDetails = helper.getUser(ClientId, id); //unitOfWork.securityRepository.getUserDetail(ClientId, id);
            userDetails.savebtnCaption = "Save";

            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }


        public JsonResult CreateUser()
        {
            long ClientId = ClientInfoData.GetClientID();

            var userDetails = new UsersDetailsVM();
            userDetails.savebtnCaption = "Save";

            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveUser(UsersDetailsVM model, bool newUser)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                string result;
                if (ModelState.IsValid)
                {
                    //string fullName = unitOfWork.generalRepository.GetCustomerName(ClientId, model.EmployeeId);
                    var name = model.UserName.Split(' ');
                    string fname = name[0];
                    string lname = name[1];

                    var client = new RegistrationService.Registration();
                    //string url = helper.getServiceUrl(); // "http://localhost:53679/Registration.svc?wsdl";
                    string url = ConfigurationManager.AppSettings["ServiceUrl"];

                    client.Url = url;
                    int customerID = 0;
                    bool custmerResult = false;
                    //int userRoleID = (model.UserRolesID.HasValue) :0 ? Convert.ToInt32(model.UserRolesID);
                    //client.SaveUser(ClientId, true, model.LoginName, model.Password, fname, lname, model.EmployeeId, true, model.UserRolesID, true, out customerID, out custmerResult);
                    helper.SaveUser(ClientId, model.LoginName, model.Password, fname, lname, model.EmployeeId, model.UserRolesID, out customerID, out custmerResult);
                    result = unitOfWork.securityRepository.saveUser(model, ClientId);
                    //result = (customerID == 0) ? "-1" : customerID.ToString();
                    model.returnCode = string.IsNullOrEmpty(result) ? 0 : Convert.ToInt16(result);
                }
                else
                {
                    model.EmployeeId = 0;
                }

                string redirectUrl = "";

                if (model.RedirectUrl.Split("?".ToArray()).Length == 1)
                    redirectUrl = new UrlHelper(Request.RequestContext).Action("Users", "Security");
                else
                    redirectUrl = new UrlHelper(Request.RequestContext).Action("UserRolesList", "Security", new { id = model.UserRolesID });

                model.RedirectUrl = redirectUrl;

                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SaveUser");
                return null;
            }
        }

        [HttpPost]
        public ActionResult ValidateUserData(UsersDetailsVM model, bool newUser)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                bool userAlreadyExists = false;
                bool loginAlreadyExists = false;
                bool error = false;
                if (ModelState.IsValid)
                {
                    //var existingUser = unitOfWork.securityRepository.GetUserByCustomerId(ClientId, model.EmployeeId);
                    var existingUser = helper.getUser(ClientId, model.EmployeeId);
                    if (existingUser != null && (existingUser.EmployeeId != model.EmployeeId || newUser))
                    {
                        userAlreadyExists = true;
                    }
                    //var existingLogin = unitOfWork.securityRepository.GetUserByLoginName(ClientId, model.LoginName);
                    var existingLogin = helper.getUserByUsername(ClientId, model.LoginName);
                    if (existingLogin != null && (existingLogin.EmployeeId != model.EmployeeId || newUser))
                    {
                        loginAlreadyExists = true;
                    }
                }
                else
                {
                    error = true;
                }

                return Json(new { userAlreadyExists, loginAlreadyExists, error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ValidateUserData");
                return null;
            }
        }

        public ActionResult Popup()
        {
            var model = new UsersDetailsVM { };

            long ClientId = ClientInfoData.GetClientID();
            //ViewBag.SecurityList = unitOfWork.securityRepository.getSecurityGroup(ClientId);


            ViewBag.userRolesList = helper.getUserRolesNamesList(ClientId); // unitOfWork.securityRepository.getUsersRolesNames(ClientId);
            ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

            ViewBag.GradeList = unitOfWork.generalRepository.getGrades(ClientId).ToList();
            ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
            ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == ClientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);

            return PartialView("Popup", model);
        }
        // User Delete Generic Handling

        public ActionResult UserDelete(int id = 0)
        {
            var model = helper.GetUserDeleteModel(id, true);

            return GetUserActionResult(Request, model);
        }

       
           private ActionResult GetUserActionResult(HttpRequestBase request, UserDeleteModel model, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            if (model != null && isGetAction != null)
            {
                if (model.IsError || isGetAction)
                {
                    return PartialView("_Delete", model);
                }
            }
            //due to bug#2581
            else
            {
                return PartialView("_Delete", model);
            }

            return RedirectToAction("Index");
        }

             

        [HttpDelete]
        [ActionName("UserDelete")]
        public ActionResult UserDeleteConfirm(int id = 0)
        {
            var model = helper.GetUserDeleteModel(id, false);

                int userId = Convert.ToInt32(id);
                long ClientId = ClientInfoData.GetClientID();
                string resultdata = "";
            try
            {
                if (!unitOfWork.securityRepository.AssociatedOrdersExists(ClientId, userId))
                {

                    resultdata = unitOfWork.securityRepository.DeleteUser(ClientId, userId);

                    var client = new RegistrationService.Registration();

                    //string url = url = ConfigurationManager.AppSettings["devServiceUrl"];
                    string url = helper.getServiceUrl(); // "http://localhost:53679/Registration.svc?wsdl";
                    client.Url = url;
                    int deletedUserResule = 0;
                    bool deletedUserBool = false;
                    client.DeleteUser(ClientId, true, userId, true, out deletedUserResule, out deletedUserBool);

                    model.IsError = false;
                    model.Message = "User has been deleted successfully!";
                    if (resultdata != "-1")
                    {
                        model.IsError = true;
                        model.Message = resultdata;
                    }
                }
                else
                {
                   // resultdata = "-5";
                    model.IsError = true;
                    model.Message = "This user can’t be deleted because it has some associated orders.";

                }
                              
            
            }
        
            catch (Exception ex)
            {
                helper.SetUserErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteUser");
                return null;
            }

            return GetUserActionResult(Request, model, false);
        }


   //End User Delete Generic

   /* Old User Delete Code has been Commented

        [HttpPost]
        public JsonResult DeleteUser(string allData)
        {

            try
            {
                int userId = Convert.ToInt32(allData);
                long ClientId = ClientInfoData.GetClientID();
                string resultdata = "";

                if (!unitOfWork.securityRepository.AssociatedOrdersExists(ClientId, userId))
                {

                    resultdata = unitOfWork.securityRepository.DeleteUser(ClientId, userId);

                    var client = new RegistrationService.Registration();

                    //string url = url = ConfigurationManager.AppSettings["devServiceUrl"];
                    string url = helper.getServiceUrl(); // "http://localhost:53679/Registration.svc?wsdl";
                    client.Url = url;
                    int deletedUserResule = 0;
                    bool deletedUserBool = false;
                    client.DeleteUser(ClientId, true, userId, true, out deletedUserResule, out deletedUserBool);
                }
                else
                {
                    resultdata = "-5";
                }

                return Json(new { result = resultdata });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteUser");
                return null;
            }
        }

  */
        public ActionResult Activate(int id = 0)
        {
            try
            {
                var clientId = ClientInfoData.GetClientID();
                var model = new ActivateModel();

                var client = new RegistrationService.Registration();
                string url = helper.getServiceUrl(); // "http://localhost:53679/Registration.svc?wsdl";
                client.Url = url;

                var customer = unitOfWork.CustomerRepository.Get(x => x.ClientID == clientId && x.ID == id).FirstOrDefault();
                var entity = client.GetUser(clientId, true, id, true);

                if ( (customer.isActive && entity.IsActive) ||
                    (customer.isActive && entity.IsActive==false) ||
                    (customer.isActive==false && entity.IsActive))
                {
                    
                    if (entity == null)
                    {
                        model.Message = "Record not found or deleted by another user.";
                        model.IsError = true;
                    }
                    else
                    {
                        model.Id = entity.CustomerId;
                        model.Name = entity.LoginName;
                        model.IsActive = entity.IsActive;
                    }

                    if (Request.IsAjaxRequest())
                    {
                        return Json(model, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    model.Message = "User can not be activated because customer is in deactivated state.";
                    model.IsError = true;
                }


                //if (model.IsError)
                //{
                //    return PartialView("_Activate", model);
                //}


                if (Request.IsAjaxRequest())
                {
                    return Json(model, JsonRequestBehavior.AllowGet);
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
        public JsonResult ActivateConfirm(int id = 0, bool isActive = false)
        {

            try
            {
                string resultdata = "";

                var ClientId = ClientInfoData.GetClientID();
                var client = new RegistrationService.Registration();

                //string url = url = ConfigurationManager.AppSettings["devServiceUrl"];
                string url = helper.getServiceUrl(); // "http://localhost:53679/Registration.svc?wsdl";
                client.Url = url;
                int deletedUserResule = 0;
                bool deletedUserBool = false;

                var user = client.GetUser(ClientId, true, id, true);

                if (user != null)
                {
                    if (user.IsActive == false)
                        client.ActivateUser(ClientId, true, id, true, out deletedUserResule, out deletedUserBool);
                    else
                        client.DeactivateUser(ClientId, true, id, true, out deletedUserResule, out deletedUserBool);
                }
                resultdata = deletedUserResule.ToString();
                return Json(new { result = resultdata , Message = string.Format("Record {0} successfully.",user.IsActive?"deactiavted":"actiavted") });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteUser");
                return null;
            }
        }

        [HttpPost]
        public JsonResult getModulesList(string systemName)
        {
          
           string POSADMIN = Enum.GetName(typeof(FSSSecurity.SecurityLists.FSS_Systems),(int)FSSSecurity.SecurityLists.FSS_Systems.POSADMIN);
           string POS = Enum.GetName(typeof(FSSSecurity.SecurityLists.FSS_Systems), (int)FSSSecurity.SecurityLists.FSS_Systems.POS);      

            var modulesList = unitOfWork.securityRepository.getModulesList(FSSSecurity.SecurityLists.FSS_Systems.POSADMIN);
            if (systemName == POS)
            {
                modulesList = modulesList.Where(x => x.POS_ObjectID != -1).ToList();

            }

            return Json(new { result = modulesList });
        }


        [HttpPost]
        public JsonResult getPermissionsList(string allData)
        {

            int moduleID = Convert.ToInt32(allData);


            var resultdata = unitOfWork.securityRepository.getPermissionsList((FSSSecurity.SecurityLists.FSS_Modules)moduleID);

            return Json(new { result = resultdata });
        }

        [HttpPost]
        public JsonResult postPermissionsList(string allData)
        {

            string permissionList = allData;

            string[] permissionListSplitted = permissionList.Split('*');
            long clientID = Convert.ToInt64(permissionListSplitted[0]);
            int ID = (Convert.ToInt32(permissionListSplitted[1]) == 0) ? -1 : Convert.ToInt32(permissionListSplitted[1]);
            string HQSystemSelected = permissionListSplitted[2];
            string UserRoleName = permissionListSplitted[3];
            string ModuleAndAactions = permissionListSplitted[4];

            string resultdata = "-1";
            var userRole = helper.GetUserRoleByName(UserRoleName);
            if (userRole != null && userRole.Id != ID)
            {
                resultdata = "-2";
                return Json(new { result = resultdata });
            }

            var adminSystem = (FSSSecurity.SecurityLists.FSS_Systems)Enum.Parse(typeof(FSSSecurity.SecurityLists.FSS_Systems), HQSystemSelected);
            if (adminSystem == FSSSecurity.SecurityLists.FSS_Systems.POSADMIN)
            {
                // var resultdata = unitOfWork.securityRepository.SaveUserRole(clientID, ID, UserRoleName, HQSystemSelected, PermissionsIds);
                resultdata = helper.CreateUpdateUserRole(clientID, ID, UserRoleName, ModuleAndAactions, adminSystem);
            }
            else
            {
                DataTable accessRights = TranslateToAccessRightsTable(ModuleAndAactions);
                if (accessRights != null && accessRights.Rows.Count > 0)
                {
                    resultdata = unitOfWork.securityRepository.SaveRolePermissions(clientID, ID, UserRoleName, accessRights);
                    if (resultdata != "-1")
                        resultdata = helper.CreateUpdateUserRole(clientID, ID, UserRoleName, ModuleAndAactions, adminSystem);
                }
            }

            //string resultdata = string.Empty;
            return Json(new { result = resultdata });
        }

        private DataTable TranslateToAccessRightsTable(string moduleAndAactions)
        {
            if (string.IsNullOrEmpty(moduleAndAactions)) return null;

            var ser = new JavaScriptSerializer();
            Dictionary<string, string> moduleActionDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(moduleAndAactions);

            DataTable accessRights = new DataTable("AccessRights");
            accessRights.Clear();
            accessRights.Columns.Add("ObjectID", typeof(int));
            accessRights.Columns.Add("CanView", typeof(bool));
            accessRights.Columns.Add("CanInsert", typeof(bool));
            accessRights.Columns.Add("CanEdit", typeof(bool));
            accessRights.Columns.Add("CanDelete", typeof(bool));

            IList<FSSSecurity.SecurityLists.ModuleDetail> modulesList = unitOfWork.securityRepository.getModulesList(FSSSecurity.SecurityLists.FSS_Systems.POSADMIN);
            foreach (var item in moduleActionDict)
            {
                DataRow newRow = accessRights.NewRow();
                FSSSecurity.SecurityLists.ModuleDetail module = modulesList.FirstOrDefault(x => Convert.ToString((int)x.ModuleID) == item.Key);
                if (module.POS_ObjectID == -1) continue;

                newRow["ObjectID"] = module.POS_ObjectID;
                newRow["CanView"] = item.Value.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.View));
                newRow["CanInsert"] = item.Value.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Create))
                    || item.Value.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Allow));

                newRow["CanEdit"] = item.Value.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Update))
                    || item.Value.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Void));
                newRow["CanDelete"] = item.Value.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Delete));

                accessRights.Rows.Add(newRow);
            }
            // Inayat [24-Jan-2016] The two rights under POS module 'Allow Cashier to Search' and 'Allow Cashier Override' are
            // mapped on Orders (ObjectID = 2) in AccessRights table. They do not map on POS (ObjectID = 25) in POS userRole.
            // And in Activity module we do not map 'View Customer Activity' permission in AccessRights table. This is only stored
            // in POS Admin UserRole(registration database).
            DataRow[] posRow = accessRights.Select("ObjectID = 25");
            DataRow[] activityRow = accessRights.Select("ObjectID = 2");
            if (posRow.Length > 0)
            {
                string posKey = Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Modules.POS);

                string posRights = moduleActionDict.FirstOrDefault(x => x.Key == posKey).Value;

                posRow[0]["CanView"] = posRights.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.View));
                posRow[0]["CanInsert"] = false;
                posRow[0]["CanEdit"] = posRights.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Update));
                posRow[0]["CanDelete"] = posRights.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Delete));

                if (activityRow.Length == 0)
                {
                    DataRow newRow = accessRights.NewRow();
                    newRow["ObjectID"] = 2;
                    newRow["CanView"] = posRights.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Allow));
                    newRow["CanInsert"] = posRights.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Create));
                    newRow["CanEdit"] = false;
                    newRow["CanDelete"] = false;
                    accessRights.Rows.Add(newRow);
                }

                if (activityRow.Length > 0) activityRow[0]["CanView"] = posRights.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Allow));
                if (activityRow.Length > 0) activityRow[0]["CanInsert"] = posRights.Contains(Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Create));
            }
            else
            {
                // No rights are checked in POS module so we simply set these both to false. However, the 'Void Customer Activity'
                // is mapped on canEdit in AccessRights table, and it is handeled in above loop.
                if (activityRow.Length > 0) activityRow[0]["CanView"] = false;
                if (activityRow.Length > 0) activityRow[0]["CanInsert"] = false;
            }

            accessRights.AcceptChanges();
            return accessRights;
        }


        [HttpPost]
        public JsonResult getModuleAndRights(string allData)
        {
            long ClientId = ClientInfoData.GetClientID();
            int userRoleID = Convert.ToInt32(allData);

            // var resultdata = unitOfWork.securityRepository.SaveUserRole(clientID, ID, UserRoleName, HQSystemSelected, PermissionsIds);
            var resultdata = helper.getModulesAndPermissionStrings(ClientId, userRoleID);

            return Json(new { result = resultdata });
        }



        public ActionResult Delete(int id = 0)
        {
            var model = helper.getDeleteModel(id);
            return GetActionResult(Request, model);
            //return PartialView("_Delete", model);
        }

        private ActionResult GetActionResult(HttpRequestBase request, UserRoleDeleteModel model, bool isGetAction = true)
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
                    helper.DeleteUserRole(id, model.Name);

                    model.Message = "User Role has been deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
            }

            return GetActionResult(Request, model, false);
        }

        /*[HttpGet]
        public JsonResult GetSystemPermissions(int roleID, string roleName, FSSSecurity.SecurityLists.FSS_Systems adminSystem)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                string resultdata = string.Empty;
                if (adminSystem == FSSSecurity.SecurityLists.FSS_Systems.POSADMIN)
                {
                    resultdata = helper.getModulesAndPermissions(ClientId, roleID);
                }
                else
                {
                    List<Admin_AccessRights_List_Result> accessRights = unitOfWork.securityRepository.GetAccessRightsList(ClientId, roleName);
                    IList<FSSSecurity.SecurityLists.ModuleDetail> modulesList = unitOfWork.securityRepository.getModulesList(FSSSecurity.SecurityLists.FSS_Systems.POSADMIN);
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    List<int> actionsIds = new List<int>();
                    foreach(var right in accessRights)
                    {
                        FSSSecurity.SecurityLists.ModuleDetail module = modulesList.FirstOrDefault(x => x.POS_ObjectID == right.ObjectID.Value);
                        if (module == null) continue;
                        IList<FSSSecurity.SecurityLists.PermissionDetail> permissionsList = unitOfWork.securityRepository.getPermissionsList((FSSSecurity.SecurityLists.FSS_Modules)module.ModuleID);
                        actionsIds.Clear();
                        foreach(var permision in permissionsList)
                        {
                            if (permision.ModuleID != module.ModuleID) continue;
                            switch(permision.ActionID)
                            {
                                case FSSSecurity.SecurityLists.FSS_Actions.View:
                                    {
                                        if (right.canView.Value) actionsIds.Add((int)FSSSecurity.SecurityLists.FSS_Actions.View);
                                    }
                                    break;
                                case FSSSecurity.SecurityLists.FSS_Actions.Create:
                                    {
                                        if (right.canInsert.Value) actionsIds.Add((int)FSSSecurity.SecurityLists.FSS_Actions.Create);
                                    }
                                    break;
                                case FSSSecurity.SecurityLists.FSS_Actions.Update:
                                    {
                                        if (right.canEdit.Value) actionsIds.Add((int)FSSSecurity.SecurityLists.FSS_Actions.Update);
                                    }
                                    break;
                                case FSSSecurity.SecurityLists.FSS_Actions.Delete:
                                    {
                                        if (right.canDelete.Value) actionsIds.Add((int)FSSSecurity.SecurityLists.FSS_Actions.Delete);
                                    }
                                    break;
                                case FSSSecurity.SecurityLists.FSS_Actions.Allow:
                                    {
                                        if (right.canView.Value) actionsIds.Add((int)FSSSecurity.SecurityLists.FSS_Actions.Allow);
                                    }
                                    break;
                                case FSSSecurity.SecurityLists.FSS_Actions.Void:
                                    {
                                        if (right.canEdit.Value) actionsIds.Add((int)FSSSecurity.SecurityLists.FSS_Actions.Void);
                                    }
                                    break;
                            }
                        }

                        dic.Add(Convert.ToString((int)module.ModuleID), string.Join(",", actionsIds));
                    }

                    this.GetActivityPermissions(dic, accessRights);
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    resultdata = ser.Serialize(dic);
                }

                return Json(new { result = resultdata }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSystemPermissions");
                return Json(new { result = -1 }, JsonRequestBehavior.AllowGet);
            }
        }

        private void GetActivityPermissions(Dictionary<string, string> dic, List<Admin_AccessRights_List_Result> accessRights)
        {
            var module = Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Modules.Activity);
            string actionList = dic[module];
            var rights = accessRights.FirstOrDefault(x => x.ObjectID == 2);
            if (rights != null)
            {
                if (rights.canInsert.Value)
                {
                    string id = Convert.ToString((int)FSSSecurity.SecurityLists.FSS_Actions.Create);
                    actionList = actionList.Length > 0 ? actionList + ", " + id : id;
                    dic[module] = actionList;
                }
            }
        }*/

    }
    public class UserRolesHelper
    {


        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork;

        public UserRolesHelper()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }

        public userRoleItem getUserRole(int? id = 0)
        {
            int tempId = 0;
            if (id.HasValue)
            {
                tempId = (int)id;
            }
            if (id == 0)
            {
                return new userRoleItem { ClientId = clientId, Id = -1, RoleName = "" };
            }
            else
            {
                return getSingleUserRole(clientId, tempId);
            }
        }

        public string getServiceUrl()
        {
            string url = "";

            url = ConfigurationManager.AppSettings["ServiceUrl"];
            return url;
        }

        public System.Collections.Generic.IEnumerable<string[]> getUserRolesList(long ClientID, int SortIndex, string sortDirection, int pageSize, int PageNumber)
        {
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            var urList = client.GetUserRolesList(ClientID, true, SortIndex, true, sortDirection, pageSize, true, PageNumber, true);
            if (urList != null && urList.Count() > 0)
            {
                return urList.Select(x => new string[] { x.Id.ToString(), x.Id.ToString(), FSSSecurity.SecurityLists.SystemsList[(FSSSecurity.SecurityLists.FSS_Systems)x.AdminHQSystem].First().DisplaySystemText, x.UserRoleName, x.Id.ToString(), x.AssignedUsers.ToString() });
            }
            else
            {
                IEnumerable<string[]> empty = Enumerable.Empty<string[]>(); ;
                return empty;
            }

        }

        public List<userRoleItem> getUserRolesNamesList(long ClientID)
        {
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            var urListChk = client.GetAllUserRoles(ClientID, true).OrderBy(x => x.UserRoleName);
            var urList = urListChk == null? null : urListChk.ToList();
            List<userRoleItem> returnList = new List<userRoleItem>();

            if (urList != null && urList.Count() > 0)
            {
                for (int i = 0; i < urList.Count(); i++)
                {
                    returnList.Add(new userRoleItem { Id = urList[i].Id, RoleName = urList[i].UserRoleName });
                }
            }

            return returnList;
        }

        public int getTotalOfUserRoles(long ClientID)
        {

            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            string count = client.getCountUsersRoles(ClientID, true);
            return Convert.ToInt32(count);
        }

        public int getPageNumber(string iDisplayStart, string iDisplayLength)
        {
            int displayStart = 0;
            int.TryParse(iDisplayStart, out displayStart);
            int length = getPageSize(iDisplayLength);
            int pageNumber = (displayStart + length) / length;
            return pageNumber;

        }

        public int getPageSize(string iDisplayLength)
        {
            int pageSize = 10;
            int.TryParse(iDisplayLength, out pageSize);
            return pageSize;
        }

        public string getModulesAndPermissions(long ClientID, int? roleID)
        {
            int RoleID = 0;
            if (roleID.HasValue)
            {
                RoleID = Convert.ToInt32(roleID);
            }
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();

            var DictData = client.GetSecurityInfo(ClientID, true, RoleID, true);
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            if (DictData.ModulePermissions != null)
            {
                foreach (var item in DictData.ModulePermissions)
                {
                    newDict.Add(item.Key.ToString(), item.Value);
                }
            }

            JavaScriptSerializer ser = new JavaScriptSerializer();
            return ser.Serialize(newDict);
        }

        public string getModulesAndPermissionStrings(long ClientID, int? roleID)
        {
            int RoleID = 0;
            if (roleID.HasValue)
            {
                RoleID = Convert.ToInt32(roleID);
            }
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();

            var DictData = client.GetSecurityInfo(ClientID, true, RoleID, true);

            Dictionary<string, string> newDict = new Dictionary<string, string>();

            if (DictData.ModulePermissions != null)
            {
                foreach (var item in DictData.ModulePermissions)
                {
                    string actions = item.Value;
                    string[] splittedActions = actions.Split(',');
                    string actionString = "";
                    string commaSeparated = "";
                    string moduleStr = "";
                    int actionID = 0;

                    moduleStr = FSSSecurity.SecurityLists.ModulesList[FSSSecurity.SecurityLists.FSS_Systems.POSADMIN].Where(o => o.ModuleID == (FSSSecurity.SecurityLists.FSS_Modules)item.Key).FirstOrDefault().DisplayModuleText;
                    for (int i = 0; i < splittedActions.Length; i++)
                    {
                        actionID = Convert.ToInt16(splittedActions[i].ToString());
                        actionString = FSSSecurity.SecurityLists.PermissionsList[(FSSSecurity.SecurityLists.FSS_Modules)item.Key].Where(o => o.ActionID == (FSSSecurity.SecurityLists.FSS_Actions)actionID).FirstOrDefault().DisplayActionText;
                        if (!string.IsNullOrEmpty(commaSeparated))
                        {
                            commaSeparated = commaSeparated + ", " + actionString;
                        }
                        else
                        {
                            commaSeparated = actionString;
                        }

                    }

                    newDict.Add(moduleStr, commaSeparated);
                }
            }
            if (newDict.Count == 0)
            {
                return "-1";
            }
            else
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                return ser.Serialize(newDict);
            }
        }

        public userRoleItem getSingleUserRole(long clientID, int? RoleID)
        {
            int tempRoleID = 0;
            if (RoleID.HasValue)
            {
                tempRoleID = Convert.ToInt32(RoleID);
            }
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            var userRole = client.GetUserRole(clientID, true, tempRoleID, true);
            return new userRoleItem
            {
                ClientId = clientID,
                Id = tempRoleID,
                RoleName = userRole.UserRoleName,
                AdminHQSystem = userRole.AdminHQSystem
            };

        }

        public string CreateUpdateUserRole(long ClientID, int UserRoleID, string UserRoleName, string ModuleWithActions, FSSSecurity.SecurityLists.FSS_Systems AdminSystem)
        {
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            string tempStr = client.CreateUpdateUserRoles(ClientID, true, UserRoleID, true, UserRoleName, ModuleWithActions, (int)AdminSystem, true);
            return tempStr;
        }


        public UserRoleDeleteModel getDeleteModel(int? id = 0)
        {
            var entity = getUserRole(id);
            return new UserRoleDeleteModel { Id = entity.Id, Message = null, Name = entity.RoleName, userExists = false };

        }

        // get
        public UserRoleDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new UserRoleDeleteModel { };
                }

                return GetDeleteModelOnError();
            }
            var entity = getUserRole(id);
            return new UserRoleDeleteModel { IsError = false, Name = entity.RoleName };
        }

        //For User Delete
        public UserDeleteModel GetUserDeleteModel(int id = 0, bool isGetAction = true)
        {

            if (id == 0)
            {
                if (isGetAction)
                {
                    return new UserDeleteModel { };
                }
            }
            var entity = getUser(ClientInfoData.GetClientID(), id);
            if (entity == null)
            {
                return null;
            }
            return new UserDeleteModel {IsError = false, Name = entity.LoginName};
        }
        public UserRoleDeleteModel GetDeleteModelOnError()
        {
            return new UserRoleDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        public UserRoleDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            var entity = getUserRole(id);

            if (entity == null)
            {
                return GetDeleteModelOnError();
            }

            return new UserRoleDeleteModel
            {
                Id = entity.Id,
                Name = entity.RoleName,
                Message = errorMessage,
                IsError = !string.IsNullOrWhiteSpace(errorMessage),
            };
        }

        public void DeleteUserRole(int id, string userRoleName)
        {
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            string tempStr = client.DeleteUserRole(clientId, true, id, true);
            if (tempStr != "-1")
            {
                tempStr = unitOfWork.securityRepository.DeleteUserRole(clientId, userRoleName);
            }
        }

        public userRoleItem GetUserRoleByName(string userRoleName)
        {
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            var userRole = client.GetUserRoleByName(clientId, true, userRoleName);

            if (userRole != null)
            {
                return new userRoleItem() { Id = userRole.Id, RoleName = userRole.UserRoleName, AdminHQSystem = userRole.AdminHQSystem, ClientId = clientId };
            }

            return null;
        }

        public void SaveUser(long ClientId, string LoginName, string Password, string FisrtName, string LastName, Int32 EmployeeId, Int32? UserRolesID, out int customerID, out bool custmerResult)
        {
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            int CustID = 0;
            bool resultbool = false;
            int RoleID = 0;

            if (UserRolesID.HasValue)
            {
                RoleID = Convert.ToInt32(UserRolesID);
            }
            client.SaveUser(ClientId, true, LoginName, Password, FisrtName, LastName, EmployeeId, true, RoleID, true, out CustID, out resultbool);
            customerID = CustID;
            custmerResult = resultbool;
        }

        public List<UsersVM> getUsersList(long ClientID, Int32? RoleID)
        {
            int userRoleID = -999;
            if (RoleID.HasValue)
            {
                userRoleID = Convert.ToInt32(RoleID);
            }
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            var usersList = client.GetAllUsers(ClientID, true, userRoleID, true);

            List<UsersVM> returnList = new List<UsersVM>();

            for (int i = 0; i < usersList.Length; i++)
            {
                returnList.Add(new UsersVM { CustomerId = usersList[i].CustomerId, LoginName = usersList[i].LoginName, UserName = usersList[i].UserName, UserRole_Id = usersList[i].UserRoles_ID, UserRoleName = usersList[i].UserRoleName, isActive = usersList[i].IsActive, isPrimary = usersList[i].IsPrimary });
            }

            return returnList;
        }


        public List<UsersVM> getUsersListFiltered(long ClientID, int? RoleID, string searchString, string onlyActiveStr, string onlyPrimaryStr, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection)
        {
            int userRoleID = -999;
            string SearchString = "";
            int OnlyActive = -1;
            int OnlyPrimary = -1;



            /*– Pagination Parameters */
            int PageNo = 1;
            int PageSize = iDisplayLength;

            /*– Sorting Parameters */
            string SortColumn = "";
            string SortOrder = "";

            if (RoleID.HasValue && RoleID != 0)
            {
                userRoleID = Convert.ToInt32(RoleID);
            }

            //OnlyActive = onlyActive == null ? false : onlyActive.ToLower() == "active";
            if (onlyActiveStr != null)  OnlyActive = onlyActiveStr.ToLower() == "active"? 1 : 0;

            //OnlyPrimary = onlyPrimary == null ? false : onlyPrimary.ToLower() == "primary";
            if (onlyPrimaryStr != null) OnlyPrimary = onlyPrimaryStr.ToLower() == "primary" ? 1 : 0;

            /**/
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;// SessionHelper.SearchStr;
            }


            SortColumn = getColmnName(sortColumnIndex);

            SortOrder = sortDirection == "asc" ? "ASC" : "DESC";
            PageNo = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(iDisplayStart) / Convert.ToDouble(iDisplayLength)) + 1);
            /**/
            try 
            { 
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            //var usersList = client.GetAllUsersFiltered(ClientID, true, userRoleID, true, SearchString, Convert.ToInt32(OnlyActive), true, Convert.ToInt32(OnlyPrimary), true, PageNo, true, PageSize, true, SortColumn, SortOrder);
            var usersList = client.GetAllUsersFiltered(ClientID, true, userRoleID, true, SearchString, OnlyActive, true, OnlyPrimary, true, PageNo, true, PageSize, true, SortColumn, SortOrder);

            List<UsersVM> returnList = new List<UsersVM>();

            if (usersList != null && usersList.Count() > 0)
            {
                for (int i = 0; i < usersList.Length; i++)
                {
                    returnList.Add(new UsersVM { UserId = usersList[i].Id, CustomerId = usersList[i].CustomerId, UserName = usersList[i].UserName, LoginName = usersList[i].LoginName, UserRole_Id = usersList[i].UserRoles_ID, UserRoleName = usersList[i].UserRoleName, isActive = usersList[i].IsActive, isPrimary = usersList[i].IsPrimary, AllRecordsCount = usersList[i].AllRecordsCount, IsDeleted = usersList[i].IsDeleted });
                }
                return returnList;
            }
            else
            {
                return null;
            }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 1:
                    retVal = "UserName";
                    break;
                case 2:
                    retVal = "FirstName";
                    break;
                case 3:
                    retVal = "UserRoleName";
                    break;
                case 4:
                    retVal = "IsPrimary";
                    break;
                case 5:
                    retVal = "IsActive";
                    break;
                default:
                    retVal = "UserName";
                    break;
            }

            return retVal;
        }

        public UsersDetailsVM getUser(long ClientID, Int32? CustomerID)
        {
            int customerID = -999;
            if (CustomerID.HasValue)
            {
                customerID = Convert.ToInt32(CustomerID);
            }
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            var user = client.GetUser(ClientID, true, customerID, true);

            UsersDetailsVM userDVM = null;
            if (user != null)
            {
                userDVM = new UsersDetailsVM { LoginName = user.LoginName, Password = user.Password, UserName = user.UserName, UserRolesID = user.UserRoles_ID, EmployeeId = customerID };
            }

            return userDVM;
        }

        public UsersDetailsVM getUserByUsername(long ClientID, string Username)
        {
            var client = new RegistrationService.Registration();
            client.Url = getServiceUrl();
            UsersDetailsVM userDVM = null;
            var user = client.GetUserByUsername(ClientID, true, Username);
            if (user != null)
            {
                userDVM = new UsersDetailsVM { LoginName = user.LoginName, Password = user.Password, UserName = user.UserName, UserRolesID = user.UserRoles_ID, EmployeeId = user.CustomerId };
            }

            return userDVM;
        }

        public void SetErrors(UserRoleDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetUserErrors(UserDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

    }


}

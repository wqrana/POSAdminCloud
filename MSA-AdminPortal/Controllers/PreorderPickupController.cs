using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using MSA_AdminPortal.Helpers;
using Newtonsoft.Json;
using AdminPortalModels.ViewModels;
using Repository;
using Repository.Helpers;
using System.Globalization;
using MSA_AdminPortal.App_Code;

namespace MSA_AdminPortal.Controllers
{
    public class PreorderPickupController : BaseAuthorizedController
    {
        private PreorderPickupHelper preorderPickupHelper = null;
        UnitOfWork unitOfWork = null;
        private HomeRoomHelper homeroomHelper = new HomeRoomHelper();
        private string[] emptyStringArray = new string[0];
        public PreorderPickupController()
        {
            preorderPickupHelper = new PreorderPickupHelper();
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        }
        
        public ActionResult Index()
        {
            try
            {
               long clientId = ClientInfoData.GetClientID();
               UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
               ViewBag.SearchByList = unitOfWork.generalRepository.GetSearchDDLItems();

               ViewBag.GradeList = unitOfWork.generalRepository.getGrades(clientId).ToList();
               ViewBag.HomeRoomList = homeroomHelper.GetSelectList(0);
               ViewBag.SchoolList = unitOfWork.SchoolRepository.GetQuery(x => x.ClientID == clientId && (!x.isDeleted)).Select(x => new { id = x.ID, name = x.SchoolName }).OrderBy(x => x.name);

               PreorderPickupModel model = preorderPickupHelper.GetPreorderPickupModel(clientId);
              // @ViewBag.categoryType = model.categoryTypeList.Select(s => new SelectListItem { Value = s.Value, Text = s.Text }).ToList<SelectListItem>(); 
     
               return View(model);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Index");
                return View();
            }
        }

        public JsonResult GetDropdownData()
        {
            dynamic dropdownData = null;
            string[] emptyStringArray = new string[0];

            ItemSelectionFilters filters = new ItemSelectionFilters();
            filters = new JavaScriptSerializer().Deserialize<ItemSelectionFilters>(Request["filterData"]);

            try
            {
                if (filters != null)
                {
                    if (filters.selectionType == 1)
                    {
                        dropdownData = preorderPickupHelper.GetCategoryTypeList();

                    }
                    else if (filters.selectionType == 2)
                    {
                        dropdownData = preorderPickupHelper.GetCategoryList(filters.categoryType);

                    }

                    else if (filters.selectionType == 3)
                    {

                        dropdownData = preorderPickupHelper.GetItemList(filters.categoryType, filters.category);
                    }
                }

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call");
                return null;
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = dropdownData,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }

        public JsonResult GetPreorderPickupItemsCount()
        {
            PreorderPickupItemsCount resultSet;
            try { 
              PreorderPickupFilters filters = new PreorderPickupFilters();
              filters = new JavaScriptSerializer().Deserialize<PreorderPickupFilters>(Request["filterData"]);

              resultSet = preorderPickupHelper.GetPreorderPickupCount(filters);


                }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call to Picupitem Count");
                return null;
            }

            JsonResult jsonResult = new JsonResult()
            {
                Data = resultSet,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet

            };

            return jsonResult;
        }
        public JsonResult LoadPreorderPickupData(JQueryDataTableParamModel param, PreorderPickupFilters filters)
        {
            

          

          //  PreorderPickupFilters filters = new PreorderPickupFilters();
          //  filters = new JavaScriptSerializer().Deserialize<PreorderPickupFilters>(Request["filterData"]);

            try
            {
                if (filters != null)
                {
                    //Call the helper function to get data
                    int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                    string sortDirection = Request["sSortDir_0"]; // asc or desc
                    int totalRecords;
                    //Default Setting
                    param.iDisplayStart = param.iDisplayStart == 1 ? 0 : param.iDisplayStart;
                    param.iDisplayLength = param.iDisplayLength == 0 ? 25 : param.iDisplayLength;

                    param.iDisplayStart = param.iDisplayStart == -1 ? 0 : param.iDisplayStart;
                    param.iDisplayLength = param.iDisplayLength == -1 ? 1000000 : param.iDisplayLength;
                    


                    int totalDisplayRecords = (param.iDisplayStart - 1) * param.iDisplayLength + param.iDisplayLength;
                    
                    sortDirection = sortDirection == null? "ASC":sortDirection;

                    var preorderData = preorderPickupHelper.GetPreorderPickupData(param, filters, sortColumnIndex, sortDirection, out totalRecords).
                        Select(x => new
                                 {
                                                    
                                    PreOrderItemId= x.PreOrderItemId,
                                    preOrderId= x.preOrderId,
                                    transactionId= x.transactionId,
                                    Grade=x.Grade,
                                    customerName= x.customerName,
                                    userId= x.userId,
                                    CategoryType_Id = x.CategoryType_Id,
                                    Category_Id = x.Category_Id,
                                    itemName = x.itemName,
                                    datePurchased = x.datePurchased.ToString(),
                                    dateToServe = string.Format("{0:MM/dd/yyyy}", x.dateToServe),
                                    
                                    datePickedUp =  x.datePickedUp.ToString(),
                                    received = x.received,
                                    itemVoid = x.itemVoid,
                                    qty = x.qty,
                                    orderVoid= x.orderVoid,
                                    @void = x.@void
                                                            
                        });


                 return Json(new
                 {
                     sEcho = param.sEcho,
                     iTotalRecords = totalRecords,
                     iTotalDisplayRecords = totalRecords,
                     aaData = preorderData
                 },
                 JsonRequestBehavior.AllowGet);

                   
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call to load preorder data");
                return null;
            }


            return null;
          

        }

       // Load Void Order Data
        public JsonResult LoadVoidOrderData(JQueryDataTableParamModel param, VoidRequestType requestParm)
        {

            try
            {
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc
                int totalRecords = 0 ;

                var dataSet = preorderPickupHelper.GetOrderForVoidData(param, requestParm.callingParam, sortColumnIndex, sortDirection, out totalRecords)
                    .Select(x => new
                    {
                        Id = x.Id,
                        OrderId = x.OrderId,
                        PreSaleTrans_Id = x.PreSaleTrans_Id,
                        UserID = x.UserID,
                        CustomerName=x.CustomerName,
                        CanVoid = x.CanVoid,
                        Grade = x.Grade,
                        HasPayment = x.HasPayment,
                        PurchasedDate = string.Format("{0:MM/dd/yyyy}", x.PurchasedDate),
                        Void = x.Void,
                        
                     
                    });
               

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = dataSet
                },
              JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call to load Void Order data");
                return null;

            }

            
        }

        // Load Void Item Data

        public JsonResult LoadVoidItemData(JQueryDataTableParamModel param, VoidRequestType requestParm)
        {

            try
            {

                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"]; // asc or desc
                int totalRecords = 0 ;

                var dataSet = preorderPickupHelper.GetItemForVoidData(param, requestParm.callingParam, sortColumnIndex, sortDirection, out totalRecords)
                    .Select(x=>
                    new
                    {
                       Id = x.Id,
                       orderId =x.orderId,
                       PreorderId =x.PreorderId,
                       PreSaleTrans_Id = x.PreSaleTrans_Id,
                       ItemName = x.ItemName,
                       UserID = x.UserID,
                       customerId = x.customerId,
                       CustomerName = x.CustomerName,
                       Grade = x.Grade,
                       CanVoid = x.CanVoid,
                       isVoid = x.isVoid,
                       Qty = x.Qty,
                       ServingDate = string.Format("{0:MM/dd/yyyy}", x.ServingDate),
                       purchasedDate = string.Format("{0:MM/dd/yyyy}",x.purchasedDate)
                                         
                    });

                foreach (var rec in dataSet)
                {
                    Console.WriteLine(rec.CustomerName);
                }

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = dataSet
                },
              JsonRequestBehavior.AllowGet);                              

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call to load Void Item data");
                return null;

            }


        }
        //Update Void Status
        public JsonResult UpdateVoidStatus()
        {
            VoidUpdateResult resultSet = null;
            VoidRequestType requestParm = new VoidRequestType();
            requestParm = new JavaScriptSerializer().Deserialize<VoidRequestType>(Request["voidRequestParm"]);

            try
            {
                requestParm.clientId = int.Parse(ClientInfoData.GetClientID().ToString());

                resultSet = preorderPickupHelper.UpdateVoid(requestParm);             

            }
            catch (Exception ex)
            {
                Nullable<int> id = null;
                switch(requestParm.callingParam){
                    case "Order":
                        id= requestParm.orderId;
                        break;
                    case "Item":
                        id= requestParm.itemId;
                        break;
                    }
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderPickupController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call to Update Void " +requestParm.callingParam +" For ID:"+ id.Value);

                resultSet = new VoidUpdateResult() { Result = -1, ErrorMessage = "Error while updating Void " + requestParm.callingParam + " For ID:" + id.Value };

                return Json(new
                {

                    aaData = resultSet
                },
          JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {

                aaData = resultSet
            },
           JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxPickupItemsHandler(JQueryDataTableParamModel param, string PreorderIdsList)
        {
            try
            {
                if (Convert.ToInt32(param.sEcho) > 1)
                {
                    int totalRecords;
                    long ClientId = ClientInfoData.GetClientID();
                    int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                    string sortDirection = Request["sSortDir_0"]; // asc or desc

                    var preorderItems = unitOfWork.customPreOrderPickupRespository.GetPreorderPickupItemsList(ClientId, PreorderIdsList, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, out totalRecords);

                    var result = from i in preorderItems
                                 select new[] { i.Id.ToString(), i.TransactionId.ToString(), i.Grade, i.CustomerName, i.UserId, i.ItemName, i.DatePurchased.ToString(), string.Format("{0:M/d/yyyy}", i.DateToServe), i.DatePickedUp.ToString(), i.Qty.ToString(), i.Received.ToString(), i.Void, i.PickupQty.ToString() };

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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PickupItemsController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxPickupItemsHandler");
                return null;
            }
        }

        [HttpPost]
        public ActionResult ProcessPickupItems(List<SelectedPreorderItems> items)
        {
            int status = 0;
            try
            {
                if (items.Count() > 0)
                {
                    long ClientId = ClientInfoData.GetClientID();
                    //Waqar Q. 27/9/2017
                    //DateTime localDateTime = items.FirstOrDefault().DatePickedUp.Value;
                   // unitOfWork.customPreOrderPickupRespository.ProcessPickupPreorderItems(ClientId, ClientInfoData.GetCustomerID(), DateTime.UtcNow, items, out status);

                    DateTime localDateTime = TimeZoneHelper.GetClientTimeZoneLocalDateTime();

                    unitOfWork.customPreOrderPickupRespository.ProcessPickupPreorderItems(ClientId, ClientInfoData.GetCustomerID(), localDateTime, items, out status);

                    return Json(new { result = status }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { result = status }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ProcessPickupItems", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ProcessPickupItems");
                return Json(new { result = status }, JsonRequestBehavior.AllowGet);
            }
        }
   }

// Controller Helper Class
    public class PreorderPickupHelper
    {
        private UnitOfWork unitOfWork = null;
        private CategoryHelper categoryHelper = null;
        private CategoryTypeHelper categoryTypeHelper = null;
        private MenuHelper menuHelper = null;
        
        public PreorderPickupHelper()
        {
            unitOfWork              = new UnitOfWork(ClientInfoData.getConectionString());
            categoryHelper          = new CategoryHelper();
            categoryTypeHelper      = new CategoryTypeHelper();
            menuHelper              = new MenuHelper();


        }

        public PreorderPickupModel GetPreorderPickupModel(long clientID)
        {
            PreorderPickupModel model = new PreorderPickupModel();
            

             try
            {        
                          
               model.ClientID = clientID;
               model.customersList = unitOfWork.reportsRepository.getReportsCustomersList(clientID);
               model.locationList = unitOfWork.generalRepository.getSchools(clientID, null, false).ToList();
               model.gradesList = unitOfWork.generalRepository.getGrades(clientID).ToList();
               model.homeRoomList = unitOfWork.generalRepository.getHomeRooms(clientID).ToList();

               model.dateRangeTypesList = new List<DateRangeType>() { new DateRangeType() { id = 1, name="Serving Date" }, 
                                                                      new DateRangeType() { id=  2, name="Pickup Date" }, 
                                                                      new DateRangeType() { id = 3, name= "Purchased Date" } 
                                                                    };

               model.itemStatusList = new List<ItemStatus>()        {   new ItemStatus(){id=1,name="Not Void"} , 
                                                                        new ItemStatus(){id=2,name="Void"},
                                                                        new ItemStatus(){id=3,name="Both"}
                                                           
                                                                    };

               model.itemSelectionTypeList = new List<ItemSelectionType>() {
                                                                            
                                                                                new ItemSelectionType() {id=1,name= "Category Type"}, 
                                                                                new ItemSelectionType() {id=2,name= "Category"},
                                                                                new ItemSelectionType() {id=3,name= "Item"}
                                                                    
                                                                            };

               model.categoryTypeList = categoryTypeHelper.GetSelectList().OrderBy(x=>x.Text);

               //model.categoryList = categoryHelper.GetSelectList();
              // model.categoryList = new List<SelectListItem>();
               model.categoryList = GetCategoryList(null);
             //  model.itemList = menuHelper.GetAll().OrderBy(x=>x.ItemName);
               model.itemList = GetItemList(null, null);

                               
              

            }
            catch (Exception ex)
            {
               throw ex;
            }

             return model;
        }

        public IEnumerable<SelectListItem> GetCategoryTypeList()
        {

            return categoryTypeHelper.GetSelectList().OrderBy(x=>x.Text);
        }

        public IEnumerable<SelectListItem> GetCategoryList(long? categoryTypeID)
        {

            if (categoryTypeID == null){

                return categoryHelper.GetSelectList().OrderBy(x=>x.Text);
            }
            else
            {

                return categoryHelper.GetAll().Where(x => x.CategoryType_Id == categoryTypeID).Select(s => new SelectListItem() { Value = s.ID.ToString(), Text = s.Name }).OrderBy(x=>x.Text);

            }
        }

        public IEnumerable<SelectListItem> GetItemList(int? categoryTypeID, int? categoryID)
        {

            return unitOfWork.customPreOrderPickupRespository.GetMenuItems(categoryTypeID, categoryID).
                Select(x => new SelectListItem() { Value = x.ID.ToString(), Text = x.ItemName });
                   

            /*

            if (categoryTypeID == null && categoryID == null)
            {
                return menuHelper.GetAll().Where(x=> x.isDeleted == false).OrderBy(x => x.ItemName).Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.ItemName });

            }

            else if (categoryTypeID != null && categoryID == null)
            {

                return menuHelper.GetAll(categoryTypeID.Value).Where(x => x.isDeleted == false).OrderBy(x => x.ItemName).Select(i => new SelectListItem() { Value = i.ID.ToString(), Text = i.ItemName });

            }
            else if (categoryTypeID != null && categoryID != null)
            {

                return menuHelper.GetIndexModelByCategory(categoryID.Value).Where(x =>x.IsDeleted == false).OrderBy(x => x.Name).Select(i => new SelectListItem() { Value = i.Id.ToString(), Text = i.Name });

            }
            */
          
        }

        public PreorderPickupItemsCount GetPreorderPickupCount(PreorderPickupFilters filters)
        {

            return unitOfWork.customPreOrderPickupRespository.GetPreOrderPickupItemsCount(filters);
        }

        public IEnumerable<PreorderPickupList> GetPreorderPickupData(JQueryDataTableParamModel param, PreorderPickupFilters filters, int sortColumnIndex, string sortDirection, out int totalRecords)

        {

            return unitOfWork.customPreOrderPickupRespository.GetPreOrderPickupList(param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, filters, out totalRecords );
                                      


        }

        public IEnumerable<LoadOrderVoidList> GetOrderForVoidData(JQueryDataTableParamModel param, string dataParam, int sortColumnIndex, string sortDirection, out int totalRecords)
        {

            return unitOfWork.customPreOrderPickupRespository.GetOrderForVoidList(param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, dataParam, out totalRecords);

        }


        public IEnumerable<LoadItemVoidList> GetItemForVoidData(JQueryDataTableParamModel param, string dataParam, int sortColumnIndex, string sortDirection, out int totalRecords)
        {

            return unitOfWork.customPreOrderPickupRespository.GetItemForVoidList(param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection, dataParam, out totalRecords);

        }


        public VoidUpdateResult UpdateVoid(VoidRequestType voidRequest){

            VoidUpdateResult result = null;

            if (voidRequest != null)
            {
                try
                {
                    if (voidRequest.callingParam == "Order"){


                     result= unitOfWork.customPreOrderPickupRespository.
                                UpdateVoidOrder(

                                voidRequest.clientId,
                                voidRequest.orderId,
                                voidRequest.orderLogId,
                                voidRequest.orderType,
                                voidRequest.voidPayment

                            );
                      }
                   else if (voidRequest.callingParam == "Item")
                    {
                      result= unitOfWork.customPreOrderPickupRespository.UpdateVoidItem(
                                voidRequest.clientId, 
                                voidRequest.itemId, 
                                voidRequest.orderId, 
                                voidRequest.orderLogId, 
                                voidRequest.customerId, 
                                voidRequest.orderType
                                
                                );

                      }

                    
                }
                catch (Exception ex)
                {
                    throw ex;
                    
                }

            }

            return result;

        }

    }
    
    
}


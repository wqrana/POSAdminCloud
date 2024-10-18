using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Repository;
using Repository.edmx;
using Repository.Helpers;
using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using MSA_AdminPortal.Helpers;

//using MSA_AdminPortal.Models;

namespace MSA_AdminPortal.Controllers
{
    public class MenuController : BaseAuthorizedController
    {
        private MenuHelper helper = new MenuHelper();

        public ActionResult Index(int? id)
        {
            if (!SecurityManager.viewMenu) return RedirectToAction("NoAccess", "Security", new { id = "nomenuitems" });
            if (Request.Cookies["menuView"] != null && Encryption.Decrypt(Request.Cookies["menuView"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table/" + id);
            }

            return RedirectToAction("Tile/" + id);
        }

        public ActionResult GetItemsByCategory(int id)
        {
            if (!SecurityManager.viewMenu) return RedirectToAction("NoAccess", "Security", new { id = "nomenuitems" });
            if (Request.Cookies["menuView"] != null && Encryption.Decrypt(Request.Cookies["menuView"].Value).ToLower() == "table")
            {
                Response.Cookies["menuView"].Value = Encryption.Encrypt("Table");
                var model = helper.GetIndexViewModel();
                model.Category_Id = id;

                return View("Table", model);
            }
            else
            {
                Response.Cookies["menuView"].Value = Encryption.Encrypt("Tile");
                IEnumerable<MenuIndexModel> model = model = helper.GetIndexModelByCategory(id);

                return View("Tile", model);
            }
        }

        public ActionResult Table(int? id)
        {
            Response.Cookies["menuView"].Value = Encryption.Encrypt("Table");
            var model = helper.GetIndexViewModel();

            if (id.HasValue)
            {
                model.CategoryType_Id = id;
                ////changed by farrukh m (allshore) to fix PA-514.
                //   //model.CategoryType_Id = id;
                //   model.Category_Id = id;
                //-------------------------------------
            }


            return View(model);
        }

        //
        // GET: /District/
        public ActionResult Tile(int? id)
        {
            Response.Cookies["menuView"].Value = Encryption.Encrypt("Tile");

            IEnumerable<MenuIndexModel> model = null;
            if (id.HasValue)
            {
                model = helper.GetIndexModel(Convert.ToInt32(id));
            }
            else
            {
                model = helper.GetIndexModel();
            }

            return View(model);
        }

        public JsonResult CheckCategory(int id)
        {
            long clientId = ClientInfoData.GetClientID();
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

                Category temp = unitOfWork.CategoryRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId).Where(x => x.ID == id).FirstOrDefault();

                IEnumerable<CategoryType> stuff = unitOfWork.CategoryTypeRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId);
                CategoryType model = stuff.Where(x => x.ID == temp.CategoryType_Id).FirstOrDefault();

                bool reduced = (bool)model.canReduce;
                bool canFree = (bool)model.canFree;
                if (canFree || reduced)
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "CheckCategory");
                return null;
            }

        }

        [HttpPost]
        public ActionResult SearchAndFilters(MenuIndexViewModel viewmodel)
        {
            var vm = helper.GetIndexViewModel();

            TryUpdateModel<MenuIndexViewModel>(vm);

            return View("Index", vm);
        }

        // also use for SearchAndFilters
        public ActionResult AjaxHandler(JQueryDataTableParamModel param, MenuIndexSearchModel model)
        {
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"]; // asc or desc
            return GetGridJson(param, model, sortColumnIndex, sortDirection);
        }

        //[ChildActionOnly]
        public ActionResult Popup()
        {
            var model = helper.GetCreateModel();

            return GetActionResult(Request, model);
        }

        //[ChildActionOnly]
        public ActionResult Create()
        {
            var model = helper.GetCreateModel();

            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult Create(MenuCreateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = helper.Get();

                    TryUpdateModel<Menu>(entity);

                    entity.ID = helper.GetNextId();
                    entity.KitchenItem = (model.KitchenItem.HasValue && model.KitchenItem.Value) ? 1 : 0;
                    entity.LastUpdateLocal = DateTime.Now;
                    entity.LastUpdate = DateTime.UtcNow;


                    try
                    {
                        var msg = helper.Create(entity);
                        if (!string.IsNullOrEmpty(msg))
                            helper.SetErrors(model, msg);
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                        helper.SetErrors(model, ViewData);
                    }

                    model.Id = entity.ID;
                }
                else
                {
                    helper.SetErrors(model, ViewData);
                }

                return GetActionResult(Request, model, false);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
                return null;
            }
        }

        //[ChildActionOnly]
        public ActionResult Edit(int id = 0)
        {
            var model = helper.GetEditModel(id);

            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult Edit(MenuUpdateModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = helper.Get(model.Id);

                    entity.LastUpdateLocal = DateTime.Now;
                    entity.LastUpdate = DateTime.UtcNow;


                    if (entity == null)
                    {
                        return GetActionResult(Request, helper.GetEditModelOnError(), false);
                    }

                    TryUpdateModel<Menu>(entity);

                    entity.KitchenItem = (model.KitchenItem.HasValue && model.KitchenItem.Value) ? 1 : 0;

                    try
                    {
                        var msg = helper.Update(entity);
                        if (!string.IsNullOrEmpty(msg))
                            helper.SetErrors(model, msg);
                    }
                    catch (Exception ex)
                    {
                        //Error logging in cloud tables 
                        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
                        helper.SetErrors(model, ViewData);
                    }
                }
                else
                {
                    helper.SetErrors(model, ViewData);
                }

                return GetActionResult(Request, model, false);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
                return null;
            }
        }

        //[ChildActionOnly]
        public ActionResult Delete(int id = 0)
        {
            var model = helper.GetDeleteModel(id, true);

            return GetActionResult(Request, model);
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
                    helper.SoftDelete(id);

                    model.Message = "The item record has been deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }

            return GetActionResult(Request, model, false);
        }

        private ActionResult GetGridJson(JQueryDataTableParamModel param, MenuIndexSearchModel model, int sortColumnIndex, string sortDirection)
        {

            try
            {
                int outItemsCount = 0;
                IEnumerable<Admin_Menu_List_Result> menuquery = helper.GetMenuItems(param, model, out outItemsCount, sortColumnIndex, sortDirection);

                IEnumerable<Admin_Menu_List_Result> menuItems = null;
                menuItems = menuquery;
                var displayedItems = menuItems;

                var result = displayedItems.Select(x => new string[] { x.MenuID.ToString(), x.MenuID.ToString(), x.MenuName, x.CategoryColor, x.CategoryName, x.UPC });
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = outItemsCount,
                    iTotalDisplayRecords = outItemsCount,
                    aaData = result
                },
                JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetGridJson");
                return null;
            }
        }

        private ActionResult GetActionResult(HttpRequestBase request, MenuModel model, bool isGetAction = true)
        {
            var vm = helper.GetViewModel(model);

            return GetActionResult(request, vm, isGetAction);
        }

        private ActionResult GetActionResult(HttpRequestBase request, MenuViewModel viewModel, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            if (viewModel.MenuItem.IsError || isGetAction)
            {
                return PartialView("Popup", viewModel);
            }

            return RedirectToAction("Index");
        }

        private ActionResult GetActionResult(HttpRequestBase request, MenuDeleteModel model, bool isGetAction = true)
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
    }

    public class MenuHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString(), ClientInfoData.GetClientID());

        // for create/update
        public void SetErrors(MenuModel model, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    model.Message += error.ErrorMessage + "<br />";
                }
            }

            model.IsError = true;
        }

        // for delete
        public void SetErrors(MenuDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetErrors(MenuCreateModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }
        public void SetErrors(MenuUpdateModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetErrors(MenuDeleteModel model, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    model.Message += error.ErrorMessage + "<br />";
                }
            }

            model.IsError = true;
        }

        public long GetNextId()
        {
            try
            {
                return unitOfWork.MenuRepository.Get(x => x.ClientID == clientId).DefaultIfEmpty().Max(x => x == null ? 0 : x.ID) + 1;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetNextId");
                return 0;
            }
        }

        public IEnumerable<Menu> GetAll()
        {
            try
            {
                return unitOfWork.MenuRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public IEnumerable<Menu> GetAll(long categoryTypeID)
        {
            try
            {
                IQueryable<Menu> menu = unitOfWork.MenuRepository.GetQuery(o => o.ClientID == clientId);
                IQueryable<Category> category = unitOfWork.CategoryRepository.GetQuery(o => o.ClientID == clientId);
                IQueryable<CategoryType> catType = unitOfWork.CategoryTypeRepository.GetQuery(o => o.ClientID == clientId);

                var query = from m in menu
                            join c in category on new { m.Category_Id } equals new { Category_Id = c.ID }
                            join ct in catType on (long?)c.CategoryType_Id equals ct.ID
                            where m.Category_Id != null && c.CategoryType_Id != null
                            && m.isDeleted != null && c.CategoryType_Id == categoryTypeID && c.isActive == true
                            select m;

                return query.AsEnumerable<Menu>();

                //return unitOfWork.MenuRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted.Value) && x.ClientID == clientId && x.Category.CategoryType.ID == categoryTypeID);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll(int)");
                return null;
            }
        }

        public Menu Get()
        {
            return new Menu { ClientID = clientId, isDeleted = false };
        }

        public Menu Get(long id)
        {
            try
            {
                return GetAll().Where(x => x.ID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public IEnumerable<MenuIndexModel> GetIndexModel()
        {
            try
            {
                //joins
                IQueryable<Menu> menu = unitOfWork.MenuRepository.GetQuery(x => x.ClientID == clientId);
                IQueryable<Category> category = unitOfWork.CategoryRepository.GetQuery(x => x.ClientID == clientId);

                var query = from m in menu
                            join c in category
                            on new { m.Category_Id } equals new { Category_Id = c.ID }
                            //where m.Category_Id !=null 
                            where m.isDeleted == false
                            orderby m.ItemName
                            select (
                           new MenuIndexModel
                           {
                               Id = m.ID,
                               Name = m.ItemName,
                               CategoryName = c.Name,
                               UPCCode = m.UPC,
                               Color = c.Color,
                           });
                return query;

                //return GetAll().OrderBy(o => o.ItemName)
                //       .Select(x =>
                //            new MenuIndexModel
                //            {
                //                Id = x.ID,
                //                Name = x.ItemName,
                //                CategoryName = x.Category.Name,
                //                UPCCode = x.UPC,
                //                Color = x.Category.Color,
                //            });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public IEnumerable<MenuIndexModel> GetIndexModelByCategory(int categoryId)
        {
            try
            {
                //joins
                IQueryable<Menu> menu = unitOfWork.MenuRepository.GetQuery(x => x.ClientID == clientId);
                IQueryable<Category> category = unitOfWork.CategoryRepository.GetQuery(x => x.ClientID == clientId);

                var query = from m in menu
                            join c in category
                            on new { m.Category_Id } equals new { Category_Id = c.ID }
                            where m.Category_Id == categoryId && m.isDeleted == false
                            select (
                           new MenuIndexModel
                           {
                               Id = m.ID,
                               Name = m.ItemName,
                               CategoryName = c.Name,
                               UPCCode = m.UPC,
                               Color = c.Color,
                               IsDeleted = m.isDeleted
                           });
                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public IEnumerable<MenuIndexModel> GetIndexModel(Int32 categoryTypeID)
        {
            try
            {
                var query = from menu in GetAll(categoryTypeID)
                            join category in unitOfWork.CategoryRepository.Get(c => c.ClientID == clientId) on
                            menu.Category_Id equals category.ID
                            where menu.isDeleted == false
                            orderby menu.ItemName
                            select new MenuIndexModel
                            {
                                Id = menu.ID,
                                Name = menu.ItemName,
                                CategoryName = category.Name,
                                UPCCode = menu.UPC,
                                Color = category.Color,
                            };

                return query;

                //return GetAll(categoryTypeID).OrderBy(o => o.ItemName)
                //        .Select(x =>
                //             new MenuIndexModel
                //             {
                //                 Id = x.Id,
                //                 Name = x.ItemName,
                //                 CategoryName = x.Category.Name,
                //                 UPCCode = x.UPC,
                //                 Color = x.Category.Color,
                //             });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }

        public MenuIndexViewModel GetIndexViewModel()
        {
            try
            {
                var categoryHelper = new CategoryHelper();
                var categoryTypeHelper = new CategoryTypeHelper();

                var yesNoList = new SelectListItem[] { new SelectListItem() { Text = "Both", Value = "" }, new SelectListItem() { Text = "Yes", Value = "1" }, new SelectListItem() { Text = "No", Value = "0" } };
                var searchByList = new SelectListItem[] { new SelectListItem() { Text = "Item Name", Value = "0" }, new SelectListItem() { Text = "Category Name", Value = "1" }, new SelectListItem() { Text = "UPC Code", Value = "2" } };

                return new MenuIndexViewModel
                {
                    SearchByList = searchByList,
                    CategoryList = categoryHelper.GetSelectList().OrderBy(x=> x.Text),
                    CategoryTypeList = categoryTypeHelper.GetSelectList(),
                    TaxableList = yesNoList,
                    KitchenItemList = yesNoList,
                    ScaleItemList = yesNoList,
                };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexViewModel");
                return null;
            }
        }

        public MenuViewModel GetViewModel(MenuModel model)
        {
            var categoryHelper = new CategoryHelper();

            return new MenuViewModel
            {
                CategoryList = categoryHelper.GetSelectList().OrderBy(x=> x.Text),
                MenuItem = model
            };
        }

        // get
        public MenuCreateModel GetCreateModel()
        {
            return new MenuCreateModel { };
        }

        public MenuUpdateModel GetEditModelOnError()
        {
            return new MenuUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public MenuUpdateModel GetEditModel(int id)
        {
            try
            {
                var entity = Get(id);

                if (entity == null)
                {
                    return GetEditModelOnError();
                }

                MenuUpdateModel MnuUpdateMdl = new MenuUpdateModel();
                MnuUpdateMdl.Id = entity.ID;
                MnuUpdateMdl.Category_Id = entity.Category_Id;
                MnuUpdateMdl.ItemName = entity.ItemName;
                MnuUpdateMdl.PreOrderDesc = entity.PreOrderDesc;
                MnuUpdateMdl.StudentFullPrice = entity.StudentFullPrice;
                MnuUpdateMdl.StudentRedPrice = entity.StudentRedPrice;
                MnuUpdateMdl.EmployeePrice = entity.EmployeePrice;
                MnuUpdateMdl.GuestPrice = entity.GuestPrice;
                MnuUpdateMdl.ItemType = entity.ItemType == 1 ? ItemType.LunchItem : entity.ItemType == 2 ? ItemType.Breakfast : ItemType.NA;
                MnuUpdateMdl.isTaxable = entity.isTaxable;
                MnuUpdateMdl.isScaleItem = entity.isScaleItem;
                MnuUpdateMdl.isOnceDay = entity.isOnceDay;
                MnuUpdateMdl.KitchenItem = (entity.KitchenItem.HasValue && entity.KitchenItem.Value == 1);
                MnuUpdateMdl.UPC = entity.UPC;
                MnuUpdateMdl.ButtonCaption = entity.ButtonCaption;
                MnuUpdateMdl.displayReducedPrice = isReducedPrice(entity.Category_Id);
                return MnuUpdateMdl;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetEditModel");
                return null;
            }


        }
        private bool isReducedPrice(long categoryID)
        {
            try
            {
                long clientId = ClientInfoData.GetClientID();
                UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

                Category temp = unitOfWork.CategoryRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId).Where(x => x.ID == categoryID).FirstOrDefault();

                IEnumerable<CategoryType> stuff = unitOfWork.CategoryTypeRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId);
                CategoryType model = stuff.Where(x => x.ID == temp.CategoryType_Id).FirstOrDefault();

                bool reduced = (bool)model.canReduce;
                bool canFree = (bool)model.canFree;
                if (canFree || reduced)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "isReducedPrice");
                return false;
            }
        }

        public MenuDeleteModel GetDeleteModelOnError()
        {
            return new MenuDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public MenuDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new MenuDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }

        // post
        public MenuDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            try
            {
                var entity = Get(id);

                if (entity == null)
                {
                    return GetDeleteModelOnError();
                }

                return new MenuDeleteModel
                {
                    Id = entity.ID,
                    Name = entity.ItemName,
                    Message = errorMessage,
                    IsError = !string.IsNullOrWhiteSpace(errorMessage),
                };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDeleteModel");
                return null;
            }
        }

        public string Create(Menu entity)
        {
            try
            {
                bool alreadyExist = false;
                alreadyExist = unitOfWork.MenuRepository.Get(x => x.ClientID == clientId && x.ItemName == entity.ItemName).Count() > 0 ? true : false;
                if (alreadyExist)
                {
                    return "Menu item already exists or it is marked as deleted.";
                }
                else
                {
                    unitOfWork.MenuRepository.Insert(entity);
                    unitOfWork.Save();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
                return ex.Message;
            }
        }

        public string Update(Menu entity)
        {
            try
            {
                //aginst bug 1903
                //bool alreadyExist = false;
                //alreadyExist = unitOfWork.MenuRepository.Get(x => x.ClientID == clientId && x.ItemName == entity.ItemName).Count() > 0 ? true : false;
                //if (alreadyExist)
                //{
                //    return "Menu item already exists and it is marked deleted.";
                //}
                //else
                //{
                    unitOfWork.MenuRepository.Update(entity);
                    unitOfWork.Save();
                    return "";
                //}
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Update");
                return ex.Message;
            }
        }

        public void SoftDelete(int id = 0)
        {
            var entity = Get(id);

            SoftDelete(entity);
        }

        public void SoftDelete(Menu entity)
        {
            entity.isDeleted = true;

            Update(entity);
        }

        public IEnumerable<Admin_Menu_List_Result> GetMenuItems(JQueryDataTableParamModel param, MenuIndexSearchModel model, out int itemsCount, int sortColumnIndex, string sortDirection)
        {
            try
            {
                IEnumerable<CategoryType> CategoryTypes = unitOfWork.CategoryTypeRepository.Get(x => x.ClientID == clientId);
                IEnumerable<Category> Category = unitOfWork.CategoryRepository.Get(x => x.ClientID == clientId);
                IEnumerable<Menu> Menu = unitOfWork.MenuRepository.Get(x => x.ClientID == clientId);

                var query = from ct in CategoryTypes
                            join c in Category on ct.ID equals (long?)c.CategoryType_Id
                            join m in Menu on c.ID equals m.Category_Id
                            where (ct.isDeleted == false && m.isDeleted == false)
                            select new Admin_Menu_List_Result()
                            {
                                CategoryColor = c.Color,
                                CategoryId = m.Category_Id,
                                CategoryName = c.Name,
                                CategoryTypeId = c.CategoryType_Id,
                                ClientID = ct.ClientID,
                                isMenuDeleted = m.isDeleted,
                                isScaleItem = m.isScaleItem,
                                isTaxable = m.isTaxable,
                                KitchenItem = m.KitchenItem,
                                MenuID = m.ID,
                                MenuName = m.ItemName,
                                UPC = m.UPC
                            };

                //IQueryable<Menu> menuQuery = unitOfWork..MenuRepository.GetQuery(x => (x.isDeleted.Equals(null) || !x.isDeleted.Value) && x.ClientID == clientId);
                IQueryable<Admin_Menu_List_Result> menuQuery = query.AsQueryable();//unitOfWork.MenuSPRepository.AsQueryable(); // .(x => (x.isMenuDeleted.Equals(null) || !x.isMenuDeleted.Value) && x.ClientID == clientId);
                itemsCount = menuQuery.Count();
                bool isSearchDone = false;



                if (model.SearchBy_Id.HasValue && !string.IsNullOrWhiteSpace(model.SearchBy))
                {
                    isSearchDone = true;
                    if (model.SearchBy_Id.Value == 0)
                    {
                        menuQuery = menuQuery.Where(x => x.MenuName.ToLower().Contains(model.SearchBy.ToLower())).OrderBy(x => x.MenuName.IndexOf(model.SearchBy.ToLower()));
                    }
                    else if (model.SearchBy_Id.Value == 1)
                    {
                        menuQuery = menuQuery.Where(x => x.CategoryName.ToLower().Contains(model.SearchBy.ToLower())).OrderBy(x => x.CategoryName.IndexOf(model.SearchBy.ToLower()));
                    }
                    else if (model.SearchBy_Id.Value == 2)
                    {
                        menuQuery = menuQuery.Where(x => x.UPC.ToLower().Contains(model.SearchBy.ToLower())).OrderBy(x => x.UPC.IndexOf(model.SearchBy.ToLower()));
                    }
                }
                else if (model.SearchBy_Id.HasValue)
                {
                    if (sortColumnIndex == 0)
                    {
                        if (model.SearchBy_Id.Value == 0)
                        {
                            sortColumnIndex = 1;
                        }
                        else if (model.SearchBy_Id.Value == 1)
                        {
                            sortColumnIndex = 2;
                        }
                        else if (model.SearchBy_Id.Value == 2)
                        {
                            sortColumnIndex = 3;
                        }
                    }
                }

                if (model.CategoryType_Id.HasValue)
                {
                    menuQuery = menuQuery.Where(x => x.CategoryTypeId == model.CategoryType_Id.Value);
                }

                if (model.Category_Id.HasValue)
                {
                    menuQuery = menuQuery.Where(x => x.CategoryId == model.Category_Id.Value);
                }

                if (model.hdCategory_Id.HasValue)
                {
                    menuQuery = menuQuery.Where(x => x.CategoryId == model.hdCategory_Id.Value);
                }

                if (model.Taxable_Id.HasValue)
                {
                    menuQuery = menuQuery.Where(x => x.isTaxable == (model.Taxable_Id.Value == 1));
                }

                if (model.KitchenItem_Id.HasValue)
                {
                    menuQuery = menuQuery.Where(x => x.KitchenItem == model.KitchenItem_Id.Value);//(model.KitchenItem_Id.Value == 1));
                }

                if (model.ScaleItem_Id.HasValue)
                {
                    menuQuery = menuQuery.Where(x => x.isScaleItem == (model.ScaleItem_Id.Value == 1));
                }

                if (!isSearchDone)
                {
                    string columnName = getColmnName(sortColumnIndex);

                    if (columnName == "ItemName")
                    {
                        if (sortDirection == "asc")
                        {
                            menuQuery = menuQuery.OrderBy(p => p.MenuName);
                        }
                        else
                        {
                            menuQuery = menuQuery.OrderByDescending(p => p.MenuName);
                        }
                    }
                    else if (columnName == "CatName")
                    {

                        if (sortDirection == "asc")
                        {
                            menuQuery = menuQuery.OrderBy(p => p.CategoryName);
                        }
                        else
                        {
                            menuQuery = menuQuery.OrderByDescending(p => p.CategoryName);
                        }
                    }
                    else if (columnName == "UPCCode")
                    {

                        if (sortDirection == "asc")
                        {
                            menuQuery = menuQuery.OrderBy(p => p.UPC);
                        }
                        else
                        {
                            menuQuery = menuQuery.OrderByDescending(p => p.UPC);
                        }
                    }
                    else
                    {
                        if (sortDirection == "asc")
                        {
                            menuQuery = menuQuery.OrderBy(p => p.MenuName);
                        }
                        else
                        {
                            menuQuery = menuQuery.OrderByDescending(p => p.MenuName);
                        }
                    }
                }
                itemsCount = menuQuery.Count();
                if (param.iDisplayLength != -1)
                {
                    menuQuery = menuQuery
                                    .Skip(param.iDisplayStart)
                                    .Take(param.iDisplayLength);
                }

                return menuQuery.AsEnumerable();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "MenuController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetMenuItems");
                itemsCount = 0;
                return null;
            }
        }

        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 0:
                    retVal = "ItemName";
                    break;
                case 1:
                    retVal = "ItemName";
                    break;
                case 2:
                    retVal = "CatName";
                    break;
                case 3:
                    retVal = "UPCCode";
                    break;
                default:
                    retVal = "ItemName";
                    break;
            }

            return retVal;
        }

    }


}
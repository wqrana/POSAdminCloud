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

namespace MSA_AdminPortal.Controllers
{
    public class CategoryController : BaseAuthorizedController
    {
        static int categoryType = 0;
        private CategoryHelper helper = new CategoryHelper();



        public ActionResult Index(int? id)
        {
            if (!SecurityManager.viewCategories) return RedirectToAction("NoAccess", "Security", new { id = "nocategories" });

            if (Request.Cookies["categoryview"] != null && Encryption.Decrypt(Request.Cookies["categoryview"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table/" + id);
            }

            return RedirectToAction("Tile/" + id);

        }

        public ActionResult Table(int? id)
        {
            Response.Cookies["categoryview"].Value = Encryption.Encrypt("Table");
            if (id.HasValue)
            {
                categoryType = Convert.ToInt32(id);
            }
            else
            {
                categoryType = 0;
            }
            return View();
        }

        //
        // GET: /District/
        public ActionResult Tile(int? id)
        {
            Response.Cookies["categoryview"].Value = Encryption.Encrypt("Tile");
            IEnumerable<CategoryIndexModel> model = null;

            try
            {
                if (id.HasValue)
                {
                    model = helper.GetIndexModel(Convert.ToInt32(id));
                }
                else
                {
                    model = helper.GetIndexModel();
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Tile");
            }

            return View(model);
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param, MenuIndexSearchModel model, int? id)
        {
            if (categoryType != 0)
            {
                model.CategoryType_Id = categoryType;
            }
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"]; // asc or desc
            return GetGridJson(param, model, sortColumnIndex, sortDirection);
        }

        private ActionResult GetGridJson(JQueryDataTableParamModel param, MenuIndexSearchModel model, int sortColumnIndex, string sortDirection)
        {

            try
            {
                int outItemsCount = 0;
                IEnumerable<CategoryIndexModel> menuquery = helper.GetCategories(param, model, out outItemsCount, sortColumnIndex, sortDirection);
                //var filteredItems = menuquery;

                IEnumerable<CategoryIndexModel> menuItems = null;
                menuItems = menuquery;
                var displayedItems = menuItems;

                var result = displayedItems.Select(x => new string[] { x.Id.ToString(), x.Id.ToString(), x.Color, x.Name, x.CategoryType, x.ItemCount.ToString(), x.Id.ToString() });
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetGridJson");
                return null;
            }
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
            model.IsActive = true;
            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult Create(CategoryCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = helper.Get();

                TryUpdateModel<Category>(entity);

                //entity.Id = helper.GetNextId();

                try
                {
                    var msg=helper.Create(entity);
                    if (!string.IsNullOrEmpty(msg))
                        helper.SetErrors(model, msg);
                }
                catch (Exception ex)
                {
                    helper.SetErrors(model, ViewData);
                    //Error logging in cloud tables 
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
                }

                model.Id = entity.ID;
            }
            else
            {
                helper.SetErrors(model, ViewData);
            }

            return GetActionResult(Request, model, false);
        }

        //[ChildActionOnly]
        public ActionResult Edit(int id = 0)
        {
            var model = helper.GetEditModel(id);

            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult Edit(CategoryUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = helper.Get(model.Id);

                if (entity == null)
                {
                    return GetActionResult(Request, helper.GetEditModelOnError(), false);
                }

                TryUpdateModel<Category>(entity);
                try
                {

                    var msg=helper.Update(entity);
                    if (!string.IsNullOrEmpty(msg))
                        helper.SetErrors(model, msg);
                }
                catch (Exception ex)
                {
                    //Error logging in cloud tables 
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
                    helper.SetErrors(model, ViewData);
                }
            }
            else
            {
                helper.SetErrors(model, ViewData);
            }

            return GetActionResult(Request, model, false);
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

                    string msg = helper.SoftDelete(id);
                    if (msg == "childRecordsExists")
                    {
                        string message = "This Category type can’t be deleted, some item(s) are referencing it.";
                        helper.SetErrors(model, message);
                    }
                    else
                    {

                        model.Message = "The Category has been deleted successfully.";
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
                helper.SetErrors(model, ex.Message);
            }

            return GetActionResult(Request, model, false);
        }

        private ActionResult GetActionResult(HttpRequestBase request, CategoryModel model, bool isGetAction = true)
        {
            var vm = helper.GetViewModel(model);

            return GetActionResult(request, vm, isGetAction);
        }

        private ActionResult GetActionResult(HttpRequestBase request, CategoryViewModel viewModel, bool isGetAction = true)
        {
            if (request.IsAjaxRequest())
            {
                return Json(viewModel, JsonRequestBehavior.AllowGet);
            }

            if (viewModel.CategoryModel.IsError || isGetAction)
            {
                return PartialView("Popup", viewModel);
            }

            return RedirectToAction("Index");
        }

        private ActionResult GetActionResult(HttpRequestBase request, CategoryDeleteModel model, bool isGetAction = true)
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

    public class CategoryHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
        private CategoryTypeHelper categoryTypeHelper = new CategoryTypeHelper();

        // for create/update
        public void SetErrors(CategoryModel model, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    model.Message += error.ErrorMessage + "\r\n";
                }
            }

            model.IsError = true;
        }



        // for delete
        public void SetErrors(CategoryDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetErrors(CategoryCreateModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetErrors(CategoryUpdateModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetErrors(CategoryDeleteModel model, ViewDataDictionary viewData)
        {
            foreach (ModelState modelState in viewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    model.Message += error.ErrorMessage + "\r\n";
                }
            }

            model.IsError = true;
        }

        public long GetNextId()
        {
            try
            {
                return unitOfWork.CategoryRepository.Get(x => x.ClientID == clientId).DefaultIfEmpty().Max(x => x == null ? 0 : x.ID) + 1;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetNextId");
                return 0;
            }
        }

        public IEnumerable<SelectListItem> GetSelectList()
        {
            return GetAll().Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name });
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                IQueryable<Category> category = unitOfWork.CategoryRepository.GetQuery(o => o.ClientID == clientId);
                IQueryable<CategoryType> catType = unitOfWork.CategoryTypeRepository.GetQuery(o => o.ClientID == clientId);

                var query = from c in category
                            join ct in catType on (long?)c.CategoryType_Id equals ct.ID
                            where (c.CategoryType_Id != null && ct.isDeleted == false && c.isDeleted==false)

                            select c;

                return query.AsEnumerable<Category>();
                //return unitOfWork.CategoryRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted.Value) && (x.CategoryType.isDeleted.Equals(null) || !x.CategoryType.isDeleted.Value) && x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public IEnumerable<Category> GetAll(Int32 categoryTypeID)
        {
            try
            {
                //IQueryable<Menu> menu = unitOfWork.MenuRepository.GetQuery(o => o.ClientID == clientId);
                IQueryable<Category> category = unitOfWork.CategoryRepository.GetQuery(o => o.ClientID == clientId);
                IQueryable<CategoryType> catType = unitOfWork.CategoryTypeRepository.GetQuery(o => o.ClientID == clientId);

                var query = from c in category
                            //join c in category on new { m.Category_Id } equals new { Category_Id = c.ID }
                            join ct in catType on (long?)c.CategoryType_Id equals ct.ID
                            where c.CategoryType_Id != null
                            && c.CategoryType_Id == categoryTypeID
                            select c;

                return query.AsEnumerable<Category>();
                //return unitOfWork.CategoryRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted.Value) && (x.CategoryType.isDeleted.Equals(null) || !x.CategoryType.isDeleted.Value) && x.ClientID == clientId && x.CategoryType_Id == categoryTypeID);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll(int CateogryTyeID)");
                return null;
            }
        }

        public Category Get()
        {
            return new Category { ClientID = clientId, isDeleted = false };
        }

        public Category Get(long id)
        {
            return GetAll().Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<CategoryIndexModel> GetIndexModel()
        {
            try
            {
                IQueryable<Menu> menu = unitOfWork.MenuRepository.GetQuery(x => x.ClientID == clientId && x.isDeleted==false);
                var categoriesindexModel = from c in unitOfWork.menuFunctionsRepository.GetCategoriesList(clientId, -999, -999, 0, 1000, 0, "asc")

                                          select new CategoryIndexModel
                                          {
                                              Id = c.catID ?? 0,
                                              Name = c.catName,
                                              CategoryType = c.catTypeName,
                                              Color = c.catColor,
                                              ItemCount = menu.Where(x=>x.Category_Id==c.catID).Count(),


                                          };

                //IEnumerable<Menu> menu = unitOfWork.MenuRepository.GetQuery(o => o.ClientID == clientId);
                //IEnumerable<Category> category = unitOfWork.CategoryRepository.GetQuery(o => o.ClientID == clientId);
                //IEnumerable<CategoryType> catType = unitOfWork.CategoryTypeRepository.GetQuery(o => o.ClientID == clientId);

                //IEnumerable<CategoryIndexModel> categoriesindexModel = from c in category
                //                                                       join ct in catType on (long?)c.CategoryType_Id equals ct.ID
                //                                                       where c.CategoryType_Id != null
                //                                                       select new CategoryIndexModel
                //                                                       {
                //                                                           Id = c.ID,
                //                                                           Name = c.Name,
                //                                                           CategoryType = ct.Name,
                //                                                           Color = c.Color,
                //                                                           ItemCount = unitOfWork.generalRepository.getItemsCountofCategory(c.ClientID, c.ID)
                //                                                       };
                return categoriesindexModel;

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel");
                return null;
            }
        }

        public IEnumerable<CategoryIndexModel> GetIndexModel(Int32 categoryTypeID)
        {
            try
            {
                IQueryable<Menu> menu = unitOfWork.MenuRepository.GetQuery(x => x.ClientID == clientId && x.isDeleted == false);
                var categoriesWithCount = from c in unitOfWork.menuFunctionsRepository.GetCategoriesList(clientId, -999, Convert.ToInt64(categoryTypeID), 0, 1000, 0, "asc")

                                     select new CategoryIndexModel
                                     {
                                         Id = c.catID ?? 0,
                                         Name = c.catName,
                                         CategoryType = c.catTypeName,
                                         Color = c.catColor,
                                         ItemCount = menu.Where(x => x.Category_Id == c.catID).Count(),


                                     };

                //IQueryable<Menu> menu = unitOfWork.MenuRepository.GetQuery(o => o.ClientID == clientId);
                //IQueryable<Category> category = unitOfWork.CategoryRepository.GetQuery(o => o.ClientID == clientId);
                //IQueryable<CategoryType> catType = unitOfWork.CategoryTypeRepository.GetQuery(o => o.ClientID == clientId);

                //IEnumerable<CategoryIndexModel> categoriesWithCount = from c in category
                //                                                      //join c in category on new { m.Category_Id } equals new { Category_Id = c.ID }
                //                                                      join ct in catType on (long?)c.CategoryType_Id equals ct.ID
                //                                                      where c.CategoryType_Id != null && c.CategoryType_Id == categoryTypeID
                //                                                      select new CategoryIndexModel
                //                                                      {
                //                                                          Id = c.ID,
                //                                                          Name = c.Name,
                //                                                          CategoryType = ct.Name,
                //                                                          Color = c.Color,
                //                                                          ItemCount = unitOfWork.generalRepository.getItemsCountofCategory(c.ClientID, c.ID)
                //                                                      };
                return categoriesWithCount;

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetIndexModel(int)");
                return null;
            }
        }

        public IEnumerable<CategoryIndexModel> GetCategories(JQueryDataTableParamModel param, MenuIndexSearchModel model, out int itemsCount, int sortColumnIndex, string sortDirection)
        {
            try
            {
                var categoriesList = from c in unitOfWork.menuFunctionsRepository.GetCategoriesList(clientId, -999, model.CategoryType_Id == null ? -999 : model.CategoryType_Id.Value, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection)

                                                                 select new CategoryIndexModel
                                                                 {
                                                                     Id = c.catID ??0,
                                                                     Name = c.catName,
                                                                     CategoryType = c.catTypeName,
                                                                     Color = c.catColor,
                                                                     ItemCount = c.ItemCount??0,
                                                                     
                                                                     
                                                                 };
                var singleCategory = unitOfWork.menuFunctionsRepository.GetCategoriesList(clientId, -999, -999, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortDirection).FirstOrDefault();
                itemsCount = Convert.ToInt32(singleCategory.totalCount);
                return categoriesList;
                //IEnumerable<Category> categoryQuery = unitOfWork.CategoryRepository.GetQuery(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId);

                //if (model.CategoryType_Id.HasValue)
                //{
                //    categoryQuery = categoryQuery.Where(p => p.CategoryType_Id == model.CategoryType_Id);
                //}
                //itemsCount = categoryQuery.Count();

                //IEnumerable<Menu> menu = unitOfWork.MenuRepository.GetQuery(o => o.ClientID == clientId);
                //IEnumerable<Category> category = unitOfWork.CategoryRepository.GetQuery(o => o.ClientID == clientId);
                //IEnumerable<CategoryType> catType = unitOfWork.CategoryTypeRepository.GetQuery(o => o.ClientID == clientId);


                //IEnumerable<CategoryIndexModel> categoriesWithCount = from c in category
                //                                                      join ct in catType on (long?)c.CategoryType_Id equals ct.ID
                //                                                      where c.CategoryType_Id != null
                //                                                      select new CategoryIndexModel
                //                                                      {
                //                                                          Id = c.ID,
                //                                                          Name = c.Name,
                //                                                          CategoryType = ct.Name,
                //                                                          Color = c.Color,
                //                                                          ItemCount = 55//temporary adjustment
                //                                                      };



                //if (sortColumnIndex == 0 || sortColumnIndex == 1)
                //{
                //    if (sortDirection == "asc")
                //    {
                //        categoriesWithCount = categoriesWithCount.OrderBy(p => p.Name);
                //    }
                //    else
                //    {
                //        categoriesWithCount = categoriesWithCount.OrderByDescending(p => p.Name);
                //    }
                //}
                //else if (sortColumnIndex == 2)
                //{
                //    if (sortDirection == "asc")
                //    {
                //        categoriesWithCount = categoriesWithCount.OrderBy(p => p.CategoryType);
                //    }
                //    else
                //    {
                //        categoriesWithCount = categoriesWithCount.OrderByDescending(p => p.CategoryType);
                //    }
                //}
                //else if (sortColumnIndex == 3)
                //{

                //    if (sortDirection == "asc")
                //    {
                //        categoriesWithCount = categoriesWithCount.OrderBy(p => p.ItemCount);
                //    }
                //    else
                //    {
                //        categoriesWithCount = categoriesWithCount.OrderByDescending(p => p.ItemCount);
                //    }
                //}

                //if (param.iDisplayLength != -1)
                //{
                //    categoriesWithCount = categoriesWithCount
                //                    .Skip(param.iDisplayStart)
                //                    .Take(param.iDisplayLength);
                //}
                //return categoriesWithCount;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : get cateogries :: " + ex.Message, CommonClasses.getCustomerID(), "GetCategories");
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
                    retVal = "Name";
                    break;
                case 1:
                    retVal = "CategoryType";
                    break;
                default:
                    retVal = "Name";
                    break;
            }

            return retVal;
        }



        public CategoryViewModel GetViewModel(CategoryModel model)
        {
            return new CategoryViewModel
            {
                CategoryTypeList = categoryTypeHelper.GetSelectList().OrderBy(x=>x.Text),
                CategoryModel = model
            };
        }

        // get
        public CategoryCreateModel GetCreateModel()
        {
            return new CategoryCreateModel { Color = "#ff0000" };
        }

        public CategoryUpdateModel GetEditModelOnError()
        {
            return new CategoryUpdateModel
                {
                    Id = -1,
                    ErrorMessage2 = "Record not found or deleted by another user.",
                    IsError = true,
                };
        }

        // get
        public CategoryUpdateModel GetEditModel(int id)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetEditModelOnError();
            }

            return new CategoryUpdateModel
               {
                   Id = entity.ID,
                   Name = entity.Name,
                   CategoryType_Id = entity.CategoryType_Id,
                   IsActive = entity.isActive ?? false,
                   Color = entity.Color,
               };
        }

        public CategoryDeleteModel GetDeleteModelOnError()
        {
            return new CategoryDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public CategoryDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new CategoryDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }

        // post
        public CategoryDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetDeleteModelOnError();
            }

            return new CategoryDeleteModel
            {
                Id = entity.ID,
                Name = entity.Name,
                Message = errorMessage,
                IsError = !string.IsNullOrWhiteSpace(errorMessage),
            };
        }

        public string Create(Category entity)
        {
            try
            {
                bool alreadyExist = false;
                alreadyExist = unitOfWork.CategoryRepository.Get(x => x.ClientID == clientId && x.Name == entity.Name && x.isDeleted==false && x.isActive==true).Count() > 0 ? true : false;
                if (alreadyExist)
                {
                    return "Category with this name already exists.";
                }
                else
                {
                    unitOfWork.CategoryRepository.Insert(entity);
                    unitOfWork.Save();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : Cateogry create :: " + ex.Message, CommonClasses.getCustomerID(), "Create");
                return ex.Message;
            }
        }

        public string Update(Category entity)
        {
            try
            {
                bool alreadyExist = false;
                alreadyExist = unitOfWork.CategoryRepository.Get(x => x.ClientID == clientId && x.Name == entity.Name && x.isDeleted == false && x.isActive == true && x.ID != entity.ID).Count() > 0 ? true : false;
                if (alreadyExist)
                {
                    return "Category with this name already exists.";
                }
                else
                {
                    unitOfWork.CategoryRepository.Update(entity);
                    unitOfWork.Save();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : Cateogry update :: " + ex.Message, CommonClasses.getCustomerID(), "Update");
                return ex.Message;
            }
        }

        public string SoftDelete(int id = 0)
        {
            try
            {
                int RelatedItems = unitOfWork.MenuRepository.Get(i => i.ClientID == clientId && i.Category_Id == id && i.isDeleted != true).Count();
                if (RelatedItems > 0)
                {
                    return "childRecordsExists";
                }
                else
                {
                    var entity = Get(id);

                    SoftDelete(entity);
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : soft delete :: " + ex.Message, CommonClasses.getCustomerID(), "SoftDelete");
                return "";
            }
        }

        public void SoftDelete(Category entity)
        {
            try
            {
                entity.isDeleted = true;

                Update(entity);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryController", "Error : soft delete :: " + ex.Message, CommonClasses.getCustomerID(), "SoftDelete");
            }
        }
    }
}
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

    public class CategoryTypeController : BaseAuthorizedController
    {

        private CategoryTypeHelper helper = new CategoryTypeHelper();

        public ActionResult Index()
        {
            if (!SecurityManager.viewCategoryTypes) return RedirectToAction("NoAccess", "Security", new { id = "nocategorytype" });
            if (Request.Cookies["categoryTypeview"] != null && Encryption.Decrypt(Request.Cookies["categoryTypeview"].Value).ToLower() == "table")
            {
                return RedirectToAction("Table");
            }

            return RedirectToAction("Tile");


        }

        public ActionResult Table()
        {
            Response.Cookies["categoryTypeview"].Value = Encryption.Encrypt("Table");
            return View();
        }

        //
        // GET: /District/
        public ActionResult Tile()
        {
            Response.Cookies["categoryTypeview"].Value = Encryption.Encrypt("Tile");

            var model = helper.GetIndexModel();

            return View(model);
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param, MenuIndexSearchModel model)
        {
            int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            string sortDirection = Request["sSortDir_0"]; // asc or desc
            return GetGridJson(param, model, sortColumnIndex, sortDirection);
        }

        private ActionResult GetGridJson(JQueryDataTableParamModel param, MenuIndexSearchModel model, int sortColumnIndex, string sortDirection)
        {

            try
            {
                int outItemsCount = 0;
                IEnumerable<CategoryTypeIndexModel> menuquery = helper.GetCategoryTypes(param, model, out outItemsCount, sortColumnIndex, sortDirection);
                IEnumerable<CategoryTypeIndexModel> menuItems = null;
                menuItems = menuquery;
                var displayedItems = menuItems;

                var result = displayedItems.Select(x => new string[] { x.Id.ToString(), x.Id.ToString(), x.Name, x.Categories.ToString(), Convert.ToString(x.Items), x.Id.ToString() });
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
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetGridJson");
                return null;
            }
        }

        //[ChildActionOnly]
        public ActionResult Popup()
        {
            var model = helper.GetCreateModel();

            if (Request.IsAjaxRequest())
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            return PartialView("Popup", model);
        }

        //[ChildActionOnly]
        public ActionResult Create()
        {
            var model = helper.GetCreateModel();

            return GetActionResult(Request, model);
        }

        [HttpPost]
        //[ChildActionOnly]
        public ActionResult Create(CategoryTypeCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = helper.Get();

                TryUpdateModel<CategoryType>(entity);

                //entity.ID = helper.GetNextId();

                try
                {
                    //helper.Create(entity);

                    var msg = helper.Create(entity);
                    if (!string.IsNullOrEmpty(msg))
                        helper.SetErrors(model, msg);
                }
                catch (Exception ex)
                {
                    helper.SetErrors(model, ViewData);
                    //Error logging in cloud tables 
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
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
        public ActionResult Edit(CategoryTypeUpdateModel model)
        {
            if (ModelState.IsValid)
            {
                var entity = helper.Get(model.Id);

                if (entity == null)
                {
                    return GetActionResult(Request, helper.GetEditModelOnError(), false);
                }

                TryUpdateModel<CategoryType>(entity);

                try
                {
                  //  helper.Update(entity);
                    var msg = helper.Update(entity);
                    if (!string.IsNullOrEmpty(msg))
                        helper.SetErrors(model, msg);
                }
                catch (Exception ex)
                {
                    helper.SetErrors(model, ViewData);
                    //Error logging in cloud tables 
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Edit");
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
                    long clientId = ClientInfoData.GetClientID();

                    string msg = helper.SoftDelete(clientId, id);
                    if (msg == "parentRecordsExists")
                    {
                        string message = "This Category type can’t be deleted, some items are referencing it.";
                        helper.SetErrors(model, message);
                    }
                    else
                    {
                        model.Message = "The Category type has been deleted successfully.";
                    }


                }
            }
            catch (Exception ex)
            {
                helper.SetErrors(model, ex.Message);
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteConfirm");
            }

            return GetActionResult(Request, model, false);
        }

        private ActionResult GetActionResult(HttpRequestBase request, CategoryTypeModel model, bool isGetAction = true)
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

        private ActionResult GetActionResult(HttpRequestBase request, CategoryTypeDeleteModel model, bool isGetAction = true)
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

    public class CategoryTypeHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        // for create/update
        public void SetErrors(CategoryTypeModel model, ViewDataDictionary viewData)
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

        public void SetErrors(CategoryTypeModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }


        // for delete
        public void SetErrors(CategoryTypeDeleteModel model, string errorMessage)
        {
            model.Message = errorMessage;
            model.IsError = true;
        }

        public void SetErrors(CategoryTypeDeleteModel model, ViewDataDictionary viewData)
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

        //public int GetNextId()
        //{
        //    return unitOfWork.CategoryTypeRepository.Get(x => x.ClientID == clientId).DefaultIfEmpty().Max(x => x == null ? 0 : x.ID) + 1;

        //}

        public IEnumerable<SelectListItem> GetSelectList()
        {
            return GetAll().Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name });
        }

        public IEnumerable<CategoryType> GetAll()
        {
            try
            {
                return unitOfWork.CategoryTypeRepository.Get(x => (x.isDeleted.Equals(null) || !x.isDeleted) && x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public CategoryType Get()
        {
            return new CategoryType { ClientID = clientId, isDeleted = false };
        }

        public CategoryType Get(long id)
        {
            return GetAll().Where(x => x.ID == id).FirstOrDefault();
        }

        public IEnumerable<CategoryTypeIndexModel> GetIndexModel()
        {

            IEnumerable<CategoryType> CategoryTypes = unitOfWork.CategoryTypeRepository.Get(x => x.ClientID == clientId);
            IEnumerable<Category> Category = unitOfWork.CategoryRepository.Get(x => x.ClientID == clientId && x.isActive == true && x.isDeleted == false);
            IEnumerable<Menu> Menu = unitOfWork.MenuRepository.Get(x => x.ClientID == clientId && x.isDeleted==false);

            var query = from ct in CategoryTypes
                        join c in Category
                        on ct.ID
                        equals (long?)c.CategoryType_Id into CategoryTypesGroup
                        where ct.isDeleted == false || ct.isDeleted == null
                        orderby ct.Name
                        select new CategoryTypeIndexModel
                        {
                            Id = ct.ID,
                            Name = ct.Name,
                            Categories = CategoryTypesGroup.Where(x => (x.isActive == true) && (x.isDeleted == null || x.isDeleted == false)).Count(),
                            Items = unitOfWork.generalRepository.getItemsCountofCategoryType(ct.ClientID, ct.ID)
                        };

            /* Old Query
            var query = from ct in CategoryTypes
                        join c in Category on ct.ID equals (long?)c.CategoryType_Id
                        where ct.isDeleted == false || ct.isDeleted == null
                        //join m in Menu on c.ID equals m.Category_Id 
                       // where ct.isDeleted == false && m.isDeleted==false 
                        select new CategoryTypeIndexModel
                        {
                            Id = ct.ID,
                            Name = ct.Name,
                            Categories = Category.Where(x=>x.CategoryType_Id==ct.ID).Count(),
                            Items = unitOfWork.generalRepository.getItemsCountofCategoryType(ct.ClientID, ct.ID)
                        };
             */ 
            return query;
            //return GetAll().OrderBy(o => o.Name)
            //    .Select(x =>
            //         new CategoryTypeIndexModel
            //         {
            //             Id = x.ID,
            //             Name = x.Name,
            //             Categories = x.Categories.Where(p => p.isDeleted == false).Count(),
            //             // needs to recheck the item count
            //             Items = x.Categories.Sum(o => o.Menus.Where(m => m.isDeleted == false).Count())
            //         });
        }



        public IEnumerable<CategoryTypeIndexModel> GetCategoryTypes(JQueryDataTableParamModel param, MenuIndexSearchModel model, out int itemsCount, int sortColumnIndex, string sortDirection)
        {
            try
            {
                IQueryable<CategoryType> categoryTypesQuery = unitOfWork.CategoryTypeRepository.GetQuery(x => (x.isDeleted.Equals(null) || x.isDeleted == false) && x.ClientID == clientId);
                itemsCount = categoryTypesQuery.Count();



                //IEnumerable<CategoryType> temp = categoryTypesQuery.AsEnumerable();

                IEnumerable<CategoryType> CategoryTypes = unitOfWork.CategoryTypeRepository.Get(x => x.ClientID == clientId);
                IEnumerable<Category> Category = unitOfWork.CategoryRepository.Get(x => x.ClientID == clientId);
                IEnumerable<Menu> Menu = unitOfWork.MenuRepository.Get(x => x.ClientID == clientId);


                var query = from ct in CategoryTypes
                            join c in Category
                            on ct.ID
                            equals (long?)c.CategoryType_Id into CategoryTypesGroup
                            where ct.isDeleted == false || ct.isDeleted == null
                            select new CategoryTypeIndexModel
                            {
                                Id = ct.ID,
                                Name = ct.Name,
                                Categories = CategoryTypesGroup.Where(x => (x.isActive == true) && (x.isDeleted == null || x.isDeleted == false)).Count(),
                                Items = unitOfWork.generalRepository.getItemsCountofCategoryType(ct.ClientID, ct.ID)
                            };




                if (sortDirection == "asc")
                {
                    query = query.OrderBy(p => p.Name);
                }
                else
                {
                    query = query.OrderByDescending(p => p.Name);
                }


                if (param.iDisplayLength != -1)
                {
                    query = query
                                    .Skip(param.iDisplayStart)
                                    .Take(param.iDisplayLength);
                }

                return query;

                //return temp.Select(x =>
                //         new CategoryTypeIndexModel
                //         {
                //             Id = x.ID,
                //             Name = x.Name,
                //             Categories = 5, //x.Categories.Where(o => o.isDeleted == false).Count(),
                //             Items = unitOfWork.generalRepository.getItemsCountofCategoryType(x.ClientID,x.ID) //  ;x.Categories.Sum(o => o.Menus.Where(m => m.isDeleted == false).Count())
                //         });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetCategoryTypes");
                itemsCount = 0;
                return null;
            }
        }

        // get
        public CategoryTypeCreateModel GetCreateModel()
        {
            return new CategoryTypeCreateModel { };
        }

        public CategoryTypeUpdateModel GetEditModelOnError()
        {
            return new CategoryTypeUpdateModel
            {
                Id = -1,
                ErrorMessage2 = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public CategoryTypeUpdateModel GetEditModel(int id)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetEditModelOnError();
            }

            return new CategoryTypeUpdateModel
            {
                Id = entity.ID,
                Name = entity.Name,
                CanFree = entity.canFree ?? false,
                CanReduce = entity.canReduce ?? false,
                IsMealPlan = entity.isMealPlan ?? false,
                IsMealEquiv = entity.isMealEquiv ?? false,

            };
        }

        public CategoryTypeDeleteModel GetDeleteModelOnError()
        {
            return new CategoryTypeDeleteModel
            {
                Message = "Record not found or deleted by another user.",
                IsError = true,
            };
        }

        // get
        public CategoryTypeDeleteModel GetDeleteModel(int id = 0, bool isGetAction = true)
        {
            if (id == 0)
            {
                if (isGetAction)
                {
                    return new CategoryTypeDeleteModel { };
                }

                return GetDeleteModelOnError();
            }

            return GetDeleteModel(id, null);
        }

        // post
        public CategoryTypeDeleteModel GetDeleteModel(int id, string errorMessage = null)
        {
            var entity = Get(id);

            if (entity == null)
            {
                return GetDeleteModelOnError();
            }

            return new CategoryTypeDeleteModel
            {
                Id = entity.ID,
                Name = entity.Name,
                Message = errorMessage,
                IsError = !string.IsNullOrWhiteSpace(errorMessage),
            };
        }

        public string Create(CategoryType entity)
        {
            try
            {
                              
               // unitOfWork.CategoryTypeRepository.Insert(entity);
              //  unitOfWork.Save();


                bool alreadyExist = false;
                alreadyExist = unitOfWork.CategoryTypeRepository.Get(x => x.ClientID == clientId && x.Name == entity.Name && x.isDeleted == false ).Count() > 0 ? true : false;
                if (alreadyExist)
                {
                    return "Category Type with this name already exists.";
                }
                else
                {
                     unitOfWork.CategoryTypeRepository.Insert(entity);
                     unitOfWork.Save();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
                return ex.Message;
            }
        }

        public string Update(CategoryType entity)
        {
            try
            {
                //unitOfWork.CategoryTypeRepository.Update(entity);
               // unitOfWork.Save();

                bool alreadyExist = false;
                alreadyExist = unitOfWork.CategoryTypeRepository.Get(x => x.ClientID == clientId && x.Name == entity.Name && x.isDeleted == false && x.ID != entity.ID).Count() > 0 ? true : false;
                if (alreadyExist)
                {
                    return "Category Type with this name already exists.";
                }
                else
                {
                    unitOfWork.CategoryTypeRepository.Update(entity);
                    unitOfWork.Save();
                    return "";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Update");
                return ex.Message;
            }
        }

        public string SoftDelete(long ClientID, int id = 0)
        {
            int relatedCategories = unitOfWork.CategoryRepository.Get(c => c.CategoryType_Id == id && c.isDeleted != true && c.ClientID == ClientID).Count();
            if (relatedCategories > 0)
            {
                return "parentRecordsExists";
            }
            else
            {
                var entity = Get(id);
                SoftDelete(entity);
                return "okay";
            }
        }

        public void SoftDelete(CategoryType entity)
        {
            try
            {
                entity.isDeleted = true;

                Update(entity);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CategoryTypeController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SoftDelete");
            }
        }

    }
}

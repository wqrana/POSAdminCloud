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
    public class PreorderDashboardController : BaseAuthorizedController
    {
        private PreorderDashboardHelper preorderDashboardHelper;

        public PreorderDashboardController()
        {
            preorderDashboardHelper = new PreorderDashboardHelper();

        }
    public ActionResult index(){
        PreorderDashboardModel model = null;
        try
        {

            model = preorderDashboardHelper.GetPreorderDashboardModel();

        }
        catch (Exception ex)
        {

            ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderDashboardController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxPickupItemsHandler");
            return null;
        }

        return View(model);

    }

    public JsonResult LoadTopSellingItemData()
    {
        dynamic ItemData = null;
        string[] emptyStringArray = new string[0];

        int PeriodTypeID = int.Parse(Request["PeriodTypeID"]);

        try
        {
            ItemData = preorderDashboardHelper.GetTopSellingItemOverview(PeriodTypeID);

        }
        catch (Exception ex)
        {
            //Error logging in cloud tables 
            ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "PreorderDashboardController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Ajax Call to load top selling itmes");
            return null;
        }

        JsonResult jsonResult = new JsonResult()
        {
            Data = ItemData,
            JsonRequestBehavior = JsonRequestBehavior.AllowGet

        };

        return jsonResult;
    }

    
   }

    public class PreorderDashboardHelper
    {

        private UnitOfWork unitOfWork = null;

        public PreorderDashboardHelper()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        }

        public PreorderDashboardModel GetPreorderDashboardModel(){

            PreorderDashboardModel model = new PreorderDashboardModel();

            model.PeriodTypeID = 1;
            model.PeriodTypeList = new List<PeriodType>() {

                new PeriodType(){id= 0, name="This Week"},
                new PeriodType(){id= 1, name="This Month"},
                new PeriodType(){id= 2, name="This Year"},
                 new PeriodType(){id= 3, name="Fiscal Year"}
                
                
            };
            //Current Preorder overview
            model.CurrentPreorderStatList   = unitOfWork.customPreOrderPickupRespository.GetCurrentPreorderOverviewList();
            //Avg Incoming preorders
            model.AvgInPreorderStatList = unitOfWork.customPreOrderPickupRespository.GetAvgInPreorderOverviewList();
            //Top Selling items
            model.TopSellingItemStatList = unitOfWork.customPreOrderPickupRespository.GetTopSellingItemOverviewList(model.PeriodTypeID);

            return model;

        }

      

        public IEnumerable<TopSellingItem> GetTopSellingItemOverview(int peroidTypeID)
        {

            return this.unitOfWork.customPreOrderPickupRespository.GetTopSellingItemOverviewList(peroidTypeID);
        }
    }
   
}

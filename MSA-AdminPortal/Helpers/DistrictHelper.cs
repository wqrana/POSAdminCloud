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


namespace MSA_AdminPortal.Helpers
{
    public class DistrictHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        public District Get(int id)
        {
            try
            {
                return GetAll().Where(x => x.ID == id && x.ClientID == clientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public IEnumerable<District> GetAll()
        {
            try
            {
                return unitOfWork.DistrictRepository.Get(x => x.ClientID == clientId && (x.isDeleted.Equals(null) || !x.isDeleted));
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public IEnumerable<SelectListItem> GetSelectList(long id = 0)
        {
            try
            {
                return GetAll().Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.DistrictName, Selected = (x.ID == id) });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DistrictHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSelectList");
                return null;
            }
        }
    }
}
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
    public class CustomerHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        public Customer Get(int id)
        {
            try
            {
                return GetAll().Where(x => x.ID == id && x.ClientID == clientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            try
            {
                var query = (from t1 in unitOfWork.CustomerRepository.Get(x => x.ClientID == clientId)
                             join t2 in unitOfWork.DistrictRepository.Get(x => x.ClientID == clientId)
                             on t1.District_Id equals t2.ID
                             where t2.isDeleted != true
                             select t1);

                return query;
                //return unitOfWork.CustomerRepository.Get(x => x.ClientID == clientId).Where(c=>c.District_Id = );

                //return unitOfWork.CustomerRepository.Get(x => x.ClientID == clientId && x.d.District.isDeleted != true);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public string GetSingleCustomer( long customerID)
        {
            string customerName = "";
            if (customerID != 0)
            {
                var onecustomer = unitOfWork.CustomerRepository.GetQuery(x => x.ID == customerID && x.ClientID == clientId).FirstOrDefault();
                if (onecustomer != null)
                {
                    customerName = onecustomer.LastName + " " + onecustomer.FirstName;
                }
            }
           
           return customerName;
        
        }

        public IEnumerable<SelectListItem> GetSelectListForDistrict(long dist, long id = 0)
        {
            try
            {
                return GetAll().Where(x => x.District_Id == dist).Select(x => new SelectListItem { Value = x.ID.ToString(), Text = (x.FirstName + " " + x.LastName), Selected = (x.ID == id) });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSelectListForDistrict");
                return null;
            }
        }

        public IEnumerable<SelectListItem> GetSelectListForAdults(int id = 0)
        {
            try
            {
                return  GetAll().Select(x => new SelectListItem { Value = x.ID.ToString(), Text = (x.FirstName + " " + x.LastName), Selected = (x.ID == id) });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "CustomerHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSelectListForAdults");
                return null;
            }
        }
        public static Int32 getSearchBy_Id()
        {
            Int32 retStr = 0;

            CustomerFilters filters = null;
            if (HttpContext.Current.Session["CustomerFilters"] !=null)
            {
            filters = (CustomerFilters)HttpContext.Current.Session["CustomerFilters"] ;
            }
            if (filters != null)
            {
                if (filters.SearchBy_Id.HasValue)
                {
                    retStr = Convert.ToInt32(filters.SearchBy_Id);
                }
            }

            return retStr;
        
        }

        public static string getSearchByStr()
        {
            string retStr = "";

            CustomerFilters filters = null;
            if (HttpContext.Current.Session["CustomerFilters"] != null)
            {
                filters = (CustomerFilters)HttpContext.Current.Session["CustomerFilters"];
            }
            if (filters != null)
            {
                    retStr = filters.SearchBy;
            }

            return retStr;

        }
    }
}
using AdminPortalModels.Models;
using Repository;
using Repository.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MSA_AdminPortal.Helpers
{
    public class ActivityHelper
    {



        /// <summary>
        /// defaul constructor
        /// </summary>
        public ActivityHelper()
        {

        }
        /// <summary>
        /// Get All Orders by Student
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="customerID"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<DetailOrdersModel> getAllOrders(long clientID, int customerID, DateTime startDate, DateTime endDate, out int status)
        {
            try
            {
                UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionStringByClientID(clientID));
                List<DetailOrdersModel> activityOrdersList = unitOfWork.orderRepository.GetOrdersDetailList(clientID, customerID, startDate, endDate, out status);
                if (activityOrdersList == null)
                {
                    status = 0;
                    return null;
                }
                //foreach (var item in activityOrdersList)
                //{
                //    if (item.OrderDate != null)
                //    {
                //        item.OrderDate = TimeZoneHelper.ConvertDateTimeToClientTime(Convert.ToDateTime(item.OrderDate), (long)clientID);
                //    }
                //}
                return activityOrdersList;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "getAllOrders", "Error : " + ex.Message, customerID.ToString(), "ActivityHelper");
                status = 0;
                return null;
            }
        }


        /// <summary>
        /// Compare Order Activity Date
        /// </summary>
        /// <param name="OrderDate"></param>
        /// <param name="StudentID"></param>
        /// <returns></returns>
        //public bool compareOrderActivityDate(DateTime OrderDate, int CustomerID, out int status)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "compareOrderActivityDate", "Error : " + ex.Message, customerID.ToString(), "ActivityHelper");
        //        status = 0;
        //        return null;           
        //    }
        //}
    }
}
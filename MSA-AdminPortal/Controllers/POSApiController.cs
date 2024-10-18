using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using MSA_AdminPortal.Helpers;
using MSA_AdminPortal.App_Code;
using Repository;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace MSA_AdminPortal.Controllers
{
    public class POSApiController : ApiController
    {
        public POSApiController()
        {

        }

        /// <summary>
        /// Method to get Order Detail by student
        /// </summary>
        /// <param name="OrderParam"></param>
        /// <returns></returns>
        public List<DetailOrdersModel> getAllOrdersByStudent([FromUri] ApiOrderParam OrderParam)
        {
            ActivityHelper activityHelper = new ActivityHelper();
            int status;
            var detailOrdersList = activityHelper.getAllOrders(OrderParam.clientID, OrderParam.customerID, OrderParam.startDate, OrderParam.endDate, out status);
            if (status == 0)
            {
                throw NoContent("Orders not found");
            }
            else if (status == -1)
            {
                throw ExpectationFailed("Error in processing API request ");
            }
            else
            {
                return detailOrdersList;
            }

        }

        /// <summary>
        /// compare Order Activity Date
        /// </summary>
        /// <param name="OrderDate"></param>
        /// <param name="CustomerID"></param>
        /// <returns></returns>
        //public bool compareOrderActivityDate(DateTime OrderDate, int CustomerID)
        //{
            
        //}

        #region Response Messsage Handling

        /// <summary>
        /// creates an <see cref="HttpResponseException"/> with a response code of 400
        /// and places the reason in the reason header and the body.
        /// </summary>
        /// <param name="reason">Explanation text for the client.</param>
        /// <returns>A new HttpResponseException</returns>
        protected new HttpResponseException BadRequest(string reason)
        {
            return CreateHttpResponseException(reason, HttpStatusCode.BadRequest);
        }

        /// <summary>
        /// creates an <see cref="HttpResponseException"/> with a response code of 404
        /// and places the reason in the reason header and the body.
        /// </summary>
        /// <param name="reason">Explanation text for the client.</param>
        /// <returns>A new HttpResponseException</returns>
        protected HttpResponseException NoContent(string reason)
        {
            return CreateHttpResponseException(reason, HttpStatusCode.NoContent);
        }

        /// <summary>
        /// creates an <see cref="HttpResponseException"/> with a response code of 404
        /// and places the reason in the reason header and the body.
        /// </summary>
        /// <param name="reason">Explanation text for the client.</param>
        /// <returns>A new HttpResponseException</returns>
        protected HttpResponseException NotFound(string reason)
        {
            return CreateHttpResponseException(reason, HttpStatusCode.NotFound);
        }

        /// <summary>
        /// creates an <see cref="HttpResponseException"/> with a response code of 404
        /// and places the reason in the reason header and the body.
        /// </summary>
        /// <param name="reason">Explanation text for the client.</param>
        /// <returns>A new HttpResponseException</returns>
        protected HttpResponseException ExpectationFailed(string reason)
        {
            return CreateHttpResponseException(reason, HttpStatusCode.ExpectationFailed);
        }

        /// <summary>
        /// Creates an <see cref="HttpResponseException"/> to be thrown by the api.
        /// </summary>
        /// <param name="reason">Explanation text, also added to the body.</param>
        /// <param name="code">The HTTP status code.</param>
        /// <returns>A new <see cref="HttpResponseException"/></returns>
        private static HttpResponseException CreateHttpResponseException(string reason, HttpStatusCode code)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = code,
                ReasonPhrase = reason,
                Content = new StringContent(reason)
            };
            throw new HttpResponseException(response);
        }

        #endregion


    }


}

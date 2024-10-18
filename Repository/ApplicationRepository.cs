using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using Repository.edmx;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Repository
{
    public class ApplicationRepository : GenericRepository<Applications>, IApplicationRepository, IDisposable
    {
        private PortalContext context;
        public ApplicationRepository(PortalContext context)
            : base(context)
        {
            this.context = context;
        }

        public IEnumerable<Application> GetApplicationsList(Nullable<long> clientID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortColumn, string sortDirection, ApplicationFilters filters, out int totalrecords)
        {

            long ClientID = clientID.HasValue ? clientID.Value : 0;
            totalrecords = 0;

            /*– Pagination Parameters */
            int PageNo = 1;
            int PageSize = iDisplayLength;

            /*– Sorting Parameters */
            string SortColumn = "";
            string SortOrder = "";

            try
            {
                //SortColumn = getColmnName(sortColumnIndex);
                SortColumn = sortColumn;

                SortOrder = sortDirection == "asc" ? "ASC" : "DESC";
                PageNo = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(iDisplayStart) / Convert.ToDouble(iDisplayLength)) + 1);

                IEnumerable<Admin_Applications_SortedList_Result> dataSet = this.context.Admin_Applications_SortedList(ClientID, filters.SearchBy, filters.SearchBy_Id, filters.SignedDate, filters.ApprovalStatus, filters.Entered, filters.Updated, PageNo, PageSize, SortColumn, SortOrder);
                var query = dataSet.ToList();

                IEnumerable<Application> sortedApplications = query.Select(a => new Application
               {
                   Application_Id = a.Application_Id,
                   Parent_Name = a.Parent_Name,
                   Parent_Id = a.Parent_Id,
                   Student_Name = a.Student_Name,
                   Student_Id = a.Student_Id,
                   Member_Name = a.Member_Name,
                   Member_Id = a.Member_Id,
                   District_Name = a.District_Name,
                   Household_Size = a.Household_Size,
                   No_Of_Students = a.No_Of_Students,
                   No_Of_Members = a.No_Of_Members,
                   App_Signer_Name = a.App_Signer_Name,
                   Approval_Status = a.Approval_Status,
                   Entered = a.Entered,
                   Updated = a.Updated
               }).ToList();

                var obj = query.FirstOrDefault();
                if (obj != null)
                totalrecords = obj.AllRecordsCount.HasValue ? obj.AllRecordsCount.Value : 0;

                return sortedApplications;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetApplicationsList");
                totalrecords = 0;
                return null;
            }
        }

        public IEnumerable<AppStudent> GetAppStudentsList(long clientId, int appId)
        {
            try
            {
                GenericRepository<edmx.Applications> repo = new GenericRepository<Applications>(context);
                return null;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAppStudentsList");
                return null;
            }
        }

        /// <summary>
        /// This function disposes all the memory occupied by this object
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}


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
    public class BeginningBalanceRepository : IBeginningBalanceRepository, IDisposable
    {
        private PortalContext context;

        public BeginningBalanceRepository(PortalContext context)
        {
            this.context = context;
        }

        public IEnumerable<BeginningBalanceViewModel> GetFilteredSortedBeginningBalance(long clientId, string searchString, string schoolFilter, string gradeFilter, string homeRoomFilter, string districtFilter, string sortBy, string sortDirection)
        {
            try
            {
                return this.context.Admin_BeginningBalance_Get(clientId, searchString, schoolFilter, gradeFilter,homeRoomFilter,districtFilter, sortBy, sortDirection).Select(c => new BeginningBalanceViewModel
                {
                    Id = c.Customer_Id,
                    UserId = c.UserID,
                    CustomerName = c.CustomerName,
                    MealPlan= c.MealPlan,
                    AlaCartePlan = c.Alacarte,
                    Balance= c.Balance,
                    Grade = c.Grade,
                    PrevMealPlanBalance=c.PrevMealPlan,
                    PrevAlaCartePlanBalance=c.PrevAlaCarte
                }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "BeginningBalanceRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetFilteredSortedBeginningBalance");
                return null;
            }
        }

        public IEnumerable<GraduateSeniorsDistinctHomeRoomViewModel> FetchDistinctHomeRoomForBeginningBalance(long ClientId, long SchoolId, long GradeId, long districtId)
        {
            try
            {
                var list=this.context.FetchDistinctHomeRoomForBeginningBalance(ClientId, SchoolId,GradeId,districtId).Select(c => new GraduateSeniorsDistinctHomeRoomViewModel
                {
                     HomeRoomId = c.HomeRoomId,
                     HomeRoomName = c.NAME
                }).ToList();

                if (list != null && list.Count > 0)
                    list.Insert(0, new GraduateSeniorsDistinctHomeRoomViewModel { HomeRoomId = 0, HomeRoomName = "Not Assigned" });

                return list;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "BeginningBalanceRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "FetchDistinctHomeRoomForBeginningBalance");
                return null;
            }
        }

        #region Dispose
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
        #endregion
    }
}

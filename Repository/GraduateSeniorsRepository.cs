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
    public class GraduateSeniorsRepository : IGraduateSeniorsRepository, IDisposable
    {
        private PortalContext context;


        public GraduateSeniorsRepository(PortalContext context)
        {
            this.context = context;

        }

        public IEnumerable<GraduateSeniorsViewModel> GetFilteredSortedGraduateSeniors(long clientId, string searchString, string schoolFilter, string gradeFilter, string districtFilter, string sortBy, string sortDirection)
        {
            try
            {
                return this.context.FetchGraduateSeniors(clientId, searchString, schoolFilter, gradeFilter, districtFilter, sortBy, sortDirection).Select(c => new GraduateSeniorsViewModel
                {
                    Id = c.id,
                    UserId = c.UserId,
                    CustomerName = c.CustomerName,
                    Graduate = c.Graduate,
                    MBalance = c.MBalance,
                    ABalance = c.ABalance,
                    TotalBalance = c.TotalBalance,
                    Grade = c.Grade,
                    SchoolName = c.SchoolName
                }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GraduateSeniorsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetFilteredSortedGraduateSeniors");
                return null;
            }
        }

        public IEnumerable<GraduateSeniorsDistinctGradesViewModel> FetchDistinctGradesForGraduateSeniors(long ClientId, long SchoolId, long districtId)
        {
            try
            {
                return this.context.FetchDistinctGradesForGraduateSeniors(ClientId, SchoolId, districtId).Select(c => new GraduateSeniorsDistinctGradesViewModel
                {
                    GradeId = c.GradeID,
                    GradeName = c.NAME
                }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GraduateSeniorsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "FetchDistinctGradesForGraduateSeniors");
                return null;
            }
        }

        public IEnumerable<GraduateSeniorsDistinctSchoolsViewModel> FetchDistinctSchoolsForGraduateSeniors(long ClientId, long districtId)
        {
            try
            {
                return this.context.FetchDistinctSchoolsForGraduateSeniors2(ClientId, districtId).Select(c => new GraduateSeniorsDistinctSchoolsViewModel
                {
                    SchoolId = c.SchoolID,
                    SchoolName = c.SchoolName
                }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GraduateSeniorsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "FetchDistinctGradesForGraduateSeniors");
                return null;
            }
        }

        public bool GraduateSeniors(long ClientId, int custId)
        {
            try
            {
                this.context.GRADUATESENIORS(ClientId, custId, 0, 0, 0, 1, true, null);
                return true;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "GraduateSeniors", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetFilteredSortedGraduateSeniors");
                return false;
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

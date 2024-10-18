using Repository.edmx;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.Models;

namespace Repository
{
    public class SchoolRepository : ISchoolRepository, IDisposable
    {
        private PortalContext context;

        public SchoolRepository(PortalContext context)
        {
            this.context = context;
        }

        public IEnumerable<School> GetSchools()
        {
            try
            {
                return context.Schools.ToList();
            }
            catch (Exception ex)
            { 
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchools");
                return null;
            }
        }

        public string DeleteSchool(int SchoolID, long ClientId)
        {
            string retValue = string.Empty;
            //call db function to deleet
            try
            {
                Admin_School_Delete_Result stuff = (Admin_School_Delete_Result)this.context.Admin_School_Delete(ClientId, SchoolID, false, false, false, false).Single();
                if (stuff.School_Id == 0)
                {
                    retValue = stuff.ErrorMessage;
                }
                else
                {
                    retValue = "-1";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteSchool");
             
            }
            return retValue;
        }

        public School GetSchoolById(long ClientID, int id)
        {
            try
            {
                return context.Schools.Find(ClientID, id);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchoolById");
                return null;                
            }
        }

        public string SaveSchool(SchoolUpdateModel schoolUpdateModel)
        {
            string retStr = "";
            try
            {

                var result = context.Admin_School_Save
                    (schoolUpdateModel.ClientID, Convert.ToInt32(schoolUpdateModel.Id), Convert.ToInt32(schoolUpdateModel.District_Id), schoolUpdateModel.SchoolID,
                    schoolUpdateModel.SchoolName, DateTime.Now, schoolUpdateModel.Emp_Director_Id, schoolUpdateModel.Emp_Administrator_Id, schoolUpdateModel.Address1,
                    schoolUpdateModel.Address2, schoolUpdateModel.City, schoolUpdateModel.State, schoolUpdateModel.Zip, schoolUpdateModel.Phone1,
                    schoolUpdateModel.Phone2, schoolUpdateModel.Comment, schoolUpdateModel.isSevereNeed, schoolUpdateModel.isDeleted,
                    schoolUpdateModel.UseDistDirAdmin, schoolUpdateModel.AlaCarteLimit, schoolUpdateModel.MealPlanLimit, schoolUpdateModel.DoPinPreFix,
                    schoolUpdateModel.PinPreFix, schoolUpdateModel.PhotoLogging, false, schoolUpdateModel.BarCodeLength,
                    schoolUpdateModel.SchoolYearStartDate, schoolUpdateModel.SchoolYearEndDate, schoolUpdateModel.StripZeros);

                var SingleResult = result.FirstOrDefault();
                string schID = SingleResult.School_Id.ToString();
                // Change by farrukh on 8-april-2016
                retStr = schID == "0" ? "-1" : schID.ToString();

            }
            catch (Exception ex)
            {
                
                throw;
            }

            return retStr;
        
        }

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
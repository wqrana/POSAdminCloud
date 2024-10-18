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
    public class SchoolOptionHelper
    {
        private long clientId = ClientInfoData.GetClientID();
        private UnitOfWork unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());

        public SchoolOption Get(int id)
        {
            try
            {
                return GetAll().Where(x => x.School_Id == id && x.ClientID == clientId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolOptionHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Get");
                return null;
            }
        }

        public SchoolOption Get()
        {
            return new SchoolOption { ClientID = clientId };
        }

        public IEnumerable<SchoolOption> GetAll()
        {
            try
            {
                return unitOfWork.SchoolOptionRepository.Get(x => x.ClientID == clientId);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolOptionHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAll");
                return null;
            }
        }

        public void Create(SchoolOption entity)
        {
            try
            {
                //entity.School = unitOfWork.SchoolRepository.Get(x => x.ID == entity.School_Id).FirstOrDefault();
                unitOfWork.SchoolOptionRepository.Insert(entity);
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SchoolOptionHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Create");
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.edmx;
using AdminPortalModels.ViewModels;
using Repository.Helpers;
using System.Data.Entity.Core.Objects;
using System.Data.SqlTypes;

namespace Repository
{
    public class SettingsRepository : ISettingsRepository, IDisposable
    {
        private PortalContext context;

        public SettingsRepository(PortalContext context)
        {
            this.context = context;
        }
        public IEnumerable<School> GetSchools(long clientid)
        {
            try
            {
                return context.Schools.Where(s => s.ClientID == clientid);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchools");
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Schoolid"></param>
        /// <returns></returns>
        public School GetSchollByID(int Schoolid)
        {
            try
            {
                return context.Schools.Find(Schoolid);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchollByID");
                return null;
            }
        }

        public IEnumerable<POSVM> GetPOSbySchoolID(long Schoolid, long ClientID)
        {
            try
            {
                return this.context.Admin_POS_List(ClientID, Schoolid).Select(p1 => new POSVM { Name = p1.POS_Name, ClientID = ClientID, Id = p1.POS_Id, SessionStatus = p1.POS_Open_Session, EnableCCProcessing = p1.Credit_Card_Enabled, VeriFoneUserId = p1.VeriFoneUserId, VeriFonePassword = "temppass", SchoolName = p1.School_Name }).AsEnumerable<POSVM>();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetPOSbySchoolID");
                return null;
            }
        }

        public string SessionStatus(int posID, long ClientID)
        {
            try
            {
                return this.context.Admin_POS_List(ClientID, null).Where(p => p.POS_Id == posID).FirstOrDefault().POS_Open_Session;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SessionStatus");
                return "";
            }
        }

        public IEnumerable<POSVM> GetPOSListPage(Nullable<long> clientID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, out int totalrecords)
        {
            try
            {
                string columnName = getColmnName(sortColumnIndex);

                IQueryable<Admin_POS_List_Result> query = this.context.Admin_POS_List(clientID, null);
                IEnumerable<Admin_POS_List_Result> posResult = null;

                totalrecords = query.Count();
                if (sortDirection == "asc")
                {
                    posResult = query.OrderBy(columnName).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_POS_List_Result>;
                }
                else
                {
                    posResult = query.OrderByDescending(columnName).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_POS_List_Result>;
                }

                return posResult.Select(c => new POSVM
                {
                    Id = c.POS_Id,
                    Name = c.POS_Name,
                    SchoolName = c.School_Name,
                    EnableCCProcessing = c.Credit_Card_Enabled,
                    SessionStatus = c.POS_Open_Session,
                    VeriFonePassword = c.VeriFonePassword,
                    VeriFoneUserId = c.VeriFoneUserId,
                });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetPOSListPage");
                totalrecords = 0;
                return null;
            }

        }

        public IQueryable<POSVM> GetPOSList(Nullable<long> clientID, out int totalrecords)
        {
            try
            {
                IQueryable<Admin_POS_List_Result> query = this.context.Admin_POS_List(clientID, null);
                totalrecords = query.Count();
                return query.Select(c => new POSVM
                {
                    ClientID = c.ClientID,
                    Id = c.POS_Id,
                    Name = c.POS_Name,
                    School_Id = c.SchoolID.HasValue ? c.SchoolID.Value : 0,
                    SchoolName = c.School_Name,
                    EnableCCProcessing = c.Credit_Card_Enabled,
                    SessionStatus = c.POS_Open_Session,
                    VeriFonePassword = c.VeriFonePassword,
                    VeriFoneUserId = c.VeriFoneUserId,
                });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetPOSList");
                totalrecords = 0;
                return null;
            }
        }

        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 1:
                    retVal = "POS_Name";
                    break;
                case 2:
                    retVal = "School_Name";
                    break;
                case 3:
                    retVal = "POS_Open_Session";
                    break;
                case 4:
                    retVal = "VeriFoneUserId";
                    break;
                default:
                    retVal = "POS_Name";
                    break;
            }

            return retVal;
        }

        public int GetAllPOSCount(long ClientID)
        {
            try
            {
                return context.POS.Where(p => p.ClientID == ClientID).Count();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAllPOSCount");
                return 0;
            }
        }

        public IEnumerable<POS> GetFullPOSbyID(int ClientID, int posID)
        {
            try
            {
                return context.POS.Where(p => p.ClientID == ClientID && p.ID == posID);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetFullPOSbyID");
                return null;
            }
        }

        public void Savepos(POS pos)
        {
            try
            {
                context.Entry<POS>(pos).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Savepos");
            }

        }

        ////districts methods
        public DistrictVM getDistrict(long clientid, long districtId)
        {
            try
            {
                if (districtId != 0)
                {
                    return this.context.Districts.Where(d => d.ClientID == clientid && d.ID == districtId).Select(
                        dvm => new DistrictVM
                        {
                            ClientID = dvm.ClientID,
                            Id = dvm.ID,
                            DistrictName = dvm.DistrictName,
                            Address1 = dvm.Address1,
                            Address2 = dvm.Address2,
                            City = dvm.City,
                            State = dvm.State,
                            Zip = dvm.Zip,
                            Phone1 = dvm.Phone1,
                            Phone2 = dvm.Phone2,
                            BankRoute = dvm.BankRoute,
                            BankAccount = dvm.BankAccount,
                            BankName = dvm.BankName,
                            BankAddr1 = dvm.BankAddr1,
                            BankAddr2 = dvm.BankAddr2,
                            BankCity = dvm.BankCity,
                            BankState = dvm.BankState,
                            BankZip = dvm.BankZip,
                            empAdmin = dvm.Emp_Administrator_Id,
                            empDirector = dvm.Emp_Director_Id
                        }).FirstOrDefault();
                }
                else
                {
                    return new DistrictVM(clientid);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistrict");
                return new DistrictVM(clientid);
            }
        }

        public long GetFirstDistrictID(long clientid)
        {
            try
            {
                return this.context.Districts.Where(dis => dis.ClientID == clientid).FirstOrDefault().ID;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetFirstDistrictID");
                return 0;
            }
        }

        public long GetFirstSchoolID(long clientid)
        {
            return this.context.Schools.Where(sch => sch.ClientID == clientid).FirstOrDefault().ID;
        }

        public DistrictOptionsVM getDistrictOptions(long clientid, long districtId)
        {
            var districtOptionsVM =  new DistrictOptionsVM();
            try
            {
                if (districtId != 0)
                {
                   districtOptionsVM =  this.context.DistrictOptions.Where(d => d.ClientID == clientid && d.District_Id == districtId).Select(
                        dvm => new DistrictOptionsVM
                        {
                            isStudentPaidTaxable = dvm.isStudentPaidTaxable,
                            isStudentRedTaxable = dvm.isStudentRedTaxable,
                            isStudentFreeTaxable = dvm.isStudentFreeTaxable,
                            isEmployeeTaxable = dvm.isEmployeeTaxable,
                            isMealPlanTaxable = dvm.isMealPlanTaxable,
                            isGuestTaxable = dvm.isGuestTaxable,
                            isStudCashTaxable = dvm.isStudCashTaxable,
                            StartSchoolYear = dvm.StartSchoolYear,
                            EndSchoolYear = dvm.EndSchoolYear
                        }).FirstOrDefault();
                }

                if (districtOptionsVM != null)
                {
                    return districtOptionsVM;
                }
                else
                {
                    return new DistrictOptionsVM();
                }
               
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistrictOptions");
                return new DistrictOptionsVM();
            }
        }
        public IList<DistrictNames> getDistrictNames(long clientid)
        {
            try
            {
                return this.context.Districts.Where(d => d.ClientID == clientid).Select(
                     dis => new DistrictNames
                    {
                        districtId = dis.ID,
                        districtName = dis.DistrictName
                    }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistrictNames");
                return null;
            }
        }
        public DistrictsData getDistrictsData(long clientid, long districtId)
        {
            try
            {
                return new DistrictsData
                   {
                       DistrictVM = getDistrict(clientid, districtId),
                       DistrictOptionsVM = getDistrictOptions(clientid, districtId),
                       DistrictList = getDistrictNames(clientid)
                   };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistrictsData");
                return null;
            }
        }


        public string SaveDistrictData(string districtData, out Int32 districtId)
        {
            string[] fildsToUpdate = districtData.Split('*');
            int retValue = 1;
            int districtsId = 0;
            if (fildsToUpdate.Length == 29)
            {
                long clientid = Convert.ToInt32(fildsToUpdate[0]);
                districtsId = Convert.ToInt32(fildsToUpdate[1]);
                string distName = fildsToUpdate[2];
                string addr1 = fildsToUpdate[3];
                string addr2 = fildsToUpdate[4];
                string cityname = fildsToUpdate[5];
                string statename = fildsToUpdate[6];
                string zip = fildsToUpdate[7];
                string PhoneNo1 = fildsToUpdate[8];
                string PhoneNo2 = fildsToUpdate[9];

                bool isStudentPaidTaxable = Convert.ToBoolean(fildsToUpdate[10]);
                bool isStudentRedTaxable = Convert.ToBoolean(fildsToUpdate[11]);
                bool isStudentFreeTaxable = Convert.ToBoolean(fildsToUpdate[12]);
                bool isEmployeeTaxable = Convert.ToBoolean(fildsToUpdate[13]);
                bool isMealPlanTaxable = Convert.ToBoolean(fildsToUpdate[14]);
                bool isGuestTaxable = Convert.ToBoolean(fildsToUpdate[15]);
                bool isStudCashTaxable = Convert.ToBoolean(fildsToUpdate[16]);

                DateTime? StartSchoolYear = checkDateTime(fildsToUpdate[17]);
                DateTime? EndSchoolYear = checkDateTime(fildsToUpdate[18]);

                string BankRoute = fildsToUpdate[19];
                string BankAccount = fildsToUpdate[20];
                string BankName = fildsToUpdate[21];
                string BankAddr1 = fildsToUpdate[22];
                string BankAddr2 = fildsToUpdate[23];
                string BankCity = fildsToUpdate[24];
                string dllBankState = fildsToUpdate[25];
                string BankZip = fildsToUpdate[26];
                int? empAdminID = null;
                int? empDirectorID = null;

                if (!string.IsNullOrEmpty(fildsToUpdate[27]))
                {
                    empAdminID = Convert.ToInt32(fildsToUpdate[27]);
                }

                if (!string.IsNullOrEmpty(fildsToUpdate[28]))
                {
                    empDirectorID = Convert.ToInt32(fildsToUpdate[28]);
                }

                var newDistrictId = new ObjectParameter("newDistrictId", typeof(int));

                string timeZoneOffset = string.Empty;
                if (System.Web.HttpContext.Current.Session["UserTimeZone"] != null)
                {
                    timeZoneOffset = System.Web.HttpContext.Current.Session["UserTimeZone"].ToString();
                }
                else if (System.Web.HttpContext.Current.Response.Cookies["ClientTimeZone"].Value != null)
                {
                    timeZoneOffset = System.Web.HttpContext.Current.Response.Cookies["ClientTimeZone"].Value.ToString();
                }

                DateTime? ChangedDate = DateTime.Now;
                //if (!string.IsNullOrEmpty(timeZoneOffset))
                //{
                //    ChangedDate = DateTimeZoneHelper.GetUserCurrentTimeWithOffSetValue(timeZoneOffset);
                //}

                if (districtsId == 0)
                {
                    districtsId = -1;
                }
                try
                {
                    if (!isDistrictAlreadyExists(clientid, distName, districtsId))
                    {
                        if (!isDeletedDistrict(clientid, distName, districtsId))
                        {
                            var adminDistrictSave = this.context.Admin_District_Save(clientid, districtsId, distName, isEmployeeTaxable, isStudentFreeTaxable, isStudentPaidTaxable,
                                isStudentRedTaxable, isMealPlanTaxable, isGuestTaxable, isStudCashTaxable, null, empAdminID, empDirectorID, addr1, addr2, cityname, statename, zip, PhoneNo1, PhoneNo2,
                                BankName, BankAddr1, BankAddr2, BankCity, dllBankState, BankZip, BankRoute, BankAccount, StartSchoolYear, EndSchoolYear, ChangedDate, newDistrictId);
                            if (districtsId == -1)
                            {
                                retValue = Convert.ToInt32(adminDistrictSave.FirstOrDefault());
                            }
                        }
                        else
                        {
                            retValue = -999;
                        }
                    }
                    else
                    {
                        retValue = -1000;
                    }

                }
                catch (Exception ex)
                {
                    //Error logging in cloud tables
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SaveDistrictData");
                }
            }
            districtId = districtsId;

            return Convert.ToString(retValue);
        }
        public string ReactivateDistrict(string data)
        {
            try
            {
                bool done = false;
                string[] fildsToUpdate = data.Split('*');
                if (fildsToUpdate.Length == 2)
                {
                    string districtName = Convert.ToString(fildsToUpdate[0].ToString());
                    long clientid = Convert.ToInt32(fildsToUpdate[1]);
                    done = Reactivate(clientid, districtName);
                    if (done) return "-1"; else return "1";
                }
                else
                {
                    return "-2";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ReactivateDistrict");
                return "-2";
            }

        }
        public bool Reactivate(long clientId, string districtName)
        {
            try
            {
                var dist = context.Districts.Where(x => x.ClientID == clientId && x.DistrictName == districtName && x.isDeleted == true).FirstOrDefault();
                dist.isDeleted = false;
                context.Entry(dist).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Reactivate");
                return false;
            }
        }

        /// <summary>
        /// School page stuff
        /// </summary>
        /// <param name="getSchoolData"></param>
        /// <returns></returns>
        ///

        public IEnumerable<District> GetDistricts(long clientid)
        {
            try
            {
                return context.Districts.Where(s => s.ClientID == clientid);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetDistricts");
                return null;   
            }
        }

        public IEnumerable<SchoolIndexData> GetSchoolbyDistrictID(long Districtid, long ClientID)
        {
            try
            {
                return this.context.Admin_School_List(ClientID, Districtid, false).Select(s => new SchoolIndexData { School_Name = s.School_Name, District_Id = s.District_Id, District_Name = s.District_Name, Id = s.Id, POSNum = (int)s.POSNum, School_ID = s.School_ID }).AsEnumerable<SchoolIndexData>();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchoolbyDistrictID");
                return null;   
            }
        }


        public bool isDeletedDistrict(long clientid, string districtName, int distId)
        {
            try
            {
                if (distId == -1)
                {
                    int Count = 0;

                    Count = context.Districts.Where(s => s.ClientID == clientid && s.DistrictName == districtName && s.isDeleted == true).Count();
                    if (Count > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "isDeletedDistrict");
                return false;      
            }
        }

        public bool isDistrictAlreadyExists(long clientid, string districtName, int distId)
        {
            try
            {
                if (distId == -1)
                {
                    return context.Districts.Where(s => s.ClientID == clientid && s.DistrictName == districtName && s.isDeleted == false).Any();
                }
                else
                {
                    return context.Districts.Where(s => s.ClientID == clientid && s.DistrictName == districtName && s.ID != distId && s.isDeleted == false).Any();
                    // return false;
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "isDistrictAlreadyExists");
                return false;    
            }

        }

        public string SaveSchoolData(string schoolData, out Int32 schoolId)
        {
            string[] fildsToUpdate = schoolData.Split('*');
            int schoolsId = 0;
            int success = 0;

            if (fildsToUpdate.Length == 28)
            {
                long clientid = Convert.ToInt32(fildsToUpdate[0]);
                schoolsId = Convert.ToInt32(fildsToUpdate[1]);
                int distId = Convert.ToInt32(fildsToUpdate[2]);
                string schoolIdent = fildsToUpdate[3];
                string schoolName = fildsToUpdate[4];
                Nullable<int> DirectorID = Convert.ToInt32(fildsToUpdate[5]);
                Nullable<int> AdminID = Convert.ToInt32(fildsToUpdate[6]);
                string addr1 = fildsToUpdate[7];
                string addr2 = fildsToUpdate[8];
                string city = fildsToUpdate[9];
                string state = fildsToUpdate[10];
                string zip = fildsToUpdate[11];
                string Phone1 = fildsToUpdate[12];
                string Phone2 = fildsToUpdate[13];
                string Notes = fildsToUpdate[14];
                Boolean severNeed = Convert.ToBoolean(fildsToUpdate[15]);
                Boolean deleted = Convert.ToBoolean(fildsToUpdate[16]);
                Boolean useDistExec = Convert.ToBoolean(fildsToUpdate[17]);
                Double alaCarteLimit = Convert.ToDouble(fildsToUpdate[18]);
                Double mealPlanLimit = Convert.ToDouble(fildsToUpdate[19]);
                Boolean UsePinPreFix = Convert.ToBoolean(fildsToUpdate[20]);
                string pinPreFix = fildsToUpdate[21];
                Boolean usePhotos = Convert.ToBoolean(fildsToUpdate[22]);
                Boolean useFingerIdent = Convert.ToBoolean(fildsToUpdate[23]);
                int? BarCodeLength;
                if (fildsToUpdate[24] == "")
                    BarCodeLength = null;
                else
                    BarCodeLength = Convert.ToInt32(fildsToUpdate[24]);
                DateTime? startDate = Convert.ToDateTime(fildsToUpdate[25]);
                if (startDate == DateTime.MinValue)
                    startDate = null;
                DateTime? endDate = Convert.ToDateTime(fildsToUpdate[26]);
                if (endDate == DateTime.MinValue)
                    endDate = null;
                Boolean stripZero = Convert.ToBoolean(fildsToUpdate[27]);

                string timeZoneOffset = string.Empty;
                if (System.Web.HttpContext.Current.Session["UserTimeZone"] != null)
                {
                    timeZoneOffset = System.Web.HttpContext.Current.Session["UserTimeZone"].ToString();
                }
                else if (System.Web.HttpContext.Current.Response.Cookies["ClientTimeZone"].Value != null)
                {
                    timeZoneOffset = System.Web.HttpContext.Current.Response.Cookies["ClientTimeZone"].Value.ToString();
                }

                DateTime? ChangedDate = DateTime.Now;

                if (useDistExec == true)
                {
                    DirectorID = null;
                    AdminID = null;
                }

                if (schoolsId == 0)
                {
                    schoolsId = -1;
                }
                try
                {
                    var retValue = this.context.Admin_School_Save(clientid, schoolsId, distId, schoolIdent, schoolName, ChangedDate, DirectorID, AdminID, addr1, addr2, city, state, zip, Phone1,
                         Phone2, Notes, severNeed, deleted, useDistExec, alaCarteLimit, mealPlanLimit, UsePinPreFix, pinPreFix, usePhotos, useFingerIdent, BarCodeLength, startDate,
                         endDate, stripZero);

                    if (schoolsId != -1)
                    {
                        success = 1;

                    }

                    schoolId = retValue.FirstOrDefault() == null ? 0 : Convert.ToInt32(retValue.FirstOrDefault());

                    if (success == 1)
                        return "The school record has been updated successfully.";
                    else
                        return Convert.ToString(retValue.FirstOrDefault());

                }
                catch (Exception ex)
                {
                    //Error logging in cloud tables
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SaveSchoolData");
                }
            }
            schoolId = schoolsId;
            return "";
        }

        public SchoolsVM getSchool(long clientid, long schoolId)
        {
            try
            {
                if (schoolId != 0)
                {
                    return this.context.Schools.Where(s => s.ClientID == clientid && s.ID == schoolId).Select(
                        svm => new SchoolsVM
                        {
                            ClientID = svm.ClientID,
                            Id = svm.ID,
                            SchoolID = svm.SchoolID,
                            Emp_Director_Id = svm.Emp_Director_Id,
                            Emp_Administrator_Id = svm.Emp_Administrator_Id,
                            SchoolName = svm.SchoolName,
                            District_Id = svm.District_Id,
                            Address1 = svm.Address1,
                            Address2 = svm.Address2,
                            City = svm.City,
                            State = svm.State,
                            Zip = svm.Zip,
                            Phone1 = svm.Phone1,
                            Phone2 = svm.Phone2,
                            Comments = svm.Comment,
                            isSevereNeed = svm.isSevereNeed,
                            UseDistDirAdmin = svm.UseDistDirAdmin
                        }).FirstOrDefault();
                }
                else
                {
                    return new SchoolsVM(clientid);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSchool");
                return new SchoolsVM(clientid);
                
            }
        }

        public DistrictExecs getDistrictExecs(long Clientid, long districtId)
        {
            try
            {
                DistrictExecs execs = this.context.Districts.Where(d => d.ClientID == Clientid && d.ID == districtId).Select(ds => new DistrictExecs
                   {
                       Emp_Administrator_Id = ds.Emp_Administrator_Id,
                       Emp_Director_Id = ds.Emp_Director_Id
                   }).FirstOrDefault();

                return execs;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistrictExecs");
                return null;
            }
        }

        public string getCustomerName(long Clientid, long? Customerid)
        {
            try
            {
                string myName = string.Empty;

                Customer_Detail_VM name = this.context.Admin_Customer_Detail(Clientid, Customerid).Select(c => new Customer_Detail_VM
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName
                }).FirstOrDefault();

                if (name != null)
                    myName = name.FirstName + " " + name.LastName;

                return myName;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getCustomerName");
                return "";   
            }
        }

        public SchoolOptionsVM getSchoolOptions(long clientid, long schoolId)
        {
            try
            {
                if (schoolId != 0)
                {
                    return this.context.SchoolOptions.Where(s => s.ClientID == clientid && s.School_Id == schoolId).Select(
                        svm => new SchoolOptionsVM
                        {
                            DoPinPreFix = svm.DoPinPreFix,
                            PhotoLogging = svm.PhotoLogging,
                            StripZeros = svm.StripZeros,
                            AlaCarteLimit = (double)svm.AlaCarteLimit,
                            MealPlanLimit = (double)svm.MealPlanLimit,
                            StartSchoolYear = svm.StartSchoolYear,
                            EndSchoolYear = svm.EndSchoolYear,
                            PinPreFix = svm.PinPreFix,
                            BarCodeLength = svm.BarCodeLength
                        }).FirstOrDefault();
                }
                else
                {
                    return new SchoolOptionsVM();
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSchoolOptions");
                return null;
            }
        }

        public IList<SchoolNames> getSchoolNames(long clientid)
        {
            try
            {
                return this.context.Schools.Where(d => d.ClientID == clientid).Select(
                    sch => new SchoolNames
                    {
                        schoolId = sch.ID,
                        schoolName = sch.SchoolName
                    }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSchoolNames");
                return null;
            }
        }

        public IEnumerable<Customer_List> getAdultCustomers(long clientId)
        {
            try
            {
                IEnumerable<Customer_List> results;

                IQueryable<Admin_Customer_List_Result> query = this.context.Admin_Customer_List(clientId, "", 0, "", 1);
                query.Where(c => c.Adult == true);

                results = query.Select(c => new Customer_List
                {
                    id = c.Customer_Id,
                    First_Name = c.First_Name + " " + c.Last_Name
                });

                return results;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getAdultCustomers");
                return null;   
            }
        }

        public DistrictDates getDistrictDates(long clientid, long schoolId)
        {
            try
            {
                if (schoolId != 0)
                {
                    return this.context.DistrictOptions.Join(this.context.Schools, d => d.District_Id, s => s.District_Id, (d, s) =>
                        new { dist = d, school = s }).Where(s => s.school.ID == schoolId).Select(
                        dis => new DistrictDates 
                     {
                         
                         StartSchoolYear =  dis.dist.StartSchoolYear,
                         EndSchoolYear = dis.dist.EndSchoolYear
                         
                     }).FirstOrDefault();
                }
                else
                {
                    return new DistrictDates();
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDistrictDates");
                return new DistrictDates();
            }
        }

        public SchoolsData getSchoolsData(long clientid, long schoolId)
        {
            try
            {
                return new SchoolsData
                  {
                      SchoolVM = getSchool(clientid, schoolId),
                      SchoolOptionsVM = getSchoolOptions(clientid, schoolId),
                      SchoolList = getSchoolNames(clientid),
                      DistrictDates = getDistrictDates(clientid, schoolId)
                  };
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSchoolsData");
                return null;   
            }
        }

        public string DeleteSchool(string SchoolIDs)
        {
            string retValue = "1";
            string[] fildsToUpdate = SchoolIDs.Split('*');
            if (fildsToUpdate.Length == 2)
            {
                long ClientID = Convert.ToInt32(fildsToUpdate[0]);
                int schoolID = Convert.ToInt32(fildsToUpdate[1]);
                //call db function to deleet
                try
                {
                    Admin_School_Delete_Result stuff = (Admin_School_Delete_Result)this.context.Admin_School_Delete((long?)ClientID, (int?)schoolID, false, false, false, false).Single();
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
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteSchool");
                }
            }
            return Convert.ToString(retValue);
        }

        private DateTime? checkDateTime(string tempDate)
        {
            if (tempDate != "")
            {
                return Convert.ToDateTime(tempDate);
            }
            else
            {
                return null;
            }

        }
        public string SavePOSData(string posData)
        {
            string retValue = "1";
            string[] fildsToUpdate = posData.Split('*');
            if (fildsToUpdate.Length == 7)
            {
                long ClientID = Convert.ToInt32(fildsToUpdate[0]);
                int POSID = Convert.ToInt32(fildsToUpdate[1]);
                int school_Id = Convert.ToInt32(fildsToUpdate[2]);
                string Name = fildsToUpdate[3];
                bool EnableCCProcessing = Convert.ToBoolean(fildsToUpdate[4]);
                string VeriFoneUserId = fildsToUpdate[5];
                string VeriFonePassword = fildsToUpdate[6];
                if (VeriFonePassword == "temppass")
                {
                    VeriFonePassword = "";
                }
                try
                {
                    retValue = this.context.Admin_POS_Save(ClientID, POSID, school_Id, Name, EnableCCProcessing, VeriFoneUserId, VeriFonePassword).FirstOrDefault().Result.ToString();
                }
                catch (Exception ex)
                {
                    //Error logging in cloud tables
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SavePOSData");
                }
            }
            return retValue;
        }
        public Admin_POS_Delete_Result DeletePOS(string posIDs)
        {
            
            string[] fildsToUpdate = posIDs.Split('*');
            if (fildsToUpdate.Length == 2)
            {
                long ClientID = Convert.ToInt32(fildsToUpdate[0]);
                int POSID = Convert.ToInt32(fildsToUpdate[1]);
                //call db function to deleet
                try
                {
                    return this.context.Admin_POS_Delete(ClientID, POSID,true,true).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    //Error logging in cloud tables
                    ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeletePOS");
                }
            }
            return new Admin_POS_Delete_Result();
        }

        /// <summary>
        /// 
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

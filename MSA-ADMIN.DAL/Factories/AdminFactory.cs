using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MSA_ADMIN.DAL.Models;
using MSA_ADMIN.DAL.Common;
using System.Collections.ObjectModel;

namespace MSA_ADMIN.DAL.Factories
{
    public sealed class AdminFactory 
    {
        #region District
        #region Private Function
        private static DistrictData PopulateDistrictDataFromReader(SafeDataReader reader)
        {
            DistrictData dd = new DistrictData();
            dd.Id = reader.GetInt32("Id");
            dd.DistrictName = reader.GetString("DistrictName").Trim();
            dd.Address1 = reader.GetString("Address1").Trim();
            dd.City = reader.GetString("City").Trim();
            dd.State = reader.GetString("State").Trim();
            dd.Zip = reader.GetString("Zip").Trim();
            dd.BankCity = reader.GetString("BankCity").Trim();
            dd.BankState = reader.GetString("BankState").Trim();
            dd.BankZip = reader.GetString("BankZip").Trim();
            dd.BankName = reader.GetString("BankName").Trim();
            dd.BankAddr1 = reader.GetString("BankAddr1").Trim();
            dd.BankRoute = reader.GetString("BankRoute").Trim();
            dd.BankAccount = reader.GetString("BankAccount").Trim();
            //dd.BankMICR = reader.geto
            return dd;

        }

        private static AdteligibilityData PopulateAdteligibilityDataFromReader(SafeDataReader reader)
        {
            AdteligibilityData ad = new AdteligibilityData();

            ad.Id = reader.GetInt32("Id");
            ad.AFAnnual = reader.GetFloat("AFAnnual");
            ad.AFMonthly = reader.GetFloat("AFMonthly");
            ad.AFWeekly = reader.GetFloat("AFWeekly");
            ad.ARAnnual = reader.GetFloat("ARAnnual");
            ad.ARMonthly = reader.GetFloat("ARMonthly");
            ad.ARWeekly = reader.GetFloat("ARWeekly");
            ad.District_Id = reader.GetInt32("District_Id");

            return ad;

        }

        private static AHouseHoldIDData PopulateAHouseHoldIDDataFromReader(SafeDataReader reader)
        {
            AHouseHoldIDData hhd = new AHouseHoldIDData();

            hhd.Id = reader.GetInt32("Id");
            hhd.AvailHHID = reader.GetString("AvailHHID");
            hhd.District_Id = reader.GetInt32("District_Id");

            return hhd;

        }

        private static EligibilityData PopulateEligibilityDataFromReader(SafeDataReader reader)
        {
            EligibilityData ed = new EligibilityData();

            ed.Id = reader.GetInt32("Id");
            ed.FamSize = reader.GetInt32("FamSize");
            ed.FAnnual = reader.GetFloat("FAnnual");
            ed.FMonthly = reader.GetFloat("FMonthly");
            ed.FWeekly = reader.GetFloat("FWeekly");
            ed.RAnnual = reader.GetFloat("RAnnual");
            ed.RMonthly = reader.GetFloat("RMonthly");
            ed.RWeekly = reader.GetFloat("RWeekly");
            ed.District_Id = reader.GetInt32("District_Id");

            return ed;
        }

        private static DistrictOptionsData PopulateDistrictOptionsDataFromReader(SafeDataReader reader)
        {
            DistrictOptionsData dod = new DistrictOptionsData();
            //dod.District= new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("District_Id"));
            dod.ChangedDate = reader.GetSmartDate("ChangedDate");
            dod.LetterWarning1 = reader.GetInt32("LetterWarning1");
            dod.LetterWarning2 = reader.GetInt32("LetterWarning2");
            dod.LetterWarning3 = reader.GetInt32("LetterWarning3");
            dod.TaxPercent = reader.GetDouble("TaxPercent");
            dod.isEmployeeTaxable = reader.GetBoolean("isEmployeeTaxable");
            dod.isStudentFreeTaxable = reader.GetBoolean("isStudentFreeTaxable");
            dod.isStudentPaidTaxable = reader.GetBoolean("isStudentPaidTaxable");
            dod.isStudentRedTaxable = reader.GetBoolean("isStudentRedTaxable");
            dod.StartSchoolYear = reader.GetSmartDate("StartSchoolYear");
            dod.EndSchoolYear = reader.GetSmartDate("EndSchoolYear");
            dod.StartForms = reader.GetSmartDate("StartForms");
            dod.EndForms = reader.GetSmartDate("EndForms");
            dod.ID = reader.GetInt32("Id");
            dod.SetFormsDates = reader.GetBoolean("SetFormsDates");

            //dd.BankMICR = reader.geto
            return dod;
        }

        private static LettersData PopulateLettersDataFromReader(SafeDataReader reader)
        {
            LettersData ld = new LettersData();

            ld.Id = reader.GetInt32("Id");
            ld.District = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("District_Id"));
            ld.Language = new NameValuePair(reader.GetString("Name"), reader.GetInt32("Language_Id"));
            ld.Letter1 = reader.GetString("Letter1");
            ld.Letter2 = reader.GetString("Letter2");
            ld.Letter3 = reader.GetString("Letter3");

            return ld;

        }

        private static HouseHoldData PopulateHouseHoldDataFromReader(SafeDataReader reader)
        {
            HouseHoldData hhd = new HouseHoldData();
            hhd.HouseholdID = reader.GetString("HouseholdID");
            hhd.HHSize = reader.GetInt32("HHSize");
            hhd.AdditionalMembers = reader.GetInt32("AdditionalMembers");
            hhd.DateSigned = reader.GetSmartDate("DateSigned");
            hhd.FSNum = reader.GetString("FSNum");
            hhd.Addr1 = reader.GetString("Addr1");
            hhd.Addr2 = reader.GetString("Addr2");
            hhd.City = reader.GetString("City");
            hhd.State = reader.GetString("State");
            hhd.Zip = reader.GetString("Zip");
            hhd.Phone1 = reader.GetString("Phone1");
            hhd.Phone2 = reader.GetString("Phone2");
            hhd.Language = reader.GetString("Language");
            hhd.Cert = reader.GetString("Cert");
            hhd.MilkOnly = reader.GetString("MilkOnly");
            hhd.HealthDept = reader.GetString("HealthDept");
            hhd.TempCode = reader.GetString("TempCode");
            hhd.TempCodeExpDate = reader.GetSmartDate("TempCodeExpDate");
            hhd.Comment = reader.GetString("Comment");
            hhd.Signed_SSN = reader.GetString("Signed_SSN");
            hhd.SignedLName = reader.GetString("SignedLName");
            hhd.SignedFName = reader.GetString("SignedFName");
            hhd.SignedMI = reader.GetString("SignedMI");
            hhd.HHAreaCode = reader.GetString("HHAreaCode");
            hhd.District = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("District_Id"));
            hhd.Id = reader.GetInt32("Id");
            hhd.TANF = reader.GetString("TANF");
            hhd.TempStatus = reader.GetInt32("TempStatus");
            hhd.AppLetterSent = reader.GetBoolean("AppLetterSent");
            hhd.NoIncome = reader.GetBoolean("NoIncome");
            hhd.Migrant = reader.GetString("Migrant");



            //dd.BankMICR = reader.geto
            return hhd;

        }

        private static ReducedMealsData PopulateReducedMealsDataFromReader(SafeDataReader reader)
        {
            ReducedMealsData rmd = new ReducedMealsData();
            rmd.Lunch = reader.GetFloat("Lunch");
            rmd.Breakfast = reader.GetFloat("Breakfast");
            rmd.Snacks = reader.GetFloat("Snacks");
            rmd.Id = reader.GetInt32("Id");
            rmd.District = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("District_Id"));

            return rmd;

        }

        private static StudentData PopulateStudentDataFromReader(SafeDataReader reader)
        {
            StudentData std = new StudentData();

            std.Id = reader.GetInt32("Id");
            std.UserID = reader.GetString("UserID");
            std.Race = reader.GetString("Race");
            std.AppDate = reader.GetSmartDate("AppDate");
            std.FosterChild = reader.GetBoolean("FosterChild");
            std.FosterIncome = reader.GetFloat("FosterIncome");
            std.ApprovalStatus = reader.GetString("ApprovalStatus");
            std.DateEntered = reader.GetSmartDate("DateEntered");
            std.DateChanged = reader.GetSmartDate("DateChanged");
            std.HouseholdID = reader.GetString("HouseholdID");
            std.District = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("District_Id"));
            NameValuePair[] nvpcon = new NameValuePair[3];
            nvpcon[0].Value = reader.GetInt32("Id");
            nvpcon[0].Name = reader.GetInt32("FirstName").ToString();
            nvpcon[1].Value = reader.GetInt32("Id");
            nvpcon[1].Name = reader.GetInt32("LastName").ToString();
            nvpcon[2].Value = reader.GetInt32("Id");
            nvpcon[2].Name = reader.GetInt32("Middle").ToString();
            std.Customer = new NameValuePairCollection(nvpcon);
            std.AppLetterSent = reader.GetBoolean("AppLetterSent");

            return std;

        }

        private static ParentData PopulateParentDataFromReader(SafeDataReader reader)
        {
            ParentData pd = new ParentData();

            pd.Id = reader.GetInt32("Id");
            pd.HouseholdID = reader.GetString("HouseholdID");
            pd.LName = reader.GetString("LName");
            pd.FName = reader.GetString("FName");
            pd.MI = reader.GetString("MI");
            pd.SSN = reader.GetString("SSN");
            pd.ParentalStatus = reader.GetString("ParentalStatus");
            pd.Income1 = reader.GetFloat("Income1");
            pd.Income2 = reader.GetFloat("Income2");
            pd.Income3 = reader.GetFloat("Income3");
            pd.Income4 = reader.GetFloat("Income4");
            pd.Income5 = reader.GetFloat("Income5");
            pd.TotalIncome = reader.GetFloat("TotalIncome");
            pd.Frequency1 = reader.GetString("Frequency1");
            pd.Frequency2 = reader.GetString("Frequency2");
            pd.Frequency3 = reader.GetString("Frequency3");
            pd.Frequency4 = reader.GetString("Frequency4");
            pd.Frequency5 = reader.GetString("Frequency5");
            pd.Districtd = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("District_Id"));
            pd.Frequency4 = reader.GetString("Frequency4");
            //dd.BankMICR = reader.geto
            return pd;

        }

        #endregion

        #region Public Function
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DistrictData[] ListDistrict(int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<DistrictData> ddList = new List<DistrictData>();
            try
            {

                //string strSQL = "select Max(ExpenseAccNumber) as ExpMaxNumber from ExpenseAccountsType where OwnerID=" + pOwnerID;
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_DIS_GetDistrict", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    DistrictData dd = PopulateDistrictDataFromReader(reader);
                    ddList.Add(dd);

                }
                return ddList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pId"></param>
        /// <returns></returns>
        public static DistrictData[] GetDistrictByDistrictId(int pId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<DistrictData> ddList = new List<DistrictData>();
            try
            {
                data.AddIntParameter("@arg_Id", pId);
                reader = data.GetDataReader("usp_DIS_GetDistrictByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    DistrictData dd = PopulateDistrictDataFromReader(reader);
                    ddList.Add(dd);
                }
                return ddList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForDistrict(string name)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as DistrictCount from Districts where Name='" + name + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("DistrictCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetDistrictsCount()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as listscount from Districts";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static DataSet getDistrictExistCount(int Id)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.FillDataSet("select count(*) from Schools where District_Id='" + Id + "'", DataPortal.QueryType.QueryString, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                if (data != null)
                    data.Dispose();
            }
        }

        public static string  GetDistrictsNameByDistrictID(int pDistrictID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                string pDistrictName = "";
                string strSQL = "select Name as DistrictName from Districts where id=" + pDistrictID;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    pDistrictName = reader.GetString("DistrictName");
                }
                return pDistrictName;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }


        public static int GetDistrictLogin(string UserName, string Password)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int pDistrictID = 0;
                string strSQL = "select * from Districts where (AdminUserName='" + UserName + "' ) AND (AdminPassword= '" + Password + " ') ";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    pDistrictID = reader.GetInt32("Id");
                }
                return pDistrictID;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }


        public static int GetSecurityToken(string TokenID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int pDistrictID = 0;
                string strSQL = "select * from SecurityToken where TokenID = '" + TokenID + "' ";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {   
                    pDistrictID = reader.GetInt32("DistrictID");
                }
                return pDistrictID;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }


        public static AdteligibilityData[] GetAdteligibilityByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<AdteligibilityData> adlist = new List<AdteligibilityData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetAdteligibilityByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    AdteligibilityData ad = PopulateAdteligibilityDataFromReader(reader);
                    adlist.Add(ad);
                }
                return adlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }

        }

        public static AHouseHoldIDData[] GetAHouseHoldIDByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<AHouseHoldIDData> hhdlist = new List<AHouseHoldIDData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetAHouseHoldIDByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    AHouseHoldIDData hhd = PopulateAHouseHoldIDDataFromReader(reader);
                    hhdlist.Add(hhd);
                }
                return hhdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static EligibilityData[] GetEligibilityByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EligibilityData> edlist = new List<EligibilityData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetEligibilityByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    EligibilityData ed = PopulateEligibilityDataFromReader(reader);
                    edlist.Add(ed);
                }
                return edlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static DistrictOptionsData[] GetDistrictOptionsByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<DistrictOptionsData> dodlist = new List<DistrictOptionsData>();
            try
            {
                data.AddIntParameter("@arg_DistrictID", pDistrictId);
                reader = data.GetDataReader("usp_DIS_GetDistrictOptionsByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    DistrictOptionsData dod = PopulateDistrictOptionsDataFromReader(reader);
                    dodlist.Add(dod);
                }
                return dodlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static LettersData[] GetLettersByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<LettersData> ldlist = new List<LettersData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetLettersByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    LettersData ld = PopulateLettersDataFromReader(reader);
                    ldlist.Add(ld);
                }
                return ldlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static HouseHoldData[] GetHouseHoldByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<HouseHoldData> hhdlist = new List<HouseHoldData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetHouseholdByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    HouseHoldData hhd = PopulateHouseHoldDataFromReader(reader);
                    hhdlist.Add(hhd);
                }
                return hhdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static ReducedMealsData[] GetReducedMealsByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<ReducedMealsData> rmdlist = new List<ReducedMealsData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetReducedMealsByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    ReducedMealsData rmd = PopulateReducedMealsDataFromReader(reader);
                    rmdlist.Add(rmd);
                }
                return rmdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static StudentData[] GetStudentByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<StudentData> stdlist = new List<StudentData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetStudentByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    StudentData std = PopulateStudentDataFromReader(reader);
                    stdlist.Add(std);
                }
                return stdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static ParentData[] GetParentByDistrictId(int pDistrictId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<ParentData> pdlist = new List<ParentData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", pDistrictId);
                reader = data.GetDataReader("usp_ADM_GetParentByDistrictId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    ParentData pd = PopulateParentDataFromReader(reader);
                    pdlist.Add(pd);
                }
                return pdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Del,Add,Chg

        #region District
        public static int AddDistrict(int Emp_Administrator_Id, int Emp_Director_Id, string DistrictName, string Address1, string Address2, string City, string State, string Zip, string Phone1, string Phone2, bool isDeleted, string BankCity, string BankState, string BankZip, string BankName, string BankAddr1, string BankAddr2, string BankRoute, string BankAccount, byte[] BankMICR)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Emp_Administrator_Id != 0)
                    data.AddIntParameter("@arg_Emp_Administrator_Id", Emp_Administrator_Id);
                if (Emp_Director_Id != 0)
                    data.AddIntParameter("@arg_Emp_Director_Id", Emp_Director_Id);
                if (DistrictName != "")
                    data.AddStringParameter("@arg_DistrictName", DistrictName);
                if (Address1 != "")
                    data.AddStringParameter("@arg_Address1", Address1);
                if (Address2 != "")
                    data.AddStringParameter("@arg_Address2", Address2);
                if (City != "")
                    data.AddStringParameter("@arg_City", City);
                if (State != "")
                    data.AddStringParameter("@arg_State", State);
                if (Zip != "")
                    data.AddStringParameter("@arg_Zip", Zip);
                if (Phone1 != "")
                    data.AddStringParameter("@arg_Phone1", Phone1);
                if (Phone2 != "")
                    data.AddStringParameter("@arg_Phone2", Phone2);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (BankCity != "")
                    data.AddStringParameter("@arg_BankCity", BankCity);
                if (BankState != "")
                    data.AddStringParameter("@arg_BankState", BankState);
                if (BankZip != "")
                    data.AddStringParameter("@arg_BankZip", BankZip);
                if (BankName != "")
                    data.AddStringParameter("@arg_BankName", BankName);
                if (BankAddr1 != "")
                    data.AddStringParameter("@arg_BankAddr1", BankAddr1);
                if (BankAddr2 != "")
                    data.AddStringParameter("@arg_BankAddr2", BankAddr2);
                if (BankRoute != "")
                    data.AddStringParameter("@arg_BankRoute", BankRoute);
                if (BankAccount != "")
                    data.AddStringParameter("@arg_BankAccount", BankAccount);
                if (BankMICR != null)
                    data.AddImageParameter("@arg_BankMICR", BankMICR);

                data.SubmitData("USP_DIS_AddDistrict", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgDistrict(int Id, int Emp_Administrator_Id, int Emp_Director_Id, string DistrictName, string Address1, string Address2, string City, string State, string Zip, string Phone1, string Phone2, bool isDeleted, string BankCity, string BankState, string BankZip, string BankName, string BankAddr1, string BankAddr2, string BankRoute, string BankAccount, byte[] BankMICR)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Emp_Administrator_Id != 0)
                    data.AddIntParameter("@arg_Emp_Administrator_Id", Emp_Administrator_Id);
                if (Emp_Director_Id != 0)
                    data.AddIntParameter("@arg_Emp_Director_Id", Emp_Director_Id);
                if (DistrictName != "")
                    data.AddStringParameter("@arg_DistrictName", DistrictName);
                if (Address1 != "")
                    data.AddStringParameter("@arg_Address1", Address1);
                if (Address2 != "")
                    data.AddStringParameter("@arg_Address2", Address2);
                if (City != "")
                    data.AddStringParameter("@arg_City", City);
                if (State != "")
                    data.AddStringParameter("@arg_State", State);
                if (Zip != "")
                    data.AddStringParameter("@arg_Zip", Zip);
                if (Phone1 != "")
                    data.AddStringParameter("@arg_Phone1", Phone1);
                if (Phone2 != "")
                    data.AddStringParameter("@arg_Phone2", Phone2);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (BankCity != "")
                    data.AddStringParameter("@arg_BankCity", BankCity);
                if (BankState != "")
                    data.AddStringParameter("@arg_BankState", BankState);
                if (BankZip != "")
                    data.AddStringParameter("@arg_BankZip", BankZip);
                if (BankName != "")
                    data.AddStringParameter("@arg_BankName", BankName);
                if (BankAddr1 != "")
                    data.AddStringParameter("@arg_BankAddr1", BankAddr1);
                if (BankAddr2 != "")
                    data.AddStringParameter("@arg_BankAddr2", BankAddr2);
                if (BankRoute != "")
                    data.AddStringParameter("@arg_BankRoute", BankRoute);
                if (BankAccount != "")
                    data.AddStringParameter("@arg_BankAccount", BankAccount);
                if (BankMICR != null)
                    data.AddImageParameter("@arg_BankMICR", BankMICR);

                data.SubmitData("USP_DIS_ChgDistrictByDistrictID", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelDistrict(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("USP_DIS_DelDistrictByDistrictID", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #region Adteligibility

        public static int AddAdteligibility(float AFAnnual, float AFMonthly, float AFWeekly, float ARAnnual, float ARMonthly, float ARWeekly, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (AFAnnual != 0)
                    data.AddFloatParameter("@arg_AFAnnual", AFAnnual);
                if (AFMonthly != 0)
                    data.AddFloatParameter("@arg_AFMonthly", AFMonthly);
                if (AFWeekly != 0)
                    data.AddFloatParameter("@arg_AFWeekly", AFWeekly);
                if (ARAnnual != 0)
                    data.AddFloatParameter("@arg_ARAnnual", ARAnnual);
                if (ARMonthly != 0)
                    data.AddFloatParameter("@arg_ARMonthly", ARMonthly);
                if (ARWeekly != 0)
                    data.AddFloatParameter("@arg_ARWeekly", ARWeekly);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);


                data.SubmitData("usp_ADM_AddAdteligibility", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgAdteligibility(int Id, float AFAnnual, float AFMonthly, float AFWeekly, float ARAnnual, float ARMonthly, float ARWeekly, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (AFAnnual != 0)
                    data.AddFloatParameter("@arg_AFAnnual", AFAnnual);
                if (AFMonthly != 0)
                    data.AddFloatParameter("@arg_AFMonthly", AFMonthly);
                if (AFWeekly != 0)
                    data.AddFloatParameter("@arg_AFWeekly", AFWeekly);
                if (ARAnnual != 0)
                    data.AddFloatParameter("@arg_ARAnnual", ARAnnual);
                if (ARMonthly != 0)
                    data.AddFloatParameter("@arg_ARMonthly", ARMonthly);
                if (ARWeekly != 0)
                    data.AddFloatParameter("@arg_ARWeekly", ARWeekly);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);


                data.SubmitData("usp_ADM_ChgAdteligibility", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelAdteligibility(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelAdteligibility", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region AHouseHoldId

        public static int AddAHouseHoldID(string AvailHHID, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (AvailHHID != "")
                    data.AddStringParameter("@arg_AvailHHID", AvailHHID);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);


                data.SubmitData("usp_ADM_AddAHouseHoldID", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgAHouseHoldID(int Id, string AvailHHID, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (AvailHHID != "")
                    data.AddStringParameter("@arg_AvailHHID", AvailHHID);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);


                data.SubmitData("usp_ADM_ChgAHouseHoldID", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelAHouseHoldID(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelAHouseHoldID", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Eligibility

        public static int AddEligibility(int FamSize, float FAnnual, float FMonthly, float FWeekly, float RAnnual, float RMonthly, float RWeekly, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (FamSize != 0)
                    data.AddIntParameter("@arg_FamSize", FamSize);
                if (FAnnual != 0)
                    data.AddFloatParameter("@arg_FAnnual", FAnnual);
                if (FMonthly != 0)
                    data.AddFloatParameter("@arg_FMonthly", FMonthly);
                if (FWeekly != 0)
                    data.AddFloatParameter("@arg_FWeekly", FWeekly);
                if (RAnnual != 0)
                    data.AddFloatParameter("@arg_RAnnual", RAnnual);
                if (RMonthly != 0)
                    data.AddFloatParameter("@arg_RMonthly", RMonthly);
                if (RWeekly != 0)
                    data.AddFloatParameter("@arg_RWeekly", RWeekly);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);


                data.SubmitData("usp_ADM_AddEligibility", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgEligibility(int Id, int FamSize, float FAnnual, float FMonthly, float FWeekly, float RAnnual, float RMonthly, float RWeekly, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (FamSize != 0)
                    data.AddIntParameter("@arg_FamSize", FamSize);
                if (FAnnual != 0)
                    data.AddFloatParameter("@arg_FAnnual", FAnnual);
                if (FMonthly != 0)
                    data.AddFloatParameter("@arg_FMonthly", FMonthly);
                if (FWeekly != 0)
                    data.AddFloatParameter("@arg_FWeekly", FWeekly);
                if (RAnnual != 0)
                    data.AddFloatParameter("@arg_RAnnual", RAnnual);
                if (RMonthly != 0)
                    data.AddFloatParameter("@arg_RMonthly", RMonthly);
                if (RWeekly != 0)
                    data.AddFloatParameter("@arg_RWeekly", RWeekly);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);


                data.SubmitData("usp_ADM_ChgEligibility", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelEligibility(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelEligibility", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region DistrictOptions

        public static int AddDistrictOptions(int District_Id, SmartDate ChangedDate, int LetterWarning1, int LetterWarning2, int LetterWarning3, float TaxPercent, bool isEmployeeTaxable, bool isStudentFreeTaxable, bool isStudentPaidTaxable, bool isStudentRedTaxable, SmartDate StartSchoolYear, SmartDate EndSchoolYear, SmartDate StartForms, SmartDate EndForms, bool SetFormsDates)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (ChangedDate != new SmartDate())
                    data.AddDateParameter("@arg_ChangedDate", ChangedDate);
                //if (LetterWarning1 != 0)
                data.AddIntParameter("@arg_LetterWarning1", LetterWarning1);
                //if (LetterWarning2 != 0)
                data.AddIntParameter("@arg_LetterWarning2", LetterWarning2);
                //if (LetterWarning3 != 0)
                data.AddIntParameter("@arg_LetterWarning3", LetterWarning3);
                //if (TaxPercent != 0)
                data.AddFloatParameter("@arg_TaxPercent", TaxPercent);
                //if (isEmployeeTaxable != false)
                data.AddBoolParameter("@arg_isEmployeeTaxable", isEmployeeTaxable);
                //if (isStudentFreeTaxable != false)
                data.AddBoolParameter("@arg_isStudentFreeTaxable", isStudentFreeTaxable);
                //if (isStudentPaidTaxable != false)
                data.AddBoolParameter("@arg_isStudentPaidTaxable", isStudentPaidTaxable);
                //if (isStudentRedTaxable != false)
                data.AddBoolParameter("@arg_isStudentRedTaxable", isStudentRedTaxable);
                if (StartSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_StartSchoolYear", StartSchoolYear);
                if (EndSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_EndSchoolYear", EndSchoolYear);
                if (StartForms != new SmartDate())
                    data.AddDateParameter("@arg_StartForms", StartForms);
                if (EndForms != "")
                    data.AddDateParameter("@arg_EndForms", EndForms);
                //if (SetFormsDates != false)
                data.AddBoolParameter("@arg_SetFormsDates", SetFormsDates);

                data.SubmitData("usp_ADM_AddDistrictOptions", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgDistrictOptions(int Id, int District_Id, SmartDate ChangedDate, int LetterWarning1, int LetterWarning2, int LetterWarning3, float TaxPercent, bool isEmployeeTaxable, bool isStudentFreeTaxable, bool isStudentPaidTaxable, bool isStudentRedTaxable, SmartDate StartSchoolYear, SmartDate EndSchoolYear, SmartDate StartForms, SmartDate EndForms, bool SetFormsDates)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (ChangedDate != new SmartDate())
                    data.AddDateParameter("@arg_ChangedDate", ChangedDate);
                //if (LetterWarning1 != 0)
                data.AddIntParameter("@arg_LetterWarning1", LetterWarning1);
                //if (LetterWarning2 != 0)
                data.AddIntParameter("@arg_LetterWarning2", LetterWarning2);
                //if (LetterWarning3 != 0)
                data.AddIntParameter("@arg_LetterWarning3", LetterWarning3);
                //if (TaxPercent != 0)
                data.AddFloatParameter("@arg_TaxPercent", TaxPercent);
                //if (isEmployeeTaxable != false)
                data.AddBoolParameter("@arg_isEmployeeTaxable", isEmployeeTaxable);
                //if (isStudentFreeTaxable != false)
                data.AddBoolParameter("@arg_isStudentFreeTaxable", isStudentFreeTaxable);
                //if (isStudentPaidTaxable != false)
                data.AddBoolParameter("@arg_isStudentPaidTaxable", isStudentPaidTaxable);
                //if (isStudentRedTaxable != false)
                data.AddBoolParameter("@arg_isStudentRedTaxable", isStudentRedTaxable);
                if (StartSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_StartSchoolYear", StartSchoolYear);
                if (EndSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_EndSchoolYear", EndSchoolYear);
                if (StartForms != new SmartDate())
                    data.AddDateParameter("@arg_StartForms", StartForms);
                if (EndForms != "")
                    data.AddDateParameter("@arg_EndForms", EndForms);
                // if (SetFormsDates != false)
                data.AddBoolParameter("@arg_SetFormsDates", SetFormsDates);

                data.SubmitData("usp_DIS_ChgDistrictOptionsByDistrictOptionsID", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelDistrictOptions(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelDistrictOptions", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Letters

        public static int AddLetters(int District_Id, int Language_Id, string Letter1, string Letter2, string Letter3)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Language_Id != 0)
                    data.AddIntParameter("@arg_Language_Id", Language_Id);
                if (Letter1 != "")
                    data.AddStringParameter("@arg_Letter1", Letter1);
                if (Letter2 != "")
                    data.AddStringParameter("@arg_Letter2", Letter2);
                if (Letter3 != "")
                    data.AddStringParameter("@arg_Letter3", Letter3);

                data.SubmitData("usp_ADM_AddLetters", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgLetters(int Id, int District_Id, int Language_Id, string Letter1, string Letter2, string Letter3)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Language_Id != 0)
                    data.AddIntParameter("@arg_Language_Id", Language_Id);
                if (Letter1 != "")
                    data.AddStringParameter("@arg_Letter1", Letter1);
                if (Letter2 != "")
                    data.AddStringParameter("@arg_Letter2", Letter2);
                if (Letter3 != "")
                    data.AddStringParameter("@arg_Letter3", Letter3);

                data.SubmitData("usp_ADM_ChgLetters", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelLetters(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelLetters", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region HouseHold

        public static int AddHouseHold(string HouseholdID, int HHSize, int AdditionalMembers, SmartDate DateSigned, string FSNum, string Addr1, string Addr2, string City, string State, string Zip, string Phone1, string Phone2, string Language, string Cert, string MilkOnly, string HealthDept, string TempCode, SmartDate TempCodeExpDate, string Comment, string Signed_SSN, string SignedLName, string SignedFName, string SignedMI, string HHAreaCode, int District_Id, string TANF, int TempStatus, bool AppLetterSent, bool NoIncome, string Migrant)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (HouseholdID != "")
                    data.AddStringParameter("@arg_HouseholdID", HouseholdID);
                if (HHSize != 0)
                    data.AddIntParameter("@arg_HHSize", HHSize);
                if (AdditionalMembers != 0)
                    data.AddIntParameter("@arg_AdditionalMembers", AdditionalMembers);
                if (FSNum != "")
                    data.AddStringParameter("@arg_FSNum", FSNum);
                if (Addr1 != "")
                    data.AddStringParameter("@arg_Addr1", Addr1);
                if (Addr2 != "")
                    data.AddStringParameter("@arg_Addr2", Addr2);
                if (City != "")
                    data.AddStringParameter("@arg_City", City);
                if (State != "")
                    data.AddStringParameter("@arg_State", State);
                if (Zip != "")
                    data.AddStringParameter("@arg_Zip", Zip);
                if (Phone1 != "")
                    data.AddStringParameter("@arg_Phone1", Phone1);
                if (Phone2 != "")
                    data.AddStringParameter("@arg_Phone2", Phone2);
                if (Language != "")
                    data.AddStringParameter("@arg_Language", Language);
                if (Cert != "")
                    data.AddStringParameter("@arg_Cert", Cert);
                if (MilkOnly != "")
                    data.AddStringParameter("@arg_MilkOnly", MilkOnly);
                if (HealthDept != "")
                    data.AddStringParameter("@arg_HealthDept", HealthDept);
                if (TempCode != "")
                    data.AddStringParameter("@arg_TempCode", TempCode);
                if (TempCodeExpDate != new SmartDate())
                    data.AddDateParameter("@arg_TempCodeExpDate", TempCodeExpDate);
                if (Comment != "")
                    data.AddStringParameter("@arg_Comment", Comment);
                if (Signed_SSN != "")
                    data.AddStringParameter("@arg_Signed_SSN", Signed_SSN);
                if (SignedLName != "")
                    data.AddStringParameter("@arg_SignedLName", SignedLName);
                if (SignedFName != "")
                    data.AddStringParameter("@arg_SignedFName", SignedFName);
                if (SignedMI != "")
                    data.AddStringParameter("@arg_SignedMI", SignedMI);
                if (HHAreaCode != "")
                    data.AddStringParameter("@arg_HHAreaCode", HHAreaCode);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (TANF != "")
                    data.AddStringParameter("@arg_TANF", TANF);
                if (TempStatus != 0)
                    data.AddIntParameter("@arg_TempStatus", TempStatus);
                if (AppLetterSent != false)
                    data.AddBoolParameter("@arg_AppLetterSent", AppLetterSent);
                if (NoIncome != false)
                    data.AddBoolParameter("@arg_NoIncome", NoIncome);
                if (Migrant != "")
                    data.AddStringParameter("@arg_Migrant", Migrant);


                data.SubmitData("usp_ADM_AddHouseHold", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgHouseHold(int Id, string HouseholdID, int HHSize, int AdditionalMembers, SmartDate DateSigned, string FSNum, string Addr1, string Addr2, string City, string State, string Zip, string Phone1, string Phone2, string Language, string Cert, string MilkOnly, string HealthDept, string TempCode, SmartDate TempCodeExpDate, string Comment, string Signed_SSN, string SignedLName, string SignedFName, string SignedMI, string HHAreaCode, int District_Id, string TANF, int TempStatus, bool AppLetterSent, bool NoIncome, string Migrant)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (HouseholdID != "")
                    data.AddStringParameter("@arg_HouseholdID", HouseholdID);
                if (HHSize != 0)
                    data.AddIntParameter("@arg_HHSize", HHSize);
                if (AdditionalMembers != 0)
                    data.AddIntParameter("@arg_AdditionalMembers", AdditionalMembers);
                if (FSNum != "")
                    data.AddStringParameter("@arg_FSNum", FSNum);
                if (Addr1 != "")
                    data.AddStringParameter("@arg_Addr1", Addr1);
                if (Addr2 != "")
                    data.AddStringParameter("@arg_Addr2", Addr2);
                if (City != "")
                    data.AddStringParameter("@arg_City", City);
                if (State != "")
                    data.AddStringParameter("@arg_State", State);
                if (Zip != "")
                    data.AddStringParameter("@arg_Zip", Zip);
                if (Phone1 != "")
                    data.AddStringParameter("@arg_Phone1", Phone1);
                if (Phone2 != "")
                    data.AddStringParameter("@arg_Phone2", Phone2);
                if (Language != "")
                    data.AddStringParameter("@arg_Language", Language);
                if (Cert != "")
                    data.AddStringParameter("@arg_Cert", Cert);
                if (MilkOnly != "")
                    data.AddStringParameter("@arg_MilkOnly", MilkOnly);
                if (HealthDept != "")
                    data.AddStringParameter("@arg_HealthDept", HealthDept);
                if (TempCode != "")
                    data.AddStringParameter("@arg_TempCode", TempCode);
                if (TempCodeExpDate != new SmartDate())
                    data.AddDateParameter("@arg_TempCodeExpDate", TempCodeExpDate);
                if (Comment != "")
                    data.AddStringParameter("@arg_Comment", Comment);
                if (Signed_SSN != "")
                    data.AddStringParameter("@arg_Signed_SSN", Signed_SSN);
                if (SignedLName != "")
                    data.AddStringParameter("@arg_SignedLName", SignedLName);
                if (SignedFName != "")
                    data.AddStringParameter("@arg_SignedFName", SignedFName);
                if (SignedMI != "")
                    data.AddStringParameter("@arg_SignedMI", SignedMI);
                if (HHAreaCode != "")
                    data.AddStringParameter("@arg_HHAreaCode", HHAreaCode);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (TANF != "")
                    data.AddStringParameter("@arg_TANF", TANF);
                if (TempStatus != 0)
                    data.AddIntParameter("@arg_TempStatus", TempStatus);
                if (AppLetterSent != false)
                    data.AddBoolParameter("@arg_AppLetterSent", AppLetterSent);
                if (NoIncome != false)
                    data.AddBoolParameter("@arg_NoIncome", NoIncome);
                if (Migrant != "")
                    data.AddStringParameter("@arg_Migrant", Migrant);


                data.SubmitData("usp_ADM_ChgHouseHold", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelHouseHold(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelHouseHold", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region ReducedMeals

        public static int AddReducedMeals(float Lunch, float Breakfast, float Snacks, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Lunch != 0)
                    data.AddFloatParameter("@arg_Lunch", Lunch);
                if (Breakfast != 0)
                    data.AddFloatParameter("@arg_Breakfast", Breakfast);
                if (Snacks != 0)
                    data.AddFloatParameter("@arg_Snacks", Snacks);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);

                data.SubmitData("usp_ADM_AddReducedMeals", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgReducedMeals(int Id, float Lunch, float Breakfast, float Snacks, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Lunch != 0)
                    data.AddFloatParameter("@arg_Lunch", Lunch);
                if (Breakfast != 0)
                    data.AddFloatParameter("@arg_Breakfast", Breakfast);
                if (Snacks != 0)
                    data.AddFloatParameter("@arg_Snacks", Snacks);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);

                data.SubmitData("usp_ADM_ChgReducedMeals", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelReducedMeals(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelReducedMeals", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Student

        public static int AddStudent(string UserID, string Race, SmartDate AppDate, bool FosterChild, float FosterIncome, string ApprovalStatus, SmartDate DateEntered, SmartDate DateChanged, string HouseholdID, int District_Id, int Customer_Id, bool AppLetterSent)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (UserID != "")
                    data.AddStringParameter("@arg_UserID", UserID);
                if (Race != "")
                    data.AddStringParameter("@arg_Race", Race);
                if (AppDate != new SmartDate())
                    data.AddDateParameter("@arg_AppDate", AppDate);
                if (FosterChild != false)
                    data.AddBoolParameter("@arg_FosterChild", FosterChild);
                if (FosterIncome != 0)
                    data.AddFloatParameter("@arg_FosterIncome", FosterIncome);
                if (ApprovalStatus != "")
                    data.AddStringParameter("@arg_ApprovalStatus", ApprovalStatus);
                if (DateEntered != new SmartDate())
                    data.AddDateParameter("@arg_DateEntered", DateEntered);
                if (DateChanged != new SmartDate())
                    data.AddDateParameter("@arg_DateChanged", DateChanged);
                if (HouseholdID != "")
                    data.AddStringParameter("@arg_HouseholdID", HouseholdID);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (AppLetterSent != false)
                    data.AddBoolParameter("@arg_AppLetterSent", AppLetterSent);

                data.SubmitData("usp_ADM_AddStudent", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgStudent(int Id, string UserID, string Race, SmartDate AppDate, bool FosterChild, float FosterIncome, string ApprovalStatus, SmartDate DateEntered, SmartDate DateChanged, string HouseholdID, int District_Id, int Customer_Id, bool AppLetterSent)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (UserID != "")
                    data.AddStringParameter("@arg_UserID", UserID);
                if (Race != "")
                    data.AddStringParameter("@arg_Race", Race);
                if (AppDate != new SmartDate())
                    data.AddDateParameter("@arg_AppDate", AppDate);
                if (FosterChild != false)
                    data.AddBoolParameter("@arg_FosterChild", FosterChild);
                if (FosterIncome != 0)
                    data.AddFloatParameter("@arg_FosterIncome", FosterIncome);
                if (ApprovalStatus != "")
                    data.AddStringParameter("@arg_ApprovalStatus", ApprovalStatus);
                if (DateEntered != new SmartDate())
                    data.AddDateParameter("@arg_DateEntered", DateEntered);
                if (DateChanged != new SmartDate())
                    data.AddDateParameter("@arg_DateChanged", DateChanged);
                if (HouseholdID != "")
                    data.AddStringParameter("@arg_HouseholdID", HouseholdID);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (AppLetterSent != false)
                    data.AddBoolParameter("@arg_AppLetterSent", AppLetterSent);

                data.SubmitData("usp_ADM_ChgStudent", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelStudent(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelStudent", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Parent

        public static int AddParent(string HouseholdID, string LName, string FName, string MI, string SSN, string ParentalStatus, float Income1, float Income2, float Income3, float Income4, float Income5, float TotalIncome, string Frequency1, string Frequency2, string Frequency3, string Frequency4, string Frequency5, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (HouseholdID != "")
                    data.AddStringParameter("@arg_HouseholdID", HouseholdID);
                if (LName != "")
                    data.AddStringParameter("@arg_LName", LName);
                if (FName != "")
                    data.AddStringParameter("@arg_FName", FName);
                if (MI != "")
                    data.AddStringParameter("@arg_MI", MI);
                if (SSN != "")
                    data.AddStringParameter("@arg_SSN", SSN);
                if (ParentalStatus != "")
                    data.AddStringParameter("@arg_ParentalStatus", ParentalStatus);
                if (Income1 != 0)
                    data.AddFloatParameter("@arg_Income1", Income1);
                if (Income2 != 0)
                    data.AddFloatParameter("@arg_Income2", Income2);
                if (Income3 != 0)
                    data.AddFloatParameter("@arg_Income3", Income3);
                if (Income4 != 0)
                    data.AddFloatParameter("@arg_Income4", Income4);
                if (Income5 != 0)
                    data.AddFloatParameter("@arg_Income5", Income5);
                if (Frequency1 != "")
                    data.AddStringParameter("@arg_Frequency1", Frequency1);
                if (Frequency2 != "")
                    data.AddStringParameter("@arg_Frequency2", Frequency2);
                if (Frequency3 != "")
                    data.AddStringParameter("@arg_Frequency3", Frequency3);
                if (Frequency4 != "")
                    data.AddStringParameter("@arg_Frequency4", Frequency4);
                if (Frequency5 != "")
                    data.AddStringParameter("@arg_Frequency5", Frequency5);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                //if (BankMICR != "")
                //    data.AddImageParameter("@arg_BankMICR", BankMICR);

                data.SubmitData("usp_ADM_AddParent", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddParent(int Id, string HouseholdID, string LName, string FName, string MI, string SSN, string ParentalStatus, float Income1, float Income2, float Income3, float Income4, float Income5, float TotalIncome, string Frequency1, string Frequency2, string Frequency3, string Frequency4, string Frequency5, int District_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (HouseholdID != "")
                    data.AddStringParameter("@arg_HouseholdID", HouseholdID);
                if (LName != "")
                    data.AddStringParameter("@arg_LName", LName);
                if (FName != "")
                    data.AddStringParameter("@arg_FName", FName);
                if (MI != "")
                    data.AddStringParameter("@arg_MI", MI);
                if (SSN != "")
                    data.AddStringParameter("@arg_SSN", SSN);
                if (ParentalStatus != "")
                    data.AddStringParameter("@arg_ParentalStatus", ParentalStatus);
                if (Income1 != 0)
                    data.AddFloatParameter("@arg_Income1", Income1);
                if (Income2 != 0)
                    data.AddFloatParameter("@arg_Income2", Income2);
                if (Income3 != 0)
                    data.AddFloatParameter("@arg_Income3", Income3);
                if (Income4 != 0)
                    data.AddFloatParameter("@arg_Income4", Income4);
                if (Income5 != 0)
                    data.AddFloatParameter("@arg_Income5", Income5);
                if (Frequency1 != "")
                    data.AddStringParameter("@arg_Frequency1", Frequency1);
                if (Frequency2 != "")
                    data.AddStringParameter("@arg_Frequency2", Frequency2);
                if (Frequency3 != "")
                    data.AddStringParameter("@arg_Frequency3", Frequency3);
                if (Frequency4 != "")
                    data.AddStringParameter("@arg_Frequency4", Frequency4);
                if (Frequency5 != "")
                    data.AddStringParameter("@arg_Frequency5", Frequency5);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                //if (BankMICR != "")
                //    data.AddImageParameter("@arg_BankMICR", BankMICR);

                data.SubmitData("usp_ADM_AddParent", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelParent(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelParent", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static SchoolsData PopulateSchoolDataFromReader(SafeDataReader reader)
        {
            SchoolsData sd = new SchoolsData();

            sd.Id = reader.GetInt32("Id");
            //sd.District = new NameValuePair(reader.GetString("DistrictName"), reader.GetInt32("Id"));
            sd.District = new NameValuePair(reader.GetString("DistrictName").Trim(), reader.GetInt32("District_Id"));
            sd.SchoolId = reader.GetString("SchoolID");
            sd.SchoolName = reader.GetString("SchoolName").Trim();
            sd.Address1 = reader.GetString("Address1").Trim();
            sd.Address2 = reader.GetString("Address2").Trim();
            sd.City = reader.GetString("City").Trim();
            sd.State = reader.GetString("State").Trim();
            sd.Zip = reader.GetString("Zip").Trim();

            // WaheedM [26.09.2013] 
            sd.IsPreorderTaxable = reader.GetBoolean("IsPreorderTaxable");
            sd.IsEasyPayTaxable = reader.GetBoolean("IsEasyPayTaxable");

            return sd;
        }

        #endregion

        #endregion
        #endregion

        #region School
        #region Private Functions

        private static SchoolOptionsData PopulateSchoolOptionsDataFromReader(SafeDataReader reader)
        {
            SchoolOptionsData sod = new SchoolOptionsData();

            sod.Id = reader.GetInt32("Id");
            sod.School = new NameValuePair(reader.GetString("SchoolName").Trim(), reader.GetInt32("School_Id"));
            sod.ChangedDate = reader.GetSmartDate("ChangedDate");
            sod.AlaCarteLimit = reader.GetDouble("AlaCarteLimit");
            sod.MealPlanLimit = reader.GetDouble("MealPlanLimit");
            sod.DoPinPreFix = reader.GetBoolean("DoPinPreFix");
            sod.PinPreFix = reader.GetString("PinPreFix").Trim();
            sod.PhotoLogging = reader.GetBoolean("PhotoLogging");
            sod.FingerPrinting = reader.GetBoolean("FingerPrinting");
            sod.BarCodeLength = reader.GetInt32("BarCodeLength");
            sod.StartSchoolYear = reader.GetSmartDate("StartSchoolYear");
            sod.EndSchoolYear = reader.GetSmartDate("EndSchoolYear");
            sod.DistrictExecutive = reader.GetBoolean("DistrictExecutive");
            sod.DistrictDates = reader.GetBoolean("DistrictDates");

            return sod;
        }

        private static EditCheckWData PopulateEditCheckWDataFromReader(SafeDataReader reader)
        {
            EditCheckWData ecd = new EditCheckWData();

            ecd.Id = reader.GetInt32("Id");
            ecd.EmpPreparedBy = new NameValuePair(reader.GetString("LoginName").Trim(), reader.GetInt32("Emp_PreparedBy_Id"));
            ecd.School = new NameValuePair(reader.GetString("SchoolName").Trim(), reader.GetInt32("School_Id"));
            ecd.ReportDate = reader.GetSmartDate("ReportDate");
            ecd.PreparedDate = reader.GetSmartDate("PreparedDate");
            ecd.AttendenceFactor = reader.GetDouble("AttendenceFactor");

            return ecd;
        }

        #endregion

        #region Public Function

        public static SchoolOptionsData[] GetSchoolOptionsBySchoolID(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<SchoolOptionsData> sodlist = new List<SchoolOptionsData>();
            try
            {
                data.AddIntParameter("@arg_School_Id", pSchool_Id);
                reader = data.GetDataReader("usp_ADM_GetSchoolOptionsBySchoolID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    SchoolOptionsData sod = PopulateSchoolOptionsDataFromReader(reader);
                    sodlist.Add(sod);
                }
                return sodlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static SchoolOptionsData[] GetSchoolOptionsBySchoolOptionsID(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<SchoolOptionsData> sodlist = new List<SchoolOptionsData>();
            try
            {
                data.AddIntParameter("@arg_SchoolOptions_Id", Id);
                reader = data.GetDataReader("usp_ADM_GetSchoolOptionsBySchoolOptionsID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    SchoolOptionsData sod = PopulateSchoolOptionsDataFromReader(reader);
                    sodlist.Add(sod);
                }
                return sodlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static EditCheckWData[] GetEditCheckWBySchoolID()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EditCheckWData> ecdlist = new List<EditCheckWData>();
            try
            {
                //data.AddIntParameter("@arg_School_Id", pSchool_Id);
                reader = data.GetDataReader("usp_ADM_GetEditCheckWBySchoolID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    EditCheckWData ecd = PopulateEditCheckWDataFromReader(reader);
                    ecdlist.Add(ecd);
                }
                return ecdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static EditCheckEligData[] GetEditCheckEligBySchoolID(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EditCheckEligData> ecdlist = new List<EditCheckEligData>();
            try
            {
                data.AddIntParameter("@arg_School_Id", pSchool_Id);
                reader = data.GetDataReader("usp_ADM_GetEditCheckEligBySchoolID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    EditCheckEligData ecd = PopulateEditCheckEligDataFromReader(reader);
                    ecdlist.Add(ecd);
                }
                return ecdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Add,Del,Chg

        #region School Options

        public static int AddSchoolOptions(int School_Id, SmartDate ChangedDate, float AlaCarteLimit, float MealPlanLimit, bool DoPinPreFix, string PinPreFix, bool PhotoLogging, bool FingerPrinting, int BarCodeLength, SmartDate StartSchoolYear, SmartDate EndSchoolYear, bool DistrictExecutive, bool DistrictDates)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                if (ChangedDate != new SmartDate())
                    data.AddDateParameter("@arg_ChangedDate", ChangedDate);
                //if (AlaCarteLimit != 0)
                data.AddFloatParameter("@arg_AlaCarteLimit", AlaCarteLimit);
                //if (MealPlanLimit != 0)
                data.AddFloatParameter("@arg_MealPlanLimit", MealPlanLimit);
                //if (DoPinPreFix != false)
                data.AddBoolParameter("@arg_DoPinPreFix", DoPinPreFix);
                if (PinPreFix == "")
                {
                    PinPreFix = "0000";
                    data.AddStringParameter("@arg_PinPreFix", PinPreFix);
                }
                else
                {
                    data.AddStringParameter("@arg_PinPreFix", PinPreFix);
                }
                // if (PhotoLogging != false)
                data.AddBoolParameter("@arg_PhotoLogging", PhotoLogging);
                // if (FingerPrinting != false)
                data.AddBoolParameter("@arg_FingerPrinting", FingerPrinting);
                // if (BarCodeLength != 0)
                data.AddIntParameter("@arg_BarCodeLength", BarCodeLength);
                if (StartSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_StartSchoolYear", StartSchoolYear);
                if (EndSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_EndSchoolYear", EndSchoolYear);
                //if (DistrictExecutive != false)
                data.AddBoolParameter("@arg_DistrictExecutive", DistrictExecutive);

                data.SubmitData("usp_ADM_AddSchoolOptions", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgSchoolOptions(int Id, int School_Id, SmartDate ChangedDate, float AlaCarteLimit, float MealPlanLimit, bool DoPinPreFix, string PinPreFix, bool PhotoLogging, bool FingerPrinting, int BarCodeLength, SmartDate StartSchoolYear, SmartDate EndSchoolYear, bool DistrictExecutive, bool DistrictDates)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                if (ChangedDate != new SmartDate())
                    data.AddDateParameter("@arg_ChangedDate", ChangedDate);
                //if (AlaCarteLimit != 0)
                data.AddFloatParameter("@arg_AlaCarteLimit", AlaCarteLimit);
                //if (MealPlanLimit != 0)
                data.AddFloatParameter("@arg_MealPlanLimit", MealPlanLimit);
                //if (DoPinPreFix != false)
                data.AddBoolParameter("@arg_DoPinPreFix", DoPinPreFix);
                if (PinPreFix == "")
                {
                    PinPreFix = "0000";
                    data.AddStringParameter("@arg_PinPreFix", PinPreFix);
                }
                else
                {
                    data.AddStringParameter("@arg_PinPreFix", PinPreFix);
                }
                //if (PhotoLogging != false)
                data.AddBoolParameter("@arg_PhotoLogging", PhotoLogging);
                //if (FingerPrinting != false)
                data.AddBoolParameter("@arg_FingerPrinting", FingerPrinting);
                //if (BarCodeLength != 0)
                data.AddIntParameter("@arg_BarCodeLength", BarCodeLength);
                if (StartSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_StartSchoolYear", StartSchoolYear);
                if (EndSchoolYear != new SmartDate())
                    data.AddDateParameter("@arg_EndSchoolYear", EndSchoolYear);
                //if (DistrictExecutive != false)
                data.AddBoolParameter("@arg_DistrictExecutive", DistrictExecutive);

                data.SubmitData("usp_ADM_ChgSchoolOptions", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelSchoolOptions(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelSchoolOptions", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region EditCheckW

        public static int AddEditCheckW(int Emp_PreparedBy_Id, int School_Id, SmartDate ReportDate, SmartDate PreparedDate, float AttendenceFactor)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Emp_PreparedBy_Id != 0)
                    data.AddIntParameter("@arg_Emp_PreparedBy_Id", Emp_PreparedBy_Id);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                if (ReportDate != new SmartDate())
                    data.AddDateParameter("@arg_ReportDate", ReportDate);
                if (PreparedDate != new SmartDate())
                    data.AddDateParameter("@arg_PreparedDate", PreparedDate);
                if (AttendenceFactor != 0)
                    data.AddFloatParameter("@arg_AttendenceFactor", AttendenceFactor);

                data.SubmitData("usp_ADM_AddEditCheckW", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgEditCheckW(int Id, int Emp_PreparedBy_Id, int School_Id, SmartDate ReportDate, SmartDate PreparedDate, float AttendenceFactor)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Emp_PreparedBy_Id != 0)
                    data.AddIntParameter("@arg_Emp_PreparedBy_Id", Emp_PreparedBy_Id);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                if (ReportDate != new SmartDate())
                    data.AddDateParameter("@arg_ReportDate", ReportDate);
                if (PreparedDate != new SmartDate())
                    data.AddDateParameter("@arg_PreparedDate", PreparedDate);
                if (AttendenceFactor != 0)
                    data.AddFloatParameter("@arg_AttendenceFactor", AttendenceFactor);

                data.SubmitData("usp_ADM_ChgEditCheckW", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelEditCheckW(int pId)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", pId);
                data.SubmitData("usp_ADM_DelEditCheckW", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #endregion

        #endregion

        #region EditCheckElig and EditCheckEligAssigned

        private static EditCheckEligData PopulateEditCheckEligDataFromReader(SafeDataReader reader)
        {
            EditCheckEligData eced = new EditCheckEligData();

            eced.Date = reader.GetSmartDate("Date");
            eced.School = reader.GetInt32("School_Id");
            eced.FreeElig = reader.GetInt32("FreeElig");
            eced.RedElig = reader.GetInt32("RedElig");
            eced.PaidElig = reader.GetInt32("PaidElig");
            eced.FreeClaimed = reader.GetInt32("FreeClaimed");
            eced.RedClaimed = reader.GetInt32("RedClaimed");
            eced.PaidClaimed = reader.GetInt32("PaidClaimed");

            return eced;
        }

        private static EditCheckEligAssignedData PopulateEditCheckEligAssignedDataFromReader(SafeDataReader reader)
        {
            EditCheckEligAssignedData eced1 = new EditCheckEligAssignedData();

            eced1.Date = reader.GetSmartDate("Date");
            eced1.School = reader.GetInt32("School_Id");
            eced1.FreeEligA = reader.GetInt32("FreeEligA");
            eced1.RedEligA = reader.GetInt32("RedEligA");
            eced1.PaidEligA = reader.GetInt32("PaidEligA");
            eced1.FreeClaimedA = reader.GetInt32("FreeClaimedA");
            eced1.RedClaimedA = reader.GetInt32("RedClaimedA");
            eced1.PaidClaimedA = reader.GetInt32("PaidClaimedA");

            return eced1;
        }

        public static void AddEditCheckElig(SmartDate date, int School_Id, int FreeElig, int RedElig, int PaidElig, int FreeClaimed, int RedClaimed, int PaidClaimed)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (date != new SmartDate())
                    data.AddDateParameter("@arg_Date", date);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                //if (FreeElig != 0)
                data.AddIntParameter("@arg_FreeElig", FreeElig);
                //if (RedElig != 0)
                data.AddIntParameter("@arg_RedElig", RedElig);
                //if (PaidElig != 0)
                data.AddIntParameter("@arg_PaidElig", PaidElig);
                data.AddIntParameter("@arg_FreeClaimed", FreeClaimed);
                data.AddIntParameter("@arg_RedClaimed", RedClaimed);
                data.AddIntParameter("@arg_PaidClaimed", PaidClaimed);

                data.SubmitData("usp_ADM_AddEditCheckElig", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddTempEditCheckElig(SmartDate date, int School_Id, int FreeElig, int RedElig, int PaidElig, int FreeClaimed, int RedClaimed, int PaidClaimed)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (date != new SmartDate())
                    data.AddDateParameter("@arg_Date", date);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                //if (FreeElig != 0)
                data.AddIntParameter("@arg_FreeElig", FreeElig);
                //if (RedElig != 0)
                data.AddIntParameter("@arg_RedElig", RedElig);
                //if (PaidElig != 0)
                data.AddIntParameter("@arg_PaidElig", PaidElig);
                data.AddIntParameter("@arg_FreeClaimed", FreeClaimed);
                data.AddIntParameter("@arg_RedClaimed", RedClaimed);
                data.AddIntParameter("@arg_PaidClaimed", PaidClaimed);

                data.SubmitData("usp_ADM_AddTempEditCheckElig", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgEditCheckElig(SmartDate date, int School_Id, int FreeElig, int RedElig, int PaidElig)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (date != new SmartDate())
                    data.AddDateParameter("@arg_Date", date);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                if (FreeElig != 0)
                    data.AddIntParameter("@arg_FreeElig", FreeElig);
                if (RedElig != 0)
                    data.AddIntParameter("@arg_RedElig", RedElig);
                if (PaidElig != 0)
                    data.AddIntParameter("@arg_PaidElig", PaidElig);

                data.SubmitData("usp_ADM_ChgEditCheckElig", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelEditCheckElig(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_School_Id", pSchool_Id);
                data.SubmitData("usp_ADM_DelEditCheckElig", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        //////////////////////////////For EditChecksheetReport

        public static int GetCountForFreeStudents(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count (*)  as FreeCount from Customers c inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id  where c.iSStudent = 'true' and c.LunchType = 3 and s.Id='" + pSchool_Id + "' and cs.isPrimary='true'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("FreeCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForReducedStudents(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count (*)  as ReducedCount from Customers c inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id  where c.iSStudent = 'true' and c.LunchType = 2 and s.Id='" + pSchool_Id + "' and cs.isPrimary='true'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("ReducedCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForPaidStudents(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count (*)  as PaidCount from Customers c inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id  where c.iSStudent = 'true' and c.LunchType = 1 and s.Id='" + pSchool_Id + "' and cs.isPrimary='true'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("PaidCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForFreeClaimedStudents(int pSchool_Id, int pCategoryId, SmartDate pStartDtae, SmartDate pEndDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select  count (*)  as FreeClaimedCount  from Orders o inner join  Customers c on o.Customer_Id=c.Id inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id inner join Items i on i.Order_Id=o.id inner join Menu m on m.id= i.Menu_Id inner join Category cat on cat.Id=m.Category_Id inner join CategoryTypes ctype on ctype.id= cat.CategoryType_Id where c.iSStudent = 'true' and c.LunchType = 3 and s.Id='" + pSchool_Id + "' and cs.isPrimary='true' and cat.id=" + pCategoryId + " and o.OrderDate >=" + pStartDtae + " and o.OrderDate <=" + pEndDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("FreeClaimedCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForReducedClaimedStudents(int pSchool_Id, int pCategoryId, SmartDate pStartDtae, SmartDate pEndDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select  count (*)  as ReducedClaimedCount  from Orders o inner join  Customers c on o.Customer_Id=c.Id inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id inner join Items i on i.Order_Id=o.id inner join Menu m on m.id= i.Menu_Id inner join Category cat on cat.Id=m.Category_Id inner join CategoryTypes ctype on ctype.id= cat.CategoryType_Id where c.iSStudent = 'true' and c.LunchType = 2 and s.Id='" + pSchool_Id + "' and cs.isPrimary='true' and cat.id=" + pCategoryId + " and o.OrderDate >=" + pStartDtae + " and o.OrderDate <=" + pEndDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("ReducedClaimedCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForPaidClaimedStudents(int pSchool_Id, int pCategoryId, SmartDate pStartDtae, SmartDate pEndDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select  count (*)  as PiadClaimedCount  from Orders o inner join  Customers c on o.Customer_Id=c.Id inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id inner join Items i on i.Order_Id=o.id inner join Menu m on m.id= i.Menu_Id inner join Category cat on cat.Id=m.Category_Id inner join CategoryTypes ctype on ctype.id= cat.CategoryType_Id where c.iSStudent = 'true' and c.LunchType = 1 and s.Id='" + pSchool_Id + "' and cs.isPrimary='true' and cat.id=" + pCategoryId + " and o.OrderDate >=" + pStartDtae + " and o.OrderDate <=" + pEndDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("PiadClaimedCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static EditCheckEligData[] GetEditCheckElig()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EditCheckEligData> cdlist = new List<EditCheckEligData>();
            try
            {
                reader = data.GetDataReader("usp_ADM_GetEditCheckElig", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    EditCheckEligData cd = PopulateEditCheckEligDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetEditCheckEligByDate(SmartDate pDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as GetEditCheckEligByDateCount from EditCheckElig where Date=" + pDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("GetEditCheckEligByDateCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelTempEditCheckElig()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                string strSQL = "delete from TempEditCheckElig";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        ///////////////////////////////////For Assigned School EditCheckSheet Report

        public static void AddEditCheckEligAssigned(SmartDate date, int School_Id, int FreeEligA, int RedEligA, int PaidEligA, int FreeClaimedA, int RedClaimedA, int PaidClaimedA)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (date != new SmartDate())
                    data.AddDateParameter("@arg_Date", date);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                //if (FreeElig != 0)
                data.AddIntParameter("@arg_FreeEligA", FreeEligA);
                //if (RedElig != 0)
                data.AddIntParameter("@arg_RedEligA", RedEligA);
                //if (PaidElig != 0)
                data.AddIntParameter("@arg_PaidEligA", PaidEligA);
                data.AddIntParameter("@arg_FreeClaimedA", FreeClaimedA);
                data.AddIntParameter("@arg_RedClaimedA", RedClaimedA);
                data.AddIntParameter("@arg_PaidClaimedA", PaidClaimedA);

                data.SubmitData("usp_ADM_AddEditCheckEligAssigned", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddTempEditCheckEligAssigned(SmartDate date, int School_Id, int FreeEligA, int RedEligA, int PaidEligA, int FreeClaimedA, int RedClaimedA, int PaidClaimedA)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (date != new SmartDate())
                    data.AddDateParameter("@arg_Date", date);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                //if (FreeElig != 0)
                data.AddIntParameter("@arg_FreeEligA", FreeEligA);
                //if (RedElig != 0)
                data.AddIntParameter("@arg_RedEligA", RedEligA);
                //if (PaidElig != 0)
                data.AddIntParameter("@arg_PaidEligA", PaidEligA);
                data.AddIntParameter("@arg_FreeClaimedA", FreeClaimedA);
                data.AddIntParameter("@arg_RedClaimedA", RedClaimedA);
                data.AddIntParameter("@arg_PaidClaimedA", PaidClaimedA);

                data.SubmitData("usp_ADM_AddTempEditCheckEligAssigned", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForFreeStudentsA(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count (*)  as FreeCountA from Customers c inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id  where c.iSStudent = 'true' and c.LunchType = 3 and s.Id='" + pSchool_Id + "' and cs.isPrimary='false'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("FreeCountA");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForReducedStudentsA(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count (*)  as ReducedCountA from Customers c inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id  where c.iSStudent = 'true' and c.LunchType = 2 and s.Id='" + pSchool_Id + "' and cs.isPrimary='false'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("ReducedCountA");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForPaidStudentsA(int pSchool_Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count (*)  as PaidCountA from Customers c inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id  where c.iSStudent = 'true' and c.LunchType = 1 and s.Id='" + pSchool_Id + "' and cs.isPrimary='false'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("PaidCountA");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForFreeClaimedStudentsA(int pSchool_Id, int pCategoryId, SmartDate pStartDtae, SmartDate pEndDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select  count (*)  as FreeClaimedCountA  from Orders o inner join  Customers c on o.Customer_Id=c.Id inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id inner join Items i on i.Order_Id=o.id inner join Menu m on m.id= i.Menu_Id inner join Category cat on cat.Id=m.Category_Id inner join CategoryTypes ctype on ctype.id= cat.CategoryType_Id where c.iSStudent = 'true' and c.LunchType = 3 and s.Id='" + pSchool_Id + "' and cs.isPrimary='false' and cat.id=" + pCategoryId + " and o.OrderDate >=" + pStartDtae + " and o.OrderDate <=" + pEndDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("FreeClaimedCountA");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForReducedClaimedStudentsA(int pSchool_Id, int pCategoryId, SmartDate pStartDtae, SmartDate pEndDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select  count (*)  as ReducedClaimedCountA  from Orders o inner join  Customers c on o.Customer_Id=c.Id inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id inner join Items i on i.Order_Id=o.id inner join Menu m on m.id= i.Menu_Id inner join Category cat on cat.Id=m.Category_Id inner join CategoryTypes ctype on ctype.id= cat.CategoryType_Id where c.iSStudent = 'true' and c.LunchType = 2 and s.Id='" + pSchool_Id + "' and cs.isPrimary='false' and cat.id=" + pCategoryId + " and o.OrderDate >=" + pStartDtae + " and o.OrderDate <=" + pEndDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("ReducedClaimedCountA");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForPaidClaimedStudentsA(int pSchool_Id, int pCategoryId, SmartDate pStartDtae, SmartDate pEndDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select  count (*)  as PiadClaimedCountA  from Orders o inner join  Customers c on o.Customer_Id=c.Id inner join Customer_School cs on c.id=cs.Customer_Id inner join Schools s on s.id=cs.School_Id inner join Items i on i.Order_Id=o.id inner join Menu m on m.id= i.Menu_Id inner join Category cat on cat.Id=m.Category_Id inner join CategoryTypes ctype on ctype.id= cat.CategoryType_Id where c.iSStudent = 'true' and c.LunchType = 1 and s.Id='" + pSchool_Id + "' and cs.isPrimary='false' and cat.id=" + pCategoryId + " and o.OrderDate >=" + pStartDtae + " and o.OrderDate <=" + pEndDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("PiadClaimedCountA");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static EditCheckEligAssignedData[] GetEditCheckEligAssigned()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EditCheckEligAssignedData> cdlist = new List<EditCheckEligAssignedData>();
            try
            {
                reader = data.GetDataReader("usp_ADM_GetEditCheckEligAssigned", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    EditCheckEligAssignedData cd = PopulateEditCheckEligAssignedDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetEditCheckEligAssignedByDate(SmartDate pDate)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as GetEditCheckEligAssignedByDateCount from EditCheckEligAssigned where Date=" + pDate;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("GetEditCheckEligAssignedByDateCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelTempEditCheckEligAssigned()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                string strSQL = "delete from TempEditCheckEligAssigned";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Customer

        #region Private Function
        #region CustomerData
        private static CustomersData PopulateCustomersDataFromReader(SafeDataReader reader)
        {
            CustomersData cd = new CustomersData();

            cd.CustomerID = reader.GetInt32("Id");
            cd.District = new NameValuePair(reader.GetString("DistrictName").Trim(), reader.GetInt32("District_Id"));
          //  cd.Language = new NameValuePair(reader.GetString("LanguageName").Trim(), reader.GetInt32("Language_Id"));
          //  cd.Grade = new NameValuePair(reader.GetString("GradeName").Trim(), reader.GetInt32("Grade_Id"));
         //   cd.HomeRoom = new NameValuePair(reader.GetString("HomeRoomName").Trim(), reader.GetInt32("HomeRoom_Id"));
            cd.isStudent = reader.GetBoolean("isStudent");
            cd.UserID = reader.GetString("UserID").Trim();
            cd.PIN = reader.GetString("PIN").Trim();
            cd.LastName = reader.GetString("LastName").Trim();
            cd.FirstName = reader.GetString("FirstName").Trim();
            cd.Middle = reader.GetString("Middle").Trim();
            cd.Gender = reader.GetString("Gender").Trim();
            cd.SSN = reader.GetString("SSN").Trim();
            cd.Address1 = reader.GetString("Address1").Trim();
            cd.Address2 = reader.GetString("Address2").Trim();
            cd.City = reader.GetString("City").Trim();
            cd.State = reader.GetString("State").Trim();
            cd.Zip = reader.GetString("Zip").Trim();
            cd.Phone = reader.GetString("Phone").Trim();
            cd.LunchType = reader.GetInt32("LunchType");
            cd.AllowAlaCarte = reader.GetBoolean("AllowAlaCarte");
            cd.CashOnly = reader.GetBoolean("CashOnly");
            cd.isActive = reader.GetBoolean("isActive");
            cd.GraduationDate = reader.GetSmartDate("GraduationDate");
            cd.SchoolDat = reader.GetString("SchoolDat");
            cd.isDeleted = reader.GetBoolean("isDeleted");
            cd.ExtraInfo = reader.GetString("ExtraInfo").Trim();
            cd.EMail = reader.GetString("EMail").Trim();
            cd.DOB = reader.GetSmartDate("DOB");
            cd.ACH = reader.GetBoolean("ACH");
            cd.isSnack = reader.GetBoolean("isSnack");
            cd.isStudentWorker = reader.GetBoolean("isStudentWorker");

            //dd.BankMICR = reader.geto
            return cd;
        }
        #endregion

        #region PicturesData
        private static PicturesData PopulatePicturesDataFromReader(SafeDataReader reader)
        {
            PicturesData pd = new PicturesData();

            pd.Id = reader.GetInt32("Id");
            NameValuePair[] nvpcon = new NameValuePair[3];
            nvpcon[0].Value = reader.GetInt32("Customer_Id");
            nvpcon[0].Name = reader.GetString("FirstName").ToString().Trim();
            nvpcon[1].Value = reader.GetInt32("Customer_Id");
            nvpcon[1].Name = reader.GetString("LastName").ToString().Trim();
            nvpcon[2].Value = reader.GetInt32("Customer_Id");
            nvpcon[2].Name = reader.GetString("Middle").ToString().Trim();
            pd.Customer = new NameValuePairCollection(nvpcon);
            pd.Picture = reader.GetValue("Picture");
            pd.PictureType = reader.GetString("PictureType").Trim();

            return pd;

        }

        #endregion

        #region ChargeCountsData

        private static ChargeCountsData PopulateChargeCountsDataFromReader(SafeDataReader reader)
        {
            ChargeCountsData ccd = new ChargeCountsData();

            ccd.Id = reader.GetInt32("Id");
            NameValuePair[] nvpcon = new NameValuePair[3];
            nvpcon[0].Value = reader.GetInt32("Id");
            nvpcon[0].Name = reader.GetString("FirstName").ToString().Trim();
            nvpcon[1].Value = reader.GetInt32("Id");
            nvpcon[1].Name = reader.GetString("LastName").ToString().Trim();
            nvpcon[2].Value = reader.GetInt32("Id");
            nvpcon[2].Name = reader.GetString("Middle").ToString().Trim();
            ccd.Customer = new NameValuePairCollection(nvpcon);
            //pd.Picture = reader.geto
            ccd.SDate = reader.GetSmartDate("SDate");
            ccd.EDate = reader.GetSmartDate("EDate");
            ccd.WLetter1 = reader.GetSmartDate("WLetter1");
            ccd.WLetter2 = reader.GetSmartDate("WLetter2");
            ccd.WLetter3 = reader.GetSmartDate("WLetter3");

            return ccd;
        }

        #endregion

        #region BiometricsData

        private static BiometricsData PopulateBiometricsDataFromReader(SafeDataReader reader)
        {
            BiometricsData bd = new BiometricsData();
            bd.Customer_Id = reader.GetInt32("Customer_Id");
            //bd.Finger = reader.geti

            return bd;
        }

        #endregion

        #region CustomerLogData

        private static CustomerLogData PopulateCustomerLogDataFromReader(SafeDataReader reader)
        {
            CustomerLogData cld = new CustomerLogData();
            cld.Id = reader.GetInt32("Id");
            NameValuePair[] nvpcus = new NameValuePair[2];
            nvpcus[0].Value = reader.GetInt32("Customer_Id");
            nvpcus[0].Name = reader.GetString("FirstName").Trim();
            nvpcus[1].Value = reader.GetInt32("Customer_Id");
            nvpcus[1].Name = reader.GetString("LastName").Trim();
            cld.Customer = new NameValuePairCollection(nvpcus);

            NameValuePair[] nvpemp = new NameValuePair[2];
            nvpemp[0].Value = reader.GetInt32("Emp_Changed_Id");
            nvpemp[0].Name = reader.GetString("EmpFirstName").Trim();
            nvpemp[1].Value = reader.GetInt32("Emp_Changed_Id");
            nvpemp[1].Name = reader.GetString("EmpLastName").Trim();
            cld.Employee = new NameValuePairCollection(nvpemp);

            cld.ChangedDate = reader.GetSmartDate("ChangedDate");
            cld.Notes = reader.GetString("Notes").Trim();
            cld.Comment = reader.GetString("Comment").Trim();

            return cld;
        }

        #endregion

        #region AccountInfoData

        private static AccountInfoData PopulateAccountInfoDataFromReader(SafeDataReader reader)
        {
            AccountInfoData aid = new AccountInfoData();

            aid.Id = reader.GetInt32("Id");
            NameValuePair[] nvpcon = new NameValuePair[3];
            nvpcon[0].Value = reader.GetInt32("Customer_Id");
            nvpcon[0].Name = reader.GetString("FirstName").Trim();
            nvpcon[1].Value = reader.GetInt32("Customer_Id");
            nvpcon[1].Name = reader.GetString("LastName").Trim();
            nvpcon[2].Value = reader.GetInt32("Customer_Id");
            nvpcon[2].Name = reader.GetString("Middle").Trim();
            aid.Customer = new NameValuePairCollection(nvpcon);
            //pd.Picture = reader.geto
            aid.ABalance = reader.GetDecimal("ABalance");
            aid.MBalance = reader.GetDecimal("MBalance");

            return aid;
        }

        #endregion

        #region Customer_School

        private static CustomerSchoolData PopulateCustomerSchoolDataFromReader(SafeDataReader reader)
        {
            CustomerSchoolData csd = new CustomerSchoolData();
            csd.CustID = reader.GetInt32("CustID");
            NameValuePair[] nvpcon = new NameValuePair[4];
            nvpcon[0].Value = reader.GetInt32("Customer_Id");
            nvpcon[0].Name = reader.GetString("FirstName").ToString().Trim();
            nvpcon[1].Value = reader.GetInt32("Customer_Id");
            nvpcon[1].Name = reader.GetString("LastName").ToString().Trim();
            nvpcon[2].Value = reader.GetInt32("Customer_Id");
            nvpcon[2].Name = reader.GetString("Middle").ToString().Trim();
            nvpcon[3].Value = reader.GetInt32("Customer_Id");
            nvpcon[3].Name = reader.GetString("UserID").ToString().Trim();
            csd.Customer = new NameValuePairCollection(nvpcon);
            //pd.Picture = reader.geto
            csd.School = new NameValuePair(reader.GetString("SchoolName").Trim(), reader.GetInt32("School_Id"));
            csd.Grades = new NameValuePair(reader.GetString("GradeName").Trim(), reader.GetInt32("Grade_Id"));
            //csd.Homeroom = new NameValuePair(reader.GetString("HomeroomName"), reader.GetInt32("Homeroom_Id"));
            csd.isPrimary = reader.GetBoolean("isPrimary");
            return csd;
        }


        #endregion

        #endregion

        #region Public Function
        #region CustomerData

        public static CustomersData[] ListCustomers(int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomersData> cdList = new List<CustomersData>();

            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetCustomers", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomersData cd = PopulateCustomersDataFromReader(reader);
                    cdList.Add(cd);

                }
                return cdList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForCustomerUserID(string userid)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as CustUserIDCount from Customers where UserID='" + userid + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("CustUserIDCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForCustomerPincode(string pincode)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as PincodeCount from Customers where PIN='" + pincode + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("PincodeCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomersData[] GetCustomersByCustomerID(int pId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomersData> cdlist = new List<CustomersData>();
            try
            {
                data.AddIntParameter("@arg_Id", pId);
                reader = data.GetDataReader("usp_ADM_GetCustomersByCustomerID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomersData cd = PopulateCustomersDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomersData[] GetCustomersByHomeroomID(int pHomeroomId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomersData> cdlist = new List<CustomersData>();
            try
            {
                data.AddIntParameter("@arg_HomeroomId", pHomeroomId);
                reader = data.GetDataReader("usp_ADM_GetCustomersByHomeroomID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomersData cd = PopulateCustomersDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomersData[] GetCustomersByGradeID(int pGradeId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomersData> cdlist = new List<CustomersData>();
            try
            {
                data.AddIntParameter("@arg_GradeId", pGradeId);
                reader = data.GetDataReader("usp_ADM_GetCustomersByGradeID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomersData cd = PopulateCustomersDataFromReader(reader);
                    cdlist.Add(cd);
                }
                return cdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomersData[] ListCustomersBySearch(string pKeyword, int pSchool_Id, int pHomeRoom_Id, int pGrade_Id, bool pAllSchool, bool pIsStudent, bool pIsActive, string pUserId, string pPIN, string pLastName, string pFirstName, int pPageIndex, int pPageSize, string pUserIDType, string pPINType, string pLastNameType, string pFirstNameType)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomersData> cdList = new List<CustomersData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                if (pSchool_Id != 0)
                    data.AddIntParameter("@arg_School_Id", pSchool_Id);
                if (pHomeRoom_Id != 0)
                    data.AddIntParameter("@arg_HomeRoom_Id", pHomeRoom_Id);
                if (pGrade_Id != 0)
                    data.AddIntParameter("@arg_Grade_Id", pGrade_Id);
                if (pAllSchool == true)
                    data.AddBoolParameter("@arg_IsAllStudents", pAllSchool);
                if (pIsStudent == true)
                    data.AddBoolParameter("@arg_IsStudentsOnly", pIsStudent);
                if (pIsActive == true)
                    data.AddBoolParameter("@arg_IsActiveOnly", pIsActive);
                if (pUserId != "")
                {
                    if (pUserIDType == "Begins")
                    {
                        data.AddStringParameter("@arg_UserId", pUserId + "%");
                    }
                    else
                        data.AddStringParameter("@arg_UserId", pUserId);
                }
                //else
                //    data.AddStringParameter("@arg_UserId", pUserId);
                if (pPIN != "")
                    if (pPINType == "Begins")
                        data.AddStringParameter("@arg_PIN", pPIN + "%");
                    else
                        data.AddStringParameter("@arg_PIN", pPIN);
                //else
                //    data.AddStringParameter("@arg_PIN", pPIN);
                if (pLastName != "")
                    if (pLastNameType == "Begins")
                        data.AddStringParameter("@arg_LastName", pLastName + "%");
                    else
                        data.AddStringParameter("@arg_LastName", pLastName);
                //else
                //    data.AddStringParameter("@arg_LastName", pLastName);
                if (pFirstName != "")
                    if (pFirstNameType == "Begins")
                        data.AddStringParameter("@arg_FirstName", pFirstName + "%");
                    else
                        data.AddStringParameter("@arg_FirstName", pFirstName);
                //else
                //    data.AddStringParameter("@arg_FirstName", pFirstName);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);

                reader = data.GetDataReader("usp_ADM_GetCustomersBySearch", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomersData cd = PopulateCustomersDataFromReader(reader);
                    cdList.Add(cd);
                }
                return cdList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCustomersCountBySearch(string pKeyword, int pSchool_Id, int pHomeRoom_Id, int pGrade_Id, bool pAllSchool, bool pIsStudent, bool pIsActive, string pUserId, string pPIN, string pLastName, string pFirstName, string pUserIDType, string pPINType, string pLastNameType, string pFirstNameType)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                if (pSchool_Id != 0)
                    data.AddIntParameter("@arg_School_Id", pSchool_Id);
                if (pHomeRoom_Id != 0)
                    data.AddIntParameter("@arg_HomeRoom_Id", pHomeRoom_Id);
                if (pGrade_Id != 0)
                    data.AddIntParameter("@arg_Grade_Id", pGrade_Id);
                if (pAllSchool == true)
                    data.AddBoolParameter("@arg_IsAllStudents", pAllSchool);
                if (pIsStudent == true)
                    data.AddBoolParameter("@arg_IsStudentsOnly", pIsStudent);
                if (pIsActive == true)
                    data.AddBoolParameter("@arg_IsActiveOnly", pIsActive);
                if (pUserId != "")
                {
                    if (pUserIDType == "Begins")
                    {
                        data.AddStringParameter("@arg_UserId", pUserId + "%");
                    }
                    else
                        data.AddStringParameter("@arg_UserId", pUserId);
                }
                if (pPIN != "")
                    if (pPINType == "Begins")
                        data.AddStringParameter("@arg_PIN", pPIN + "%");
                    else
                        data.AddStringParameter("@arg_PIN", pPIN);
                if (pLastName != "")
                    if (pLastNameType == "Begins")
                        data.AddStringParameter("@arg_LastName", pLastName + "%");
                    else
                        data.AddStringParameter("@arg_LastName", pLastName);
                if (pFirstName != "")
                    if (pFirstNameType == "Begins")
                        data.AddStringParameter("@arg_FirstName", pFirstName + "%");
                    else
                        data.AddStringParameter("@arg_FirstName", pFirstName);

                reader = data.GetDataReader("usp_ADM_GetCustomersCountBySearch", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #region PicturesData

        public static PicturesData[] GetPicturesByCustomerId(int pCustomerID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<PicturesData> pdlist = new List<PicturesData>();
            try
            {
                data.AddIntParameter("@arg_Customer_Id", pCustomerID);
                reader = data.GetDataReader("usp_ADM_GetPicturesByCustomerId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    PicturesData pd = PopulatePicturesDataFromReader(reader);
                    pdlist.Add(pd);
                }
                return pdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region ChargeCountsData

        public static ChargeCountsData[] GetChargeCountsByCustomerId(int pCustomerID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<ChargeCountsData> ccdlist = new List<ChargeCountsData>();
            try
            {
                data.AddIntParameter("@arg_Customer_Id", pCustomerID);
                reader = data.GetDataReader("usp_ADM_GetChargeCountsByCustomerId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    ChargeCountsData ccd = PopulateChargeCountsDataFromReader(reader);
                    ccdlist.Add(ccd);
                }
                return ccdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #region BiometricsData

        public static BiometricsData[] GetBiometricsByCustomerId(int pCustomerID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<BiometricsData> bdlist = new List<BiometricsData>();
            try
            {
                data.AddIntParameter("@arg_Customer_Id", pCustomerID);
                reader = data.GetDataReader("usp_ADM_GetBiometricsByCustomerId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    BiometricsData bd = PopulateBiometricsDataFromReader(reader);
                    bdlist.Add(bd);
                }
                return bdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region CustomerLogData

        public static CustomerLogData[] GetCustomerLogByCustomerId(int pCustomerID, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerLogData> cldlist = new List<CustomerLogData>();
            try
            {
                if (pCustomerID != 0)
                    data.AddIntParameter("@arg_Customer_Id", pCustomerID);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetCustomerLogByCustomerId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomerLogData cld = PopulateCustomerLogDataFromReader(reader);
                    cldlist.Add(cld);
                }
                return cldlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCustomerLogCountByCustomerId(int pCustomerID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pCustomerID != 0)
                    data.AddIntParameter("@arg_Customer_Id", pCustomerID);

                reader = data.GetDataReader("usp_ADM_GetCustomerLogCountByCustomerId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listcount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region AccountInfoData

        public static AccountInfoData[] GetAccountInfoByCustomerId(int pCustomerID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<AccountInfoData> aidlist = new List<AccountInfoData>();
            try
            {
                data.AddIntParameter("@arg_Customer_Id", pCustomerID);
                reader = data.GetDataReader("usp_ADM_GetAccountInfoByCustomerId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    AccountInfoData aid = PopulateAccountInfoDataFromReader(reader);
                    aidlist.Add(aid);
                }
                return aidlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Customer_School

        public static CustomerSchoolData[] GetCustomerSchoolByCustomerId(int pCustomerID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerSchoolData> csdlist = new List<CustomerSchoolData>();
            try
            {
                data.AddIntParameter("@arg_Customer_Id", pCustomerID);
                reader = data.GetDataReader("usp_ADM_GetCustomerSchoolByCustomerId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    CustomerSchoolData csd = PopulateCustomerSchoolDataFromReader(reader);
                    csdlist.Add(csd);
                }
                return csdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static SchoolsData[] GetchgAvailableSchoolsBychgCustomerId(int DistId, int CustId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            SchoolsData sd = new SchoolsData();
            List<SchoolsData> sdlist = new List<SchoolsData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", DistId);
                data.AddIntParameter("@arg_Customer_id", CustId);
                reader = data.GetDataReader("usp_ADM_GetAvailableSchoolbyDistrictIDCustomerID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    sd = PopulateSchoolDataFromReader(reader);
                    sdlist.Add(sd);
                }
                return sdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static SchoolsData[] GetchgAssignedSchoolsBychgCustomerId(int DistId, int CustId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            SchoolsData sd = new SchoolsData();
            List<SchoolsData> sdlist = new List<SchoolsData>();
            try
            {
                data.AddIntParameter("@arg_District_Id", DistId);
                data.AddIntParameter("@arg_Customer_id", CustId);
                reader = data.GetDataReader("usp_ADM_GetAssignedSchoolbyDistrictIDCustomerID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    sd = PopulateSchoolDataFromReader(reader);
                    sdlist.Add(sd);
                }
                return sdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCustomerSchoolCount(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as CustomerSchoolCount from Customer_School where Customer_Id='" + Id + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("CustomerSchoolCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelAllCustomerSchool(int CustomerID, bool pisPrimary)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CustomerID", CustomerID);
                data.AddBoolParameter("@arg_isPrimary", pisPrimary);
                data.SubmitData("usp_ADM_DelALLCustomerSchool", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomerSchoolData[] GetCustomerSchoolBySchoolId(int pSchoolID, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerSchoolData> csdlist = new List<CustomerSchoolData>();
            try
            {

                data.AddIntParameter("@arg_School_Id", pSchoolID);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetCustomerSchoolsBySchoolId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomerSchoolData csd = PopulateCustomerSchoolDataFromReader(reader);
                    csdlist.Add(csd);
                }
                return csdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomerSchoolData[] GetCustomerSchoolBySchoolIdandGradeId(int pSchoolID, int pGradeID, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerSchoolData> csdlist = new List<CustomerSchoolData>();
            try
            {
                data.AddIntParameter("@arg_School_Id", pSchoolID);
                data.AddIntParameter("@arg_Grade_Id", pGradeID);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetCustomerSchoolsBySchoolIdandGradeId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomerSchoolData csd = PopulateCustomerSchoolDataFromReader(reader);
                    csdlist.Add(csd);
                }
                return csdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetBeginBalanceCount(int pSchoolID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerSchoolData> csdlist = new List<CustomerSchoolData>();
            try
            {
                data.AddIntParameter("@arg_School_Id", pSchoolID);
                reader = data.GetDataReader("usp_ADM_GetCustomerSchoolsBySchoolIdCount", DataPortal.QueryType.StoredProc);
                int count = 0;
                while (reader.Read())
                {
                    count = reader.GetInt32("fieldcount");
                }
                return count; ;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetGraduateCustomerCount(int pSchoolID, int pGradeID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerSchoolData> csdlist = new List<CustomerSchoolData>();
            try
            {
                data.AddIntParameter("@arg_School_Id", pSchoolID);
                data.AddIntParameter("@arg_Grade_Id", pGradeID);
                reader = data.GetDataReader("usp_ADM_GetCustomerSchoolsBySchoolIdandGradeIdCount", DataPortal.QueryType.StoredProc);
                int count = 0;
                while (reader.Read())
                {
                    count = reader.GetInt32("fieldcount");
                }
                return count; ;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #endregion

        #region Add,Del,Chg

        #region Customer
        public static int AddCustomer(int District_Id, int Language_Id, int Grade_Id, int HomeRoom_Id, bool isStudent, string UserID, string PIN, string LastName, string FirstName, string Middle, string Gender, string SSN, string Address1, string Address2, string City, string State, string Zip, string Phone, int LunchType, bool AllowAlaCarte, bool CashOnly, bool isActive, SmartDate GraduationDate, string SchoolDat, bool isDeleted, string ExtraInfo, string EMail, SmartDate DOB, bool ACH, bool isSnack, bool isStudentWorker)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Language_Id != 0)
                    data.AddIntParameter("@arg_Language_Id", Language_Id);
                if (Grade_Id != 0)
                    data.AddIntParameter("@arg_Grade_Id", Grade_Id);
                if (HomeRoom_Id != 0)
                    data.AddIntParameter("@arg_HomeRoom_Id", HomeRoom_Id);
                //if (isStudent != false)
                data.AddBoolParameter("@arg_isStudent", isStudent);
                if (UserID != "")
                    data.AddStringParameter("@arg_UserID", UserID);
                if (PIN != "")
                    data.AddStringParameter("@arg_PIN", PIN);
                if (LastName != "")
                    data.AddStringParameter("@arg_LastName", LastName);
                if (FirstName != "")
                    data.AddStringParameter("@arg_FirstName", FirstName);
                if (Middle != "")
                    data.AddStringParameter("@arg_Middle", Middle);
                if (Gender != "")
                    data.AddStringParameter("@arg_Gender", Gender);
                if (SSN != "")
                    data.AddStringParameter("@arg_SSN", SSN);
                if (Address1 != "")
                    data.AddStringParameter("@arg_Address1", Address1);
                if (Address2 != "")
                    data.AddStringParameter("@arg_Address2", Address2);
                if (City != "")
                    data.AddStringParameter("@arg_City", City);
                if (State != "")
                    data.AddStringParameter("@arg_State", State);
                if (Zip != "")
                    data.AddStringParameter("@arg_Zip", Zip);
                if (Phone != "")
                    data.AddStringParameter("@arg_Phone", Phone);
                if (LunchType != 0)
                    data.AddIntParameter("@arg_LunchType", LunchType);
                //if (AllowAlaCarte != false)
                data.AddBoolParameter("@arg_AllowAlaCarte", AllowAlaCarte);
                //if (CashOnly != false)
                data.AddBoolParameter("@arg_CashOnly", CashOnly);
                //if (isActive != false)
                data.AddBoolParameter("@arg_isActive", isActive);
                if (GraduationDate != "")
                    data.AddDateParameter("@arg_GraduationDate", GraduationDate);
                if (SchoolDat != "")
                    data.AddStringParameter("@arg_SchoolDat", SchoolDat);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (ExtraInfo != "")
                    data.AddStringParameter("@arg_ExtraInfo", ExtraInfo);
                if (EMail != "")
                    data.AddStringParameter("@arg_EMail", EMail);
                if (DOB != "")
                    data.AddDateParameter("@arg_DOB", DOB);
                //if (ACH != false)
                data.AddBoolParameter("@arg_ACH", ACH);
                data.AddBoolParameter("@arg_isSnack", isSnack);
                data.AddBoolParameter("@arg_isStudentWorker", isStudentWorker);

                data.SubmitData("usp_ADM_AddCustomer", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgCustomer(int Id, int District_Id, int Language_Id, int Grade_Id, int HomeRoom_Id, bool isStudent, string UserID, string PIN, string LastName, string FirstName, string Middle, string Gender, string SSN, string Address1, string Address2, string City, string State, string Zip, string Phone, int LunchType, bool AllowAlaCarte, bool CashOnly, bool isActive, SmartDate GraduationDate, string SchoolDat, bool isDeleted, string ExtraInfo, string EMail, SmartDate DOB, bool ACH, bool isSnack, bool isStudentWorker)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Language_Id != 0)
                    data.AddIntParameter("@arg_Language_Id", Language_Id);
                if (Grade_Id != 0)
                    data.AddIntParameter("@arg_Grade_Id", Grade_Id);
                if (HomeRoom_Id != 0)
                    data.AddIntParameter("@arg_HomeRoom_Id", HomeRoom_Id);
                //if (isStudent != false)
                data.AddBoolParameter("@arg_isStudent", isStudent);
                if (UserID != "")
                    data.AddStringParameter("@arg_UserID", UserID);
                if (PIN != "")
                    data.AddStringParameter("@arg_PIN", PIN);
                if (LastName != "")
                    data.AddStringParameter("@arg_LastName", LastName);
                if (FirstName != "")
                    data.AddStringParameter("@arg_FirstName", FirstName);
                if (Middle != "")
                    data.AddStringParameter("@arg_Middle", Middle);
                if (Gender != "")
                    data.AddStringParameter("@arg_Gender", Gender);
                if (SSN != "")
                    data.AddStringParameter("@arg_SSN", SSN);
                if (Address1 != "")
                    data.AddStringParameter("@arg_Address1", Address1);
                if (Address2 != "")
                    data.AddStringParameter("@arg_Address2", Address2);
                if (City != "")
                    data.AddStringParameter("@arg_City", City);
                if (State != "")
                    data.AddStringParameter("@arg_State", State);
                if (Zip != "")
                    data.AddStringParameter("@arg_Zip", Zip);
                if (Phone != "")
                    data.AddStringParameter("@arg_Phone", Phone);
                if (LunchType != 0)
                    data.AddIntParameter("@arg_LunchType", LunchType);
                //if (AllowAlaCarte != false)
                data.AddBoolParameter("@arg_AllowAlaCarte", AllowAlaCarte);
                //if (CashOnly != false)
                data.AddBoolParameter("@arg_CashOnly", CashOnly);
                //if (isActive != false)
                data.AddBoolParameter("@arg_isActive", isActive);
                if (GraduationDate != "")
                    data.AddDateParameter("@arg_GraduationDate", GraduationDate);
                if (SchoolDat != "")
                    data.AddStringParameter("@arg_SchoolDat", SchoolDat);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                if (ExtraInfo != "")
                    data.AddStringParameter("@arg_ExtraInfo", ExtraInfo);
                if (EMail != "")
                    data.AddStringParameter("@arg_EMail", EMail);
                if (DOB != "")
                    data.AddDateParameter("@arg_DOB", DOB);
                //if (ACH != false)
                data.AddBoolParameter("@arg_ACH", ACH);
                data.AddBoolParameter("@arg_isSnack", isSnack);
                data.AddBoolParameter("@arg_isStudentWorker", isStudentWorker);

                data.SubmitData("usp_ADM_ChgCustomer", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelCustomer(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelCustomer", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #region Pictures

        public static int AddPicture(int pCustomer_Id, byte[] pPicture, string pPictureType)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (pCustomer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", pCustomer_Id);
                if (pPicture != null)
                    data.AddImageParameter("@arg_Picture", pPicture);
                if (pPictureType != "")
                    data.AddStringParameter("@arg_PictureType", pPictureType);

                data.SubmitData("usp_ADM_AddPictures", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgPicture(int pId, int pCustomer_Id, byte[] pPicture, string pPictureType)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (pId != 0)
                    data.AddIntParameter("@arg_Id", pId);
                if (pCustomer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", pCustomer_Id);
                if (pPicture != null)
                    data.AddImageParameter("@arg_Picture", pPicture);
                if (pPictureType != "")
                    data.AddStringParameter("@arg_PictureType", pPictureType);

                data.SubmitData("usp_ADM_ChgPictures", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelPicture(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelPictures", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #region ChargeCountsData

        public static int AddChargeCounts(int Customer_Id, SmartDate SDate, SmartDate EDate, SmartDate WLetter1, SmartDate WLetter2, SmartDate WLetter3)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (SDate != new SmartDate())
                    data.AddDateParameter("@arg_SDate", SDate);
                if (EDate != new SmartDate())
                    data.AddDateParameter("@arg_EDate", EDate);
                if (WLetter1 != new SmartDate())
                    data.AddDateParameter("@arg_WLetter1", WLetter1);
                if (WLetter2 != new SmartDate())
                    data.AddDateParameter("@arg_WLetter2", WLetter2);
                if (WLetter3 != new SmartDate())
                    data.AddDateParameter("@arg_WLetter3", WLetter3);

                data.SubmitData("usp_ADM_AddChargeCounts", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgChargeCounts(int Id, int Customer_Id, SmartDate SDate, SmartDate EDate, SmartDate WLetter1, SmartDate WLetter2, SmartDate WLetter3)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (SDate != new SmartDate())
                    data.AddDateParameter("@arg_SDate", SDate);
                if (EDate != new SmartDate())
                    data.AddDateParameter("@arg_EDate", EDate);
                if (WLetter1 != new SmartDate())
                    data.AddDateParameter("@arg_WLetter1", WLetter1);
                if (WLetter2 != new SmartDate())
                    data.AddDateParameter("@arg_WLetter2", WLetter2);
                if (WLetter3 != new SmartDate())
                    data.AddDateParameter("@arg_WLetter3", WLetter3);

                data.SubmitData("usp_ADM_ChgChargeCounts", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelChargeCounts(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelChargeCounts", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion

        #region Biometrics

        public static void AddBiometrics(int Customer_Id)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                //if (Finger != "")
                //    data.AddImageParameter("@arg_Finger", Picture);

                data.SubmitData("usp_ADM_AddBiometrics", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgBiometrics(int Customer_Id)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                //if (Finger != "")
                //    data.AddImageParameter("@arg_Finger", Picture);

                data.SubmitData("usp_ADM_ChgBiometrics", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelBiometrics(int Customer_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                data.SubmitData("usp_ADM_DelBiometrics", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region CustomerLog

        public static int AddCustomerLog(int Customer_Id, int Emp_Changed_Id, SmartDate ChangedDate, string Notes, string Comment)
        {
            DataPortal data = new DataPortal();
            try
            {

                data.AddIntParameter("@arg_Id", true);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                //if (Pictures != "")
                //    data.AddImageParameter("@arg_Picture", Picture);
                if (Emp_Changed_Id != 0)
                    data.AddIntParameter("@arg_Emp_Changed_Id", Emp_Changed_Id);
                if (ChangedDate != new SmartDate())
                    data.AddDateParameter("@arg_ChangedDate", ChangedDate);
                if (Notes != "")
                    data.AddStringParameter("@arg_Notes", Notes);
                if (Comment != "")
                    data.AddStringParameter("@arg_Comment", Comment);


                data.SubmitData("usp_ADM_AddCustomerLog", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgCustomerLog(int Id, int Customer_Id, int Emp_Changed_Id, SmartDate ChangedDate, string Notes, string Comment)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                //if (Pictures != "")
                //    data.AddImageParameter("@arg_Picture", Picture);
                if (Emp_Changed_Id != 0)
                    data.AddIntParameter("@arg_Emp_Changed_Id", Emp_Changed_Id);
                if (ChangedDate != new SmartDate())
                    data.AddDateParameter("@arg_ChangedDate", ChangedDate);
                if (Notes != "")
                    data.AddStringParameter("@arg_Notes", Notes);
                if (Comment != "")
                    data.AddStringParameter("@arg_Comment", Comment);


                data.SubmitData("usp_ADM_ChgCustomerLog", DataPortal.QueryType.StoredProc);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelCustomerLog(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelCustomerLog", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region AccountInfo

        public static int AddAccountInfo(int Customer_Id, decimal ABalance, decimal MBalance)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                //if (Pictures != "")
                //    data.AddImageParameter("@arg_Picture", Picture);
                //if (ABalance != 0)
                data.AddDecimalParameter("@arg_ABalance", ABalance);
                //if (MBalance != 0)
                data.AddDecimalParameter("@arg_MBalance", MBalance);

                data.SubmitData("usp_ADM_AddAccountInfo", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgAccountInfo(int Id, int Customer_Id, decimal ABalance, decimal MBalance)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                //if (Pictures != "")
                //    data.AddImageParameter("@arg_Picture", Picture);
                //if (ABalance != 0)
                data.AddDecimalParameter("@arg_ABalance", ABalance);
                //if (MBalance != 0)
                data.AddDecimalParameter("@arg_MBalance", MBalance);

                data.SubmitData("usp_ADM_ChgAccountInfo", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelAccountInfo(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelAccountInfo", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Customer_School

        public static int AddCustomerSchool(int Customer_Id, int School_Id, bool isPrimary)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CustID", true);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                //if (isPrimary != false)
                data.AddBoolParameter("@arg_isPrimary", isPrimary);

                data.SubmitData("usp_ADM_AddCustomerSchool", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_CustID");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgCustomerSchool(int CustID, int Customer_Id, int School_Id, bool isPrimary)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (CustID != 0)
                    data.AddIntParameter("@arg_CustID", CustID);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (School_Id != 0)
                    data.AddIntParameter("@arg_School_Id", School_Id);
                //if (isPrimary != false)
                data.AddBoolParameter("@arg_isPrimary", isPrimary);

                data.SubmitData("usp_ADM_ChgCustomerSchool", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelCustomerSchool(int CustID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_CustID", CustID);
                data.SubmitData("usp_ADM_DelCustomerSchool", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Employee(User)

        #region Private Function
        private static EmployeeData PopulateEmployeeDataFromReader(SafeDataReader reader)
        {
            EmployeeData ed = new EmployeeData();
            ed.EmployeeID = reader.GetInt32("Id");

            NameValuePair[] nvpcon = new NameValuePair[3];
            nvpcon[0].Value = reader.GetInt32("Customer_Id");
            nvpcon[0].Name = reader.GetString("FirstName").Trim();

            nvpcon[1].Value = reader.GetInt32("Customer_Id");
            nvpcon[1].Name = reader.GetString("LastName").Trim();

            //nvpcon[2].Value = reader.GetInt32("Customer_Id");
            //nvpcon[2].Name = reader.GetString("Middle");

            ed.Customer = new NameValuePairCollection(nvpcon);
            ed.LoginName = reader.GetString("LoginName").Trim();
           // ed.SecurityGroup = new NameValuePair(reader.GetString("GroupName").Trim(), reader.GetInt32("SecurityGroup_Id"));
            return ed;
        }

        private static CashResultsData PopulateCashResultsDataFromReader(SafeDataReader reader)
        {
            Byte[] pOpenBlob = new Byte[1000];
            Byte[] pCloseBlob = new Byte[1000];
            CashResultsData cr = new CashResultsData();
            cr.CashResultsID = reader.GetInt32("Id");
            cr.POS = new NameValuePair(reader.GetString("Name").Trim(), reader.GetInt32("POS_Id"));
            // cr.EmpCashier = new NameValuePairCollection(reader.GetString("EmpCashierName"), reader.GetInt32("Emp_Cashier_Id"));
            cr.OpenDate = reader.GetSmartDate("OpenDate");
            cr.CloseDate = reader.GetSmartDate("CloseDate");
            //cr.TotalCash = reader.GetDouble("TotalCash");
            //cr.OverShort = reader.GetDouble("OverShort");
            //cr.Additional = reader.GetDouble("Additional");
            //cr.PaidOuts = reader.GetDouble("PaidOuts");
            //cr.OpenAmount = reader.GetDouble("OpenAmount");
            //cr.CloseAmount = reader.GetDouble("CloseAmount");
            //cr.Sales = reader.GetDouble("Sales");
            cr.EmpCashierId = reader.GetInt32("Emp_Cashier_Id");
            cr.TotalCash = Convert.ToDouble(reader.GetDecimal("TotalCash"));
            cr.OverShort = Convert.ToDouble(reader.GetDecimal("OverShort"));
            cr.Additional = Convert.ToDouble(reader.GetDecimal("Additional"));
            cr.PaidOuts = Convert.ToDouble(reader.GetDecimal("PaidOuts"));
            cr.OpenAmount = Convert.ToDouble(reader.GetDecimal("OpenAmount"));
            cr.CloseAmount = Convert.ToDouble(reader.GetDecimal("CloseAmount"));
            cr.Sales = Convert.ToDouble(reader.GetDecimal("Sales"));

            cr.Finished = reader.GetBoolean("Finished");
            //cr.OpenBlob = reader.GetBytes("OpenBlob", 0, byte1, 0, 1);
            //cr.CloseBlob = reader.GetByte("CloseBlob");
            long x = reader.GetBytes("OpenBlob", 0, pOpenBlob, 0, 1000);
            long y = reader.GetBytes("CloseBlob", 0, pCloseBlob, 0, 1000);
            //cr.OpenBlob = pOpenBlob;
            //cr.CloseBlob = pCloseBlob;

            if (x == 0)
            {
                cr.OpenBlob = null;
            }
            else
            {
                cr.OpenBlob = pOpenBlob;
            }
            if (y == 0)
            {
                cr.CloseBlob = null;
            }
            else
            {
                cr.CloseBlob = pCloseBlob;
            }

            return cr;
        }
        #endregion

        #region Public Function
        public static EmployeeData[] ListEmployeeByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EmployeeData> edList = new List<EmployeeData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetEmployeeByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    EmployeeData ed = PopulateEmployeeDataFromReader(reader);
                    edList.Add(ed);
                }
                return edList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CashResultsData GetCashResultsByUserID(int pCashierID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            //List<CashResultsData> edList = new List<CashResultsData>();
            try
            {
                data.AddIntParameter("@arg_CashierID", pCashierID);
                reader = data.GetDataReader("usp_ADM_GetCashResultsByUserID", DataPortal.QueryType.StoredProc);
                CashResultsData ed = new CashResultsData();
                if (reader.Read())
                {
                    ed = PopulateCashResultsDataFromReader(reader);
                    //edList.Add(ed);
                }
                return ed;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CashResultsData[] GetCashResultsList()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CashResultsData> edList = new List<CashResultsData>();
            try
            {
                reader = data.GetDataReader("usp_ADM_GetCashResults", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CashResultsData ed = PopulateCashResultsDataFromReader(reader);
                    edList.Add(ed);
                }
                return edList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelTempCashResults()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                string strSQL = "delete from TempCashResults";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void AddTempCashResults(int CashResultsID, int POS_Id, int Emp_Cashier_Id, SmartDate OpenDate, SmartDate CloseDate, double TotalCash, double OverShort, double Additional, double PaidOuts, double OpenAmount, double CloseAmount, double Sales, bool Finished, decimal Open1C, decimal Open5C, decimal Open10C, decimal Open25C, decimal Open1D, decimal Open5D, decimal Open10D, decimal Open20D, decimal OpenChange, decimal OpenCheck, decimal Close1C, decimal Close5C, decimal Close10C, decimal Close25C, decimal Close1D, decimal Close5D, decimal Close10D, decimal Close20D, decimal CloseChange, decimal CloseCheck)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (CashResultsID != 0)
                    data.AddIntParameter("@arg_Id", CashResultsID);
                if (POS_Id != 0)
                    data.AddIntParameter("@arg_POS_Id", POS_Id);
                if (Emp_Cashier_Id != 0)
                    data.AddIntParameter("@arg_Emp_Cashier_Id", Emp_Cashier_Id);
                if (OpenDate != "")
                    data.AddDateParameter("@arg_OpenDate", OpenDate);
                if (CloseDate != "")
                    data.AddDateParameter("@arg_CloseDate", CloseDate);
                //if (TotalCash != 0)
                data.AddFloatParameter("@arg_TotalCash", float.Parse(TotalCash.ToString()));
                //if (OverShort != "")
                data.AddFloatParameter("@arg_OverShort", float.Parse(OverShort.ToString()));
                //if (Additional != "")
                data.AddFloatParameter("@arg_Additional", float.Parse(Additional.ToString()));
                //if (PaidOuts != "")
                data.AddFloatParameter("@arg_PaidOuts", float.Parse(PaidOuts.ToString()));
                //if (OpenAmount != "")
                data.AddFloatParameter("@arg_OpenAmount", float.Parse(OpenAmount.ToString()));
                //if (CloseAmount != 0)
                data.AddFloatParameter("@arg_CloseAmount", float.Parse(CloseAmount.ToString()));
                //if (Sales != "")
                data.AddFloatParameter("@arg_Sales", float.Parse(Sales.ToString()));
                //if (Finished != false)
                data.AddBoolParameter("@arg_Finished", Finished);
                /////////////////////////////////////////////////////////////
                data.AddDecimalParameter("@arg_Open1C", Open1C);
                data.AddDecimalParameter("@arg_Open5C", Open5C);
                data.AddDecimalParameter("@arg_Open10C", Open10C);
                data.AddDecimalParameter("@arg_Open25C", Open25C);
                data.AddDecimalParameter("@arg_Open1D", Open1D);
                data.AddDecimalParameter("@arg_Open5D", Open5D);
                data.AddDecimalParameter("@arg_Open10D", Open10D);
                data.AddDecimalParameter("@arg_Open20D", Open20D);
                data.AddDecimalParameter("@arg_OpenChange", OpenChange);
                data.AddDecimalParameter("@arg_OpenCheck", OpenCheck);

                data.AddDecimalParameter("@arg_Close1C", Close1C);
                data.AddDecimalParameter("@arg_Close5C", Close5C);
                data.AddDecimalParameter("@arg_Close10C", Close10C);
                data.AddDecimalParameter("@arg_Close25C", Close25C);
                data.AddDecimalParameter("@arg_Close1D", Close1D);
                data.AddDecimalParameter("@arg_Close5D", Close5D);
                data.AddDecimalParameter("@arg_Close10D", Close10D);
                data.AddDecimalParameter("@arg_Close20D", Close20D);
                data.AddDecimalParameter("@arg_CloseChange", CloseChange);
                data.AddDecimalParameter("@arg_CloseCheck", CloseCheck);

                data.SubmitData("usp_ADM_AddTempCashResults", DataPortal.QueryType.StoredProc);
                //return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static EmployeeData[] GetEmployeeByEmployeeID(int pId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EmployeeData> edlist = new List<EmployeeData>();
            try
            {
                data.AddIntParameter("@arg_Id", pId);
                reader = data.GetDataReader("usp_ADM_GetEmployeeByEmployeeId", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    EmployeeData ed = PopulateEmployeeDataFromReader(reader);
                    edlist.Add(ed);
                }
                return edlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static EmployeeData[] GetEmployeeByLoginNameANDPassword(string pLoginname, string pPassword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<EmployeeData> edlist = new List<EmployeeData>();
            try
            {
                data.AddStringParameter("@arg_LoginName", pLoginname);
                data.AddStringParameter("@arg_Password", pPassword);
                reader = data.GetDataReader("usp_ADM_GetEmployeeByLoginNameandPassword", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    EmployeeData ed = PopulateEmployeeDataFromReader(reader);
                    edlist.Add(ed);
                }
                return edlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForEmployeeLoginName(string name)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as LoginNameCount from Employee where LoginName='" + name + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("LoginNameCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCountForEmployeeCustomerID(int custid)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as EmpCustomerIDCount from Employee where Customer_Id=" + custid;
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("EmpCustomerIDCount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Add,Del,chg
        public static int AddEmployee(int Customer_Id, int SecurityGroup_Id, string LoginName, string Password)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (SecurityGroup_Id != 0)
                    data.AddIntParameter("@arg_SecurityGroup_Id", SecurityGroup_Id);
                if (LoginName != "")
                    data.AddStringParameter("@arg_LoginName", LoginName);
                if (Password != "")
                    data.AddStringParameter("@arg_Password", Password);

                data.SubmitData("usp_ADM_AddEmployee", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgEmployee(int Id, int Customer_Id, int SecurityGroup_Id, string LoginName)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (SecurityGroup_Id != 0)
                    data.AddIntParameter("@arg_SecurityGroup_Id", SecurityGroup_Id);
                if (LoginName != "")
                    data.AddStringParameter("@arg_LoginName", LoginName);
                //if (Password != "")
                //    data.AddStringParameter("@arg_Password", Password);

                data.SubmitData("usp_ADM_ChgEmployee", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgEmployeePasswordById(int Id, string PassWord)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (PassWord != "")
                    data.AddStringParameter("@arg_Password", PassWord);

                data.SubmitData("usp_ADM_ChgEmployeePassword", DataPortal.QueryType.StoredProc);


            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelEmployee(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelEmoloyee", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #endregion

        #region Security
        #region Access Rights
        #region Private Function
        private static AccessRightsData PopulateAccessRightsDataFromReader(SafeDataReader reader)
        {
            AccessRightsData ard = new AccessRightsData();
            ard.Id = reader.GetInt32("Id");
            ard.ObjectP = new NameValuePair(reader.GetString("ObjectName"), reader.GetInt32("ObjectID"));
            ard.SecurityGroup = new NameValuePair(reader.GetString("GroupName").Trim(), reader.GetInt32("SecurityGroup_Id"));
            ard.canInsert = reader.GetBoolean("canInsert");
            ard.canDelete = reader.GetBoolean("canDelete");
            ard.canView = reader.GetBoolean("canView");
            ard.canEdit = reader.GetBoolean("canEdit");
            return ard;
        }
        #endregion

        #region Public Function

        public static AccessRightsData[] GetAccessRightsBySecurityGroupIdAndObjectId(int pSecurityGroup_Id, int pObjectId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<AccessRightsData> ardlist = new List<AccessRightsData>();
            try
            {
                data.AddIntParameter("@arg_SecurityGroup_Id", pSecurityGroup_Id);
                data.AddIntParameter("@arg_ObjectID", pObjectId);
                reader = data.GetDataReader("usp_ADM_GetAccessRightsBySecurityGroupIDAndObjectID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    AccessRightsData ard = PopulateAccessRightsDataFromReader(reader);
                    ardlist.Add(ard);
                }
                return ardlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static DataSet GetIndexGen()
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.FillDataSet("usp_ADM_GetIndexGen", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Add,Del,chg
        public static int AddAccessRights(int ObjectID, int SecurityGroup_Id, bool canInsert, bool canDelete, bool canView, bool canEdit)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (ObjectID != 0)
                    data.AddIntParameter("@arg_ObjectID", ObjectID);
                if (SecurityGroup_Id != 0)
                    data.AddIntParameter("@arg_SecurityGroup_Id", SecurityGroup_Id);
                //if (canInsert != false)
                data.AddBoolParameter("@arg_canInsert", canInsert);
                //if (canDelete != false)
                data.AddBoolParameter("@arg_canDelete", canDelete);
                //if (canView != false)
                data.AddBoolParameter("@arg_canView", canView);
                //if (canEdit != false)
                data.AddBoolParameter("@arg_canEdit", canEdit);

                data.SubmitData("usp_ADM_AddAccessRights", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void ChgAccessRights(int Id, int ObjectID, int SecurityGroup_Id, bool canInsert, bool canDelete, bool canView, bool canEdit)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (ObjectID != 0)
                    data.AddIntParameter("@arg_ObjectID", ObjectID);
                if (SecurityGroup_Id != 0)
                    data.AddIntParameter("@arg_SecurityGroup_Id", SecurityGroup_Id);
                //if (canInsert != false)
                data.AddBoolParameter("@arg_canInsert", canInsert);
                //if (canDelete != false)
                data.AddBoolParameter("@arg_canDelete", canDelete);
                //if (canView != false)
                data.AddBoolParameter("@arg_canView", canView);
                //if (canEdit != false)
                data.AddBoolParameter("@arg_canEdit", canEdit);

                data.SubmitData("usp_ADM_ChgAccessRights", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelAccessRights(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelAccessRights", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion
        #endregion

        #region Security Group
        #region Private Functions

        private static SecurityGroupData PopulateSecurityGroupDataFromReader(SafeDataReader reader)
        {
            SecurityGroupData sgd = new SecurityGroupData();
            sgd.Id = reader.GetInt32("Id");
            sgd.GroupName = reader.GetString("GroupName").Trim();
            return sgd;
        }

        #endregion

        #region Public Function

        public static SecurityGroupData[] ListSecurityGroup(int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<SecurityGroupData> sgdList = new List<SecurityGroupData>();

            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetSecurityGroup", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    SecurityGroupData sgd = PopulateSecurityGroupDataFromReader(reader);
                    sgdList.Add(sgd);

                }
                return sgdList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static SecurityGroupData[] GetSecurityGroupBySecurityGroupId(int pId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<SecurityGroupData> sgdlist = new List<SecurityGroupData>();
            try
            {
                data.AddIntParameter("@arg_Id", pId);
                reader = data.GetDataReader("usp_ADM_GetSecurityGroupBySecurityGroupId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    SecurityGroupData sgd = PopulateSecurityGroupDataFromReader(reader);
                    sgdlist.Add(sgd);
                }
                return sgdlist.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static DataSet GetSecurityGroupCount(string Name)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.FillDataSet("select count(*) from SecurityGroup where GroupName='" + Name + "'", DataPortal.QueryType.QueryString, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {

                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Add,Chg,Del

        public static int AddSecurityGroup(string GroupName)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (GroupName != "")
                    data.AddStringParameter("@arg_GroupName", GroupName);
                data.SubmitData("usp_ADM_AddSecurityGroup", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_Id");
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void ChgSecurityGroup(int Id, string GroupName)
        {
            DataPortal data = new DataPortal();
            try
            {
                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (GroupName != "")
                    data.AddStringParameter("@arg_GroupName", GroupName);

                data.SubmitData("usp_ADM_ChgSecurityGroup", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static void DelSecurityGroup(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelSecurityGroup", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion
        #endregion

        #endregion

        #region POS
        public static DataSet ListPOS()
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.FillDataSet("usp_pos_GetPOS", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region DashBoard
        public static DataSet GetSummary()
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.FillDataSet("usp_ADM_GetSummary", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static DataSet GetSalesInfoByCurrentDate(SmartDate pCurrentDate)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddDateParameter("@arg_StartDate", pCurrentDate);
                data.FillDataSet("usp_ADM_GetSalesInfoByCurrentDate", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static DataSet GetSalesInfoByDateRange(SmartDate pStartDate, SmartDate pEndDate)
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.AddDateParameter("@arg_StartDate", pStartDate);
                data.AddDateParameter("@arg_EndDate", pEndDate);
                data.FillDataSet("usp_ADM_GetSalesInfoByDateRange", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        public static DataSet GetAccountBalance()
        {
            DataPortal data = new DataPortal();
            DataSet ds = new DataSet();
            try
            {
                data.FillDataSet("usp_ADM_GetAccountBalance", DataPortal.QueryType.StoredProc, ds);
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Admin Search

        public static DistrictData[] ListDistrictsByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<DistrictData> ddList = new List<DistrictData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetDistrictsByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    DistrictData dd = PopulateDistrictDataFromReader(reader);
                    ddList.Add(dd);

                }
                return ddList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomersData[] ListCustomersByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomersData> cdList = new List<CustomersData>();

            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetCustomersByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomersData cd = PopulateCustomersDataFromReader(reader);
                    cdList.Add(cd);

                }
                return cdList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static SecurityGroupData[] ListSecurityGroupByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<SecurityGroupData> sgdList = new List<SecurityGroupData>();

            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetSecurityGroupByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    SecurityGroupData sgd = PopulateSecurityGroupDataFromReader(reader);
                    sgdList.Add(sgd);

                }
                return sgdList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Counts

        public static int GetCustomersCount()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as listscount from Customers";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetEmployeesCount()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as listscount from Employee";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetEmployeesCountByPassword(int id, string password)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as passwordcount from Employee where ID=" + id + "and password='" + password + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("passwordcount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetSecurityGroupCount()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as listscount from SecurityGroup";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetPOSCount()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as listscount from POS";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetOrderCountByCustomerId(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as ordercount from Orders where Customer_Id='" + Id + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("ordercount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        //public static int GetAdultsCount(string pSchoolName)
        //{
        //     DataPortal data = new DataPortal();
        //    SafeDataReader reader = null;
        //    try
        //    {
        //        int count = 0;
        //        if (pSchoolName != "")
        //            data.AddStringParameter("@arg_School_Name", pSchoolName);
        //        reader = data.GetDataReader("usp_ADM_GetAdultStudents", DataPortal.QueryType.StoredProc);
        //        while (reader.Read())
        //        {
        //            count = reader.GetInt32("AdultCount");
        //        }
        //        return count;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}

        //public static int GetFreeStudentsCount(string pSchoolName)
        //{
        //    DataPortal data = new DataPortal();
        //    SafeDataReader reader = null;
        //    try
        //    {
        //        int count = 0;
        //        if (pSchoolName != "")
        //            data.AddStringParameter("@arg_School_Name", pSchoolName);
        //        reader = data.GetDataReader("usp_ADM_GetFreeStudents", DataPortal.QueryType.StoredProc);
        //        while (reader.Read())
        //        {
        //            count = reader.GetInt32("FreeCount");
        //        }
        //        return count;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}

        //public static int GetPaidStudentsCount(string pSchoolName)
        //{
        //    DataPortal data = new DataPortal();
        //    SafeDataReader reader = null;
        //    try
        //    {
        //        int count = 0;
        //        if (pSchoolName != "")
        //            data.AddStringParameter("@arg_School_Name", pSchoolName);
        //        reader = data.GetDataReader("usp_ADM_GetPaidStudents", DataPortal.QueryType.StoredProc);
        //        while (reader.Read())
        //        {
        //            count = reader.GetInt32("PaidCount");
        //        }
        //        return count;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}

        //public static int GetReducedStudentsCount(string pSchoolName)
        //{
        //    DataPortal data = new DataPortal();
        //    SafeDataReader reader = null;
        //    try
        //    {
        //        int count = 0;
        //        if (pSchoolName != "")
        //            data.AddStringParameter("@arg_School_Name", pSchoolName);
        //        reader = data.GetDataReader("usp_ADM_GetReducedStudents", DataPortal.QueryType.StoredProc);
        //        while (reader.Read())
        //        {
        //            count = reader.GetInt32("ReducedCount");
        //        }
        //        return count;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}

        #endregion

        #region keywordcount

        public static int GetDistrictsCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_ADM_GetDistrictsCountByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listcount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCustomersCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_ADM_GetCustomersCountByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listcount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetSecurityGroupsCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_ADM_GetSecurityGroupCountByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetEmployeeCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_ADM_GetEmployeeCountByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region OtherActivity

        #region CustomerActivity
        #region Private Function
        private static CustomerActivityData PopulateCustomerActivityDataFromReader(SafeDataReader reader)
        {
            CustomerActivityData ca = new CustomerActivityData();
            ca.ID = reader.GetInt32("ID");
            ca.Customer_Id = reader.GetInt32("Customer_Id");
            NameValuePair[] nvpCustActivity = new NameValuePair[5];
            nvpCustActivity[0].Value = reader.GetInt32("ActivityItem_Id");
            nvpCustActivity[0].Name = reader.GetString("ItemName").ToString().Trim();
            nvpCustActivity[1].Value = reader.GetInt32("ActivityItem_Id");
            nvpCustActivity[1].Name = reader.GetDecimal("StudentPrice").ToString();
            nvpCustActivity[2].Value = reader.GetInt32("ActivityItem_Id");
            nvpCustActivity[2].Name = reader.GetBoolean("IsTaxable").ToString();
            nvpCustActivity[3].Value = reader.GetInt32("ActivityItem_Id");
            nvpCustActivity[3].Name = reader.GetBoolean("IsDeleted").ToString();
            ca.ActivityItem = new NameValuePairCollection(nvpCustActivity);
            return ca;
        }

        #endregion

        #region Public Function
        //public static CustomerActivityData[] ListCustomerActivityByCustomerID(int pID, int pCustomer_Id,int pActivityItem_Id)
        //{
        //    DataPortal data = new DataPortal();
        //    SafeDataReader reader = null;
        //    List<CustomerActivityData> caList = new List<CustomerActivityData>();

        //    try
        //    {

        //        reader = data.GetDataReader("usp_ADM_GetCustomerActivityByCustomerID", DataPortal.QueryType.StoredProc);
        //        while (reader.Read())
        //        {
        //            CustomerActivityData ca = PopulateCustomerActivityDataFromReader(reader);
        //            caList.Add(ca);

        //        }
        //        return caList.ToArray();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}

        public static CustomerActivityData[] ListCustomerActivityByCustomerID(int pcustomerID, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerActivityData> cdList = new List<CustomerActivityData>();
            try
            {
                data.AddIntParameter("@arg_CustomerID", pcustomerID);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetCustomerActivityByCustomerID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomerActivityData ca = PopulateCustomerActivityDataFromReader(reader);
                    cdList.Add(ca);
                }
                return cdList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static int GetCustomerActivityItemsCount(int CustomerId)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                data.AddIntParameter("@arg_CustomerID", CustomerId);
                reader = data.GetDataReader("usp_ADM_GetCustomerActivityItemsPaymentDueCountByCustomerID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    count = reader.GetInt32("listscount");
                }
                return count;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #region Del,Add,Chg

        public static int AddCustomerActivity(int Customer_Id, int ActivityItem_Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_ID", true);
                if (Customer_Id != 0)
                    data.AddIntParameter("@arg_Customer_Id", Customer_Id);
                if (ActivityItem_Id != 0)
                    data.AddIntParameter("@arg_ActivityItem_Id", ActivityItem_Id);


                data.SubmitData("usp_ADM_AddCustomerActivity", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_ID");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelCustomerActivity(int ID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_ID", ID);
                data.SubmitData("USP_ADM_DelCustomerActivity", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #endregion

        #region Customer_ActivityPayment

        #region Private Function

        private static CustomerActivityPaymentData PopulateCustomerActivityPaymentDataFromReader(SafeDataReader reader)
        {
            CustomerActivityPaymentData cp = new CustomerActivityPaymentData();
            cp.ID = reader.GetInt32("ID");
            cp.Customer_ID = reader.GetInt32("Customer_ID");
            NameValuePair[] nvpcustpayment = new NameValuePair[5];
            nvpcustpayment[0].Value = reader.GetInt32("ActivityItem_Id");
            nvpcustpayment[0].Name = reader.GetString("ItemName").ToString().Trim();
            nvpcustpayment[1].Value = reader.GetInt32("ActivityItem_Id");
            nvpcustpayment[1].Name = reader.GetDecimal("StudentPrice").ToString();
            nvpcustpayment[2].Value = reader.GetInt32("ActivityItem_Id");
            nvpcustpayment[2].Name = reader.GetBoolean("IsTaxable").ToString();
            nvpcustpayment[3].Value = reader.GetInt32("ActivityItem_Id");
            nvpcustpayment[3].Name = reader.GetBoolean("IsDeleted").ToString();
            cp.ActivityItem = new NameValuePairCollection(nvpcustpayment);
            cp.ADebit = reader.GetDecimal("ADebit");
            cp.ACredit = reader.GetDecimal("ACredit");
            cp.PriorABal = reader.GetDecimal("PriorABal");
            cp.TransType = reader.GetInt32("TransType");
            cp.PaymentDate = reader.GetDateTime("PaymentDate");
            return cp;
        }

        #endregion

        #region Public Function
        public static CustomerActivityPaymentData[] ListCustomerActivityPaymentByCustomerID(int pcustomerID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerActivityPaymentData> caList = new List<CustomerActivityPaymentData>();

            try
            {
                data.AddIntParameter("@arg_CustomerID", pcustomerID);
                reader = data.GetDataReader("usp_ADM_GetCustomerActPayByCustomerID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomerActivityPaymentData ca = PopulateCustomerActivityPaymentDataFromReader(reader);
                    caList.Add(ca);

                }
                return caList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        public static CustomerActivityData[] ListCustomerActivityItemPaymentDueByCustomerID(int pcustomerID, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            List<CustomerActivityData> cdList = new List<CustomerActivityData>();

            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                data.AddIntParameter("@arg_CustomerID", pcustomerID);
                reader = data.GetDataReader("usp_ADM_GetCustomerActivityItemsPaymentDueByCustomerID", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    CustomerActivityData ca = PopulateCustomerActivityDataFromReader(reader);
                    cdList.Add(ca);

                }
                return cdList.ToArray();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (data != null)
                    data.Dispose();
            }
        }

        //public static CustomerActivityPaymentData[] ListCustomerActivityItemPaymentByCustomerID(int pcustomerID)
        //{
        //    DataPortal data = new DataPortal();
        //    SafeDataReader reader = null;
        //    List<CustomerActivityPaymentData> caList = new List<CustomerActivityPaymentData>();

        //    try
        //    {

        //        data.AddIntParameter("@arg_CustomerID", pcustomerID);
        //        reader = data.GetDataReader("usp_ADM_GetCustomerActivityByCustomerID", DataPortal.QueryType.StoredProc);
        //        while (reader.Read())
        //        {
        //            CustomerActivityPaymentData ca = PopulateCustomerActivityPaymentDataFromReader(reader);
        //            caList.Add(ca);

        //        }
        //        return caList.ToArray();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //        {
        //            reader.Close();
        //            reader.Dispose();
        //            reader = null;
        //        }
        //        if (data != null)
        //            data.Dispose();
        //    }
        //}

        #endregion

        #region Del,Add,Chg
        public static int AddCustomerActivityPayment(int Customer_ID, int ActivityItem_ID, decimal ADebit, decimal ACredit, decimal PriorABal, int TransType, SmartDate PaymentDate)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_ID", true);
                if (Customer_ID != 0)
                    data.AddIntParameter("@arg_Customer_ID", Customer_ID);
                if (ActivityItem_ID != 0)
                    data.AddIntParameter("@arg_ActivityItem_ID", ActivityItem_ID);
                if (ADebit != 0)
                    data.AddDecimalParameter("@arg_ADebit", ADebit);
                if (ACredit != 0)
                    data.AddDecimalParameter("@arg_ACredit", ACredit);
                if (PriorABal != 0)
                    data.AddDecimalParameter("@arg_PriorABal", PriorABal);
                if (TransType != 0)
                    data.AddIntParameter("@arg_TransType", TransType);
                if (PaymentDate != new DateTime())
                    data.AddDateParameter("@arg_PaymentDate", PaymentDate);

                data.SubmitData("usp_ADM_AddCustomerActivityPayment", DataPortal.QueryType.StoredProc);
                return (int)data.GetParameterValue("@arg_ID");

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }

        }

        public static void DelCustomerActivityPayment(int ID)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_ID", ID);
                data.SubmitData("USP_ADM_DelCustomerActivityPayment", DataPortal.QueryType.StoredProc);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Low Balance Notification Settings

        public static int SaveLowBalNotifSettings(int districtId, int parentId, bool balNotify, bool paymentNotify, bool vipNotify, bool preorderNotify, string email, DataTable studentsPreferences)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@DistrictId", districtId);
                data.AddIntParameter("@ParentId", parentId);
                data.AddBoolParameter("@BalNotify", balNotify);
                data.AddBoolParameter("@PaymentNotify", paymentNotify);
                data.AddBoolParameter("@VipNotify", vipNotify);
                data.AddBoolParameter("@PreorderNotify", preorderNotify);
                data.AddStringParameter("@Email", email);
                data.AddDataTableParameter("@studentsPreferences", studentsPreferences);

                return data.SubmitData("msa_UpdateStudentsLowBalSettings", DataPortal.QueryType.StoredProc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (data != null)
                    data.Dispose();
            }
        }
        #endregion
    }
}
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Data;
using MSA_ADMIN.DAL.Models;
using MSA_ADMIN.DAL.Common;
//using FSSAdmin.Data;
//using Common;

namespace MSA_ADMIN.DAL.Factories
{
    public class SchoolsFactory
    {
        #region Static Function

        public static Collection<SchoolsData> ListSchool(int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<SchoolsData> sdlist = new Collection<SchoolsData>();
            try
            {
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetSchools", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    SchoolsData sd = AdminFactory.PopulateSchoolDataFromReader(reader);
                    sdlist.Add(sd);
                }
                return sdlist;
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

        public static Collection<SchoolsData> ListSchoolsByDistrictID(int pDistrictID, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<SchoolsData> sdlist = new Collection<SchoolsData>();
            try
            {
                if (pDistrictID != 0)
                    data.AddIntParameter("@arg_District_Id", pDistrictID);
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetSchoolsByDistrictId", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    SchoolsData sd = AdminFactory.PopulateSchoolDataFromReader(reader);
                    sdlist.Add(sd);
                }
                return sdlist;
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

        public static Collection<SchoolsData> ListSchoolsByKeyword(string pKeyword, int pPageIndex, int pPageSize)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<SchoolsData> sdlist = new Collection<SchoolsData>();
            try
            {
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                data.AddIntParameter("@PageIndex", pPageIndex);
                data.AddIntParameter("@PageSize", pPageSize);
                reader = data.GetDataReader("usp_ADM_GetSchoolsByKeyword", DataPortal.QueryType.StoredProc);
                while (reader.Read())
                {
                    SchoolsData sd = AdminFactory.PopulateSchoolDataFromReader(reader);
                    sdlist.Add(sd);
                }
                return sdlist;
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

        public static Collection<SchoolsData> GetSchoolsBySchoolID(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            Collection<SchoolsData> sdlist = new Collection<SchoolsData>();
            try
            {
                data.AddIntParameter("@arg_SchoolID", Id);
                reader = data.GetDataReader("usp_ADM_GetSchoolsBySchoolID", DataPortal.QueryType.StoredProc);
                if (reader.Read())
                {
                    SchoolsData sd = AdminFactory.PopulateSchoolDataFromReader(reader);
                    sdlist.Add(sd);
                }
                return sdlist;
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

        public static int GetCountForSchoolName(string name)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as SchoolNameCount from Schools where SchoolName='" + name + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("SchoolNameCount");
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

        public static int GetCountForSchoolID(string Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as SchoolIDCount from Schools where SchoolID='" + Id + "'";
                reader = data.GetDataReader(strSQL, DataPortal.QueryType.QueryString);
                while (reader.Read())
                {
                    count = reader.GetInt32("SchoolIDCount");
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
        public static int GetSchoolsCount()
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as listscount from Schools";
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

        public static int GetSchoolsByDistrictID(int DistrictID)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as listscount from Schools where District_Id='" + DistrictID + "'";
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

        public static int GetOrderCountBySchoolId(int Id)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                string strSQL = "select count(*) as ordercount from Orders where School_Id='" + Id + "'";
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

        public static int GetSchoolsCountByKeyword(string pKeyword)
        {
            DataPortal data = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                int count = 0;
                if (pKeyword != "")
                    data.AddStringParameter("@arg_Keyword", pKeyword + "%");
                reader = data.GetDataReader("usp_ADM_GetSchoolsCountByKeyword", DataPortal.QueryType.StoredProc);
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

        public static int AddSchool(int District_Id, int Emp_Director_Id, int Emp_Administrator_Id, string SchoolID, string SchoolName, string Address1, string Address2, string City, string State, string Zip, string Phone1, string Phone2, string Comment, bool isSevereNeed, bool isDeleted)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_Id", true);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Emp_Director_Id != 0)
                    data.AddIntParameter("@arg_Emp_Director_Id", Emp_Director_Id);
                if (Emp_Administrator_Id != 0)
                    data.AddIntParameter("@arg_Emp_Administrator_Id", Emp_Administrator_Id);
                if (SchoolID != "")
                    data.AddStringParameter("@arg_SchoolID", SchoolID);
                if (SchoolName != "")
                    data.AddStringParameter("@arg_SchoolName", SchoolName);
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
                if (Comment != "")
                    data.AddStringParameter("@arg_Comment", Comment);
                //if (isSevereNeed != false)
                data.AddBoolParameter("@arg_isSevereNeed", isSevereNeed);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);

                data.SubmitData("usp_ADM_AddSchools", DataPortal.QueryType.StoredProc);
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

        public static void ChgSchool(int Id, int District_Id, int Emp_Director_Id, int Emp_Administrator_Id, string SchoolID, string SchoolName, string Address1, string Address2, string City, string State, string Zip, string Phone1, string Phone2, string Comment, bool isSevereNeed, bool isDeleted)
        {
            DataPortal data = new DataPortal();
            try
            {

                if (Id != 0)
                    data.AddIntParameter("@arg_Id", Id);
                if (District_Id != 0)
                    data.AddIntParameter("@arg_District_Id", District_Id);
                if (Emp_Director_Id != 0)
                    data.AddIntParameter("@arg_Emp_Director_Id", Emp_Director_Id);
                if (Emp_Administrator_Id != 0)
                    data.AddIntParameter("@arg_Emp_Administrator_Id", Emp_Administrator_Id);
                if (SchoolID != "")
                    data.AddStringParameter("@arg_SchoolID", SchoolID);
                if (SchoolName != "")
                    data.AddStringParameter("@arg_SchoolName", SchoolName);
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
                if (Comment != "")
                    data.AddStringParameter("@arg_Comment", Comment);
                // if (isSevereNeed != false)
                data.AddBoolParameter("@arg_isSevereNeed", isSevereNeed);
                //if (isDeleted != false)
                data.AddBoolParameter("@arg_isDeleted", isDeleted);
                data.SubmitData("usp_ADM_ChgSchools", DataPortal.QueryType.StoredProc);


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

        public static void DelSchool(int Id)
        {
            DataPortal data = new DataPortal();
            try
            {
                data.AddIntParameter("@arg_id", Id);
                data.SubmitData("usp_ADM_DelSchools", DataPortal.QueryType.StoredProc);
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
    }
}

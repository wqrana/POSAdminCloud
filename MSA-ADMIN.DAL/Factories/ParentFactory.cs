using MSA_ADMIN.DAL.Common;
using MSA_ADMIN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Factories
{
    public class ParentFactory
    {
        public static List<Parent> GetParentList(int displayLenght, int displayStart, int sortColumnIndex, string sortDirection, out int totalDisplayRecords, long districtId, string searchValue, string searchBy)
        {
            totalDisplayRecords = 0;

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@DisplayLength", displayLenght);
                dataPortal.AddIntParameter("@DisplayStart", displayStart);
                dataPortal.AddIntParameter("@SortCol", sortColumnIndex);
                dataPortal.AddStringParameter("@SortDir", sortDirection);
                dataPortal.AddLongParameter("@DistrictId", districtId);
                dataPortal.AddStringParameter("@SearchValue", searchValue);
                dataPortal.AddStringParameter("@SearchBy", searchBy);

                reader = dataPortal.GetDataReader("[msa_GetParentList]", DataPortal.QueryType.StoredProc);
                List<Parent> parentList = PopulateParentListFromReader(reader, out totalDisplayRecords);
                return parentList;
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }
        public static Parent GetParent(int parentId)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@ParentId", parentId);

                reader = dataPortal.GetDataReader("[msa_GetParent]", DataPortal.QueryType.StoredProc);
                Parent parent = PopulateParentFromReader(reader);
                return parent;
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }
        public static List<Student> GetStudentList(int parentId)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@ParentId", parentId);

                reader = dataPortal.GetDataReader("[msa_GetStudentDetailByParentId]", DataPortal.QueryType.StoredProc);
                List<Student> studentList = PopulateStudentListFromReader(reader);
                return studentList;
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }
        public static List<Student> GetActiveStudentList(int parentId)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@ParentId", parentId);

                reader = dataPortal.GetDataReader("[msa_GetActiveStudentDetailByParentId]", DataPortal.QueryType.StoredProc);
                List<Student> studentList = PopulateActiveStudentListFromReader(reader);
                return studentList;
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }
        public static List<TransactionHistory> GetTransactionList(int displayLenght, int displayStart, int sortColumnIndex, string sortDirection, out int totalDisplayRecords, int parentId)
        {
            totalDisplayRecords = 0;

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@DisplayLength", displayLenght);
                dataPortal.AddIntParameter("@DisplayStart", displayStart);
                dataPortal.AddIntParameter("@SortCol", sortColumnIndex);
                dataPortal.AddStringParameter("@SortDir", sortDirection);
                dataPortal.AddIntParameter("@ParentId", parentId);

                reader = dataPortal.GetDataReader("[msa_GetTransactionDetailByParentId]", DataPortal.QueryType.StoredProc);
                List<TransactionHistory> transactionHistoryList = PopulateTransactionHistoryListFromReader(reader, out totalDisplayRecords);
                return transactionHistoryList;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }


        }
        public static int DeleteStudent(int studentId, int parentId)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@Student_Id", studentId);
                dataPortal.AddIntParameter("@Parent_Id", parentId);
                return dataPortal.SubmitData("[msa_RemoveStudent]", DataPortal.QueryType.StoredProc);
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static int UpdateParentEmail(int parentId, string email)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@ParentId", parentId);
                dataPortal.AddStringParameter("@NewEmail", email);
                return dataPortal.SubmitData("[msa_UpdateParentEmail]", DataPortal.QueryType.StoredProc);
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static int UpdateParentPassword(int parentId, string password)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddIntParameter("@ParentId", parentId);
                dataPortal.AddStringParameter("@NewPassword", password);
                return dataPortal.SubmitData("[msa_UpdateParentPassword]", DataPortal.QueryType.StoredProc);
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }

        public static bool GetDistOptionUseLivePOSDataFlag(long districtId)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            bool doesUseLivePOSData = false;

            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);

                reader = dataPortal.GetDataReader("[msa_GetDistrictOptionsByDistrictId]", DataPortal.QueryType.StoredProc);
                DistrictOption districtOption = PopulateDistrictOptionFromReader(reader);
                if (districtOption != null)
                {
                    doesUseLivePOSData = districtOption.useLivePOSData ?? false;
                }
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }

            return doesUseLivePOSData;
        }

        public static List<LowBalanceStudent> GetLowBalStudentsList(long districtId, int parentId)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddLongParameter("@DistrictId", districtId);
                dataPortal.AddIntParameter("@ParentId", parentId);
                reader = dataPortal.GetDataReader("msa_GetLowBalStudentsByParentId", DataPortal.QueryType.StoredProc);
                return PopulateLowBalStudentsList(reader);
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
                if (dataPortal != null)
                    dataPortal.Dispose();
            }
        }
        
        private static List<Parent> PopulateParentListFromReader(SafeDataReader reader, out int totalDisplayRecords)
        {
            List<Parent> parentList = new List<Parent>();
            totalDisplayRecords = 0;

            while (reader.Read())
            {
                totalDisplayRecords = reader.GetInt32("TotalDisplayCount");

                Parent parent = new Parent();

                parent.Id               = reader.GetInt32("Id");
                parent.FirstName        = reader.GetString("FIRSTNAME");
                parent.LastName         = reader.GetString("LASTNAME");
                parent.Address          = reader.GetString("ADDRESS");
                parent.City             = reader.GetString("CITY");
                parent.State            = reader.GetString("STATE");
                parent.Zip              = reader.GetString("ZIP");
                parent.Email            = reader.GetString("EMAIL");
                parent.Phone            = reader.GetString("PHONE");
                parent.UserID           = reader.GetString("USERID");
                parent.Password         = reader.GetString("PASSWORD");
                parent.SetupDate        = Convert.ToDateTime(reader.GetString("SETUP"));
                parent.LastLogin        = Convert.ToDateTime(reader.GetString("LSTLOGIN"));
                parent.NumberOfStudents = reader.GetInt32("STUDENTNUM");
                parentList.Add(parent);
            }
            return parentList;

        }
        private static Parent PopulateParentFromReader(SafeDataReader reader)
        {
            Parent parent = new Parent();

            //This reader will return us only one record. Because TOP 1 is used in Select Query
            while (reader.Read())
            {
                parent.Id                = reader.GetInt32("Id");
                parent.District_Id       = reader.GetInt32("District_Id");
                parent.UserID            = reader.GetString("UserID");
                parent.Password          = reader.GetString("Password");
                parent.SetupDate         = reader.GetDateTime("SetupDate");
                parent.VerifiedDate      = reader.GetDateTime("VerifiedDate");
                parent.LastLogin         = reader.GetDateTime("LastLogin");
                parent.Verified          = reader.GetBoolean("Verified");
                parent.EmailVerified     = reader.GetString("EmailVerified");
                parent.Email             = reader.GetString("Email");
                parent.VerificationCode  = reader.GetString("VerificationCode");
                parent.NumberOfStudents  = reader.GetInt32("NumberOfStudents");
                parent.LastName          = reader.GetString("LastName");
                parent.FirstName         = reader.GetString("FirstName");
                parent.Middle            = reader.GetString("Middle");
                parent.Address           = reader.GetString("Address");
                parent.City              = reader.GetString("City");
                parent.State             = reader.GetString("State");
                parent.Zip               = reader.GetString("Zip");
                parent.Phone             = reader.GetString("Phone");
                parent.PmtType           = reader.GetInt32("PmtType");
                parent.CheckRouting      = reader.GetString("CheckRouting");
                parent.CheckAccount      = reader.GetString("CheckAccount");
                parent.SaveRouting       = reader.GetString("SaveRouting");
                parent.SaveAccount       = reader.GetString("SaveAccount");
                parent.CreditCardAccount = reader.GetString("CreditCardAccount");
                parent.CreditCardExpire  = reader.GetString("CreditCardExpire");
                parent.LastTransaction   = reader.GetInt32("LastTransaction");
                parent.User_Group        = reader.GetString("User_Group");
                parent.CheckHash         = reader.GetString("CheckHash");
                parent.SaveHash          = reader.GetString("SaveHash");
                parent.CreditHash        = reader.GetString("CreditHash");
                parent.BadParent         = reader.GetBoolean("BadParent");
                parent.VIPNotify         = reader.GetBoolean("VIPNotify");
                parent.PaymentNotify     = reader.GetBoolean("PaymentNotify");
                parent.BalNotify         = reader.GetBoolean("BalNotify");
                parent.NotifyEmail       = reader.GetString("NotifyEmail");
                parent.LastInfoChange    = reader.GetDateTime("LastInfoChange");
                parent.isDisabled        = reader.GetBoolean("isDisabled");
                parent.DisabledDate      = reader.GetDateTime("DisabledDate");
                parent.AccountGuid       = reader.GetString("AccountGuid");
                parent.PreorderNotify    = reader.GetBoolean("PreorderNotify");
            }
            return parent;
        }
        private static List<Student> PopulateStudentListFromReader(SafeDataReader reader)
        {
            List<Student> studentList = new List<Student>();

            while (reader.Read())
            {
                Student student = new Student();

                student.UserId         = reader.GetString("UserID");
                student.FirstName      = reader.GetString("FirstName");
                student.LastName       = reader.GetString("LastName");
                student.SchoolName     = reader.GetString("SchoolName");
                student.Balance        = reader.GetString("Balance");
                student.Active         = reader.GetBoolean("Active");
                student.StudentId      = reader.GetInt32("StudentId");
                student.ClientCustId   = reader.GetInt64("Client_Cust_Id");
                student.DistrictCustId = reader.GetInt32("District_Cust_Id");

                studentList.Add(student);
            }
            return studentList;
        }
        private static List<TransactionHistory> PopulateTransactionHistoryListFromReader(SafeDataReader reader, out int totalDisplayRecords)
        {
            List<TransactionHistory> transactionHistoryList = new List<TransactionHistory>();
            totalDisplayRecords = 0;

            while (reader.Read())
            {
                totalDisplayRecords = reader.GetInt32("TotalDisplayCount");

                TransactionHistory transactionHistory = new TransactionHistory();

                transactionHistory.TransactionID    = reader.GetInt32("TransactionID");
                transactionHistory.TransactionDate  = reader.GetDateTime("TransactionDate");
                transactionHistory.PaymentType      = reader.GetString("PaymentType");
                transactionHistory.TransactionTotal = reader.GetDouble("TransactionTotal");
                transactionHistory.NsfFee           = Double.Parse(reader.GetDecimal("NsfFee").ToString());
                transactionHistory.ReturnReason     = reader.GetString("ReturnReason");
                transactionHistory.PaymentStatus    = reader.GetString("PaymentStatus");

                transactionHistoryList.Add(transactionHistory);
            }
            return transactionHistoryList;
        }
        private static List<Student> PopulateActiveStudentListFromReader(SafeDataReader reader)
        {
            List<Student> studentList = new List<Student>();

            while (reader.Read())
            {
                Student student = new Student();

                student.StudentId  = reader.GetInt32("StudentId");
                student.FirstName  = reader.GetString("FirstName");
                student.LastName   = reader.GetString("LastName");
                student.SchoolName = reader.GetString("SchoolName");
                student.Balance    = reader.GetString("Balance");
                student.Grade      = reader.GetString("Grade");
                student.HomeRoom   = reader.GetString("HomeRoom");
                student.DOB        = reader.GetDateTime("DOB");

                studentList.Add(student);
            }
            return studentList;
        }


        private static DistrictOption PopulateDistrictOptionFromReader(SafeDataReader reader)
        {
            DistrictOption districtOption = new DistrictOption();

            //This reader will return us only one record. Because TOP 1 is used in Select Query
            while (reader.Read())
            {
                districtOption.ID                                  = reader.GetInt64("ID");
                districtOption.District_ID                         = reader.GetInt32("District_ID");
                districtOption.ignoreDistrictBitValuesForReporting = reader.GetBoolean("ignoreDistrictBitValuesForReporting");
                districtOption.isStudentFreeTaxable                = reader.GetBoolean("isStudentFreeTaxable");
                districtOption.isStudentReducedTaxable             = reader.GetBoolean("isStudentReducedTaxable");
                districtOption.isStudentPaidTaxable                = reader.GetBoolean("isStudentPaidTaxable");
                districtOption.isMealPlanTaxable                   = reader.GetBoolean("isMealPlanTaxable");
                districtOption.isEmployeeTaxable                   = reader.GetBoolean("isEmployeeTaxable");
                districtOption.RemoveStalePreorderCartItems        = reader.GetBoolean("RemoveStalePreorderCartItems");
                districtOption.allowPreorderNegativeBalances       = reader.GetBoolean("allowPreorderNegativeBalances");
                districtOption.useNewCheckoutCart                  = reader.GetBoolean("useNewCheckoutCart");
                districtOption.loadResourcesFromSession            = reader.GetBoolean("loadResourcesFromSession");
                districtOption.DisplayMSAAlertsFirst               = reader.GetBoolean("DisplayMSAAlertsFirst");
                districtOption.useVariableCCFee                    = reader.GetBoolean("useVariableCCFee");
                districtOption.usePaymentCap                       = reader.GetBoolean("usePaymentCap");
                districtOption.useFiveDayWeekCutOff                = reader.GetBoolean("useFiveDayWeekCutOff");
                districtOption.useLivePOSData                      = reader.GetBoolean("useLivePOSData");
                districtOption.useCCPaymentCap                     = reader.GetBoolean("useCCPaymentCap");
                districtOption.useACHPaymentCap                    = reader.GetBoolean("useACHPaymentCap");
                districtOption.useReimbursablePreorder             = reader.GetBoolean("useReimbursablePreorder");
                districtOption.useSameDayOrdering                  = reader.GetBoolean("useSameDayOrdering");
            }
            return districtOption;
        }

        private static List<LowBalanceStudent> PopulateLowBalStudentsList(SafeDataReader reader)
        {
            List<LowBalanceStudent> lowBalStudentsList = new List<LowBalanceStudent>();
            while(reader.Read())
            {
                LowBalanceStudent student = new LowBalanceStudent();
                student.StudentId = reader.GetInt32("Id");
                student.StudentName = reader.GetString("StudentName");
                student.CurrentBalance = (decimal)reader.GetDouble("CurrentBalance");
                student.MinimumBalance = (decimal)reader.GetDouble("MinimumBal");
                student.IsNotifyEnabled = reader.GetBoolean("EnableNotify");

                lowBalStudentsList.Add(student);
            }

            return lowBalStudentsList;
        }
    }
}

using MSA_ADMIN.DAL.Common;
using MSA_ADMIN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Factories
{
    public class SettingsFactory
    {
        public static List<MSA_ADMIN.DAL.Models.School> GetSchools(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddLongParameter("@DistrictID", districtId);

                reader = dataPortal.GetDataReader("[msa_GetSchoolsByDistrictId]", DataPortal.QueryType.StoredProc);
                List<MSA_ADMIN.DAL.Models.School> schools = PopulateSchoolsFromReader(reader);
                return schools;
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

        //public static DistrictOption GetDistOption(long districtId)
        //{
        //    DataPortal dataPortal = new DataPortal();
        //    SafeDataReader reader = null;
        //    bool doesUseLivePOSData = false;

        //    try
        //    {
        //        dataPortal.AddLongParameter("@District_ID", districtId);

        //        reader = dataPortal.GetDataReader("[usp_ADMIN_getDistrictOptionsbyDistrict]", DataPortal.QueryType.StoredProc);
        //        DistrictOption districtOption = PopulateDistrictOptionFromReader(reader);
        //        return districtOption;
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
        //        if (dataPortal != null)
        //            dataPortal.Dispose();
        //    }

        //}


        public static int UpdateDistrictInformation(long districtId, string name, string phone, string email, 
                                                    bool lowBalance, bool transfer, bool studentAttach,
                                                    bool voids, bool adjustment, string preorderTaxableSchools,
                                                    string easypayTaxableSchools, bool cutOff_5, bool forcePayment)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {

                dataPortal.AddLongParameter("@DistrictID", districtId);
                dataPortal.AddStringParameter("@name", name);
                dataPortal.AddStringParameter("@phone", phone);
                dataPortal.AddStringParameter("@email", email);
                dataPortal.AddBoolParameter("@lowBalance", lowBalance);
                dataPortal.AddBoolParameter("@transfer", transfer);
                dataPortal.AddBoolParameter("@studentAttachment", studentAttach);
                dataPortal.AddBoolParameter("@voids", voids);
                dataPortal.AddBoolParameter("@adjustment", adjustment);
                dataPortal.AddStringParameter("@preorderTaxableSchools", preorderTaxableSchools);
                dataPortal.AddStringParameter("@easypayTaxableSchools", easypayTaxableSchools);
                dataPortal.AddBoolParameter("@useFiveDayWeekCutOff", cutOff_5);
                dataPortal.AddBoolParameter("@forcepayment", forcePayment);

                return dataPortal.SubmitData("[msa_UpdateDistrictInformation]", DataPortal.QueryType.StoredProc);
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


        public static int UpdateDistrictOptions(int districtId, bool validatedPreorderStatus, bool preorderNegative)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {

                dataPortal.AddBoolParameter("@RemoStaleItem", validatedPreorderStatus);
                dataPortal.AddLongParameter("@DistrictID", districtId);
                dataPortal.AddBoolParameter("@Allownegative", preorderNegative);
                return dataPortal.SubmitData("[msa_UpdateDistrictOptions]", DataPortal.QueryType.StoredProc);
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

        public static int UpdateCommunicationOptions(int districtId, bool displayMsaAlert)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {

                dataPortal.AddBoolParameter("@msaAlert", displayMsaAlert);
                dataPortal.AddIntParameter("@DistrictID", districtId);
                return dataPortal.SubmitData("[usp_ADMIN_UpdateCommOptions]", DataPortal.QueryType.StoredProc);
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






        private static List<MSA_ADMIN.DAL.Models.School> PopulateSchoolsFromReader(SafeDataReader reader)
        {
            List<MSA_ADMIN.DAL.Models.School> schools = new List<MSA_ADMIN.DAL.Models.School>();

            while (reader.Read())
            {
                MSA_ADMIN.DAL.Models.School school = new MSA_ADMIN.DAL.Models.School();

                school.Id                = reader.GetInt32("Id");
                school.SchoolName        = reader.GetString("SchoolName");
                school.isPreorderTaxable = reader.IsDBNull(reader.GetOrdinal("isPreorderTaxable")) ? (bool?)null : reader.GetBoolean("isPreorderTaxable");
                school.isEasyPayTaxable  = reader.IsDBNull(reader.GetOrdinal("isEasyPayTaxable")) ? (bool?)null : reader.GetBoolean("isEasyPayTaxable");

                schools.Add(school);
            }
            return schools;
        }


        //private static DistrictOption PopulateDistrictOptionFromReader(SafeDataReader reader)
        //{
        //    DistrictOption districtOption = new DistrictOption();

        //    //This reader will return us only one record. Because TOP 1 is used in Select Query
        //    while (reader.Read())
        //    {
        //        districtOption.ID                                  = reader.GetInt64("ID");
        //        districtOption.District_ID                         = reader.GetInt32("District_ID");
        //        districtOption.ignoreDistrictBitValuesForReporting = reader.GetBoolean("ignoreDistrictBitValuesForReporting");
        //        districtOption.isStudentFreeTaxable                = reader.GetBoolean("isStudentFreeTaxable");
        //        districtOption.isStudentReducedTaxable             = reader.GetBoolean("isStudentReducedTaxable");
        //        districtOption.isStudentPaidTaxable                = reader.GetBoolean("isStudentPaidTaxable");
        //        districtOption.isMealPlanTaxable                   = reader.GetBoolean("isMealPlanTaxable");
        //        districtOption.isEmployeeTaxable                   = reader.GetBoolean("isEmployeeTaxable");
        //        districtOption.RemoveStalePreorderCartItems        = reader.GetBoolean("RemoveStalePreorderCartItems");
        //        districtOption.allowPreorderNegativeBalances       = reader.GetBoolean("allowPreorderNegativeBalances");
        //        districtOption.useNewCheckoutCart                  = reader.GetBoolean("useNewCheckoutCart");
        //        districtOption.loadResourcesFromSession            = reader.GetBoolean("loadResourcesFromSession");
        //        districtOption.DisplayMSAAlertsFirst               = reader.GetBoolean("DisplayMSAAlertsFirst");
        //        districtOption.useVariableCCFee                    = reader.GetBoolean("useVariableCCFee");
        //        districtOption.usePaymentCap                       = reader.GetBoolean("usePaymentCap");
        //        districtOption.useFiveDayWeekCutOff                = reader.GetBoolean("useFiveDayWeekCutOff");
        //        districtOption.useLivePOSData                      = reader.GetBoolean("useLivePOSData");
        //        districtOption.useCCPaymentCap                     = reader.GetBoolean("useCCPaymentCap");
        //        districtOption.useACHPaymentCap                    = reader.GetBoolean("useACHPaymentCap");
        //        districtOption.useReimbursablePreorder             = reader.GetBoolean("useReimbursablePreorder");
        //        districtOption.useSameDayOrdering                  = reader.GetBoolean("useSameDayOrdering");
        //    }
        //    return districtOption;
        //}
    }
}

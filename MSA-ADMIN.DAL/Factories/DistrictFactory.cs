using MSA_ADMIN.DAL.Common;
using MSA_ADMIN.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MSA_ADMIN.DAL.Factories
{
    public class DistrictFactory
    {

        public static District DistrictInSession
        {
            get { return (District)HttpContext.Current.Session["District"]; }
            set { HttpContext.Current.Session["District"] = value; }
        }
        public static DistrictOption DistrictOptionInSession
        {
            get { return (DistrictOption)HttpContext.Current.Session["DistrictOption"]; }
            set { HttpContext.Current.Session["DistrictOption"] = value; }
        }



        public static MSA_ADMIN.DAL.Models.District GetDistrict(long districtId)
        {

            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            try
            {
                dataPortal.AddLongParameter("@DistrictID", districtId);

                reader = dataPortal.GetDataReader("[usp_ADMIN_GetDistrictData]", DataPortal.QueryType.StoredProc);
                MSA_ADMIN.DAL.Models.District district = PopulateDistrictFromReader(reader);
                return district;
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

        public static DistrictOption GetDistrictOption(long districtId)
        {
            DataPortal dataPortal = new DataPortal();
            SafeDataReader reader = null;
            bool doesUseLivePOSData = false;

            try
            {
                dataPortal.AddLongParameter("@District_ID", districtId);

                reader = dataPortal.GetDataReader("[usp_ADMIN_getDistrictOptionsbyDistrict]", DataPortal.QueryType.StoredProc);
                DistrictOption districtOption = PopulateDistrictOptionFromReader(reader);
                return districtOption;
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





        private static MSA_ADMIN.DAL.Models.District PopulateDistrictFromReader(SafeDataReader reader)
        {
            MSA_ADMIN.DAL.Models.District district = new MSA_ADMIN.DAL.Models.District();

            //This reader will return us only one record.
            while (reader.Read())
            {
                district.Id = reader.GetInt32("Id");
                district.Name = reader.GetString("Name");
                district.UserName = reader.GetString("UserName");
                district.Password = reader.GetString("Password");
                district.Address = reader.GetString("Address");
                district.City = reader.GetString("City");
                district.State = reader.GetString("State");
                district.Zip = reader.GetString("Zip");
                district.Phone = reader.GetString("Phone");
                district.Contact = reader.GetString("Contact");
                district.Email = reader.GetString("Email");
                district.BankName = reader.GetString("BankName");
                district.BankAddress = reader.GetString("BankAddress");
                district.BankCity = reader.GetString("BankCity");
                district.BankState = reader.GetString("BankState");
                district.BankZip = reader.GetString("BankZip");
                district.BankAccount = reader.GetString("BankAccount");
                district.BankRouting = reader.GetString("BankRouting");
                district.ClientVersion = reader.GetString("ClientVersion");
                district.POSVersion = reader.GetString("POSVersion");
                district.DistribVersion = reader.GetString("DistribVersion");
                district.NSFNotify = reader.GetString("NSFNotify");
                district.AdminUserName = reader.GetString("AdminUserName");
                district.AdminPassword = reader.GetString("AdminPassword");
                district.DistrictGUID = reader.GetString("DistrictGUID");
                district.IsActive = reader.IsDBNull(reader.GetOrdinal("IsActive")) ? (bool?)null : reader.GetBoolean("IsActive");
                district.isVisible = reader.IsDBNull(reader.GetOrdinal("isVisible")) ? (bool?)null : reader.GetBoolean("isVisible");
                district.DistrictPay = reader.IsDBNull(reader.GetOrdinal("DistrictPay")) ? (bool?)null : reader.GetBoolean("DistrictPay");
                district.UsingQuickZip = reader.IsDBNull(reader.GetOrdinal("UsingQuickZip")) ? (bool?)null : reader.GetBoolean("UsingQuickZip");
                district.allowACH = reader.IsDBNull(reader.GetOrdinal("allowACH")) ? (bool?)null : reader.GetBoolean("allowACH");
                district.allowCreditCard = reader.IsDBNull(reader.GetOrdinal("allowCreditCard")) ? (bool?)null : reader.GetBoolean("allowCreditCard");
                district.allowWebLunch = reader.IsDBNull(reader.GetOrdinal("allowWebLunch")) ? (bool?)null : reader.GetBoolean("allowWebLunch");
                district.AllowStudentUsageFee = reader.IsDBNull(reader.GetOrdinal("AllowStudentUsageFee")) ? (bool?)null : reader.GetBoolean("AllowStudentUsageFee");
                district.AllowEarlyBirdFee = reader.IsDBNull(reader.GetOrdinal("AllowEarlyBirdFee")) ? (bool?)null : reader.GetBoolean("AllowEarlyBirdFee");
                district.AllowFreeReduced = reader.IsDBNull(reader.GetOrdinal("AllowFreeReduced")) ? (bool?)null : reader.GetBoolean("AllowFreeReduced");
                district.VirtualDistrict = reader.IsDBNull(reader.GetOrdinal("VirtualDistrict")) ? (bool?)null : reader.GetBoolean("VirtualDistrict");
                district.showMessage = reader.IsDBNull(reader.GetOrdinal("showMessage")) ? (bool?)null : reader.GetBoolean("showMessage");
                district.AllowStudentTransfers = reader.IsDBNull(reader.GetOrdinal("AllowStudentTransfers")) ? (bool?)null : reader.GetBoolean("AllowStudentTransfers");
                district.allowEBilling = reader.IsDBNull(reader.GetOrdinal("allowEBilling")) ? (bool?)null : reader.GetBoolean("allowEBilling");
                district.allowFundraising = reader.IsDBNull(reader.GetOrdinal("allowFundraising")) ? (bool?)null : reader.GetBoolean("allowFundraising");
                district.allowRegistration = reader.IsDBNull(reader.GetOrdinal("allowRegistration")) ? (bool?)null : reader.GetBoolean("allowRegistration");
                district.allowSchoolDonation = reader.IsDBNull(reader.GetOrdinal("allowSchoolDonation")) ? (bool?)null : reader.GetBoolean("allowSchoolDonation");
                district.allowOnlineStore = reader.IsDBNull(reader.GetOrdinal("allowOnlineStore")) ? (bool?)null : reader.GetBoolean("allowOnlineStore");
                district.AllowManageStudents = reader.IsDBNull(reader.GetOrdinal("AllowManageStudents")) ? (bool?)null : reader.GetBoolean("AllowManageStudents");
                district.AllowVisibleManageStudents = reader.IsDBNull(reader.GetOrdinal("AllowVisibleManageStudents")) ? (bool?)null : reader.GetBoolean("AllowVisibleManageStudents");
                district.AllowVisibleDepositFunds = reader.IsDBNull(reader.GetOrdinal("AllowVisibleDepositFunds")) ? (bool?)null : reader.GetBoolean("AllowVisibleDepositFunds");
                district.AllowVisibleWebLunch = reader.IsDBNull(reader.GetOrdinal("AllowVisibleWebLunch")) ? (bool?)null : reader.GetBoolean("AllowVisibleWebLunch");
                district.AllowVisibleEbilling = reader.IsDBNull(reader.GetOrdinal("AllowVisibleEbilling")) ? (bool?)null : reader.GetBoolean("AllowVisibleEbilling");
                district.AllowVisibleOnlineStore = reader.IsDBNull(reader.GetOrdinal("AllowVisibleOnlineStore")) ? (bool?)null : reader.GetBoolean("AllowVisibleOnlineStore");
                district.ForceBalancePaymentOnPreorder = reader.IsDBNull(reader.GetOrdinal("ForceBalancePaymentOnPreorder")) ? (bool?)null : reader.GetBoolean("ForceBalancePaymentOnPreorder");
                district.DiscardPersonalData = reader.IsDBNull(reader.GetOrdinal("DiscardPersonalData")) ? (bool?)null : reader.GetBoolean("DiscardPersonalData");
                district.DisplayVoids = reader.IsDBNull(reader.GetOrdinal("DisplayVoids")) ? (bool?)null : reader.GetBoolean("DisplayVoids");
                district.DisplayAdjustments = reader.IsDBNull(reader.GetOrdinal("DisplayAdjustments")) ? (bool?)null : reader.GetBoolean("DisplayAdjustments");
                district.AllowMobile = reader.IsDBNull(reader.GetOrdinal("AllowMobile")) ? (bool?)null : reader.GetBoolean("AllowMobile");
                district.AllowTuition = reader.IsDBNull(reader.GetOrdinal("AllowTuition")) ? (bool?)null : reader.GetBoolean("AllowTuition");
                district.AllowVisibleTuition = reader.IsDBNull(reader.GetOrdinal("AllowVisibleTuition")) ? (bool?)null : reader.GetBoolean("AllowVisibleTuition");
                district.AllowEasyPay = reader.IsDBNull(reader.GetOrdinal("AllowEasyPay")) ? (bool?)null : reader.GetBoolean("AllowEasyPay");
                district.AllowVisibleEasyPay = reader.IsDBNull(reader.GetOrdinal("AllowVisibleEasyPay")) ? (bool?)null : reader.GetBoolean("AllowVisibleEasyPay");
                district.ForceMSAStudentLinkSC = reader.IsDBNull(reader.GetOrdinal("ForceMSAStudentLinkSC")) ? (bool?)null : reader.GetBoolean("ForceMSAStudentLinkSC");
                district.applyCCPaymentCapPerStudent = reader.IsDBNull(reader.GetOrdinal("applyCCPaymentCapPerStudent")) ? (bool?)null : reader.GetBoolean("applyCCPaymentCapPerStudent");
                district.applyACHPaymentCapPerStudent = reader.IsDBNull(reader.GetOrdinal("applyACHPaymentCapPerStudent")) ? (bool?)null : reader.GetBoolean("applyACHPaymentCapPerStudent");
                district.SetupType = reader.IsDBNull(reader.GetOrdinal("SetupType")) ? (int?)null : reader.GetInt32("SetupType");
                district.AccountType = reader.IsDBNull(reader.GetOrdinal("AccountType")) ? (int?)null : reader.GetInt32("AccountType");
                district.Vendor_Id = reader.IsDBNull(reader.GetOrdinal("Vendor_Id")) ? (int?)null : reader.GetInt32("Vendor_Id");
                district.ConnectionTime = reader.IsDBNull(reader.GetOrdinal("ConnectionTime")) ? (int?)null : reader.GetInt32("ConnectionTime");
                district.LoginAttempts = reader.IsDBNull(reader.GetOrdinal("LoginAttempts")) ? (int?)null : reader.GetInt32("LoginAttempts");
                district.DatabaseErrors = reader.IsDBNull(reader.GetOrdinal("DatabaseErrors")) ? (int?)null : reader.GetInt32("DatabaseErrors");
                district.AllowLBN = reader.IsDBNull(reader.GetOrdinal("AllowLBN")) ? (int?)null : reader.GetInt32("AllowLBN");
                district.ShoppingCart_Id = reader.IsDBNull(reader.GetOrdinal("ShoppingCart_Id")) ? (int?)null : reader.GetInt32("ShoppingCart_Id");
                district.AdminMessageAccepted = reader.IsDBNull(reader.GetOrdinal("AdminMessageAccepted")) ? (int?)null : reader.GetInt32("AdminMessageAccepted");
                district.AdminMessageCount = reader.IsDBNull(reader.GetOrdinal("AdminMessageCount")) ? (int?)null : reader.GetInt32("AdminMessageCount");
                district.AchFee = reader.IsDBNull(reader.GetOrdinal("AchFee")) ? (double?)null : reader.GetDouble("AchFee");
                district.CreditFee = reader.IsDBNull(reader.GetOrdinal("CreditFee")) ? (double?)null : reader.GetDouble("CreditFee");
                district.Deposit = reader.IsDBNull(reader.GetOrdinal("Deposit")) ? (double?)null : reader.GetDouble("Deposit");
                district.GatewayFee = reader.IsDBNull(reader.GetOrdinal("GatewayFee")) ? (double?)null : reader.GetDouble("GatewayFee");
                district.MasterCardFee = reader.IsDBNull(reader.GetOrdinal("MasterCardFee")) ? (double?)null : reader.GetDouble("MasterCardFee");
                district.VisaFee = reader.IsDBNull(reader.GetOrdinal("VisaFee")) ? (double?)null : reader.GetDouble("VisaFee");
                district.DiscoverFee = reader.IsDBNull(reader.GetOrdinal("DiscoverFee")) ? (double?)null : reader.GetDouble("DiscoverFee");
                district.AmxFee = reader.IsDBNull(reader.GetOrdinal("AmxFee")) ? (double?)null : reader.GetDouble("AmxFee");
                district.DefaultFee = reader.IsDBNull(reader.GetOrdinal("DefaultFee")) ? (double?)null : reader.GetDouble("DefaultFee");
                district.ShoppingCartFee = reader.IsDBNull(reader.GetOrdinal("ShoppingCartFee")) ? (double?)null : reader.GetDouble("ShoppingCartFee");
                district.TuitionFee = reader.IsDBNull(reader.GetOrdinal("TuitionFee")) ? (double?)null : reader.GetDouble("TuitionFee");
                district.VariableCCFee = reader.IsDBNull(reader.GetOrdinal("VariableCCFee")) ? (double?)null : reader.GetDouble("VariableCCFee");
                district.DefaultLBN = reader.IsDBNull(reader.GetOrdinal("DefaultLBN")) ? (double?)null : reader.GetDouble("DefaultLBN");
                district.CCPaymentCap = reader.IsDBNull(reader.GetOrdinal("CCPaymentCap")) ? (double?)null : reader.GetDouble("CCPaymentCap");
                district.ACHPaymentCap = reader.IsDBNull(reader.GetOrdinal("ACHPaymentCap")) ? (double?)null : reader.GetDouble("ACHPaymentCap");
                district.PaymentCap = reader.IsDBNull(reader.GetOrdinal("PaymentCap")) ? (double?)null : reader.GetDouble("PaymentCap");
                district.LastUpdate = reader.IsDBNull(reader.GetOrdinal("LastUpdate")) ? (DateTime?)null : reader.GetDateTime("LastUpdate");
                district.StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? (DateTime?)null : reader.GetDateTime("StartDate");
                district.StopDate = reader.IsDBNull(reader.GetOrdinal("StopDate")) ? (DateTime?)null : reader.GetDateTime("StopDate");
                district.UsageFeeExpireDate = reader.IsDBNull(reader.GetOrdinal("UsageFeeExpireDate")) ? (DateTime?)null : reader.GetDateTime("UsageFeeExpireDate");
                district.EarlyBirdExpireDate = reader.IsDBNull(reader.GetOrdinal("EarlyBirdExpireDate")) ? (DateTime?)null : reader.GetDateTime("EarlyBirdExpireDate");
                district.StudentUsageFee = reader.IsDBNull(reader.GetOrdinal("StudentUsageFee")) ? (decimal?)null : reader.GetDecimal("StudentUsageFee");
                district.EarlyBirdFee = reader.IsDBNull(reader.GetOrdinal("EarlyBirdFee")) ? (decimal?)null : reader.GetDecimal("EarlyBirdFee");

            }
            return district;
        }


        private static DistrictOption PopulateDistrictOptionFromReader(SafeDataReader reader)
        {
            DistrictOption districtOption = new DistrictOption();

            //This reader will return us only one record
            while (reader.Read())
            {
                districtOption.ID = reader.GetInt64("ID");
                districtOption.District_ID = reader.GetInt32("District_ID");
                districtOption.ignoreDistrictBitValuesForReporting = reader.GetBoolean("ignoreDistrictBitValuesForReporting");
                districtOption.isStudentFreeTaxable = reader.GetBoolean("isStudentFreeTaxable");
                districtOption.isStudentReducedTaxable = reader.GetBoolean("isStudentReducedTaxable");
                districtOption.isStudentPaidTaxable = reader.GetBoolean("isStudentPaidTaxable");
                districtOption.isMealPlanTaxable = reader.GetBoolean("isMealPlanTaxable");
                districtOption.isEmployeeTaxable = reader.GetBoolean("isEmployeeTaxable");
                districtOption.RemoveStalePreorderCartItems = reader.GetBoolean("RemoveStalePreorderCartItems");
                districtOption.allowPreorderNegativeBalances = reader.GetBoolean("allowPreorderNegativeBalances");
                districtOption.useNewCheckoutCart = reader.GetBoolean("useNewCheckoutCart");
                districtOption.loadResourcesFromSession = reader.GetBoolean("loadResourcesFromSession");
                districtOption.DisplayMSAAlertsFirst = reader.GetBoolean("DisplayMSAAlertsFirst");
                districtOption.useVariableCCFee = reader.GetBoolean("useVariableCCFee");
                districtOption.usePaymentCap = reader.GetBoolean("usePaymentCap");
                districtOption.useFiveDayWeekCutOff = reader.GetBoolean("useFiveDayWeekCutOff");
                districtOption.useLivePOSData = reader.GetBoolean("useLivePOSData");
                districtOption.useCCPaymentCap = reader.GetBoolean("useCCPaymentCap");
                districtOption.useACHPaymentCap = reader.GetBoolean("useACHPaymentCap");
                districtOption.useReimbursablePreorder = reader.GetBoolean("useReimbursablePreorder");
                districtOption.useSameDayOrdering = reader.GetBoolean("useSameDayOrdering");
            }
            return districtOption;
        }

    }

}



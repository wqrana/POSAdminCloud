using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public class District
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsActive { get; set; }
        public bool? isVisible { get; set; }
        public int? SetupType { get; set; }
        public double? AchFee { get; set; }
        public double? CreditFee { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankCity { get; set; }
        public string BankState { get; set; }
        public string BankZip { get; set; }
        public string BankAccount { get; set; }
        public string BankRouting { get; set; }
        public int? AccountType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public bool? DistrictPay { get; set; }
        public double? Deposit { get; set; }
        public int? Vendor_Id { get; set; }
        public string ClientVersion { get; set; }
        public int? ConnectionTime { get; set; }
        public int? LoginAttempts { get; set; }
        public int? DatabaseErrors { get; set; }
        public bool? UsingQuickZip { get; set; }
        public string POSVersion { get; set; }
        public string DistribVersion { get; set; }
        public string NSFNotify { get; set; }
        public string AdminUserName { get; set; }
        public string AdminPassword { get; set; }
        public bool? allowACH { get; set; }
        public bool? allowCreditCard { get; set; }
        public bool? allowWebLunch { get; set; }
        public bool? AllowStudentUsageFee { get; set; }
        public decimal? StudentUsageFee { get; set; }
        public DateTime? UsageFeeExpireDate { get; set; }
        public bool? AllowEarlyBirdFee { get; set; }
        public decimal? EarlyBirdFee { get; set; }
        public DateTime? EarlyBirdExpireDate { get; set; }
        public double? GatewayFee { get; set; }
        public double? MasterCardFee { get; set; }
        public double? VisaFee { get; set; }
        public double? DiscoverFee { get; set; }
        public double? AmxFee { get; set; }
        public double? DefaultFee { get; set; }
        public int? AllowLBN { get; set; }
        public bool? AllowFreeReduced { get; set; }
        public bool? VirtualDistrict { get; set; }
        public bool? showMessage { get; set; }
        public bool? AllowStudentTransfers { get; set; }
        public int? ShoppingCart_Id { get; set; }
        public string DistrictGUID { get; set; }
        public double? ShoppingCartFee { get; set; }
        public bool? allowEBilling { get; set; }
        public bool? allowFundraising { get; set; }
        public bool? allowRegistration { get; set; }
        public bool? allowSchoolDonation { get; set; }
        public bool? allowOnlineStore { get; set; }
        public bool? AllowManageStudents { get; set; }
        public bool? AllowVisibleManageStudents { get; set; }
        public bool? AllowVisibleDepositFunds { get; set; }
        public bool? AllowVisibleWebLunch { get; set; }
        public bool? AllowVisibleEbilling { get; set; }
        public bool? AllowVisibleOnlineStore { get; set; }
        public bool? ForceBalancePaymentOnPreorder { get; set; }
        public int? AdminMessageAccepted { get; set; }
        public int? AdminMessageCount { get; set; }
        public bool? DiscardPersonalData { get; set; }
        public bool? DisplayVoids { get; set; }
        public bool? DisplayAdjustments { get; set; }
        public bool? AllowMobile { get; set; }
        public bool? AllowTuition { get; set; }
        public bool? AllowVisibleTuition { get; set; }
        public double? TuitionFee { get; set; }
        public bool? AllowEasyPay { get; set; }
        public bool? AllowVisibleEasyPay { get; set; }
        public bool? ForceMSAStudentLinkSC { get; set; }
        public double? VariableCCFee { get; set; }
        public double? DefaultLBN { get; set; }
        public double? CCPaymentCap { get; set; }
        public bool? applyCCPaymentCapPerStudent { get; set; }
        public double? ACHPaymentCap { get; set; }
        public bool? applyACHPaymentCapPerStudent { get; set; }
        public double? PaymentCap { get; set; }

    }
}

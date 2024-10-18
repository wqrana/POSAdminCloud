using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    [Serializable]
    public class DistrictOption
    {
        public long ID { get; set; }

        public int District_ID { get; set; }

        public bool? ignoreDistrictBitValuesForReporting { get; set; }

        public bool? isStudentFreeTaxable { get; set; }

        public bool? isStudentReducedTaxable { get; set; }

        public bool? isStudentPaidTaxable { get; set; }

        public bool? isMealPlanTaxable { get; set; }

        public bool? isEmployeeTaxable { get; set; }

        public bool? RemoveStalePreorderCartItems { get; set; }

        public bool? allowPreorderNegativeBalances { get; set; }

        public bool? useNewCheckoutCart { get; set; }

        public bool? loadResourcesFromSession { get; set; }

        public bool? DisplayMSAAlertsFirst { get; set; }

        public bool? useVariableCCFee { get; set; }

        public bool? usePaymentCap { get; set; }

        public bool? useFiveDayWeekCutOff { get; set; }

        public bool? useLivePOSData { get; set; }

        public bool? useCCPaymentCap { get; set; }

        public bool? useACHPaymentCap { get; set; }

        public bool? useReimbursablePreorder { get; set; }

        public bool? useSameDayOrdering { get; set; }

        //public virtual District District { get; set; }
    }
}

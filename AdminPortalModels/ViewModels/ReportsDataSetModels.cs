using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{
    class ReportsDataSetModels
    {
        public partial class DepositTicket
        {
            public int CASHRESID { get; set; }
            public int ORDID { get; set; }
            public int CSTID { get; set; }
            public int POSID { get; set; }
            public int DISTID { get; set; }
            public int SCHID { get; set; }
            public int DepositType { get; set; }
            public int PaymentType { get; set; }
            public string PaymentTypeName { get; set; }
            public System.DateTime OrderDate { get; set; }
            public System.DateTime ReportDate { get; set; }
            public string DistrictName { get; set; }
            public string DistAddress1 { get; set; }
            public string DistAddress2 { get; set; }
            public string DistCity { get; set; }
            public string DistState { get; set; }
            public string DistZip { get; set; }
            public string DistBankName { get; set; }
            public string DistBankAddress1 { get; set; }
            public string DistBankAddress2 { get; set; }
            public string DistBankCity { get; set; }
            public string DistBankState { get; set; }
            public string DistBankZip { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Middle { get; set; }
            public int CheckNumber { get; set; }
            public double CheckAmount { get; set; }
            public double POSCheckAmount { get; set; }
            public double AdminCheckAmount { get; set; }
            public double CreditAmount { get; set; }
            public double POSCreditAmount { get; set; }
            public double AdminCreditAmount { get; set; }
            public double CashAmount { get; set; }
            public double MiscCashAmount { get; set; }
            public double POSCashAmount { get; set; }
            public double AdminCashAmount { get; set; }
            public double POSTotalAmount { get; set; }
            public double AdminTotalAmount { get; set; }
            public double CashOutAmount { get; set; }
            public double Amount { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class TransactionHistory
    {
        public int TransactionID { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string PaymentType { get; set; }
        public double? TransactionTotal { get; set; }
        public double? NsfFee { get; set; }
        public string ReturnReason { get; set; }
        public string PaymentStatus { get; set; }
    }
}

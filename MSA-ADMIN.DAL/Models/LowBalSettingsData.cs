using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class LowBalanceStudent
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal MinimumBalance { get; set; }
        public bool IsNotifyEnabled { get; set; }
    }
}

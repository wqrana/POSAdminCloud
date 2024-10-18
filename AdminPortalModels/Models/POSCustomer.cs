using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.Models
{
    public class POSCustomer
    {
        public long CustomerID { get; set; }
        public string UserId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? DOB { get; set; }
        public string SchoolName { get; set; }
        public long? Grade { get; set; }
        public string HomeRoom { get; set; }
        public double Balance { get; set; }
        public long? Local_ID { get; set; }
        public bool Active { get; set; }
    }
}

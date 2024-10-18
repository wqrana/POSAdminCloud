using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SchoolName { get; set; }
        public string Balance { get; set; }
        public bool Active { get; set; }
        public DateTime? DOB { get; set; }
        public string Grade { get; set; }
        public string HomeRoom { get; set; }
        public long? ClientCustId { get; set; }
        public int? DistrictCustId { get; set; }
    }
}

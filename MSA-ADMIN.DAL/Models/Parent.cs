using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSA_ADMIN.DAL.Models
{
    public class Parent
    {
        public int Id { get; set; }
        public int District_Id { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public DateTime? SetupDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool? Verified { get; set; }
        public string EmailVerified { get; set; }
        public string Email { get; set; }
        public string VerificationCode { get; set; }
        public int? NumberOfStudents { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Middle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public int? PmtType { get; set; }
        public string CheckRouting { get; set; }
        public string CheckAccount { get; set; }
        public string SaveRouting { get; set; }
        public string SaveAccount { get; set; }
        public string CreditCardAccount { get; set; }
        public string CreditCardExpire { get; set; }
        public int? LastTransaction { get; set; }
        public string User_Group { get; set; }
        public string CheckHash { get; set; }
        public string SaveHash { get; set; }
        public string CreditHash { get; set; }
        public bool? BadParent { get; set; }
        public bool? VIPNotify { get; set; }
        public bool? PaymentNotify { get; set; }
        public bool? BalNotify { get; set; }
        public string NotifyEmail { get; set; }
        public DateTime? LastInfoChange { get; set; }
        public bool? isDisabled { get; set; }
        public DateTime? DisabledDate { get; set; }
        public string AccountGuid { get; set; }
        public bool? PreorderNotify { get; set; }

        //public string ParentName { get; set; }
    }
}

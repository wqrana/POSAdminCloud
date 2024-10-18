using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.ViewModels
{
    [Serializable]
    public class ApplicationFilters
    {
        public string SearchBy { get; set; }
        public int? SearchBy_Id { get; set; }
        public DateTime? SignedDate { get; set; }
        public int? ApprovalStatus { get; set; }
        public bool? Entered { get; set; }
        public bool? Updated { get; set; }
    }

    public class Application
    {
        public Nullable<int> AllRecordsCount { get; set; }
        public Nullable<long> Application_Id { get; set; }
        public string Parent_Name { get; set; }
        public Nullable<int> Parent_Id { get; set; }
        public string Member_Name { get; set; }
        public Nullable<int> Member_Id { get; set; }
        public string Student_Name { get; set; }
        public Nullable<int> Student_Id { get; set; }
        public int? District_Id { get; set; }
        public string District_Name { get; set; }
        public Nullable<int> Household_Size { get; set; }
        public DateTime? DateEntered { get; set; }
        public string POS_Status { get; set; }
        public bool? Precertified { get; set; }
        public string Status { get; set; }
        public string DirectCert { get; set; }
        public Nullable<int> No_Of_Students { get; set; }
        public Nullable<int> No_Of_Members { get; set; }
        public string Beneficiary_Name { get; set; }
        public string Case_Number { get; set; }
        public string SpecialCircum { get; set; }
        public bool? IsPrinted { get; set; }
        public DateTime? PrintedDate { get; set; }
        public Nullable<int> Approval_Status { get; set; }
        public Nullable<bool> Entered { get; set; }
        public Nullable<bool> Updated { get; set; }
        public string App_Signer_Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Ethnicity { get; set; }
        public string Race { get; set; }
        public string Email { get; set; }
        public DateTime? SignedDate { get; set; }
        public string ZIP { get; set; }
        public string City { get; set; }
        public string SSN { get; set; }
        public string Phone { get; set; }
        public string Cell_Phone { get; set; }
        public string State { get; set; }
        public string Comments { get; set; }
        public IEnumerable<AppStudent> Students { get; set; }
        public IEnumerable<AppMember> StudentsNotEnrolled { get; set; }
        public IEnumerable<AppMember> Members { get; set; }
        public double? StudentsTotalIncome { get; set; }
        public int? StudentsTotalIncomeFreq { get; set; }
        public double? TotalIncome { get; set; }
        public int? TotalIncomeFreq { get; set; }
        public bool IsStep3
        {
            get
            {
                return string.IsNullOrEmpty(this.Case_Number);
            }
        }
    }

    public class Income
    {
        public bool HasIncome { get; set; }
        public double? Job1Income { get; set; }
        public int? Job1FrequencyId { get; set; }
        public string Job1FrequencyName { get; set; }
        public int? Job1FrequencyMultiplier { get; set; }

        public double? Job2Income { get; set; }
        public int? Job2FrequencyId { get; set; }
        public string Job2FrequencyName { get; set; }
        public int? Job2FrequencyMultiplier { get; set; }


        public double? Job3Income { get; set; }
        public int? Job3FrequencyId { get; set; }
        public string Job3FrequencyName { get; set; }
        public int? Job3FrequencyMultiplier { get; set; }


        public double? WelfareIncome { get; set; }
        public int? WelfareFrequencyId { get; set; }
        public string WelfareFrequencyName { get; set; }
        public int? WelfareFrequencyMultiplier { get; set; }


        public double? PensionIncome { get; set; }
        public int? PensionFrequencyId { get; set; }
        public string PensionFrequencyName { get; set; }
        public int? PensionFrequencyMultiplier { get; set; }


        public double? OtherIncome { get; set; }
        public int? OtherFrequencyId { get; set; }
        public string OtherFrequencyName { get; set; }
        public int? OtherFrequencyMultiplier { get; set; }
    }

    public class AppStudent : Income
    {
        public long Id { get; set; }
        public string StudentName { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle { get; set; }
        public string Homeroom { get; set; }
        public string Grade { get; set; }
        public bool? Foster { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? POS_Status { get; set; }
        public int? Status { get; set; }
        public int? DirectCert { get; set; }
        public bool? Precertified { get; set; }
    }

    public class AppMember : Income
    {
        public long Id { get; set; }
        public string MemberName { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Middle { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string SSN { get; set; }
        public bool? FosterChild { get; set; }
        public bool IsStudent { get; set; }
        public int? Status { get; set; }
        public int? DirectCert { get; set; }
        public bool? Precertified { get; set; }

        public double? WorkEarningsTotal { get; set; }
        public int? WorkingEarningTotalFreq { get; set; }

        public double? WelfareTotalIncome { get; set; }
        public int? WelfareTotalIncomeFreq { get; set; }

        public double? OtherTotalIncome { get; set; }
        public int? OtherTotalIncomeFreq { get; set; }
    }

    public class IncomeFrequency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Multiplier { get; set; }
    }
}

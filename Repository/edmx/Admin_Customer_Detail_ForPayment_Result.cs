//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository.edmx
{
    using System;
    
    public partial class Admin_Customer_Detail_ForPayment_Result
    {
        public long Customer_Id { get; set; }
        public long District_Id { get; set; }
        public string District_Name { get; set; }
        public Nullable<long> School_Id { get; set; }
        public string School_Name { get; set; }
        public Nullable<long> Grade_Id { get; set; }
        public string Grade { get; set; }
        public Nullable<long> Homeroom_Id { get; set; }
        public string Homeroom { get; set; }
        public Nullable<long> Language_Id { get; set; }
        public string Language { get; set; }
        public Nullable<int> Ethnicity_Id { get; set; }
        public string Ethnicity { get; set; }
        public string UserID { get; set; }
        public string PIN { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Middle { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string Customer_Addr1 { get; set; }
        public string Customer_Addr2 { get; set; }
        public string Customer_City { get; set; }
        public string Customer_State { get; set; }
        public string Customer_Zip { get; set; }
        public string Customer_Phone { get; set; }
        public string Customer_Notes { get; set; }
        public string EMail { get; set; }
        public Nullable<System.DateTime> Date_Of_Birth { get; set; }
        public Nullable<System.DateTime> GraduationDate { get; set; }
        public Nullable<bool> GraduationDateSet { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public Nullable<bool> NotInDistrict { get; set; }
        public Nullable<int> LunchType { get; set; }
        public string LunchType_String { get; set; }
        public Nullable<bool> Student { get; set; }
        public Nullable<bool> Snack_Participant { get; set; }
        public Nullable<bool> Student_Worker { get; set; }
        public Nullable<bool> AllowAlaCarte { get; set; }
        public Nullable<bool> No_Credit_On_Account { get; set; }
        public double BonusBalance { get; set; }
        public double MealPlanBalance { get; set; }
        public double AlaCarteBalance { get; set; }
        public double TotalBalance { get; set; }
        public string PictureExtension { get; set; }
        public string StorageAccountName { get; set; }
        public string ContainerName { get; set; }
        public string PictureFileName { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public int AllowBiometrics { get; set; }
        public Nullable<bool> Allow_ACH { get; set; }
    }
}
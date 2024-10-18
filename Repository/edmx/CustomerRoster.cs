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
    using System.Collections.Generic;
    
    public partial class CustomerRoster
    {
        public long ClientID { get; set; }
        public long CSTID { get; set; }
        public long DISTID { get; set; }
        public Nullable<long> LANGUAGEID { get; set; }
        public Nullable<long> SCHID { get; set; }
        public Nullable<long> GRID { get; set; }
        public Nullable<long> HRID { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> GraduationDate { get; set; }
        public Nullable<System.DateTime> DeactiveDate { get; set; }
        public Nullable<System.DateTime> ReactiveDate { get; set; }
        public bool isActive { get; set; }
        public bool isDeleted { get; set; }
        public int isActiveInt { get; set; }
        public int isDeletedInt { get; set; }
        public Nullable<bool> AllowBio { get; set; }
        public Nullable<bool> BioPresent { get; set; }
        public string UserID { get; set; }
        public string PIN { get; set; }
        public Nullable<bool> AllowAlaCarte { get; set; }
        public Nullable<bool> CashOnly { get; set; }
        public int LunchType { get; set; }
        public Nullable<bool> isStudent { get; set; }
        public int isFrozen { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Middle { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string EMail { get; set; }
        public string ExtraInfo { get; set; }
        public string SSN { get; set; }
        public string Grade { get; set; }
        public string Homeroom { get; set; }
        public string SchoolName { get; set; }
        public string SchoolID { get; set; }
        public string SchoolAddress1 { get; set; }
        public string SchoolAddress2 { get; set; }
        public string SchoolCity { get; set; }
        public string SchoolState { get; set; }
        public string SchoolZip { get; set; }
        public string SchoolPhone1 { get; set; }
        public string SchoolPhone2 { get; set; }
        public string DistrictName { get; set; }
        public string DistrictAddress1 { get; set; }
        public string DistrictAddress2 { get; set; }
        public string DistrictCity { get; set; }
        public string DistrictState { get; set; }
        public string DistrictZip { get; set; }
        public string DistrictPhone1 { get; set; }
        public string DistrictPhone2 { get; set; }
        public string LunchTypeStatus { get; set; }
        public int LunchTypeSort { get; set; }
        public string MealPlanAssignment { get; set; }
        public Nullable<int> NumMealsLeft { get; set; }
        public string ActiveStatus { get; set; }
        public Nullable<int> ActiveStatusSort { get; set; }
        public int QTYPaid { get; set; }
        public int QTYReduced { get; set; }
        public int QTYFree { get; set; }
        public int QTYAdult { get; set; }
        public int QTYMealPlan { get; set; }
        public int QTYPFR { get; set; }
    }
}
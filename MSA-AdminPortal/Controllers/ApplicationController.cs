using AdminPortalModels.ViewModels;
using Repository;
using Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using AdminPortalModels.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using MSA_AdminPortal.Helpers;
using System.Configuration;
using System.Text;
using SelectPdf;
using System.Linq.Expressions;
using Repository.edmx;

namespace MSA_AdminPortal.Controllers
{
    public class ApplicationController : BaseAuthorizedController
    {
        private IApplicationRepository applicationRepository;
        private UnitOfWork unitOfWork;
        /// <summary>
        /// Constructer for class
        /// </summary>
        public ApplicationController()
        {
            unitOfWork = new UnitOfWork(ClientInfoData.getConectionString());
            this.applicationRepository = unitOfWork.ApplicationRepository;
        }

        /// <summary>
        /// Initially populates model
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (!SecurityManager.ViewApplications) return RedirectToAction("NoAccess", "Security", new { id = "noapplications" });
            var clientId = ClientInfoData.GetClientID();
            try
            {
                ViewBag.SearchByOptions = this.GetSearchByOptions();
                ViewBag.ApprovalStatusOptions = this.GetApprovalStatusOptions();
                ViewBag.EnteredOptions = this.GetEnteredOptions();
                ViewBag.UpdatedOptions = this.GetUpdatedOptions();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Index");
            }
            return View();
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param, ApplicationFilters filters)
        {
            try
            {
                int totalRecords;
                long ClientId = ClientInfoData.GetClientID();
                int sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                string sortDirection = Request["sSortDir_0"];

                string[] columns = param.sColumns.Split(',');
                string sortColumn = columns[sortColumnIndex];

                var applicationsData = applicationRepository.GetApplicationsList(ClientId, param.iDisplayStart, param.iDisplayLength, sortColumnIndex, sortColumn, sortDirection, filters, out totalRecords);
                if (applicationsData != null)
                {
                    var result = from a in applicationsData
                                 select new[] {"", a.Application_Id.ToString(), a.Student_Name, a.Member_Name, a.District_Name, a.Household_Size.HasValue ? a.Household_Size.Value.ToString() : "0", a.No_Of_Students.ToString(), a.No_Of_Members.ToString(), a.App_Signer_Name, a.Approval_Status.HasValue ? a.Approval_Status.Value.ToString() : "-1" };

                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = totalRecords,
                        iTotalDisplayRecords = totalRecords,
                        aaData = result
                    },
                    JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        sEcho = param.sEcho,
                        iTotalRecords = 0,
                        iTotalDisplayRecords = 0,
                        aaData = new string[0]
                    },
                    JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxHandler");
                return null;
            }
        }

        public ActionResult AjaxHandlerStudentsList(JQueryDataTableParamModel param, int? appId)
        {
            try
            {
                var Students = this.LoadStudents(appId);

                var result = Students.Select(x => new
                {
                    Id = x.Id.ToString(),
                    StudentName = x.StudentName,
                    Homeroom = x.Homeroom,
                    DateOfBirth = !x.DateOfBirth.HasValue ? "" : x.DateOfBirth.Value.ToShortDateString(),
                    HasIncome = x.HasIncome ? "Yes" : "No",
                    POS_Status = x.POS_Status,
                    Status = x.Status,
                    DirectCert = x.DirectCert,
                    Precertified = x.Precertified,
                    Job1FrequencyName = x.Job1FrequencyName,
                    Job1Income = !x.Job1Income.HasValue ? "" : x.Job1Income.Value.ToString(),
                    Job2FrequencyName = x.Job2FrequencyName,
                    Job2Income = !x.Job2Income.HasValue ? "" : x.Job2Income.Value.ToString(),
                    Job3FrequencyName = x.Job3FrequencyName,
                    Job3Income = !x.Job3Income.HasValue ? "" : x.Job3Income.Value.ToString(),
                    WelfareFrequencyName = x.WelfareFrequencyName,
                    WelfareIncome = !x.WelfareIncome.HasValue ? "" : x.WelfareIncome.Value.ToString(),
                    PensionFrequencyName = x.PensionFrequencyName,
                    PensionIncome = !x.PensionIncome.HasValue ? "" : x.PensionIncome.Value.ToString(),
                    OtherFrequencyName = x.OtherFrequencyName,
                    OtherIncome = !x.OtherIncome.HasValue ? "" : x.OtherIncome.Value.ToString(),
                }).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = result.Count(),
                    iTotalDisplayRecords = result.Count(),
                    aaData = result
                },
             JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxHandlerStudentsList");
                return null;
            }
        }

        public ActionResult AjaxHandlerMembersList(JQueryDataTableParamModel param, int? appId)
        {
            try
            {
                var members = this.LoadMembers(appId);

                var result = members.Select(x => new
                {
                    Id = x.Id.ToString(),
                    MemberName = x.MemberName,
                    DateOfBirth = !x.DateOfBirth.HasValue ? "" : x.DateOfBirth.Value.ToShortDateString(),
                    Email = x.Email,
                    SSN = x.SSN,
                    FosterChild = x.FosterChild,
                    HasIncome = x.HasIncome ? "Yes" : "No",
                    IsStudent = x.IsStudent,
                    Status = x.Status,
                    DirectCert = x.DirectCert,
                    Precertified = x.Precertified,
                    Job1FrequencyName = x.Job1FrequencyName,
                    Job1Income = !x.Job1Income.HasValue ? "" : x.Job1Income.Value.ToString(),
                    Job2FrequencyName = x.Job2FrequencyName,
                    Job2Income = !x.Job2Income.HasValue ? "" : x.Job2Income.Value.ToString(),
                    Job3FrequencyName = x.Job3FrequencyName,
                    Job3Income = !x.Job3Income.HasValue ? "" : x.Job3Income.Value.ToString(),
                    WelfareFrequencyName = x.WelfareFrequencyName,
                    WelfareIncome = !x.WelfareIncome.HasValue ? "" : x.WelfareIncome.Value.ToString(),
                    PensionFrequencyName = x.PensionFrequencyName,
                    PensionIncome = !x.PensionIncome.HasValue ? "" : x.PensionIncome.Value.ToString(),
                    OtherFrequencyName = x.OtherFrequencyName,
                    OtherIncome = !x.OtherIncome.HasValue ? "" : x.OtherIncome.Value.ToString(),
                }).ToList();

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = result.Count(),
                    iTotalDisplayRecords = result.Count(),
                    aaData = result
                },
             JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AjaxHandlerMembersList");
                return null;
            }
        }

        private List<AppStudent> LoadStudents(int? appId)
        {
            long ClientId = ClientInfoData.GetClientID();

            var results =

                from cust in this.unitOfWork.CustomerRepository.GetQuery()
                join appMem in this.unitOfWork.App_MembersRepository.GetQuery() on cust.ID equals appMem.Customer_Id
                join mem_income in this.unitOfWork.App_Member_IncomesRepository.GetQuery() on appMem.Id equals mem_income.App_Member_Id
                into incomeData
                from income in incomeData.DefaultIfEmpty()
                join hom in this.unitOfWork.HomeroomRepository.GetQuery() on cust.Homeroom_Id equals hom.ID
                into homeData
                from home in homeData.DefaultIfEmpty()
                join grad in this.unitOfWork.GradeRepository.GetQuery() on cust.Grade_Id equals grad.ID
                into gradeData
                from grade in gradeData.DefaultIfEmpty()
                join stat in this.unitOfWork.App_Member_StatusesRepository.GetQuery() on appMem.Id equals stat.App_Member_Id
                into statusData
                from status in statusData.DefaultIfEmpty()
                where appMem.Application_Id == appId && appMem.ClientID == ClientId && appMem.isStudent && appMem.Member_Id == null
                select new
                {
                    CustId = cust.ID,
                    AppMemId = appMem.Id,
                    DOB = cust.DOB,
                    FirstName = cust.FirstName,
                    LastName = cust.LastName,
                    Middle = cust.Middle,
                    HomeRoom = home.Name,
                    Grade = grade.Name,
                    Foster = appMem.Foster,
                    NoIncome = appMem.NoIncome,
                    Income_Type_Id = (income != null) ? income.Income_Type_Id : 0,
                    Frequency_Id = (income != null) ? income.Frequency_Id : 0,
                    Income = (income != null) ? income.Income : 0.0,
                    POS_Status = cust.LunchType,
                    Status = status.Status_Type_Id,
                    DirectCert = status.Direct_Cert_Id,
                    Precertified = status.Precertified
                };

            // do not need to group data in db. It will generate a slower query.
            var groups = results.AsEnumerable().GroupBy(x => x.AppMemId);
            List<AppStudent> Students = new List<AppStudent>();

            foreach (var group in groups)
            {
                AppStudent student = new AppStudent();
                var member = group.FirstOrDefault();

                student.Id = member.CustId;
                student.DateOfBirth = member.DOB;
                student.StudentName = member.FirstName + " " + member.LastName;
                student.First_Name = member.FirstName;
                student.Last_Name = member.LastName;
                student.Middle = member.Middle;
                student.Homeroom = member.HomeRoom;
                student.Grade = member.Grade;
                student.Foster = member.Foster;
                student.HasIncome = member.NoIncome.HasValue ? !member.NoIncome.Value : false;

                student.POS_Status = member.POS_Status;
                student.Status = member.Status;
                student.DirectCert = member.DirectCert;
                student.Precertified = member.Precertified;

                if (student.HasIncome)
                {
                    student = this.LoadMemberIncome(group, student) as AppStudent;
                }

                Students.Add(student);
            }

            return Students;
        }

        private List<AppMember> LoadMembers(int? appId, Expression<Func<App_Members, bool>> filter = null)
        {
            long ClientId = ClientInfoData.GetClientID();

            var results =

                from mem in this.unitOfWork.MembersRepository.GetQuery()
                join appMem in this.unitOfWork.App_MembersRepository.GetQuery(filter) on mem.Id equals appMem.Member_Id
                join mem_income in this.unitOfWork.App_Member_IncomesRepository.GetQuery() on appMem.Id equals mem_income.App_Member_Id
                into incomeData
                from income in incomeData.DefaultIfEmpty()
                join stat in this.unitOfWork.App_Member_StatusesRepository.GetQuery() on appMem.Id equals stat.App_Member_Id
                into statusData
                from status in statusData.DefaultIfEmpty()
                where appMem.Application_Id == appId && appMem.ClientID == ClientId && appMem.Customer_Id == null && appMem.Member_Id != null
                select new
                {
                    MemberId = mem.Id,
                    AppMemId = appMem.Id,
                    DOB = mem.DOB,
                    Email = mem.Email,
                    SSN = mem.SSN,
                    FosterChild = appMem.Foster,
                    FirstName = mem.First_Name,
                    LastName = mem.Last_Name,
                    Middle = mem.Middle,
                    NoIncome = appMem.NoIncome,
                    IsStudent = appMem.isStudent,
                    Income_Type_Id = (income != null) ? income.Income_Type_Id : 0,
                    Frequency_Id = (income != null) ? income.Frequency_Id : 0,
                    Income = (income != null) ? income.Income : 0.0,

                    Status = status.Status_Type_Id,
                    DirectCert = status.Direct_Cert_Id,
                    Precertified = status.Precertified
                };

            // do not need to group data in db. It will generate a slower query.
            var groups = results.AsEnumerable().GroupBy(x => x.AppMemId);
            List<AppMember> members = new List<AppMember>();

            foreach (var group in groups)
            {
                AppMember mem = new AppMember();
                var member = group.FirstOrDefault();

                mem.Id = member.MemberId;
                mem.MemberName = member.FirstName + " " + member.LastName;
                mem.First_Name = member.FirstName;
                mem.Last_Name = member.LastName;
                mem.Middle = member.Middle;
                mem.DateOfBirth = member.DOB;
                mem.Email = member.Email;
                mem.SSN = member.SSN;
                mem.FosterChild = member.FosterChild;
                mem.HasIncome = member.NoIncome.HasValue ? !member.NoIncome.Value : false;

                mem.IsStudent = member.IsStudent;

                mem.Status = member.Status;
                mem.DirectCert = member.DirectCert;
                mem.Precertified = member.Precertified;

                if (mem.HasIncome)
                {
                    mem = this.LoadMemberIncome(group, mem) as AppMember;
                }

                members.Add(mem);
            }

            return members;
        }

        public ActionResult Review(long? appId)
        {
            if (!SecurityManager.ViewApplications) return RedirectToAction("NoAccess", "Security", new { id = "noapplications" });
            try
            {
                long clientId = ClientInfoData.GetClientID();

                var data =
                        from app in this.unitOfWork.ApplicationRepository.GetQuery()
                        join dist in this.unitOfWork.DistrictRepository.GetQuery() on app.District_Id equals dist.ID
                        into appData
                        from district in appData.DefaultIfEmpty()
                        join stat in this.unitOfWork.App_Statuses_Repository.GetQuery() on app.Id equals stat.Application_Id
                        into statusData
                        from status in statusData.DefaultIfEmpty()
                        join eth in this.unitOfWork.EtnicityRepository.GetQuery() on app.Id equals eth.Application_Id
                        into ethData
                        from ethnic in ethData.DefaultIfEmpty()
                        join note in this.unitOfWork.App_Notes_Repository.GetQuery() on app.Id equals note.Application_Id
                        into noteData
                        from notes in noteData.DefaultIfEmpty()
                        join sign in this.unitOfWork.App_Signer_Repository.GetQuery() on app.Id equals sign.Application_Id
                        into signData
                        from signer in signData.DefaultIfEmpty()
                        where app.Id == appId.Value && app.ClientID == clientId
                        select new
                        {
                            Application_Id = app.Id,
                            DistrictName = district.DistrictName,
                            Household_Size = (int?)app.Household_Size,
                            Precertified = status.Precertified,
                            Status = (int?)status.Status_Type_Id,
                            DirectCert = (int?)status.Direct_Cert_Id,
                            DateEntered = app.DateEntered,
                            BeneficiaryName = app.Beneficiary_Name,
                            CaseNumber = app.Case_Number,
                            Homeless = app.Homeless_On_App,
                            Migrant = app.Migrant_On_App,
                            Runaway = app.Runaway_On_App,
                            Entered = app.Entered,
                            Updated = app.Updated,
                            IsPrinted = app.Printed,
                            PrintedDate = app.PrintedDate,
                            Ethnic_Id = (int?)ethnic.Ethnic_Id,
                            SignerName = signer.Signature,
                            SignDate = signer.Signed_Date,
                            Address1 = signer.Address1,
                            Address2 = signer.Address2,
                            Email = signer.Email,
                            Zip = signer.Zip,
                            City = signer.City,
                            State = signer.State,
                            SSN = signer.SSN,
                            Phone = signer.Phone,
                            Cell_Phone = signer.Cell_Phone,
                            Comments = notes.Comment
                        };

                var result = data.ToList().FirstOrDefault();
                
                if (result != null)
                {
                    return View(new Application()
                    {
                        Application_Id = result.Application_Id,
                        District_Name = result.DistrictName,
                        Household_Size = result.Household_Size,
                        Precertified = result.Precertified,
                        DirectCert = this.GetAppDirectCert(result.DirectCert),
                        DateEntered = result.DateEntered,
                        Status = this.GetAppStatus(result.Status),
                        Beneficiary_Name = result.BeneficiaryName,
                        Case_Number = result.CaseNumber,
                        SpecialCircum = this.GetSpecialCircum(result.Homeless.HasValue ? result.Homeless.Value : false, result.Migrant.HasValue ? result.Migrant.Value : false, result.Runaway.HasValue ? result.Runaway.Value : false),
                        Entered = result.Entered,
                        Updated = result.Updated,
                        IsPrinted = result.IsPrinted,
                        PrintedDate = result.PrintedDate,
                        Ethnicity = this.GetEthnicity((long)(result.Ethnic_Id.HasValue?result.Ethnic_Id.Value:-1)),
                        Race = this.GetRace(appId.Value, clientId),
                        App_Signer_Name = result.SignerName,
                        SignedDate = result.SignDate,
                        Address1 = result.Address1,
                        Address2 = result.Address2,
                        Email = result.Email,
                        ZIP = result.Zip,
                        City = result.City,
                        State = result.State,
                        SSN = result.SSN,
                        Phone = result.Phone,
                        Cell_Phone = result.Cell_Phone,
                        Comments = result.Comments
                    });
                }

                return View(new Application());
            }
            catch(Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Review");
                return null;
            }
        }

        public string GetSpecialCircum(bool homeless, bool migrant, bool runaway)
        {
            if (!(homeless || migrant || runaway)) return "Not provided";

            var ret = "";
            if (homeless) ret += ", Homeless";
            if (migrant) ret += ", Migrant";
            if (runaway) ret += ", Runaway";
            if (ret.Length == 0) return "";
            return ret.Substring(2);
        }

        public string GetEthnicity(long ethnicId)
        {
            switch(ethnicId)
            {
                case 1:
                    return "Latino/ Hisponic";
                case 2:
                    return "Non Latino/ Hisponic";
                default:
                    return "Not provided";
            }
        }

        public string GetRace(long appId, long clientId)
        {
            var racesResults = this.unitOfWork.RacesRepository.Get(x => x.Application_Id == appId && x.ClientID == clientId);
            List<string> raceArray = new List<string>();
            foreach (var race in racesResults)
            {
                if (race.Race_Id == (int)FSS.FSS.Race.Asian)
                {
                    raceArray.Add("Asian");
                }
                if (race.Race_Id == (int)FSS.FSS.Race.White)
                {
                    raceArray.Add("White");
                }
                if (race.Race_Id == (int)FSS.FSS.Race.Black)
                {
                    raceArray.Add("Black");
                }
                if (race.Race_Id == (int)FSS.FSS.Race.AmericanIndian)
                {
                    raceArray.Add("American Indian");
                }
                if (race.Race_Id == (int)FSS.FSS.Race.NativeHawaiian)
                {
                    raceArray.Add("Native Hawaiian");
                }
            }

            return (raceArray.Count() > 0) ? string.Join(", ", raceArray) : "Not provided";
        }

        private string GetPosStatus(int statusId)
        {
            switch(statusId)
            {
                case 1:
                    return "Active";
                case 2:
                    return "Non Active";
                default:
                    return "No Status";
            }
        }

        private string GetAppStatus(int? statusId)
        {
            if (!statusId.HasValue) statusId = 0;
            switch (statusId)
            {
                case 1:
                    return "Accepted";
                case 2:
                    return "Rejected";
                default:
                    return "No Status";
            }
        }

        private string GetAppDirectCert(int? statusId)
        {
            if (!statusId.HasValue) statusId = 0;

            switch (statusId)
            {
                case 1:
                    return "Cert 1";
                case 2:
                    return "Cert 2";
                default:
                    return "No Cert";
            }
        }

        private Income LoadMemberIncome(IEnumerable<dynamic> group, Income income)
        {
            var incomefrequencies = unitOfWork.generalRepository.GetIncomeFrequencies().ToList();

            var job1 = group.Where(x => x.Income_Type_Id == (int)FSS.FSS.IncomeType.Job1).FirstOrDefault();
            income.Job1FrequencyId = job1 != null ? job1.Frequency_Id : null;
            income.Job1FrequencyName = job1 != null ? incomefrequencies.FirstOrDefault(x => x.Id == job1.Frequency_Id).Name : "Income Frequency";
            income.Job1Income = job1 != null ? job1.Income : null;

            var job2 = group.Where(x => x.Income_Type_Id == (int)FSS.FSS.IncomeType.Job2).FirstOrDefault();
            income.Job2FrequencyId = job2 != null ? job2.Frequency_Id : null;
            income.Job2FrequencyName = job2 != null ? incomefrequencies.FirstOrDefault(x => x.Id == job2.Frequency_Id).Name : "Income Frequency";
            income.Job2Income = job2 != null ? job2.Income : null;

            var job3 = group.Where(x => x.Income_Type_Id == (int)FSS.FSS.IncomeType.Job3).FirstOrDefault();
            income.Job3FrequencyId = job3 != null ? job3.Frequency_Id : null;
            income.Job3FrequencyName = job3 != null ? incomefrequencies.FirstOrDefault(x => x.Id == job3.Frequency_Id).Name : "Income Frequency";
            income.Job3Income = job3 != null ? job3.Income : null;

            var welfareIncome = group.Where(x => x.Income_Type_Id == (int)FSS.FSS.IncomeType.Welfare).FirstOrDefault();
            income.WelfareFrequencyId = welfareIncome != null ? welfareIncome.Frequency_Id : null;
            income.WelfareFrequencyName = welfareIncome != null ? incomefrequencies.FirstOrDefault(x => x.Id == welfareIncome.Frequency_Id).Name : "Income Frequency";
            income.WelfareIncome = welfareIncome != null ? welfareIncome.Income : null;

            var pensionIncome = group.Where(x => x.Income_Type_Id == (int)FSS.FSS.IncomeType.Pension).FirstOrDefault();
            income.PensionFrequencyId = pensionIncome != null ? pensionIncome.Frequency_Id : null;
            income.PensionFrequencyName = pensionIncome != null ? incomefrequencies.FirstOrDefault(x => x.Id == pensionIncome.Frequency_Id).Name : "Income Frequency";
            income.PensionIncome = pensionIncome != null ? pensionIncome.Income : null;

            var otherIncome = group.Where(x => x.Income_Type_Id == (int)FSS.FSS.IncomeType.Other).FirstOrDefault();
            income.OtherFrequencyId = otherIncome != null ? otherIncome.Frequency_Id : null;
            income.OtherFrequencyName = otherIncome != null ? incomefrequencies.FirstOrDefault(x => x.Id == otherIncome.Frequency_Id).Name : "Income Frequency";
            income.OtherIncome = otherIncome != null ? otherIncome.Income : null;

            return income;
        }

        private Dictionary<int, string> GetSearchByOptions()
        {
            Dictionary<int, string> options = new Dictionary<int, string>();
            options.Add(0, "Application ID");
            options.Add(1, "Parent Name or Signer");
            options.Add(2, "Student Name (First, Last, User ID)");

            return options;
        }
        private Dictionary<int, string> GetApprovalStatusOptions()
        {
            Dictionary<int, string> options = new Dictionary<int, string>();
            options.Add(2, "Rejected");
            options.Add(1, "Accepted");

            return options;
        }
        private Dictionary<bool, string> GetEnteredOptions()
        {
            Dictionary<bool, string> options = new Dictionary<bool, string>();
            options.Add(false, "No");
            options.Add(true, "Yes");
            return options;
        }
        private Dictionary<bool, string> GetUpdatedOptions()
        {
            Dictionary<bool, string> options = new Dictionary<bool, string>();
            options.Add(false, "No");
            options.Add(true, "Yes");
            return options;
        }

        public ActionResult Print(int? appId)
        {
            if (!SecurityManager.ViewApplications) return RedirectToAction("NoAccess", "Security", new { id = "noapplications" });
            long clientId = ClientInfoData.GetClientID();

            var data =
                    from app in this.unitOfWork.ApplicationRepository.GetQuery()
                    join dist in this.unitOfWork.DistrictRepository.GetQuery() on app.District_Id equals dist.ID
                    into appData
                    from district in appData.DefaultIfEmpty()
                    join stat in this.unitOfWork.App_Statuses_Repository.GetQuery() on app.Id equals stat.Application_Id
                    into statusData
                    from status in statusData.DefaultIfEmpty()
                    join eth in this.unitOfWork.EtnicityRepository.GetQuery() on app.Id equals eth.Application_Id
                    into ethData
                    from ethnic in ethData.DefaultIfEmpty()
                    join sign in this.unitOfWork.App_Signer_Repository.GetQuery() on app.Id equals sign.Application_Id
                    into signData
                    from signer in signData.DefaultIfEmpty()
                    where app.Id == appId.Value && app.ClientID == clientId
                    select new
                    {
                        Application_Id = app.Id,
                        DistrictName = district.DistrictName,
                        Household_Size = (int?)app.Household_Size,
                        Precertified = status.Precertified,
                        Status = (int?)status.Status_Type_Id,
                        DirectCert = (int?)status.Direct_Cert_Id,
                        DateEntered = app.DateEntered,
                        BeneficiaryName = app.Beneficiary_Name,
                        CaseNumber = app.Case_Number,
                        //CaseNumber = string.Empty,
                        Homeless = app.Homeless_On_App,
                        Migrant = app.Migrant_On_App,
                        Runaway = app.Runaway_On_App,
                        Entered = app.Entered,
                        Updated = app.Updated,
                        IsPrinted = app.Printed,
                        PrintedDate = app.PrintedDate,
                        Ethnic_Id = (int?)ethnic.Ethnic_Id,
                        SignerName = signer.Signature,
                        SignDate = signer.Signed_Date,
                        Address1 = signer.Address1,
                        Address2 = signer.Address2,
                        Email = signer.Email,
                        Zip = signer.Zip,
                        City = signer.City,
                        State = signer.State,
                        SSN = signer.SSN,
                        Phone = signer.Phone,
                        Cell_Phone = signer.Cell_Phone
                    };

                var result = data.ToList().FirstOrDefault();

                var model = new Application()
                     {
                         Application_Id = result.Application_Id,
                         District_Name = result.DistrictName,
                         Household_Size = result.Household_Size,
                         Precertified = result.Precertified,
                         DirectCert = this.GetAppDirectCert(result.DirectCert),
                         DateEntered = result.DateEntered,
                         Status = this.GetAppStatus(result.Status),
                         Beneficiary_Name = result.BeneficiaryName,
                         Case_Number = result.CaseNumber,
                         SpecialCircum = this.GetSpecialCircum(result.Homeless.HasValue ? result.Homeless.Value : false, result.Migrant.HasValue ? result.Migrant.Value : false, result.Runaway.HasValue ? result.Runaway.Value : false),
                         Entered = result.Entered,
                         Updated = result.Updated,
                         IsPrinted = result.IsPrinted,
                         PrintedDate = result.PrintedDate,
                         Ethnicity = result.Ethnic_Id.ToString(),
                         Race = this.GetRace(appId.Value, clientId),
                         App_Signer_Name = result.SignerName,
                         SignedDate = result.SignDate,
                         Address1 = result.Address1,
                         Address2 = result.Address2,
                         Email = result.Email,
                         ZIP = result.Zip,
                         City = result.City,
                         State = result.State,
                         SSN = result.SSN,
                         Phone = result.Phone,
                         Cell_Phone = result.Cell_Phone
                     };

                model.Students = this.LoadStudents(appId);
                model.StudentsNotEnrolled = this.LoadMembers(appId, x => x.isStudent == true);
                model.Members = this.LoadMembers(appId, x => x.isStudent == false);

                var lst1 = model.Students as IEnumerable<Income>;
                var lst2 = model.StudentsNotEnrolled as IEnumerable<Income>;
                var children = lst1.Concat(lst2);

                var isFreqSame = this.IsSameFrequency(children);
                model.StudentsTotalIncome = this.SumIncome(children, isFreqSame);
                model.StudentsTotalIncomeFreq = isFreqSame ? this.GetFirstFrequencyId(children) : 5;

                var allMembers = children.Concat(model.Members);
                isFreqSame = this.IsSameFrequency(allMembers);
                model.TotalIncome = this.SumIncome(allMembers, isFreqSame);
                model.TotalIncomeFreq = isFreqSame ? this.GetFirstFrequencyId(allMembers) : 5;

                var frequencyList = unitOfWork.generalRepository.GetIncomeFrequencies();
                foreach (var mem in model.Members)
                {
                    if (mem.HasIncome)
                    {
                        int? lastFrequency = null;
                        isFreqSame = this.IsSameFrequency(mem, lastFrequency, FSS.FSS.TargetLevel.WorkOnly);
                        mem.WorkEarningsTotal = Math.Round(this.SumIncome(mem, frequencyList, isFreqSame, FSS.FSS.TargetLevel.WorkOnly), 2);
                        mem.WorkingEarningTotalFreq = isFreqSame ? this.GetFirstFrequencyId(mem, FSS.FSS.TargetLevel.WorkOnly) : 5;

                        lastFrequency = null;
                        isFreqSame = this.IsSameFrequency(mem, lastFrequency, FSS.FSS.TargetLevel.WelfareOnly);
                        mem.WelfareTotalIncome = Math.Round(this.SumIncome(mem, frequencyList, isFreqSame, FSS.FSS.TargetLevel.WelfareOnly), 2);
                        mem.WelfareTotalIncomeFreq = isFreqSame ? this.GetFirstFrequencyId(mem, FSS.FSS.TargetLevel.WelfareOnly) : 5;

                        lastFrequency = null;
                        isFreqSame = this.IsSameFrequency(mem, lastFrequency, FSS.FSS.TargetLevel.OthersOnly);
                        mem.OtherTotalIncome = Math.Round(this.SumIncome(mem, frequencyList, isFreqSame, FSS.FSS.TargetLevel.OthersOnly), 2);
                        mem.OtherTotalIncomeFreq = isFreqSame ? this.GetFirstFrequencyId(mem, FSS.FSS.TargetLevel.OthersOnly) : 5;
                    }
                }


                var htmlString = this.RenderPartialToString("Print", model);

                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();
                //converter.Options.EmbedFonts = true;
                converter.Options.MarginTop = 10;
                converter.Options.MarginLeft = 10;
                converter.Options.MarginRight = 10;
                converter.Options.MarginBottom = 10;
                converter.Options.PdfPageSize = PdfPageSize.Letter;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Landscape;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;
                converter.Options.KeepTextsTogether = true;

                // create a new pdf document converting an url
                PdfDocument doc = converter.ConvertHtmlString(htmlString);

                // save pdf document
                byte[] pdf = doc.Save();

                // close pdf document
                doc.Close();

                // To display inside browser.
                return File(pdf, "application/pdf");

                // To download as a file.
                //// return resulted pdf document
                //   FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                //   fileResult.FileDownloadName = "Document.pdf";
                //   return fileResult;
        }

        private bool IsSameFrequency(IEnumerable<Income> incomes, FSS.FSS.TargetLevel level = FSS.FSS.TargetLevel.All)
        {
            int? lastFrequency = null;
            foreach (var income in incomes)
            {
                if (!lastFrequency.HasValue)
                {
                    lastFrequency = this.GetFirstFrequencyId(income, level);
                }
                if (income.HasIncome)
                {
                    if (!IsSameFrequency(income, lastFrequency, level))
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }

        public double SumIncome(IEnumerable<Income> incomes, bool isSameFrequency, FSS.FSS.TargetLevel level = FSS.FSS.TargetLevel.All)
        {
            double totalIncome = 0;
            var frequencyList = unitOfWork.generalRepository.GetIncomeFrequencies();
            foreach(var income in incomes)
            {
                if(income.HasIncome)
                {
                    totalIncome += SumIncome(income, frequencyList, isSameFrequency, level);
                }
            }
            return totalIncome;
        }

        public double SumIncome(Income income, IEnumerable<IncomeFrequency> frequencyList, bool isSameFrequency, FSS.FSS.TargetLevel level = FSS.FSS.TargetLevel.All)
        {
            double totalIncome = 0;
            switch(level)
            {
                case FSS.FSS.TargetLevel.All:
                default:
                    {
                        if (income.Job1Income.HasValue)
                        {
                            totalIncome += income.Job1Income.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.Job1FrequencyId.Value).Multiplier);
                        }
                        if (income.Job2Income.HasValue)
                        {
                            totalIncome += income.Job2Income.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.Job2FrequencyId.Value).Multiplier);
                        }
                        if (income.Job3Income.HasValue)
                        {
                            totalIncome += income.Job3Income.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.Job3FrequencyId.Value).Multiplier);
                        }
                        if (income.WelfareIncome.HasValue)
                        {
                            totalIncome += income.WelfareIncome.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.WelfareFrequencyId.Value).Multiplier);
                        }
                        if (income.PensionIncome.HasValue)
                        {
                            totalIncome += income.PensionIncome.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.PensionFrequencyId.Value).Multiplier);
                        }
                        if (income.OtherIncome.HasValue)
                        {
                            totalIncome += income.OtherIncome.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.OtherFrequencyId.Value).Multiplier);
                        }
                    }
                    break;
                case FSS.FSS.TargetLevel.WorkOnly:
                    {
                        if (income.Job1Income.HasValue)
                        {
                            totalIncome += income.Job1Income.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.Job1FrequencyId.Value).Multiplier);
                        }
                        if (income.Job2Income.HasValue)
                        {
                            totalIncome += income.Job2Income.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.Job2FrequencyId.Value).Multiplier);
                        }
                        if (income.Job3Income.HasValue)
                        {
                            totalIncome += income.Job3Income.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.Job3FrequencyId.Value).Multiplier);
                        }
                    }
                    break;
                case FSS.FSS.TargetLevel.WelfareOnly:
                    {
                        if (income.WelfareIncome.HasValue)
                        {
                            totalIncome += income.WelfareIncome.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.WelfareFrequencyId.Value).Multiplier);
                        }
                    }
                    break;
                case FSS.FSS.TargetLevel.OthersOnly:
                    {
                        if (income.PensionIncome.HasValue)
                        {
                            totalIncome += income.PensionIncome.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.PensionFrequencyId.Value).Multiplier);
                        }
                        if (income.OtherIncome.HasValue)
                        {
                            totalIncome += income.OtherIncome.Value * (isSameFrequency ? 1 : frequencyList.FirstOrDefault(x => x.Id == income.OtherFrequencyId.Value).Multiplier);
                        }
                    }
                    break;
            }

            return totalIncome;
        }

        public bool IsSameFrequency(Income income, int? lastFrequency, FSS.FSS.TargetLevel level = FSS.FSS.TargetLevel.All)
        {
            if (!lastFrequency.HasValue)
            {
                lastFrequency = this.GetFirstFrequencyId(income, level);
            }

            if (lastFrequency.HasValue)
            {
                switch (level)
                {
                    case FSS.FSS.TargetLevel.All:
                    default:
                        {
                            return (
                                (!income.Job1FrequencyId.HasValue || income.Job1FrequencyId == lastFrequency.Value)
                            && (!income.Job2FrequencyId.HasValue || income.Job2FrequencyId.Value == lastFrequency.Value)
                            && (!income.Job3FrequencyId.HasValue || income.Job3FrequencyId.Value == lastFrequency.Value)
                            && (!income.WelfareFrequencyId.HasValue || income.WelfareFrequencyId.Value == lastFrequency.Value)
                            && (!income.PensionFrequencyId.HasValue || income.PensionFrequencyId.Value == lastFrequency.Value)
                            && (!income.OtherFrequencyId.HasValue || income.OtherFrequencyId.Value == lastFrequency.Value)
                            );
                        }
                    case FSS.FSS.TargetLevel.WorkOnly:
                        {
                            return (
                                (!income.Job1FrequencyId.HasValue || income.Job1FrequencyId == lastFrequency.Value)
                            && (!income.Job2FrequencyId.HasValue || income.Job2FrequencyId.Value == lastFrequency.Value)
                            && (!income.Job3FrequencyId.HasValue || income.Job3FrequencyId.Value == lastFrequency.Value));
                        }
                    case FSS.FSS.TargetLevel.WelfareOnly:
                        {
                            return (!income.WelfareFrequencyId.HasValue || income.WelfareFrequencyId.Value == lastFrequency.Value);
                        }
                    case FSS.FSS.TargetLevel.OthersOnly:
                        {
                            return (!income.PensionFrequencyId.HasValue || income.PensionFrequencyId.Value == lastFrequency.Value)
                            && (!income.OtherFrequencyId.HasValue || income.OtherFrequencyId.Value == lastFrequency.Value);
                        }
                }
            }

            return true;
        }

        private int? GetFirstFrequencyId(IEnumerable<Income> incomes, FSS.FSS.TargetLevel level = FSS.FSS.TargetLevel.All)
        {
            foreach(var income in incomes)
            {
                if(income.HasIncome)
                {
                    int? freq = this.GetFirstFrequencyId(income, level);
                    if(freq != null)
                    {
                        return freq;
                    }
                }
            }

            return null;
        }

        private int? GetFirstFrequencyId(Income income, FSS.FSS.TargetLevel level = FSS.FSS.TargetLevel.All)
        {
            switch(level)
            {
                case FSS.FSS.TargetLevel.All:
                default:
                    {
                        if (income.Job1FrequencyId.HasValue) return income.Job1FrequencyId.Value;
                        if (income.Job2FrequencyId.HasValue) return income.Job2FrequencyId.Value;
                        if (income.Job3FrequencyId.HasValue) return income.Job3FrequencyId.Value;
                        if (income.WelfareFrequencyId.HasValue) return income.WelfareFrequencyId.Value;
                        if (income.PensionFrequencyId.HasValue) return income.PensionFrequencyId.Value;
                        if (income.OtherFrequencyId.HasValue) return income.OtherFrequencyId.Value;
                    }
                    break;
                case FSS.FSS.TargetLevel.WorkOnly:
                    {
                        if (income.Job1FrequencyId.HasValue) return income.Job1FrequencyId.Value;
                        if (income.Job2FrequencyId.HasValue) return income.Job2FrequencyId.Value;
                        if (income.Job3FrequencyId.HasValue) return income.Job3FrequencyId.Value;
                    }
                    break;
                case FSS.FSS.TargetLevel.WelfareOnly:
                    {
                        if (income.WelfareFrequencyId.HasValue) return income.WelfareFrequencyId.Value;
                    }
                    break;
                case FSS.FSS.TargetLevel.OthersOnly:
                    {
                        if (income.PensionFrequencyId.HasValue) return income.PensionFrequencyId.Value;
                        if (income.OtherFrequencyId.HasValue) return income.OtherFrequencyId.Value;
                    }
                    break;
            }

            return null;
        }

        [HttpPost]
        public ActionResult ToggleAppStatus(int appId, int status, string comment)
        {
            try
            {
                long ClientId = ClientInfoData.GetClientID();
                var app = this.unitOfWork.App_Statuses_Repository.Get(x => x.Application_Id == appId).FirstOrDefault();
                if (app == null)
                {
                    this.unitOfWork.App_Statuses_Repository.Insert(new App_Statuses { ClientID = ClientId, Application_Id = appId, Status_Type_Id = status });
                }
                else
                {
                    app.Status_Type_Id = status;
                    this.unitOfWork.App_Statuses_Repository.Update(app);
                }

                var com = this.unitOfWork.App_Notes_Repository.Get(x => x.Application_Id == appId).FirstOrDefault();
                if(com == null)
                {
                    this.unitOfWork.App_Notes_Repository.Insert(new App_Notes() { ClientID = ClientId, Application_Id = appId, Comment = comment });
                }
                else
                {
                    com.Comment = comment;
                    this.unitOfWork.App_Notes_Repository.Update(com);
                }
                
                this.unitOfWork.Save();
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables 
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ApplicationController", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Approve");
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

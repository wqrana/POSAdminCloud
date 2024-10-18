using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.Models;

namespace AdminPortalModels.ViewModels
{
    class SecurityModels
    {
    }

    public class UsersVM
    {
        public int AllRecordsCount { get; set; }
        public bool IsDeleted { get; set; }
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public string LoginName { get; set; }
        public string UserName { get; set; }
        //public string SecurityGroup { get; set; }
        //public string CashedInStatus { get; set; }
        //public string POSName { get; set; }
        public string UserRoleName { get; set; }
        public int? UserRole_Id { get; set; }
        public bool isActive { get; set; }
        public bool isPrimary { get; set; }
        public string isActiveClass {
            get {
                if (isActive) 
                    return "fa-check";
                else
                    return "fa-times";
            }
        }
        public string isPrimaryClass
        {
            get
            {
                if (isPrimary)
                    return "fa-check";
                else
                    return "fa-times";
            }
        }


    }

    public class UsersDetailsVM
    {
        public int EmployeeId { get; set; }
        public string UserName { get; set; }
        public int SecurityGroupId { get; set; }
        public string SecurityGroup { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string savebtnCaption { get; set; }
        public int? UserRolesID { get; set; }
        public int? returnCode { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class SecurityList
    {
        public long Id { get; set; }
        public string groupName { get; set; }
    }

    public class userRoleItem
    {
        public long ClientId { get; set; }
        public int Id { get; set; }
        public string RoleName { get; set; }
        public int AdminHQSystem { get; set; }
    }

    public class UserRoleModel
    {
        public long ClientID { get; set; }
        public int ID { get; set; }
        public string SelectedHQSystem { get; set; }
        public string RoleName { get; set; }
        public string ModulesWithPermissions { get; set; }
        public IList<FSSSecurity.SecurityLists.SystemDetail> SystemsList { get; set; }
        public IList<FSSSecurity.SecurityLists.ModuleDetail> ModulesList { get; set; }
    }


    //public class AdminHQSystem
    //{
    //    public AdminHQSystem(int id, string sysName)
    //    {
    //        ID = id;
    //        SystemName = sysName;
    //    }
    //    public int ID { get; set; }
    //    public string SystemName { get; set; }
    //}

    public class UserRoleDeleteModel : DeleteModel
    {
        public override string Title { get { return "User Role"; } }
        public override string DeleteUrl { get { return "/security/Delete"; } }
        public bool userExists { get; set; }
    }

    public class UserDeleteModel : DeleteModel
    {
        public override string Title { get { return "User"; } }
        public override string DeleteUrl { get { return "/security/UserDelete"; } }
    }

    [Serializable]
    public class UserFilters
    {
        public string SearchBy { get; set; }
        public int UserRoleId { get; set; }
        public string ActiveStr { get; set; }
        public string PrimaryStr { get; set; }
    }
}

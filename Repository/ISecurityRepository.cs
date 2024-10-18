using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.edmx;
using AdminPortalModels.ViewModels;
using AdminPortalModels.Models;
using System.Data;

namespace Repository
{
    public interface ISecurityRepository :IDisposable
    {
        IEnumerable<UsersVM> getUsers(long clientId, int? roleID);
        //UsersDetailsVM getUserDetail(long clientId, int employeeID);
        //IList<SecurityList> getSecurityGroup(long ClientId);
        string saveUser(UsersDetailsVM userDetails, long ClientID);
        string DeleteUser(long clientID, int userId);
        string DeleteUserRole(long clientId, string securityGroupName);
        bool AssociatedOrdersExists(long ClientId, int customerID);
        IList<FSSSecurity.SecurityLists.SystemDetail> getSystemList();
        IList<FSSSecurity.SecurityLists.ModuleDetail> getModulesList(FSSSecurity.SecurityLists.FSS_Systems system);
        //ICollection<AdminHQSystem> getSystemNames();
        IList<FSSSecurity.SecurityLists.PermissionDetail> getPermissionsList(FSSSecurity.SecurityLists.FSS_Modules module);
        //string SaveUserRole(long ClientID, int ID, string UserRoleName, int AdminHQSystemSelected, string UserRolePermissions_IDS);
        //UserRole getUserRol(int id, long clientID);
        //IEnumerable<Admin_UserRole_List_Result> GetUserRolesList(Nullable<long> clientID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, out int totalrecords);
        //void DeleteUserRole(int id, long clientID);

        //IList<userRoleItem> getUsersRolesNames(long ClientId);
        //string getPermissionString(long clientID, int customerID);
        UsersVM GetUserByCustomerId(long clientId, int customerId);
        UsersVM GetUserByLoginName(long clientId, string loginName);
        string SaveRolePermissions(long clientId, int securityGroupId, string securityGroupName, DataTable accessrights);
        List<Admin_AccessRights_List_Result> GetAccessRightsList(long clientId, string securityGroup);

    }
}

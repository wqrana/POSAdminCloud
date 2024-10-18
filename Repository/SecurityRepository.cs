using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.edmx;
using AdminPortalModels.ViewModels;
using AdminPortalModels.Models;
using Repository.Helpers;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;

namespace Repository
{
    public class SecurityRepository : ISecurityRepository, IDisposable
    {
        private PortalContext context;

        public SecurityRepository(PortalContext context)
        {
            this.context = context;
        }

        public IEnumerable<UsersVM> getUsers(long clientId, int? roleID)
        {
            try
            {
                if (roleID.HasValue)
                {
                    return context.Admin_Users_List(clientId, "", 0, "", 0).Where(ur => ur.UserRole_Id == roleID).Select(u => new UsersVM
                    {
                        UserName = u.UserName,
                        LoginName = u.LoginName,
                        CustomerId = u.Customer_Id,
                        UserRoleName = u.UserRoleName,
                        UserRole_Id = u.UserRole_Id
                    });
                }
                else
                {
                    return context.Admin_Users_List(clientId, "", 0, "", 0).Select(u => new UsersVM
                    {
                        UserName = u.UserName,
                        LoginName = u.LoginName,
                        CustomerId = u.Customer_Id,
                        UserRoleName = u.UserRoleName,
                        UserRole_Id = u.UserRole_Id
                    });
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getUsers");
                return null;
            }
        }

        //public UsersDetailsVM getUserDetail(long clientId, int employeeID)
        //{
        //    try
        //    {
        //        return context.Admin_Users_Detail(clientId, employeeID).Select(u => new UsersDetailsVM
        //        {
        //            UserName = u.UserName,
        //            SecurityGroupId = u.SecurityGroup_Id,
        //            SecurityGroup = u.SecurityGroup,
        //            Password = u.Password,
        //            LoginName = u.LoginName,
        //            EmployeeId = u.EmployeeID,
        //            UserRolesID = u.UserRoles_ID
        //        }).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error logging in cloud tables
        //        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getUserDetail");
        //        return null;
        //    }
        //}

        public string saveUser(UsersDetailsVM userDetails, long ClientID)
        {
            try
            {
                ObjectParameter errMsg = new ObjectParameter("ErrorMessage", string.Empty);
                ObjectParameter res = new ObjectParameter("ResultCode", 0);
                ObjectParameter empId = new ObjectParameter("EmployeeID", userDetails.EmployeeId);


                context.Admin_Users_Save(ClientID, empId, userDetails.LoginName, userDetails.Password, userDetails.SecurityGroupId, userDetails.UserRolesID, res, errMsg);

                return res.Value.ToString();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "saveUser");
                return null;
            }
        }

        public string DeleteUser(long clientID, int userId)
        {
            string retValue = "1";

            ObjectParameter user = new ObjectParameter("employeeID", userId);
            ObjectParameter result = new ObjectParameter("resultCode", 0);
            ObjectParameter errorMsg = new ObjectParameter("ErrorMessage", string.Empty);

            //call db function to deleet
            try
            {
                this.context.Admin_Users_Delete(clientID, user, result, errorMsg);

                if (Convert.ToInt16(result.Value) != 0)
                {
                    retValue = errorMsg.Value.ToString();
                }
                else
                {
                    retValue = "-1";
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteUser");
            }
            return retValue;
        }

        public string DeleteUserRole(long clientId, string securityGroupName)
        {
            string retValue = "-1";
            ObjectParameter result = new ObjectParameter("resultCode", -1);
            ObjectParameter errorMsg = new ObjectParameter("ErrorMessage", string.Empty);

            //call db function to deleet
            try
            {
                this.context.Admin_UserRole_Delete(clientId, securityGroupName, result, errorMsg);
                retValue = result.Value.ToString();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteUserRole");
            }
            return retValue;
        }

        public bool AssociatedOrdersExists(long ClientId, int customerID)
        {
            try
            {
                return context.Orders.Where(o => o.ClientID == ClientId && o.Emp_Cashier_Id == customerID).Any();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AssociatedOrdersExists");
                return false;
            }
        }

        //public IList<SecurityList> getSecurityGroup(long ClientId)
        //{
        //    try
        //    {
        //        return context.SecurityGroups.Where(sg => sg.ClientID == ClientId).Select(s => new SecurityList
        //        {
        //            Id = s.ID,
        //            groupName = s.GroupName
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        //Error logging in cloud tables
        //        ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSecurityGroup");
        //        return null;
        //    }

        //}

        //public IList<userRoleItem> getUsersRolesNames(long ClientId)
        //{
        //    return context.UserRoles.Where(ur => ur.ClientID == ClientId).Select(s => new userRoleItem
        //    {
        //        Id = s.Id,
        //        RoleName = s.UserRoleName
        //    }).ToList();
        //}


        //public IList<AdminHQSystem> getSystemsList(long ClientId)
        //{
        //    IList<AdminHQSystem> tempList = new List<AdminHQSystem>();
        //    tempList.Add(new AdminHQSystem { ID = 1, SystemName = "POS ADMIN" });
        //    return tempList;
        //}

        //public FSSSecurity.SecurityLists.UserRoleSystem getUserRoleSystem()
        //{
        //    return FSSSecurity.SecurityLists.getUserRoleSystem().UserRoleSystemsCollection.Where(o => o.ID == 1).FirstOrDefault();
        //}

        public IList<FSSSecurity.SecurityLists.SystemDetail> getSystemList()
        {
            return FSSSecurity.SecurityLists.getSystemsList();
        }

        public IList<FSSSecurity.SecurityLists.ModuleDetail> getModulesList(FSSSecurity.SecurityLists.FSS_Systems system)
        {
            return FSSSecurity.SecurityLists.getModulesList(system);
        }

        public IList<FSSSecurity.SecurityLists.PermissionDetail> getPermissionsList(FSSSecurity.SecurityLists.FSS_Modules module)
        {
            try
            {
                return FSSSecurity.SecurityLists.getPermissionsList(module);
            }
            catch (Exception)
            {

                return null;
            }
        }
        public string SaveUserRole(long ClientID, int ID, string UserRoleName, int AdminHQSystemSelected, string UserRolePermissions_IDS)
        {

            var obj = context.Admin_UserRole_Save(ClientID, ID, UserRoleName, AdminHQSystemSelected, UserRolePermissions_IDS);
            string tempstr = obj.FirstOrDefault().ErrorMessage;

            if (tempstr == "")
            {
                return "0";
            }
            else
            {
                return "-1";
            }

        }

        public UsersVM GetUserByCustomerId(long clientId, int customerId)
        {
            try
            {
                return context.Admin_Users_List(clientId, "", 0, "", 0).Where(ur => ur.Customer_Id == customerId).Select(u => new UsersVM
                {
                    UserName = u.UserName,
                    LoginName = u.LoginName,
                    CustomerId = u.Customer_Id,
                    UserRoleName = u.UserRoleName,
                    UserRole_Id = u.UserRole_Id
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetUserByCustomerId");
                throw;
            }
        }

        public UsersVM GetUserByLoginName(long clientId, string loginName)
        {
            try
            {
                return context.Admin_Users_List(clientId, "", 0, "", 0).Where(ur => ur.LoginName.ToLower().Trim() == loginName.ToLower().Trim()).Select(u => new UsersVM
                {
                    UserName = u.UserName,
                    LoginName = u.LoginName,
                    CustomerId = u.Customer_Id,
                    UserRoleName = u.UserRoleName,
                    UserRole_Id = u.UserRole_Id
                }).SingleOrDefault();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetUserByLoginName");
                throw;
            }
        }

        public List<Admin_AccessRights_List_Result> GetAccessRightsList(long clientId, string securityGroup)
        {
            try
            {
                return context.Admin_AccessRights_List(clientId, securityGroup).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SecurityRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetAccessRightsList");
                throw;
            }
        }

        public string SaveRolePermissions(long clientId, int securityGroupId, string securityGroupName, DataTable accessrights)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(context.Database.Connection.ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "Admin_RolePermission_Save";
                        cmd.Parameters.Add(new SqlParameter() { ParameterName = "@ClientID", SqlDbType = SqlDbType.BigInt, Value = clientId });
                        cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SecurityGroupID", SqlDbType = SqlDbType.Int, Value = securityGroupId });
                        cmd.Parameters.Add(new SqlParameter() { ParameterName = "@SecurityGroupName", SqlDbType = SqlDbType.VarChar, Value = securityGroupName });

                        SqlParameter tableParam = new SqlParameter();
                        tableParam.ParameterName = "@AccessRights";
                        tableParam.SqlDbType = SqlDbType.Structured;
                        tableParam.Value = accessrights;
                        cmd.Parameters.Add(tableParam);

                        SqlParameter errorCode = new SqlParameter("@ResultCode", -1);
                        errorCode.DbType = DbType.Int32;
                        errorCode.Direction = ParameterDirection.Output;

                        SqlParameter errorMsg = new SqlParameter("@ErrorMsg", "");
                        errorMsg.SqlDbType = SqlDbType.VarChar;
                        errorMsg.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(errorCode);
                        cmd.Parameters.Add(errorMsg);

                        cmd.ExecuteNonQuery();

                        return Convert.ToString(errorCode.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "SettingsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "SaveRolePermissions");
                return "-1";
            }
        }


        //public UserRole getUserRol(int id, long clientID)
        //{
        //    return context.UserRoles.Where(u => u.Id == id && u.ClientID == clientID).FirstOrDefault();
        //}

        //public void DeleteUserRole(int id, long clientID)
        //{
        //    context.UserRoles.Remove(getUserRol(id, clientID));
        //    context.SaveChanges();
        //}

        //public IEnumerable<Admin_UserRole_List_Result> GetUserRolesList(Nullable<long> clientID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection, out int totalrecords)
        //{
        //    string columnName = getColmnName(sortColumnIndex);

        //    IQueryable<Admin_UserRole_List_Result> query = this.context.Admin_UserRole_List(clientID);
        //    IEnumerable<Admin_UserRole_List_Result> rolesResult = null;

        //    totalrecords = query.Count();
        //    if (sortDirection == "asc")
        //    {
        //        rolesResult = query.OrderBy(columnName).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_UserRole_List_Result>;
        //    }
        //    else
        //    {
        //        rolesResult = query.OrderByDescending(columnName).Skip(iDisplayStart).Take(iDisplayLength) as IEnumerable<Admin_UserRole_List_Result>;
        //    }


        //    return rolesResult;
        //}

        //public string getPermissionString(long clientID, int customerID)
        //{
        //    int RoleID = context.Employees.Where(e => e.ClientID == clientID && e.Customer_Id == customerID).FirstOrDefault().UserRoles_ID ?? 0;
        //    bool exists = context.UserRoles.Where(ur => ur.ClientID == clientID && ur.Id == RoleID).Any();
        //    if (exists)
        //    {
        //        return context.UserRoles.Where(ur => ur.ClientID == clientID && ur.Id == RoleID).FirstOrDefault().UserRolePermissions_IDS ?? "";
        //    }
        //    else
        //    {
        //        return "";
        //    }
        //}

        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 1:
                    retVal = "UserRoleName";
                    break;
                case 2:
                    retVal = "AdminHQSystem";
                    break;
                case 3:
                    retVal = "usersCount";
                    break;
                default:
                    retVal = "UserRoleName";
                    break;

                    
            }

            return retVal;
        }
        /// <summary>
        /// Disposal
        /// </summary>
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}



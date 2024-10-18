using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MSA_AdminPortal.Helpers
{
    public class SecurityManager
    {
        public static bool CheckPermission(FSSSecurity.SecurityLists.FSS_Modules module, FSSSecurity.SecurityLists.FSS_Actions action)
        {
            int moduleID = (int)module;
            string actionID = Convert.ToString((int)action);

            // Bypass security if it is the primary account (Always has Admin access)
            if (ClientInfoData.GetIsPrimary()) return true;

            Dictionary<int, string> tempDict = ClientInfoData.getModulePermissionsList();
            bool found = false;
            if (tempDict.ContainsKey(moduleID))
            {
                string actionsString = tempDict[moduleID];
                string[] tempStr = actionsString.Replace(" ", "").Trim().Split(',');
                if (tempStr.Contains(actionID))
                {
                    found = true;
                }
                else
                {
                    found = false;
                }
            }
            else
            {
                found = false;
            }

            return found;
        }

        // Dashboard
        public static bool viewDashboard
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Dashboard, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        // District
        public static bool viewDistricts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Districts, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateDistricts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Districts, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateDistricts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Districts, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteDistricts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Districts, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        //Schools
        public static bool viewSchools
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Schools, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateSchools
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Schools, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateSchools
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Schools, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteSchools
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Schools, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //POS
        public static bool viewPOS
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POS, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool viewSettingsPreorder
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.SettingsPreorder, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool updateSettingsPreorder
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.SettingsPreorder, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }
        public static bool viewPreorderDashboard
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.PreorderDashboard, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreatePOS
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POS, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdatePOS
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POS, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeletePOS
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POS, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //USER
        public static bool viewUsers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Users, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateUsers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Users, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateUsers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Users, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteUsers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Users, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //User Roles
        public static bool viewUserRoles
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.UserRoles, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateUserRoles
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.UserRoles, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateUserRoles
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.UserRoles, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteUserRoles
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.UserRoles, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Customers
        public static bool viewCustomers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Customers, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateCustomers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Customers, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateCustomers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Customers, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteCustomers
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Customers, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Home Rooms
        public static bool viewHomerooms
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Homerooms, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateHomerooms
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Homerooms, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateHomerooms
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Homerooms, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteHomerooms
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Homerooms, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Grades
        public static bool viewGrades
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Grades, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateGrades
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Grades, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateGrades
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Grades, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteGrades
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Grades, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Order Managemnet
        public static bool viewActivity
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Activity, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool viewPayments
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Payments, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool viewRefunds
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Refunds, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool viewAdjustments
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Adjustments, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool Voidctivity
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Activity, FSSSecurity.SecurityLists.FSS_Actions.Void); }
        }

        public static bool AllowNewPayments
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Payments, FSSSecurity.SecurityLists.FSS_Actions.Allow); }
        }

        public static bool AllowRefunding
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Refunds, FSSSecurity.SecurityLists.FSS_Actions.Allow); }
        }

        public static bool AllowAccountAdjustments
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Adjustments, FSSSecurity.SecurityLists.FSS_Actions.Allow); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Category Types

        public static bool viewCategoryTypes
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.CategoryType, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateCategoryTypes
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.CategoryType, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateCategoryTypes
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.CategoryType, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteCategoryType
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.CategoryType, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Categories
        public static bool viewCategories
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Categories, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateCategories
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Categories, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateCategories
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Categories, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteCategories
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Categories, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Menu
        public static bool viewMenu
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MenuItems, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateMenuObjects
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MenuItems, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateMenuDetails
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MenuItems, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteMenuObjects
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MenuItems, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Reports

        public static bool ViewCustomerReports
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.CustomerReports, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool ViewFinancialReports
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.FinancialReports, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool ViewPreOrderReports
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.PreOrderReports, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool ViewShoppingCartReports
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.ShoppingCartReports, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }
        public static bool ViewMSAAdminReports
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSAAdminReports, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }



        //Rights for MSA ADMIN
        /////////////////////////////////////////////////////////////////////////////////////////////////
        //Preorder
        public static bool ViewPreorderCalendars
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.PreorderCalendars, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreatePreorderCalendars
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.PreorderCalendars, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdatePreorderCalendars
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.PreorderCalendars, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeletePreorderCalendars
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.PreorderCalendars, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        public static bool viewPickUp
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Pickup, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        //Parent
        public static bool viewParent
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Parents, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateParent
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Parents, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateParent
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Parents, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteParent
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Parents, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        //MSAAlerts
        public static bool viewMSAAlerts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSAAlerts, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateMSAAlerts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSAAlerts, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateMSAAlerts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSAAlerts, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteMSAAlerts
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSAAlerts, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        //MSA Settings
        public static bool viewMSASettings
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSA_Settings, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateMSASettings
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSA_Settings, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateMSASettings
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSA_Settings, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteMSASettings
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.MSA_Settings, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        //Taxes
        public static bool viewTaxes
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Taxes, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreateTaxes
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Taxes, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdateTaxes
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Taxes, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeleteTaxes
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Taxes, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        //POS Notifications

        public static bool viewPOSNotifications
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POSNotifications, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        public static bool CreatePOSNotifications
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POSNotifications, FSSSecurity.SecurityLists.FSS_Actions.Create); }
        }

        public static bool UpdatePOSNotifications
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POSNotifications, FSSSecurity.SecurityLists.FSS_Actions.Update); }
        }

        public static bool DeletePOSNotifications
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.POSNotifications, FSSSecurity.SecurityLists.FSS_Actions.Delete); }
        }

        //Applications
        public static bool ViewApplications
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.Applications, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        //Beginning Balance
        public static bool viewBeginningBalance
        {
            get { return CheckPermission(FSSSecurity.SecurityLists.FSS_Modules.BeginningBalance, FSSSecurity.SecurityLists.FSS_Actions.View); }
        }

        //viewDashboard = 1,
        //   viewDistricts, CreateDistricts, UpdateDistricts, DeleteDistricts,
        //   viewSchools, CreateSchools, UpdateSchools, DeleteSchools,
        //   viewPOS, CreatePOS, UpdatePOS, DeletePOS,
        //   viewUserRoles, CreateUserRoles, UpdateUserRoles, DeleteUserRoles,
        //   viewUsers, CreateUsers, UpdateUsers, DeleteUsers,
        //   viewCustomers, CreateCustomers, UpdateCustomers, DeleteCustomers,
        //   viewHomerooms, CreateHomerooms, UpdateHomerooms, DeleteHomerooms,
        //   viewGrades, CreateGrades, UpdateGrades, DeleteGrades,
        //   viewActivity, Voidctivity,
        //   AllowNewPayments,
        //   AllowRefunding,
        //   AllowAccountAdjustments,
        //   viewCategoryTypes, CreateCategoryTypes, UpdateCategoryTypes, DeleteCategoryType,
        //   viewCategories, CreateCategories, UpdateCategories, DeleteCategories,
        //   viewMenu, CreateMenuObjects, UpdateMenuDetails, DeleteMenuObjects,
        //   ViewCustomerReports, ViewFinancialReports
    }
}
using Repository.edmx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Helpers
{
    public class DBHelper
    {
    }
    public static class ObjectContextEXT
    {
        public static void ExecuteStoreProcedure(this DbContext context, string storeProcName, params object[] parameters)
        {
            try
            {
                string command = "EXEC " + storeProcName + " @ClientID , @CustomerID OUTPUT, @DistrictID, @SchoolID, @UserID, @PIN, @LastName, @FirstName,	@LunchType,	@Student, @EatingAssignments, @LanguageID, @GradeID, @HomeroomID, @EthnicityID,	@Middle, @Gender, @SSN,	@Addr1,	@Addr2,	@City, @State, @Zip, @Phone, @GraduationDate, @graduationDateSet, @Notes, @Email, @DateOfBirth, @Active, @AllowAlaCarte, @NoCreditOnAccount, @AllowACH, @SnackProgram, @StudentWorker, @NotInDistrict, @CreationDate, @LocalTime, @PictureExtension, @StorageAccountName, @ContainerName, @PictureFileName, @EmployeeID, @ResultCode OUTPUT, @ErrorMessage OUTPUT";
                (context as IObjectContextAdapter).ObjectContext.ExecuteStoreCommand(command, parameters);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DBHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ExecuteStoreProcedure");
            }
        }

        public static void ExecuteVoidProcedure(this DbContext context, string storeProcName, params object[] parameters)
        {
            try
            {
                string command = "EXEC " + storeProcName + " @ClientID , @EMPLOYEEID, @ORDERID, @ORDERTYPE, @VOIDPAYMENT, @ORDLOGID, @ORDLOGNOTE, @ErrorMessage OUTPUT";
                (context as IObjectContextAdapter).ObjectContext.ExecuteStoreCommand(command, parameters);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DBHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ExecuteVoidProcedure");
            }
        }
        public static void ExecuteProcessPreorderPickupItems(this DbContext context, string storeProcName, params object[] parameters)
        {
            try
            {
                string command = "EXEC " + storeProcName + " @ClientID, @CashierId, @LocalDateTime, @SelectedPreorderItems, @Result OUTPUT, @ErrorMsg OUTPUT";
                (context as IObjectContextAdapter).ObjectContext.ExecuteStoreCommand(command, parameters);
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "DBHelper", "Error : " + ex.Message, CommonClasses.getCustomerID(), "ExecuteProcessPreorderPickupItems");
            }
        }
    }
}

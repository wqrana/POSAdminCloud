using AdminPortalModels.ViewModels;
using Repository.edmx;
using Repository.Helpers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;

namespace Repository
{
    public class ReportsRepository : IReportsRepository, IDisposable
    {
        private PortalContext context;

        public ReportsRepository(PortalContext context)
        {
            this.context = context;
        }

        public IEnumerable<CustomersList> getReportsCustomersList(long clientID)
        {
            try
            {
                return this.context.Admin_Customer_List(clientID, "", 0, "", 0).Select(c => new CustomersList
             {
                 value = c.Customer_Id,
                 data = c.First_Name
             }).ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getReportsCustomersList");
                return null;
            }
        }
        public IEnumerable<DepositTicket> getDepositTicket(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                IQueryable<DepositTicket> query = this.context.DepositTickets.AsNoTracking().Where(BuildWhereClause(clientID, reportID, rptFilters));
                //School(locations) id list
                var schoolsIDList = getCommaArray(rptFilters.location);

                if (schoolsIDList != null)
                {
                    Expression<Func<DepositTicket, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                    query = query.Where(theSchoolPredicate);
                }

                return query as IEnumerable<DepositTicket>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDepositTickets");
                return null;
            }


        }

        public IEnumerable<DailyCashier> getDailyCashier(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                IQueryable<DailyCashier> query = this.context.DailyCashiers.AsNoTracking().Where(BuildWhereClause(clientID, reportID, rptFilters));
                //School(locations) id list
                var schoolsIDList = getCommaArray(rptFilters.location);

                if (schoolsIDList != null)
                {
                    Expression<Func<DailyCashier, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                    query = query.Where(theSchoolPredicate);
                }

                return query as IEnumerable<DailyCashier>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDailyCashier");
                return null;
            }


        }
        //get customer's roster from databse
        public IEnumerable<CustomerRoster> getCustomerRoster(long clientID, int reportID, ReportsFilters rptFilters)
        {
            //
            try
            {
                IQueryable<CustomerRoster> query = this.context.CustomerRosters.Where(BuildWhereClause(clientID, reportID, rptFilters));

                //School(locations) id list
                var schoolsIDList = getCommaArray(rptFilters.location);

                if (schoolsIDList != null)
                {
                    Expression<Func<CustomerRoster, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                    query = query.Where(theSchoolPredicate);
                }
                ///////////////////////////////////
                //Homeroom id list
                var homeroomsIDList = getCommaArray(rptFilters.homeRoom);

                if (homeroomsIDList != null)
                {
                    Expression<Func<CustomerRoster, bool>> theHomeromPredicate = x => homeroomsIDList.Contains((long)x.HRID);
                    query = query.Where(theHomeromPredicate);
                }
                //////////////////////////
                //Grades id list
                var gradesIDList = getCommaArray(rptFilters.grade);

                if (gradesIDList != null)
                {
                    Expression<Func<CustomerRoster, bool>> theGradePredicate = x => gradesIDList.Contains((long)x.GRID);
                    query = query.Where(theGradePredicate);
                }
                //////////////////////
                //query = query.OrderBy(BuildOrderByClause(reportID, rptFilters));
                var customerIds = getSelectedCustomers(rptFilters); // new int[] { 4, 40, 43 };
                if (customerIds != null)
                {
                    query = query.Where(roster => customerIds.Contains(roster.CSTID));
                }

                return query as IEnumerable<CustomerRoster>;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getCustomerRoster");
                throw;
            }
            finally
            {

            }
            //
        }
        /// <summary>
        /// Function name is self-explanatory  it is mapped in database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="reportID"></param>
        /// <param name="rptFilters"></param>
        /// <returns></returns>
        public IEnumerable<CashierPayments> getPaymentsByCashier(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                IQueryable<CashierPayment> query = this.context.CashierPayments.Where(BuildWhereClause(clientID, reportID, rptFilters));
                if (rptFilters.location != "")
                {
                    //School(locations) id list
                    var schoolsIDList = getCommaArray(rptFilters.location);

                    if (schoolsIDList != null)
                    {
                        Expression<Func<CashierPayment, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                        query = query.Where(theSchoolPredicate);
                    }
                }

                return query.Select(cp => new CashierPayments
                {
                    ClientID = cp.ClientID,
                    DISTID = cp.DISTID,
                    SCHID = cp.SCHID,
                    CSTID = cp.CSTID,
                    EMPID = cp.EMPID,
                    POSID = cp.POSID,
                    DistrictName = cp.DistrictName,
                    SchoolID = cp.SchoolID,
                    SchoolName = cp.SchoolName,
                    EmpUserID = cp.EmpUserID,
                    CashierName = cp.CashierName,
                    OrderDate = cp.OrderDate,
                    PaymentDate = cp.PaymentDate,
                    CustUserID = cp.CustUserID,
                    LastName = cp.LastName,
                    FirstName = cp.FirstName,
                    Middle = cp.Middle,
                    TransType = cp.TransType,
                    PaymentAmount = cp.PaymentAmount,
                    Comments = cp.Comments,
                    TotalCashPayments = cp.TotalCashPayments,
                    TotalChecks = cp.TotalChecks,
                    TotalCredits = cp.TotalCredits,
                    OnlineCreditPayments = cp.OnlineCreditPayments,
                    OnlineACHPayments = cp.OnlineACHPayments,
                    OnlineACHReturns = cp.OnlineACHReturns,
                    Refunds = cp.Refunds,
                    AccountPayments = cp.AccountPayments,
                    CashSaleMonies = cp.CashSaleMonies,
                    TotalMonies = cp.TotalMonies,
                    TotalDeposit = cp.TotalDeposit,
                    LineCount = cp.LineCount,
                    GroupType = cp.GroupType,
                    FullName = cp.FullName
                });
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getPaymentsByCashier");
                return null;
            }

        }

        public IEnumerable<CustomerBalanceData_Result> getCustomerBalanceData(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                // Added by farrukh m (allshore) on 04/29/16 to fix item PA-513
                var customerIDs = getSelectedCustomersStr(rptFilters);

                // Inayat [15-Aug-2016] added start and end dates to the customer balance report.
                DateTime startDate = string.IsNullOrEmpty(rptFilters.fromDate) ? new DateTime() : Convert.ToDateTime(rptFilters.fromDate);
                DateTime endDate = string.IsNullOrEmpty(rptFilters.toDate) ? new DateTime() : Convert.ToDateTime(rptFilters.toDate);
                //------------------------------------------------

                string customerList = string.IsNullOrEmpty(customerIDs) ? "" : string.Format("({0})", customerIDs);
                string schoolList = string.IsNullOrEmpty(rptFilters.location) ? "" : string.Format("({0})", rptFilters.location);
                string gradeList = string.IsNullOrEmpty(rptFilters.grade) ? "" : string.Format("({0})", rptFilters.grade);
                string hoomroomList = string.IsNullOrEmpty(rptFilters.homeRoom) ? "" : string.Format("({0})", rptFilters.homeRoom);

                IEnumerable<CustomerBalanceData_Result> query = this.context.CustomerBalanceData(clientID, customerList, schoolList, gradeList, hoomroomList, startDate, endDate);

                //--------------------------------------------

                var gradesArray = getCommaArray(rptFilters.grade);
                var homeroomsArray = getCommaArray(rptFilters.homeRoom);

                if (homeroomsArray != null)
                {
                    query = query.Where(x => homeroomsArray.Contains((long)x.HRID));
                }

                if (gradesArray != null)
                {
                    query = query.Where(x => gradesArray.Contains((long)x.GRID));
                }
                //----------------------------------------------------------


                if (!string.IsNullOrEmpty(rptFilters.balanceActTypes))
                {
                    string tempbalanceActTypes = rptFilters.balanceActTypes;

                    switch (tempbalanceActTypes)
                    {
                        case "All Except Zero Balances":
                            query = query.Where(b => b.ABalance + b.MBalance != 0);
                            break;
                        case "All Except Free with Zero Balances":
                            //NOT (Customers.Lunchtype = 3 and Balance = 0.00)
                            query = query.Where(b => b.ABalance + b.MBalance != 0);
                            //Added by farrukh m (allshore) on 05/11/16 to fix item PA-513
                            query = query.Where(b => b.LunchType != 3);

                            break;

                        default:
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(rptFilters.accountype))
                {
                    string tempAccountStr = rptFilters.accountype;

                    switch (tempAccountStr)
                    {
                        case "Positive":
                            query = query.Where(b => b.ABalance + b.MBalance > 0);
                            break;
                        case "Negative":
                            query = query.Where(b => b.ABalance + b.MBalance < 0);
                            break;
                        case "Range":
                            double startRange = double.Parse(rptFilters.range_slider_input_start.Replace("$", ""));
                            double endrange = double.Parse(rptFilters.range_slider_input_end.Replace("$", ""));
                            query = query.Where(b => b.ABalance + b.MBalance >= startRange && b.ABalance + b.MBalance <= endrange);
                            break;

                        case "Greater Than":
                            double minvalue = double.Parse(rptFilters.slider_range_min_amount.Replace("$", ""));
                            query = query.Where(b => b.ABalance + b.MBalance > minvalue);
                            break;

                        case "Less Than":
                            string tempStr = rptFilters.slider_range_max_amount.Replace("$", "");
                            double maxValue = double.Parse(tempStr);
                            query = query.Where(b => b.ABalance + b.MBalance < maxValue);
                            break;
                        default:
                            break;
                    }

                }

                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getCustomerBalanceData");
                return null;
            }

        }

        /// <summary>
        /// Function name is self-explanatory, it is mapped in database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="reportID"></param>
        /// <param name="rptFilters"></param>
        /// <returns></returns>
        public IEnumerable<DetailCafeCategory> getDetailCafeCategories(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                IQueryable<DetailCafeCategory> query = this.context.DetailCafeCategories.Where(BuildWhereClause(clientID, reportID, rptFilters));

                //School(locations) id list
                var schoolsIDList = getCommaArray(rptFilters.location);

                if (schoolsIDList != null)
                {
                    Expression<Func<DetailCafeCategory, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                    query = query.Where(theSchoolPredicate);
                }

                return query as IEnumerable<DetailCafeCategory>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDetailCafeCategories");
                return null;
            }
        }
        /// <summary>
        /// Function name is self-explanatory,  it is mapped in database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<SalesTaxOnOrder> getSalesTaxOnOrders(long clientID)
        {
            try
            {


                IQueryable<SalesTaxOnOrder> query = this.context.SalesTaxOnOrders.Where(" ClientID=" + clientID);


                return query as IEnumerable<SalesTaxOnOrder>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSalesTaxOnOrders");
                return null;
            }
        }
        /// <summary>
        /// Function name is self-explanatory,  it is mapped in database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<DetailCafeCashier> getDetailCafeCashiers(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                string whereClause = " ClientID=" + clientID;

                whereClause += " and OpenDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                whereClause += " and OpenDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";

                IQueryable<DetailCafeCashier> query = this.context.DetailCafeCashiers.Where(whereClause);

                //School(locations) id list
                if (rptFilters.location != "")
                {
                    var schoolsIDList = getCommaArray(rptFilters.location);

                    if (schoolsIDList != null)
                    {
                        Expression<Func<DetailCafeCashier, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                        query = query.Where(theSchoolPredicate);
                    }
                }

                return query as IEnumerable<DetailCafeCashier>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDetailCafeCashiers");
                return null;
            }
        }
        /// <summary>
        /// Function name is self-explanatory,  it is mapped in database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<DetailCafeSummary> getDetailCafeSummaries(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                string whereClause = " ClientID=" + clientID;
                whereClause += " and ReportDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                whereClause += " and ReportDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";
                IQueryable<DetailCafeSummary> query = this.context.DetailCafeSummaries.Where(whereClause);

                //School(locations) id list
                if (rptFilters.location != "")
                {
                    var schoolsIDList = getCommaArray(rptFilters.location);

                    if (schoolsIDList != null)
                    {
                        Expression<Func<DetailCafeSummary, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                        query = query.Where(theSchoolPredicate);
                    }
                }


                return query as IEnumerable<DetailCafeSummary>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDetailCafeSummaries");
                return null;
            }
        }
        /// <summary>
        /// Function name is self-explanatory,  it is mapped in database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<AccountBalanceInfo_Result> getAccountInfoes(long clientID, ReportsFilters rptFilters)
        {
            try
            {
                DateTime? StartDate = Convert.ToDateTime(rptFilters.fromDate);
                DateTime? EndDate = Convert.ToDateTime(rptFilters.toDate);
                DateTime? OrderDate = Convert.ToDateTime(rptFilters.fromDate);
                string whereClause = " ClientID=" + clientID;
                // TODO: Change this to use a sql statement, stored procedure, view, function, etc.  Remove table
                IQueryable<AccountBalanceInfo_Result> query = this.context.AccountBalanceInfo(clientID, StartDate, EndDate, OrderDate, string.IsNullOrEmpty(rptFilters.location) ? "" : string.Format("({0})", rptFilters.location));
                return query as IEnumerable<AccountBalanceInfo_Result>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getAccountInfoes");
                return null;
            }
        }
        /// <summary>
        /// Function name is self-explanatory,  it is mapped in database 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public IEnumerable<DetailCafeNetChange> getDetailCafeNetChanges(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                string whereClause = " ClientID=" + clientID;

                IQueryable<DetailCafeNetChange> query = this.context.DetailCafeNetChanges.Where(whereClause);

                //School(locations) id list
                var schoolsIDList = getCommaArray(rptFilters.location);

                if (schoolsIDList != null)
                {
                    Expression<Func<DetailCafeNetChange, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                    query = query.Where(theSchoolPredicate);
                }

                return query as IEnumerable<DetailCafeNetChange>;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDetailCafeNetChanges");
                return null;
            }
        }

        public IEnumerable<SoldItems> getSoldItems(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                string whereClause = " ClientID= " + clientID;


                whereClause += " and GDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                whereClause += " and GDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";

                IQueryable<SoldItem> query = this.context.SoldItems.Where(whereClause);

                if (rptFilters.location != "")
                {
                    var schoolsIDList = getCommaArray(rptFilters.location);

                    if (schoolsIDList != null)
                    {
                        Expression<Func<SoldItem, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                        query = query.Where(theSchoolPredicate);
                    }
                }

                // changed by farrukh on 04/27/16 to fix item PA-510
                var soldItems = (from si in query
                                 group si by new { si.clientid, si.SCHID, si.GDate, si.MenuItem, si.Price } into g
                                 select new SoldItems
                                 {
                                     clientid = g.Key.clientid,
                                     SCHID = g.Key.SCHID,
                                     GDate = g.Key.GDate,
                                     MenuItem = g.Key.MenuItem,
                                     Qty = g.Sum(m => m.Qty),
                                     Price = g.Key.Price
                                 }).ToList();

                //farrukh commented this code on 04/27/16 to fix item PA-510
                /*  return query.Select(si => new SoldItems
                  {
                      clientid = si.clientid,
                      SCHID = si.SCHID,
                      GDate = si.GDate,
                      MenuItem = si.MenuItem,
                      Qty = si.Qty,
                      Price = si.Price

                  });*/

                return soldItems;

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getSoldItems");
                return null;
            }
        }


        public IEnumerable<VoidsD> getVoidItems(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                string whereClause = " ClientID= " + clientID;

                // Commented by farrukh m (allshore) on 05/10/16 to fix item PA-519
                //if (rptFilters.location != "" && rptFilters.location != null)
                //{
                //  whereClause = whereClause + " and SCHID=" + rptFilters.location;
                //}

                ////Date Types (by farrukh m (allshore) on 05/10/16 (PA-519)
                if (!string.IsNullOrEmpty(rptFilters.dateRangeTypes))
                {
                    string tempdateRangeTypes = rptFilters.dateRangeTypes;

                    switch (tempdateRangeTypes)
                    {
                        case "By Order Date":

                            whereClause += "  and GDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                            whereClause += "  and GDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";
                            whereClause += "  and Type = 0 ";

                            break;
                        case "By Voided Date":

                            whereClause += " and voidtime >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                            whereClause += " and voidtime <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";
                            whereClause += "  and Type = 1 ";
                            break;

                        default:
                            break;
                    }
                }
                ///--------------------------------- farrukh m (allshore) in 05/10/16




                IQueryable<VoidsD> query = this.context.VoidsDS.Where(whereClause);

                //School(locations) id list
                if (rptFilters.location != "" && rptFilters.location != null)
                {
                    var schoolsIDList = getCommaArray(rptFilters.location);
                    if (schoolsIDList != null)
                    {
                        Expression<Func<VoidsD, bool>> theSchoolPredicate = x => schoolsIDList.Contains((long)x.SCHID);
                        query = query.Where(theSchoolPredicate);
                    }
                }

                return query;

            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getVoidItems");
                return null;
            }
        }
        public IEnumerable<Reporting_SoldItemsByCategory_Result> getSoldItemsByCategory(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                IQueryable<Reporting_SoldItemsByCategory_Result> query = this.context.Reporting_SoldItemsByCategory(clientID, rptFilters.location, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate));

                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getStatementOrdersSummary");
                return null;
            }
        }
        public IEnumerable<Reporting_CustomerBySoldItems_Result> getCustomerBySoldItems(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                string lunchtypes = getLunchTypeList(rptFilters.qualificationTypes);
                string selectedList = (rptFilters.selectedTypeList != null && rptFilters.selectedTypeList.Length > 0) ? string.Join(",", rptFilters.selectedTypeList) : null;
                IQueryable<Reporting_CustomerBySoldItems_Result> query = this.context.Reporting_CustomerBySoldItems(clientID, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate), rptFilters.location, rptFilters.grade, rptFilters.homeRoom, lunchtypes, Convert.ToInt32(rptFilters.itemSelectionType), selectedList, rptFilters.selectedQtyType, rptFilters.minQty, rptFilters.maxQty);

                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getStatementOrdersSummary");
                return null;
            }
        }

        public IEnumerable<Reporting_StatementOrdersSummary_Result> getStatementOrdersSummary(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {

                //Int32 activeType = GetActiveTypeParam(reportID, rptFilters);
                //Int32 deletedType = GetdeletedTypeParam(reportID, rptFilters);

                // by farrukh on 05/02/2016 to fix PA-508
                if (rptFilters.allCustomers == true)
                {
                    rptFilters.SelectedCustomersList = "";
                }
                else
                {
                    rptFilters.SelectedCustomersList = rptFilters.SelectedCustomersList.Replace("|", ",");
                }

                IQueryable<Reporting_StatementOrdersSummary_Result> query = this.context.Reporting_StatementOrdersSummary(clientID, rptFilters.SelectedCustomersList, rptFilters.location, rptFilters.grade, rptFilters.homeRoom, 2, 2, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate));
                if (rptFilters.sortOrder == "School")
                {
                    query = query.OrderBy(o => o.SCHID);
                }

                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getStatementOrdersSummary");
                return null;
            }
        }

        public IEnumerable<Reporting_CustomerRosterSummary_Result> getCustomerRosterSummary(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                DateTime? dt = Convert.ToDateTime("1/1/1900");

                //Added by farrukh M. (Allshore) on 05/03/2016 to fix PA-508
                //  Int32 activeType = GetActiveTypeParam(reportID, rptFilters);
                // Int32 deletedType = GetdeletedTypeParam(reportID, rptFilters);
                //---------------------------------------------------------

                string zeroBalStr = "ALL";
                if (!string.IsNullOrEmpty(rptFilters.balanceActTypes))
                {
                    zeroBalStr = rptFilters.balanceActTypes.Replace(" ", "").ToUpper();
                }

                string accountTypeParam = "ALL";
                double startRange = -10000;
                double endrange = 10000;
                double minvalue = -10000;
                double maxValue = 10000;

                if (!string.IsNullOrEmpty(rptFilters.accountype))
                {
                    //string tempAccountStr = rptFilters.accountype.Replace(" ", "").ToUpper();

                    string tempAccountStr = rptFilters.accountype;

                    switch (tempAccountStr)
                    {
                        case "Positive":
                            accountTypeParam = "Positive";
                            break;
                        case "Negative":
                            accountTypeParam = "Negative";
                            break;
                        case "Range":
                            accountTypeParam = "Range";
                            startRange = double.Parse(rptFilters.range_slider_input_start.Replace("$", ""));
                            endrange = double.Parse(rptFilters.range_slider_input_end.Replace("$", ""));
                            break;

                        case "Greater Than":
                            accountTypeParam = "GreaterThan";
                            minvalue = double.Parse(rptFilters.slider_range_min_amount.Replace("$", ""));
                            break;

                        case "Less Than":
                            accountTypeParam = "LessThan";
                            string tempStr = rptFilters.slider_range_max_amount.Replace("$", "");
                            maxValue = double.Parse(tempStr);
                            break;
                        default:
                            break;
                    }
                }


                var customersList = getSelectedCustomersStr(rptFilters);

                customersList = customersList ?? "";

                //changed by farrukh M. (Allshore) on 05/16/2016 to fix PA-498
                IQueryable<Reporting_CustomerRosterSummary_Result> query = this.context.Reporting_CustomerRosterSummary(clientID, customersList, rptFilters.location, rptFilters.grade, rptFilters.homeRoom, "", 2, 2, zeroBalStr, accountTypeParam, startRange, endrange, minvalue, maxValue, false, dt, dt, 0, null, true, Convert.ToDateTime(rptFilters.fromDate)).Where(BuildWhereClause(null, reportID, rptFilters, false, false, false));

                //IQueryable<Reporting_CustomerRosterSummary_Result> query = this.context.Reporting_CustomerRosterSummary(44, "", "", "", "", "", 2, 2, "ALLEXCEPTZEROBALANCES", "ALL", -10000, 10000, -10000, 10000, false, dt, dt, 0, null, true, dt);

                //if (rptFilters.sortOrder1 == "School")
                //{
                //    query = query.OrderBy(o => o.SCHID);
                //}


                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getStatementOrdersSummary");
                return null;
            }
        }

        public IEnumerable<Reporting_StatementDetailedOrders_Result> getStatementDetailedOrders(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                //Commented by farrukh M. (Allshore) on 05/04/2016 to fix PA-508 --
                //Int32 activeType = GetActiveTypeParam(reportID, rptFilters);
                //Int32 deletedType = GetdeletedTypeParam(reportID, rptFilters);

                // by farrukh on 05/03/2016 to fix PA-508
                if (rptFilters.allCustomers == true)
                {
                    rptFilters.SelectedCustomersList = "";
                }
                else
                {
                    rptFilters.SelectedCustomersList = rptFilters.SelectedCustomersList.Replace("|", ",");
                }

                //Added by farrukh M. (Allshore) on 05/03/2016 to fix PA-508
                IQueryable<Reporting_StatementDetailedOrders_Result> query = this.context.Reporting_StatementDetailedOrders(clientID, rptFilters.SelectedCustomersList, rptFilters.location, rptFilters.grade, rptFilters.homeRoom, 2, 2, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate));

                //Commented by farrukh M. (Allshore) on 05/03/2016 to fix PA-508 ------------------------------------------
                //IQueryable<Reporting_StatementDetailedOrders_Result> query = this.context.Reporting_StatementDetailedOrders(clientID, rptFilters.SelectedCustomersList, rptFilters.location, rptFilters.grade, rptFilters.homeRoom, activeType, deletedType, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate)).Where(BuildWhereClause(null, reportID, rptFilters, false, false, false));
                //---------------Commented by farrukh M. (Allshore) on 05/03/2016 to fix PA-508 ----------------------------------------

                //if (rptFilters.sortOrder1 == "School")
                //{
                //    query = query.OrderBy(o => o.SCHID);
                //}

                //var schIDList = new Int64[]{};
                //if (rptFilters.location != "" && rptFilters.location != null)
                //{
                //    var schIDList = getCommaArray(rptFilters.location);
                //    query = query.Where(x => schIDList.Contains(x.SCHID));
                //}

                //if (!string.IsNullOrEmpty(rptFilters.homeRoom))
                //{
                //    var homeroomIDList = getCommaArray(rptFilters.homeRoom);
                //    query = query.Where(x => homeroomIDList.Contains(x.SCHID));
                //}

                ////add grade filter
                //if (!string.IsNullOrEmpty(rptFilters.grade))
                //{
                //    var gardeIDList = getCommaArray(rptFilters.grade);
                //    query = query.Where(x => gardeIDList.Contains(x.gr.SCHID));
                //}

                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getStatementDetailedOrders");
                return null;
            }
        }

        public IEnumerable<Reporting_StatementDetailedItems_Result> getStatementDetailedItems(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                //Int32 activeType = GetActiveTypeParam(reportID, rptFilters);
                //Int32 deletedType = GetdeletedTypeParam(reportID, rptFilters);

                // by farrukh on 05/03/2016 to fix PA-508
                if (rptFilters.allCustomers == true)
                {
                    rptFilters.SelectedCustomersList = "";
                }
                else
                {
                    rptFilters.SelectedCustomersList = rptFilters.SelectedCustomersList.Replace("|", ",");
                }

                //Added by farrukh M. (Allshore) on 05/03/2016 to fix PA-508
                IQueryable<Reporting_StatementDetailedItems_Result> query = this.context.Reporting_StatementDetailedItems(clientID, rptFilters.SelectedCustomersList, rptFilters.location, rptFilters.grade, rptFilters.homeRoom, 2, 2, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate));

                //Commented by farrukh M. (Allshore) on 05/03/2016 to fix PA-508 ------------------------------------------
                //IQueryable<Reporting_StatementDetailedItems_Result> query = this.context.Reporting_StatementDetailedItems(clientID, rptFilters.SelectedCustomersList, rptFilters.location, rptFilters.grade, rptFilters.homeRoom, activeType, deletedType, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate)).Where(BuildWhereClause(null, reportID, rptFilters, false, false, false));
                //---------------Commented by farrukh M. (Allshore) on 05/03/2016 to fix PA-508 ----------------------------------------

                //if (rptFilters.sortOrder1 == "School")
                //{
                //    query = query.OrderBy(o => o.SCHID);
                //}
                //var schIDList = new Int64[]{};
                //if (rptFilters.location != "" && rptFilters.location != null)
                //{
                //    var schIDList = getCommaArray(rptFilters.location);
                //    query = query.Where(x => schIDList.Contains(x.SCHID));
                //}

                //if (!string.IsNullOrEmpty(rptFilters.homeRoom))
                //{
                //    var homeroomIDList = getCommaArray(rptFilters.homeRoom);
                //    query = query.Where(x => homeroomIDList.Contains(x.SCHID));
                //}

                ////add grade filter
                //if (!string.IsNullOrEmpty(rptFilters.grade))
                //{
                //    var gardeIDList = getCommaArray(rptFilters.grade);
                //    query = query.Where(x => gardeIDList.Contains(x.gr.SCHID));
                //}

                return query;
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getStatementDetailedOrders");
                return null;
            }
        }

        public IEnumerable<Reporting_DetailedTransactions_Result> getDetailedTransactions(long clientID, int reportID, ReportsFilters rptFilters)
        {
            try
            {
                IQueryable<Reporting_DetailedTransactions_Result> query = this.context.Reporting_DetailedTransactions(clientID, rptFilters.location, Convert.ToDateTime(rptFilters.fromDate), Convert.ToDateTime(rptFilters.toDate));
                return query.ToList();
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "getDetailedTransactions");
                return null;
            }
        }


        /// <summary>
        /// Basic purpose of this function is to build dynamic where clause and pass it IQueryable
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="rptID"></param>
        /// <param name="rptFilters"></param>
        /// <returns></returns>
        private string BuildWhereClause(long? clientID, int rptID, ReportsFilters rptFilters, bool includeLocation = true, bool includeHR = true, bool includeGR = true)
        {
            try
            {

                int reportID = rptID;
                string retWhereClause = " 1 = 1 ";
                string clientIdstr = string.Empty;

                if (clientID != null && clientID.HasValue)
                {
                    clientIdstr = Convert.ToString(clientID);
                }

                if (!string.IsNullOrEmpty(clientIdstr))
                {
                    retWhereClause = " ClientID = " + clientIdstr;
                }


                ShowHideReportsFilters showHideReportsFilters = new ShowHideReportsFilters(reportID);

                if (showHideReportsFilters.ShowDateRange)
                {
                    FSS.Reports.FSS_REPORTS ReportID = ((FSS.Reports.FSS_REPORTS)reportID);

                    // build where clause according to column name, for each report 
                    switch (ReportID)
                    {
                        case FSS.Reports.FSS_REPORTS.REP_DAILY_CASHIER:

                            retWhereClause += " and OpenDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                            retWhereClause += " and OpenDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";
                            break;

                        case FSS.Reports.FSS_REPORTS.REP_DEPOSIT_TICKET:
                            retWhereClause += " and ReportDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                            retWhereClause += " and ReportDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";
                            break;
                        case FSS.Reports.FSS_REPORTS.REP_PAYMENTS_BY_CASHIER:
                            retWhereClause += " and PaymentDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                            retWhereClause += " and PaymentDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";
                            break;

                        case FSS.Reports.FSS_REPORTS.REP_CAFETERIA:

                            retWhereClause += " and OrderDate >= (\"" + DateTime.Now.ToString(rptFilters.fromDate) + "\")";
                            retWhereClause += " and OrderDate <= (\"" + DateTime.Now.ToString(rptFilters.toDate) + "\")";

                            break;

                        default:
                            break;
                    }

                }

                // add location filter
                //if (showHideReportsFilters.ShowLocactions && rptFilters.location != "" && rptFilters.location != null && includeLocation)
                //{
                //    //getCommaArray
                //    retWhereClause = retWhereClause + " and SCHID = " + rptFilters.location; // +rptFilters.location;
                //}

                ////add homeroom filter
                //if (showHideReportsFilters.ShowHomeRooms && rptFilters.homeRoom != "" && includeHR)
                //{
                //    retWhereClause = retWhereClause + " and HRID =" + rptFilters.homeRoom;
                //}
                ////add grade filter
                //if (showHideReportsFilters.ShowGrade && rptFilters.grade != "" && includeGR)
                //{
                //    retWhereClause = retWhereClause + " and GRID =" + rptFilters.grade;
                //}

                // add account status
                if (showHideReportsFilters.ShowAccountStatus && rptFilters.accountStatus.Length > 0)
                {
                    //isActiveInt 0 1
                    retWhereClause = retWhereClause + " and ( ";
                    if (rptFilters.accountStatus.Any("Active".Contains))
                    {
                        retWhereClause = retWhereClause + " isActive = true or";
                    }
                    if (rptFilters.accountStatus.Any("Inactive".Contains))
                    {
                        retWhereClause = retWhereClause + " isActive = false or";
                    }
                    if (rptFilters.accountStatus.Any("Deleted".Contains))
                    {
                        retWhereClause = retWhereClause + " isDeleted = true or";
                    }
                    if (rptFilters.accountStatus.Any("Frozen".Contains))
                    {
                        retWhereClause = retWhereClause + " isFrozen = 1";
                    }
                    retWhereClause = retWhereClause + ") ";
                    retWhereClause = retWhereClause.Replace("or)", " ) ");

                }

                //Qualification Types: 
                if (showHideReportsFilters.ShowQualificationTypes && rptFilters.qualificationTypes.Length > 0)
                {
                    retWhereClause = retWhereClause + " and ( ";
                    //check if user selected paid
                    if (rptFilters.qualificationTypes.Any("Paid".Contains))
                    {
                        retWhereClause = retWhereClause + " LunchType = 1 or";
                    }
                    if (rptFilters.qualificationTypes.Any("Reduced".Contains))
                    {
                        retWhereClause = retWhereClause + " LunchType = 2 or";
                    }
                    if (rptFilters.qualificationTypes.Any("Free".Contains))
                    {
                        retWhereClause = retWhereClause + " LunchType = 3 or";
                    }
                    if (rptFilters.qualificationTypes.Any("Adults".Contains))
                    {
                        retWhereClause = retWhereClause + " LunchType = 4 or"; // Changed LunchType > 5 to LunchType = 4  by farrukh M. (Allshore) on 05/03/2016
                    }
                    if (rptFilters.qualificationTypes.Any("Meal Plan".Contains))
                    {
                        retWhereClause = retWhereClause + " LunchType = 5 or";
                    }
                    retWhereClause = retWhereClause + ") ";
                    retWhereClause = retWhereClause.Replace("or)", " ) ");
                }
                //Deposit Type:
                if (showHideReportsFilters.ShowDepositType)
                {
                    if (rptFilters.DepositTypeList == "POS & Admin")
                    {
                        retWhereClause = retWhereClause + " and (DepositType = 0 OR DepositType = 1 OR DepositType = 2 )";
                    }
                    else if (rptFilters.DepositTypeList == "POS Deposit Only")
                    {
                        retWhereClause = retWhereClause + " and DepositType = 0 ";
                    }
                    else if (rptFilters.DepositTypeList == "Admin Deposit Only")
                    {
                        retWhereClause = retWhereClause + " and (DepositType = 1 OR DepositType = 2) ";
                    }
                }
                //Session Type:
                if (showHideReportsFilters.ShowSessionType)
                {
                    if (rptFilters.SessionTypeList == "POS Session Only")
                    {
                        retWhereClause = retWhereClause + " and LoginName = \"Administrator\" ";
                    }
                    else if (rptFilters.SessionTypeList == "Admin Session Only")
                    {
                        retWhereClause = retWhereClause + " and LoginName <> \"Administrator\" ";
                    }
                    else if (rptFilters.SessionTypeList == "Both")
                    {
                        //do nothing
                    }

                }


                return retWhereClause;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "BuildWhereClause");
                throw;
            }

        }

        //this function builds dynmaic where clause
        private string BuildOrderByClause(int rptID, ReportsFilters rptFilters)
        {
            int reportID = rptID;
            string retOrderByClause = "";
            ShowHideReportsFilters showHideReportsFilters = new ShowHideReportsFilters(reportID);

            try
            {
                //retOrderByClause = retOrderByClause + rptFilters.sortOrder;
                //retOrderByClause = retOrderByClause.Replace(","," , ");
                var sortOrderList = getSortingList(rptFilters.sortOrder);


                foreach (var item in sortOrderList)
                {
                    retOrderByClause = retOrderByClause + getColumnName(item) + ",";
                }
                retOrderByClause = retOrderByClause.TrimEnd(',');
                retOrderByClause = retOrderByClause.Replace(",", " , ");

                //if (showHideReportsFilters.ShowsortOrder && rptFilters.sortOrder1 != "")
                //{
                //    retOrderByClause = retOrderByClause + getColumnName(rptFilters.sortOrder1) + " , ";
                //}
                //if (showHideReportsFilters.ShowsortOrder && rptFilters.sortOrder2 != "")
                //{
                //    retOrderByClause = retOrderByClause + getColumnName(rptFilters.sortOrder2) + " , ";
                //}
                //if (showHideReportsFilters.ShowsortOrder && rptFilters.sortOrder3 != "")
                //{
                //    retOrderByClause = retOrderByClause + getColumnName(rptFilters.sortOrder3) + " , ";
                //}
                //if (showHideReportsFilters.ShowsortOrder && rptFilters.sortOrder4 != "")
                //{
                //    retOrderByClause = retOrderByClause + getColumnName(rptFilters.sortOrder4);
                //}
            }
            catch (Exception ex)
            {
                //Error logging in cloud tables
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "ReportsRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "BuildOrderByClause");
            }
            return retOrderByClause;
        }


        /// <summary>
        /// This function takes customer ids in format [2|43|33] 
        /// And converts them to integer array.
        /// </summary>
        /// <param name="rptFilters"></param>
        /// <returns></returns>
        private Int64[] getSelectedCustomers(ReportsFilters rptFilters)
        {
            Int64[] tempArray = null;
            if (rptFilters.allCustomers == true) return null;
            if (rptFilters.SelectedCustomersList.Length > 0)
            {
                tempArray = rptFilters.SelectedCustomersList.Split('|').Select(Int64.Parse).ToArray(); // { 4, 40, 43 };
            }
            else
            {

            }
            return tempArray;

        }

        private string getSelectedCustomersStr(ReportsFilters rptFilters)
        {
            string tempStr = "";
            if (rptFilters.allCustomers == true) return tempStr;
            if (rptFilters.SelectedCustomersList.Length > 0)
            {
                tempStr = rptFilters.SelectedCustomersList.Replace("|", ",");
            }
            return tempStr;

        }

        public List<string> getSortingList(string sortingList)
        {

            List<string> sortinglist = sortingList.Split(',').Select(x => x.Trim()).ToList();
            return sortinglist;
        }

        private Int64[] getCommaArray(string inputStr)
        {
            Int64[] tempArray = null;
            if (inputStr == "") return null;
            if (inputStr.Length > 0)
            {
                tempArray = inputStr.Split(',').Select(Int64.Parse).ToArray(); // { 4, 40, 43 };
            }

            return tempArray;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="colName"></param>
        /// <returns></returns>

        private string getColumnName(string colName)
        {
            string tempColName = colName;
            string retStr = "";
            switch (tempColName)
            {
                case "School":
                    retStr = "SCHID";
                    break;
                case "Grade":
                    retStr = "GRID";
                    break;
                case "Homeroom":
                    retStr = "HRID";
                    break;
                case "Customer Name":
                    retStr = "LastName";
                    break;

                default:
                    retStr = "LastName";
                    break;
            }
            return retStr;
        }

        private Int32 GetdeletedTypeParam(int rptID, ReportsFilters rptFilters)
        {
            Int32 deletedParam = 2;
            ShowHideReportsFilters showHideReportsFilters = new ShowHideReportsFilters(rptID);
            if (showHideReportsFilters.ShowAccountStatus && rptFilters.accountStatus.Length > 0)
            {
                if (rptFilters.accountStatus.Any("Deleted".Contains))
                {
                    deletedParam = 1; // change 0 to 1 by farrukh on 05/03/16
                }

                //}
                //if (rptFilters.accountStatus.Any("Frozen".Contains))
                //{
                //    retWhereClause = retWhereClause + " isFrozen = 1";
                //}
                //retWhereClause = retWhereClause + ") ";
                //retWhereClause = retWhereClause.Replace("or)", " ) ");



            }
            return deletedParam;
        }

        private Int32 GetActiveTypeParam(int rptID, ReportsFilters rptFilters)
        {
            Int32 activeParam = 2;
            ShowHideReportsFilters showHideReportsFilters = new ShowHideReportsFilters(rptID);
            if (showHideReportsFilters.ShowAccountStatus && rptFilters.accountStatus.Length > 0)
            {
                if (rptFilters.accountStatus.Any("Active".Contains) && rptFilters.accountStatus.Any("Inactive".Contains))
                {
                    activeParam = 2;
                }
                else
                {
                    if (rptFilters.accountStatus.Any("Active".Contains))
                    {
                        activeParam = 1; // change 0 to 1 by farrukh on 05/03/16
                    }
                    else
                    {
                        if (rptFilters.accountStatus.Any("Inactive".Contains))
                        {
                            activeParam = 0;
                        }

                    }

                }
            }
            return activeParam;
        }

        private string getLunchTypeList(string[] LunhTypelist)
        {
            IList<string> lunchValues = new List<string>();

            if (LunhTypelist.Any("Paid".Contains))
            {
                lunchValues.Add("1");
            }
            if (LunhTypelist.Any("Reduced".Contains))
            {
                lunchValues.Add("2");
            }
            if (LunhTypelist.Any("Free".Contains))
            {
                lunchValues.Add("3");
            }
            if (LunhTypelist.Any("Adults".Contains))
            {
                lunchValues.Add("4");
            }
            if (LunhTypelist.Any("Meal Plan".Contains))
            {
                lunchValues.Add("5");
            }
            string joined = string.Join(",", lunchValues);
            return joined;


        }

        //private IQueryable<T> GetQueryable<T>(IQueryable<T> query, string location)
        //{
        //    var schIDList = getCommaArray(location);
        //    query = query.Where(x => schIDList.Contains((long)x.SCHID));
        //    return query;
        //}



        /// <summary>
        /// dispose after usage
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

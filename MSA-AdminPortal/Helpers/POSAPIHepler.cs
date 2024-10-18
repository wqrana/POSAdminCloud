using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.Azure;

namespace MSA_AdminPortal.Helpers
{
    public class POSAPIHepler
    {
        private const string POSUrl = "POSLiveUrl";
        private static readonly string BaseUrl = CloudConfigurationManager.GetSetting(POSUrl);

        public static DataSet GetProductionSummary(long districtID, string startDate, string endDate, out bool isError, out string message)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string serviceUrl = "api/ReportsAPI/GetMsaAdminProductionSummary?clientID=" + districtID + "&startDate=" + startDate + "&endDate=" + endDate;

                var response = CallPOSAPI(serviceUrl);

                var ordersData = response.Content.ReadAsAsync<ProductionSummaryDetailed>().Result;
                var students = ToDataTable(ordersData.Students);
                var parents = ToDataTable(ordersData.Parents);
                var schools = ToDataTable(ordersData.Schools);
                var menus = ToDataTable(ordersData.Menus);
                var districts = ToDataTable(ordersData.Districts);
                var categories = ToDataTable(ordersData.Categories);
                var presaleTransactions = ToDataTable(ordersData.PreSaleTransactions);

                //var distTable = GetDistrictById(districtID);
                //districts.Rows[0]["Name"] = distTable.Rows[0]["Name"];

                students.TableName = "Students";
                parents.TableName = "Parents";
                schools.TableName = "Schools";
                menus.TableName = "Menu";
                districts.TableName = "Districts";
                categories.TableName = "Category";
                presaleTransactions.TableName = "PreSaleTransactions";

                dataSet.Tables.Add(students);
                dataSet.Tables.Add(parents);
                dataSet.Tables.Add(schools);
                dataSet.Tables.Add(menus);
                dataSet.Tables.Add(districts);
                dataSet.Tables.Add(categories);
                dataSet.Tables.Add(presaleTransactions);
                isError = false;
                message = string.Empty;

            }
            catch (Exception ex)
            {
                isError = true;
                message = ex.Message;
            }
            return dataSet;
        }

        public static DataSet GetPreorderDistribution(long districtID, string startDate, out bool isError, out string message)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string serviceUrl = "api/ReportsAPI/GetMsaAdminPreorderDistribution?clientID=" + districtID + "&startDate=" + startDate;

                var response = CallPOSAPI(serviceUrl);

                var ordersData = response.Content.ReadAsAsync<ProductionSummaryDetailed>().Result;
                var students = ToDataTable(ordersData.Students);
                var menus = ToDataTable(ordersData.Menus);
                var districts = ToDataTable(ordersData.Districts);
                var presaleTransactions = ToDataTable(ordersData.PreSaleTransactions);

                //var distTable = GetDistrictById(districtID);
                //districts.Rows[0]["Name"] = distTable.Rows[0]["Name"];

                students.TableName = "Students";
                menus.TableName = "Menu";
                districts.TableName = "Districts";
                presaleTransactions.TableName = "PreSaleTransactions";

                dataSet.Tables.Add(students);
                dataSet.Tables.Add(menus);
                dataSet.Tables.Add(districts);
                dataSet.Tables.Add(presaleTransactions);
                isError = false;
                message = string.Empty;

            }
            catch (Exception ex)
            {
                isError = true;
                message = ex.Message;
            }
            return dataSet;
        }

        public static DataSet GetProductionSummaryByGrade(long districtID, string startDate, string endDate, string schoolId, out bool isError, out string message)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string serviceUrl = "api/ReportsAPI/GetMsaAdminProductionSummaryByGrade?clientID=" + districtID + "&startDate=" + startDate + "&endDate=" + endDate + "&schoolId=" + schoolId;

                var response = CallPOSAPI(serviceUrl);

                var ordersData = response.Content.ReadAsAsync<ProductionSummaryDetailed>().Result;
                var students = ToDataTable(ordersData.Students);
                var parents = ToDataTable(ordersData.Parents);
                var schools = ToDataTable(ordersData.Schools);
                var menus = ToDataTable(ordersData.Menus);
                var districts = ToDataTable(ordersData.Districts);
                var categories = ToDataTable(ordersData.Categories);
                var presaleTransactions = ToDataTable(ordersData.PreSaleTransactions);

                //var distTable = GetDistrictById(districtID);
                //districts.Rows[0]["Name"] = distTable.Rows[0]["Name"];

                students.TableName = "Students";
                parents.TableName = "Parents";
                schools.TableName = "Schools";
                menus.TableName = "Menu";
                districts.TableName = "Districts";
                categories.TableName = "Category";
                presaleTransactions.TableName = "PreSaleTransactions";

                dataSet.Tables.Add(students);
                dataSet.Tables.Add(parents);
                dataSet.Tables.Add(schools);
                dataSet.Tables.Add(menus);
                dataSet.Tables.Add(districts);
                dataSet.Tables.Add(categories);
                dataSet.Tables.Add(presaleTransactions);
                isError = false;
                message = string.Empty;

            }
            catch (Exception ex)
            {
                isError = true;
                message = ex.Message;
            }
            return dataSet;
        }

        public static DataSet GetProductionSummaryPerDistrictsByGrade(long districtID, string startDate, string endDate, out bool isError, out string message)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string serviceUrl = "api/ReportsAPI/GetMsaAdminProductionSummaryPerDistrictByGrade?clientID=" + districtID + "&startDate=" + startDate + "&endDate=" + endDate;

                var response = CallPOSAPI(serviceUrl);

                var ordersData = response.Content.ReadAsAsync<ProductionSummaryDetailed>().Result;
                var students = ToDataTable(ordersData.Students);
                var menus = ToDataTable(ordersData.Menus);
                var districts = ToDataTable(ordersData.Districts);
                var categories = ToDataTable(ordersData.Categories);
                var presaleTransactions = ToDataTable(ordersData.PreSaleTransactions);

                //var distTable = GetDistrictById(districtID);
                //districts.Rows[0]["Name"] = distTable.Rows[0]["Name"];

                students.TableName = "Students";
                menus.TableName = "Menu";
                districts.TableName = "Districts";
                categories.TableName = "Category";
                presaleTransactions.TableName = "PreSaleTransactions";

                dataSet.Tables.Add(students);
                dataSet.Tables.Add(menus);
                dataSet.Tables.Add(districts);
                dataSet.Tables.Add(categories);
                dataSet.Tables.Add(presaleTransactions);
                isError = false;
                message = string.Empty;

            }
            catch (Exception ex)
            {
                isError = true;
                message = ex.Message;
            }
            return dataSet;
        }

        public static DataSet GetPreorderDistributionLabels(long districtID, string startDate, string endDate, out bool isError, out string message)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string serviceUrl = "api/ReportsAPI/GetMsaAdminPreorderDistributionLabels?clientID=" + districtID + "&startDate=" + startDate + "&endDate=" + endDate;

                var response = CallPOSAPI(serviceUrl);

                var ordersData = response.Content.ReadAsAsync<ProductionSummaryDetailed>().Result;
                var students = ToDataTable(ordersData.Students);
                var menus = ToDataTable(ordersData.Menus);
                var districts = ToDataTable(ordersData.Districts);
                var schools = ToDataTable(ordersData.Schools);
                var presaleTransactions = ToDataTable(ordersData.PreSaleTransactions);

                //var distTable = GetDistrictById(districtID);
                //districts.Rows[0]["Name"] = distTable.Rows[0]["Name"];

                students.TableName = "Students";
                menus.TableName = "Menu";
                districts.TableName = "Districts";
                schools.TableName = "Schools";
                presaleTransactions.TableName = "PreSaleTransactions";

                dataSet.Tables.Add(students);
                dataSet.Tables.Add(menus);
                dataSet.Tables.Add(districts);
                dataSet.Tables.Add(schools);
                dataSet.Tables.Add(presaleTransactions);
                isError = false;
                message = string.Empty;

            }
            catch (Exception ex)
            {
                isError = true;
                message = ex.Message;
            }
            return dataSet;
        }

        public static DataSet GetPreorderDistributionLabelsBySchools(long districtID, string startDate, string endDate, string schoolsList, out bool isError, out string message)
        {
            DataSet dataSet = new DataSet();
            try
            {
                string serviceUrl = "api/ReportsAPI/GetMsaAdminPreorderDistributionLabelsBySchools?clientID=" + districtID + "&startDate=" + startDate + "&endDate=" + endDate + "&schoolsList=" + schoolsList;

                var response = CallPOSAPI(serviceUrl);

                var ordersData = response.Content.ReadAsAsync<ProductionSummaryDetailed>().Result;
                var students = ToDataTable(ordersData.Students);
                var menus = ToDataTable(ordersData.Menus);
                var districts = ToDataTable(ordersData.Districts);
                var schools = ToDataTable(ordersData.Schools);
                var presaleTransactions = ToDataTable(ordersData.PreSaleTransactions);

                //var distTable = GetDistrictById(districtID);
                //districts.Rows[0]["Name"] = distTable.Rows[0]["Name"];

                students.TableName = "Students";
                menus.TableName = "Menu";
                districts.TableName = "Districts";
                schools.TableName = "Schools";
                presaleTransactions.TableName = "PreSaleTransactions";

                dataSet.Tables.Add(students);
                dataSet.Tables.Add(menus);
                dataSet.Tables.Add(districts);
                dataSet.Tables.Add(schools);
                dataSet.Tables.Add(presaleTransactions);
                isError = false;
                message = string.Empty;

            }
            catch (Exception ex)
            {
                isError = true;
                message = ex.Message;
            }
            return dataSet;
        }

        private static System.Net.Http.HttpResponseMessage CallPOSAPI(string requestUrl)
        {
            using (var client = new System.Net.Http.HttpClient { BaseAddress = new Uri(BaseUrl) })
            {
                client.DefaultRequestHeaders.Clear();

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(requestUrl).Result;

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    throw (new Exception(string.Format("ERROR Calling POS API : {0}", response.ReasonPhrase)));
                }
            }
        }

        public static DataTable ToDataTable<T>(List<T> items) where T : class, new()
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                tb.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (var i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }
    }

    public class ProductionSummaryDetailed
    {
        public List<POSDistrict> Districts { get; set; }
        public List<POSSchool> Schools { get; set; }
        public List<POSStudent> Students { get; set; }
        public List<MSAParent> Parents { get; set; }
        public List<POSMenu> Menus { get; set; }
        public List<POSCategory> Categories { get; set; }
        public List<POSPreSaleTransactions> PreSaleTransactions { get; set; }
    }

    public class POSSchool
    {
        public int Id { get; set; }
        public int District_Id { get; set; }
        public string SchoolID { get; set; }
        public string SchoolName { get; set; }
    }
    public class POSStudent
    {
        public int Id { get; set; }
        public int Parent_Id { get; set; }
        public int District_Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SchoolName { get; set; }
        public string UserId { get; set; }
        public string Homeroom { get; set; }
        public string Grade { get; set; }
        public int School_Id { get; set; }
    }

    public class POSMenu
    {
        public int Id { get; set; }
        public int DistrictID { get; set; }
        public int Category_Id { get; set; }
        public string ItemName { get; set; }
        public decimal StudentFullPrice { get; set; }
    }

    public class POSDistrict
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class POSCategory
    {
        public int Id { get; set; }
        public int Dist_Category_Id { get; set; }
        public int DistrictID { get; set; }
        public string Name { get; set; }
    }

    public class POSPreSaleTransactions
    {
        public int Id { get; set; }
        public int Transaction_Id { get; set; }
        public int Student_Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public DateTime ServingDate { get; set; }
        public DateTime TransferDate { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public bool isComplete { get; set; }
        public int TransType { get; set; }
        public int Parent_Id { get; set; }
        public int Dist_Menu_Id { get; set; }
        public int MSA_Menu_Id { get; set; }
        public int Quantity { get; set; }
        public bool IsVoid { get; set; }
    }

    public class MSAParent
    {
        public int Id { get; set; }
        public int District_Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
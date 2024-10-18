using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace MSA_AdminPortal.Models
{
    public class SqlAzureDbConfiguration : DbConfiguration
    {
        public SqlAzureDbConfiguration()
        {
            //Default Try count : 5, maxDelay : 30s
            //The formula used is: MIN(random(1, 1.1) * (2 ^ retryCount - 1), maxDelay)
            //it retries slower and slower each time, but no slower than 30 seconds (maxDelay).

            SetExecutionStrategy("System.Data.SqlClient", () => new SqlAzureExecutionStrategy());
        }
    }
}
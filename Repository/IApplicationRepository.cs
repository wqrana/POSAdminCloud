using AdminPortalModels.Models;
using AdminPortalModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IApplicationRepository : IDisposable
    {
        IEnumerable<Application> GetApplicationsList(Nullable<long> clientID, int iDisplayStart, int pageSize, int sortColumnIndex, string sortColumn, string sortDirection, ApplicationFilters filters, out int totalrecords);
        IEnumerable<AppStudent> GetAppStudentsList(long clientId, int appId);
    }
}

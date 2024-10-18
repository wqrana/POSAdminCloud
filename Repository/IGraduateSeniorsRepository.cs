using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.ViewModels;
using Repository.edmx;

namespace Repository
{
    public interface IGraduateSeniorsRepository : IDisposable
    {
        IEnumerable<GraduateSeniorsViewModel> GetFilteredSortedGraduateSeniors(long clientId, string searchString, string schoolFilter, string gradeFilter, string districtFilter, string sortBy, string sortDirection);

        IEnumerable<GraduateSeniorsDistinctGradesViewModel> FetchDistinctGradesForGraduateSeniors(long ClientId, long SchoolId, long districtId);

        IEnumerable<GraduateSeniorsDistinctSchoolsViewModel> FetchDistinctSchoolsForGraduateSeniors(long ClientId, long districtId);

        bool GraduateSeniors(long ClientId, int custId);
    }
}

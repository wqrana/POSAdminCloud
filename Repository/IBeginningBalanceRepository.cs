using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.ViewModels;
using Repository.edmx;


namespace Repository
{
    public interface  IBeginningBalanceRepository: IDisposable
    {
        IEnumerable<BeginningBalanceViewModel> GetFilteredSortedBeginningBalance(long clientId, string searchString, string schoolFilter, string gradeFilter,string homeRoomFilter,string districtFilter, string sortBy, string sortDirection);

        IEnumerable<GraduateSeniorsDistinctHomeRoomViewModel> FetchDistinctHomeRoomForBeginningBalance(long ClientId, long SchoolId, long GradeId, long districtId);
    }
}

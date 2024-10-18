using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.Models;
using Repository.edmx;

namespace Repository
{
    public interface ISchoolRepository : IDisposable
    {
        IEnumerable<School> GetSchools();
        School GetSchoolById(long ClientID, int id);
        string DeleteSchool(int SchoolID, long ClientId);
        string SaveSchool(SchoolUpdateModel schoolUpdateModel);
    }
}

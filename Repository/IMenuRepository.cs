using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.edmx;

namespace Repository
{
    public interface IMenuRepository
    {
        IEnumerable<Admin_Categories_List_Result> GetCategoriesList(Nullable<long> clientID, long categoryID, long categoryTypeID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.edmx;

namespace Repository
{
    public class MenuRepository : IMenuRepository, IDisposable
    {
        private PortalContext context;

        public MenuRepository(PortalContext context)
        {
            this.context = context;
        }

        public IEnumerable<Admin_Categories_List_Result> GetCategoriesList(Nullable<long> clientID, long categoryID, long categoryTypeID, int iDisplayStart, int iDisplayLength, int sortColumnIndex, string sortDirection)
        {
            long ClientID = clientID.HasValue ? clientID.Value : 0;
            long CategoryID = categoryID;
            long CategoryTypeID = categoryTypeID;
            /*– Pagination Parameters */
            int PageNo = 1;
            int PageSize = iDisplayLength;

            /*– Sorting Parameters */
            string SortColumn = "";
            string SortOrder = "";

            SortColumn = getColmnName(sortColumnIndex);
            SortOrder = sortDirection == "asc" ? "ASC" : "DESC";
            PageNo = Convert.ToInt16(Math.Ceiling(Convert.ToDouble(iDisplayStart) / Convert.ToDouble(iDisplayLength)) + 1);

            try
            {
                IQueryable<Admin_Categories_List_Result> CategoriesLast = context.Admin_Categories_List(ClientID, CategoryID, CategoryTypeID, PageNo, PageSize, SortColumn, SortOrder);
                return CategoriesLast;
            }
            catch (Exception)
            {

                return null;
            }



        }

        private string getColmnName(int sortColumnIndex)
        {
            int temIndex = sortColumnIndex;
            string retVal = "0";
            switch (temIndex)
            {
                case 0:
                case 1:
                    retVal = "catName";
                    break;
                case 2:
                    retVal = "catTypeName";
                    break;
                case 3:
                    retVal = "ItemCount";
                    break;
                default:
                    retVal = "catName";
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// This function disposes all the memory occupied by this object
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

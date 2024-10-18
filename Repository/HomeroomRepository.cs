using Repository.edmx;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSS;
using System.Data.Entity.Core.Objects;
using Repository.Helpers;

namespace Repository
{
    public class HomeroomRepository : GenericRepository<Homeroom>
    {
        public HomeroomRepository(PortalContext context) : base(context)
        {
        }

        public override void Insert(Homeroom homeroom)
        {
           
            try
            {
                if (homeroom != null)
                {
                    context.Homerooms.Add(homeroom);
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "HomeroomRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "Insert");
                throw;
            }

        }
    }
}

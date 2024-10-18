using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPortalModels.Models
{
    public class TaxesDeleteModel : DeleteModel
    {
        public override string Title { get { return "Tax"; } }
        public override string DeleteUrl { get { return "/Taxes/Delete"; } }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPortalModels.Models;

namespace AdminPortalModels.ViewModels
{
    public class Taxes : ErrorModel
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string Name { get; set; }
        public double TaxRate { get; set; }
        public bool isSelected { get; set; }
    }

    public class SchoolTaxes
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public long SchoolId { get; set; }
        public long TaxId { get; set; }
        public string SchoolName { get; set; }

        public bool isSelected { get; set; }
    }

    public class TaxListViewModel
    {
        public Taxes Tax { get; set; }
        public List<SchoolTaxes> SchoolTax { get; set; }
    }

    public class TaxCreateModel : Taxes
    {
        public override string Title { get { return "Tax: <b>Create New</b>"; } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : "An error occured while creating a new tax."; } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save");
            }
        }
    }

    public class TaxUpdateModel : Taxes
    {
        public override string Title { get { return string.Format("Edit: {0}", Name.Trim()); } }
        public override string ErrorMessage { get { return !string.IsNullOrWhiteSpace(ErrorMessage2) ? ErrorMessage2 : string.Format("An error occured while updating {0} tax.",Name ); } }
        public override string savebtnCaption
        {
            get
            {
                return string.Format("{0}", "Save Changes");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.edmx;
using AdminPortalModels.ViewModels;

namespace Repository
{
    public interface ITaxRepository : IDisposable
    {
        IList<TaxListViewModel> GetTaxes(long clientID);

        Taxes AddTax(Taxes tax);

        bool DeleteTax(long Id);
        Taxes EditTax(Taxes tax);

        List<SchoolTaxes> GetSchoolTaxByClientSchoolID(long clientID,long schoolID);

        List<SchoolTaxes> GetSchoolTaxByClientTaxID(long clientID, long TaxID);

        Taxes GetTaxByTaxId(long TaxId);
        bool GetTaxByIdClientIdAndName(long TaxId,long clientId, string Name);
        bool GetTaxByClientIdAndName(long clientId, string Name);

        bool DeleteTaxFromSchool(long clientID, long schoolID, long TaxId);

        bool DeleteSchoolTaxByTaxId(long id);

        bool AddSchoolToTax(long taxId,long schoolID,long clientID);

        List<Taxes> GetTaxesByClientSchoolID(long clientID, long schoolID);
    }
}

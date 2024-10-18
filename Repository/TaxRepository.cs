using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.edmx;
using Repository.Helpers;
using AdminPortalModels.ViewModels;
using System.Data.Entity;

namespace Repository
{
    public class TaxRepository : ITaxRepository, IDisposable
    {
        private PortalContext context;

        public TaxRepository(PortalContext context)
        {
            this.context = context;
        }
        public IList<TaxListViewModel> GetTaxes(long clientID)
        {
            List<TaxListViewModel> olstTaxListViewModel = new List<TaxListViewModel>();
            try
            {

                IQueryable<TaxEntities1> olstTaxEntities = context.TaxEntities1Set.Where(x => x.ClientID == clientID && x.IsDeleted==false);
                List<School> olstSchool = context.Schools.Where(x => x.ClientID == clientID && x.isDeleted == false).ToList();

                foreach (TaxEntities1 taxEntities in olstTaxEntities)
                {
                    TaxListViewModel oTaxListViewModel = new TaxListViewModel();
                    Taxes t = new Taxes();

                    t.Id = taxEntities.ID;
                    t.ClientId = taxEntities.ClientID;
                    t.TaxRate = taxEntities.TaxRate == null ? 0 : taxEntities.TaxRate.Value;
                    t.Name = taxEntities.TaxName;

                    IEnumerable<School_Tax1> olstSchool_Tax = context.School_Tax1Set.Where(x => x.ClientID == clientID && x.TaxEntity_Id == t.Id);
                  
                    List<SchoolTaxes> olstSchoolTaxes = new List<SchoolTaxes>();

                    foreach (School school in olstSchool)
                    {
                        SchoolTaxes st = new SchoolTaxes();

                        var school_Tax = olstSchool_Tax.FirstOrDefault(x => x.School_Id == school.ID && x.TaxEntity_Id == t.Id);
                        if (school_Tax != null)
                        {
                            st.ClientId = school_Tax.ClientID;
                            st.SchoolId = school_Tax.School_Id;
                            st.TaxId = school_Tax.TaxEntity_Id;
                            st.Id = school_Tax.ID;
                            st.SchoolName = school.SchoolName;
                            st.isSelected = true;
                        }
                        else
                        {
                            st.ClientId = school.ClientID;
                            st.SchoolId = school.ID;
                            st.TaxId = 0;
                            st.Id = 0;
                            st.SchoolName = school.SchoolName;
                            st.isSelected = false;
                        }

                        olstSchoolTaxes.Add(st);

                    }

                    oTaxListViewModel.Tax = t;
                    oTaxListViewModel.SchoolTax = olstSchoolTaxes;
                    olstTaxListViewModel.Add(oTaxListViewModel);
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetTaxes");
                return new List<TaxListViewModel>();
            }

            return olstTaxListViewModel;
        }

        public Taxes AddTax(Taxes tax)
        {
            TaxEntities1 oTaxEntities = new TaxEntities1();
            try
            {
                oTaxEntities.TaxName = tax.Name;
                oTaxEntities.ClientID = tax.ClientId;
                oTaxEntities.TaxRate = tax.TaxRate;
                oTaxEntities.LastUpdatedUTC = DateTime.UtcNow;

                context.TaxEntities1Set.Add(oTaxEntities);

                context.SaveChanges();

                tax.Id = oTaxEntities.ID;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AddTax");
                return null;
            }
            return tax;
        }

        public bool GetTaxByClientIdAndName(long clientID, string Name)
        {
            
            try
            {
                TaxEntities1 oTaxEntities = context.TaxEntities1Set.FirstOrDefault(x => x.ClientID == clientID && x.TaxName.ToLower() == Name.ToLower());    

                if (oTaxEntities != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetTaxByClientIdAndName");
                return false;
            }
            //return oTaxEntities;
        }

        public bool GetTaxByIdClientIdAndName(long TaxId, long clientId, string Name)
        {
            TaxEntities1 oTaxEntities = context.TaxEntities1Set.FirstOrDefault(x => x.ClientID == clientId && x.ID == TaxId && x.TaxName.ToLower() == Name.ToLower());
            try
            {
                if (oTaxEntities != null)
                {
                    return false;
                }
                else
                {
                    return GetTaxByClientIdAndName(clientId, Name);
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetTaxByClientIdAndName");
                return false;
            }
        }

        public bool DeleteTax(long Id)
        {
            
            try
            {
                TaxEntities1 oTaxEntities = context.TaxEntities1Set.FirstOrDefault(x => x.ID == Id);

                if (oTaxEntities != null)
                {
                    //oTaxEntities.TaxName = tax.Name;
                    //oTaxEntities.TaxRate = tax.TaxRate;

                    oTaxEntities.IsDeleted = true;
                    oTaxEntities.LastUpdatedUTC = DateTime.UtcNow;


                    context.Entry(oTaxEntities).State = EntityState.Modified;

                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Record Not Found");
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteTax");
                return false;
            }
            return true;
        }
        public Taxes EditTax(Taxes tax)
        {
            //Taxes tax = new Taxes();
            try
            {
                TaxEntities1 oTaxEntities = context.TaxEntities1Set.FirstOrDefault(x => x.ID == tax.Id);

                if (oTaxEntities != null)
                {
                    oTaxEntities.TaxName = tax.Name;
                    oTaxEntities.TaxRate = tax.TaxRate;
                    oTaxEntities.LastUpdatedUTC = DateTime.UtcNow;
                    

                    context.Entry(oTaxEntities).State = EntityState.Modified;

                    context.SaveChanges();

                    tax.Id = oTaxEntities.ID;
                    tax.Name = oTaxEntities.TaxName;
                    tax.TaxRate = oTaxEntities.TaxRate == null ? 0 : oTaxEntities.TaxRate.Value; 
                    tax.ClientId = oTaxEntities.ClientID;
                }
                else
                {
                    throw new Exception("Record Not Found");
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "EditTax");
                return null;
            }
            return tax;
        }

        public List<SchoolTaxes> GetSchoolTaxByClientSchoolID(long clientID, long schoolID)
        {
            try
            {
                List<SchoolTaxes> olstSchoolTaxes= context.School_Tax1Set.Where(x => x.ClientID == clientID && x.School_Id == schoolID).Select(p1 => new SchoolTaxes { Id = p1.ID, ClientId = p1.ClientID, SchoolId = p1.School_Id, TaxId = p1.TaxEntity_Id }).ToList<SchoolTaxes>();

                if (olstSchoolTaxes != null && olstSchoolTaxes.Count > 0)
                {
                    return olstSchoolTaxes;
                }
                else
                {
                    return new List<SchoolTaxes>();
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchoolTaxByClientSchoolID");
                return new List<SchoolTaxes>();
            }
        }

        public List<SchoolTaxes> GetSchoolTaxByClientTaxID(long clientID, long TaxID)
        {
            try
            {
                List<SchoolTaxes> olstSchoolTaxes = context.School_Tax1Set.Where(x => x.ClientID == clientID && x.TaxEntity_Id == TaxID).Select(p1 => new SchoolTaxes { Id = p1.ID, ClientId = p1.ClientID, SchoolId = p1.School_Id, TaxId = p1.TaxEntity_Id }).ToList<SchoolTaxes>();

                if (olstSchoolTaxes != null && olstSchoolTaxes.Count > 0)
                {
                    return olstSchoolTaxes;
                }
                else
                {
                    return new List<SchoolTaxes>();
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetSchoolTaxByClientTaxID");
                return new List<SchoolTaxes>();
            }
        }

        public Taxes GetTaxByTaxId(long TaxId)
        {
            var tax= context.TaxEntities1Set.FirstOrDefault(x => x.ID == TaxId);
            var taxes = new Taxes();
            try
            {
                if (tax != null)
                {
                    taxes.Id = tax.ID;
                    taxes.ClientId = tax.ClientID;
                    taxes.Name = tax.TaxName;
                    taxes.TaxRate = tax.TaxRate == null ? 0 : tax.TaxRate.Value;

                    return taxes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetTaxByTaxId");
                return null;
            }

        }

        public bool DeleteTaxFromSchool(long clientID, long schoolID, long TaxId)
        {
            try
            {
                School_Tax1 oSchool_Tax = context.School_Tax1Set.FirstOrDefault(x => x.ClientID == clientID && x.School_Id == schoolID && x.TaxEntity_Id == TaxId);

                if (oSchool_Tax != null)
                {
                    context.Entry(oSchool_Tax).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Record Not Found");

                }

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteTaxFromSchool");
                return false;
            }
            return true;
        }

        public bool DeleteSchoolTaxByTaxId(long id)
        {
            try
            {
                School_Tax1 oSchool_Tax = context.School_Tax1Set.FirstOrDefault(x => x.ID == id);

                if (oSchool_Tax != null)
                {
                    context.Entry(oSchool_Tax).State = EntityState.Deleted;
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Record Not Found");

                }

            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "DeleteSchoolTaxByTaxId");
                return false;
            }
            return true;
        }

        public bool AddSchoolToTax(long taxId, long schoolID, long clientID)
        {
            School_Tax1 oSchool_Tax1 = new School_Tax1();
            try
            {
                oSchool_Tax1.TaxEntity_Id = taxId;
                oSchool_Tax1.ClientID = clientID;
                oSchool_Tax1.School_Id = schoolID;
                oSchool_Tax1.LastUpdatedUTC = DateTime.UtcNow;

                context.School_Tax1Set.Add(oSchool_Tax1);

                context.SaveChanges();

                //tax.Id = oSchool_Tax1.ID;
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "AddSchoolToTax");
                return false;
            }
            return true;
        }

        public List<Taxes> GetTaxesByClientSchoolID(long clientID, long schoolID)
        {
            try
            {
                List<SchoolTaxes> olstSchoolTaxes = context.School_Tax1Set.Where(x => x.ClientID == clientID && x.School_Id == schoolID)
                    .Select(p1 => new SchoolTaxes { Id = p1.ID, ClientId = p1.ClientID, SchoolId = p1.School_Id, TaxId = p1.TaxEntity_Id }).ToList<SchoolTaxes>();

                List<TaxEntities1> olstTaxEntities = context.TaxEntities1Set.Where(x => x.ClientID == clientID && x.IsDeleted==false).ToList();

                List<Taxes> olstTaxesReturn = new List<Taxes>();

                if (olstTaxEntities != null && olstTaxEntities.Count > 0)
                {
                    foreach (var tax in olstTaxEntities)
                    {
                        Taxes oTaxes = new Taxes();

                        var oTaxAppliedOnSchool = olstSchoolTaxes.FirstOrDefault(x => x.TaxId == tax.ID);
                        if (oTaxAppliedOnSchool != null)
                        {
                            oTaxes.ClientId = tax.ClientID;
                            oTaxes.Id = tax.ID;
                            oTaxes.Name = tax.TaxName;
                            oTaxes.TaxRate = tax.TaxRate != null ? tax.TaxRate.Value : 0;
                            oTaxes.isSelected = true;
                        }
                        else
                        {
                            oTaxes.ClientId = tax.ClientID;
                            oTaxes.Id = tax.ID;
                            oTaxes.Name = tax.TaxName;
                            oTaxes.TaxRate = tax.TaxRate != null ? tax.TaxRate.Value : 0;
                            oTaxes.isSelected = false;
                        }
                        olstTaxesReturn.Add(oTaxes);
                    }
                    return olstTaxesReturn;
                }
                else
                {
                    return new List<Taxes>();
                }
            }
            catch (Exception ex)
            {
                ErrorLogHelper.InsertLog(Constants.ERROR, TimeZoneSettings.Instance.GetLocalTime(), "TaxRepository", "Error : " + ex.Message, CommonClasses.getCustomerID(), "GetTaxesByClientSchoolID");
                return new List<Taxes>();
            }
        }

        #region IDispose
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
        #endregion
    }
}

using System;
using System.Linq;
using FSS;
using Repository.edmx;

namespace Repository
{
    public class UnitOfWork : IDisposable
    {
        private long _ClientID;
        private PortalContext context;
        private GenericRepository<District> districtRepository;
        private GenericRepository<CategoryType> categoryTypeRepository;
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Menu> menuRepository;
        private GenericRepository<Item> itemRepository;
        private HomeroomRepository homeroomRepository;
        private GenericRepository<School> schoolRepository;
        private GenericRepository<Customer> customerRepository;
        private GenericRepository<SchoolOption> schoolOptionRepository;
        private GenericRepository<Grade> gradeRepository;
        private GenericRepository<POS> posRepository;
        private GenericRepository<DistrictOption> districtOptionRepository;
        private GenericRepository<SchoolOption> scholOptionRepository;

        private GenericRepository<SystemOptions> systemOptionRepository;
        private IPOSNotificationsRepository _posNotificationsRepository;
      
        
        private GenericRepository<Customer_School> customer_school;

        private IQueryable<Admin_Menu_List_Result> menuSPResultRepository;


        private ICustomerRepository _customerRepository;
        private IGeneralRepository _generalRepository;
        private IReportsRepository _reportsRepository;
        private IOrderManagement _orderRepository;
        private IMenuRepository _menuRepository;
        private IBeginningBalanceRepository _beginningBalanceRepository;
        private IGraduateSeniorsRepository _graduateSeniorsRepository;

        private ISchoolRepository _customSchoolRepository;
        private ISecurityRepository _securityRepository;
        private ISettingsRepository _settingsRepository;

        
        private IDashboardRepository _dashboardRepository;

        private IPreOrderPickupRespository _customPreOrderPickupRespository; 

        private ITaxRepository _taxRepository;

        private ApplicationRepository _applicationRepository;
        private GenericRepository<App_Members> _appMemberRepository;
        private GenericRepository<App_Member_Incomes> _appMemberIncomesRepository;
        private GenericRepository<App_Member_Statuses> _appMemberStatusesRepository;
        private GenericRepository<Member> _membersRepository;
        private GenericRepository<App_Statuses> _appStatusesRepository;
        private GenericRepository<App_Ethnicity> _ethnicityRepository;
        private GenericRepository<App_Races> _racesRepository;
        private GenericRepository<App_Signers> _appSignerRepository;
        private GenericRepository<App_Notes> _appNotesRepository;

        //public UnitOfWork()
        //{
        //    context = new PortalContext();
        //}

        public UnitOfWork(string conStr)
        {
            var connectionString = ConvertToEFConnectionString(conStr);

            context = new PortalContext(connectionString);
        }

        public UnitOfWork(string conStr, long ClientID)
        {
            _ClientID = ClientID;
            var connectionString = ConvertToEFConnectionString(conStr);

            context = new PortalContext(connectionString);
        }

        private string ConvertToEFConnectionString(string conStr)
        {
            // Specify the provider name, server and database.
            var providerName = "System.Data.SqlClient";

            //var providerName = "System.Data.EntityClient";

            // Initialize the EntityConnectionStringBuilder.
            var entityBuilder = new System.Data.EntityClient.EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = conStr;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/edmx.fss_adminportal.csdl|res://*/edmx.fss_adminportal.ssdl|res://*/edmx.fss_adminportal.msl";

            return entityBuilder.ToString();
        }



        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (customerRepository == null)
                {
                    customerRepository = new GenericRepository<Customer>(context);
                }
                return customerRepository;
            }
        }

        public GenericRepository<District> DistrictRepository
        {
            get
            {
                if (districtRepository == null)
                {
                    districtRepository = new GenericRepository<District>(context);
                }
                return districtRepository;
            }
        }

        public GenericRepository<CategoryType> CategoryTypeRepository
        {
            get
            {
                if (categoryTypeRepository == null)
                {
                    categoryTypeRepository = new GenericRepository<CategoryType>(context);
                }
                return categoryTypeRepository;
            }
        }

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if (categoryRepository == null)
                {
                    categoryRepository = new GenericRepository<Category>(context);
                }
                return categoryRepository;
            }
        }

        public GenericRepository<Menu> MenuRepository
        {
            get
            {
                if (menuRepository == null)
                {
                    menuRepository = new GenericRepository<Menu>(context);
                }
                return menuRepository;
            }
        }

        public IQueryable<Admin_Menu_List_Result> MenuSPRepository
        {
            get
            {
                if (menuSPResultRepository == null)
                {
                    menuSPResultRepository = context.Admin_Menu_List(_ClientID);
                }
                return menuSPResultRepository;
            }
        }


        public GenericRepository<Item> ItemRepository
        {
            get
            {
                if (itemRepository == null)
                {
                    itemRepository = new GenericRepository<Item>(context);
                }
                return itemRepository;
            }
        }

        public GenericRepository<DistrictOption> DistrictOptionRepository
        {
            get
            {
                if (districtOptionRepository == null)
                {
                    districtOptionRepository = new GenericRepository<DistrictOption>(context);
                }
                return districtOptionRepository;
            }
        }

        public GenericRepository<SchoolOption> ScholOptionRepository
        {
            get
            {
                if (scholOptionRepository == null)
                {
                    scholOptionRepository = new GenericRepository<SchoolOption>(context);
                }
                return scholOptionRepository;
            }
        }

        public GenericRepository<SystemOptions> SystemOptionRepository
        {
            get
            {
                if (systemOptionRepository == null)
                {
                    systemOptionRepository = new GenericRepository<SystemOptions>(context);
                }
                return systemOptionRepository;
            }
        }

        public HomeroomRepository HomeroomRepository
        {
            get
            {
                if (this.homeroomRepository == null)
                {
                    this.homeroomRepository = new HomeroomRepository(context);
                }
                return homeroomRepository;
            }
        }

        public GenericRepository<Grade> GradeRepository
        {
            get
            {
                if (this.gradeRepository == null)
                {
                    this.gradeRepository = new GenericRepository<Grade>(context);
                }
                return gradeRepository;
            }
        }


        public GenericRepository<POS> POSRepository
        {
            get
            {
                if (this.posRepository == null)
                {
                    this.posRepository = new GenericRepository<POS>(context);
                }
                return posRepository;
            }
        }

        public GenericRepository<School> SchoolRepository
        {
            get
            {
                if (this.schoolRepository == null)
                {
                    this.schoolRepository = new GenericRepository<School>(context);
                }
                return schoolRepository;
            }
        }

        public IGeneralRepository generalRepository
        {
            get
            {
                if (this._generalRepository == null)
                {
                    this._generalRepository = new GeneralRepository(context);
                }
                return this._generalRepository;
            }
        }

        public IPreOrderPickupRespository customPreOrderPickupRespository
        {
            get
            {
                if (this._customPreOrderPickupRespository == null)
                {
                    this._customPreOrderPickupRespository = new PreOrderPickupRespository(context);

                }
                return this._customPreOrderPickupRespository;
            }
        }

        public ICustomerRepository CustomCustomerRepository
        {
            get
            {
                if (this._customerRepository == null)
                {
                    this._customerRepository = new CustomerRepository(context);
                }
                return this._customerRepository;

            }
        }

        public IDashboardRepository dashboardRepository
        {
            get
            {
                if (this._dashboardRepository == null)
                {
                    this._dashboardRepository = new DashboardRepository(context);
                }
                return this._dashboardRepository;
            }
        }


        public IReportsRepository reportsRepository
        {
            get
            {
                if (this._reportsRepository == null)
                {
                    this._reportsRepository = new ReportsRepository(context);
                }
                return this._reportsRepository;
            }
        }

        public IOrderManagement orderRepository
        {
            get
            {
                if (this._orderRepository == null)
                {
                    this._orderRepository = new OrderManagement(context);
                }
                return this._orderRepository;
            }

        }

        public GenericRepository<SchoolOption> SchoolOptionRepository
        {
            get
            {
                if (this.schoolOptionRepository == null)
                {
                    this.schoolOptionRepository = new GenericRepository<SchoolOption>(context);
                }
                return schoolOptionRepository;
            }
        }

        public GenericRepository<Customer_School> CustomersSchoolsRepository
        {
            get
            {
                if (this.customer_school == null)
                {
                    this.customer_school = new GenericRepository<Customer_School>(context);
                }
                return customer_school;
            }
        }
        public ISchoolRepository customSchoolRepository
        {
            get
            {
                if (this._customSchoolRepository == null)
                {
                    this._customSchoolRepository = new SchoolRepository(context);
                }
                return this._customSchoolRepository;

            }
        }

        public ISecurityRepository securityRepository
        {
            get
            {
                if (this._securityRepository == null)
                {
                    this._securityRepository = new SecurityRepository(context);
                }
                return this._securityRepository;
            }
        }

        public ISettingsRepository settingsRepository
        {
            get
            {
                if (this._settingsRepository == null)
                {
                    this._settingsRepository = new SettingsRepository(context);
                }
                return this._settingsRepository;
            }

        }

        public IMenuRepository menuFunctionsRepository
        {
            get
            {
                if (this._menuRepository == null)
                {
                    this._menuRepository = new MenuRepository(context);
                
                }
                return this._menuRepository;
            
            }
        
        }

        public ITaxRepository taxRepository
        {
            get
            {
                if (this._taxRepository == null)
                {
                    this._taxRepository = new TaxRepository(context);

                }
                return this._taxRepository;
        
        }

        }

        public IPOSNotificationsRepository posNotificationsRepository
        {
            get
            {
                if (this._posNotificationsRepository == null)
                {
                    this._posNotificationsRepository = new POSNotificationsRepository(context);

                }
                return this._posNotificationsRepository;

            }
        }
        public ApplicationRepository ApplicationRepository
        {
            get
            {
                if(this._applicationRepository == null)
                {
                    this._applicationRepository = new ApplicationRepository(context);
                }
                return this._applicationRepository;
            }
        }

        public GenericRepository<App_Members> App_MembersRepository
        {
            get
            {
                if(this._appMemberRepository == null)
                {
                    this._appMemberRepository = new GenericRepository<App_Members>(context);
                }
                return this._appMemberRepository;
            }
        }

        public GenericRepository<App_Member_Incomes> App_Member_IncomesRepository
        {
            get
            {
                if(this._appMemberIncomesRepository == null)
                {
                    this._appMemberIncomesRepository = new GenericRepository<App_Member_Incomes>(context);
                }
                return _appMemberIncomesRepository;
            }
        }

        public GenericRepository<App_Member_Statuses> App_Member_StatusesRepository
        {
            get
            {
                if(this._appMemberStatusesRepository == null)
                {
                    this._appMemberStatusesRepository = new GenericRepository<App_Member_Statuses>(context);
                }
                return _appMemberStatusesRepository;
            }
        }

        public GenericRepository<Member> MembersRepository
        {
            get
            {
                if(this._membersRepository == null)
                {
                    this._membersRepository = new GenericRepository<Member>(context);
                }
                return _membersRepository;
            }
        }

        public GenericRepository<App_Statuses> App_Statuses_Repository
        {
            get
            {
                if(this._appStatusesRepository == null)
                {
                    this._appStatusesRepository = new GenericRepository<App_Statuses>(context);
                }
                return _appStatusesRepository;
            }
        }

        public GenericRepository<App_Ethnicity> EtnicityRepository
        {
            get
            {
                if (_ethnicityRepository == null)
                {
                    _ethnicityRepository = new GenericRepository<App_Ethnicity>(context);
            }
                return _ethnicityRepository;
            }
        }
        
        public GenericRepository<App_Races> RacesRepository
        {
            get
            {
                if (_racesRepository == null)
                {
                    _racesRepository = new GenericRepository<App_Races>(context);

                } 
                return _racesRepository;
            }
        }

        public IBeginningBalanceRepository beginningBalanceRepository
        {
            get
            {
                if (this._beginningBalanceRepository == null)
                {
                    this._beginningBalanceRepository = new BeginningBalanceRepository(context);

                }
                return this._beginningBalanceRepository;

            }

        }


        public IGraduateSeniorsRepository GraduateSeniorsRepository
        {
            get
            {
                if (this._graduateSeniorsRepository == null)
                {
                    this._graduateSeniorsRepository = new GraduateSeniorsRepository(context);
                }
                return this._graduateSeniorsRepository;

            }
        }

        public GenericRepository<App_Signers> App_Signer_Repository
        {
            get
            {
                if(_appSignerRepository == null)
                {
                    _appSignerRepository = new GenericRepository<App_Signers>(context);
                }
                return _appSignerRepository;
            }
        }

        public GenericRepository<App_Notes> App_Notes_Repository
        {
            get
            {
                if (_appNotesRepository == null)
                {
                    _appNotesRepository = new GenericRepository<App_Notes>(context);
                }
                return _appNotesRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }


        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

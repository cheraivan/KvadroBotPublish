using KopterBot.Repository;

namespace KopterBot.Services
{
    class ServiceProvider :RepositoryProvider
    {
        private BuisnessTaskService _buisnessTaskService;
        private UserService _userService;
        private AdminService _adminService;
        private HubService _hubService;
        private DronService _dronService;
        private StepService _stepService;
        private StorageService _storageService;
        private ProposalService _proposalService;
        private ShowOrderService _showOrderService;
        private OfferService _offerService;

        public OfferService offerService
        {
            get
            {
                if (_offerService == null)
                    _offerService = new OfferService();
                return _offerService;
            }
        }

        public ShowOrderService showOrderService
        {
            get
            {
                if (_showOrderService == null)
                    _showOrderService = new ShowOrderService();
                return _showOrderService;
            }
        }

        public ProposalService proposalService
        {
            get
            {
                if (_proposalService == null)
                    _proposalService = new ProposalService();
                return _proposalService;
            }
        }

        public BuisnessTaskService buisnessTaskService
        {
            get
            {
                if (_buisnessTaskService == null)
                    _buisnessTaskService = new BuisnessTaskService();
                return _buisnessTaskService;
            }
        }

        public DronService dronService
        {
            get
            {
                if (_dronService == null)
                    _dronService = new DronService();
                return _dronService;
            }
        }
        public StepService stepService
        {
            get
            {
                if (_stepService == null)
                    _stepService = new StepService();
                return _stepService;
            }
        }
        public StorageService storageService
        {
            get
            {
                if (_storageService == null)
                    _storageService = new StorageService();
                return _storageService;
            }
        }

        public UserService userService
        {
            get
            {
                if (_userService == null)
                    _userService = new UserService();
                return _userService;
            }
        }
        public AdminService adminService
        {
            get
            {
                if (_adminService == null)
                    _adminService = new AdminService();
                return _adminService;
            }
        }
        public HubService hubService
        {
            get
            {
                if (_hubService == null)
                    _hubService = new HubService();
                return _hubService;
            }
        }
    }
}

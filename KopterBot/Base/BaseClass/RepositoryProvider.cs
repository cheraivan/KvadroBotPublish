using KopterBot.Services;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class RepositoryProvider
    {
        private UserRepository _userRepository;
        private DronRepository _dronRepository;
        private HubRepository _hubRepository;
        private AdminRepository _adminRepository;
        private ProposalRepository _proposalRepository;
        private StorageRepository _storageRepository;
        private CountProposeRepository _countProposeRepository;
        private ManagerRepository _managerRepository;
        private BuisnessTaskRepository _buisnessTaskRepository;
        private ShowOrdersRepository _showOrdersRepository;
        private OfferRepository _offerRepository;
        private sosTableRepository _sosTableRepository;
        private ShowUserRepository _showUserRepository;

        protected ShowUserRepository showUserRepository
        {
            get
            {
                if (_showUserRepository == null)
                    _showUserRepository = new ShowUserRepository();
                return _showUserRepository;
            }
        }

        protected sosTableRepository sosTableRepository
        {
            get
            {
                if (_sosTableRepository == null)
                    _sosTableRepository = new sosTableRepository();
                return _sosTableRepository;
            }
        }

        protected OfferRepository offerRepository
        {
            get
            {
                if (_offerRepository == null)
                    _offerRepository = new OfferRepository();
                return _offerRepository;
            }
        }

        protected ShowOrdersRepository showOrdersRepository
        {
            get
            {
                if (_showOrdersRepository == null)
                    _showOrdersRepository = new ShowOrdersRepository();
                return _showOrdersRepository;
            }
        }

        protected BuisnessTaskRepository buisnessTaskRepository
        {
            get
            {
                if (_buisnessTaskRepository == null)
                    _buisnessTaskRepository = new BuisnessTaskRepository();
                return _buisnessTaskRepository;
            }
        }
        protected ManagerRepository managerRepository
        {
            get
            {
                if (_managerRepository == null)
                    _managerRepository = new ManagerRepository();
                return _managerRepository;
            }
        }

        protected CountProposeRepository countProposeRepository
        {
            get
            {
                if (_countProposeRepository == null)
                    _countProposeRepository = new CountProposeRepository();
                return _countProposeRepository;
            }
        }

        protected UserRepository userRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository();
                return _userRepository;
            }
        }
        protected DronRepository dronRepository
        {
            get
            {
                if (_dronRepository == null)
                    _dronRepository = new DronRepository();
                return _dronRepository;
            }
        }
        protected HubRepository hubRepository
        {
            get
            {
                if (_hubRepository == null)
                    _hubRepository = new HubRepository();
                return _hubRepository;
            }
        }
        protected AdminRepository adminRepository
        {
            get
            {
                if (_adminRepository == null)
                    _adminRepository = new AdminRepository();
                return _adminRepository;
            }
        }
        protected ProposalRepository proposalRepository
        {
            get
            {
                if (_proposalRepository == null)
                    _proposalRepository = new ProposalRepository();
                return _proposalRepository;
            }
        }

        protected StorageRepository storageRepository
        {
            get
            {
                if (_storageRepository == null)
                    _storageRepository = new StorageRepository();
                return _storageRepository;
            }
        }
    }
}

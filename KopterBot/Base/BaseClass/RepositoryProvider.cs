﻿namespace KopterBot.Repository
{
    class RepositoryProvider
    {
        private UserRepository _userRepository;
        private DronRepository _dronRepository;
        private HubRepository _hubRepository;
        private AdminRepository _adminRepository;
        private ProposalRepository _proposalRepository;
        private StorageRepository _storageRepository;

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

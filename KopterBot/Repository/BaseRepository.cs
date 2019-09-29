using KopterBot.DTO;
using KopterBot.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Repository
{
    class BaseRepository
    {
        protected ApplicationContext db;
        public BaseRepository()
        {
            db = new ApplicationContext();
        }
    }
    class RepositoryProvider:BaseRepository
    {
        protected UserRepository userRepository;
        protected DronRepository dronRepository;
        protected BotRepository botRepository;
        protected HubRepository hubRepository;
        protected AdminRepository adminRepository;
        protected ProposalRepository proposalRepository;


        public RepositoryProvider()
        {
            userRepository = new UserRepository();
            dronRepository = new DronRepository();
            botRepository = new BotRepository();
            adminRepository = new AdminRepository();
            proposalRepository = new ProposalRepository();
            hubRepository = new HubRepository();
        }
    }
}

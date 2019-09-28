using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class BotRepository
    {
        private ApplicationContext context;
        GenericRepository<UserDTO> genericRepository;
        public BotRepository(ApplicationContext context)
        {
            this.context = context;
            genericRepository = new GenericRepository<UserDTO>(context);
        }
      
    }
}

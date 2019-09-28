using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class BotRepository:BaseRepository
    {
        GenericRepository<UserDTO> genericRepository;
        public BotRepository()
        {
            genericRepository = new GenericRepository<UserDTO>(db);
        }
      
    }
}

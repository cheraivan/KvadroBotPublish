using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Repository
{
    class ManagerRepository:BaseProviderImpementation<ManagerDTO>
    {
        public async ValueTask<int> CountManager() =>
            await db.Managers.CountAsync();

        public async ValueTask<List<long>> ManagerId()
        {
            return await db.Managers.Select(i => i.ChatId).ToListAsync();
        } 
    }
}

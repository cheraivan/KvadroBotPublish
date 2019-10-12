using KopterBot.Base.BaseClass;
using KopterBot.Bot;
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
    class AdminRepository: BaseProviderImpementation<AdminDTO>
    {
        
        public AdminRepository() { }

        #region Public_Methods

        public async override ValueTask<AdminDTO> FindById(long id)
        {
            return await db.Admins.FirstOrDefaultAsync(i => i.ChatId == id);
        }

        #endregion
    }
}

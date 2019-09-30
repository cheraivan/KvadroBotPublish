using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Repository
{
    class AdminRepository: BaseProviderImpementation<AdminDTO>
    {
        
        public AdminRepository() { }

        #region Public_Methods

        public async ValueTask<bool> IsAdmin(long chatid)
        {
            AdminDTO admin = await FindById(chatid);
            return admin == null ? false
                : admin.Wish == 1 ? false : true;
        }

        public async Task ChangeWish(long chatid)
        {
            AdminDTO admin = await FindById(chatid);
            int wish = admin.Wish;
            wish = wish == 0 ? 1 : 0;
            await Update(admin);
        }
        #endregion
    }
}

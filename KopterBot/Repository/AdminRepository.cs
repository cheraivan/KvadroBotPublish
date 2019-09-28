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
    class AdminRepository:BaseRepository
    {
        
        public AdminRepository() { }

        #region Public_Methods

        public async ValueTask<bool> IsAdmin(long chatid)
        {
            AdminDTO admin = await db.Admins.FirstOrDefaultAsync(i => i.ChatId == chatid);

            // протестировать админ-вход

            return admin == null ? false
                : admin.Wish == 1 ? false : true;
        }

        public async Task ChangeWish(long chatid)
        {
            AdminDTO admin = await db.Admins.FirstOrDefaultAsync(i => i.ChatId == chatid);
            int wish = admin.Wish;
            wish = wish == 0 ? 1 : 0;
            db.Entry(admin).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
        #endregion
    }
}

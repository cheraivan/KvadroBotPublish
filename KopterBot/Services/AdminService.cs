using KopterBot.DTO;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class AdminService : RepositoryProvider
    {

        public async ValueTask<int> CountAdmins() =>
            await adminRepository.Get().CountAsync();

        public async ValueTask<List<long>> GetChatId() =>
            await adminRepository.Get().Select(i => i.ChatId).ToListAsync();
        public async ValueTask<bool> IsAdmin(long chatid)
        {
            AdminDTO admin = await adminRepository.FindById(chatid);
            return admin == null ? false
                : admin.Wish == 1 ? false : true;
        }

        public async Task ChangeWish(long chatid)
        {
            AdminDTO admin = await adminRepository.FindById(chatid);
            int wish = admin.Wish;
            wish = wish == 0 ? 1 : 0;
            await adminRepository.Update(admin);
        }
    }
}

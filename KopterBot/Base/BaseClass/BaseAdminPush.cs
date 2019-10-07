using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Base.BaseClass
{
    class BaseAdminPush:BaseCommand
    {
        public BaseAdminPush(TelegramBotClient client,MainProvider provider) : base(client, provider) { }
        public virtual async Task BaseAdminMessage()
        {
            int countAdmin = await provider.adminService.CountAdmins();
            if (countAdmin == 0)
                return;
            List<long> admins = await provider.adminService.GetChatId();
        }
    }
}

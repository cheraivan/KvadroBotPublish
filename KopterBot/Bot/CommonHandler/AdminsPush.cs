using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Bot.CommonHandler
{
    class AdminsPush:BaseHandler
    {
        public AdminsPush(TelegramBotClient client,ApplicationContext db) : base(client, db) { }

        public async Task MessageRequisition(long chatid)
        {
            int countAdmin = await db.Admins.CountAsync();
            if (countAdmin == 0)
                return;
            long[] chatids = new long[countAdmin];

        }
    }
}

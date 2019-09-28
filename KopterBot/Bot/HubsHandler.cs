using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Bot
{
    class HubsHandler
    {
        private static ApplicationContext db;
        static HubsHandler()
        {
            db = new ApplicationContext();
        }
        public async static ValueTask<long> GetReceviedChatId(long chatid)
        {
            HubDTO hub = await db.Hubs.FindAsync(chatid);
            return hub.ChatIdReceiver;
        }
        public async static ValueTask<long[]> GetChatId(long chatid)
        {
            HubDTO hub1 = await db.Hubs.FindAsync(chatid);
            long[] res = new long[2];
            res[0] = hub1.ChatIdCreater;
            res[1] = hub1.ChatIdReceiver;
            return res;
        }
        public async static ValueTask<bool> IsChatActive(long chatid)
        {
            HubDTO hub1 = await db.Hubs.FindAsync(chatid);

            if(hub1 == null || hub1.ChatIdReceiver == 0)
            {
                return false;
            }

            HubDTO hub2 = await db.Hubs.Where(i => i.ChatIdCreater == hub1.ChatIdReceiver).FirstOrDefaultAsync();
            return hub2 == null ? false : true;
        }
    }
}

using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class HubRepository: BaseProviderImpementation<HubDTO>
    {
        
        static HubRepository()
        {

        }

        public async Task ConfirmDialog(string confirm,long CreaterChatId, long ReceiverChatId)
        {
            if (confirm == "Начать")
            {
                HubDTO reletedHub = new HubDTO(ReceiverChatId, CreaterChatId);
                await Create(reletedHub);
                return;
            }
            else
            {
                HubDTO hub = await db.Hubs.FindAsync(CreaterChatId);
                await Delete(hub);
            }
        }

        public async Task CreateDialog(long CreaterChatId, long ReceiverChatId)
        {
            HubDTO hub = await db.Hubs.FindAsync(CreaterChatId);
            if (hub == null)
            {
                hub = new HubDTO(CreaterChatId, ReceiverChatId);
                await Create(hub);
                return;
            }
            hub.ChatIdReceiver = ReceiverChatId;
            await Update(hub);
        }
    }
}

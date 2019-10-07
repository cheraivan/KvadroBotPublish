using KopterBot.DTO;
using KopterBot.Exception;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class HubService : RepositoryProvider
    {
        public async ValueTask<long> GetReceviedChatId(long chatid)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == chatid);
            return hub.ChatIdReceiver;
        }
        public async Task StopChat(long chatid)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == chatid);
            if(hub == null)
            {
                throw new System.Exception("Чата не существует");
            }
            HubDTO reletedHub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);
            if(reletedHub == null)
            {
                throw new System.Exception("Чата не существует");
            }
            await hubRepository.Delete(hub);
            await hubRepository.Delete(reletedHub);
        }
        public async ValueTask<long[]> GetChatId(long chatid)
        {
            HubDTO hub1 = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);
            long[] res = new long[2];
            res[0] = hub1.ChatIdCreater;
            res[1] = hub1.ChatIdReceiver;
            return res;
        }
        public async ValueTask<bool> IsChatActive(long chatid)
        {
            HubDTO hub1 = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == chatid);

            if (hub1 == null || hub1.ChatIdReceiver == 0)
            {
                return false;
            }

            HubDTO hubs = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == hub1.ChatIdReceiver);
            return hubs == null ? false : true;
        }

        public async Task ConfirmDialog(long CreaterChatId, long ReceiverChatId,bool isStart)
        {
            if (isStart)
            {
                HubDTO reletedHub = new HubDTO(ReceiverChatId, CreaterChatId);
                await hubRepository.Create(reletedHub);
                return;
            }
            else
            {
                HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == CreaterChatId);
                await hubRepository.Delete(hub);
            }
        }

        public async ValueTask<bool> PilotInDialog(long chatid)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdCreater == chatid);

            if (hub == null)
                return false;

            HubDTO reletedHub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);
            if (reletedHub == null)
                return false;

            return true;
        }

        public async Task CreateDialog(long CreaterChatId, long ReceiverChatId)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i=>i.ChatIdCreater==CreaterChatId);
            if (hub == null)
            {
                hub = new HubDTO(CreaterChatId, ReceiverChatId);
                await hubRepository.Create(hub);
                return;
            }
            hub.ChatIdReceiver = ReceiverChatId;
            await hubRepository.Update(hub);
        }
    }
}

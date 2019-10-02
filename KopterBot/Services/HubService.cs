using KopterBot.DTO;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class HubService : RepositoryProvider
    {
        public async Task ConfirmDialog(string confirm, long CreaterChatId, long ReceiverChatId)
        {
            if (confirm == "Начать")
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

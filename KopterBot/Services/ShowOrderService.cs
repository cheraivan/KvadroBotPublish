using KopterBot.DTO;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class ShowOrderService:RepositoryProvider
    {
        // важно сделать проверку если элемент всего один
        private async ValueTask<int> ProductWhithMinId() // for max
        {
            int min =await buisnessTaskRepository.MinId();
            return buisnessTaskRepository.Get().Where(i => i.Id > min).FirstOrDefault().Id;
        }

        public async ValueTask<int> GetMessageId(long chatid)
        {
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if(order == null)
            {
                order = new ShowOrdersDTO()
                {
                    ChatId = chatid
                };
                await showOrdersRepository.Create(order);

            }
            return order.MessageId;
        }

        public async Task ChangeMessageId(long chatid,int messageId)
        {
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (order == null)
            {
                order = new ShowOrdersDTO();
                order.ChatId = chatid;
                order.MessageId = messageId;
                order.CurrentProductId = 1;
                await showOrdersRepository.Create(order);
                return;
            }
            order.MessageId = messageId;
            await showOrdersRepository.Update(order);
        }

        public async ValueTask<BuisnessTaskDTO> GetPreviousProduct(long chatid)
        {
            int messageId = await GetMessageId(chatid);
            int? idPreviousProduct = await PreviousProduct(chatid, messageId);
            BuisnessTaskDTO task = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idPreviousProduct);
            if (task == null)
                return null;
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            order.CurrentProductId = task.Id;
            await showOrdersRepository.Update(order);

            return task;
        }
        public async ValueTask<BuisnessTaskDTO> GetNextProduct(long chatid)
        {
            int messageId = await GetMessageId(chatid);
            int? idNextProduct = await NextProduct(chatid, messageId);
            BuisnessTaskDTO task = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idNextProduct);
            if (task == null)
                return null;

            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            order.CurrentProductId = task.Id;
            await showOrdersRepository.Update(order);

            return task;
        }

        public async Task SetDefaultProduct(long chatid)
        {
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (order == null)
                throw new Exception("order cannot be null");
            order.CurrentProductId = await buisnessTaskRepository.MinId();
            await showOrdersRepository.Update(order);
        }

        private async ValueTask<int?> PreviousProduct(long chatid,int MessageId)
        {
            int min =await buisnessTaskRepository.MinId();
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (order == null)
            {
                ShowOrdersDTO _order = new ShowOrdersDTO
                {
                    ChatId = chatid,
                    CurrentProductId = min,
                    MessageId = MessageId,
                };
                await showOrdersRepository.Create(_order);
                return null;
            }
            if (min == order.Id)
                return null;
            int currIdProduct = order.Id;
            BuisnessTaskDTO result = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id < currIdProduct);
            return result.Id;
        }
        private async ValueTask<int?> NextProduct(long chatid,int MessageId)
        {
            ShowOrdersDTO order =await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if(order == null)
            {
                ShowOrdersDTO _order = new ShowOrdersDTO
                {
                    ChatId = chatid,
                    CurrentProductId =await ProductWhithMinId(),
                    MessageId = MessageId
                };
                await showOrdersRepository.Create(_order);
                return _order.CurrentProductId;
            }
            int currIdProduct = order.CurrentProductId;
            int maxId =await buisnessTaskRepository.MaxId();
            if (currIdProduct == maxId)
                return null;
            BuisnessTaskDTO result = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id > currIdProduct);
            return result.Id;
        }
    }
}

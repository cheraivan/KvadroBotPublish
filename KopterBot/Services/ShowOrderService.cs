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
        public async ValueTask<ShowOrdersDTO> CurrentProductId(long chatid)
        {
           return  await showOrdersRepository.Get().Where(i => i.ChatId == chatid).FirstOrDefaultAsync();
        }

        public async ValueTask<List<int>> GetIdTasksForUser(long chatid)
        {
            List<int> result = new List<int>();
            await buisnessTaskRepository.Get().Where(i => i.ChatId == chatid).ForEachAsync((item) =>
            {
                result.Add(item.Id);
            });
            return result;
        }

        private async ValueTask<int> ProductWhithMinId(long chatid,bool isBuisnessman) // for max
        {
            int min;
            if (isBuisnessman)
            {
                min = await buisnessTaskRepository.MinId(chatid);
            }
            else
            {
                min = await buisnessTaskRepository.MinId();
            }
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



        public async ValueTask<List<BuisnessTaskDTO>> AllTaskForBuisnessman(long chatid)
        {
            List<BuisnessTaskDTO> result = new List<BuisnessTaskDTO>();
            result =await buisnessTaskRepository.Get().Where(i => i.ChatId == chatid).ToListAsync();
            return result;
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

        public async ValueTask<BuisnessTaskDTO> GetPreviousProduct(long chatid,bool isBuisnessman = false)
        {
            int messageId = await GetMessageId(chatid);
            int? idPreviousProduct;
            BuisnessTaskDTO task;
            ShowOrdersDTO order;

            if (isBuisnessman)
            {

                idPreviousProduct = await PreviousProduct(chatid, messageId, true);
                task = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idPreviousProduct);
                if (task == null)
                    return null;
                order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
                order.CurrentProductId = task.Id;
                await showOrdersRepository.Update(order);
                return task;
            }
            idPreviousProduct = await PreviousProduct(chatid, messageId);
            task = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idPreviousProduct);
            if (task == null)
                return null;
            order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            order.CurrentProductId = task.Id;
            await showOrdersRepository.Update(order);

            return task;
        }
        public async ValueTask<BuisnessTaskDTO> GetNextProduct(long chatid,bool isBuisnessman = false)
        {
            int messageId = await GetMessageId(chatid);
            int? idNextProduct;
            BuisnessTaskDTO task;
            ShowOrdersDTO order;
            if (isBuisnessman)
            {
                idNextProduct = await NextProduct(chatid, messageId, true);
                task = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idNextProduct);
                if(task == null)
                    return null;
                order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
                order.CurrentProductId = task.Id;
                await showOrdersRepository.Update(order);
                return task;
            }
            idNextProduct = await NextProduct(chatid, messageId);
            task = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idNextProduct);
            if (task == null)
                return null;

            order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            order.CurrentProductId = task.Id;
            await showOrdersRepository.Update(order);

            return task;
        }

        public async Task SetDefaultProduct(long chatid,bool isBuisnessMan = false)
        {
            ShowOrdersDTO order;

            if(isBuisnessMan)
            {
                order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
                if (order == null)
                    throw new Exception("order cannot be null");
                order.CurrentProductId = await buisnessTaskRepository.MinId(chatid);
                await showOrdersRepository.Update(order);
                return;
            }
            order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (order == null)
            {
                order = new ShowOrdersDTO();
                order.ChatId = chatid;
                order.CurrentProductId = await buisnessTaskRepository.MinId();
                await showOrdersRepository.Create(order);
            }
            order.CurrentProductId = await buisnessTaskRepository.MinId();
            await showOrdersRepository.Update(order);
        }

        private async ValueTask<int?> PreviousProduct(long chatid,int MessageId,bool isBuisnessman = false)
        {
            int min;
            ShowOrdersDTO order;
            int currIdProduct;
            BuisnessTaskDTO result;
            // частный случай вывода своих заказов
            if (isBuisnessman)
            {
                min = await buisnessTaskRepository.MinId(chatid);

                order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);

                if(order == null)
                {
                    ShowOrdersDTO newOrder = new ShowOrdersDTO
                    {
                        ChatId = chatid,
                        CurrentProductId = min,
                        MessageId = MessageId
                    };
                    await showOrdersRepository.Create(newOrder);
                    return null;
                }

                if(min == order.Id)
                {
                    return null;
                }
                currIdProduct = order.Id;
                result = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id < currIdProduct && i.ChatId == chatid);
                return result.Id;
            }
            min =await buisnessTaskRepository.MinId();
            order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
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
            currIdProduct = order.CurrentProductId;
            result = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id < currIdProduct);
            return result.Id;
        }
        private async ValueTask<int?> NextProduct(long chatid,int MessageId,bool isBuisnessman = false)
        {
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid); ;
            int currIdProduct;
            BuisnessTaskDTO result;
            int maxId;
            ShowOrdersDTO _order;


            if (isBuisnessman)
            {
                if(order == null)
                {
                    _order = new ShowOrdersDTO
                    {
                        ChatId = chatid,
                        CurrentProductId = await ProductWhithMinId(chatid,true),
                        MessageId = MessageId
                    };
                    await showOrdersRepository.Create(_order);
                    return _order.CurrentProductId;
                }
                currIdProduct = order.CurrentProductId;
                maxId = await buisnessTaskRepository.MaxId(chatid);
                if (currIdProduct == maxId)
                    return null;
                result = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id > currIdProduct && i.ChatId == chatid);
                return result.Id;
            }

            if(order == null)
            {
                _order = new ShowOrdersDTO
                {
                    ChatId = chatid,
                    CurrentProductId =await ProductWhithMinId(chatid,false),
                    MessageId = MessageId
                };
                await showOrdersRepository.Create(_order);
                return _order.CurrentProductId;
            }
            currIdProduct = order.CurrentProductId;
            maxId = await buisnessTaskRepository.MaxId();
            if (currIdProduct == maxId)
                return null;
            result = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id > currIdProduct);
            return result.Id;
        }
    }
}

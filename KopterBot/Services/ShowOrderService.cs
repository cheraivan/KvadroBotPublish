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
        private int ProductWhithMinId() // for max
        {
            int min = buisnessTaskRepository.MinId();
            return buisnessTaskRepository.Get().Where(i => i.Id > min).FirstOrDefault().Id;
        }

        public async ValueTask<BuisnessTaskDTO> GetPreviousProduct(long chatid)
        {
            int? idPreviousProduct = await PreviousProduct(chatid);
            return idPreviousProduct == null?null: await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idPreviousProduct);
        }

        public async ValueTask<BuisnessTaskDTO> GetNextProduct(long chatid)
        {
            int? idNextProduct = await NextProduct(chatid);
            return idNextProduct == null ? null : await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id == idNextProduct);
        }

        private async ValueTask<int?> PreviousProduct(long chatid)
        {
            int min = buisnessTaskRepository.MinId();
            ShowOrdersDTO order = await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (order == null)
            {
                ShowOrdersDTO _order = new ShowOrdersDTO
                {
                    ChatId = chatid,
                    CurrentProductId = min
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
        private async ValueTask<int?> NextProduct(long chatid)
        {
            ShowOrdersDTO order =await showOrdersRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if(order == null)
            {
                ShowOrdersDTO _order = new ShowOrdersDTO
                {
                    ChatId = chatid,
                    CurrentProductId = ProductWhithMinId() 
                };
                await showOrdersRepository.Create(_order);
                return _order.CurrentProductId;
            }
            int currIdProduct = order.CurrentProductId;
            int maxId = buisnessTaskRepository.MaxId();
            if (currIdProduct == maxId)
                return null;
            BuisnessTaskDTO result = await buisnessTaskRepository.Get().FirstOrDefaultAsync(i => i.Id > currIdProduct);
            return result.Id;
        }
    }
}

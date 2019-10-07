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
    class OfferService:RepositoryProvider
    {
        public async Task Create(long chatid,OfferDTO offer)
        {
            if (offer == null)
                throw new SystemException("offer cannot be null");
            int count = await offerRepository.Get().Where(i => i.TaskId == offer.TaskId && i.ChatId == chatid).CountAsync();
            if(count == 0)
            {
                await offerRepository.Create(offer);
            }
        }
        public async ValueTask<List<UserDTO>> GetUsersOffer(int id)// id продукта , переспросить насчет запроса
        {
            List<long> lstOfChatIds = await offerRepository.Get().Where(i => i.TaskId == id)
                .Select(p => p.ChatId)
                .ToListAsync();
            List<UserDTO> result = new List<UserDTO>();
            foreach(var i in lstOfChatIds)
            {
                UserDTO user = await userRepository.FindById(i);
                result.Add(user);
            }
            return result;
        }
    }
}

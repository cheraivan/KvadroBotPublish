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
                throw new Exception("offer cannot be null");
            int count = await offerRepository.Get().Where(i => i.ChatId == chatid).CountAsync();
            if(count == 0)
            {
                await offerRepository.Create(offer);
            }
        }
    }
}

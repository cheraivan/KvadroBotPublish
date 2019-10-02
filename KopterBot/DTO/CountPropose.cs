using KopterBot.Base;
using KopterBot.Bot;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.DTO
{
    class CountPropose
    {
        [Key]
        public int Id { get; set; }
        public int Count { get; set; }
    }
    class CountProposeHandler:RepositoryProvider
    {
        public async ValueTask<int> GetCount()
        {
           CountPropose c = await countProposeRepository.Get().FirstOrDefaultAsync();
            return c.Count;
        }

        public async Task ChangeProposeCount()
        {
            int countFields = await countProposeRepository.CountAsync();
            if(countFields == 0)
            {
                CountPropose c = new CountPropose();
                c.Count = 1;
                await countProposeRepository.Create(c);
            }
            CountPropose countPropose = await countProposeRepository.Get().FirstOrDefaultAsync();
            countPropose.Count = countPropose.Count + 1;
            await countProposeRepository.Update(countPropose);
        }
    }
}
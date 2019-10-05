using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class BuisnessTaskRepository:BaseProviderImpementation<BuisnessTaskDTO>
    {
        public async ValueTask<int> MaxId()
        {
            List<int> lstOfId = await db.buisnessTasks.Select(i => i.Id).ToListAsync();
            lstOfId.Sort();
            return lstOfId[lstOfId.Count-1];
        }
        public async ValueTask<int> MinId()
        {
            List<int> lstOfId = await db.buisnessTasks.Select(i => i.Id).ToListAsync();
            lstOfId.Sort();
            return lstOfId[0];
        }
    }
}

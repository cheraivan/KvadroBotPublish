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
    class CountProposeRepository:BaseProviderImpementation<CountPropose>
    {
        public async ValueTask<int> CountAsync()
        {
            return await db.CountPurpose.CountAsync();
        }
    }
}

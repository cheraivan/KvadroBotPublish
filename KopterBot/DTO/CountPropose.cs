using KopterBot.Bot;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.DTO
{
    class CountPropose
    {
        public int Count { get; set; }
    }
    class CountProposeHandler
    {
        private static ApplicationContext db;
        static CountProposeHandler()
        {
            db = new ApplicationContext();
        }

        public async static ValueTask<int> GetCount()
        {
            CountPropose c = await db.CountPurpose.FirstOrDefaultAsync();
            return c.Count;
        }

        public async static Task ChangeProposeCount()
        {
            int countFields = await db.CountPurpose.CountAsync();
            if(countFields == 0)
            {
                CountPropose c = new CountPropose();
                c.Count = 1;
                db.CountPurpose.Add(c);
                await db.SaveChangesAsync();
            }
            CountPropose countPropose = await db.CountPurpose.FirstOrDefaultAsync();
            countPropose.Count = countPropose.Count + 1;
            db.Entry(countPropose).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }
    }
}
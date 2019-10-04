using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class BuisnessTaskRepository:BaseProviderImpementation<BuisnessTaskDTO>
    {
        public int MaxId() =>
            db.buisnessTasks.Max(i => i.Id);
        public int MinId() =>
            db.buisnessTasks.Min(i => i.Id);
    }
}

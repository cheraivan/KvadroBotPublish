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
    class DronRepository: BaseProviderImpementation<DronDTO>
    {

        public async Task CreateDron(DronDTO dto)
        {
            IEnumerable<DronDTO> drons = await Get(dr => dto.Mark == dr.Mark);
            drons = drons.Take(1);
            if (drons == null)
            { 
                await Create(dto);
            }
            return;
        }
    }
}

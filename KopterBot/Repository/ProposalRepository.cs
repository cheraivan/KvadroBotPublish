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
    class ProposalRepository:BaseProviderImpementation<ProposalDTO>
    {
        public async ValueTask<int> GetCurrentNumberProposalAsync(long chatid)
        {
            UserDTO user = await db.Users.FirstOrDefaultAsync(i => i.ChatId == chatid);
            return user.proposals.Count;
        }
        public async Task Create(long chatid)
        {
            UserDTO user = await db.Users.FirstOrDefaultAsync(i => i.ChatId == chatid);

            ProposalDTO porposal = new ProposalDTO()
            {
                ChatId = chatid
            };
            await Create(porposal);
        }

        public async Task DeleteNotFillProposalAsync(long chatid)
        {
            int count = await db.proposalsDTO.Where(i => i.ChatId == chatid)
                .CountAsync();
            if (count == 0)
                return;
            count = await db.proposalsDTO.Where(i => i.longtitude == null).CountAsync();
            if (count == 0)
                return;
            IEnumerable<ProposalDTO> Ids = db.proposalsDTO.Where(i => i.longtitude == null);
            db.proposalsDTO.RemoveRange(Ids);
            await db.SaveChangesAsync();
        }
    }
}

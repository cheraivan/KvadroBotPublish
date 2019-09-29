using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class ProposalRepository:BaseRepository
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
            user.proposals.Add(porposal);

            await db.SaveChangesAsync();
        }

        public async ValueTask<ProposalDTO> FindById(long chatid)
        {
            return await db.proposalsDTO.AsNoTracking().LastOrDefaultAsync(i => i.ChatId == chatid);
        }

        public async Task Update(ProposalDTO item)
        {
            db.Entry(item).State = EntityState.Modified;
            await db.SaveChangesAsync();
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

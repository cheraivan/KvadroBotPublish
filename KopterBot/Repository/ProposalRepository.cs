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
            UserDTO user = await db.Users.AsNoTracking().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (user == null)
                throw new NullReferenceException("User cannot be null");
            ProposalDTO porposal = await db.proposalsDTO.AsNoTracking().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (porposal != null)
                return;
            porposal = new ProposalDTO()
            {
                ChatId = chatid
            };
            await Create(porposal);
        }

        public async override ValueTask<ProposalDTO> FindById(long id)
        {
            return await db.proposalsDTO
                .FirstOrDefaultAsync(i => i.ChatId == id);
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

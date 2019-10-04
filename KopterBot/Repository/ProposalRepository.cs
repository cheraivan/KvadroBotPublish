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
        public async Task RemoveRange(IEnumerable<ProposalDTO> lst)
        {
            db.RemoveRange(lst);
            await db.SaveChangesAsync();
        }
        public async Task Create(UserDTO user)
        {
            if (user == null)
                throw new NullReferenceException("User cannot be null");
            ProposalDTO porposal = await db.proposalsDTO.AsNoTracking().FirstOrDefaultAsync(i => i.ChatId == user.ChatId);
            if (porposal != null)
                return;
            porposal = new ProposalDTO()
            {
                ChatId = user.ChatId
            };
            await base.Create(porposal);
        }
    }
}

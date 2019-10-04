using KopterBot.DTO;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class ProposalService : RepositoryProvider
    {
        public async Task DeleteNotFillProposalAsync(long chatid)
        {
            int count = await proposalRepository.Get().Where(i => i.ChatId == chatid)
                .CountAsync();
            if (count == 0)
                return;
            count = await  proposalRepository.Get().Where(i => i.longtitude == null).CountAsync();
            if (count == 0)
                return;
            IEnumerable<ProposalDTO> Ids = proposalRepository.Get().Where(i => i.longtitude == null);
            await proposalRepository.RemoveRange(Ids);
        }
        public async ValueTask<int> GetCurrentNumberProposalAsync(long chatid)
        {
            UserDTO user = await userRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            return user.proposals.Count;
        }
        public async Task Create(UserDTO user)
        {
            await proposalRepository.Create(user);
        }
        public async Task Update(ProposalDTO proposal)
        {
            await proposalRepository.Update(proposal);
        }

        public async ValueTask<ProposalDTO> FindById(long chatid)
        {
            return proposalRepository.Get().Where(i => i.ChatId == chatid).FirstOrDefault();
        }
    }
}

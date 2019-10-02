using KopterBot.DTO;
using KopterBot.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class ProposalService:RepositoryProvider
    {
        public async ValueTask<ProposalDTO> FindById(long chatid)
        {
            return await proposalRepository.FindById(chatid);
        }
    }
}

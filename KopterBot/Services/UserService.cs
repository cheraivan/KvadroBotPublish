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
    class UserService:RepositoryProvider
    {
        public async Task<UserDTO> FindUserByPredicate(Func<UserDTO,bool> predicate)
        {
            IQueryable<UserDTO> users = userRepository.Get();
            return users.Where(predicate).FirstOrDefault();
        }
        public async ValueTask<UserDTO> FindById(int id)
        {
            return await userRepository.FindById(id);
        }
        public async ValueTask<UserDTO> FindById(long chatid)
        {
            return await userRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
        }

        public async ValueTask<List<UserDTO>> GetUsersById(long chatid)
        {
            List<long> usersid = await GetUsersIdByRegion(chatid);
            if (usersid.Count == 0)
                return null;

            List<UserDTO> result = new List<UserDTO>();

            foreach(var i in usersid)
            {
                UserDTO user = await userRepository.FindById(i);
                result.Add(user);
            }
            return result;
        }

        public async ValueTask<List<long>> GetUsersIdByRegion(long chatid)
        {
            List<long> result = new List<long>();
            ProposalDTO proposal = await proposalRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            string region = "";
            if(proposal != null)
            {
                if(proposal.RealAdress != null)
                {
                    int index = proposal.RealAdress.IndexOf(",")+2;
                    int lastindex = proposal.RealAdress.IndexOf(",", index+1);

                    for (int i = index; i < lastindex; i++)
                        region += proposal.RealAdress[i];
                }
            }
            result = await proposalRepository.Get().Where(i => i.RealAdress.IndexOf(region) != -1).
                Select(p => p.ChatId)
                .ToListAsync();
            return result;
        }

        public async Task Update(UserDTO user)
        {
             await userRepository.Update(user);
        }
        public async Task AuthenticateUser(long chatid)
        {
            UserDTO user = await userRepository.FindById(chatid);
            if (user == null)
            {
                user = new UserDTO();
                user.ChatId = chatid;
                await userRepository.Create(user);
            }
            return;
        }
        public async ValueTask<string> GetCurrentActionName(long chatid)
        {
            UserDTO user = await userRepository.Get().AsNoTracking().Include(i => i.step)
                .FirstOrDefaultAsync(c => c.ChatId == chatid);
            return user != null ? user.step.NameOfStep : "null";
        }
        public async ValueTask<int> GetCurrentActionStep(long chatid)
        {
            UserDTO user = await userRepository.Get().Include(i => i.step)
                .FirstOrDefaultAsync(c => c.ChatId == chatid);
            return user.step.CurrentStep;
        }
        public async Task RecoveryUser(long chatid)
        {
            UserDTO user = await userRepository.FindById(chatid);
            if (user == null)
                await AuthenticateUser(chatid);
            else
            {
                user = new UserDTO() { ChatId = chatid };
                await userRepository.Update(user);
            }
        }
        public async Task ChangeAction(long chatid, string nameAction, int step)
        {
            UserDTO user = await userRepository.Get().Include(i => i.step)
                .FirstOrDefaultAsync(j => j.ChatId == chatid);
            if (user == null)
            {
                await AuthenticateUser(chatid);
                return;
            }

            user.step.NameOfStep = nameAction;
            user.step.CurrentStep = step;
            await userRepository.Update(user);
        }
        public async ValueTask<bool> IsAuthenticate(long chatid)
        {
            return await userRepository.FindById(chatid) != null ? true : false;
        }

        public async ValueTask<bool> IsUserRegistration(long chatid)
        {
            ProposalDTO proposal = await proposalRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (proposal == null)
                return false;
            if (proposal.longtitude.HasValue && proposal.latitude.HasValue)
                return true;
            return false;
        }
    }
}

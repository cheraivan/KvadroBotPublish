using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using KopterBot.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class UserRepository:BaseProviderImpementation<UserDTO>
    {
        #region repository_methods
        public override async Task Create(UserDTO user)
        {
            if (user.ChatId == 0)
                throw new System.Exception("chat id cant be null");
            user.step = new StepDTO();
            //user.step.ChatId = user.ChatId;
            user.proposals = new List<ProposalDTO>();
            int currMaxId;
            try
            {
                currMaxId = await db.Users.MaxAsync(i => i.IdForShow) + 1;
            } catch (System.Exception ex)
            {
                currMaxId = 1;
            }
            user.IdForShow = currMaxId;
            await base.Create(user);
        }
      
        #endregion

        public async Task AuthenticateUser(long chatid)
        {
            UserDTO user = await FindById(chatid);
            if (user == null)
            {
                user = new UserDTO();
                user.ChatId = chatid;
                await Create(user);
            }
            return;
        }
        public async ValueTask<string> GetCurrentActionName(long chatid)
        {
            UserDTO user = await db.Users.AsNoTracking().Include(i => i.step)
                .FirstOrDefaultAsync(c => c.ChatId == chatid);
            return user != null ? user.step.NameOfStep : "null";
        }
        public async ValueTask<int> GetCurrentActionStep(long chatid)
        {
            UserDTO user = await db.Users.Include(i => i.step)
                .FirstOrDefaultAsync(c => c.ChatId == chatid);
            return user.step.CurrentStep;
        }
        public async Task RecoveryUser(long chatid)
        {
            UserDTO user = await FindById(chatid);
            if (user == null)
                await AuthenticateUser(chatid);
            else
            {
                user = new UserDTO() { ChatId = chatid };
                await Update(user);
            }
        }
        public async Task ChangeAction(long chatid,string nameAction,int step)
        {
            UserDTO user = await db.Users.Include(i => i.step)
                .FirstOrDefaultAsync(j => j.ChatId == chatid);
            if (user == null)
            {
                await AuthenticateUser(chatid);
                return;
            }
         
            user.step.NameOfStep = nameAction;
            user.step.CurrentStep = step;
            await Update(user);
        }
        public async ValueTask<bool> IsAuthenticate(long chatid)
        {
            return await FindById(chatid) != null ? true : false;
        }

        public async ValueTask<bool> IsUserRegistration(long chatid)
        {
            ProposalDTO proposal = await db.proposalsDTO.FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (proposal == null)
                return false;
            if(proposal.longtitude.HasValue && proposal.latitude.HasValue)
                return true;
            return false;
        }
    }
}

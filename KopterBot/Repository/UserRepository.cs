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
    class UserRepository:UserDTO,IRepository<UserDTO>
    {
        #region repository_methods
        public async ValueTask<UserDTO> FindById(long chatid)
        {
            return await db.Users.AsNoTracking().
                FirstOrDefaultAsync(i => i.ChatId == chatid);
        }
        public async Task Create(UserDTO user)
        {
            if (user.ChatId == 0)
                throw new Exception("chat id cant be null");
            user.step = new StepDTO();
            user.step.ChatId = user.ChatId;
            user.proposals = new List<ProposalDTO>();
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
        public async Task Update(UserDTO user)
        {
            db.Entry(user).State = EntityState.Detached;
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync(); ;
        }
        public async Task Delete(UserDTO user)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync();
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
        public async ValueTask<string>GetCurrentActionName(long chatid)
        {
            var count = await db.Users.AsNoTracking()
                  .Include(i => i.step)
                  .Where(i => i.ChatId == chatid)
                  .FirstOrDefaultAsync();
            return count== null ? "null" : count.step.NameOfStep;
        }
        public async ValueTask<int> GetCurrentActionStep(long chatid)
        {
            UserDTO dto = await db.Users.AsNoTracking()
                .Include(i => i.step)
                .Where(j => j.ChatId == chatid)
                .FirstOrDefaultAsync();
            return dto.step.CurrentStep;
           
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
            UserDTO user = await FindById(chatid);
            if (user == null)
            {
                await AuthenticateUser(chatid);
                return;
            }
            UserDTO _user = await db.Users.AsNoTracking()
                .Include(i => i.step)
                .Where(j => j.ChatId == chatid)
                .FirstOrDefaultAsync();
            
            user.step = _user.step;
            user.step.NameOfStep = nameAction;
            user.step.CurrentStep = step;
            await Update(user);
        }
        public async ValueTask<bool> IsUserInAction(long chatid)
        {
            string name =await GetCurrentActionName(chatid);
            return name == null ? false : true; 
        }
        public async ValueTask<bool> IsAuthenticate(long chatid)
        {
            return await FindById(chatid) != null ? true : false;
        }
    }
}

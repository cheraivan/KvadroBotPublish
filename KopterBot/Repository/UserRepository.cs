using KopterBot.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Repository
{
    class UserRepository:UserDTO
    {
        private GenericRepository<UserDTO> repository;
        public UserRepository()
        {
            repository = new GenericRepository<UserDTO>(db);
        }
        public async Task AuthenticateUser(long chatid)
        {
            UserDTO user = await db.Users.FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (user == null)
            {
                user = new UserDTO();
                user.ChatId = chatid;
                user.step = new StepDTO();
                user.proposals = new List<ProposalDTO>();
                await repository.Create(user);
            }
            return;
        }
        public string GetCurrentActionName(long chatid)
        {
          var lst = db.Users.Join(db.Steps,
               i => i.ChatId,
               s => s.ChatId,
               (i, s) => new UserDTO
               {
                   step = i.step
               }).ToList();
            return lst.Count() == 0 ? "null" : lst[0].step.NameOfStep;
        }
        public int GetCurrentActionStep(long chatid) =>
            db.Users.Join(db.Steps,
                i => i.ChatId,
                s => s.ChatId,
                (i, s) => new UserDTO
                {
                    step = i.step
                }).ToList()[0].step.CurrentStep;
        public async Task RecoveryUser(long chatid)
        {
            UserDTO user = await db.Users.FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (user == null)
                await AuthenticateUser(chatid);
            else
            {
                user = new UserDTO() { ChatId = chatid };
                await repository.Update(user);
            }
        }
        public async Task ChangeAction(long chatid,string nameAction,int step)
        {
            UserDTO user = await db.Users.FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (user == null)
            {
                await AuthenticateUser(chatid);
                return;
            }
            UserDTO _user = db.Users.Join(db.Steps,
                i => i.ChatId,
                s => s.ChatId,
                (i, s) => new UserDTO
                {
                    step = i.step
                }).ToList()[0];
            user.step = _user.step;
            user.step.NameOfStep = nameAction;
            user.step.CurrentStep = step;
            await repository.Update(user);
        }

        public bool IsUserInAction(long chatid)
        {
            string name = GetCurrentActionName(chatid);
            return name == "null" || name == "NULL" ? false : true; 
        }
        public async ValueTask<bool> IsAuthenticate(long chatid)
        {
            return await repository
                .FindById(chatid) != null ? true : false;
        }
    }
}

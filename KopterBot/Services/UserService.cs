using KopterBot.DTO;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Services
{
    class UserService:RepositoryProvider
    {
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
    class AdminService: RepositoryProvider
    {

    }
    class DronService : RepositoryProvider
    {

    }
    class BuisnessTaskService : RepositoryProvider
    {
    }
    class StepService : RepositoryProvider
    {
    }
    class HubService : RepositoryProvider
    {
    }
    class StorageService
    {
    }

    class ServiceProvider:RepositoryProvider
    {
        private UserService _userService;
        public UserService userService
        {
            get
            {
                if (_userService == null)
                    _userService = new UserService();
                return _userService;
            }
        }
    }
}

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
    class ShowUserService:RepositoryProvider
    {
        public async ValueTask<int> CountUsersAsync() =>
            await userRepository.Get().CountAsync();
        public async ValueTask<int> GetMessageId(long chatid)
        {
            ShowUsersDTO showUsersTable = await showUserRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if(showUsersTable == null)
            {
                showUsersTable = new ShowUsersDTO()
                {
                    ChatId = chatid
                };
                await showUserRepository.Create(showUsersTable);
            }
            return showUsersTable.MessageId;
        }
        public async Task ChangeMessageId(long chatid,int messageId)
        {
            ShowUsersDTO dto = await showUserRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            dto.MessageId = messageId;
            await showUserRepository.Update(dto);
        }

        // получаем первого пользователя для просмотра команды просмотра
        public async ValueTask<UserDTO> GetFirstUserForCommand(long chatid,string region) 
        {
            List<UserDTO> userLst =await (from u in userRepository.Get()
                                    join p in proposalRepository.Get()
                                    on u.ChatId equals p.ChatId
                                    where p.Region == region
                                    select new UserDTO
                                    {
                                        BuisnesPrivilag = u.BuisnesPrivilag,
                                        ChatId = u.ChatId,
                                        FIO = u.FIO,
                                        IdForShow = u.IdForShow,
                                        IsRegister = u.IsRegister,
                                        Login = u.Login,
                                        Phone = u.Phone,
                                        PilotPrivilag = u.PilotPrivilag
                                    }).ToListAsync();

            /*
            int minId = await userRepository.Get().MinAsync(i => i.IdForShow);
            UserDTO currUser = await userRepository.FindById(chatid);
            UserDTO userWithMinIdForShow = await userRepository.Get().FirstOrDefaultAsync(i => i.IdForShow == minId);
            long chatIdSearchUsers; // chatid искомого пользователя
            if (currUser.ChatId == userWithMinIdForShow.ChatId)
            {
                currUser = await userRepository.Get().FirstOrDefaultAsync(i => i.IdForShow > minId);
                minId = currUser.IdForShow;
            }
            else
            {
                currUser = await userRepository.Get().FirstOrDefaultAsync(i => i.IdForShow == minId);
            }
            ShowUsersDTO showUsersTable = await showUserRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if(showUsersTable == null)
            {
                showUsersTable = new ShowUsersDTO()
                {
                    ChatId = chatid,
                    CurrentId = minId
                };
            }
            showUsersTable.CurrentId = minId;
            await showUserRepository.Update(showUsersTable);
            return currUser;
            */
            int minId = userLst.Min(i => i.IdForShow);
            UserDTO currUser = userLst.Find(i => i.ChatId == chatid);
            UserDTO userWithMinIdForShow = userLst.FirstOrDefault(i => i.IdForShow == minId);
            if(currUser.ChatId == userWithMinIdForShow.ChatId)
            {
                currUser = userLst.FirstOrDefault(i => i.IdForShow > minId);
                minId = currUser.IdForShow;
            }
            else
            {
                currUser = userLst.FirstOrDefault(i => i.IdForShow == minId);
            }
            ShowUsersDTO showUsersTable = await showUserRepository.Get().FirstOrDefaultAsync(i => i.ChatId == chatid);
            if (showUsersTable == null)
            {
                showUsersTable = new ShowUsersDTO()
                {
                    ChatId = chatid,
                    CurrentId = minId
                };
                await showUserRepository.Create(showUsersTable);
            }
            showUsersTable.CurrentId = minId;
            await showUserRepository.Update(showUsersTable);
            return currUser;
        }

       /* public async ValueTask<UserDTO> GetPreviousUser(long chatid)
        {
            
        }*/
    }
}

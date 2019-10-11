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

        // получаем первого пользователя именно для просмотра
        public async ValueTask<UserDTO> GetFirstUserForCommand(long chatid) 
        {
            int minId = await userRepository.Get().MinAsync(i => i.IdForShow);
            UserDTO user = await userRepository.FindById(chatid);
            long chatIdSearchUsers; // chatid искомого пользователя
            if (minId == user.IdForShow)
            {
                user = await userRepository.Get().FirstOrDefaultAsync(i => i.IdForShow > minId);
                minId = user.IdForShow;
            }
            user = awa
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
            return await userRepository.Get().FirstOrDefaultAsync(i=>i.)
        }
        public async ValueTask<UserDTO> GetPreviousUser(long chatid)
        {
            
        }
    }
}

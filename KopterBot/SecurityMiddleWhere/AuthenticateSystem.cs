using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.Commons;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.SecurityMiddleWhere
{
    class AuthenticateSystem
    {
        TelegramBotClient client;
        MainProvider provider;

        public AuthenticateSystem(TelegramBotClient client,MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }
        
        public async Task GiveAdminPrivilage(UserDTO user)
        {
            bool isAdmin = await IsExistAdminWithCurrChatId(user);
            if (isAdmin)
            {
                await provider.adminService.ChangeWish(user.ChatId);
            }
        }

        public async ValueTask<bool> IsAdmin(UserDTO user)
        {
            return await provider.adminService.IsAdmin(user.ChatId);
        }

        public async ValueTask<bool> IsExistAdminWithCurrChatId(UserDTO user)
        {
            if (user == null)
                return false;
            List<long> adminChatId = await provider.adminService.GetChatId();
            if (adminChatId.Contains(user.ChatId))
                return true;
            return false;
        }



        public async ValueTask<bool> isAllowedUser(UserDTO user, int privil)
        {
            if (user == null)
                return false;
            if(user.PilotPrivilag < privil)
            {
                await client.SendTextMessageAsync(user.ChatId, "Вам недоступна данная функция",0,false,false,0,KeyBoardHandler.Murkup_Start_AfterChange());
                return false;
            }
            return true;
        }

   /*     public async ValueTask<List<string>> AllowedCommandList(UserDTO user)
        {
            List<string> result = new List<string>();

            if(user == null)
            {
                CommandList.RegistrationPilotCommandList().ForEach((item) =>
                {
                    result.Add(item);
                });
                CommandList.RegistrationBuisnessCommandList().ForEach((item) =>
                {
                    result.Add(item);
                });
                return result;
            }
            else
            {
                if(user.PilotPrivilag == 1)
                {
                    CommandList.PilotCommandListWithMinPrivilage().ForEach((item) =>
                    {
                        result.Add(item);
                    });
                    return result;
                }
                if(user.PilotPrivilag == 2)
                {
                    CommandList.PilotCommandListWithMaxPrivilage().ForEach((item) =>
                    {
                        result.Add(item);
                    });
                }
            }
            return null;
        }*/
    }
}

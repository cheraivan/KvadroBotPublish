using KopterBot.Base.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.PilotCommands.CallBacks
{
    class ShowUsersCommand:BaseCommand
    {
        public ShowUsersCommand(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task Response(long chatid)
        {
            int countUser = await provider.showUserService.CountUsersAsync();
            if(countUser == 1)
            {
                await client.SendTextMessageAsync(chatid, "К сожалению,вы единственный пользователь");
                return;
            }
            else
            {

            }
        }
    }
}

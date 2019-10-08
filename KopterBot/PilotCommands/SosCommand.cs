using KopterBot.Base.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.PilotCommands
{
    class SosCommand:BaseCommand
    {
        public SosCommand(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task SosHandler(MessageEventArgs messageObject)
        {
            long chatid = messageObject.Message.Chat.Id;
            string message = messageObject.Message.Text;

            int currStep = await provider.userService.GetCurrentActionStep(chatid);

            if(currStep == 1)
            {

            }
        }
    }
}

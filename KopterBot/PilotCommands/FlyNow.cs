using KopterBot.Base.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.PilotCommands
{
    class FlyNow:BaseCommand 
    {
        public FlyNow(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task Fly(MessageEventArgs messageObject)
        {
            string message = messageObject.Message.Text;
            long chatid = messageObject.Message.Chat.Id;

            int currStep = await provider.userService.GetCurrentActionStep(chatid);

            if(currStep == 1)
            {
                if(messageObject.Message.Location == null)
                {
                    await client.SendTextMessageAsync(chatid, "Геолокация была получена некорректно,сбросьте еще раз");
                    return;
                }
                float longtitude = messageObject.Message.Location.Longitude;
                float lautitude = messageObject.Message.Location.Latitude;


            }
        }
    }
}

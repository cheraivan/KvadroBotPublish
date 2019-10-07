using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using KopterBot.Exception;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Chat
{
    class StopChat:BaseCommand 
    {
        public StopChat(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public override async Task Request(long chatid)
        {
            if (!await provider.hubService.IsChatActive(chatid))
            {
                await client.SendTextMessageAsync(chatid, "У вас нету активных соеденений");
                await ExceptionMessage.SendExceptionMessage(client, "Возможно ошибка с закрытием диалога");
            }
            long[] chatIds = await provider.hubService.GetChatId(chatid);
            await provider.hubService.StopChat(chatid);
            await client.SendTextMessageAsync(chatid, "Выберите один из вариантов",0,false,false,0,KeyBoardHandler.Murkup_Start_AfterChange());
            await client.SendTextMessageAsync(chatIds[0], "Выберите один из вариантов", 0, false, false, 0, KeyBoardHandler.Murkup_Start_AfterChange());
        }
    }
}

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
    class ChatCommands:BaseCommand 
    {
        public ChatCommands(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task ConfirmPilot(long chatid)
        {
            long[] chatIds = await provider.hubService.GetChatId(chatid);
            UserDTO user = await provider.userService.FindById(chatIds[1]);

            BuisnessTaskDTO task = await provider.buisnessTaskService.LastTaskForUser(user.ChatId);
            task.ChatIdPerformer = chatIds[0];
            await provider.buisnessTaskService.Update(task);

            await client.SendTextMessageAsync(chatid, "Пилот успешно утвержден");
            await client.SendTextMessageAsync(chatIds[1], "Вас утвердили на этот заказ");
        }

        public async Task EndChat(long chatid)
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

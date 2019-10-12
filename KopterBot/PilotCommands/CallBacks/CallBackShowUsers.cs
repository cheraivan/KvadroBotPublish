using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.PilotCommands.CallBacks
{
    class CallBackShowUsers:BaseCallback
    {
        public CallBackShowUsers(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async override Task SendCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;
            string message;
            if(callback.CallbackQuery.Data == "ShowUserNext")
            {
                UserDTO user = await provider.showUserService.GetNextUser(chatid);
                if(user == null)
                {
                    await client.SendTextMessageAsync(chatid, "Вы просматриваете последнего пользователя");
                    return;
                }
                int messageId = await provider.showUserService.GetMessageId(chatid);
                message = $"Пилот:{user.FIO} \n" +
                            $"Телефон:{user.Phone}";
                await client.EditMessageTextAsync(chatid, messageId+2, message, 0, false, (InlineKeyboardMarkup)KeyBoardHandler.CallBackShowForUser());
                return;
            }
            if(callback.CallbackQuery.Data == "ShowUserPrevious")
            {
                UserDTO user = await provider.showUserService.GetPreviousUser(chatid);
                if (user == null)
                {
                    await client.SendTextMessageAsync(chatid, "Вы просматриваете первого пользователя");
                    return;
                }
                int messageId = await provider.showUserService.GetMessageId(chatid);
                message = $"Пилот:{user.FIO} \n" +
                          $"Телефон:{user.Phone}";
                await client.EditMessageTextAsync(chatid, messageId + 2, message, 0, false, (InlineKeyboardMarkup)KeyBoardHandler.CallBackShowForUser());
                return;
            }
        }
    }
}

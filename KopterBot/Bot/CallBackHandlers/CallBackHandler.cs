using KopterBot.DTO;
using KopterBot.Interfaces;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace KopterBot.Bot
{
    class CallBackHandler:RepositoryProvider,ICallbackHandler
    {
        TelegramBotClient client;
        public CallBackHandler(TelegramBotClient client)
        {
            this.client = client;
        }

        #region PrivateHandlers
        private async Task CallBackHandler_Confirm(long chatid)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);
            await hubRepository.ConfirmDialog("Начать", hub.ChatIdCreater, chatid);
        }
        #endregion

        public async Task BaseCallBackHandler(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id; // receiver
            if (callback.CallbackQuery.Data == "confirm")
            {
                await CallBackHandler_Confirm(chatid);
                HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);
               
                long chatIdCreater = hub.ChatIdCreater;
                await client.SendTextMessageAsync(chatIdCreater, "Подключение установлено");
                await client.SendTextMessageAsync(chatid, "Подключение установлено");
                return;
            }
        }
    }
}

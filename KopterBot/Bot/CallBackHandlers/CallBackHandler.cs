using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using KopterBot.Interfaces;
using KopterBot.PilotCommands;
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
        MainProvider provider;
        CallBackOrders ordersCallback;
        public CallBackHandler(TelegramBotClient client,MainProvider provider)
        {
            this.provider = provider;
            this.client = client;
            ordersCallback = new CallBackOrders(client, provider);
        }

        #region PrivateHandlers
        private async Task CallBackHandler_Confirm(long chatid)
        {
            HubDTO hub = await hubRepository.Get().FirstOrDefaultAsync(i => i.ChatIdReceiver == chatid);
            //await hubRepository.ConfirmDialog("Начать", hub.ChatIdCreater, chatid);
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
            if(callback.CallbackQuery.Data == "Next" || callback.CallbackQuery.Data == "Back")
            {
                await ordersCallback.ShowOrdersCallBack(callback);
            }
        }
    }
}

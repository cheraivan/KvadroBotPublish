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
    class CallBackHandler:BaseHandler,ICallbackHandler
    {
        HubRepository hubRepository;
        public CallBackHandler(TelegramBotClient client,ApplicationContext context):base(client,context)
        {
            hubRepository = new HubRepository(db);
        }

        #region PrivateHandlers
        private async Task CallBackHandler_Confirm(long chatid)
        {
            HubDTO hub = await db.Hubs.Where(i=>i.ChatIdReceiver == chatid).FirstOrDefaultAsync();
            await hubRepository.ConfirmDialog("Начать", hub.ChatIdCreater, chatid);
        }
        #endregion

        public async Task BaseCallBackHandler(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id; // receiver
            if (callback.CallbackQuery.Data == "confirm")
            {
                await CallBackHandler_Confirm(chatid);
                HubDTO hub = await db.Hubs.Where(i => i.ChatIdReceiver == chatid).FirstOrDefaultAsync();
                long chatIdCreater = hub.ChatIdCreater;
                await client.SendTextMessageAsync(chatIdCreater, "Подключение установлено");
                await client.SendTextMessageAsync(chatid, "Подключение установлено");
                return;
            }
        }
    }
}

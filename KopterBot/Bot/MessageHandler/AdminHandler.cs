using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using KopterBot.Interfaces;
using KopterBot.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.Bot
{
    class AdminHandler:IMessageAdminHandler
    {
        TelegramBotClient client;
        MainProvider provider;
    
        public AdminHandler(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }
        public async Task BaseAdminMessageHandler(MessageEventArgs args)
        {
            long chatid = args.Message.Chat.Id;

            await client.SendTextMessageAsync(chatid, "тест");
        }
    }
}

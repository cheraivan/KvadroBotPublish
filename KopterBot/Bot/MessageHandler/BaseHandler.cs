using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace KopterBot.Bot
{
    class BaseHandler
    {
        protected TelegramBotClient client;
        protected ApplicationContext db;
        public BaseHandler(TelegramBotClient client,ApplicationContext db)
        {
            this.client = client;
            this.db = db;
        }
    }
}

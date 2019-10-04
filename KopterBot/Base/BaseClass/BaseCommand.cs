using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace KopterBot.Base.BaseClass
{
    class BaseCommand
    {
        protected TelegramBotClient client;
        protected MainProvider provider;

        public BaseCommand(TelegramBotClient client,MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }
    }
}

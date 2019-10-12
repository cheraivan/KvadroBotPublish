using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

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

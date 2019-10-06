using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.Base.BaseClass
{
    class BaseCallback
    {
        protected TelegramBotClient client;
        protected MainProvider provider;

        public BaseCallback(TelegramBotClient client,MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }
        
        public virtual Task SendCallBack(CallbackQueryEventArgs callback)
        {
            throw new Exception("This method has to be override");
        }
    }
}

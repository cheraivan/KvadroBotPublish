using KopterBot.Base.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace KopterBot.PilotCommands
{
    class FlyNow:BaseCommand 
    {
        public FlyNow(TelegramBotClient client, MainProvider provider) : base(client, provider) { }


    }
}

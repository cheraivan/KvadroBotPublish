using KopterBot.Base.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.BuisnessCommand.CallBacks
{
    class ShowMyOffersCallBack:BaseCommand 
    {
        public ShowMyOffersCallBack(TelegramBotClient client,MainProvider provider):base(client,provider) { }

        public async Task ShowOffersCallBack(CallbackQueryEventArgs callback)
        {
           
        }
    }
}

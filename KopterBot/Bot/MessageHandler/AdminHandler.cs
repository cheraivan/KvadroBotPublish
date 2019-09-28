using KopterBot.DTO;
using KopterBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.Bot
{
    class AdminHandler:BaseHandler,IBaseAdminHandler
    { 
        public AdminHandler(TelegramBotClient client, ApplicationContext db) : base(client, db) { }



        public async Task BaseAdminMessage(MessageEventArgs message)
        {

        }
    }
}

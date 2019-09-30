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
    class AdminHandler:RepositoryProvider
    { 
        public AdminHandler(TelegramBotClient client) { }

        public async Task BaseAdminMessage(MessageEventArgs message)
        {
            
        }
    }
}

﻿using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using KopterBot.Interfaces;
using KopterBot.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.InputFiles;

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
            string message = args.Message.Text;

            if(message == "/unop")
            {
                await provider.adminService.ChangeWish(chatid);
                await client.SendTextMessageAsync(chatid,"Выберите один из следующих вариантов",0,false,false,0,KeyBoardHandler.Murkup_Start_AfterChange());
                return;
            }

            if(message == "Модерирование чатов")
            {

            }
        }
    }
}

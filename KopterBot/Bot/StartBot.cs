﻿using KopterBot.Commons;
using KopterBot.DTO;
using KopterBot.Interfaces;
using KopterBot.Interfaces.Bot;
using KopterBot.Logs;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.Bot
{
    class StartBot
    {
        ApplicationContext context = new ApplicationContext();
        TelegramBotClient client;
        public void BotRun()
        {
            context.Database.EnsureCreated();
            Services.ServiceProvider provider = new Services.ServiceProvider();
            client = new TelegramBotClient(Constant.Token);
            client.StartReceiving();

            List<string> commandList = CommandList.GetCommands();
            var scope = new ServiceCollection().AddScoped<IMessageHandler, MessageHandler>(x => new MessageHandler(client,provider))
                .AddScoped<ICallbackHandler,CallBackHandler>(i=>new CallBackHandler(client)).BuildServiceProvider();

            client.OnCallbackQuery += async (object sender, CallbackQueryEventArgs args) =>
            {
                var callbackHandler = scope.GetService<ICallbackHandler>();
                await callbackHandler.BaseCallBackHandler(args);
            };

            client.OnMessage += async (object sender, MessageEventArgs args) =>
            {
                var handler = scope.GetService<IMessageHandler>();
                await handler.BaseHandlerMessage(args, args.Message.Text);
            };
        }
    }
}

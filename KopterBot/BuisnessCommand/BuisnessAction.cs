﻿using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using KopterBot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.BuisnessCommand
{
    class BuisnessAction
    {
        TelegramBotClient client;
        MainProvider provider;
        public BuisnessAction(MainProvider provider,TelegramBotClient client)
        {
            this.provider = provider;
            this.client = client;
        }
        public async Task CreateTask(long chatid,string message,MessageEventArgs messageObject)
        {
            bool isUserBuisnessman = await provider.buisnessTaskService.IsUserBuisnessman(chatid);
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            BuisnessTaskDTO currTask = await provider.buisnessTaskService.FindTask(chatid);
            if(!isUserBuisnessman)
            {
                await client.SendTextMessageAsync(chatid, "Вам нужно зарегистрироваться как заказчик чтобы вы могли оставлять заказы");
                return;
            }
            if(currentStep == 1)
            {
                BuisnessTaskDTO newTask = new BuisnessTaskDTO()
                {
                    ChatId = chatid,
                    Region = message
                };
                await provider.buisnessTaskService.Create(newTask);
                await client.SendTextMessageAsync(chatid, "Введите описание вашего задания");
                await provider.userService.ChangeAction(chatid, "Создание задачи", ++currentStep);
                return;
            }
            if(currentStep == 2)
            {
                currTask.Description = message;
                await provider.buisnessTaskService.Update(currTask);
                await provider.userService.ChangeAction();
                await client.SendTextMessageAsync(chatid, "Введите примерную сумму которую вы готовы потратить");
                return;
            }
        }
    }
}
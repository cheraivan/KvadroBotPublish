﻿using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.PilotCommands
{
    class ShowOrders:BaseCommand
    {
        public ShowOrders(TelegramBotClient client,MainProvider provider) : base(client, provider) { }


        public async Task ShowAllOrders(long chatid, MessageEventArgs messageObject)
        {
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            int countTask = await provider.buisnessTaskService.CountTask();

            if (countTask == 0)
            {
                // отправлять 
                await client.SendTextMessageAsync(chatid, "Еще нету созданных задач");
                await provider.userService.ChangeAction(chatid, "NULL", 0);
                return;
            }
            BuisnessTaskDTO task = await provider.buisnessTaskService.GetFirstElement();
            string message = $"Заявка номер: {task.Id} " +
                $"Регион: {task.Region} " +
                $"Описание: {task.Description} " +
                $"Сумма: {task.Sum}";
            await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyBoardHandler.CallBackShowOrders());
        }
    }
    class CallBackOrders : ShowOrders
    {
        public CallBackOrders(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task ShowOrdersCallBack(CallbackQueryEventArgs callback)
        {

        }
    }
}

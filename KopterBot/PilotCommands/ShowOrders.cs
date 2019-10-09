using KopterBot.Base.BaseClass;
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


        public async Task ShowAllOrders(long chatid, MessageEventArgs messageObject, bool isBuisnessman = false)
        {
            int countTask;
            BuisnessTaskDTO task;
            string message;
            // для бизнесменов 
            if (isBuisnessman)
            {
                countTask = await provider.buisnessTaskService.CountTask(chatid);
                if(countTask == 0)
                {
                    await client.SendTextMessageAsync(chatid,"Вы не создали ниодной задачи");
                }
                task = await provider.buisnessTaskService.GetFirstElement(chatid);

                message = $"Заявка номер: {task.Id} \n" +
                   $"Регион: {task.Region} \n" +
                   $"Описание: {task.Description} \n" +
                   $"Сумма: {task.Sum}";

  
                await provider.showOrderService.SetDefaultProduct(chatid,true);
                await provider.showOrderService.ChangeMessageId(chatid, messageObject.Message.MessageId);
                await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyBoardHandler.CallBackShowOrdersForBuisnessman());
                return;
            }
            countTask = await provider.buisnessTaskService.CountTask();

            if (countTask == 0)
            {
                // отправлять 
                await client.SendTextMessageAsync(chatid, "Еще нету созданных задач");
                await provider.userService.ChangeAction(chatid, "NULL", 0);
                return;
            }
            task = await provider.buisnessTaskService.GetFirstElement();
            message = $"Заявка номер: {task.Id} \n" +
               $"Регион: {task.Region} \n" +
               $"Описание: {task.Description} \n" +
               $"Сумма: {task.Sum}";

            
            await provider.showOrderService.SetDefaultProduct(chatid);
            await provider.showOrderService.ChangeMessageId(chatid, messageObject.Message.MessageId);
            await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyBoardHandler.CallBackShowOrders());
        }
    }
}

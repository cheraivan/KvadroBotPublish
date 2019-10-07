using KopterBot.Base.BaseClass;
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
    class CreateBuisnessTask
    {
        TelegramBotClient client;
        MainProvider provider;
        public CreateBuisnessTask(MainProvider provider,TelegramBotClient client)
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
                await provider.userService.ChangeAction(chatid, "Создать новую задачу", ++currentStep);
                return;
            }
            if(currentStep == 2)
            {
                currTask.Description = message;
                await provider.buisnessTaskService.Update(currTask);
                await provider.userService.ChangeAction(chatid,"Создать новую задачу",++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите примерную сумму которую вы готовы потратить");
                return;
            }
            if(currentStep==3)
            {
                currTask.Sum = Convert.ToInt32(message);
                await provider.buisnessTaskService.Update(currTask);
                await client.SendTextMessageAsync(chatid, "Задача успешно создана");
                return;
            }
        }
    }
}

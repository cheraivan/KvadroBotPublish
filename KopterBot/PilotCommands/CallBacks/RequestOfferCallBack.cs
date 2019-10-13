using KopterBot.Base.BaseClass;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.PilotCommands
{
    class RequestOfferCallBack:BaseCommand 
    {
        public RequestOfferCallBack(TelegramBotClient client ,MainProvider provider):base(client,provider) { }

        public async Task SendRequest(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;

            ShowOrdersDTO order = await provider.showOrderService.CurrentProduct(chatid);

            List<int> idProducts = await provider.showOrderService.GetIdTasksForUser(chatid);
            if(idProducts.Contains(order.CurrentProductId))
            {
                await client.SendTextMessageAsync(chatid, "Нельзя брать заказ у самого себя");
                return;
            }

            BuisnessTaskDTO task = await provider.buisnessTaskService.FindTaskByTaskId(order.CurrentProductId);

            if (task.ChatIdPerformer.HasValue)
            {
                await client.SendTextMessageAsync(chatid, "К сожалению,этот заказ уже выполняется другим пилотом");
                return;
            }

            OfferDTO offer = new OfferDTO
            {
                ChatId = chatid,
                TaskId = task.Id
            };

            UserDTO user = await provider.userService.FindById(chatid);



            await provider.offerService.Create(chatid,offer);

            string message = $"Пилот {user.FIO} хочет выполнить ваш заказ. " +
                $"Данные по заказу:\n " +
                $"Id заказа:{task.Id} \n " +
                $"Описание заказа:{task.Description} \n " +
                $"Сумма заказа:{task.Sum} \n " +
                $"Подробнее можете посмотреть в разделе просмотра своих заявок";

            await client.SendTextMessageAsync(task.ChatId, message);

            await client.SendTextMessageAsync(chatid, "Заявка успеешно создана");
        }
    }
}

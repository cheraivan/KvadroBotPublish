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

            ShowOrdersDTO order = await provider.showOrderService.CurrentProductId(chatid);

            List<int> idProducts = await provider.showOrderService.GetIdTasksForUser(chatid);
            if(idProducts.Contains(order.CurrentProductId))
            {
                await client.SendTextMessageAsync(chatid, "Нельзя брать заказ у самого себя");
                return;
            }

            BuisnessTaskDTO task = await provider.buisnessTaskService.FindTaskByTaskId(order.CurrentProductId);

             
            OfferDTO offer = new OfferDTO
            {
                ChatId = chatid,
                TaskId = task.Id
            };

            await provider.offerService.Create(chatid,offer);
            await client.SendTextMessageAsync(chatid, "Заявка успеешно создана");
        }
    }
}

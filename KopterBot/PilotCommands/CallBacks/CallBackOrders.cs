using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.PilotCommands
{
    class CallBackOrders : ShowOrders
    {
        public CallBackOrders(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task ShowOrdersCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;
            if(callback.CallbackQuery.Data == "Next")
            {
                BuisnessTaskDTO task = await provider.showOrderService.GetNextProduct(chatid);
                if(task == null)
                {
                    await client.SendTextMessageAsync(chatid, "Это была последняя задача");
                    return;
                }
                int messageId = await provider.showOrderService.GetMessageId(chatid);
                string message = $"Заявка номер: {task.Id} \n" +
                   $"Регион: {task.Region} \n" +
                   $"Описание: {task.Description} \n" +
                   $"Сумма: {task.Sum}";
                await client.EditMessageTextAsync(chatid, messageId+1, message,0,false,(InlineKeyboardMarkup)KeyBoardHandler.CallBackShowOrders());
            }
            if(callback.CallbackQuery.Data == "Back")
            {
                BuisnessTaskDTO task = await provider.showOrderService.GetPreviousProduct(chatid);
                if(task == null)
                {
                    await client.SendTextMessageAsync(chatid, "Это первая задача");
                }
                int messageId = await provider.showOrderService.GetMessageId(chatid);
                string message = $"Заявка номер: {task.Id} \n" +
                   $"Регион: {task.Region} \n" +
                   $"Описание: {task.Description} \n" +
                   $"Сумма: {task.Sum}";
                await client.EditMessageTextAsync(chatid, messageId+1, message, 0, false, (InlineKeyboardMarkup)KeyBoardHandler.CallBackShowOrders());
            }
        }
    }
}


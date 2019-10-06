using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.Chat.CallBack
{
    class StartDialogCallBack:BaseCallback
    {
        public StartDialogCallBack(TelegramBotClient client,MainProvider provider) : base(client, provider)
        {
        }
        public async override Task SendCallBack(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;
            string message = callback.CallbackQuery.Message.Text;

            string id = String.Empty;
            int index = message.IndexOf(":") + 1;

            for(int i = index; i <index+10; i ++)
            {
                id += message[i];
            }
            
            long chatIdReceiver;
            if (!long.TryParse(id, out chatIdReceiver))
                throw new Exception("Incorrect parse");

            UserDTO user = await provider.userService.FindById(chatid);

            BuisnessTaskDTO task = await provider.buisnessTaskService.GetCurrentTask(chatid);
            
            // проверка пилот в диалоге

            string messageAnswer = $"{user.FIO} хочет с вами связаться \n " +
                $"Заявка в регионе {task.Region} \n" +
                $"Описание заявки: {task.Description} ";
            // попытка установки соеденения 
            await provider.hubService.CreateDialog(chatid, chatIdReceiver);
            await client.SendTextMessageAsync(chatIdReceiver, messageAnswer, 0, false, false, 0,KeyBoardHandler.ChatConfirm());
        }

        public async Task StartCommenication(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id;

            long[] chatIds = await provider.hubService.GetChatId(chatid);

            if (chatIds.Length == 0)
                throw new Exception("Dialog is incorrect");

            await provider.hubService.ConfirmDialog(chatid,chatIds[1],true);

            await client.SendTextMessageAsync(chatIds[1], "Подключение установлено", 0, false, false, 0, KeyBoardHandler.EndDialog());
            await client.SendTextMessageAsync(chatid, "Подключение установлено", 0, false, false, 0, KeyBoardHandler.EndDialog());
        }

    }
}

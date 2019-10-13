using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.Payment
{
    class AddCash:BaseCommand
    {
        public AddCash(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task ReplenishAccount(MessageEventArgs messageObject)
        {
            string messageText = messageObject.Message.Text;
            long chatId = messageObject.Message.Chat.Id;

            int currStep = await provider.userService.GetCurrentActionStep(chatId);

            if(currStep == 1)
            {
                string cart = EasyPay.GetCart();
                await client.SendTextMessageAsync(chatId,$"Внесите оплату на кошелек:{cart}\nПосле этого введите ваш номер чека");
                await provider.userService.ChangeAction(chatId, "Пополнить баланс", ++currStep);
                return;
            }

            if(currStep == 2)
            {
                Regex isPayCorrect = new Regex(@"/^(\d){1,13}$/g");
                if(isPayCorrect.IsMatch(messageText))
                {
                    int? sum = await EasyPay.IsPayCorrect(messageText);
                    if(sum==null)
                    {
                        await client.SendTextMessageAsync(chatId, "Неправильный кошелек", 0, false, false, 0, KeyBoardHandler.Murkup_BuisnessmanMenu());
                        await provider.userService.ChangeAction(chatId, "NULL", 0);
                        return;
                    }
                    UserDTO user = await provider.userService.FindById(chatId);
                    user.balance = sum;
                    await provider.userService.Update(user);
                    await client.SendTextMessageAsync(chatId, "Вы успешно пополнили баланс", 0, false, false, 0, KeyBoardHandler.Murkup_BuisnessmanMenu());
                }
                else
                {
                    await client.SendTextMessageAsync(chatId, "Вы ввели некорректный формат чека",0,false,false,0,KeyBoardHandler.Murkup_BuisnessmanMenu());
                    await provider.userService.ChangeAction(chatId, "NULL", 0);
                    return;
                }
            }
        }
    }
}

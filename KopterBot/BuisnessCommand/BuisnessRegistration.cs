using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.Commons;
using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.BuisnessCommand
{
    class BuisnessRegistration:BaseCommand
    {
        public BuisnessRegistration(TelegramBotClient client,MainProvider provider):base(client,provider) { }

        private async Task CommandHandler_BuisnessRegistrationKorporativ(long chatid, string message, MessageEventArgs messageObject)
        {
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);

            UserDTO user = await provider.userService.FindById(chatid);

            DronDTO dron = new DronDTO();

            if (currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Корпоративная бизнесс-регистрация", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите номер телефона");
                return;
            }

            if (currentStep == 2)
            {
                if (RegularExpression.IsTelephoneCorrect(message))
                {
                    user.Phone = message;
                    user.BuisnesPrivilag = 1;
                    await provider.userService.Update(user);
                    await provider.userService.ChangeAction(chatid, "NULL", 0);
                    await client.SendTextMessageAsync(chatid, "Вы успешно зарегистрировались", 0, false, false, 0, KeyBoardHandler.Murkup_BuisnessmanMenu());
                    await provider.managerPush.SendMessage(client, chatid);
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы ввели некорректный телефон,попробуйте еще раз");
                }
            }
        }

    }
}

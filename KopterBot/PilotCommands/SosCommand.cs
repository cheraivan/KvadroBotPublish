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
    class SosCommand : BaseCommand
    {
        public SosCommand(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        public async Task SosHandler(MessageEventArgs messageObject)
        {
            long chatid = messageObject.Message.Chat.Id;
            string message = messageObject.Message.Text;

            int currStep = await provider.userService.GetCurrentActionStep(chatid);
            SosDTO sos = await provider.sosTableServide.FindById(chatid);
            if(currStep == 0)
            {
                await provider.userService.ChangeAction(chatid, "SOS", ++currStep);
                return;
            }
            if (currStep == 1)
            {
                if (message == "Страховой случай" || message == "Аварийный случай")
                {
                    if (message == "Страховой случай")
                    {
                        await client.SendTextMessageAsync(chatid, "Не указано откуда брать данные и куда их слать");
                        return;
                    }
                    else if (message == "Аварийный случай")
                    {
                        await provider.sosTableServide.Create(new SosDTO
                        {
                            ChatId = chatid,
                            Type = 1
                        });
                        await client.SendTextMessageAsync(chatid, "Сбросьте вашу геолокацию",0,false,false,0,KeyBoardHandler.Markup_Back_From_First_Action());
                        await provider.userService.ChangeAction(chatid, "SOS", ++currStep);
                        return;
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы выбрали несуществующий вариант");
                }
            }
            if (currStep == 2)
            {
                if (sos.Type == 1)
                {
                    if (messageObject.Message.Location != null)
                    {
                        sos.lautitude = messageObject.Message.Location.Latitude;
                        sos.longtitude = messageObject.Message.Location.Longitude;
                        await provider.sosTableServide.Update(sos);
                        List<long> lstId = await provider.userService.GetUsersIdByRegion(chatid);

                        UserDTO user = await provider.userService.FindById(chatid);

                        string _message = $"У пилота {user.ChatId} проблемы \n" +
                            $"свяжитесь с ним по телефону {user.Phone} \n " +
                            $"Могу добавить еще описание проблемы";

                        foreach (var i in lstId)
                        {
                            await client.SendTextMessageAsync(i, _message);
                            await client.SendLocationAsync(chatid, sos.lautitude, sos.longtitude);
                        }
                        await provider.userService.ChangeAction(chatid, "NULL", 0);
                        await client.SendTextMessageAsync(chatid, "Мы разослали письмо о помощи нашем партнерам");
                        return;
                    }
                }
            }
        }
    }
}


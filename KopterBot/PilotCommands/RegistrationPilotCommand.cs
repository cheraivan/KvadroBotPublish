using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.Commons;
using KopterBot.DTO;
using KopterBot.Geolocate;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace KopterBot.PilotCommands
{
    class RegistrationPilotCommand:BaseCommand
    {
        public RegistrationPilotCommand(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task CommandHandler_PaidRegistrationWithoutInsurance(UserDTO user, string message, MessageEventArgs messageObject)
        {
            long chatid = user.ChatId;
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await provider.proposalService.FindById(chatid);

            if (currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите мобильный телефон");
                return;
            }
            if (currentStep == 2)
            {
                if (RegularExpression.IsTelephoneCorrect(message))
                {
                    user.Phone = message;
                    await provider.userService.Update(user);
                    await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", ++currentStep);
                    await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы ввели некорректный телефон.Попробуйте еще раз");
                }
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await provider.dronService.Create(dron);
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите адрес");
                return;
            }
            if (currentStep == 4)
            {
                await provider.proposalService.Create(user);
                proposal.Adress = message;
                await provider.proposalService.Update(proposal);
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Сбросьте вашу геолокацию");
                return;
            }
            if (currentStep == 5)
            {
                if (messageObject.Message.Location != null)
                {
                    proposal.longtitude = messageObject.Message.Location.Longitude;
                    proposal.latitude = messageObject.Message.Location.Latitude;
                    string realAdres = await GeolocateHandler.GetAddressFromCordinat(proposal.longtitude, proposal.latitude);
                    proposal.Region = GetGeolocateRegion.GetRegion(realAdres);
                    proposal.RealAdress = realAdres;
                    await provider.proposalService.Update(proposal);
                    await provider.proposeHandler.ChangeProposeCount();
                    user.PilotPrivilag = 1;
                    await provider.userService.Update(user);
                    await provider.adminPush.MessageAboutRegistrationPilot(client, provider, chatid);
                }
            }
        }
        public async Task CommandHandler_PaidRegistrationWithInsurance(UserDTO user, string message, MessageEventArgs messageObject = null)
        {
            long chatid = user.ChatId;
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await provider.proposalService.FindById(chatid);

            if (currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите телефон");
                return;
            }

            if (currentStep == 2)
            {
                if (RegularExpression.IsTelephoneCorrect(message))
                {
                    await provider.proposalService.Create(user);
                    user.Phone = message;
                    await provider.userService.Update(user);
                    await provider.userService.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                    await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы ввели некорректный телефон,попробуйте еще раз");
                    return;
                }
            }
            if (currentStep == 3)
            {
                dron.Mark = message;
                await provider.dronService.Create(dron);

                await provider.userService.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите тип страховки");
                return;
            }
            if (currentStep == 4)
            {
                proposal.TypeOfInsurance = message;
                await provider.proposalService.Update(proposal);
                await provider.userService.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите адрес");
                return;
            }
            if (currentStep == 5)
            {
                proposal.Adress = message;
                await provider.proposalService.Update(proposal);
                await provider.userService.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Сбросьте вашу геопозицию");
                return;
            }
            if (currentStep == 6)
            {
                if (messageObject.Message.Location != null)
                {
                    proposal.longtitude = messageObject.Message.Location.Longitude;
                    proposal.latitude = messageObject.Message.Location.Latitude;
                    string realAdres = await GeolocateHandler.GetAddressFromCordinat(proposal.longtitude, proposal.latitude);
                    proposal.Region = GetGeolocateRegion.GetRegion(realAdres);
                    proposal.RealAdress = realAdres;
                    await provider.proposalService.Update(proposal);
                    await client.SendTextMessageAsync(chatid, "Ожидаем оплату,если все нормально - кидаем клаву с этими кнопками и если все оплатил кидаем в админ-уведомление"
                        , 0, false, false, 0, KeyBoardHandler.PilotWithSubscribe_Murkup());

                    await provider.proposeHandler.ChangeProposeCount();
                    user.PilotPrivilag = 2;
                    await provider.userService.Update(user);
                    await provider.adminPush.MessageAboutRegistrationPilot(client, provider, chatid);
                    await provider.userService.ChangeAction(chatid, "NULL", 0);
                    // можно считать человека зарегистрированым только после оплаты , и определяем насколько он крут в плане полномочий
                    string region = GetGeolocateRegion.GetRegion(realAdres);
                    await provider.regionService.Create(region);
                    await client.SendTextMessageAsync(chatid, "Вы успешно зарегистрировались");
                    return;
                }
            }

        }
    }
}

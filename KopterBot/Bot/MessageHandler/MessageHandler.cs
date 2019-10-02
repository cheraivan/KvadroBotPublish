using KopterBot.DTO;
using KopterBot.Interfaces.Bot;
using KopterBot.Logs;
using KopterBot.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
using KopterBot.Commons;
using System.Xml;
using System.Linq;
using KopterBot.Geolocate;
using Microsoft.EntityFrameworkCore;
using KopterBot.Bot.CommonHandler;
using KopterBot.Base.BaseClass;
using KopterBot.Services;

namespace KopterBot.Bot
{
    /// <summary>
    /// ВАЖНО!!!!!!! ПОСЛЕ ПЕРВОЙ РЕГИСТРАЦИИ В КАЖДОМ РЕЖИМЕ НУЖНО ОГРАНИЧИВАТЬ ВОЗМОЖНОСТЬ РЕГИСТРИРОВАТЬСЯ
    /// </summary>
    class MessageHandler: IMessageHandler
    {
        long chatid;
        TelegramBotClient client;
        MainProvider provider;

        public MessageHandler(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }

        #region PrivateHandlers
        #region BuisnessRegistration
        private async Task CommandHandler_BuisnessRegistrationKorporativ(long chatid,string message,MessageEventArgs messageObject)
        {
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);

            UserDTO user = await provider.userService.FindById(chatid);

            DronDTO dron = new DronDTO();

            if(currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Корпоративная бизнесс-регистрация", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите номер телефона");
                return;
            }

            if(currentStep == 2)
            {
                user.Phone = message;
                user.BuisnesPrivilag = 1;
                await provider.userService.Update(user);
                await client.SendTextMessageAsync(chatid, "Вы успешно зарегистрировались");
                await provider.managerPush.SendMessage(client, chatid);
                return;
            }
        }
        #endregion
        private async Task CommandHandler_Start(long chatid)
        {
            await client.SendTextMessageAsync(chatid, "Вводный текст в бота,тут чет придумаем", 0, false, false, 0,KeyBoardHandler.Murkup_Start_AfterChange());
        }
        #region PaymantRegistration
        private async Task CommandHandler_PaidRegistrationWithoutInsurance(UserDTO user,string message,MessageEventArgs messageObject)
        {
            long chatid = user.ChatId;
            int currentStep = await provider.userService.GetCurrentActionStep(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await provider.proposalService.FindById(chatid);
            if(currentStep == 1)
            {
                user.FIO = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки",++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите мобильный телефон");
                return;
            }
            if(currentStep == 2)
            {
                user.Phone = message;
                await provider.userService.Update(user);
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                return;
            }
            if(currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.Create(dron);
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите адрес");
                return;
            }
            if(currentStep == 4)
            {
                await proposalRepository.Create(user);
                proposal.Adress = message;
                await provider.proposalService.Update(proposal);
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Сбросьте вашу геолокацию");
                return;
            }
            if(currentStep == 5)
            {
                if (messageObject.Message.Location != null)
                {
                    proposal.longtitude = messageObject.Message.Location.Longitude;
                    proposal.latitude = messageObject.Message.Location.Latitude;
                    string realAdres = await GeolocateHandler.GetAddressFromCordinat(proposal.longtitude, proposal.latitude);
                    proposal.RealAdress = realAdres;
                    await proposalRepository.Update(proposal);
                    await proposeHandler.ChangeProposeCount();
                    user.PilotPrivilag = 1;
                    await userRepository.Update(user);
                    await adminPush.MessageRequisitionAsync(client, chatid);
                }
            }
        }
        private async Task CommandHandler_PaidRegistrationWithInsurance(UserDTO user,string message,MessageEventArgs messageObject = null)
        {
            long chatid = user.ChatId;
            int currentStep =await provider.userService.GetCurrentActionStep(chatid);
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
    
            if(currentStep == 2)
            {
                await proposalRepository.Create(user);
                user.Phone = message;
                await userRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                return;
            }
            if(currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.Create(dron);
                
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Введите тип страховки");
                return;
            }
            if(currentStep == 4)
            {
                proposal.TypeOfInsurance = message;
                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid,"Введите адрес");
                return;
            }
            if(currentStep == 5)
            {
                proposal.Adress = message;
                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", ++currentStep);
                await client.SendTextMessageAsync(chatid, "Сбросьте вашу геопозицию");
                return;
            }
            if(currentStep == 6)
            {
                if(messageObject.Message.Location!=null)
                {
                    proposal.longtitude = messageObject.Message.Location.Longitude;
                    proposal.latitude = messageObject.Message.Location.Latitude;
                    string realAdres = await GeolocateHandler.GetAddressFromCordinat(proposal.longtitude, proposal.latitude);
                    proposal.RealAdress = realAdres;
                    await proposalRepository.Update(proposal);
                    await client.SendTextMessageAsync(chatid, "Ожидаем оплату,если все нормально - кидаем клаву с этими кнопками и если все оплатил кидаем в админ-уведомление"
                        , 0, false, false, 0,KeyBoardHandler.PilotWithSubscribe_Murkup());

                    await proposeHandler.ChangeProposeCount();
                    user.PilotPrivilag = 2;
                    await userRepository.Update(user);
                    await adminPush.MessageRequisitionAsync(client,chatid);
                    // можно считать человека зарегистрированым только после оплаты , и определяем насколько он крут в плане полномочий
                    await client.SendTextMessageAsync(chatid, "Вы успешно зарегистрировались");
                }
            }

        }
        #endregion
        #endregion

        public async Task BaseHandlerMessage(MessageEventArgs message,string text)
        {
            Console.WriteLine($"Сообщение прислал : {chatid}\nТекст:{message.Message.Text}\n");
            chatid = message.Message.Chat.Id;
            string messageText = message.Message.Text;
            string action = await provider.userService.GetCurrentActionName(chatid);
            UserDTO user = await provider.userService.FindById(chatid);

            await provider.userService.AuthenticateUser(chatid);
            await UserLogs.WriteLog(chatid, messageText);

            bool isRegistration = await provider.userService.IsUserRegistration(chatid);
            if (messageText == "Назад")
            {
                await provider.userService.ChangeAction(chatid, "NULL", 0);
                await CommandHandler_Start(chatid);

                return;
            }
            if (messageText == "/start")
            {
                await CommandHandler_Start(chatid);
                return;
            }
            if(CommandList.RegistrationPilotCommandList().Contains(messageText) && user.PilotPrivilag!=0)
            {
                await client.SendTextMessageAsync(chatid, "Вы уже зарегестрированы", 0, false, false, 0, KeyBoardHandler.ChangeKeyBoardPilot(user.PilotPrivilag));
                return;
            }
            if(CommandList.RegistrationBuisnessCommandList().Contains(messageText) && user.BuisnesPrivilag!=0)
            {
                await client.SendTextMessageAsync(chatid, "Вы уже зарегестрированы", 0, false, false, 0, KeyBoardHandler.Murkup_BuisnessmanMenu());
                return;
            }
            if (messageText == "Пилот")
            {
                if (user.PilotPrivilag == 0)
                {
                    await client.SendTextMessageAsync(chatid, "Вы зашли как Пилот"
                        , 0, false, false, 0, KeyBoardHandler.Murkup_Start_Pilot_Mode());
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы зашли как пилот",
                        0, false, false, 0, KeyBoardHandler.ChangeKeyBoardPilot(user.PilotPrivilag));
                }
            }
            if (messageText == "Полный функционал платно")
            {
                await client.SendTextMessageAsync(chatid, "Есть несколько вариантов регистрации", 0, false, false, 0, KeyBoardHandler.Markup_Start_Pilot_Payment_Mode());
            }
            if (messageText == "Частичные возможности бесплатно")
            {
                await client.SendTextMessageAsync(chatid, "Есть несоклько вариантов регистрации",
                    0, false, false, 0, KeyBoardHandler.Murkup_Start_Pilot_UnBuyer_Mode());
            }
            #region Платная регистрация для пилота
            if (messageText == "Со страхованием")
            {
                await provider.userService.ChangeAction(chatid, "Co страхованием", 1);
                await client.SendTextMessageAsync(chatid, "Введите ФИО", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
                return;
            }
            if (messageText == "Без страховки")
            {
                await provider.userService.ChangeAction(chatid, "Без страховки", 1);
                await client.SendTextMessageAsync(chatid, "Введите ФИО", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
            }
            if (messageText == "Платная регистрация со страховкой")
            {
                await provider.userService.ChangeAction(chatid, "Платная регистрация со страховкой", 1);
                await client.SendTextMessageAsync(chatid, "Введите ФИО", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
                return;
            }
            if (messageText == "Платная регистрация без страховки")
            {
                await provider.userService.ChangeAction(chatid, "Платная регистрация без страховки", 1);
                await client.SendTextMessageAsync(chatid, "Введите ФИО", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
                return;
            }

            #endregion
            #region бесплатная регистрация для пилота 

            #endregion

            #region Бизнес-Регистрация
            if (messageText == "Корпоративный")
            {
                await provider.userService.ChangeAction(chatid, "Корпоративная бизнесс-регистрация", 1);
                await client.SendTextMessageAsync(chatid,"Введите ФИО",0,false,false,0,KeyBoardHandler.Markup_Back_From_First_Action());

                return;
            }
            if(messageText == "Индивидуальный")
            {

            }
            #endregion

            if (messageText == "Заказчик услуг")
            {
                if (user.BuisnesPrivilag == 0)
                {
                    await client.SendTextMessageAsync(chatid, "Есть несколько вариантов регистрации",
                        0, false, false, 0, KeyBoardHandler.Murkup_Start_Buisness_Mode());
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы уже зарегистрированы",
                        0, false, false,0,KeyBoardHandler.Murkup_BuisnessmanMenu());
                }
                return;
            }
            if (action!=null)
            {
                if(action == "Платная регистрация со страховкой")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(user,messageText,message);
                    return;
                }
                if(action == "Платная регистрация без страховки")
                {
                    await CommandHandler_PaidRegistrationWithoutInsurance(user, messageText, message);
                    return;
                }
                if(action == "Со страхованием")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(user, messageText, message);
                    return;
                }
                if(action == "Без страховки")
                {
                    await CommandHandler_PaidRegistrationWithoutInsurance(user, messageText, message);
                    return;
                }
                if(action == "Корпоративная бизнесс-регистрация")
                {
                    await CommandHandler_BuisnessRegistrationKorporativ(chatid, messageText, message);
                    return;
                }
            }
        }
    }
}
 
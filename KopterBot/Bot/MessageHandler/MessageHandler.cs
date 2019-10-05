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
using KopterBot.BuisnessCommand;
using KopterBot.PilotCommands;

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

        BuisnessAction buisnessAction;
        RegistrationPilotCommand registrationPilotsCommand;
        ShowOrders showOrders;

        public MessageHandler(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
            buisnessAction = new BuisnessAction(provider, client);
            registrationPilotsCommand = new RegistrationPilotCommand(client,provider);
            showOrders = new ShowOrders(client, provider);
        }
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

        public async Task BaseHandlerMessage(MessageEventArgs message,string text)
        {
            Console.WriteLine($"Сообщение прислал : {chatid}\nТекст:{message.Message.Text}\n");
            chatid = message.Message.Chat.Id;
            string messageText = message.Message.Text;
            string action = await provider.userService.GetCurrentActionName(chatid);
            UserDTO user = await provider.userService.FindById(chatid);

            await provider.userService.AuthenticateUser(chatid);
          //  await UserLogs.WriteLog(chatid, messageText);

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
            if(messageText == "Частный клиент")
            {
                await provider.userService.ChangeAction(chatid, "Корпоративная бизнесс-регистрация", 1);
                await client.SendTextMessageAsync(chatid, "Введите ФИО", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
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

            if(messageText == "Создать новую задачу")
            {
                await provider.userService.ChangeAction(chatid, "Создать новую задачу", 1);
                await client.SendTextMessageAsync(chatid,"Введите регион");
            }

            if(messageText == "Просмотр заказов")
            {
                await showOrders.ShowAllOrders(chatid, message);
            }
            if(messageText == "Просмотреть свои заказы")
            {
                await showOrders.ShowAllOrders(chatid, message, true);
            }
           
            if (action!=null)
            {
                if(action == "Платная регистрация со страховкой")
                {
                    await registrationPilotsCommand.CommandHandler_PaidRegistrationWithInsurance(user,messageText,message);
                    return;
                }
                if(action == "Платная регистрация без страховки")
                {
                    await registrationPilotsCommand.CommandHandler_PaidRegistrationWithoutInsurance(user, messageText, message);
                    return;
                }
                if(action == "Со страхованием")
                {
                    await registrationPilotsCommand.CommandHandler_PaidRegistrationWithInsurance(user, messageText, message);
                    return;
                }
                if(action == "Без страховки")
                {
                    await registrationPilotsCommand.CommandHandler_PaidRegistrationWithoutInsurance(user, messageText, message);
                    return;
                }
                if(action == "Корпоративная бизнесс-регистрация")
                {
                    await CommandHandler_BuisnessRegistrationKorporativ(chatid, messageText, message);
                    return;
                }
                if(action == "Создать новую задачу")
                {
                    await buisnessAction.CreateTask(chatid, messageText, message);
                    return;
                }
            }
        }
    }
}

﻿using KopterBot.DTO;
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

namespace KopterBot.Bot
{
    class MessageHandler:IMessageHandler
    {
        long chatid;
        TelegramBotClient client;
        ApplicationContext db;
        #region repository
        UserRepository userRepository;
        GenericRepository<UserDTO> genericUserRepository;
        DronRepository dronRepository;
        BotRepository botRepository;
        HubRepository hubRepository;
        AdminRepository adminRepository;
        #endregion
        public MessageHandler(TelegramBotClient client,ApplicationContext context)
        {
            this.client = client;
            db = context;
            userRepository = new UserRepository();
            genericUserRepository = new GenericRepository<UserDTO>(db);
            dronRepository = new DronRepository();
            botRepository = new BotRepository();
            hubRepository = new HubRepository();
            adminRepository = new AdminRepository();
        }

        #region PrivateHandlers
        private async Task CommandHandler_Start(long chatid)
        {
            await client.SendTextMessageAsync(chatid, "Вводный текст в бота,тут чет придумаем", 0, false, false, 0,KeyBoardHandler.Markup_Start());
        }
        private async Task CommandHandler_PaidRegistrationWithInsurance(long chatid,string message,MessageEventArgs messageObject = null)
        {
            int currentStep = userRepository.GetCurrentActionStep(chatid);
            UserDTO user = await genericUserRepository.FindById(chatid);
            DronDTO dron = new DronDTO();
            if (currentStep == 1)
            {
                user.FIO = message;
                await genericUserRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 2);
                await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                return;
            }
            if(currentStep == 2)
            {
                // на первое время,потом вероятнее всего дроны будут из выборки тянуться
                dron.Mark = message;
                await dronRepository.CreateDron(dron);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 3);
                await client.SendTextMessageAsync(chatid, "Введите тип страховки");
                return;
            }
            if(currentStep == 3)
            {
                user.TypeOfInsurance = message;
                await genericUserRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой",4);
                await client.SendTextMessageAsync(chatid, "Введите адресс доставки");
                return;
            }
            if(currentStep == 4)
            {
                user.Adress = message;
                await genericUserRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 5);
                await client.SendTextMessageAsync(chatid,"Сбросьте вашу геолокацию");
            }
            if(currentStep == 5)
            {
                if(messageObject.Message.Location!=null)
                {
                    user.longtitude = messageObject.Message.Location.Longitude;
                    user.latitude = messageObject.Message.Location.Latitude;

                    await genericUserRepository.Update(user);
                    string realAdres =await GeolocateHandler.GetAddressFromCordinat(user.longtitude, user.latitude);
                    await client.SendTextMessageAsync(chatid, $"Твой адрес:{realAdres}");
                    await client.SendTextMessageAsync(chatid, "Запретные зоны ниже");
                    await client.SendTextMessageAsync(chatid, GeolocateHandler.GetRestrictedAreas(user.longtitude, user.latitude));
                }
            }
        }
        #endregion

        public async Task BaseHandlerMessage(MessageEventArgs message,string text)
        {
            Console.WriteLine($"Сообщение прислал : {chatid}\nТекст:{message.Message.Text}\n");
            chatid = message.Message.Chat.Id;
            string messageText = message.Message.Text;

            //Постоянная аутентификация пользователя
            await userRepository.AuthenticateUser(chatid);

            //обязательно переписывать надо
            if(await HubsHandler.IsChatActive(chatid))
            {
                long[] arrChatid = await HubsHandler.GetChatId(chatid);

                long chatIdRecive = arrChatid[0] == chatid ? arrChatid[1] : arrChatid[0];
                await client.SendTextMessageAsync(chatIdRecive, messageText);

                return;
            }


           /* if(chatid == 700781435)
            {
                await client.SendTextMessageAsync(chatid, "Режим пидараса активирован",0,false,false,0,KeyBoardHandler.MarkupForPidor());
                return;
            }*/

            await UserLogs.WriteLog(chatid, messageText);

            if (messageText == "/start")
            {
                await CommandHandler_Start(chatid);
            }

            if(messageText == "/chat")
            {
                await hubRepository.CreateDialog(chatid, 700781435);
                var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
                {
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Подтвердить",CallbackData="confirm"}
                    },
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Отмена",CallbackData="cancel"}
                    }
                });
                await client.SendTextMessageAsync(700781435, $"{message.Message.Chat.Username} хочет с вами связаться", 0, false, false, 0, keyboard);
            }

            // меняем пользователя на админа
            if (messageText == "/op")
            {

            }
            /*
             *Меняем действие на NULL, обнуляем вводимые данные пользователя 
             * */

            if (messageText == "Назад")
            {
                await userRepository.ChangeAction(chatid, "NULL", 0);
                await CommandHandler_Start(chatid);

                return;
            }

            if (messageText == "Платная регистрация со страховкой")
            {
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 1);
                await client.SendTextMessageAsync(chatid, "Введите ФИО", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
                return;
            }

            if(messageText == "Режим покупателя")
            {
                await client.SendTextMessageAsync(chatid, "Вы зашли как покупатель"
                    ,0,false,false,0,KeyBoardHandler.Markup_Start_BuyerMode());
            }

            if (userRepository.IsUserInAction(chatid))
            {
                string action = userRepository.GetCurrentActionName(chatid);
                if(action == "Платная регистрация со страховкой")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(chatid,messageText,message);
                }
                return;
            }

        }
    }
}
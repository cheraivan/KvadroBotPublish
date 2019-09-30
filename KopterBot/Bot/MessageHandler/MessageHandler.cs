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

namespace KopterBot.Bot
{
    /// <summary>
    /// ВАЖНО!!!!!!! ПОСЛЕ ПЕРВОЙ РЕГИСТРАЦИИ В КАЖДОМ РЕЖИМЕ НУЖНО ОГРАНИЧИВАТЬ ВОЗМОЖНОСТЬ РЕГИСТРИРОВАТЬСЯ
    /// </summary>
    class MessageHandler: RepositoryProvider, IMessageHandler
    {
        long chatid;
        TelegramBotClient client;

        AdminsPush adminsPush;
        public MessageHandler(TelegramBotClient client)
        {
            this.client = client;
            adminsPush = new AdminsPush(client);
        }

        #region PrivateHandlers
        private async Task CommandHandler_Start(long chatid)
        {
            await client.SendTextMessageAsync(chatid, "Вводный текст в бота,тут чет придумаем", 0, false, false, 0,KeyBoardHandler.Markup_Start());
        }
        private async Task CommandHandler_PaidRegistrationWithoutInsurance(long chatid,string message,MessageEventArgs messageObject)
        {
            int currentStep = await userRepository.GetCurrentActionStep(chatid);
            UserDTO user =await userRepository.FindById(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await proposalRepository.FindById(chatid);
            long _c = user.ChatId;
            if(currentStep == 1)
            {
                user = await userRepository.FindById(chatid);
                user.FIO = message;
                await userRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Co страховкой", 2);
                await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                return;
            }
            if(currentStep == 2)
            {
                dron.Mark = message;
                await dronRepository.CreateDron(dron);
                await userRepository.ChangeAction(chatid, "Co страховкой", 3);
                await client.SendTextMessageAsync(chatid, "Введите адрес");
                return;
            }
            if(currentStep == 3)
            {
                await proposalRepository.Create(chatid);
                proposal = await proposalRepository.FindById(chatid);
                proposal.Adress = message;
                await userRepository.ChangeAction(chatid, "Co страховкой", 4);
                await client.SendTextMessageAsync(chatid, "Сбросьте геопозицию");
            }
            if(currentStep == 4)
            {
                if(messageObject.Message.Location != null)
                {
                    proposal.longtitude = messageObject.Message.Location.Longitude;
                    proposal.latitude = messageObject.Message.Location.Latitude;
                    string realAdres = await GeolocateHandler.GetAddressFromCordinat(proposal.longtitude, proposal.latitude);
                    proposal.RealAdress = realAdres;
                    await proposalRepository.Update(proposal);
                    await CountProposeHandler.ChangeProposeCount();
                    await adminsPush.MessageRequisitionAsync(chatid);
                }
            }
        }
        private async Task CommandHandler_PaidRegistrationWithInsurance(long chatid,string message,MessageEventArgs messageObject = null)
        {
            int currentStep =await userRepository.GetCurrentActionStep(chatid);
            UserDTO user = await userRepository.FindById(chatid);
            DronDTO dron = new DronDTO();
            ProposalDTO proposal = await proposalRepository.FindById(chatid);

            if (currentStep == 1)
            {
                user.FIO = message;
                await userRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 2);
                await client.SendTextMessageAsync(chatid, "Введите телефон");
                return;
            }
    
            if(currentStep == 2)
            {
                await proposalRepository.Create(chatid);
                user.Phone = message;
                await userRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 3);
                await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                return;
            }
            if(currentStep == 3)
            {
                dron.Mark = message;
                await dronRepository.CreateDron(dron);
                
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой",4);
                await client.SendTextMessageAsync(chatid, "Введите тип страховки");
                return;
            }
            if(currentStep == 4)
            {
                proposal.TypeOfInsurance = message;
                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 5);
                await client.SendTextMessageAsync(chatid,"Введите адрес");
            }
            if(currentStep == 5)
            {
                proposal.Adress = message;
                await proposalRepository.Update(proposal);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 6);
                await client.SendTextMessageAsync(chatid, "Сбросьте вашу геопозицию");
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
                        , 0, false, false, 0,KeyBoardHandler.Murkup_After_Registration());
                    await CountProposeHandler.ChangeProposeCount();
                    await adminsPush.MessageRequisitionAsync(chatid);
                    // можно считать человека зарегистрированым только после оплаты
                }
            }

        }
        #endregion

        public async Task BaseHandlerMessage(MessageEventArgs message,string text)
        {
            Console.WriteLine($"Сообщение прислал : {chatid}\nТекст:{message.Message.Text}\n");
            chatid = message.Message.Chat.Id;
            string messageText = message.Message.Text;
            string action = await userRepository.GetCurrentActionName(chatid);

            //Постоянная аутентификация пользователя
            //  await userRepository.AuthenticateUser(chatid);

            //обязательно переписывать надо
            /*    if(await HubsHandler.IsChatActive(chatid))
                {
                    long[] arrChatid = await HubsHandler.GetChatId(chatid);

                    long chatIdRecive = arrChatid[0] == chatid ? arrChatid[1] : arrChatid[0];
                    await client.SendTextMessageAsync(chatIdRecive, messageText);

                    return;
                }*/
        //    await UserLogs.WriteLog(chatid, messageText);

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
               // await proposalRepository.DeleteNotFillProposalAsync(chatid);
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

            if(messageText == "Полный функционал платно")
            {
                await client.SendTextMessageAsync(chatid, "Есть несколько вариантов регистрации",0,false,false,0,KeyBoardHandler.Markup_Start_Pilot_Payment_Mode());
            }

            if(messageText == "Пилот")
            {
                await client.SendTextMessageAsync(chatid, "Вы зашли как Пилот"
                    ,0,false,false,0, KeyBoardHandler.Murkup_Start_Pilot_Mode());
            }

            if (action!=null)
            {
                if(action == "Платная регистрация со страховкой")
                {
                    await CommandHandler_PaidRegistrationWithInsurance(chatid,messageText,message);
                }
                if(action == "Co страховкой")
                {
                    await CommandHandler_PaidRegistrationWithoutInsurance(chatid, messageText, message);
                }
            }
            if(messageText == "Со страхованием")
            {
                await userRepository.ChangeAction(chatid, "Co страховкой", 1);
                await client.SendTextMessageAsync(chatid, "Введите ФИО",0,false,false,0,KeyBoardHandler.Markup_Back_From_First_Action());
            }
            if (messageText == "Частичные возможности бесплатно")
            {
                await client.SendTextMessageAsync(chatid, "Есть несоклько вариантов регистрации",
                    0, false, false, 0, KeyBoardHandler.Murkup_Start_Pilot_UnBuyer_Mode());
            }
        }
    }
}
 
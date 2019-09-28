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
    class MessageHandler:IMessageHandler
    {
        long chatid;
        TelegramBotClient client;
        ApplicationContext db;
        #region repository
        AdminsPush adminsPush;
        UserRepository userRepository;
        GenericRepository<UserDTO> genericUserRepository;
        DronRepository dronRepository;
        BotRepository botRepository;
        HubRepository hubRepository;
        AdminRepository adminRepository;
        ProposalRepository proposalRepository;
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
            proposalRepository = new ProposalRepository();
            adminsPush = new AdminsPush(client);
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
            ProposalDTO proposal = await proposalRepository.GetCurrentProposal(chatid); 

            if (currentStep == 1)
            {
                user.FIO = message;

                await genericUserRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 2);
                //   await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                await client.SendTextMessageAsync(chatid, "Введите телефон");
                return;
            }
    
            if(currentStep == 2)
            {
                await proposalRepository.CreateProposal(chatid);

                /*   dron.Mark = message;
                   await dronRepository.CreateDron(dron);*/
                user.Phone = message;
                await genericUserRepository.Update(user);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 3);
                await client.SendTextMessageAsync(chatid, "Введите марку дрона");
                return;
            }
            if(currentStep == 3)
            {
                /*    proposal =await proposalRepository.GetCurrentProposal(chatid);
                    proposal.TypeOfInsurance = message;*/
                dron.Mark = message;
                await dronRepository.CreateDron(dron);
                
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой",4);
                await client.SendTextMessageAsync(chatid, "Введите тип страховки");
                return;
            }
            if(currentStep == 4)
            {
                proposal.TypeOfInsurance = message;
                await proposalRepository.UpdateProposal(proposal);
                await userRepository.ChangeAction(chatid, "Платная регистрация со страховкой", 5);
                await client.SendTextMessageAsync(chatid,"Введите адрес");
            }
            if(currentStep == 5)
            {
                proposal.Adress = message;
                await proposalRepository.UpdateProposal(proposal);
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
                    await proposalRepository.UpdateProposal(proposal);
                    await genericUserRepository.Update(user);
                    await client.SendTextMessageAsync(chatid, "Ожидаем оплату,если все нормально - кидаем клаву с этими кнопками и если все оплатил кидаем в админ-уведомление"
                        , 0, false, false, 0,KeyBoardHandler.Murkup_After_Registration());
                    await adminsPush.MessageRequisitionAsync(chatid);
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
                await proposalRepository.DeleteNotFillProposalAsync(chatid);
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

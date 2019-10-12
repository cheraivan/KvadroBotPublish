using KopterBot.Base.BaseClass;
using KopterBot.Bot;
using KopterBot.DTO;
using KopterBot.PilotCommands.KeyBoards;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.PilotCommands
{
    class ShowUsersCommand:BaseCommand
    {
        public ShowUsersCommand(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async Task Response(MessageEventArgs messageObject)
        {
            if (provider == null)
                await client.SendTextMessageAsync(messageObject.Message.Chat.Id, "оч странно");
            long chatid = messageObject.Message.Chat.Id;
            int currStep = await provider.userService.GetCurrentActionStep(chatid);
            string messageText = messageObject.Message.Text;
            int countUser;
            string message;

            if (currStep == 1)
            {
                await client.SendTextMessageAsync(chatid, "Выберите один из вариантов",0,false,false,0,KeyBoardHandler.ShowPartners());
                await provider.userService.ChangeAction(chatid, "Партнеры рядом", ++currStep);
                return;
            }
            if(currStep == 2)
            {
                if(messageText == "Просмотр пилотов")
                {
                    await provider.userService.ChangeAction(chatid, "Партнеры рядом", ++currStep);
                    await client.SendTextMessageAsync(chatid, "Выберите один из вариантов", 0, false, false, 0, KeyBoardHandler.ShowPatnersPilot());
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы ввели некорректную команду");
                }
            }
            if(currStep == 3)
            {
                if(messageText == "Из геолокации" || messageText == "Выбрать по региону")
                {
                    countUser = await provider.showUserService.CountUsersAsync();
                    if (messageText == "Из геолокации")
                    {
                        if (countUser > 1)
                        {
                            ProposalDTO proposal = await provider.proposalService.FindById(chatid);
                            UserDTO user = await provider.showUserService.GetFirstUserForCommand(chatid, proposal.Region);
                            message = $"Пилот:{user.FIO} \n" +
                            $"Телефон:{user.Phone}";
                            await provider.showUserService.ChangeMessageId(chatid, messageObject.Message.MessageId);
                            await client.SendTextMessageAsync(chatid, "Вы можете просмотреть пользователей", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
                            await client.SendTextMessageAsync(chatid, message, 0, false, false, 0, KeyBoardHandler.CallBackShowForUser());
                            return;
                        }
                        else if(countUser == 1)
                        {
                            await client.SendTextMessageAsync(chatid, "К сожалению,вы зарегестрированы один", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
                            return;
                        }
                    }
                    if(messageText == "Выбрать по региону")
                    {
                        if (countUser > 1)
                        {
                            GenerateButtons buttons = new GenerateButtons(client, provider);
                            ReplyKeyboardMarkup keyboard = await buttons.GenerateKeyBoards();
                            await client.SendTextMessageAsync(chatid, "Выберите интересующий вас регион", 0, false, false, 0, keyboard);
                            await provider.userService.ChangeAction(chatid, "Партнеры рядом", ++currStep);
                            return;
                        }
                        else if(countUser == 1)
                        {
                            await client.SendTextMessageAsync(chatid, "Вы один зарегестрированный пилот",0,false,false,0,KeyBoardHandler.Markup_Back_From_First_Action());
                            return;
                        }
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы выбрали несуществующий вариант");
                    await client.SendTextMessageAsync(chatid, "Попробуйте еще раз");
                }
            }
            if (currStep == 4)
            {
                List<string> regions = await provider.regionService.GetAllRegions();
                if (regions.Contains(messageText))
                {
                    UserDTO user = await provider.showUserService.GetFirstUserForCommand(chatid, messageText);
                    await provider.showUserService.ChangeMessageId(chatid, messageObject.Message.MessageId);
                    message = $"Пилот:{user.FIO} \n" +
                    $"Телефон:{user.Phone}";
                    await client.SendTextMessageAsync(chatid, "Вы можете просмотреть пользователей", 0, false, false, 0, KeyBoardHandler.Markup_Back_From_First_Action());
                    await client.SendTextMessageAsync(chatid, message,0,false,false,0,KeyBoardHandler.CallBackShowForUser());
                    return;
                }
                else
                {
                    await client.SendTextMessageAsync(chatid, "Вы выбрали несуществующий регион");
                }
            }
        }
    }
}

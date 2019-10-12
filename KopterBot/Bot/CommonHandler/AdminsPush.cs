using KopterBot.Base;
using KopterBot.DTO;
using KopterBot.Repository;
using KopterBot.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Bot.CommonHandler
{
    class AdminsPush
    { // есть базовая отправка уведомлений
        private CountProposeHandler propose;
        public AdminsPush()
        {
            propose = new CountProposeHandler();
        }
        public async Task MessageAboutRegistrationPilot(TelegramBotClient  client ,ServiceProvider provider,long chatid)
        {
            int countAdmin = await provider.adminService.CountAdmins();
            if (countAdmin == 0)
                return;
            List<long> admins = await provider.adminService.GetChatId();

            ProposalDTO proposal =await provider.proposalService.FindById(chatid);


            int numberOfPurpost = await propose.GetCount();
            UserDTO user = await provider.userService.FindUserByPredicate(i => i.ChatId == proposal.ChatId);
           

            if (user == null)
                throw new System.Exception("user is null");

            string message = $"Количество пилотов:{propose.GetCount()}\n" +
                $"Пилот №{proposal.ChatId} зарегистрировался\n " +
                $"ФИО:{user.FIO}\n " +
                $"Номер телефона:{user.Phone}\n " +
                $"Тип страховки:{proposal.TypeOfInsurance}\n " +
                $"Адрес доставки:{proposal.Adress}\n " +
                $"Адрес определенный с геопозиции:{proposal.RealAdress}";

            admins.ForEach(async (item) =>
            {
                await client.SendTextMessageAsync(item, message);
            });
        }
    }
}

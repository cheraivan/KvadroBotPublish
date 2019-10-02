using KopterBot.Base;
using KopterBot.DTO;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Bot.CommonHandler
{
    class AdminsPush:RepositoryProvider
    {
        private CountProposeHandler propose;
        public AdminsPush()
        {
            propose = new CountProposeHandler();
        }
        public async Task MessageRequisitionAsync(TelegramBotClient  client , long chatid)
        {
            int countAdmin = await adminRepository.CountAdmins();
            if (countAdmin == 0)
                return;
            List<long> admins = await adminRepository.GetChatId();

            ProposalDTO proposal =await proposalRepository.FindById(chatid);


            int numberOfPurpost = await propose.GetCount();
            UserDTO user = await userRepository.Get().FirstOrDefaultAsync(i => i.ChatId == proposal.ChatId);

           

            if (user == null)
                throw new Exception("user is null");

            string message = $"Номер заявки:{propose.GetCount()}\n" +
                $"Пилот №{proposal.ChatId} зарегистрировался\n " +
                $"ФИО:{user.FIO}\n " +
                $"Номер телефона:{user.Phone}\n " +
                $"Тип страховки:{proposal.TypeOfInsurance}\n " +
                $"Адрес доставки:{proposal.Adress}\n " +
                $"Адрес определенный с геопозиции:{proposal.RealAdress}";

            StorageDTO storage = new StorageDTO();
            storage.Message = message;

            await storageRepository.Create(storage);
            foreach(long _chatid in admins)
            {
               await client.SendTextMessageAsync(_chatid, message);
            }
        }
    }
}

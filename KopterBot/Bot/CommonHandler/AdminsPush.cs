using KopterBot.DTO;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace KopterBot.Bot.CommonHandler
{
    class AdminsPush:BaseRepository
    {
        ProposalRepository proposalRepository;
        TelegramBotClient client;
        public AdminsPush(TelegramBotClient client)
        {
            this.client = client;
            proposalRepository = new ProposalRepository();
        }

        public async Task MessageRequisitionAsync(long chatid)
        {
            int countAdmin = await db.Admins.CountAsync();
            if (countAdmin == 0)
                return;
            long[] chatids = new long[countAdmin];
            ProposalDTO proposal =await proposalRepository.GetCurrentProposal(chatid);

            int numberOfPurpost = await CountProposeHandler.GetCount();
            UserDTO user = await db.Users.FirstOrDefaultAsync(i => i.ChatId == proposal.ChatId);

            if (user == null)
                throw new Exception("user is null");

            string message = $"Пилот №{proposal.ChatId} зарегистрировался\n" +
                $"ФИО:{user.FIO}\n " +
                $"Номер телефона:{user.Phone}\n " +
                $"Тип страховки:{proposal.TypeOfInsurance}\n " +
                $"Адрес доставки:{proposal.Adress}\n " +
                $"Адрес определенный с геопозиции:{proposal.RealAdress}";

             foreach(long _chatid in chatids)
             {
                await client.SendTextMessageAsync(_chatid, message);
             }
        }
    }
}

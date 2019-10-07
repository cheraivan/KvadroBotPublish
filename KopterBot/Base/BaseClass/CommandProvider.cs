using KopterBot.BuisnessCommand;
using KopterBot.Chat;
using KopterBot.PilotCommands;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace KopterBot.Base.BaseClass
{
    class CommandProvider
    {
        TelegramBotClient client;
        MainProvider provider;

        public CommandProvider(TelegramBotClient client, MainProvider provider)
        {
            this.client = client;
            this.provider = provider;
        }
        private CreateBuisnessTask _createBuisnessTask;
        private BuisnessRegistration _buisnessRegistration;
        private StopChat _stopChat;
        private RegistrationPilotCommand registrationPilotCommand;
        private ShowOrders _showOrders;

    }
}

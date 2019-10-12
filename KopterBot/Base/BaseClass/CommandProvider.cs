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

        private PilotCommandProvider _pilotCommandProvider;

        private BuisnessCommandsProvider _buisnessCommandProvider;

        public BuisnessCommandsProvider buisnessCommandProvider
        {
            get
            {
                if (_buisnessCommandProvider == null)
                    _buisnessCommandProvider = new BuisnessCommandsProvider(client, provider);
                return _buisnessCommandProvider;
            }
        }

        public PilotCommandProvider pilotCommandProvider
        {
            get
            {
                if (_pilotCommandProvider == null)
                    _pilotCommandProvider = new PilotCommandProvider(client, provider);
                return _pilotCommandProvider;
            }
        }
    }
}

using KopterBot.Base.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace KopterBot.BuisnessCommand
{
    class BuisnessCommandsProvider:BaseCommandProvider
    {
        public BuisnessCommandsProvider(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        private BuisnessRegistration _buisnessRegistration;
        private CreateBuisnessTask _createBuisnessTask;

        public BuisnessRegistration buisnessTaskRegistration
        {
            get
            {
                if (_buisnessRegistration == null)
                    _buisnessRegistration = new BuisnessRegistration(client, provider);
                return _buisnessRegistration;
            }
        }
        public CreateBuisnessTask createBuisnessTaskRegistration
        {
            get
            {
                if (_createBuisnessTask == null)
                    _createBuisnessTask = new CreateBuisnessTask(provider, client);
                return _createBuisnessTask;
            }
        }
    }
}

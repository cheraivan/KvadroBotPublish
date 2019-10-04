using KopterBot.Base.BaseClass;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace KopterBot.PilotCommands
{
    class PilotCommandProvider:BaseCommandProvider
    {
        public PilotCommandProvider(TelegramBotClient client, MainProvider provider) : base(client, provider) { }

        private RegistrationPilotCommand _registrationCommand;
        private ShowOrders _showOrders;

        public RegistrationPilotCommand registrationCommand
        {
            get
            {
                if (_registrationCommand == null)
                    _registrationCommand = new RegistrationPilotCommand(client,provider);
                return _registrationCommand;
            }
        }

        public ShowOrders showOrder
        {
            get
            {
                if (_showOrders == null)
                    _showOrders = new ShowOrders(client, provider);
                return _showOrders;
            }
        }
    }
}

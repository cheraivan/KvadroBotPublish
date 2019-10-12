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
        private RequestOfferCallBack _requestOffer;
        private FlyNow _flyNow;
        private SosCommand _sosCommand;
        private ShowUsersCommand _showUsersCommand;

        public ShowUsersCommand showUsersCommand
        {
            get
            {
                if (_showUsersCommand == null)
                    _showUsersCommand = new ShowUsersCommand(client, provider);
                return _showUsersCommand;
            }
        }

        public SosCommand sosCommand
        {
            get
            {
                if (_sosCommand == null)
                    _sosCommand = new SosCommand(client,provider);
                return _sosCommand;
            }
        }

        public FlyNow flyNow
        {
            get
            {
                if (_flyNow == null)
                    _flyNow = new FlyNow(client, provider);
                return _flyNow;
            }
        }

        public RegistrationPilotCommand registrationCommand
        {
            get
            {
                if (_registrationCommand == null)
                    _registrationCommand = new RegistrationPilotCommand(client,provider);
                return _registrationCommand;
            }
        }

        public ShowOrders showOrders
        {
            get
            {
                if (_showOrders == null)
                    _showOrders = new ShowOrders(client, provider);
                return _showOrders;
            }
        }
        
        public RequestOfferCallBack requestOffer
        {
            get
            {
                if (_requestOffer == null)
                    _requestOffer = new RequestOfferCallBack(client,provider);
                return _requestOffer;
            }
        }
    }
}

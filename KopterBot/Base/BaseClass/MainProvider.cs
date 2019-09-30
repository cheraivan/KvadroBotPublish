using KopterBot.Bot;
using KopterBot.Bot.CommonHandler;
using KopterBot.DTO;
using KopterBot.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace KopterBot.Base.BaseClass
{
    class MainProvider:RepositoryProvider
    {
        private AdminsPush _adminPush;
        private HubsHandler _hubsHandler;
        private CountProposeHandler _proposeHandler;
        private ManagerPush _managerPush;
        protected ManagerPush managerPush
        {
            get
            {
                if (_managerPush == null)
                    _managerPush = new ManagerPush();
                return _managerPush;
            }
        }
        protected HubsHandler hubsHandler
        {
            get
            {
                if (_hubsHandler == null)
                    _hubsHandler = new HubsHandler();
                return _hubsHandler;
            }
        }
        protected CountProposeHandler proposeHandler
        {
            get
            {
                if (_proposeHandler == null)
                    _proposeHandler = new CountProposeHandler();
                return _proposeHandler;
            }
        }
        protected AdminsPush adminPush
        {
            get
            {
                if (_adminPush == null)
                    _adminPush = new AdminsPush();
                return _adminPush;
            }
        }
    }
}

using KopterBot.Bot;
using KopterBot.Bot.CommonHandler;
using KopterBot.DTO;
using KopterBot.Providers;
using KopterBot.Repository;
using KopterBot.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace KopterBot.Base.BaseClass
{
    class MainProvider:ServiceProvider
    {
        private AdminsPush _adminPush;
        private CountProposeHandler _proposeHandler;
        private ManagerPush _managerPush;
        public ManagerPush managerPush
        {
            get
            {
                if (_managerPush == null)
                    _managerPush = new ManagerPush();
                return _managerPush;
            }
        }
        protected CommandProvider _commandProvider;

      
        public CountProposeHandler proposeHandler
        {
            get
            {
                if (_proposeHandler == null)
                    _proposeHandler = new CountProposeHandler();
                return _proposeHandler;
            }
        }
        public AdminsPush adminPush
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

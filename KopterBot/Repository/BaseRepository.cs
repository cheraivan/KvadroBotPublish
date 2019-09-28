using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace KopterBot.Repository
{
    class BaseRepository
    {
        protected ApplicationContext db;
        public BaseRepository()
        {
            db = new ApplicationContext();
        }
    }
}

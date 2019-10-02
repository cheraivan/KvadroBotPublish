using KopterBot.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace KopterBot.Base.BaseClass
{
    class DbProvider<T>
    {
        private ApplicationContext _db;
        protected ApplicationContext db
        {
            get
            {
                if (_db == null)
                    _db = new ApplicationContext();
                return _db;
            }
        }

    }
}

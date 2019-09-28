using System;
using System.Collections.Generic;
using System.Text;

namespace KopterBot.Interfaces
{
    interface IBaseUser:IBaseEntity
    {
        long ChatId { get; set; }
        long CurrentChatId { get; }
    }
}

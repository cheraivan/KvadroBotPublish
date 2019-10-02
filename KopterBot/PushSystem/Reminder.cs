using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.PushSystem
{
    class Reminder
    {
        public delegate Task MessageReminder(long chatid, string message);
        public event MessageReminder SendMessage;
    }
}

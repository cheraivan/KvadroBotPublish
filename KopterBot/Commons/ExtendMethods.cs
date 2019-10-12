using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.Commons
{
    public static class ExtendMethods
    {
        public static  List<KeyboardButton> TransformToList(this KeyboardButton butt)
        {
            List<KeyboardButton> result = new List<KeyboardButton>();
            result.Add(butt);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.Bot
{
    class AdminKeyBoardHandler
    {
        public static IReplyMarkup Start_Murkup()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Просмотр заявок")
                    },
                    new[]
                    {
                        new KeyboardButton("Модерирование обьявлений")
                    },
                    new[]
                    {
                        new KeyboardButton("Просмотр логов")
                    }
                },
                ResizeKeyboard = true
            };
        }
    }
}

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
                        new KeyboardButton("Модерирование чатов")
                    },
                    new[]
                    {
                        new KeyboardButton("Модерирование пилотов"),
                        new KeyboardButton("Модерирование обьявлений")
                    }
                },
                ResizeKeyboard = true
            };
        }
    }
}

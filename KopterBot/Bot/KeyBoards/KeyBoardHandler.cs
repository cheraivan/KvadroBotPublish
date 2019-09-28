using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.Bot
{
    class KeyBoardHandler
    {
        public static IReplyMarkup Markup_Back_From_First_Action()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Назад")
                    }
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup Markup_Start()
        {
            IReplyMarkup keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Режим покупателя")
                    },
                    new[]
                    {
                        new KeyboardButton("Режим продавца")
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }
        public static IReplyMarkup Markup_Start_BuyerMode()
        {
            IReplyMarkup keyboard = new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Платная регистрация со страховкой")
                    },
                    new[]
                    {
                        new KeyboardButton("Платная регистрация без страховки")
                    },
                    new[]
                    {
                        new KeyboardButton("Купить страховку")
                    },
                    new[]
                    {
                        new KeyboardButton("Бесплатная регистрация")
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }
    }
}

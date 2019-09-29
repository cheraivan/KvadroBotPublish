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
                        new KeyboardButton("Пилот")
                    },
                    new[]
                    {
                        new KeyboardButton("Заказчик услуг")
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }
        public static IReplyMarkup Markup_Start_Pilot_Payment_Mode()
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
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }
        public static IReplyMarkup Murkup_Start_Pilot_UnBuyer_Mode()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Со страхованием")
                    },
                    new[]
                    {
                        new KeyboardButton("Без страховки")
                    }
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup Murkup_Start_Pilot_Mode()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Полный функционал платно")
                    },
                    new[]
                    {
                        new KeyboardButton("Частичные возможности бесплатно")
                    }
                },
                ResizeKeyboard = true
            };

        }
        public static IReplyMarkup Murkup_After_Registration()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Хочу лететь здесь и сейчас")
                    },
                    new[]
                    {
                        new KeyboardButton("Запланировать полёт")

                    },
                    new[]
                    {
                        new KeyboardButton("Партнеры рядом")

                    },
                    new[]
                    {
                        new KeyboardButton("SOS")
                    }
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup Start_For_Buisnessmen()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Частный клиент")
                    },
                    new[]
                    {
                        new KeyboardButton("Корпоративный клиент")
                    }
                },
                ResizeKeyboard = true
            };
        }
    }
}

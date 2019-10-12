using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.Bot
{
    class KeyBoardHandler
    {
        public static IReplyMarkup ShowPatnersPilot()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Из геолокации")
                    },
                    new[]
                    {
                        new KeyboardButton("Выбрать по региону")
                    }
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup ShowPartners()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Просмотр пилотов")
                    },
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup EndDialog()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[] {
                new[]
                {
                    new KeyboardButton("Закончить диалог")
                },
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup PilotWithoutSubscribe_Murkup()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Создать заказ")
                    },
                    new[]
                    {
                        new KeyboardButton("Партнеры")
                    },
                    new[]
                    {
                        new KeyboardButton("Просмотр заказов")
                    },
                    new[]
                    {
                        new KeyboardButton("Назад")
                    }
                },
                ResizeKeyboard = true
            };
        }
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
        public static IReplyMarkup Murkup_Start_AfterChange()
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
        public static IReplyMarkup Murkup_BuisnessmanMenu()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Просмотреть свои заказы")
                    },
                    new[]
                    {
                        new KeyboardButton("Создать новую задачу")
                    },
                    new[]
                    {
                        new KeyboardButton("Назад")
                    }
                },
                ResizeKeyboard = true
            };
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
                    },
                    new[]
                    {
                        new KeyboardButton("Назад")
                    }
                },
                ResizeKeyboard = true
            };
            return keyboard;
        }
        public static IReplyMarkup  Murkup_Start_Buisness_Mode()
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
                        new KeyboardButton("Корпоративный")
                    },
                    new[]
                    {
                        new KeyboardButton("Назад")
                    }
                },
                ResizeKeyboard = true
            };
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
                    },
                    new[]
                    {
                        new KeyboardButton("Назад")
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
                    },
                    new[]
                    {
                        new KeyboardButton("Назад")
                    }
                },
                ResizeKeyboard = true
            };

        }
        public static IReplyMarkup PilotWithSubscribe_Murkup()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Просмотр заказов")
                    },
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
                    },
                    new[]
                    {
                        new KeyboardButton("Назад")
                    }
                },
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup AuthOrRegistration()
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new[]
                {
                    new[]
                    {
                        new KeyboardButton("Авторизация")
                    },
                    new[]
                    {
                        new KeyboardButton("Регистрация")
                    }
                }
            };
        }
        public static IReplyMarkup ChangeKeyBoardPilot(int privilagie)
        {
            if (privilagie == 1)
                return PilotWithoutSubscribe_Murkup();
            if (privilagie == 2)
                return PilotWithSubscribe_Murkup();
            throw new System.Exception("incorrect value");
        }
        public static IReplyMarkup ChatConfirm()
        {
            var keyboard = new InlineKeyboardMarkup(new InlineKeyboardButton[][]
               {
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Подтвердить",CallbackData="confirm"}
                    },
                    new[]
                    {
                        new InlineKeyboardButton(){Text="Отмена",CallbackData="cancel"}
                    }
               });
            return keyboard;
        }
        public static IReplyMarkup InviteUserToDialog()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
           {
                new[]
                {
                    new InlineKeyboardButton(){Text = "Начать диалог" , CallbackData="StartDialog"}
                }
           });
        }

        public static IReplyMarkup CallBackShowOrdersForBuisnessman()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[]
                {
                    new InlineKeyboardButton(){Text = "Данные по заявке" , CallbackData="RequestData"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏩",CallbackData="BuisnessNext"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏪",CallbackData="BuisnessBack"}
                }
            });
        }

        public static IReplyMarkup CallBackShowOrders()
        {
            return new InlineKeyboardMarkup(new InlineKeyboardButton[][]
            {
                new[]
                {
                    new InlineKeyboardButton(){Text = "Оставить заявку" , CallbackData="RequestTask"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏩",CallbackData="Next"}
                },
                new[]
                {
                    new InlineKeyboardButton(){Text = "⏪",CallbackData="Back"}
                }
            });
        }
    }
}

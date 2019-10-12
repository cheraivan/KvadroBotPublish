using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KopterBot.Commons
{
    class CommandList
    {
        public static List<string> GetCommands()
        {
            List<string> result = new List<string>();
            string line;
            using (StreamReader reader = new StreamReader("Commands.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                    result.Add(line);
            }
            return result;
        }
        public static List<string> PilotCommandListWithMaxPrivilage()
        {
            List<string> result = new List<string>();
            result.Add("Просмотр заказов");
            result.Add("Хочу лететь здесь и сейчас");
            result.Add("Запланировать полёт");
            result.Add("Партнеры рядом");
            result.Add("SOS");
            return result;
        }
        public static List<string> PilotCommandListWithMinPrivilage()
        {
            List<string> result = new List<string>();
            result.Add("Партнеры");
            result.Add("Просмотр заказов");
            return result;
        }
        public static List<string> RegistrationPilotCommandList()
        {
            List<string> result = new List<string>();
            result.Add("Полный функционал платно");
            result.Add("Частичные возможности бесплатно");
            result.Add("Со страхованием");
            result.Add("Без страховки");
            result.Add("Платная регистрация со страховкой");
            result.Add("Платная регистрация без страховки");
            return result;
        }
        public static List<string> RegistrationBuisnessCommandList()
        {
            List<string> result = new List<string>();
            result.Add("Частный клиент");
            result.Add("Корпоративный");
            return result;
        }
    }
}

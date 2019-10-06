using KopterBot.Bot;
using KopterBot.Commons;
using KopterBot.Geolocate;
using KopterBot.Logs;
using System;
using System.Linq;

namespace KopterBot
{
    class Program
    {
        static async void ReadLogs(long chatid)
        {
            if (UserLogs.GetLogs(chatid)!= null)
            {
                UserLogs.GetLogs(chatid).ToList().ForEach((s) => Console.WriteLine(s));
            }
        }
        static async void T()
        { 
            Console.WriteLine(await GeolocateHandler.GetAddressFromCordinat((float)36.30306, (float)49.95094));
        }
        static void Main(string[] args)
        {
              Console.WriteLine("Введите 1 чтобы запустить бота");
              Console.WriteLine("Введите 2 чтобы получить логи пользователя");
            if (Convert.ToInt32(Console.ReadLine()) == 1)
              {
                  Console.WriteLine("server running");
                  StartBot bot = new StartBot();
                  bot.BotRun();
              }
              if (Convert.ToInt32(Console.ReadLine()) == 2)
              {
                  Console.WriteLine("Введите сhatid пользователя");
                  long chatid = Convert.ToInt64(Console.ReadLine());
                  ReadLogs(chatid);
              }
            Console.ReadLine();
        }
    }
}

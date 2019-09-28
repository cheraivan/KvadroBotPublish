using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KopterBot.Logs
{
    class LogsException
    {
        public async static void WriteLog(string message)
        {
            DateTime date = DateTime.Now;
            await File.AppendAllTextAsync("Logs.txt", date.ToString()+"\n" + message);
        }
    }
}

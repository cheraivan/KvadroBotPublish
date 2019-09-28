using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Logs
{
    class UserLogs
    {
        public static string[] GetLogs(long chatid)
        {
            string filename = chatid.ToString();
            string path = $"{Commons.Constant.PathUserLogs}" + "\\" + $"{filename}.txt";
            if (File.Exists(path))
                return  File.ReadAllLines(path);
            return null;
        }
        public static async Task WriteLog(long chatid,string content)
        {
            var data = DateTime.Now;
            string filename = chatid.ToString();
            await File.AppendAllTextAsync($"{Commons.Constant.PathUserLogs}"+"\\" + $"{filename}.txt",data+"\n"+content+"\n");
        }
    }
}

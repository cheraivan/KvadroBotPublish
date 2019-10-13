using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KopterBot.Commons
{
    class WorkWithFiles
    {
        public static void CreateChatDirectory()
        {
            string currentCategory = Directory.GetCurrentDirectory();
            Console.WriteLine(currentCategory);
        }
    }
}

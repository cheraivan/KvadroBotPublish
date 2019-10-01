using System;
using System.Collections.Generic;
using System.Text;

namespace KopterBot.Providers
{
    class CommandProvider
    {
        public bool IsCommandCorrect(string message,string action, List<string> commands,bool isChatActive = false)
        {
            if (action != null)
                return true;
            if (commands.Contains(message))
                return true;
            return false;
        }
    }
}

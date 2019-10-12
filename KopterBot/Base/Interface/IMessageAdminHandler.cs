using System.Threading.Tasks;
using Telegram.Bot.Args;

namespace KopterBot.Bot
{
    interface IMessageAdminHandler
    {
        Task BaseAdminMessageHandler(MessageEventArgs args);
    }
}

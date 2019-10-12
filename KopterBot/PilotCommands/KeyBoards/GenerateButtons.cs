using KopterBot.Base.BaseClass;
using KopterBot.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace KopterBot.PilotCommands.KeyBoards
{
    class GenerateButtons:BaseCommand
    {
        public GenerateButtons(TelegramBotClient client,MainProvider provider) : base(client, provider) { }

        public async ValueTask<ReplyKeyboardMarkup> GenerateKeyBoards()
        {
            List<string> regions =await provider.regionService.GetAllRegions();

            List<IEnumerable<KeyboardButton>> buttRegions = new List<IEnumerable<KeyboardButton>>();

            regions.ForEach((item) =>
            {
                KeyboardButton button = new KeyboardButton() { Text = item };
                buttRegions.Add(button.TransformToList());
            });
            KeyboardButton butt = new KeyboardButton("Назад");
            IEnumerable<KeyboardButton> backButton = butt.TransformToList();
            buttRegions.Add(backButton);

            IEnumerable<IEnumerable<KeyboardButton>> resButt = buttRegions;

            return new ReplyKeyboardMarkup
            {
                Keyboard = resButt,
                ResizeKeyboard = true
            };
        }
    }
}

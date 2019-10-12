using KopterBot.Base.BaseClass;
using KopterBot.BuisnessCommand.CallBacks;
using KopterBot.Chat.CallBack;
using KopterBot.DTO;
using KopterBot.Interfaces;
using KopterBot.PilotCommands;
using KopterBot.PilotCommands.CallBacks;
using KopterBot.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace KopterBot.Bot
{
    class CallBackHandler:ICallbackHandler
    {
        // сделать калл-бэк провайдер

        TelegramBotClient client;
        MainProvider provider;
        CallBackOrders ordersCallback;
        RequestOfferCallBack offerCallback;
        ShowMyOffersCallBack myOffersCallback;
        StartDialogCallBack startDialogCallBack;
        CallBackShowUsers showUsersCallback;
        public CallBackHandler(TelegramBotClient client,MainProvider provider)
        {
            this.provider = provider;
            this.client = client;
            ordersCallback = new CallBackOrders(client, provider);
            offerCallback = new RequestOfferCallBack(client, provider);
            myOffersCallback = new ShowMyOffersCallBack(client, provider);
            startDialogCallBack = new StartDialogCallBack(client, provider);
            showUsersCallback = new CallBackShowUsers(client, provider);
        }


        public async Task BaseCallBackHandler(CallbackQueryEventArgs callback)
        {
            long chatid = callback.CallbackQuery.Message.Chat.Id; // receiver
           
            if (callback.CallbackQuery.Data == "Next" || callback.CallbackQuery.Data == "Back")
            {
                await ordersCallback.ShowOrdersCallBack(callback);
            }
            if (callback.CallbackQuery.Data == "BuisnessNext" || callback.CallbackQuery.Data == "BuisnessBack")
            {
                await ordersCallback.ShowMyOrdersCallBack(callback);
            }
            if(callback.CallbackQuery.Data == "RequestTask")
            {
                await offerCallback.SendRequest(callback);
            }
            if (callback.CallbackQuery.Data == "RequestData")
            {
                await myOffersCallback.ShowOffersCallBack(callback);
            }
            if(callback.CallbackQuery.Data == "StartDialog")
            {
                await startDialogCallBack.SendCallBack(callback);
            }
            if(callback.CallbackQuery.Data == "confirm")
            {
                await startDialogCallBack.StartCommenication(callback);
            }
            if(callback.CallbackQuery.Data == "ShowUserNext" || callback.CallbackQuery.Data == "ShowUserPrevious")
            {
                await showUsersCallback.SendCallBack(callback);
            }
        }
    }
}

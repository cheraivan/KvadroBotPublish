using KopterBot.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Payment
{
    class EasyPay
    {
        public static string GetCart()
        {
            using (StreamReader reader = new StreamReader("Cart.txt"))
            {
                string _cart = reader.ReadLine();
                return _cart;
            }
        }
        private async static ValueTask<bool> IsExist(string PayId)
        {
            List<string> result = new List<string>();
            using (StreamReader reader = new StreamReader("Checks.txt"))
            {
                string line;
                while((line = await reader.ReadLineAsync())!=null)
                {
                    result.Add(line);
                }
            }
            if (result.Contains(PayId))
                return true;
            return false;
        }
        public async static ValueTask<int?> IsPayCorrect(string PayId)
        {
            string url = Constant.EasyPayURL + PayId + "&contentType=text/html";
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            string content = "";
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    content = await reader.ReadToEndAsync();
                }
            }
            string myCart = GetCart();
            if(content.IndexOf(myCart)!=-1)
            {
                if (!await IsExist(PayId))
                {
                    await File.AppendAllTextAsync("Checks.txt", PayId);

                    //вернуть правильную сумму
                    return Convert.ToInt32(PayId); 
                }
            }
            return null;
        }
    }
}

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
        public static string CheckCart()
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
            using (StreamReader reader = new StreamReader("Cart.txt"))
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
        public async static ValueTask<bool> IsPayCorrect(string PayId,string Sum)
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
            string myCart = CheckCart();
            if(content.IndexOf(myCart)!=-1)
            {
                if(content.IndexOf(Sum)!=-1)
                {
                    if (!await IsExist(PayId))
                        return true;
                }
            }
            return false;
        }
    }
}

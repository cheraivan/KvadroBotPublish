using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KopterBot.Payment
{
    class EasyPay
    {
        public static bool Check(string cart,string check,int sum)
        {
            using (StreamReader reader = new StreamReader("Cart.txt"))
            {
                string _cart = reader.ReadLine();
                if(!cart.Equals(_cart))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

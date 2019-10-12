using System;
using System.Collections.Generic;
using System.Text;

namespace KopterBot.Commons
{
    class GetGeolocateRegion
    {
        public static string GetRegion(string address)
        {
            int index = address.IndexOf(",");
            int index2 = address.IndexOf(",", ++index);

            string result = "";

            for(int i = index+1; i<index2;i++)
            {
                result += address[i];
            }

            return result;
        }
    }
}

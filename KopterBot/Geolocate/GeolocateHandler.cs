using KopterBot.Commons;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KopterBot.Geolocate
{
    class GeolocateHandler
    {

        public async static ValueTask<string> GetAddressFromCordinat(float? longtitude,float? latitude)
        {
            GeolocateRequest request = new GeolocateRequest(longtitude, latitude);
            await request.Request("string");
            string responseFromServer = request.GetStringResponse();

            int index1 = responseFromServer.IndexOf("<text>");
            int index2 = responseFromServer.IndexOf("</text>");

            string result = "";
            for (int i = index1 + "<text>".Length; i < index2; i++)
                result += responseFromServer[i];
            return result;
        }
        public static string GetRestrictedAreas(float? longtitude,float? latitude) // ВАЖНО,КООРДИНАТЫ НАОБОРОТ ПЕРЕДАЮТСЯ
        { 
            string url =$"https://www.google.com/maps/d/u/0/viewer?mid=15zGpRn3es7trUwiS6rQnk_5UVLcJWtqd&hl=en&ll={latitude}%2C{longtitude}&z=12";
            return url;
        }
    }
}

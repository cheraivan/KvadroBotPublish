using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KopterBot.Geolocate
{
    class Weather
    {
     /*   public async static ValueTask<string> GetWeather(float longtitude,float lautitude)
        {
            string url = Commons.Constant.WhertherURL + $"lat={lautitude}&lon={longtitude}" + Commons.Constant.WheatherAPIKey;
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            string result = "";
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = await reader.ReadToEndAsync();
                }
            }



            return url;
        }*/
    }
}

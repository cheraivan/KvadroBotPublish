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
        public async static ValueTask<string> GetGeolocation(float longtitude,float lautitude)
        {
            WebRequest request = WebRequest.Create(Commons.Constant.WheterURL+$"lat={lautitude}&lon={longtitude}" + Commons.Constant.APIKEY);
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}

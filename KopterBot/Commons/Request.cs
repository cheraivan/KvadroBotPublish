using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KopterBot.Commons
{
    class BaseRequest
    {
        protected string url;
        protected WebResponse response;
        public BaseRequest() { }
        public BaseRequest(string url)
        {
            this.url = url;
        }
        protected async Task Request()
        {
            WebRequest request = WebRequest.Create(url);
            response = await request.GetResponseAsync();
        }
        protected void CheckNullable(object o)
        {
            if (o is null)
                throw new Exception("value is null");
        }
    }
    class GeolocateRequest:BaseRequest
    {
        public GeolocateRequest(float longtitude, float lautitude)
        {
            string coordinat = longtitude + "," + lautitude;
            url = "https://geocode-maps.yandex.ru/1.x/" + $"?geocode={coordinat}&apikey={Constant.APIKEY}&kind=house";
        }
        string _response;
        XmlDocument document;
        public XmlDocument GetXml()
        {
            CheckNullable(document);
            return document;
        }
        public string GetStringResponse()
        {
            CheckNullable(_response);
            return _response;
        }
        public async Task Request(string contentType) // xml-xml ,string-string
        {
            await Request();
            if(contentType == "xml")
            {
                using (Stream stream = response.GetResponseStream())
                {
                    document.Load(stream);
                }
            }else if(contentType == "string")
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        _response = await reader.ReadToEndAsync();
                    }
                }
            }
            else
            {
                throw new Exception("Incorrect format");
            }
        }
    }
}

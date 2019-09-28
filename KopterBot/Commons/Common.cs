using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KopterBot.Commons
{
    class Common
    {
        public static bool isDigit(string input) =>
            input.All(c => char.IsDigit(c));

        public async static ValueTask<XmlDocument> SendRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = await request.GetResponseAsync();
            using (Stream stream = response.GetResponseStream())
            {
                XmlDocument document = new XmlDocument();
                document.Load(stream);
                return document;
            }
        }
       
    }
}

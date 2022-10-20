using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmailSendLib.Sevices
{
    public class SMSService
    {
        public string Send(string phone, string text) // sends sms
        {
            string reports = ""; // contains Server response

            string apiKey = "ua284a79d6e7d82f53d86e032b4367972fca542c7d725246ba156d5e3cba4120e03893"; // own mobizon key
            string url = $"https://api.mobizon.ua/service/message/sendsmsmessage?recipient={phone}&text={text}&apiKey={apiKey}"; // request address to mobizon where is included phone, text and key
            WebRequest request = WebRequest.Create(url); // Initializes new WebRequest obj
            request.Credentials = CredentialCache.DefaultCredentials; // gets the network credentials used for authenticating the request with the internet resource
            WebResponse response = request.GetResponse(); // returns a response to an internet request
            reports += ((HttpWebResponse)response).StatusDescription; // adds status describing of the response
            reports += "\r\n";
            using (Stream dataStream = response.GetResponseStream()) // going request
            {
                // Here will be outputting Server's respond
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reports += responseFromServer;
            }
            response.Close(); // close requesting

            return reports; // return reports
        }
    }
}

using System.IO;
using System.Net;
using uFeed.DAL.SocialService.Exceptions;

namespace uFeed.DAL.SocialService.Services.Facebook
{
    public static class Request
    {
        public static string ExecuteFacebookRequest(string request, string accessToken, string method)
        {
            string result = string.Empty;

            if (method.Equals("get"))
            {
                result = SendGetRequest("https://graph.facebook.com/v2.7/" + request + "&access_token=" + accessToken, null);
            }

            return result;
        }
        public static string SendGetRequest(string url, string data)
        {
            WebRequest req;

            if (data == null || data.Equals(string.Empty))
            {
                req = WebRequest.Create(url);
            }
            else
            {
                req = WebRequest.Create(url + "?" + data);
            }

            WebResponse resp = req.GetResponse();

            Stream stream = resp.GetResponseStream();

            if (stream == null)
            {
                throw new SocialException("Cannot get response stream");
            }

            var sr = new StreamReader(stream);
            var Out = sr.ReadToEnd();
            sr.Close();

            return Out;
        }
    }
}
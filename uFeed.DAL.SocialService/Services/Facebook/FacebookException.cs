using System;
using uFeed.DAL.SocialService.Interfaces;

namespace uFeed.DAL.SocialService.Services.Facebook
{
    public class FacebookException : Exception, ISocialException
    {
        public bool IsNoAccessToken { get; set; }

        public FacebookException(string message, bool isNoAccessToken = false) : base(message)
        {
            IsNoAccessToken = isNoAccessToken;
        }
    }
}
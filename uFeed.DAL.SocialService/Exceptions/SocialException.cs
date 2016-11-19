using System;

namespace uFeed.DAL.SocialService.Exceptions
{
    public class SocialException : Exception
    {
        public SocialException(string message, bool invalidToken = false) : base(message)
        {
            InvalidAccessToken = invalidToken;
        }

        public bool InvalidAccessToken { get; set; }
    }
}

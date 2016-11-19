using System.Web;
using uFeed.DAL.SocialService.Interfaces;

namespace uFeed.DAL.SocialService.UnitOfWorks
{
    public class SocialUnitOfWork : ISocialUnitOfWork   {

        public ISocialApi VkApi { get; set; }

        public ISocialApi FacebookApi { get; set; }

        public void InitializeVk(string accessToken, string userId, int expiresIn, HttpSessionStateBase sessionObj)
        {
            VkApi = new Services.Vk.Api(accessToken, userId, expiresIn, sessionObj);
        }

        public void InitializeFacebook(string code, HttpSessionStateBase sessionObj)
        {
            FacebookApi = new Services.Facebook.Api(code, sessionObj);
        }
    }
}

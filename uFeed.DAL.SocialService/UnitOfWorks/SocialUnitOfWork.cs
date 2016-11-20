using uFeed.DAL.SocialService.Interfaces;

namespace uFeed.DAL.SocialService.UnitOfWorks
{
    public class SocialUnitOfWork : ISocialUnitOfWork   {

        public ISocialApi VkApi { get; set; }

        public ISocialApi FacebookApi { get; set; }

        public void InitializeVk(string accessToken, string userId, string code)
        {
            VkApi = new Services.Vk.Api(accessToken, userId, code);
        }

        public void InitializeFacebook(string token, string code)
        {
            FacebookApi = new Services.Facebook.Api(token, code);
        }
    }
}

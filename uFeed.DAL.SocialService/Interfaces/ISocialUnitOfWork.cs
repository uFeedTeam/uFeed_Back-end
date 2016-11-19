using System.Web;

namespace uFeed.DAL.SocialService.Interfaces
{
    public interface ISocialUnitOfWork
    {
        ISocialApi VkApi { get; }

        ISocialApi FacebookApi { get; }

        void InitializeVk(string accessToken, string userId, int expiresIn, HttpSessionStateBase sessionObj);

        void InitializeFacebook(string code, HttpSessionStateBase sessionObj);
    }
}

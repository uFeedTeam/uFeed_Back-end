namespace uFeed.DAL.SocialService.Interfaces
{
    public interface ISocialUnitOfWork
    {
        ISocialApi VkApi { get; }

        ISocialApi FacebookApi { get; }

        void InitializeVk(string accessToken, string userId, string code);

        void InitializeFacebook(string token, string code);
    }
}

using System.Collections.Generic;
using uFeed.Entities;
using uFeed.Entities.Social;

namespace uFeed.DAL.SocialService.Interfaces
{
    public interface ISocialApi
    {
        string GetToken();

        string GetUserId();

        UserInfo GetUserInfo();

        List<Author> GetAllAuthors();

        List<Author> GetAuthors(int page, int count);

        List<Post> GetFeed(Category category, int page, int count);
    }
}

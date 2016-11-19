using System.Collections.Generic;
using uFeed.Entities;
using uFeed.Entities.Social;

namespace uFeed.DAL.SocialService.Interfaces
{
    public interface ISocialApi
    {
        UserInfo GetUserInfo();

        List<Author> GetAllAuthors();

        List<Author> GetAuthors(int page, int count);

        //List<Author> GetAuthors(int count);

        //void GetNextAuthors(List<Author> authors);

        List<Post> GetFeed(Category category, int page, int count);

        //List<Post> GetFeed(Category category, int countPosts);

        //void GetNextFeed(List<Post> feed);
    }
}

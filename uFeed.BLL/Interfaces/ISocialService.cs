using System.Collections.Generic;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.Enums;
using uFeed.Entities;

namespace uFeed.BLL.Interfaces
{
    public interface ISocialService
    {
        void Login(Socials socialNetwork, string codeOrAccessToken, string userId = null, bool isAccessToken = false);

        string GetToken(Socials socialNetwork);

        string GetUserId(Socials socialNetwork);

        List<UserInfoDTO> GetUserInfo();

        List<AuthorDTO> GetAllAuthors(Socials socialNetwork);

        List<AuthorDTO> GetAuthors(int page, int count, Socials socialNetwork);

        List<PostDTO> GetFeed(CategoryDTO category, int page, int countPosts, Socials[] logins);

        void Dispose();
    }
}

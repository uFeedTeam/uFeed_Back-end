using System.Collections.Generic;
using System.Web;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.Enums;

namespace uFeed.BLL.Interfaces
{
    public interface ISocialService
    {
        void Login(Socials socialNetwork, string codeOrAccessToken, int? expiresIn = null, string userId = null);

        List<UserInfoDTO> GetUserInfo();

        List<AuthorDTO> GetAllAuthors(Socials socialNetwork);

        List<AuthorDTO> GetAuthors(int page, int count, Socials socialNetwork);

        List<PostDTO> GetFeed(CategoryDTO category, int page, int countPosts);

        void Dispose();
    }
}

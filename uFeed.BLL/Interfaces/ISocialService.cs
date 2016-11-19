using System.Collections.Generic;
using System.Web;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.Enums;

namespace uFeed.BLL.Interfaces
{
    public interface ISocialService
    {
        void Login(string socialNetwork, string codeOrAccessToken, HttpSessionStateBase session, int? expiresIn = null, string userId = null);

        List<UserInfoDTO> GetUserInfo();

        List<AuthorDTO> GetAllAuthors(string socialNetwork);

        List<AuthorDTO> GetAuthors(int count, Socials socialNetwork);

        void GetNextAuthors(List<AuthorDTO> authors, Socials socialNetwork);

        List<PostDTO> GetFeed(CategoryDTO category, int countPosts);

        void GetNextFeed(List<PostDTO> feedDTO);

        void Dispose();
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.Enums;
using uFeed.BLL.Interfaces;
using uFeed.DAL.Interfaces;
using uFeed.DAL.SocialService.Interfaces;
using uFeed.Entities;
using uFeed.Entities.Social;

namespace uFeed.BLL.Services
{
    public class SocialService : ISocialService
    {
        private readonly ISocialUnitOfWork _socialUnitOfWork;
        private readonly IUnitOfWork _unitOfWork;

        public SocialService(ISocialUnitOfWork socialUnitOfWork, IUnitOfWork unitOfWork)
        {
            _socialUnitOfWork = socialUnitOfWork;
            _unitOfWork = unitOfWork;
        }

        public void Login(string socialNetwork, string codeOrAccessToken, HttpSessionStateBase session, int? expiresIn = null, string userId = null)
        {
            switch (socialNetwork)
            {
                case "vk":
                    if (expiresIn != null)
                        _socialUnitOfWork.InitializeVk(codeOrAccessToken, userId, (int)expiresIn, session);
                    break;
                case "facebook":
                    _socialUnitOfWork.InitializeFacebook(codeOrAccessToken, session);
                    break;
            }
        }

        public List<UserInfoDTO> GetUserInfo()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserInfo, UserInfoDTO>();
                cfg.CreateMap<Photo, PhotoDTO>();
            });

            var mapper = config.CreateMapper();
            var userInfoes = new List<UserInfo>();
            var fbUser = _socialUnitOfWork.FacebookApi.GetUserInfo();
            var vkUser = _socialUnitOfWork.VkApi.GetUserInfo();

            userInfoes.AddRange(new[]
            {
                fbUser,
                vkUser
            });

            return mapper.Map<IEnumerable<UserInfoDTO>>(userInfoes).ToList();
        }

        public List<AuthorDTO> GetAllAuthors(string socialNetwork)
        {
            var authors = new List<Author>();

            switch (socialNetwork)
            {
                case "vk":
                    authors.AddRange(_socialUnitOfWork.VkApi.GetAllAuthors());
                    break;
                case "facebook":
                    authors.AddRange(_socialUnitOfWork.FacebookApi.GetAllAuthors());
                    break;
            }

            return Mapper.Map<IEnumerable<AuthorDTO>>(authors).ToList();
        }

        public List<AuthorDTO> GetAuthors(int count, Socials socialNetwork)
        {
            var authors = new List<Author>();

            switch (socialNetwork)
            {
                case Socials.Vk:
                    authors.AddRange(_socialUnitOfWork.VkApi.GetAuthors(count));
                    break;
                case Socials.Facebook:
                    authors.AddRange(_socialUnitOfWork.FacebookApi.GetAuthors(count));
                    break;
            }

            return Mapper.Map<IEnumerable<AuthorDTO>>(authors).ToList();
        }

        public void GetNextAuthors(List<AuthorDTO> authorsDto, Socials socialNetwork)
        {
            var authors = Mapper.Map<IEnumerable<Author>>(authorsDto).ToList();

            switch (socialNetwork)
            {
                case Socials.Vk:
                    _socialUnitOfWork.VkApi.GetNextAuthors(authors);
                    break;
                case Socials.Facebook:
                    _socialUnitOfWork.FacebookApi.GetNextAuthors(authors);
                    break;
            }
            authorsDto.Clear();

            authorsDto.AddRange(Mapper.Map<IEnumerable<AuthorDTO>>(authors));
        }

        public List<PostDTO> GetFeed(CategoryDTO categoryDto, int countPosts)
        {
            var category = Mapper.Map<Category>(categoryDto);

            var feed = new List<Post>();

            var fbCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                User = category.User,
                Authors = category.Authors.Where(x => x.Source == Entities.Enums.Socials.Facebook).ToList()
            };

            var vkCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                User = category.User,
                Authors = category.Authors.Where(x => x.Source == Entities.Enums.Socials.Vk).ToList()
            };

            feed.AddRange(_socialUnitOfWork.FacebookApi.GetFeed(fbCategory, countPosts));
            feed.AddRange(_socialUnitOfWork.VkApi.GetFeed(vkCategory, countPosts));

            return Mapper.Map<IEnumerable<PostDTO>>(feed).ToList();
        }

        public void GetNextFeed(List<PostDTO> feedDTO)
        {
            var feed = Mapper.Map<IEnumerable<Post>>(feedDTO).ToList();

            var fbFeed = feed.Where(x => x.Author.Source == Entities.Enums.Socials.Facebook).ToList();
            var vkFeed = feed.Where(x => x.Author.Source == Entities.Enums.Socials.Vk).ToList();

            _socialUnitOfWork.FacebookApi.GetNextFeed(fbFeed);
            _socialUnitOfWork.VkApi.GetNextFeed(vkFeed);

            feed.Clear();

            feed.AddRange(vkFeed);
            feed.AddRange(fbFeed);

            feedDTO.Clear();
            feedDTO.AddRange(Mapper.Map<IEnumerable<PostDTO>>(feed).ToList());
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}

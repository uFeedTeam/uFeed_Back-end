using System.Collections.Generic;
using System.Linq;
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

        public void Login(Socials socialNetwork, string codeOrAccessToken, int? expiresIn = null, string userId = null)
        {
            switch (socialNetwork)
            {
                case Socials.Vk:
                    if (expiresIn != null)
                    {
                        _socialUnitOfWork.InitializeVk(codeOrAccessToken, userId, (int)expiresIn);
                    }
                    break;
                case Socials.Facebook:
                    _socialUnitOfWork.InitializeFacebook(codeOrAccessToken);
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

            if (_socialUnitOfWork.FacebookApi != null)
            {
                var fbUser = _socialUnitOfWork.FacebookApi.GetUserInfo();
                userInfoes.Add(fbUser);
            }

            if (_socialUnitOfWork.FacebookApi != null)
            {
                var vkUser = _socialUnitOfWork.VkApi.GetUserInfo();
                userInfoes.Add(vkUser);
            }

            return mapper.Map<IEnumerable<UserInfoDTO>>(userInfoes).ToList();
        }

        public List<AuthorDTO> GetAllAuthors(Socials socialNetwork)
        {
            var authors = new List<Author>();

            switch (socialNetwork)
            {
                case Socials.Vk:
                    authors.AddRange(_socialUnitOfWork.VkApi.GetAllAuthors());
                    break;
                case Socials.Facebook:
                    authors.AddRange(_socialUnitOfWork.FacebookApi.GetAllAuthors());
                    break;
            }

            return Mapper.Map<IEnumerable<AuthorDTO>>(authors).ToList();
        }

        public List<AuthorDTO> GetAuthors(int page, int count, Socials socialNetwork)
        {
            var authors = new List<Author>();

            switch (socialNetwork)
            {
                case Socials.Vk:
                    authors.AddRange(_socialUnitOfWork.VkApi.GetAuthors(page, count));
                    break;
                case Socials.Facebook:
                    authors.AddRange(_socialUnitOfWork.FacebookApi.GetAuthors(page, count));
                    break;
            }

            return Mapper.Map<IEnumerable<AuthorDTO>>(authors).ToList();
        }

        public List<PostDTO> GetFeed(CategoryDTO categoryDto, int page, int countPosts)
        {
            var category = Mapper.Map<Category>(categoryDto);

            var fbCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                User = category.User,
                Authors = category.Authors
                    .Where(x => x.Source == Entities.Enums.Socials.Facebook).ToList()
            };

            var vkCategory = new Category
            {
                Id = category.Id,
                Name = category.Name,
                User = category.User,
                Authors = category.Authors
                    .Where(x => x.Source == Entities.Enums.Socials.Vk).ToList()
            };

            var feed = new List<Post>();
            if (fbCategory.Authors.Any())
            {
                feed.AddRange(_socialUnitOfWork.FacebookApi.GetFeed(fbCategory, page, countPosts));
            }

            if (vkCategory.Authors.Any())
            {
                feed.AddRange(_socialUnitOfWork.VkApi.GetFeed(vkCategory, page, countPosts));
            }

            return Mapper.Map<IEnumerable<PostDTO>>(feed).ToList();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}

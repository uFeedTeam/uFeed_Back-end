using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.Enums;
using uFeed.BLL.Infrastructure.Exceptions;
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

        public void Login(Socials socialNetwork, string codeOrAccessToken, string userId, bool isAccessToken = false)
        {
            switch (socialNetwork)
            {
                case Socials.Vk:
                    if (isAccessToken)
                    {
                        _socialUnitOfWork.InitializeVk(codeOrAccessToken, userId, null);
                    }
                    else
                    {
                        _socialUnitOfWork.InitializeVk(null, userId, codeOrAccessToken);
                    }
                    break;
                case Socials.Facebook:
                    if (isAccessToken)
                    {
                        _socialUnitOfWork.InitializeFacebook(codeOrAccessToken, null);
                    }
                    else
                    {
                        _socialUnitOfWork.InitializeFacebook(null, codeOrAccessToken);
                    }
                    break;
            }
        }

        public string GetToken(Socials socialNetwork)
        {
            switch (socialNetwork)
            {
                case Socials.Vk:
                    return _socialUnitOfWork.VkApi.GetToken();
                case Socials.Facebook:
                    return _socialUnitOfWork.FacebookApi.GetToken();
                default:
                    return null;
            }
        }

        public string GetUserId(Socials socialNetwork)
        {
            switch (socialNetwork)
            {
                case Socials.Vk:
                    return _socialUnitOfWork.VkApi.GetUserId();
                case Socials.Facebook:
                    return _socialUnitOfWork.FacebookApi.GetUserId();
                default:
                    return null;
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

            if (_socialUnitOfWork.VkApi != null)
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

        public List<PostDTO> GetFeed(CategoryDTO categoryDto, int page, int countPosts, Socials[] logins)
        {
            var category = Mapper.Map<Category>(categoryDto);

            var feed = new List<Post>();

            if (logins == null || logins.Length <= 0)
            {
                return Mapper.Map<IEnumerable<PostDTO>>(feed).ToList();
            }           

            if (logins.Contains(Socials.Facebook))
            {
                var fbCategory = new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    User = category.User,
                    Authors = category.Authors.Where(x => x.Source == Entities.Enums.Socials.Facebook).ToList()
                };
                if (fbCategory.Authors.Any())
                {
                    feed.AddRange(_socialUnitOfWork.FacebookApi.GetFeed(fbCategory, page, countPosts));
                }
            }
            if (logins.Contains(Socials.Vk))
            {
                var vkCategory = new Category
                {
                    Id = category.Id,
                    Name = category.Name,
                    User = category.User,
                    Authors = category.Authors.Where(x => x.Source == Entities.Enums.Socials.Vk).ToList()
                };

                if (vkCategory.Authors.Any())
                {
                    feed.AddRange(_socialUnitOfWork.VkApi.GetFeed(vkCategory, page, countPosts));
                }
            }

            return Mapper.Map<IEnumerable<PostDTO>>(feed).ToList();
        }

        public List<PostDTO> GetBookmarks(int userId)
        {
            var user = _unitOfWork.ClientProfiles.GetByUserId(userId);

            if (user == null)
            {
                throw new EntityNotFoundException("User cannot be found", "User");
            }

            var posts = new List<Post>();

            if (user.Logins == null || user.Logins.Count <= 0)
            {
                return Mapper.Map<IEnumerable<PostDTO>>(posts).ToList();
            }

            if (user.Logins.Select(login => login.LoginType).ToList()
                .Contains(Entities.Enums.Socials.Facebook))
            {
                var fbBookmarkIds =
                    user.Bookmarks
                        .Where(bookmark => bookmark.Source == Entities.Enums.Socials.Facebook)
                        .Select(bookmark => bookmark.PostId);

                posts.AddRange(_socialUnitOfWork.FacebookApi.GetPosts(fbBookmarkIds));
            }
            if (user.Logins.Select(login => login.LoginType).ToList()
                .Contains(Entities.Enums.Socials.Vk))
            {
                var vkBookmarkIds =
                    user.Bookmarks
                        .Where(bookmark => bookmark.Source == Entities.Enums.Socials.Vk)
                        .Select(bookmark => bookmark.PostId).ToList();
                var a = _socialUnitOfWork.VkApi.GetPosts(vkBookmarkIds);
                posts.AddRange(a);
            }

            var resultPosts = Mapper.Map<IEnumerable<PostDTO>>(posts);

            return resultPosts.ToList();
        }

        public void AddBookmark(int userId, string postId, Socials source)
        {
            var user = _unitOfWork.ClientProfiles.GetByUserId(userId);

            if (user == null)
            {
                throw new EntityNotFoundException("User cannot be found", "User");
            }

            postId = source == Socials.Vk ? "-" + postId : postId;

            if (_unitOfWork.ClientBookmarks.Find(x => x.PostId.Equals(postId)).Any())
            {
                throw new ValidationException("Such post Id already exists", "Id");
            }

            var bookmark = new ClientBookmark
            {
                ClientProfile = user,
                PostId = postId,
                Source = (Entities.Enums.Socials)source
            };
            



            _unitOfWork.ClientBookmarks.Create(bookmark);
            _unitOfWork.Save();
        }

        public void RemoveBookmark(string postId)
        {
            var bookmark = _unitOfWork.ClientBookmarks.Find(x => x.PostId.Trim('-').Equals(postId)).FirstOrDefault();

            if (bookmark == null)
            {
                throw new EntityNotFoundException($"Cannot find the bookmark. PostId: {postId}", "PostId");
            }

            _unitOfWork.ClientBookmarks.Delete(bookmark.Id);
            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
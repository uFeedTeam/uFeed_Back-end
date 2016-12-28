using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.DTO.Social;
using uFeed.BLL.Enums;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.WEB.ViewModels;
using uFeed.WEB.ViewModels.Social;
using uFeed.WEB.ViewModels.Social.Login;

namespace uFeed.WEB.Controllers
{
    [Authorize]
    [RoutePrefix("api/social")]
    public class SocialController : SocialBaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserService _userService;

        public SocialController(ICategoryService categoryService, IUserService userService)
        {
            _categoryService = categoryService;
            _userService = userService;
        }

        [HttpGet]
        [Route("vkauth")]
        [AllowAnonymous]
        public IHttpActionResult AuthenticationVk(string code)
        {
            SocialService.Login(Socials.Vk, code);

            var vkLogin = new VkLoginViewModel
            {
                AccessToken = SocialService.GetToken(Socials.Vk),
                UserId = SocialService.GetUserId(Socials.Vk)
            };

            return Ok(vkLogin);
        }

        [HttpGet]
        [Route("fbauth")]
        [AllowAnonymous]
        public IHttpActionResult AuthenticationFb(string code)
        {
            SocialService.Login(Socials.Facebook, code + "#");

            var facebookLogin = new FacebookLoginViewModel
            {
                AccessToken = SocialService.GetToken(Socials.Facebook)
            };

            return Ok(facebookLogin);
        }

        [HttpGet]
        public IHttpActionResult Test()
        {

            var userInfoes = SocialService.GetUserInfo();
            
            var authorsVkAll = SocialService.GetAllAuthors(Socials.Vk);
            //var authorsFacebookAll = _socialService.GetAllAuthors(Socials.Facebook);

            var authorsVk = SocialService.GetAuthors(1, 10, Socials.Vk);
            //var authorsFacebook = _socialService.GetAuthors(1, 10, Socials.Facebook);

            authorsVk = SocialService.GetAuthors(2, 10, Socials.Vk);
            //authorsFacebook = _socialService.GetAuthors(2, 10, Socials.Facebook);

            //authorsVk.AddRange(authorsFacebook);

            var socialAuthors = authorsVk.Select(authorDTO => new SocialAuthorDTO
            {
                AuthorId = authorDTO.Id,
                CategoryId = 1,
                Source = authorDTO.Source
            }).ToList();

            var feed1 = SocialService.GetFeed(new CategoryDTO { Id = 1, Authors = socialAuthors }, 1, 2, new []{Socials.Facebook, Socials.Vk });

            var feed2 = SocialService.GetFeed(new CategoryDTO { Id = 1, Authors = socialAuthors }, 2, 2, new[] { Socials.Facebook, Socials.Vk });

            return Ok();
        }

        [HttpGet]
        [Route("authors")]
        public IHttpActionResult GetAllAuthors()
        {

            var model = new GetAuthorsViewModel();

            var clientProfile = _userService.Get(CurrentUser.Id);

            if (clientProfile.Logins.Contains(Socials.Facebook))
            {
                var fbAuthorsDto = SocialService.GetAllAuthors(Socials.Facebook);
                model.FacebookAuthors = Mapper.Map<IEnumerable<AuthorViewModel>>(fbAuthorsDto);
            }

            if (clientProfile.Logins.Contains(Socials.Vk))
            {
                var vkAuthorsDto = SocialService.GetAllAuthors(Socials.Vk);
                model.VkAuthors = Mapper.Map<IEnumerable<AuthorViewModel>>(vkAuthorsDto);
            }

            return Json(model);
        }

        [HttpGet]
        [Route("authors/{page}/{count}")]
        public IHttpActionResult GetAuthors(int page, int count)
        {
            try
            {

                var model = new GetAuthorsViewModel();

                var clientProfile = _userService.Get(CurrentUser.Id);

                if (clientProfile.Logins.Contains(Socials.Facebook))
                {
                    List<AuthorDTO> fbAuthorsDto = SocialService.GetAuthors(page, count, Socials.Facebook);
                    model.FacebookAuthors = Mapper.Map<IEnumerable<AuthorViewModel>>(fbAuthorsDto);
                }

                if (clientProfile.Logins.Contains(Socials.Vk))
                {
                    List<AuthorDTO> vkAuthorsDto = SocialService.GetAuthors(page, count, Socials.Vk);
                    model.VkAuthors = Mapper.Map<IEnumerable<AuthorViewModel>>(vkAuthorsDto);
                }

                return Json(model);
            }
            catch (Exception ex)
            {               
                return BadRequest(ex.ToString());
            }            
        }

        [HttpGet]
        [Route("feed/{categoryId}/{page}/{postsPerGroup}")]
        public IHttpActionResult GetFeed(int categoryId, int page, int postsPerGroup)
        {

            try
            {
                var client = _userService.Get(CurrentUser.Id);
                var categoryDto = _categoryService.Get(categoryId);
                var feed = SocialService.GetFeed(categoryDto, page, postsPerGroup, client.Logins.ToArray());

                return Json(feed);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
}

        [HttpGet]
        [Route("bookmarks")]
        public IHttpActionResult GetBookmarks()
        {
            int userId = CurrentUser.Id;

            try
            {
                var posts = SocialService.GetBookmarks(userId);

                return Json(posts);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("bookmarks/{postId}/delete")]
        public IHttpActionResult DeleteBookmarks(string postId)
        {
            try
            {
                SocialService.RemoveBookmark(postId);

                return Ok();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("bookmarks/add/{source}/{postId}")]
        public IHttpActionResult CreateBookmarks(string postId, Socials source)
        {
            int userId = CurrentUser.Id;

            try
            {
                SocialService.AddBookmark(userId, postId, source);

                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
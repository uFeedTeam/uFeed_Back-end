using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
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
        private readonly IClientProfileService _clientProfileService;

        public SocialController(ICategoryService categoryService, IClientProfileService clientProfileService)
        {
            _categoryService = categoryService;
            _clientProfileService = clientProfileService;
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

        [HttpPost]
        public IHttpActionResult Test(SocialLoginViewModel loginModel)
        {

            var userInfoes = SocialService.GetUserInfo();

            try
            {
                throw new Exception(userInfoes.ToArray()[0].Id + " | " + userInfoes.ToArray()[0].Name);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
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

        [HttpPost]
        [Route("authors")]
        public IHttpActionResult GetAllAuthors(SocialLoginViewModel loginModel)
        {

            var model = new GetAuthorsViewModel();

            var clientProfile = _clientProfileService.Get(User.Identity.GetUserId<int>());

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

        [HttpPost]
        [Route("authors/{page}/{count}")]
        public IHttpActionResult GetAuthors(SocialLoginViewModel loginModel, int page, int count)
        {
            try
            {

                var model = new GetAuthorsViewModel();

                var clientProfile = _clientProfileService.Get(User.Identity.GetUserId<int>());

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

        [HttpPost]
        [Route("feed/{categoryId}/{page}/{postsPerGroup}")]
        public IHttpActionResult GetFeed(SocialLoginViewModel loginModel, int categoryId, int page, int postsPerGroup)
        {

            try
            {
                var client = _clientProfileService.Get(User.Identity.GetUserId<int>());
                var categoryDto = _categoryService.Get(categoryId);
                var feed = SocialService.GetFeed(categoryDto, page, postsPerGroup, client.Logins.ToArray());

                return Json(feed);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {               
                return BadRequest(ex.ToString());
            }
}

        [HttpPost]
        [Route("bookmarks")]
        public IHttpActionResult GetBookmarks(SocialLoginViewModel loginModel)
        {
            int userId = User.Identity.GetUserId<int>();

            try
            {
                var posts = SocialService.GetBookmarks(userId);

                return Json(posts);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("bookmarks/{postId}/delete")]
        public IHttpActionResult DeleteBookmarks(string postId)
        {
            try
            {
                SocialService.RemoveBookmark(postId);

                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPost]
        [Route("bookmarks/add/{source}/{postId}")]
        public IHttpActionResult CreateBookmarks(string postId, Socials source)
        {
            int userId = User.Identity.GetUserId<int>();

            try
            {
                SocialService.AddBookmark(userId, postId, source);

                return Ok();
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
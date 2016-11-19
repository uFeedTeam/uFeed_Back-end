﻿using System.Linq;
using System.Web.Http;
using uFeed.BLL.DTO;
using uFeed.BLL.Enums;
using uFeed.BLL.Interfaces;
using uFeed.WEB.ViewModels.Social.Login;

namespace uFeed.WEB.Controllers
{
    //[Authorize]
    [RoutePrefix("api/social")]
    public class SocialController : ApiController
    {
        private readonly ISocialService _socialService;

        public SocialController(ISocialService socialService)
        {
            _socialService = socialService;
        }

        private const string CLIENT_ID = "5494787";
        private const string REDIRECT_URI = "https://oauth.vk.com/blank.html";

        [HttpGet]
        [Route("vkauth")]
        public IHttpActionResult AuthenticationVk()
        {
            string url =
                $"https://oauth.vk.com/authorize?client_id={CLIENT_ID}&display=popup&redirect_uri={REDIRECT_URI}&scope=groups,audio&response_type=token&v=5.52";
            return Redirect(url);
        }

        private const string AppId = "141340716272867";
        private const string RedirectUri = "https://109.87.37.50/";
        private const string Permissions = "user_likes,user_posts,user_managed_groups";

        [HttpGet]
        [Route("fbauth")]
        public IHttpActionResult AuthenticationFb()
        {
            string url =
                $"https://www.facebook.com/dialog/oauth?client_id={AppId}&redirect_uri={RedirectUri}&scope={Permissions}";
            return Redirect(url);
        }

        [HttpPost]
        public IHttpActionResult Test(SocialLoginViewModel loginModel)
        {
            LoginSocialNetworks(loginModel);

            var userInfoes = _socialService.GetUserInfo();
            var authorsVkAll = _socialService.GetAllAuthors(Socials.Vk);
            var authorsFacebookAll = _socialService.GetAllAuthors(Socials.Facebook);

            var authorsVk = _socialService.GetAuthors(1, 10, Socials.Vk);
            var authorsFacebook = _socialService.GetAuthors(1, 10, Socials.Facebook);

            authorsVk = _socialService.GetAuthors(2, 10, Socials.Vk);
            authorsFacebook = _socialService.GetAuthors(2, 10, Socials.Facebook);

            authorsVk.AddRange(authorsFacebook);

            var socialAuthors = authorsVk.Select(authorDTO => new SocialAuthorDTO
            {
                AuthorId = authorDTO.Id, CategoryId = 1, Source = authorDTO.Source
            }).ToList();

            var feed1 = _socialService.GetFeed(new CategoryDTO { Id = 1, Authors = socialAuthors }, 1, 2);

            var feed2 = _socialService.GetFeed(new CategoryDTO { Id = 1, Authors = socialAuthors }, 2, 2);

            return Ok();
        }

        private void LoginSocialNetworks(SocialLoginViewModel loginModel)
        {
            if (loginModel.FacebookLogin != null)
            {
                _socialService.Login(Socials.Facebook, loginModel.FacebookLogin.Code);
            }

            if (loginModel.VkLogin != null)
            {
                _socialService.Login(Socials.Vk,
                    loginModel.VkLogin.AccessToken,
                    loginModel.VkLogin.ExpiresIn,
                    loginModel.VkLogin.UserId);
            }
        }
    }
}
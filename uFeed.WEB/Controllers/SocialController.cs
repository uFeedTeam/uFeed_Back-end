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
    public class SocialController : ApiController
    {
        private readonly ISocialService _socialService;
        private readonly ICategoryService _categoryService;
        private readonly IClientProfileService _clientProfileService;

        public SocialController(ISocialService socialService, ICategoryService categoryService, IClientProfileService clientProfileService)
        {
            _socialService = socialService;
            _categoryService = categoryService;
            _clientProfileService = clientProfileService;
        }

        [HttpGet]
        [Route("vkauth")]
        [AllowAnonymous]
        public IHttpActionResult AuthenticationVk(string code)
        {
            _socialService.Login(Socials.Vk, code);

            var vkLogin = new VkLoginViewModel
            {
                AccessToken = _socialService.GetToken(Socials.Vk),
                UserId = _socialService.GetUserId(Socials.Vk)
            };

            return Ok(vkLogin);
        }

        [HttpGet]
        [Route("fbauth")]
        [AllowAnonymous]
        public IHttpActionResult AuthenticationFb(string code)
        {
            _socialService.Login(Socials.Facebook, code + "#");

            var facebookLogin = new FacebookLoginViewModel
            {
                AccessToken = _socialService.GetToken(Socials.Facebook)
            };

            return Ok(facebookLogin);
        }

        [HttpPost]
        public IHttpActionResult Test(SocialLoginViewModel loginModel)
        {
            LoginSocialNetworks(loginModel);

            var userInfoes = _socialService.GetUserInfo();
            var authorsVkAll = _socialService.GetAllAuthors(Socials.Vk);
            //var authorsFacebookAll = _socialService.GetAllAuthors(Socials.Facebook);

            var authorsVk = _socialService.GetAuthors(1, 10, Socials.Vk);
            //var authorsFacebook = _socialService.GetAuthors(1, 10, Socials.Facebook);

            authorsVk = _socialService.GetAuthors(2, 10, Socials.Vk);
            //authorsFacebook = _socialService.GetAuthors(2, 10, Socials.Facebook);

            //authorsVk.AddRange(authorsFacebook);

            var socialAuthors = authorsVk.Select(authorDTO => new SocialAuthorDTO
            {
                AuthorId = authorDTO.Id,
                CategoryId = 1,
                Source = authorDTO.Source
            }).ToList();

            var feed1 = _socialService.GetFeed(new CategoryDTO { Id = 1, Authors = socialAuthors }, 1, 2);

            var feed2 = _socialService.GetFeed(new CategoryDTO { Id = 1, Authors = socialAuthors }, 2, 2);

            return Ok();
        }

        [HttpPost]
        [Route("authors")]
        public IHttpActionResult GetAllAuthors(SocialLoginViewModel loginModel)
        {
            LoginSocialNetworks(loginModel);

            var model = new GetAuthorsViewModel();

            var clientProfile = _clientProfileService.Get(User.Identity.GetUserId<int>());

            if (clientProfile.Logins.Contains(Socials.Facebook))
            {
                var fbAuthorsDto = _socialService.GetAllAuthors(Socials.Facebook);
                model.FacebookAuthors = Mapper.Map<IEnumerable<AuthorViewModel>>(fbAuthorsDto);
            }

            if (clientProfile.Logins.Contains(Socials.Vk))
            {
                var vkAuthorsDto = _socialService.GetAllAuthors(Socials.Vk);
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
                LoginSocialNetworks(loginModel);

                var model = new GetAuthorsViewModel();

                var clientProfile = _clientProfileService.Get(User.Identity.GetUserId<int>());

                if (clientProfile.Logins.Contains(Socials.Facebook))
                {
                    List<AuthorDTO> fbAuthorsDto = _socialService.GetAuthors(page, count, Socials.Facebook);
                    model.FacebookAuthors = Mapper.Map<IEnumerable<AuthorViewModel>>(fbAuthorsDto);
                }

                if (clientProfile.Logins.Contains(Socials.Vk))
                {
                    List<AuthorDTO> vkAuthorsDto = _socialService.GetAuthors(page, count, Socials.Vk);
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
            LoginSocialNetworks(loginModel);

            try
            {
                var categoryDto = _categoryService.Get(categoryId);
                var feed = _socialService.GetFeed(categoryDto, page, postsPerGroup);

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


        private void LoginSocialNetworks(SocialLoginViewModel loginModel)
        {
            if (loginModel.FacebookLogin != null)
            {
                _socialService.Login(Socials.Facebook, loginModel.FacebookLogin.AccessToken, isAccessToken: true);
            }

            if (loginModel.VkLogin != null)
            {
                _socialService.Login(Socials.Vk, loginModel.VkLogin.AccessToken, loginModel.VkLogin.UserId, true);
            }
        }
    }
}

using System;
using System.Web.Http;
using uFeed.BLL.Interfaces;

namespace uFeed.WEB.Controllers
{
    [Authorize]
    public class SocialController : ApiController
    {
        private readonly ISocialService _socialService;

        public SocialController(ISocialService socialService)
        {
            _socialService = socialService;
        }

        private const string CLIENT_ID = "5494787";
        private const string REDIRECT_URI = "https://oauth.vk.com/blank.html";

        public IHttpActionResult AuthenticationVk()
        {
            string url =
                $"https://oauth.vk.com/authorize?client_id={CLIENT_ID}&display=popup&redirect_uri={REDIRECT_URI}&scope=groups,audio&response_type=token&v=5.52";
            return Redirect(url);
        }

        private const string AppId = "141340716272867";
        private const string RedirectUri = "https://109.87.37.50/";
        private const string Permissions = "user_likes,user_posts,user_managed_groups";


        public IHttpActionResult AuthenticationFb()
        {
            string url =
                $"https://www.facebook.com/dialog/oauth?client_id={AppId}&redirect_uri={RedirectUri}&scope={Permissions}";
            return Redirect(url);
        }

        public IHttpActionResult Login(string accessFb, string accessVk, string id, int? expiresIn)
        {
            _socialService.Login("vk", accessVk, Session, expiresIn, id);
            _socialService.Login("facebook", accessFb + "#", Session);

            var userInfoes = _feedService.GetUserInfo();
            var authorsVkAll = _feedService.GetAllAuthors("vk");
            var authorsFacebookAll = _feedService.GetAllAuthors("facebook");

            var authorsVk = _feedService.GetAuthors(10, "vk");
            var authorsFacebook = _feedService.GetAuthors(10, "facebook");

            _feedService.GetNextAuthors(authorsVk, "vk");
            _feedService.GetNextAuthors(authorsFacebook, "facebook");

            authorsVk.AddRange(authorsFacebook);

            var feed = _feedService.GetFeed(new CategoryDTO { Authors = authorsVk }, 2);

            _feedService.GetNextFeed(feed);

            return View("Index", _feedService.GetUserInfo());
        }
    }
}

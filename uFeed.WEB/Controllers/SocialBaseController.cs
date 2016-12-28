using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using uFeed.BLL.Enums;
using uFeed.BLL.Interfaces;

namespace uFeed.WEB.Controllers
{
    public class SocialBaseController : ApiController
    {
        protected ISocialService SocialService
        {
            get
            {
                if (_socialService != null)
                {
                    return _socialService;
                }

                _socialService = DependencyResolver.Current.GetService<ISocialService>();

                IEnumerable<string> header;

                ControllerContext.Request.Headers.TryGetValues(FacebookTokenHeaderName, out header);
                string fbToken = header?.FirstOrDefault();

                ControllerContext.Request.Headers.TryGetValues(VkTokenHeaderName, out header);
                string vkToken = header?.FirstOrDefault();

                ControllerContext.Request.Headers.TryGetValues(VkUserIdHeaderName, out header);
                string vkUserId = header?.FirstOrDefault();

                if (!string.IsNullOrEmpty(fbToken))
                {
                    _socialService.Login(Socials.Facebook, fbToken, isAccessToken: true);
                }

                if (!string.IsNullOrEmpty(vkToken) && !string.IsNullOrEmpty(vkUserId))
                {
                    _socialService.Login(Socials.Vk, vkToken, vkUserId, true);
                }

                return _socialService;
            }
        }

        private const string FacebookTokenHeaderName = "Facebook_AccessToken";
        private const string VkTokenHeaderName = "Vk_AccessToken";
        private const string VkUserIdHeaderName = "Vk_UserId";

        private ISocialService _socialService;
    }
}
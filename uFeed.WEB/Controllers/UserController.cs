using System.Net;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using uFeed.BLL.Enums;
using uFeed.BLL.Interfaces;

namespace uFeed.WEB.Controllers
{
    [Authorize]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IClientProfileService _clientProfileService;

        public UserController(IClientProfileService clientProfileService)
        {
            _clientProfileService = clientProfileService;
        }

        [HttpPost]
        [Route("addlogin")]
        public IHttpActionResult AddLogin([FromBody] Socials socialNetwork)
        {
            switch (socialNetwork)
            {
                case Socials.Facebook:
                    _clientProfileService.AddLogin(User.Identity.GetUserId<int>(), socialNetwork);
                    break;
                case Socials.Vk:
                    _clientProfileService.AddLogin(User.Identity.GetUserId<int>(), socialNetwork);
                    break;
                default:
                    return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.OK);
        }

        [HttpPost]
        [Route("removelogin")]
        public IHttpActionResult RemoveLogin([FromBody] Socials socialNetwork)
        {
            switch (socialNetwork)
            {
                case Socials.Facebook:
                    _clientProfileService.RemoveLogin(User.Identity.GetUserId<int>(), socialNetwork);
                    break;
                case Socials.Vk:
                    _clientProfileService.RemoveLogin(User.Identity.GetUserId<int>(), socialNetwork);
                    break;
                default:
                    return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.OK);
        }

        //public void Post([FromBody]string value)
        //{
        //}

        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //public void Delete(int id)
        //{
        //}
    }
}

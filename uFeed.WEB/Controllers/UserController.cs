using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using uFeed.BLL.Enums;
using uFeed.BLL.Infrastructure.Exceptions;
using uFeed.BLL.Interfaces;
using uFeed.WEB.ViewModels;

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

        [HttpGet]
        [Route("get")]
        public IHttpActionResult Get()
        {
            return Json(_clientProfileService.Get(User.Identity.GetUserId<int>()));
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

        [HttpPut]
        [Route("newname")]
        public IHttpActionResult EditName([FromBody] ClientProfileViewModel user)
        {
            var accountId = User.Identity.GetUserId<int>();

            if (string.IsNullOrEmpty(user?.Name))
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            var userAccount = _clientProfileService.Get(accountId);
            userAccount.Name = user.Name;

            try
            {
                _clientProfileService.Update(userAccount);
            }
            catch (ServiceException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.OK);
        }

        [HttpPut]
        [Route("newphoto")]
        public IHttpActionResult NewPhoto([FromBody] byte[] file)
        {
            var accountId = User.Identity.GetUserId<int>();

            if (file == null)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            var userAccount = _clientProfileService.Get(accountId);

            userAccount.Photo = file;
            try
            {
                _clientProfileService.Update(userAccount);
            }
            catch (ServiceException)
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }

            return StatusCode(HttpStatusCode.OK);
        }

        //Getting array of bytes from posted image
        private static byte[] GetBytesFromFile(HttpPostedFile file)
        {
            using (var inputStream = file.InputStream)
            {
                var memoryStream = inputStream as MemoryStream;

                if (memoryStream != null) return memoryStream.ToArray();

                memoryStream = new MemoryStream();
                inputStream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        //Convert array of bytes to image
        public static Image ConvertToImage(byte[] arrayOfBytes)
        {
            Image image;

            using (var ms = new MemoryStream(arrayOfBytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
    }
}

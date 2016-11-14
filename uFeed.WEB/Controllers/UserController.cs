using System.Drawing;
using System.IO;
using System.Net;
using System.Web;
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
        //    _clientProfileService.Delete(id);
        //}

        [HttpPost]
        [Route("newname")]
        public void EditName(string name)
        {
            const int someAccountId = 1;

            var userAccount = _clientProfileService.Get(someAccountId);
            userAccount.Name = name;

            _clientProfileService.Update(userAccount);
        }

        [HttpPost]
        [Route("newphoto")]
        public void NewPhoto(HttpPostedFile file)
        {
            const int someAccountId = 1;

            if (file == null) return;

            var data = GetBytesFromFile(file);
            var userAccount = _clientProfileService.Get(someAccountId);

            userAccount.Photo = data;

            _clientProfileService.Update(userAccount);
        }

        //Getting array of bytes from posted image
        private static byte [] GetBytesFromFile(HttpPostedFile file)
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

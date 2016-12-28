using System.Web.Http;
using Ninject;
using uFeed.WEB.Account.Interfaces;
using uFeed.WEB.ViewModels;

namespace uFeed.WEB.Controllers
{
    public class BaseController : ApiController
    {
        [Inject]
        public IAuthentication Auth { get; set; }

        public UserViewModel CurrentUser => ((IUserProvider)Auth.CurrentUser.Identity).User;
    }
}
using System.Security.Principal;
using uFeed.BLL.Interfaces;

namespace uFeed.WEB.Account.Implementation
{
    public class UserProvider : IPrincipal
    {
        private readonly UserIdentity _userIdentity;
        private readonly IUserService _userService;

        public UserProvider(string name, IUserService userService)
        {
            _userService = userService;
            _userIdentity = new UserIdentity(name, userService);       
        }

        public IIdentity Identity => _userIdentity;

        public bool IsInRole(string role)
        {
            return true;
        }

        public override string ToString()
        {
            return _userIdentity.Name;
        }
    }
}
using System.Security.Principal;
using AutoMapper;
using uFeed.BLL.DTO;
using uFeed.BLL.Interfaces;
using uFeed.WEB.Account.Interfaces;
using uFeed.WEB.ViewModels;

namespace uFeed.WEB.Account.Implementation
{
    public class UserIdentity : IIdentity, IUserProvider
    {
        public UserIdentity(string email, IUserService userService)
        {
            if (string.IsNullOrEmpty(email))
            {
                return;
            }

            var userDto = userService.GetByEmail(email);
            User = Mapper.Map<UserDTO, UserViewModel>(userDto);
        }

        public UserViewModel User { get; set; }

        public string AuthenticationType => typeof(UserViewModel).ToString();

        public bool IsAuthenticated => User != null;

        public string Name => User?.Name;
    }
}
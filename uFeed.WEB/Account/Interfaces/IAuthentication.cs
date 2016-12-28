using System.Security.Principal;
using System.Web;
using uFeed.WEB.Account.Implementation;
using uFeed.WEB.ViewModels;

namespace uFeed.WEB.Account.Interfaces
{
    public interface IAuthentication
    {
        HttpContext HttpContext { get; set; }

        IPrincipal CurrentUser { get; }

        UserViewModel Login(string login, string password, bool isPersistent);

        TokenInfo Token(string email, string password);

        void LogOut();
    }
}
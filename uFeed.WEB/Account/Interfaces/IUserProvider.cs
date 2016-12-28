using uFeed.WEB.ViewModels;

namespace uFeed.WEB.Account.Interfaces
{
    public interface IUserProvider
    {
        UserViewModel User { get; set; }
    }
}
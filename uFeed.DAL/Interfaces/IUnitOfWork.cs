using System;
using uFeed.DAL.Repositories;
using uFeed.Entities;

namespace uFeed.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ClientProfileRepository ClientProfiles { get; }

        IRepository<Category> Categories { get; }

        IRepository<Login> Logins { get; }

        IRepository<SocialAuthor> SocialAuthors { get; }

        IRepository<ClientBookmark> ClientBookmarks { get; }

        void Save();
    }
}

using System;
using uFeed.Entities;

namespace uFeed.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ClientProfile> ClientProfiles { get; }

        IRepository<Category> Categories { get; }

        IRepository<Login> Logins { get; }

        void Save();
    }
}

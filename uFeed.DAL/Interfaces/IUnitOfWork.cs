using System;
using uFeed.DAL.Entities;

namespace uFeed.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ClientProfile> ClientProfiles { get; }

        IRepository<Category> Categories { get; }

        void Save();
    }
}

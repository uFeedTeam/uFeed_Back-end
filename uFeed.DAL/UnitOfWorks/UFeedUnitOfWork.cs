using System;
using uFeed.DAL.EF;
using uFeed.DAL.Entities;
using uFeed.DAL.Interfaces;
using uFeed.DAL.Repositories;

namespace uFeed.DAL.UnitOfWorks
{
    public class UFeedUnitOfWork : IUnitOfWork
    {
        private readonly UFeedContext _db;
        private bool _disposed;

        private IRepository<ClientProfile> _clientProfileRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<Login> _loginRepository;

        public UFeedUnitOfWork(string connectionString)
        {
            _db = new UFeedContext(connectionString);
        }

        public IRepository<ClientProfile> ClientProfiles => _clientProfileRepository ?? (_clientProfileRepository = new CommonRepository<ClientProfile>(_db));

        public IRepository<Category> Categories => _categoryRepository ?? (_categoryRepository = new CommonRepository<Category>(_db));

        public IRepository<Login> Logins => _loginRepository ?? (_loginRepository = new CommonRepository<Login>(_db));

        public void Save()
        {
            _db.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
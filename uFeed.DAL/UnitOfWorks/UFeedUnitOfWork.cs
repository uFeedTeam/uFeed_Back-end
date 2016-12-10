using System;
using uFeed.DAL.EF;
using uFeed.DAL.Interfaces;
using uFeed.DAL.Repositories;
using uFeed.Entities;

namespace uFeed.DAL.UnitOfWorks
{
    public class UFeedUnitOfWork : IUnitOfWork
    {
        private readonly UFeedContext _db;
        private bool _disposed;

        private ClientProfileRepository _clientProfileRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<Login> _loginRepository;
        private IRepository<SocialAuthor> _socialAuthorRepository;
        private IRepository<ClientBookmark> _clientBookmarkRepository;

        public UFeedUnitOfWork(string connectionString)
        {
            _db = new UFeedContext(connectionString);
        }

        public ClientProfileRepository ClientProfiles => _clientProfileRepository ?? (_clientProfileRepository = new ClientProfileRepository(_db));

        public IRepository<Category> Categories => _categoryRepository ?? (_categoryRepository = new CommonRepository<Category>(_db));

        public IRepository<Login> Logins => _loginRepository ?? (_loginRepository = new CommonRepository<Login>(_db));

        public IRepository<SocialAuthor> SocialAuthors => _socialAuthorRepository ?? (_socialAuthorRepository = new CommonRepository<SocialAuthor>(_db));
        public IRepository<ClientBookmark> ClientBookmarks => _clientBookmarkRepository ?? (_clientBookmarkRepository = new CommonRepository<ClientBookmark>(_db));

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
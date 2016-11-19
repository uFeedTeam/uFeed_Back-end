using System;
using System.Data.Entity;
using System.Linq;
using uFeed.DAL.Interfaces;
using uFeed.Entities;

namespace uFeed.DAL.Repositories
{
    public class CommonRepository<TEntity> : IRepository<TEntity> where TEntity : BaseType
    {
        private readonly DbContext _context;

        public CommonRepository(DbContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public IQueryable<TEntity> Find(Func<TEntity, bool> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).AsQueryable();
        }

        public void Create(TEntity item)
        {
            _context.Set<TEntity>().Add(item);
        }

        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            var item = Get(id);
            _context.Set<TEntity>().Remove(item);
        }
    }
}
using ChatApp_Api.Interfaces;
using ChatApp_Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace ChatApp_Api.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _Context;

        public BaseRepository(ApplicationDbContext Context)
        {
            _Context = Context;
        }

        public async Task<T> Add(T entity)
        {
            await _Context.Set<T>().AddAsync(entity);
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            _Context.Set<T>().Remove(entity);
            return entity;
        }

        public async Task<T> Update(T entity)
        {
            _Context.Set<T>().Update(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _Context.Set<T>().ToListAsync();
        }

        public async Task<T> FindById(int id)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;

        }
        public async Task<T> FindByIdWithData(int id)
        {
            var col = GetCollections(typeof(T));
            var entry = await FindById(id);
            var re = _Context.Entry(entry);
            IQueryable<T> query = _Context.Set<T>();
            foreach (var inc in col)
            {
                query = query.Include(inc).AsQueryable();
            }
            var entity = await query.Where(d => d.Equals(entry)).SingleOrDefaultAsync();
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<T> FindById(string id)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;

        }
        public async Task<T> FindByIdWithData(string id)
        {
            var col = GetCollections(typeof(T));
            var entry = await FindById(id);
            IQueryable<T> query = _Context.Set<T>();
            foreach (var inc in col)
            {
                query = query.Include(inc).AsQueryable();
            }
            var entity = await query.Where(d => d.Equals(entry)).SingleOrDefaultAsync();
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<int> CountWithCriteria(Expression<Func<T, bool>> criteria)
        {
            return await _Context.Set<T>().Where(criteria).CountAsync();
        }

        public async Task<int> Count()
        {
            return await _Context.Set<T>().CountAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithData()
        {
            var query = _Context.Set<T>().AsQueryable();
            var includes = GetCollections(typeof(T));
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }
            return await query.ToListAsync();
        }

        public async Task<T> Find(Expression<Func<T, bool>> criteria)
        {

            var result = await _Context.Set<T>().AsQueryable().AsNoTracking().FirstOrDefaultAsync(criteria);
            if (result == null) return null;
            _Context.Entry(result).State = EntityState.Detached;
            return result;

        }
        public async Task<T> FindWithData(Expression<Func<T, bool>> criteria)
        {
            var col = GetCollections(typeof(T));
            var query = _Context.Set<T>().AsQueryable();
            foreach (var inc in col)
            {
                query = query.Include(inc);
            }
            var entity = await query.SingleOrDefaultAsync(criteria);
            if (entity == null) return null;
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria)
        {
            return await _Context.Set<T>().AsQueryable().Where(criteria).ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllWithData(Expression<Func<T, bool>> criteria)
        {
            var col = GetCollections(typeof(T));
            var query = _Context.Set<T>().AsQueryable().Where(criteria);

            foreach (var inc in col)
            {
                query = query.Include(inc);
            }
            return await query.ToListAsync();
        }

        public async Task<T> FindByIdWithCustomData(int id, string Include)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).Collection(Include).Load();
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }
        public async Task<T> FindByIdWithCustomData(string id, string Include)
        {
            var entity = await _Context.Set<T>().FindAsync(id);
            if (entity == null) return null;
            _Context.Entry(entity).Collection(Include).Load();
            _Context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public void CommitChanges()
        {
            _Context.SaveChanges();
        }
        public List<string> GetCollections(Type entityType)
        {
            var col = entityType.GetProperties()
                                .Where(p => (typeof(IEnumerable).IsAssignableFrom(p.PropertyType)
                                    && p.PropertyType != typeof(string))
                                    && p.PropertyType != typeof(byte[])
                                    || (p.PropertyType.Namespace == entityType.Namespace))
                                .Select(p => p.Name)
                                .ToList();
            return col;

        }
    }
}

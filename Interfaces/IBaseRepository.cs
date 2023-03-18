using System.Linq.Expressions;

namespace ChatApp_Api.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<T> FindById(int id);
        Task<T> FindByIdWithData(int id);
        Task<T> FindById(string id);
        Task<T> FindByIdWithData(string id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> GetAllWithData();
        Task<int> Count();
        Task<int> CountWithCriteria(Expression<Func<T, bool>> criteria);
        Task<T> Find(Expression<Func<T, bool>> criteria);
        Task<T> FindWithData(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> FindAll(Expression<Func<T, bool>> criteria);
        Task<IEnumerable<T>> FindAllWithData(Expression<Func<T, bool>> criteria);
        Task<T> FindByIdWithCustomData(int id, string Include);
        Task<T> FindByIdWithCustomData(string id, string Include);
        List<string> GetCollections(Type entityType);
        void CommitChanges();

    }
}

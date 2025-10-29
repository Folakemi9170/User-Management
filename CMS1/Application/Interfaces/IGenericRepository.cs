using System.Linq.Expressions;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        IEnumerable<T> GetAllByCondition(Expression<Func<T, bool>>? expression = null, List<string>? includes = null);
        Task<T?> GetByConditionAsync(Expression<Func<T, bool>> predicate, List<string>? includes = null);
        T Get(long id);
        int Insert(T entity);
        void InsertRange(List<T> entity);
        void Update(T entity);
        void UpdateRange(List<T> entity);
        bool Delete(long id);
        bool DeleteAll(List<T> entity);
        bool Remove(long Id);
        Task<T> GetWithIncludesAsync(int id, params Expression<Func<T, object>>[] includes);
        public Task<IEnumerable<T>> GetAllWithIncludesAsync(params Expression<Func<T, object>>[] includes);

    }

}
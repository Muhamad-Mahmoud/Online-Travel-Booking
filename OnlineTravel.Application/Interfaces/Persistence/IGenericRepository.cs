using System.Linq.Expressions;
using OnlineTravel.Application.Interfaces.Specifications;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IGenericRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T?> GetEntityWithAsync(ISpecification<T> spec);
        Task<int> GetCountAsync(ISpecification<T> spec);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
    }
}

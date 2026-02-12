using System.Linq.Expressions;
using OnlineTravel.Application.Interfaces.Specifications;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IGenericRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<T?> GetEntityWithAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Delete(T entity);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}

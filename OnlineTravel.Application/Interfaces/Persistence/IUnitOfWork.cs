using OnlineTravel.Domain.Entities._Base;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> Complete();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task SaveChangesAsync();
    }
}

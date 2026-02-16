using OnlineTravel.Domain.Entities._Base;
using Microsoft.EntityFrameworkCore.Storage;

namespace OnlineTravel.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IHotelRepository Hotels { get; }
        IRoomRepository Rooms { get; }
        IGenericRepository<T> Repository<T>() where T : BaseEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<int> Complete();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
       
    }
}

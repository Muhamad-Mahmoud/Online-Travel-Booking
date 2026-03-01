using OnlineTravel.Domain.Entities._Base;

namespace OnlineTravel.Application.Interfaces.Persistence
{
	public interface IUnitOfWork : IDisposable
	{
		IHotelRepository Hotels { get; }
		IRoomRepository Rooms { get; }
		IGenericRepository<T> Repository<T>() where T : BaseEntity;
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
		Task<IAsyncDisposable> BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();

	}
}

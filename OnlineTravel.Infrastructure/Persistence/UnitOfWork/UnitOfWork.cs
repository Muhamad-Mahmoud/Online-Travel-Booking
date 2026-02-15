using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Domain.Entities.Hotels;
using OnlineTravel.Infrastructure.Persistence.Context;
using OnlineTravel.Infrastructure.Persistence.Repositories;
using System.Collections;

namespace OnlineTravel.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly OnlineTravelDbContext _dbContext;
        private Hashtable _repositories;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

        public IHotelRepository Hotels { get; }
        public IRoomRepository Rooms { get; }
        public UnitOfWork(OnlineTravelDbContext dbContext)
        {
            _dbContext = dbContext;
            Hotels = new HotelRepository(_dbContext);
            Rooms = new RoomRepository(_dbContext);
           // Bookings = new BookingRepository(_context);
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_repositories is null)
                _repositories = new Hashtable();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<T>(_dbContext);
                _repositories.Add(type, repository);

            }

            return _repositories[type] as GenericRepository<T>;

        }
        public async Task<int> Complete()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _dbContext.Dispose();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _transaction.CommitAsync();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }
    }
}

using System.Collections;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Infrastructure.Persistence.Context;
using OnlineTravel.Infrastructure.Persistence.Repositories;

namespace OnlineTravel.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly OnlineTravelDbContext _dbContext;
        private Hashtable _repositories;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

        public UnitOfWork(OnlineTravelDbContext dbContext)
        {
            _dbContext = dbContext;
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

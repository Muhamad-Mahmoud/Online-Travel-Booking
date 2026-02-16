using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using OnlineTravel.Application.Interfaces.Persistence;
using OnlineTravel.Application.Interfaces.Specifications;
using OnlineTravel.Domain.Entities._Base;
using OnlineTravel.Infrastructure.Persistence.Context;
using OnlineTravel.Infrastructure.Persistence.Specifications;

namespace OnlineTravel.Infrastructure.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {

        private readonly OnlineTravelDbContext _dbContext;

        public GenericRepository(OnlineTravelDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().ToListAsync(cancellationToken);
        }



        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecifications(spec).ToListAsync(cancellationToken);
        }


        public async Task<T?> GetEntityWithAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync(cancellationToken);

        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate, cancellationToken);
        }



        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }


        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        } 

        
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec, CancellationToken cancellationToken = default)
        {
            return await ApplySpecifications(spec).CountAsync(cancellationToken);
        }


        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

    }
}

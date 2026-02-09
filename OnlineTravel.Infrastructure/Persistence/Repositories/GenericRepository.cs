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


        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }



        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).ToListAsync();
        }


        public async Task<T?> GetEntityWithAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();

        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }



        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
        }


        public void Update(T entity)
        {
            _dbContext.Set<T>().Update(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
        }

        public async Task<int> GetCountAsync(ISpecification<T> spec)
        {
            return await ApplySpecifications(spec).CountAsync();
        }


        private IQueryable<T> ApplySpecifications(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repository;
using Talabat.Core.Specification;
using Talabt.Reporisitory.Data;

namespace Talabt.Reporisitory
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }

         async Task<IReadOnlyList<T>> IGenericRepository<T>.GetAll()
        => await _dbContext.Set<T>().ToListAsync();
         async Task<T> IGenericRepository<T>.GetById(int id)
        => await _dbContext.Set<T>().FindAsync(id);

         async Task<IReadOnlyList<T>> IGenericRepository<T>.GetAll(ISpecification<T> specification)
         => await ApplySpecification(specification).ToListAsync();

         async Task<T> IGenericRepository<T>.GetEntitybySpec(ISpecification<T> specification)
           => await ApplySpecification(specification).FirstOrDefaultAsync();
        
        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
         => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);

       async Task<int> IGenericRepository<T>.GetCount(ISpecification<T> specification)
        
        => await ApplySpecification(specification).CountAsync();

        public void Add(T entity)
        => _dbContext.Set<T>().Add(entity);

        public void Update(T entity)
        => _dbContext.Set<T>().Update(entity);

        public void Delete(T entity)
        => _dbContext.Set<T>().Remove(entity);
    }
}

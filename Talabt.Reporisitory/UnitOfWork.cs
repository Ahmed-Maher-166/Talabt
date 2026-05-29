using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repository;
using Talabt.Reporisitory.Data;

namespace Talabt.Reporisitory
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext dbContext)
        {
            _dbContext = dbContext;

            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();
        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>()
            where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repository =
                    new GenericRepository<TEntity>(_dbContext);
                _repositories.Add(type, repository);
            }

            return _repositories[type] as IGenericRepository<TEntity>;
        }
    }
}

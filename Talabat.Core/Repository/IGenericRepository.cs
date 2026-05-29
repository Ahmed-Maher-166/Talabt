using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specification;

namespace Talabat.Core.Repository
{
    public interface IGenericRepository<T> where T : BaseEntity
    {

        #region Without Specification
        Task<IReadOnlyList<T>> GetAll();
        Task<T> GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        #endregion
        #region With Specification
        public   Task<IReadOnlyList<T>> GetAll(ISpecification<T> specification);
        public Task<T> GetEntitybySpec(ISpecification<T> specification);
        public Task<int> GetCount(ISpecification<T> specification);

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specification
{
    public interface ISpecification<T> where T : BaseEntity
    {
        Expression<Func<T, bool>> Criteria { get; set; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>> OrederByASC { get; set; }
        Expression<Func<T, object>> OrederByDSC { get; set; }
        public bool IsPagination { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }

    }
}

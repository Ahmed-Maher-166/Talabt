using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specification;

namespace Talabt.Reporisitory
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(
            IQueryable<T> inputQuery,
            ISpecification<T> spec)
        {
            var query = inputQuery;

            // Apply Where
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrederByASC != null)
            {
                query = query.OrderBy(spec.OrederByASC);
            }
            if (spec.OrederByDSC != null)
            {
                query = query.OrderByDescending(spec.OrederByDSC);
            }
            if (spec.IsPagination == true)
            {
                query =query.Skip(spec.Skip).Take(spec.Take);
            }
                // Apply Includes
                query = spec.Includes.Aggregate(
                query,
                (currentQuery, includeExpression) =>
                    currentQuery.Include(includeExpression)
            );

            return query;
        }
    }
}
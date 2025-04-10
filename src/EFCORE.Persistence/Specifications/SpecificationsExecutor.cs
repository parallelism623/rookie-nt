
using EFCORE.Domain.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Persistence.Specifications;

public class SpecificationsExecutor
{
    public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
    IQueryable<TEntity> inputQueryable,
    Specification<TEntity, TKey> specification)
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
    {
        IQueryable<TEntity> queryable = inputQueryable;

        if (specification.Criteria is not null)
        {
            queryable = queryable.Where(specification.Criteria);
        }

        if(specification.IncludeExpressions.Count > 0)
        {
            specification.IncludeExpressions.Aggregate(
                                queryable,
                                (current, includeExpression) =>
                                    current.Include(includeExpression));
        }    

        if (specification.OrderByExpression is not null)
        {
            queryable = queryable.OrderBy(specification.OrderByExpression);
        }
        else if (specification.OrderByDescendingExpression is not null)
        {
            queryable = queryable.OrderByDescending(
                specification.OrderByDescendingExpression);
        }

        if (specification.IsSplitQuery)
        {
            queryable = queryable.AsSplitQuery();
        }

        return queryable;
    }
}

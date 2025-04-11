
using EFCORE.Domain.Abstract;
using Microsoft.EntityFrameworkCore;

namespace EFCORE.Persistence.Specifications;

public static class SpecificationsExecutor
{
    public static IQueryable<TEntity> GetQuery<TEntity, TKey>(
    this Specification<TEntity, TKey> specification,
    IQueryable<TEntity> inputQueryable)
    where TEntity : class, IEntity<TKey>
    where TKey : notnull
    {
        IQueryable<TEntity> queryable = inputQueryable;



        if(specification.IncludeExpressions.Count > 0)
        {
            queryable = specification.IncludeExpressions.Aggregate(
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

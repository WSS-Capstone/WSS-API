using System.Linq.Expressions;
using L.Core.Helpers.Paging.Object;

namespace L.Core.Helpers.Paging;

public static class SortingHelper
{
    public static IQueryable<TObject> GetWithSorting<TObject>(this IQueryable<TObject>? source,
        string? sortKey, PagingConstants.OrderCriteria sortOrder = PagingConstants.OrderCriteria.ASC)
        where TObject : class
    {
        if (source == null) return Enumerable.Empty<TObject>().AsQueryable();

        if (sortKey != null)
        {
            var param = Expression.Parameter(typeof(TObject), "p");
            var prop = Expression.Property(param, sortKey);
            var exp = Expression.Lambda(prop, param);
            string method;
            switch (sortOrder)
            {
                case PagingConstants.OrderCriteria.ASC:
                    method = "OrderBy";
                    break;
                default:
                    method = "OrderByDescending";
                    break;
            }

            Type[] types = new[] { source.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, source.Expression, exp);
            return source.Provider.CreateQuery<TObject>(mce);
        }

        return source;
    }
}
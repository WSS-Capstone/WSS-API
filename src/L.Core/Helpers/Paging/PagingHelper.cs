using L.Core.Helpers.Paging.Object;

namespace L.Core.Helpers.Paging;

public static class PagingHelper
{
    public static IEnumerable<TObject> GetWithPaging<TObject>(this IEnumerable<TObject>? source, int page, int pageSize,
        int safePageSizeLimit = PagingConstants.MaxPageSize)
        where TObject : class
    {
        if (pageSize > safePageSizeLimit)
        {
            throw new Exception("Input page size is over safe limitation.");
        }

        if (source == null)
        {
            return Enumerable.Empty<TObject>();
        }

        pageSize = pageSize < 1 ? 1 : pageSize;
        page = page < 1 ? 1 : page;

        source = source
            .Skip(page == 1 ? 0 : pageSize * (page - 1)) // Paging
            .Take(pageSize); // Take only a number of items

        return source;
    }
    
    public static IQueryable<TObject> GetWithPaging<TObject>(this IQueryable<TObject>? source, int page, int pageSize,
        int safePageSizeLimit = PagingConstants.MaxPageSize)
        where TObject : class
    {
        if (pageSize > safePageSizeLimit)
        {
            throw new Exception("Input page size is over safe limitation.");
        }

        if (source == null)
        {
            return Enumerable.Empty<TObject>().AsQueryable();
        }

        pageSize = pageSize < 1 ? 1 : pageSize;
        page = page < 1 ? 1 : page;

        source = source
            .Skip(page == 1 ? 0 : pageSize * (page - 1)) // Paging
            .Take(pageSize); // Take only a number of items

        return source;
    }
}
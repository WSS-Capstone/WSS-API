namespace L.Core.Helpers.Paging.Object;

public class PagingResponse<T, TKey> where TKey : System.Enum
{
    public PagingResponse(PagingParam<TKey> pagingParam, List<T> data, int total)
    {
        this.Data = data;
        this.Page = pagingParam.Page;
        this.Size = pagingParam.PageSize;
        this.Total = total;
    }

    public List<T>? Data { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public int Total { get; set; }
}

public class PagingResponseQuery<T, TKey> where TKey : System.Enum
{
    public PagingResponseQuery(PagingParam<TKey> pagingParam, IQueryable<T> data, int total)
    {
        this.Data = data;
        this.Page = pagingParam.Page;
        this.Size = pagingParam.PageSize;
        this.Total = total;
    }
  
    

    public IQueryable<T>? Data { get; set; }
    public int Page { get; set; }
    public int Size { get; set; }
    public int Total { get; set; }
}
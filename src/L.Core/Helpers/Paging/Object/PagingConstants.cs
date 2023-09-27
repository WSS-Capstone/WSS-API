namespace L.Core.Helpers.Paging.Object;

public static class PagingConstants
{
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 50;
    public const string DefaultSort = "id_asc";
    public const int MinPage = 0;
    public const int MinPageSize = 3;
    public const int MaxPageSize = 250;
        
    public enum OrderCriteria
    {
        /// <summary>
        /// descendant
        /// </summary>
        DESC,

        /// <summary>
        /// ascendant
        /// </summary>
        ASC,
    }
}
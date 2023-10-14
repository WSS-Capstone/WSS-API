namespace WSS.API.Application.Queries.CurrentPrice;

public class GetCurrentPricesQuery : PagingParam<CurrentPriceSortCriteria>,
    IRequest<PagingResponseQuery<CurrentPriceResponse, CurrentPriceSortCriteria>>
{
}

public enum CurrentPriceSortCriteria
{
    DateOfApply,
    Price,
    CreateDate
}
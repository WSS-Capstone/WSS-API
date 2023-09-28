namespace WSS.API.Application.Queries.Customer;

public class GetCustomersQuery: PagingParam<CustomerSortCriteria>, IRequest<PagingResponseQuery<CustomerResponse, CustomerSortCriteria>>
{
    
}

public enum CustomerSortCriteria
{
    Id,
    Fullname,
}
namespace WSS.API.Application.Queries.Service;

public class GetServicesQuery : PagingParam<ServiceSortCriteria>, IRequest<PagingResponseQuery<ServiceResponse, ServiceSortCriteria>>
{
    
}

public enum ServiceSortCriteria
{
    Id,
    Name,
    Quantity,
    Status
}
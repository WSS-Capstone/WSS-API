namespace WSS.API.Application.Queries.Partner;

public class GetPartnersQuery : PagingParam<PartnerSortCriteria>, IRequest<PagingResponseQuery<PartnerResponse, PartnerSortCriteria>>
{
    
}

public enum PartnerSortCriteria
{
    Id,
    Fullname,
    RoleId
}
using WSS.API.Data.Repositories.Cart;

namespace WSS.API.Application.Queries.Cart;

public class GetMyCartQuery :  PagingParam<CartSortCriteria>, IRequest<PagingResponseQuery<CartResponse, CartSortCriteria>>
{
    public Guid UserId { get; set; }
}

public enum CartSortCriteria
{
    Id,
    ServiceId,
}

public class GetMyCartQueryHandler : IRequestHandler<GetMyCartQuery, PagingResponseQuery<CartResponse, CartSortCriteria>>
{
    private IMapper _mapper;
    private ICartRepo _cartRepo;

    public GetMyCartQueryHandler(IMapper mapper, ICartRepo cartRepo)
    {
        _mapper = mapper;
        _cartRepo = cartRepo;
    }

    public async Task<PagingResponseQuery<CartResponse, CartSortCriteria>> Handle(GetMyCartQuery request, CancellationToken cancellationToken)
    {
        var query = this._cartRepo.GetCarts(c => c.UserId == request.UserId);
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<CartResponse>(query);

        return new PagingResponseQuery<CartResponse, CartSortCriteria>(request, result, total);
    }
}
using WSS.API.Data.Repositories.Voucher;

namespace WSS.API.Application.Queries.Voucher;

public class GetVoucherByIdQuery : IRequest<VoucherResponse>
{
    public GetVoucherByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
public class GetVoucherByIdQueryHandler : IRequestHandler<GetVoucherByIdQuery, VoucherResponse>
{
    private IMapper _mapper;
    private IVoucherRepo _repo;

    public GetVoucherByIdQueryHandler(IMapper mapper, IVoucherRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<VoucherResponse> Handle(GetVoucherByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetVoucherById(request.Id);
        var result = this._mapper.Map<VoucherResponse>(query);

        return result;
    }
}
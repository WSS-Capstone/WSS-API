using WSS.API.Data.Repositories.staff;

namespace WSS.API.Application.Queries.Staff;

public class GetStaffByIdQuery : IRequest<StaffResponse>
{
    public GetStaffByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}

public class GetStaffByIdQueryHandler : IRequestHandler<GetStaffByIdQuery, StaffResponse>
{
    private IMapper _mapper;
    private IStaffRepo _repo;

    public GetStaffByIdQueryHandler(IMapper mapper, IStaffRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<StaffResponse> Handle(GetStaffByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _repo.GetStaffById(request.Id);

        if (query == null)
        {
            throw new Exception("Staff not found");
        }

        var result = this._mapper.Map<StaffResponse>(query);

        return result;
    }
}
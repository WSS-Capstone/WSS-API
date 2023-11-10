using WSS.API.Data.Repositories.OrderDetail;
using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Services.Identity;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Queries.Statistic;

public class CountStatusTaskQuery : IRequest<List<StatusTaskResponse>>
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class CountStatusTaskQueryHandler : IRequestHandler<CountStatusTaskQuery, List<StatusTaskResponse>>
{
    private readonly IMapper _mapper;
    private readonly ITaskRepo _repo;
    private readonly IIdentitySvc _identitySvc;

    private readonly IDictionary<string, string> name = new Dictionary<string, string>()
    {
        { "0", "Dự kiến" },
        { "1", "Mới giao" },
        { "2", "Đang thực hiện" },
        { "3", "Đã hoàn thành" },
    };

    public CountStatusTaskQueryHandler(IMapper mapper, ITaskRepo repo, IIdentitySvc identitySvc)
    {
        _mapper = mapper;
        _repo = repo;
        _identitySvc = identitySvc;
    }

    public async Task<List<StatusTaskResponse>> Handle(CountStatusTaskQuery request,
        CancellationToken cancellationToken)
    {
        var query = _repo.GetTasks();

        if (request.FromDate != null)
        {
            query = query.Where(t =>
                request.FromDate.Value.Date <= t.StartDate.Value.Date ||
                request.FromDate.Value.Date <= t.EndDate.Value.Date);
        }

        if (request.ToDate != null)
        {
            query = query.Where(t =>
                t.EndDate.Value.Date <= request.ToDate.Value.Date ||
                t.StartDate.Value.Date <= request.ToDate.Value.Date);
        }
        var userId = await this._identitySvc.GetUserId();

        query = query.Where(x => x.StaffId == userId || x.PartnerId == userId);


        var result = query.GroupBy(t => t.Status)
            .Select(k => new StatusTaskResponse()
            {
                Code = ((TaskStatus)k.Key).ToString(),
                Name = this.name[k.Key.ToString()],
                Value = k.Count()
            });
        return await result.ToListAsync(cancellationToken: cancellationToken);
    }
}
using AutoMapper;
using L.Core.Helpers.Paging;
using Microsoft.EntityFrameworkCore;
using WSS.API.Application.Queries.Category;
using WSS.API.Data.Repositories.Task;

namespace WSS.API.Application.Queries.Task;

public class GetTasksQueryHandler: IRequestHandler<GetTasksQuery, PagingResponseQuery<TaskResponse, TaskSortCriteria>>
{
    private IMapper _mapper;
    private ITaskRepo _categoryRepo;

    public GetTasksQueryHandler(IMapper mapper, ITaskRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    public async Task<PagingResponseQuery<TaskResponse, TaskSortCriteria>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _categoryRepo.GetTasks();
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<TaskResponse>(query);

        return new PagingResponseQuery<TaskResponse, TaskSortCriteria>(request, result, total);
    }
}
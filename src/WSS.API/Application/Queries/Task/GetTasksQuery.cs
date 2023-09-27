namespace WSS.API.Application.Queries.Task;

public class GetTasksQuery: PagingParam<TaskSortCriteria>, IRequest<PagingResponseQuery<TaskResponse, TaskSortCriteria>>
{
    
}

public enum TaskSortCriteria
{
    Id,
    TaskName,
    Content,
    StartDate,
    EndDate
}
using AutoMapper;
using L.Core.Helpers.Paging;
using Microsoft.EntityFrameworkCore;
using WSS.API.Data.Repositories.Category;

namespace WSS.API.Application.Queries.Category;

public class GetCategorysQueryHandler : IRequestHandler<GetCategorysQuery, PagingResponseQuery<CategoryResponse, CategorySortCriteria>>
{
    private IMapper _mapper;
    private ICategoryRepo _categoryRepo;

    public GetCategorysQueryHandler(IMapper mapper, ICategoryRepo categoryRepo)
    {
        _mapper = mapper;
        _categoryRepo = categoryRepo;
    }

    /// <inheritdoc />
    public async Task<PagingResponseQuery<CategoryResponse, CategorySortCriteria>> Handle(GetCategorysQuery request, CancellationToken cancellationToken)
    {
        var query = _categoryRepo.GetCategorys();
        var total = await query.CountAsync(cancellationToken: cancellationToken);
        
        query = query.GetWithSorting(request.SortKey.ToString(), request.SortOrder);
        
        query = query.GetWithPaging(request.Page, request.PageSize);

        var result = this._mapper.ProjectTo<CategoryResponse>(query);

        return new PagingResponseQuery<CategoryResponse, CategorySortCriteria>(request, result, total);
    }
}
using WSS.API.Data.Repositories.WeddingInformation;

namespace WSS.API.Application.Commands.WeddingInformation;

public class UpdateWeddingInformationCommand : IRequest<WeddingInformationResponse>
{
    public UpdateWeddingInformationCommand(Guid id, UpdateWeddingInformationRequest request)
    {
        Id = id;
        NameGroom = request.NameGroom;
        NameBride = request.NameBride;
        NameBrideFather = request.NameBrideFather;
        NameBrideMother = request.NameBrideMother;
        NameGroomFather = request.NameGroomFather;
        NameGroomMother = request.NameGroomMother;
        WeddingDay = request.WeddingDay;
        ImageUrl = request.ImageUrl;
    }
    
    public Guid Id { get; set; }
    public string? NameGroom { get; set; }
    public string? NameBride { get; set; }
    public string? NameBrideFather { get; set; }
    public string? NameBrideMother { get; set; }
    public string? NameGroomFather { get; set; }
    public string? NameGroomMother { get; set; }
    public DateTime? WeddingDay { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateWeddingInformationRequest
{
    public Guid Id { get; set; }
    public string? NameGroom { get; set; }
    public string? NameBride { get; set; }
    public string? NameBrideFather { get; set; }
    public string? NameBrideMother { get; set; }
    public string? NameGroomFather { get; set; }
    public string? NameGroomMother { get; set; }
    public DateTime? WeddingDay { get; set; }
    public string? ImageUrl { get; set; }
}

public class UpdateWeddingInformationCommandHandler : IRequestHandler<UpdateWeddingInformationCommand, WeddingInformationResponse>
{
    private IMapper _mapper;
    private IWeddingInformationRepo _repo;

    public UpdateWeddingInformationCommandHandler(IMapper mapper, IWeddingInformationRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<WeddingInformationResponse> Handle(UpdateWeddingInformationCommand request, CancellationToken cancellationToken)
    {
        var weddingInformation = await _repo.GetWeddingInformationById(request.Id);
        if (weddingInformation == null)
        {
            throw new Exception("Wedding Information not found");
        }
       
        weddingInformation = this._mapper.Map(request, weddingInformation);
        
        await _repo.UpdateWeddingInformation(weddingInformation);
        var result = this._mapper.Map<WeddingInformationResponse>(weddingInformation);

        return result;
    }
}
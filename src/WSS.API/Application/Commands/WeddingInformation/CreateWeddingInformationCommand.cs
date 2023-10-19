using WSS.API.Data.Repositories.WeddingInformation;

namespace WSS.API.Application.Commands.WeddingInformation;

public class CreateWeddingInformationCommand : IRequest<WeddingInformationResponse>
{
    public string? NameGroom { get; set; }
    public string? NameBride { get; set; }
    public string? NameBrideFather { get; set; }
    public string? NameBrideMother { get; set; }
    public string? NameGroomFather { get; set; }
    public string? NameGroomMother { get; set; }
    public DateTime? WeddingDay { get; set; }
    public string? ImageUrl { get; set; }
}

public class CreateWeddingInformationCommandHandler : IRequestHandler<CreateWeddingInformationCommand, WeddingInformationResponse>
{
    private readonly IMapper _mapper;
    private readonly IWeddingInformationRepo _weddingInformationRepo;

    public CreateWeddingInformationCommandHandler(IMapper mapper, IWeddingInformationRepo weddingInformationRepo)
    {
        _mapper = mapper;
        _weddingInformationRepo = weddingInformationRepo;
    }

    public async Task<WeddingInformationResponse> Handle(CreateWeddingInformationCommand request, CancellationToken cancellationToken)
    {
        var weddingInformation = _mapper.Map<Data.Models.WeddingInformation>(request);
        weddingInformation.Id = Guid.NewGuid();
        
        weddingInformation = await _weddingInformationRepo.CreateWeddingInformation(weddingInformation);
        
        return _mapper.Map<WeddingInformationResponse>(weddingInformation);
    }
}
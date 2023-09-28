using WSS.API.Data.Repositories.Partner;

namespace WSS.API.Application.Commands.Partner;

public class UpdatePartnerCommand : IRequest<PartnerResponse>
{
    public UpdatePartnerCommand(Guid id, UpdatePartnerRequest request)
    {
        Id = id;
        Fullname = request.Fullname;
        DateOfBirth = request.DateOfBirth;
        Phone = request.Phone;
        Address = request.Address;
        ImageUrl = request.ImageUrl;
        Gender = (int?)request.Gender;
    }


    public Guid Id { get; set; }
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public int? Gender { get; set; }
}

public class UpdatePartnerRequest
{
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public Gender? Gender { get; set; }
}

public class UpdatePartnerCommandHandler : IRequestHandler<UpdatePartnerCommand, PartnerResponse>
{
    private IMapper _mapper;
    private IPartnerRepo _repo;

    public UpdatePartnerCommandHandler(IMapper mapper, IPartnerRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<PartnerResponse> Handle(UpdatePartnerCommand request, CancellationToken cancellationToken)
    {
        var partner = await _repo.GetPartnerById(request.Id);
        if (partner == null)
        {
            throw new Exception("Partner not found");
        }
       
        partner = this._mapper.Map(request, partner);
        
        await _repo.UpdatePartner(partner);
        var result = this._mapper.Map<PartnerResponse>(partner);
        return result;
    }
}
using WSS.API.Data.Repositories.staff;

namespace WSS.API.Application.Commands.Staff;

public class UpdateStaffCommand : IRequest<StaffResponse>
{
    public UpdateStaffCommand(Guid id, UpdateStaffRequest request)
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


public class UpdateStaffRequest
{
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public Gender? Gender { get; set; }
}

public class UpdateStaffCommandHandler : IRequestHandler<UpdateStaffCommand, StaffResponse>
{
    private IMapper _mapper;
    private IStaffRepo _repo;

    public UpdateStaffCommandHandler(IMapper mapper, IStaffRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<StaffResponse> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
    {
        var staff = await _repo.GetStaffById(request.Id);
        if (staff == null)
        {
            throw new Exception("Staff not found");
        }
       
        staff = this._mapper.Map(request, staff);
        
        await _repo.UpdateStaff(staff);
        var result = this._mapper.Map<StaffResponse>(staff);
        return result;
    }
}
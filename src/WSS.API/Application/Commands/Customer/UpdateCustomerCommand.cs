using WSS.API.Data.Repositories.Customer;

namespace WSS.API.Application.Commands.Customer;

public class UpdateCustomerCommand : IRequest<CustomerResponse>
{
    public UpdateCustomerCommand(Guid id, UpdateCustomerRequest request)
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

public class UpdateCustomerRequest
{
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public Gender? Gender { get; set; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerResponse>
{
    private IMapper _mapper;
    private ICustomerRepo _repo;

    public UpdateCustomerCommandHandler(IMapper mapper, ICustomerRepo repo)
    {
        _mapper = mapper;
        _repo = repo;
    }

    public async Task<CustomerResponse> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _repo.GetCustomerById(request.Id);
        if (customer == null)
        {
            throw new Exception("Customer not found");
        }
       
        customer = this._mapper.Map(request, customer);
        
        await _repo.UpdateCustomer(customer);
        var result = this._mapper.Map<CustomerResponse>(customer);

        return result;
    }
}
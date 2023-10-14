using WSS.API.Data.Repositories.Customer;

namespace WSS.API.Application.Commands.Customer;

public class CreateCustomerCommand : IRequest<CustomerResponse>
{
    public string? Fullname { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? ImageUrl { get; set; }
    public int? Gender { get; set; }
}

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerResponse>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepo _customerRepo;

    public CreateCustomerCommandHandler(IMapper mapper, ICustomerRepo customerRepo)
    {
        _mapper = mapper;
        _customerRepo = customerRepo;
    }

    public async Task<CustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = _mapper.Map<Data.Models.Customer>(request);
        customer.Id = Guid.NewGuid();
        customer.CreateDate = DateTime.UtcNow;
        
        customer = await _customerRepo.CreateCustomer(customer);
        
        return _mapper.Map<CustomerResponse>(customer);
    }
}
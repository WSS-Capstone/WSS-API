namespace WSS.API.Application.Queries.Customer;

public class GetCustomerByIdQuery : IRequest<CustomerResponse>
{
    public GetCustomerByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
namespace WSS.API.Application.Queries.Account;

public class GetAccountDetailQuery : IRequest<AccountResponse>
{
    public GetAccountDetailQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
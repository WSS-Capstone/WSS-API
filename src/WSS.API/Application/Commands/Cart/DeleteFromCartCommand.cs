using WSS.API.Data.Repositories.Cart;

namespace WSS.API.Application.Commands.Cart;

public class DeleteFromCartCommand : IRequest<CartResponse>
{
    public Guid Id { get; set; }
}

public class DeleteFromCartCommandHandler : IRequestHandler<DeleteFromCartCommand, CartResponse>
{
    private IMapper _mapper;
    private ICartRepo _cartRepo;

    public DeleteFromCartCommandHandler(IMapper mapper, ICartRepo cartRepo)
    {
        _mapper = mapper;
        _cartRepo = cartRepo;
    }

    /// <inheritdoc />
    public async Task<CartResponse> Handle(DeleteFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await this._cartRepo.GetCartById(request.Id);

        if (cart == null)
        {
            throw new Exception(nameof(Data.Models.Cart) + request.Id);
        }

        await this._cartRepo.DeleteCart(cart);

        return this._mapper.Map<CartResponse>(cart);
    }
}
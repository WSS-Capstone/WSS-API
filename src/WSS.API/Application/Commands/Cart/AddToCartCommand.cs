using WSS.API.Data.Repositories.Cart;

namespace WSS.API.Application.Commands.Cart;

public class AddToCartCommand : IRequest<CartResponse>
{
    public Guid UserId { get; set; }
    public Guid ServiceId { get; set; }
}

public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, CartResponse>
{
    private IMapper _mapper;
    private ICartRepo _cartRepo;

    public AddToCartCommandHandler(IMapper mapper, ICartRepo cartRepo)
    {
        _mapper = mapper;
        _cartRepo = cartRepo;
    }

    /// <inheritdoc />
    public async Task<CartResponse> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = new Data.Models.Cart()
        {
            UserId = request.UserId,
            ServiceId = request.ServiceId
        };

        await this._cartRepo.CreateCart(cart);

        return this._mapper.Map<CartResponse>(cart);
    }
}
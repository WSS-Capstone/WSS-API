using WSS.API.Data.Repositories.Combo;

namespace WSS.API.Application.Commands.Combo;

public class UpdateComboStatusCommand : IRequest<ComboResponse>
{
    public Guid Id { get; set; }
    public ComboStatus Status { get; set; }
}

public class UpdateComboStatusCommandHandler : IRequestHandler<UpdateComboStatusCommand, ComboResponse>
{
    private readonly IMapper _mapper;
    private readonly IComboRepo _comboRepo;

    public UpdateComboStatusCommandHandler(IMapper mapper, IComboRepo comboRepo)
    {
        _mapper = mapper;
        _comboRepo = comboRepo;
    }

    public async Task<ComboResponse> Handle(UpdateComboStatusCommand request,
        CancellationToken cancellationToken)
    {
        var service = await _comboRepo.GetComboById(request.Id);
        if (service == null)
        {
            throw new Exception("Service not found");
        }

        service.Status = (int)request.Status;
        service.CreateDate = DateTime.Now;
        await _comboRepo.UpdateCombo(service);
        return _mapper.Map<ComboResponse>(service);
    }
}
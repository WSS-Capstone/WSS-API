using WSS.API.Application.Commands.Account;
using WSS.API.Application.Commands.Category;
using WSS.API.Application.Commands.Combo;
using WSS.API.Application.Commands.Commission;
using WSS.API.Application.Commands.CurrentPrice;
using WSS.API.Application.Commands.Service;
using Task = WSS.API.Data.Models.Task;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.DataMapping;

/// <inheritdoc />
public class MappingProfile : Profile
{
    /// <inheritdoc />
    public MappingProfile()
    {
        this.AccountProfile();
        this.CategoryProfile();
        this.ComboProfile();
        this.ComboServiceProfile();
        this.CommissionProfile();
        this.CurrentPriceProfile();
        this.FeedbackProfile();
        this.MessageProfile();
        this.OrderProfile();
        this.OrderDetailProfile();
        this.PartnerPaymentHistoryProfile();
        this.PaymentHistoryProfile();
        this.ServiceProfile();
        this.ServiceImageProfile();
        this.TaskProfile();
        this.VoucherProfile();
        this.WeddingInformationProfile();
    }

    private void AccountProfile()
    {
        this.CreateMap<Account, AccountResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (AccountStatus)src.Status))
            .ReverseMap();

        this.CreateMap<Account, CreateAccountForCustomerCommand>().ReverseMap();
    }

    private void CategoryProfile()
    {
        this.CreateMap<Category, CategoryResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (CategoryStatus)src.Status))
            .ForMember(dto => dto.Commission, opt => opt.MapFrom(src => src.Commision))
            .ReverseMap();

        this.CreateMap<Category, CreateCategoryCommand>()
            .ReverseMap();

        this.CreateMap<Category, UpdateCategoryCommand>()
            .ReverseMap();

        this.CreateMap<CreateCategoryCommand, UpdateCategoryCommand>().ReverseMap();
    }

    private void ComboProfile()
    {
        this.CreateMap<Combo, ComboResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (ComboStatus)src.Status))
            .ForMember(dto => dto.ComboServices, opt => opt.MapFrom(src => src.ComboServices))
            .ReverseMap();

        this.CreateMap<Combo, AddNewComboCommand>()
            .ForMember(dto => dto.ComboServices,
                opt => opt.MapFrom(src => src.ComboServices))
            .ReverseMap();

        this.CreateMap<Combo, ComboService>()
            .ReverseMap();

        this.CreateMap<AddNewComboCommand, UpdateComboCommand>().ReverseMap();
    }

    private void ComboServiceProfile()
    {
        this.CreateMap<ComboService, ComboServicesResponse>()
            .ForMember(dto => dto.Service, opt => opt.MapFrom(src => src.Service))
            .ReverseMap();
    }

    private void CommissionProfile()
    {
        this.CreateMap<Commission, CommissionResponse>().ReverseMap();
        this.CreateMap<Commission, CreateCommissionCommand>().ReverseMap();
        this.CreateMap<Commission, UpdateCategoryCommand>().ReverseMap();
        this.CreateMap<CreateCommissionCommand, UpdateCategoryCommand>().ReverseMap();
    }

    private void CurrentPriceProfile()
    {
        this.CreateMap<CurrentPrice, CurrentPriceResponse>()
            .ForMember(dto => dto.Service, opt => opt.MapFrom(src => src.Service))
            .ReverseMap();
        this.CreateMap<CurrentPrice, CreateCurrentPriceCommand>().ReverseMap();
        this.CreateMap<CurrentPrice, UpdateCurrentPriceCommand>().ReverseMap();
    }

    private void FeedbackProfile()
    {
        this.CreateMap<Data.Models.Feedback, FeedbackResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (FeedbackStatus)src.Status))
            .ReverseMap();
    }

    private void MessageProfile()
    {
        this.CreateMap<Message, MessageResponse>()
            .ReverseMap();
    }

    private void OrderProfile()
    {
        this.CreateMap<Order, OrderResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (OrderStatus)src.Status))
            .ReverseMap();
    }

    private void OrderDetailProfile()
    {
        this.CreateMap<OrderDetail, OrderDetailResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (OrderDetailStatus)src.Status))
            .ReverseMap();
    }

    private void PartnerPaymentHistoryProfile()
    {
        this.CreateMap<PartnerPaymentHistory, PartnerPaymentHistoryResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (PartnerPaymentHistoryStatus)src.Status))
            .ReverseMap();
    }

    private void PaymentHistoryProfile()
    {
        this.CreateMap<PaymentHistory, PaymentHistoryResponse>()
            .ReverseMap();

        this.CreateMap<PaymentHistory, PartnerPaymentHistoryResponse>().ReverseMap();
    }

    private void ServiceProfile()
    {
        this.CreateMap<Service, ServiceResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (ServiceStatus)src.Status))
            .ForMember(dto => dto.Category,
                opt => opt.MapFrom(src => src.Category))
            .ForMember(dto => dto.CurrentPrices,
                opt => opt.MapFrom(src => src.CurrentPrices.FirstOrDefault()))
            .ReverseMap();

        this.CreateMap<Service, CreateServiceCommand>().ReverseMap();
        this.CreateMap<Service, UpdateServiceCommand>().ReverseMap();
        this.CreateMap<CreateServiceCommand, UpdateServiceCommand>().ReverseMap();
    }

    private void ServiceImageProfile()
    {
        this.CreateMap<ServiceImage, ServiceImageResponse>()
            .ReverseMap();
    }

    private void TaskProfile()
    {
        this.CreateMap<Task, TaskResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (TaskStatus)src.Status))
            .ReverseMap();
    }

    private void VoucherProfile()
    {
        this.CreateMap<Voucher, VoucherResponse>()
            .ReverseMap();
    }

    private void WeddingInformationProfile()
    {
        this.CreateMap<WeddingInformation, WeddingInformationResponse>()
            .ReverseMap();
    }
}
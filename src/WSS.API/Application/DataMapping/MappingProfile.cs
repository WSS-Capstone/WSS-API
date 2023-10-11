using AutoMapper;
using WSS.API.Application.Commands.Account;
using WSS.API.Application.Commands.Category;
using WSS.API.Application.Commands.Combo;
using WSS.API.Application.Commands.Commission;
using WSS.API.Application.Commands.Customer;
using WSS.API.Application.Commands.Service;
using WSS.API.Application.Commands.Staff;
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
        this.CartProfile();
        this.CategoryProfile();
        this.ComboProfile();
        this.ComboServiceProfile();
        this.CommissionProfile();
        this.CurrentPriceProfile();
        this.CustomerProfile();
        this.FeedbackProfile();
        this.MessageProfile();
        this.OrderProfile();
        this.OrderDetailProfile();
        this.OwnerProfile();
        this.PartnerProfile();
        this.PartnerPaymentHistoryProfile();
        this.PartnerServiceProfile();
        this.PaymentHistoryProfile();
        this.RoleProfile();
        this.ServiceProfile();
        this.ServiceImageProfile();
        this.StaffProfile();
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

        this.CreateMap<Account, CreateAccountCommand>().ReverseMap();
    }

    private void CartProfile()
    {
        this.CreateMap<Cart, CartResponse>().ReverseMap();
    }

    private void CategoryProfile()
    {
        this.CreateMap<Category, CategoryResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (CategoryStatus)src.Status))
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
        this.CreateMap<Commission, CommissionResponse>()
            .ForMember(dto => dto.Partner, opt => opt.MapFrom(src => src.Partner))
            .ReverseMap();

        this.CreateMap<Commission, CreateCommissionCommand>().ReverseMap();
        this.CreateMap<Commission, UpdateCategoryCommand>().ReverseMap();
        this.CreateMap<CreateCommissionCommand, UpdateCategoryCommand>().ReverseMap();
    }

    private void CurrentPriceProfile()
    {
        this.CreateMap<CurrentPrice, CurrentPriceResponse>()
            .ForMember(dto => dto.Service, opt => opt.MapFrom(src => src.Service))
            .ReverseMap();
    }

    private void CustomerProfile()
    {
        this.CreateMap<CustomerResponse, staff>().ReverseMap();
        this.CreateMap<CustomerResponse, Owner>().ReverseMap();
        this.CreateMap<CustomerResponse, Partner>().ReverseMap();


        this.CreateMap<Customer, CustomerResponse>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();

        this.CreateMap<Customer, UpdateCustomerRequest>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();
        this.CreateMap<Customer, UpdateCustomerCommand>().ReverseMap();
    }

    private void FeedbackProfile()
    {
        this.CreateMap<Feedback, FeedbackResponse>()
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

    private void OwnerProfile()
    {
        this.CreateMap<OwnerResponse, staff>();
        this.CreateMap<OwnerResponse, Customer>();
        this.CreateMap<OwnerResponse, Partner>();
        this.CreateMap<Owner, OwnerResponse>().ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();
    }

    private void PartnerProfile()
    {
        this.CreateMap<PartnerResponse, staff>().ReverseMap();
        this.CreateMap<PartnerResponse, Customer>().ReverseMap();
        this.CreateMap<PartnerResponse, Owner>().ReverseMap();

        this.CreateMap<Partner, PartnerResponse>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();
    }

    private void PartnerPaymentHistoryProfile()
    {
        this.CreateMap<PartnerPaymentHistory, PartnerPaymentHistoryResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (PartnerPaymentHistoryStatus)src.Status))
            .ReverseMap();
    }

    private void PartnerServiceProfile()
    {
        this.CreateMap<PartnerService, PartnerServiceResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (PartnerServiceStatus)src.Status))
            .ReverseMap();
    }

    private void PaymentHistoryProfile()
    {
        this.CreateMap<PaymentHistory, PaymentHistoryResponse>()
            .ReverseMap();

        this.CreateMap<PaymentHistory, PartnerPaymentHistoryResponse>().ReverseMap();
    }

    private void RoleProfile()
    {
        this.CreateMap<Role, RoleResponse>()
            .ReverseMap();
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

    private void StaffProfile()
    {
        this.CreateMap<StaffResponse, Customer>().ReverseMap();
        this.CreateMap<StaffResponse, Owner>().ReverseMap();
        this.CreateMap<StaffResponse, Partner>().ReverseMap();

        this.CreateMap<staff, StaffResponse>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();

        this.CreateMap<staff, UpdateStaffRequest>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();

        this.CreateMap<staff, UpdateStaffCommand>().ReverseMap();
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
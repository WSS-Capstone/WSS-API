using AutoMapper;
using WSS.API.Application.Commands.Category;
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
        this.TaskProfile();
        this.StaffProfile();
        this.PartnerProfile();
        this.OwnerProfile();
        this.CustomerProfile();
        this.ServiceProfile();
        this.OrderProfile();
        this.OrderDetailProfile();
    }

    private void AccountProfile()
    {
        this.CreateMap<Account, AccountResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (AccountStatus)src.Status))
            .ReverseMap();
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

    private void TaskProfile()
    {
        this.CreateMap<Task, TaskResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (TaskStatus)src.Status))
            .ReverseMap();
    }

    private void StaffProfile()
    {
        this.CreateMap<staff, StaffResponse>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();
    }

    private void PartnerProfile()
    {
        this.CreateMap<Partner, PartnerResponse>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();
    }

    private void CustomerProfile()
    {
        this.CreateMap<Customer, CustomerResponse>().ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();
    }

    private void OwnerProfile()
    {
        this.CreateMap<Owner, OwnerResponse>().ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => (Gender)src.Gender))
            .ReverseMap();
    }

    private void ServiceProfile()
    {
        this.CreateMap<Service, ServiceResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (ServiceStatus)src.Status))
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
}
using AutoMapper;
using Task = WSS.API.Data.Models.Task;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.DataMapping;

/// <inheritdoc />
public class MappingProfile : Profile
{
    /// <inheritdoc />
    public MappingProfile()
    {
        this.CategoryProfile();
        this.TaskProfile();
        this.StaffProfile();
        this.PartnerProfile();
        this.CustomerProfile();
        this.ServiceProfile();
        this.OrderProfile();
        this.OrderDetailProfile();
    }


    private void CategoryProfile()
    {
        this.CreateMap<Category, CategoryResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (CategoryStatus)src.Status))
            .ReverseMap();
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
        this.CreateMap<StaffResponse, staff>().ReverseMap();
    }

    private void PartnerProfile()
    {
        this.CreateMap<PartnerResponse, Partner>().ReverseMap();
    }
    
    private void CustomerProfile()
    {
        this.CreateMap<CustomerResponse, Partner>().ReverseMap();
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
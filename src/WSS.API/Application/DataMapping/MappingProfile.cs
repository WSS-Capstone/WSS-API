using WSS.API.Application.Commands.Account;
using WSS.API.Application.Commands.Category;
using WSS.API.Application.Commands.Combo;
using WSS.API.Application.Commands.Comment;
using WSS.API.Application.Commands.Commission;
using WSS.API.Application.Commands.CurrentPrice;
using WSS.API.Application.Commands.DayOff;
using WSS.API.Application.Commands.Order;
using WSS.API.Application.Commands.Service;
using WSS.API.Application.Commands.Task;
using WSS.API.Application.Commands.Voucher;
using WSS.API.Application.Feedback;
using WSS.API.Application.Queries.DayOff;
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
        this.DayOffProfile();
        this.CommentProfile();
    }

    private void AccountProfile()
    {
        this.CreateMap<User, UserResponse>()
            .ForMember(dto => dto.Gender,
                opt => opt.MapFrom(src => src.Gender == null ? Gender.Other : (Gender)src.Gender))
            .ReverseMap();

        this.CreateMap<Account, AccountResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => src.Status == null ? AccountStatus.InActive : (AccountStatus)src.Status))
            .ForMember(dto => dto.User, opt => opt.MapFrom(src => src.User))
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
                opt => opt.MapFrom(src => src.Status != null ? (ComboStatus)src.Status : ComboStatus.InActive))
            .ForMember(dto => dto.ComboServices, opt => opt.MapFrom(src => src.ComboServices))
            .ForMember(dto => dto.DisountPrice,
                opt => opt.MapFrom(src => src.TotalAmount / 100 * (100 - src.DiscountValueCombo)))
            .ForMember(dto => dto.Used,
                opt => opt.MapFrom(src => src.Orders.Count(o => o.StatusOrder == (int)StatusOrder.DONE)))
            .ForMember(dto => dto.Rating, opt =>
                opt.MapFrom(src => src.ComboServices
                    .Average(c => c.Service != null
                        ? c.Service.OrderDetails
                            .Average(od => od.Feedbacks
                                .Average(f => f.Rating))
                        : 0)))
            .ReverseMap();

        this.CreateMap<Combo, AddNewComboCommand>()
            .ReverseMap();

        this.CreateMap<Combo, UpdateComboCommand>()
            .ReverseMap();

        this.CreateMap<Combo, ComboService>()
            .ReverseMap();


        this.CreateMap<ComboResponse, ComboServicesResponse>().ReverseMap();

        this.CreateMap<AddNewComboCommand, UpdateComboCommand>().ReverseMap();
    }

    private void ComboServiceProfile()
    {
        this.CreateMap<ComboService, ComboServicesResponse>()
            .ForMember(dto => dto.Service, opt => opt.MapFrom(src => src.Service))
            .ForMember(dto => dto.Combo, opt => opt.MapFrom(src => src.Combo))
            .ReverseMap();
        

        this.CreateMap<ComboService, ServiceResponse>()
            .ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.ServiceId))
            .ForMember(dto => dto.Name,
                opt => opt.MapFrom(src => src.Service != null ? src.Service.Name : string.Empty))
            .ForMember(dto => dto.Quantity, opt => opt.MapFrom(src => src.Service != null ? src.Service.Quantity : 0))
            .ForMember(dto => dto.Unit,
                opt => opt.MapFrom(src => src.Service != null ? src.Service.Unit : string.Empty))
            .ForMember(dto => dto.CategoryId,
                opt => opt.MapFrom(src => src.Service != null ? src.Service.CategoryId : Guid.Empty))
            .ForMember(dto => dto.CoverUrl,
                opt => opt.MapFrom(src => src.Service != null ? src.Service.CoverUrl : string.Empty))
            .ForMember(dto => dto.ServiceImages,
                opt => opt.MapFrom(src => src.Service != null ? src.Service.ServiceImages : new List<ServiceImage>()))
            .ForMember(dto => dto.CurrentPrices,
                opt => opt.MapFrom(src =>
                    src.Service != null ? src.Service.CurrentPrices.FirstOrDefault() : new CurrentPrice()))
            .ReverseMap();
    }

    private void CommissionProfile()
    {
        this.CreateMap<Commission, CommissionResponse>().ReverseMap();
        this.CreateMap<Commission, CreateCommissionCommand>().ReverseMap();
        this.CreateMap<Commission, UpdateCategoryCommand>().ReverseMap();
        this.CreateMap<CreateCommissionCommand, UpdateCategoryCommand>().ReverseMap();
    }

    private void CommentProfile()
    {
        this.CreateMap<Comment, CommentResponse>()
            .ForMember(dto => dto.Task, opt => opt.MapFrom(src => src.Task))
            .ForMember(dto => dto.CreateBy, opt => opt.MapFrom(src => src.CreateByNavigation))
            .ReverseMap();
        this.CreateMap<Comment, CreateCommentCommand>().ReverseMap();
        this.CreateMap<Comment, UpdateCommentCommand>().ReverseMap();
        this.CreateMap<Comment, DeleteCommentCommand>().ReverseMap();
        this.CreateMap<CreateCommentCommand, UpdateCommentCommand>().ReverseMap();
    }

    private void DayOffProfile()
    {
        this.CreateMap<DayOff, DayOffResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (DayOffStatus)src.Status))
            .ReverseMap();
        this.CreateMap<DayOff, CreateDayOffCommand>().ReverseMap();
        this.CreateMap<DayOff, UpdateDayOffCommand>().ReverseMap();
        this.CreateMap<DayOff, DeleteDayOffCommand>().ReverseMap();
        this.CreateMap<CreateDayOffCommand, UpdateCategoryCommand>().ReverseMap();
    }

    private void CurrentPriceProfile()
    {
        this.CreateMap<CurrentPrice, CurrentPriceResponse>()
            // .ForMember(dto => dto.Service, opt => opt.MapFrom(src => src.Service))
            .ReverseMap();
        this.CreateMap<CurrentPrice, CreateCurrentPriceCommand>().ReverseMap();
        this.CreateMap<CurrentPrice, UpdateCurrentPriceCommand>().ReverseMap();
    }

    private void FeedbackProfile()
    {
        this.CreateMap<Data.Models.Feedback, FeedbackResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (FeedbackStatus)src.Status))
            .ForMember(dto => dto.Service,
                opt => opt.MapFrom(src => src.OrderDetail.Service))
            .ForMember(dto => dto.User,
                opt => opt.MapFrom(src => src.CreateByNavigation))
            .ReverseMap();
        this.CreateMap<Data.Models.Feedback, FeedbackResponseForGetServiceResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src => (FeedbackStatus)src.Status))
            .ForMember(dto => dto.User,
                opt => opt.MapFrom(src => src.CreateByNavigation))
            .ReverseMap();

        this.CreateMap<Data.Models.Feedback, CreateFeedbackCommand>().ReverseMap();
        this.CreateMap<Data.Models.Feedback, UpdateFeedbackCommand>().ReverseMap();
    }

    private void MessageProfile()
    {
        this.CreateMap<Message, MessageResponse>()
            .ReverseMap();
    }

    private void OrderProfile()
    {
        this.CreateMap<Order, OrderResponse>()
            .ForMember(dto => dto.StatusOrder,
                opt => opt.MapFrom(src => src.StatusOrder != null ? (StatusOrder)src.StatusOrder : StatusOrder.CANCEL))
            .ForMember(dto => dto.StatusPayment,
                opt => opt.MapFrom(src =>
                    src.StatusPayment != null ? (StatusPayment)src.StatusPayment : StatusPayment.CANCEL))
            .ForMember(opt => opt.OrderDetails,
                opt => opt.MapFrom(src => src.OrderDetails))
            .ForMember(opt => opt.Customer,
                opt => opt.MapFrom(src => src.Customer))
            .ReverseMap();
        this.CreateMap<Order, CreateOrderCommand>().ReverseMap();
        this.CreateMap<Order, ApprovalOrderByOwnerCommand>().ForMember(dto => dto.StatusOrder,
                opt => opt.MapFrom(src => src.StatusOrder != null ? (StatusOrder)src.StatusOrder : StatusOrder.PENDING))
            .ReverseMap();
        this.CreateMap<Order, UpdateOrderCommand>().ReverseMap();
    }

    private void OrderDetailProfile()
    {
        this.CreateMap<OrderDetail, OrderDetailResponse>()
            .ForMember(dto => dto.Status,
                opt => opt.MapFrom(src =>
                    src.Status != null ? (OrderDetailStatus)src.Status : OrderDetailStatus.INACTIVE))
            .ReverseMap();

        this.CreateMap<OrderDetail, OrderDetailRequest>().ReverseMap();
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
                opt => opt.MapFrom(src => src.Status != null ? (ServiceStatus)src.Status : ServiceStatus.InActive))
            .ForMember(dto => dto.Category,
                opt => opt.MapFrom(src => src.Category))
            .ForMember(dto => dto.CurrentPrices,
                opt => opt.MapFrom(src => src.CurrentPrices.OrderByDescending(s => s.DateOfApply).FirstOrDefault()))
            .ForMember(dto => dto.ComboServices,
                opt => opt.MapFrom(src => src.ComboServices))
            
            .ForMember(dto => dto.Used,
                opt => opt.MapFrom(src =>
                    src.OrderDetails.Count == 0
                        ? 0
                        : src.OrderDetails.DistinctBy(od => od.OrderId)
                            .Count(o => o.Order != null && o.Order.StatusOrder == (int)StatusOrder.DONE)))
            .ForMember(dto => dto.Rating,
                opt => opt.MapFrom(src =>
                    src.OrderDetails.Count == 0
                        ? 0
                        : src.OrderDetails.Average(o =>
                            o.Feedbacks.Count == 0 ? 0 : o.Feedbacks.Average(f => f.Rating))))
            .ForMember(dto => dto.TotalRevenue,
                opt => opt.MapFrom(src =>
                    src.OrderDetails.Count == 0
                        ? 0
                        : src.OrderDetails.Sum(o => o.Price)));

        this.CreateMap<Service, CreateServiceCommand>().ReverseMap();
        this.CreateMap<Service, UpdateServiceCommand>().ReverseMap();
        this.CreateMap<Service, ApprovalServiceCommand>().ReverseMap();
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
                opt => opt.MapFrom(src => src.Status != null ? (TaskStatus)src.Status : TaskStatus.TO_DO))
            .ForMember(dto => dto.CreateBy,
                opt => opt.MapFrom(src => src.CreateByNavigation))
            // .ForMember(dto => dto.OrderDetail,
            //     opt => opt.MapFrom(src => new List<OrderDetailResponse>(){ src.OrderDetail }))
            .ReverseMap();

        this.CreateMap<Task, CreateTaskCommand>().ReverseMap();
        this.CreateMap<Task, UpdateTaskCommand>().ReverseMap();

        this.CreateMap<TaskOrderDetail, TaskOrderDetailsResponse>()
            .ForMember(dto => dto.OrderDetail, opt => opt.MapFrom(src => src.OrderDetail))
            .ReverseMap();
    }

    private void VoucherProfile()
    {
        this.CreateMap<Voucher, VoucherResponse>()
            .ForMember(dto => dto.CreateBy, opt => opt.MapFrom(src => src.CreateByNavigation))
            .ReverseMap();
        this.CreateMap<Voucher, CreateVoucherCommand>().ReverseMap();
        this.CreateMap<Voucher, UpdateVoucherCommand>().ReverseMap();
        this.CreateMap<CreateVoucherCommand, UpdateVoucherCommand>().ReverseMap();
    }

    private void WeddingInformationProfile()
    {
        this.CreateMap<WeddingInformation, WeddingInformationResponse>()
            .ReverseMap();

        this.CreateMap<WeddingInformation, WeddingInformationRequest>().ReverseMap();
    }
}
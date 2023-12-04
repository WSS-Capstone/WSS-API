using WSS.API.Data.Repositories.Category;
using WSS.API.Data.Repositories.Feedback;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.PartnerPaymentHistory;
using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Data.Repositories.Service;

namespace WSS.API.Application.Queries.Dashboard;

public class GetDashboardQuery : IRequest<DashboardResponse>
{
    public DateTime? Month { get; set; }
    public DateTime? Year { get; set; }
}

public class DashboardRequest
{
    public DateTime? Month { get; set; }
    public DateTime? Year { get; set; }
}

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardResponse>
{
    private IPaymentHistoryRepo _paymentHistoryRepo;
    private IPartnerPaymentHistoryRepo _partnerPaymentHistoryRepo;
    private IServiceRepo _serviceRepo;
    private ICategoryRepo _categoryRepo;
    private IOrderRepo _orderRepo;
    private IFeedbackRepo _feedbackRepo;

    public GetDashboardQueryHandler(IPartnerPaymentHistoryRepo partnerPaymentHistoryRepo,
        IPaymentHistoryRepo paymentHistoryRepo, IServiceRepo serviceRepo, ICategoryRepo categoryRepo,
        IOrderRepo orderRepo, IFeedbackRepo feedbackRepo)
    {
        _partnerPaymentHistoryRepo = partnerPaymentHistoryRepo;
        _paymentHistoryRepo = paymentHistoryRepo;
        _serviceRepo = serviceRepo;
        _categoryRepo = categoryRepo;
        _orderRepo = orderRepo;
        _feedbackRepo = feedbackRepo;
    }

    public async Task<DashboardResponse> Handle(GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var result = new DashboardResponse();
        var total = new List<Dictionary<string, double?>>();
        var serviceQuantity = new List<Dictionary<string, int>>();
        var serviceOrder = new List<Dictionary<string, int>>();
        var serviceFeedback = new List<Dictionary<string, float>>();
        var partner = new List<Dictionary<string, int>>();
        var partnerPayment = new List<Dictionary<string, double?>>();

        var queryPaymentHistories = _paymentHistoryRepo.GetPaymentHistorys();
        var queryPartnerPaymentHistories = _partnerPaymentHistoryRepo.GetPartnerPaymentHistorys(null, new Expression<Func<PartnerPaymentHistory, object>>[]
        {
            pph => pph.Partner
        });
        var queryServices = _serviceRepo.GetServices();
        var queryOrders = _orderRepo.GetOrders(null, new Expression<Func<Data.Models.Order, object>>[]
        {
            o => o.OrderDetails
        });
        var queryFeedbacks = _feedbackRepo.GetFeedbacks(null, new Expression<Func<Data.Models.Feedback, object>>[]
        {
            f => f.OrderDetail
        });

        // revenue by month, year
        if (request.Month != null || request.Year != null)
        {
            var fromDate = new DateTime();
            var toDate = new DateTime();
            if (request.Month != null)
            {
                var lastDayOfMonth = DateTime.DaysInMonth(request.Month.Value.Year, request.Month.Value.Month);
                fromDate = new DateTime(request.Month.Value.Year, request.Month.Value.Month, 1);
                toDate = new DateTime(request.Month.Value.Year, request.Month.Value.Month, lastDayOfMonth);
            }
            else if (request.Year != null)
            {
                fromDate = new DateTime(request.Year.Value.Year, 1, 1);
                toDate = new DateTime(request.Year.Value.Year, 12, 31);
            }

            // select total of payment history
            queryPaymentHistories = queryPaymentHistories.Where(x => x.CreateDate.Value.Date >= fromDate &&
                                                                     x.CreateDate.Value.Date <= toDate);

            // select total of partner payment history
            queryPartnerPaymentHistories = queryPartnerPaymentHistories.Where(x =>
                x.CreateDate.Value.Date >= fromDate &&
                x.CreateDate.Value.Date <= toDate);

            // join total payment history and total partner payment history to total with day in month
            var totalPaymentHistoryPerDay = await queryPaymentHistories.GroupBy(x => x.CreateDate.Value.Day)
                .Select(x => new
                {
                    Day = x.Key,
                    Total = x.Sum(y => y.TotalAmount)
                }).ToListAsync(cancellationToken: cancellationToken);

            var totalPartnerPaymentHistoryPerDay = await queryPartnerPaymentHistories
                .GroupBy(x => x.CreateDate.Value.Day)
                .Select(x => new
                {
                    Day = x.Key,
                    Total = x.Sum(y => y.Total)
                }).ToListAsync(cancellationToken: cancellationToken);

            var totalPerDayDictionary = new Dictionary<string, double?>();
            double? totalAmountPerDay = 0.0;
            foreach (var item in totalPaymentHistoryPerDay)
            {
                var day = item.Day;
                var totalAmount = item.Total ?? 0.0;
                var totalPartnerAmount =
                    totalPartnerPaymentHistoryPerDay.FirstOrDefault(x => x.Day == day)?.Total ?? 0.0;
                totalAmountPerDay = totalAmount + totalPartnerAmount;
                totalPerDayDictionary.Add(day.ToString(), totalAmountPerDay);
            }

            total.Add(totalPerDayDictionary);
        }

        // service group by category id
        var services = await queryServices.ToListAsync(cancellationToken: cancellationToken);
        var serviceGroupByCategoryId = services.GroupBy(x => x.CategoryId).ToList();
        // get category name
        var categoryNames = await _categoryRepo.GetCategorys().Select(x => new
        {
            x.Id,
            x.Name
        }).ToListAsync(cancellationToken: cancellationToken);

        // map category name to service group by category id
        foreach (var item in serviceGroupByCategoryId)
        {
            var categoryId = item.Key;
            var categoryName = categoryNames.FirstOrDefault(x => x.Id == categoryId)?.Name;
            if (categoryName != null)
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add(categoryName, item.Count());
                serviceQuantity.Add(dictionary);
            }
        }

        // map service name to order group by service id
        var orders = await queryOrders.ToListAsync(cancellationToken: cancellationToken);
        var orderGroupByServiceId = orders.SelectMany(x => x.OrderDetails).GroupBy(x => x.ServiceId).ToList();

        foreach (var item in orderGroupByServiceId)
        {
            var serviceId = item.Key;
            var serviceName = services.FirstOrDefault(x => x.Id == serviceId)?.Name;
            if (serviceName != null)
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add(serviceName, item.Count());
                serviceOrder.Add(dictionary);
            }
        }

        // map service name to feedback group by service id and calculate average star
        var feedbacks = await queryFeedbacks.ToListAsync(cancellationToken: cancellationToken);
        var feedbackGroupByServiceId = feedbacks.GroupBy(x => x.OrderDetail!.ServiceId).ToList();
        double? averageStar = 0.0;
        foreach (var item in feedbackGroupByServiceId)
        {
            var serviceId = item.Key;
            var serviceName = services.FirstOrDefault(x => x.Id == serviceId)?.Name;
            if (serviceName != null)
            {
                var dictionary = new Dictionary<string, float>();
                averageStar = item.Average(od => od.Rating);
                dictionary.Add(serviceName, (float)averageStar);
                serviceFeedback.Add(dictionary);
            }
        }
        
        // count partner with category in service
        var partnerGroupByCategoryId = services.GroupBy(x => x.CategoryId).ToList();
        foreach (var item in partnerGroupByCategoryId)
        {
            var categoryId = item.Key;
            var categoryName = categoryNames.FirstOrDefault(x => x.Id == categoryId)?.Name;
            if (categoryName != null)
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add(categoryName, item.Select(x => x.CreateBy).Distinct().Count());
                partner.Add(dictionary);
            }
        }
        
        // count total payment of partner join user select name partner
        var partnerPaymentHistories = await queryPartnerPaymentHistories.ToListAsync(cancellationToken: cancellationToken);
        var partnerPaymentGroupByPartnerId = partnerPaymentHistories.GroupBy(x => x.PartnerId).ToList();
        foreach (var item in partnerPaymentGroupByPartnerId)
        {
            var partnerId = item.Key;
            var partnerName = partnerPaymentHistories.FirstOrDefault(x => x.PartnerId == partnerId)?.Partner.Fullname;
            if (partnerName != null)
            {
                var dictionary = new Dictionary<string, double?>();
                dictionary.Add(partnerName, item.Sum(x => x.Total));
                partnerPayment.Add(dictionary);
            }
        }
        

        result.TotalRevenue = total;
        result.ServiceQuantity = serviceQuantity;
        result.ServiceOrder = serviceOrder;
        result.ServiceFeedback = serviceFeedback;
        result.Partner = partner;
        result.PartnerPayment = partnerPayment;
        
        return result;
    }
}
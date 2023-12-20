using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.Category;
using WSS.API.Data.Repositories.Feedback;
using WSS.API.Data.Repositories.Order;
using WSS.API.Data.Repositories.PartnerPaymentHistory;
using WSS.API.Data.Repositories.PaymentHistory;
using WSS.API.Data.Repositories.Service;
using WSS.API.Data.Repositories.Task;
using WSS.API.Infrastructure.Config;
using TaskStatus = WSS.API.Application.Models.ViewModels.TaskStatus;

namespace WSS.API.Application.Queries.Dashboard;

public class GetDashboardQuery : IRequest<DashboardResponse>
{
}

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardResponse>
{
    private IPaymentHistoryRepo _paymentHistoryRepo;
    private IPartnerPaymentHistoryRepo _partnerPaymentHistoryRepo;
    private IServiceRepo _serviceRepo;
    private ICategoryRepo _categoryRepo;
    private IOrderRepo _orderRepo;
    private IFeedbackRepo _feedbackRepo;
    private IAccountRepo _accountRepo;
    private ITaskRepo _taskRepo;

    public GetDashboardQueryHandler(IPartnerPaymentHistoryRepo partnerPaymentHistoryRepo,
        IPaymentHistoryRepo paymentHistoryRepo, IServiceRepo serviceRepo, ICategoryRepo categoryRepo,
        IOrderRepo orderRepo, IFeedbackRepo feedbackRepo, IAccountRepo accountRepo, ITaskRepo taskRepo)
    {
        _partnerPaymentHistoryRepo = partnerPaymentHistoryRepo;
        _paymentHistoryRepo = paymentHistoryRepo;
        _serviceRepo = serviceRepo;
        _categoryRepo = categoryRepo;
        _orderRepo = orderRepo;
        _feedbackRepo = feedbackRepo;
        _accountRepo = accountRepo;
        _taskRepo = taskRepo;
    }

    public async Task<DashboardResponse> Handle(GetDashboardQuery request,
        CancellationToken cancellationToken)
    {
        var result = new DashboardResponse();
        var totalMonth = new List<Dictionary<string, double?>>();
        var totalYear = new List<Dictionary<string, double?>>();
        var serviceQuantity = new List<Dictionary<string, int>>();
        var serviceOrder = new List<Dictionary<string, int>>();
        var serviceFeedback = new List<Dictionary<string, float>>();
        var partner = new List<Dictionary<string, int>>();
        var staff = new List<Dictionary<string, int>>();
        var partnerPayment = new List<Dictionary<string, double?>>();

        var queryAccounts = await _accountRepo.GetAccounts(null, new Expression<Func<Data.Models.Account, object>>[]
        {
            a => a.User
        }).ToListAsync();
        var queryTasks = await _taskRepo.GetTasks().ToListAsync();
        var queryPaymentHistories = _paymentHistoryRepo.GetPaymentHistorys();
        var queryPartnerPaymentHistories = _partnerPaymentHistoryRepo.GetPartnerPaymentHistorys(null,
            new Expression<Func<PartnerPaymentHistory, object>>[]
            {
                pph => pph.Partner
            });
        var queryServices = _serviceRepo.GetServices(null, new Expression<Func<Data.Models.Service, object>>[]
        {
            s => s.CreateByNavigation
        });
        var queryOrders = _orderRepo.GetOrders(null, new Expression<Func<Data.Models.Order, object>>[]
        {
            o => o.OrderDetails
        });
        queryOrders = queryOrders.Include(o => o.OrderDetails).ThenInclude(s => s.Service);
        var queryFeedbacks = _feedbackRepo.GetFeedbacks(null, new Expression<Func<Data.Models.Feedback, object>>[]
        {
            f => f.OrderDetail
        });
        
        // get total payment history and total partner payment history in month
        var toDateMonth = DateTime.UtcNow;
        var fromDateMonth = new DateTime(toDateMonth.Year, 1, 1);
        var monthRange = Enumerable.Range(1, 12).ToList();

        // select total of payment history
        queryPaymentHistories = queryPaymentHistories.Where(x => x.CreateDate.Value.Date >= fromDateMonth &&
                                                                 x.CreateDate.Value.Date <= toDateMonth);

        // select total of partner payment history
        queryPartnerPaymentHistories = queryPartnerPaymentHistories.Where(x =>
            x.CreateDate.Value.Date >= fromDateMonth &&
            x.CreateDate.Value.Date <= toDateMonth);

        // join total payment history and total partner payment history to total with month in year
        var totalPaymentHistoryPerMonth = await queryPaymentHistories.GroupBy(x => x.CreateDate.Value.Month)
            .Select(x => new
            {
                Month = x.Key,
                Total = x.Sum(y => y.TotalAmount)
            }).ToListAsync(cancellationToken: cancellationToken);
        
        var totalOrderPerMonth = await queryOrders.GroupBy(x => x.CreateDate.Value.Month)
            .Select(x => new
            {
                Month = x.Key,
                Total = x.Count()
            }).ToListAsync(cancellationToken: cancellationToken);

        var totalPartnerPaymentHistoryPerMonth = await queryPartnerPaymentHistories
            .GroupBy(x => x.CreateDate.Value.Month)
            .Select(x => new
            {
                Month = x.Key,
                Total = x.Sum(y => y.Total)
            }).ToListAsync(cancellationToken: cancellationToken);

        var totalPerMonthDictionary = new Dictionary<string, double?>();
        double? totalAmountPerMonth = 0.0;
        // join month range and total payment history and total partner payment history map to totalPerMonthDictionary
        var joinMonthRange = (from month in monthRange
            join totalPaymentHistory in totalPaymentHistoryPerMonth on month equals totalPaymentHistory.Month into
                totalPaymentHistoryGroup
            from totalPaymentHistory in totalPaymentHistoryGroup.DefaultIfEmpty()
            join totalPartnerPaymentHistory in totalPartnerPaymentHistoryPerMonth on month equals
                totalPartnerPaymentHistory.Month into totalPartnerPaymentHistoryGroup
            from totalPartnerPaymentHistory in totalPartnerPaymentHistoryGroup.DefaultIfEmpty()
            select new
            {
                Month = month,
                Total = totalPaymentHistory?.Total,
                TotalPartner = totalPartnerPaymentHistory?.Total
            }).ToList();
        // map to totalPerMonthDictionary
        foreach (var item in joinMonthRange)
        {
            var month = item.Month;
            var totalAmount = item.Total ?? 0.0;
            var totalPartnerAmount = item.TotalPartner ?? 0.0;
            totalAmountPerMonth = totalAmount + totalPartnerAmount;
            totalPerMonthDictionary.Add(month.ToString(), totalAmountPerMonth);
        }

        totalMonth.Add(totalPerMonthDictionary);
        
        var joinMonthRangeOrder = (from month in monthRange
            join totalOrder in totalOrderPerMonth on month equals totalOrder.Month into
                totalOrderGroup
            from totalOrder in totalOrderGroup.DefaultIfEmpty()
            select new
            {
                Month = month,
                Total = totalOrder?.Total,
            }).ToList();
        
        // get total payment history and total partner payment history in year
        var toDateYear = DateTime.UtcNow;
        var fromDateYear = new DateTime(toDateYear.Year - 6, 1, 1);
        var yearRange = Enumerable.Range(fromDateYear.Year, toDateYear.Year - fromDateYear.Year + 1).ToList();
        // select total of payment history
        queryPaymentHistories = queryPaymentHistories.Where(x => x.CreateDate.Value.Date >= fromDateYear &&
                                                                 x.CreateDate.Value.Date <= toDateYear);

        // select total of partner payment history
        queryPartnerPaymentHistories = queryPartnerPaymentHistories.Where(x =>
            x.CreateDate.Value.Date >= fromDateYear &&
            x.CreateDate.Value.Date <= toDateYear);

        // join total payment history and total partner payment history to total with month in year
        var totalPaymentHistoryPerYear = await queryPaymentHistories.GroupBy(x => x.CreateDate.Value.Year)
            .Select(x => new
            {
                Year = x.Key,
                Total = x.Sum(y => y.TotalAmount)
            }).ToListAsync(cancellationToken: cancellationToken);

        var totalPartnerPaymentHistoryPerYear = await queryPartnerPaymentHistories
            .GroupBy(x => x.CreateDate.Value.Year)
            .Select(x => new
            {
                Year = x.Key,
                Total = x.Sum(y => y.Total)
            }).ToListAsync(cancellationToken: cancellationToken);

        var totalPerYearDictionary = new Dictionary<string, double?>();
        double? totalAmountPerYear = 0.0;
        // join month range and total payment history and total partner payment history map to totalPerMonthDictionary
        var joinYearRange = (from year in yearRange
            join totalPaymentHistory in totalPaymentHistoryPerYear on year equals totalPaymentHistory.Year into
                totalPaymentHistoryGroup
            from totalPaymentHistory in totalPaymentHistoryGroup.DefaultIfEmpty()
            join totalPartnerPaymentHistory in totalPartnerPaymentHistoryPerYear on year equals
                totalPartnerPaymentHistory.Year into totalPartnerPaymentHistoryGroup
            from totalPartnerPaymentHistory in totalPartnerPaymentHistoryGroup.DefaultIfEmpty()
            select new
            {
                Year = year,
                Total = totalPaymentHistory?.Total,
                TotalPartner = totalPartnerPaymentHistory?.Total
            }).ToList();
        // map to totalPerMonthDictionary
        foreach (var item in joinYearRange)
        {
            var month = item.Year;
            var totalAmount = item.Total ?? 0.0;
            var totalPartnerAmount = item.TotalPartner ?? 0.0;
            totalAmountPerYear = totalAmount + totalPartnerAmount;
            totalPerYearDictionary.Add(month.ToString(), totalAmountPerYear);
        }

        totalYear.Add(totalPerYearDictionary);
        

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
        var orderGroupByServiceId = orders.SelectMany(x => x.OrderDetails).GroupBy(x => x.Service.CategoryId).ToList();

        foreach (var item in orderGroupByServiceId)
        {
            var serviceId = item.Key;
            var serviceName = categoryNames.FirstOrDefault(x => x.Id == serviceId)?.Name;
            if (serviceName != null)
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add(serviceName, item.Count());
                serviceOrder.Add(dictionary);
            }
        }
        
        result.TotalOrderDone = orders.Count(x => x.StatusOrder == (int)StatusOrder.DONE);

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
        result.TotalFeedback = feedbacks.Count();
        result.NegativeFeedback = feedbacks.Count(x => x.Rating < 3);
        
        

        // count partner with category in service
        var partnerGroupByCategoryId = queryAccounts
            .Where(x => x.RoleName == RoleName.PARTNER)
            .GroupBy(x => x.User.CategoryId).ToList();
        foreach (var item in partnerGroupByCategoryId)
        {
            var categoryId = item.Key;
            var categoryName = categoryNames.FirstOrDefault(x => x.Id == categoryId)?.Name;
            if (categoryName != null)
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add(categoryName, item.Select(x => x.Id).Distinct().Count());
                partner.Add(dictionary);
            }
        }
        
        var staffGroupByCategoryId = queryAccounts
            .Where(x => x.RoleName == RoleName.STAFF)
            .GroupBy(x => x.User.CategoryId).ToList();
        foreach (var item in staffGroupByCategoryId)
        {
            var categoryId = item.Key;
            var categoryName = categoryNames.FirstOrDefault(x => x.Id == categoryId)?.Name;
            if (categoryName != null)
            {
                var dictionary = new Dictionary<string, int>();
                dictionary.Add(categoryName, item.Select(x => x.Id).Distinct().Count());
                staff.Add(dictionary);
            }
        }
        result.Staff = staff;

        // count total payment of partner join user select name partner
        var partnerPaymentHistories =
            await queryPartnerPaymentHistories.ToListAsync(cancellationToken: cancellationToken);
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


        result.TotalRevenueMonth = totalMonth;
        result.TotalRevenueYear = totalYear;
        result.ServiceQuantity = serviceQuantity;
        result.ServiceOrder = serviceOrder;
        result.ServiceFeedback = serviceFeedback;
        result.Partner = partner;
        result.PartnerPayment = partnerPayment;
        result.TotalOrder = orders.Count();
        result.TotalPartner = queryAccounts.Count(x => x.RoleName == RoleName.PARTNER);
        result.TotalStaff = queryAccounts.Count(x => x.RoleName == RoleName.STAFF);
        result.TaskInProgress = queryTasks.Count(x => x.Status == (int)TaskStatus.IN_PROGRESS);
        result.TaskToDo = queryTasks.Count(x => x.Status == (int)TaskStatus.TO_DO);
        result.TotalTask = queryTasks.Count();
        result.TaskDone = queryTasks.Count(x => x.Status == (int)TaskStatus.DONE);

        return result;
    }
}
using Quartz;
using WSS.API.Data.Repositories.Account;
using WSS.API.Data.Repositories.WeddingInformation;
using WSS.API.Infrastructure.Services.Mail;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Cron;

public class StartSendMailCron : IJob
{
    private readonly IWeddingInformationRepo _weddingInformationRepo;
    private readonly IAccountRepo _accountRepo;
    private readonly IMailService _mailService;

    public StartSendMailCron(IWeddingInformationRepo weddingInformationRepo, IAccountRepo accountRepo, IMailService mailService)
    {
        _weddingInformationRepo = weddingInformationRepo;
        _accountRepo = accountRepo;
        _mailService = mailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        // get all order by wedding information by wedding day
        var today = DateTime.Now.AddDays(-1).Date;
        var wds = _weddingInformationRepo.GetWeddingInformations(null,
            new Expression<Func<Data.Models.WeddingInformation, object>>[]
            {
                w => w.Orders,
                w => w.Orders.Select(o => o.CreateBy)
            }).Where(x => x.WeddingDay.Value.Date  == today).ToList();
        
        foreach (var item in wds)
        {
            var order = item.Orders.FirstOrDefault();
            var userId = order.CreateBy;
            var user = await _accountRepo.GetAccountById((Guid)userId);
            if (user != null)
            {
                // send mail
                var mail = new MailInputType()
                {
                    ToEmail = user.Username,
                    Subject = "Thông báo thanh toán.",
                    Body = @$"<html> <body> <p> +
                                Gửi ông/bà:" + user.Username +
                           "Chúng tôi vui mừng thông báo cho bạn biết rằng chúng tôi đã hoàn thành đơn hàng của bạn." +
                           "\nCảm ơn ông rất nhiều khi đã tin tưởng và sử dụng sản phẩm của cửa hàng Blissful Bell." +
                           "Hy vong chúng tôi đã đem đến cho bạn trải nghiệm tốt nhất." +
                           "\nVui lòng Bạn tiến hành thanh toán  phần còn lại của đơn hàng trong vòng 24H." +
                           " \nNếu bạn có bất kỳ câu hỏi nào, hãy liên hệ với chúng tôi tại đây hoặc gọi cho chúng tôi theo số 098.888.888" +
                           $"\n\nVui lòng thanh toán tại đây https://loveweddingservice.shop/order-history/+{order.Id} hoặc liên hệ với chúng tôi." +
                           " </p> </body> </html>"
                };
                await _mailService.SendEmailAsync(mail);
            }
        }
    }
}
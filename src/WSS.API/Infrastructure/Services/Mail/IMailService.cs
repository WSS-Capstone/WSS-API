using Task = System.Threading.Tasks.Task;

namespace WSS.API.Infrastructure.Services.Mail
{
    public interface IMailService
    {
        public Task SendEmailAsync(MailInputType mailInput);
    }
}
using MailKit.Security;
using MimeKit;
using TglSol.Tms.Hrm.Business.Services.Mail;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using Task = System.Threading.Tasks.Task;

namespace WSS.API.Infrastructure.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(MailInputType mailInput)
        {
            var _mailSetting = new MailSetting()
            {
                Mail = _configuration["MailSettings:Mail"],
                DisplayName = _configuration["MailSettings:DisplayName"],
                Password = _configuration["MailSettings:Password"],
                Host = _configuration["MailSettings:Host"],
                Port = Convert.ToInt32(_configuration["MailSettings:Port"])
            };
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            email.To.Add(MailboxAddress.Parse(mailInput.ToEmail));
            email.Subject = mailInput.Subject;
            var builder = new BodyBuilder();
            if (mailInput.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailInput.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }

                        builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }
            }

            builder.HtmlBody = mailInput.Body;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
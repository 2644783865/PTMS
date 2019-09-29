using Microsoft.Extensions.Options;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PTMS.Infrastructure
{
    public class EmailService : IEmailService
    {
        private readonly string HtmlLayout = @"
<html>
<body>
    {0}
<br/>
<br/>
С уважением, <br/>
Команда ЦОДД Администратор
</body>
</html>
";
        private readonly string PlainTextLayout = @"
    {0}
\n \n
С уважением, \n
Команда ЦОДД Администратор
";

        private readonly AppSettings _appSettings;

        public EmailService(
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task SendEmailAsync(
            string recipient, 
            string subject,  
            string htmlBody, 
            params Tuple<string, byte[]>[] attachments)
        {
            // Configure Smtp client
            var smtpClient = new SmtpClient
            {
                Port = Convert.ToInt32(_appSettings.SmtpServerPort),
                Host = _appSettings.SmtpServerName,
                EnableSsl = Convert.ToBoolean(_appSettings.SmtpUseSsl),
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            smtpClient.Credentials = new NetworkCredential(
                        _appSettings.SmtpServerLogin,
                        _appSettings.SmtpServerPassword);

            var sender = _appSettings.EmailSender;
            var from = new MailAddress(sender, "ЦОДД Администратор");
            var to = new MailAddress(recipient);

            var plainText = Regex.Replace(htmlBody, "<.*?>", string.Empty);

            var plainView = AlternateView.CreateAlternateViewFromString(string.Format(PlainTextLayout, plainText), null, "text/plain");
            var htmlView = AlternateView.CreateAlternateViewFromString(string.Format(HtmlLayout, htmlBody), null, "text/html");

            // Construct Mail Message
            var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
            };

            mailMessage.AlternateViews.Add(plainView);
            mailMessage.AlternateViews.Add(htmlView);

            //Add files into attachments
            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    var a = new Attachment(new MemoryStream(attachment.Item2), attachment.Item1);
                    mailMessage.Attachments.Add(a);
                }
            }

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

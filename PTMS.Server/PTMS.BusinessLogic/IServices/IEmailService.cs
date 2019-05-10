using System;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string recipient, 
            string subject, 
            string htmlBody, 
            params Tuple<string, byte[]>[] attachments);
    }
}

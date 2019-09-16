using System;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IPdfService
    {
        byte[] ConvertHtmlToPdf(string html);
    }
}

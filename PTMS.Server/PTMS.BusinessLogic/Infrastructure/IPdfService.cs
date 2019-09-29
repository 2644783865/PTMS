namespace PTMS.BusinessLogic.Infrastructure
{
    public interface IPdfService
    {
        byte[] ConvertHtmlToPdf(string html);
    }
}

﻿using Balbarak.WeasyPrint;
using PTMS.BusinessLogic.Infrastructure;

namespace PTMS.Infrastructure
{
    public class PdfService : IPdfService
    {
        public byte[] ConvertHtmlToPdf(string html)
        {
            using (var client = new WeasyPrintClient())
            {
                var binaryPdf = client.GeneratePdf(html);
                return binaryPdf;
            }
        }
    }
}

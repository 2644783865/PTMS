using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.Models.Shared;
using PTMS.Common.Enums;
using System;
using System.Text;
using System.Web;

namespace PTMS.Api.Controllers
{
    [PtmsAuthorize]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult CreateFileResponse(FileModel file)
        {
            if (file.FileFormat == FileFormatEnum.Pdf)
            {
                return CreatePdfResult(file.Bytes, file.Name);
            }
            else if (file.FileFormat == FileFormatEnum.Xlsx)
            {
                return CreateXlsxResult(file.Bytes, file.Name);
            }

            throw new NotImplementedException();
        }

        private IActionResult CreatePdfResult(byte[] pdfDoc, string fName)
        {
            var fileName = fName + ".pdf";
            var contentDisposition = string.Format("inline; filename=\"{0}\"; filename*=UTF-8''{0}",
                HttpUtility.UrlEncode(fileName, Encoding.UTF8));

            Response.Headers.Add("Content-Disposition", contentDisposition);
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(pdfDoc, "application/pdf");
        }

        private IActionResult CreateXlsxResult(byte[] bytes, string fName)
        {
            var fileName = fName + ".xlsx";

            return File(
                bytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName);
        }
    }
}

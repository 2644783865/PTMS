using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.Common;
using System;
using System.Text;
using System.Web;

namespace PTMS.Api.Controllers
{
    [PtmsAuthorize]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult CreatePdfResult(byte[] pdfDoc, string fName, bool inline = true)
        {
            var fileName = fName + ".pdf";
            var contentDisposition = string.Format(string.Format("inline; filename=\"{0}\"; filename*=UTF-8''{0}", HttpUtility.UrlEncode(fileName, Encoding.UTF8)));

            Response.Headers.Add("Content-Disposition", contentDisposition);
            Response.Headers.Add("X-Content-Type-Options", "nosniff");

            return File(pdfDoc, "application/pdf");
        }
    }
}

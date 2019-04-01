using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;

namespace PTMS.Api.Controllers
{
    [PtmsAuthorize]
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.Api.Controllers
{
    public class ProviderController : ApiControllerBase
    {
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService)
        {
            _providerService = providerService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/providers")]
        public async Task<ActionResult<List<ProviderModel>>> GetAll()
        {
            var result = await _providerService.GetAllAsync();
            return result;
        }
    }
}

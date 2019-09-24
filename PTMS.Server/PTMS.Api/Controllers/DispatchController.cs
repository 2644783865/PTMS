using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.Dispatch;
using PTMS.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class DispatchController : ApiControllerBase
    {
        private readonly IDispatchService _dispatchService;

        public DispatchController(IDispatchService dispatchService)
        {
            _dispatchService = dispatchService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/trolleybusTodayStatus")]
        public async Task<ActionResult<List<TrolleybusTodayStatusModel>>> GetTrolleybusTodayStatus()
        {
            var result = await _dispatchService.GetTrolleybusTodayStatus();
            return result;
        }
    }
}

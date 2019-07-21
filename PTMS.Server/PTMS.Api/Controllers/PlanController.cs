using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.Plan;
using PTMS.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class PlanController : ApiControllerBase
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/plans/byroute")]
        public async Task<ActionResult<List<PlanByRouteModel>>> GetPlansByRoutes(DateTime date)
        {
            var result = await _planService.GetPlannedNumberByRouteAsync(date);
            return result;
        }
    }
}

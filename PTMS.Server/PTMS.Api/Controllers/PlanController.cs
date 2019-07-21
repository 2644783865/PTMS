using Microsoft.AspNetCore.Mvc;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models.Plan;
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

        [HttpGet("/plans/byroute")]
        public async Task<ActionResult<List<PlanByRouteModel>>> GetPlansByRoutes(DateTime date)
        {
            var result = await _planService.GetPlannedNumberByRouteAsync(date);
            return result;
        }
    }
}

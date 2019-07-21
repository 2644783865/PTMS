using PTMS.BusinessLogic.Models.Plan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.IServices
{
    public interface IPlanService
    {
        Task<List<PlanByRouteModel>> GetPlannedNumberByRouteAsync(DateTime date);
    }
}

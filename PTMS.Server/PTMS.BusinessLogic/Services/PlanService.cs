using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.BusinessLogic.Models.Plan;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class PlanService : BusinessServiceAsync<Plans, CarTypeModel>, IPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly IProjectRouteRepository _projectRouteRepository;

        public PlanService(
            IPlanRepository planRepository,
            IProjectRouteRepository projectRouteRepository,
            IMapper mapper)
            : base(mapper)
        {
            _planRepository = planRepository;
            _projectRouteRepository = projectRouteRepository;
        }

        public async Task<List<PlanByRouteModel>> GetPlannedNumberByRouteAsync(DateTime date)
        {
            var plans = await _planRepository.GetByDateAsync(date);
            var projectRoutes = await _projectRouteRepository.GetAllActiveAsync();

            var result = projectRoutes
                .Select(x => x.RoutId)
                .Distinct()
                .Select(routeId => {
                    var plan = plans.FirstOrDefault(pl => pl.RoutId == routeId);
                    var projectRoute = projectRoutes.FirstOrDefault(pr => pr.RoutId == routeId);

                    var item = new PlanByRouteModel
                    {
                        RouteId = routeId,
                        ProjectId = projectRoute != null ? projectRoute.ProjId : 0,
                        PlannedNumber = plan != null ? plan.TypePlannedTotal : 0
                    };

                    return item;
                })
                .ToList();

            return result;
        }
    }
}

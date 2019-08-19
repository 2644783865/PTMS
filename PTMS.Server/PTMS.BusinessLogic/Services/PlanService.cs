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
    public class PlanService : BusinessServiceAsync<Plans>, IPlanService
    {
        private readonly IPlanRepository _planRepository;
        private readonly IProjectRouteRepository _projectRouteRepository;
        private readonly IFactOfObjectRouteRepository _factOfObjectRouteRepository;

        public PlanService(
            IPlanRepository planRepository,
            IProjectRouteRepository projectRouteRepository,
            IFactOfObjectRouteRepository factOfObjectRouteRepository,
            IMapper mapper)
            : base(mapper)
        {
            _planRepository = planRepository;
            _projectRouteRepository = projectRouteRepository;
            _factOfObjectRouteRepository = factOfObjectRouteRepository;
        }

        public async Task<List<PlanByRouteModel>> GetPlannedNumberByRouteAsync(DateTime date)
        {
            var plans = await _planRepository.GetByDateAsync(date);
            var factObjects = await _factOfObjectRouteRepository.GetByDateAsync(date);
            var projectRoutes = await _projectRouteRepository.GetAllActiveAsync();

            var result = projectRoutes
                .Select(x => x.RouteId)
                .Distinct()
                .Select(routeId => {
                    var plan = plans.FirstOrDefault(pl => pl.RoutId == routeId);
                    var fact = factObjects.FirstOrDefault(f => f.RouteId == routeId);
                    var projectRoute = projectRoutes.FirstOrDefault(pr => pr.RouteId == routeId);

                    var item = new PlanByRouteModel
                    {
                        RouteId = routeId,
                        ProjectId = projectRoute != null ? projectRoute.ProjectId : 0,
                        FactNumber = fact != null ? fact.CountObjects : 0,
                        PlannedNumber = plan != null ? plan.TypePlannedTotal : 0
                    };

                    return item;
                })
                .ToList();

            return result;
        }
    }
}

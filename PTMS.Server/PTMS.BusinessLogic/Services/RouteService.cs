using AutoMapper;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class RouteService : BusinessServiceAsync<Route>, IRouteService
    {
        private readonly AppUserManager _userManager;
        private readonly IRouteRepository _routeRepository;
        private readonly IProjectRouteRepository _projectRouteRepository;

        public RouteService(
            AppUserManager userManager,
            IRouteRepository routeRepository,
            IProjectRouteRepository projectRouteRepository,
            IMapper mapper)
            : base(mapper)
        {
            _routeRepository = routeRepository;
            _userManager = userManager;
            _projectRouteRepository = projectRouteRepository;
        }

        public async Task<List<RouteModel>> GetAllAsync(
            ClaimsPrincipal userPrincipal, 
            int? projectId,
            bool? active)
        {
            var userRoutesModel = await _userManager.GetAvailableRoutesModel(userPrincipal);
            var result = await _routeRepository.GetAllAsync(userRoutesModel, projectId, active);
            return MapToModel<RouteModel>(result);
        }

        public async Task<List<RouteFullModel>> GetAllForPageAsync()
        {
            var result = await _routeRepository.GetAllForPageAsync();
            return result.Select(MapToFullModel).ToList();
        }

        public async Task<RouteModel> GetByIdAsync(int id)
        {
            var result = await _routeRepository.GetByIdAsync(id);
            return MapToModel<RouteModel>(result);
        }

        public async Task<RouteFullModel> GetForEditByIdAsync(int id)
        {
            var result = await _routeRepository.GetForEditByIdAsync(id);
            return MapToFullModel(result);
        }

        public async Task<RouteModel> AddAsync(RouteFullModel model)
        {
            var entity = MapFromModel(model);
            var result = await _routeRepository.AddAsync(entity);

            await SetProjectRoute(result.Id, model.ProjectId, null);
            return MapToModel<RouteModel>(result);
        }

        public async Task<RouteModel> UpdateAsync(RouteFullModel model)
        {
            var entity = MapFromModel(model);
            var result = await _routeRepository.UpdateAsync(entity);

            var projectRoute = await _projectRouteRepository.GetByRouteIdAsync(result.Id);
            await SetProjectRoute(result.Id, model.ProjectId, projectRoute);

            return MapToModel<RouteModel>(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _routeRepository.DeleteByIdAsync(id);
        }

        private RouteFullModel MapToFullModel(Route route)
        {
            var result = MapToModel<RouteFullModel>(route);

            if (route.ProjectRoutes != null)
            {
                var projRoute = route.ProjectRoutes.FirstOrDefault();

                if (projRoute != null)
                {
                    result.ProjectId = projRoute.ProjectId;
                }
            }

            return result;
        }

        private async Task SetProjectRoute(int routeId, int? projectId, ProjectRoute existingProjectRoute)
        {
            if (existingProjectRoute != null)
            {
                if (projectId.HasValue)
                {
                    if (existingProjectRoute.ProjectId != projectId)
                    {
                        existingProjectRoute.ProjectId = projectId.Value;
                        await _projectRouteRepository.UpdateAsync(existingProjectRoute);
                    }
                }
                else
                {
                    await _projectRouteRepository.DeleteByIdAsync(existingProjectRoute.Id);
                }
            }
            else if (projectId.HasValue)
            {
                var projectRoute = new ProjectRoute
                {
                    RouteId = routeId,
                    ProjectId = projectId.Value
                };

                await _projectRouteRepository.AddAsync(projectRoute);
            }
        }
    }
}

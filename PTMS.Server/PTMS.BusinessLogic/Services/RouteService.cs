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

        public RouteService(
            AppUserManager userManager,
            IRouteRepository routeRepository,
            IMapper mapper)
            : base(mapper)
        {
            _routeRepository = routeRepository;
            _userManager = userManager;
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
            return MapToModel<RouteModel>(result);
        }

        public async Task<RouteModel> UpdateAsync(RouteFullModel model)
        {
            var entity = MapFromModel(model);
            var result = await _routeRepository.UpdateAsync(entity);
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
                    result.ProjectId = projRoute.ProjId;
                }
            }

            return result;
        }
    }
}

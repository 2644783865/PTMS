using AutoMapper;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
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
        private readonly AppCacheHelper _appCacheHelper;

        public RouteService(
            AppUserManager userManager,
            IRouteRepository routeRepository,
            IProjectRouteRepository projectRouteRepository,
            AppCacheHelper appCacheHelper,
            IMapper mapper)
            : base(mapper)
        {
            _routeRepository = routeRepository;
            _userManager = userManager;
            _projectRouteRepository = projectRouteRepository;
            _appCacheHelper = appCacheHelper;
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

        public async Task<List<RouteWithStationsModel>> GetAllActiveRoutesWithStationsAsync()
        {
            var result = await _appCacheHelper.GetCachedAsync(
               "GetAllActiveRoutesWithStations",
               async () => {
                   var routes = await _routeRepository.GetActiveWithStationsAsync();

                   var routesWithStations = new List<RouteWithStationsModel>();

                   foreach (var route in routes)
                   {
                       var item = new RouteWithStationsModel
                       {
                           Id = route.Id,
                           Name = route.Name,
                           ForwardDirectionStations = new List<BusStationForRouteModel>(),
                           BackDirectionStations = new List<BusStationForRouteModel>()
                       };

                       var allStations = route.BusStationRoutes
                            .Select(x =>
                            {
                                var station = _mapper.Map<BusStationForRouteModel>(x.BusStation);
                                station.IsEndingStation = x.IsEndingStation;
                                station.Num = x.Num;
                                return station;
                            })
                            .DistinctBy(x => x.Id)
                            .OrderBy(x => x.Num)
                            .ToList();

                       var firstEndStationIndex = allStations.FindIndex(x => x.IsEndingStation);
                       var secondEndStationIndex = allStations.FindIndex(firstEndStationIndex + 1, x => x.IsEndingStation);

                       if (secondEndStationIndex != -1)
                       {
                           item.ForwardDirectionStations = allStations.GetRange(firstEndStationIndex, secondEndStationIndex - firstEndStationIndex);
                           item.BackDirectionStations = allStations.Where(x => !item.ForwardDirectionStations.Contains(x)).ToList();
                       }
                       else
                       {
                           item.ForwardDirectionStations = allStations;
                       }

                       routesWithStations.Add(item);
                   }

                   return routesWithStations;
               },
               typeof(Route), typeof(BusStation), typeof(BusStationRoute));

            return result;
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
            entity.Name = entity.Name.PrepareRouteName();
            var result = await _routeRepository.AddAsync(entity);

            await SetProjectRoute(result.Id, model.ProjectId, null);
            return MapToModel<RouteModel>(result);
        }

        public async Task<RouteModel> UpdateAsync(RouteFullModel model)
        {
            var entity = MapFromModel(model);
            entity.Name = entity.Name.PrepareRouteName();
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

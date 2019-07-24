using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Helpers;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class RouteService : BusinessServiceAsync<Routs, RouteModel>, IRouteService
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
            return MapToModel(result);
        }

        public async Task<RouteModel> GetByIdAsync(int id)
        {
            var result = await _routeRepository.GetByIdAsync(id);
            return MapToModel(result);
        }

        public async Task<RouteModel> AddAsync(RouteModel model)
        {
            var entity = MapFromModel(model);
            var result = await _routeRepository.AddAsync(entity);
            return MapToModel(result);
        }

        public async Task<RouteModel> UpdateAsync(RouteModel model)
        {
            var entity = MapFromModel(model);
            var result = await _routeRepository.UpdateAsync(entity);
            return MapToModel(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _routeRepository.DeleteByIdAsync(id);
        }
    }
}

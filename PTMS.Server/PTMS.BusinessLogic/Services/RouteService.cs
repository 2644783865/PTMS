using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;

namespace PTMS.BusinessLogic.Services
{
    public class RouteService : BusinessServiceAsync<Route, RouteModel>, IRouteService
    {
        private readonly IRouteRepository _routeRepository;

        public RouteService(
            IRouteRepository routeRepository,
            IMapper mapper)
            : base(mapper)
        {
            _routeRepository = routeRepository;
        }

        public async Task<List<RouteModel>> GetAllAsync()
        {
            var result = await _routeRepository.GetAllAsync();
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
            var result = await _routeRepository.AddAsync(entity, true);
            return MapToModel(result);
        }

        public async Task<RouteModel> UpdateAsync(RouteModel model)
        {
            var entity = MapFromModel(model);
            var result = await _routeRepository.UpdateAsync(entity, true);
            return MapToModel(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _routeRepository.DeleteByIdAsync(id, true);
        }
    }
}

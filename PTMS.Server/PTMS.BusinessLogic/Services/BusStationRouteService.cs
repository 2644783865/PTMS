using AutoMapper;
using PTMS.BusinessLogic.Infrastructure;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.DataServices.IRepositories;
using PTMS.Domain.Entities;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Services
{
    public class BusStationRouteService : BusinessServiceAsync<BusStationRoute>, IBusStationRouteService
    {
        private readonly IBusStationRouteRepository _busStationRouteRepository;

        public BusStationRouteService(
            IBusStationRouteRepository busStationRouteRepository,
            IMapper mapper)
            : base(mapper)
        {
            _busStationRouteRepository = busStationRouteRepository;
        }

        public async Task<BusStationRouteModel> AddAsync(BusStationRouteModel model)
        {
            var entity = MapFromModel(model);
            var result = await _busStationRouteRepository.AddAsync(entity);
            return MapToModel<BusStationRouteModel>(result);
        }

        public async Task<BusStationRouteModel> UpdateAsync(int id, BusStationRouteModel model)
        {
            var entity = MapFromModel(model);
            var result = await _busStationRouteRepository.UpdateAsync(entity);
            return MapToModel<BusStationRouteModel>(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _busStationRouteRepository.DeleteByIdAsync(id);
        }
    }
}

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
    public class BusStationService : BusinessServiceAsync<BusStation>, IBusStationService
    {
        private readonly IBusStationRepository _busStationRepository;

        public BusStationService(
            IBusStationRepository busStationRepository,
            IMapper mapper)
            : base(mapper)
        {
            _busStationRepository = busStationRepository;
        }

        public async Task<List<BusStationModel>> GetAllAsync()
        {
            var result = await _busStationRepository.GetAllAsync();
            return MapToModel<BusStationModel>(result);
        }
    }
}

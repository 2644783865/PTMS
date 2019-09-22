using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PTMS.BusinessLogic.Helpers;
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
        private readonly AppCacheHelper _appCacheHelper;

        public BusStationService(
            IBusStationRepository busStationRepository,
            AppCacheHelper appCacheHelper,
            IMapper mapper)
            : base(mapper)
        {
            _busStationRepository = busStationRepository;
            _appCacheHelper = appCacheHelper;
        }

        public async Task<List<BusStationModel>> GetAllAsync()
        {
            var result = await _appCacheHelper.GetCachedAsync(
                "GetAllBusStations",
                () => _busStationRepository.GetAllAsync(),
                typeof(BusStation));

            return MapToModel<BusStationModel>(result);
        }

        public async Task<BusStationModel> AddAsync(BusStationModel model)
        {
            var entity = MapFromModel(model);
            var result = await _busStationRepository.AddAsync(entity);
            return MapToModel<BusStationModel>(result);
        }

        public async Task<BusStationModel> UpdateAsync(BusStationModel model)
        {
            var entity = MapFromModel(model);
            var result = await _busStationRepository.UpdateAsync(entity);
            return MapToModel<BusStationModel>(result);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _busStationRepository.DeleteByIdAsync(id);
        }
    }
}

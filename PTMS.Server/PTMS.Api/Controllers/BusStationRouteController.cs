using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;

namespace PTMS.Api.Controllers
{
    public class BusStationRouteController : ApiControllerBase
    {
        private readonly IBusStationRouteService _busStationRouteService;

        public BusStationRouteController(IBusStationRouteService busStationRouteService)
        {
            _busStationRouteService = busStationRouteService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/busStationRoute")]
        public async Task<BusStationRouteModel> Post([FromBody]BusStationRouteModel model)
        {
            var result = await _busStationRouteService.AddAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPut("/busStationRoute/{id}")]
        public async Task<BusStationRouteModel> Put(int id, [FromBody]BusStationRouteModel model)
        {
            var result = await _busStationRouteService.UpdateAsync(id, model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpDelete("/busStationRoute/{id}")]
        public async Task Delete(int id)
        {
            await _busStationRouteService.DeleteByIdAsync(id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PTMS.Api.Attributes;
using PTMS.BusinessLogic.IServices;
using PTMS.BusinessLogic.Models;
using PTMS.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PTMS.Api.Controllers
{
    public class BusStationController : ApiControllerBase
    {
        private readonly IBusStationService _busStationService;

        public BusStationController(IBusStationService busStationService)
        {
            _busStationService = busStationService;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpGet("/busStations")]
        public async Task<ActionResult<List<BusStationModel>>> GetAll()
        {
            var result = await _busStationService.GetAllAsync();
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPost("/busStation")]
        public async Task<BusStationModel> Post([FromBody]BusStationModel model)
        {
            var result = await _busStationService.AddAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpPut("/busStation/{id}")]
        public async Task<BusStationModel> Put(int id, [FromBody]BusStationModel model)
        {
            var result = await _busStationService.UpdateAsync(model);
            return result;
        }

        [PtmsAuthorize(RoleNames.Dispatcher)]
        [HttpDelete("/busStation/{id}")]
        public async Task Delete(int id)
        {
            await _busStationService.DeleteByIdAsync(id);
        }
    }
}
